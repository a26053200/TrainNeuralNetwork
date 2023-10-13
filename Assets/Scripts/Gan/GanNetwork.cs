//
// ClassName：GanNetwork
// Author：zhengnan
// DateTime：2023年09月28日 星期四 11:15
//		

using System;
using System.Collections.Generic;
using System.Text;
using NeuralNetwork;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using XCharts.Runtime;

namespace Gan
{
    /// <summary>
    /// 
    /// </summary>
    public class GanNetwork : MonoBehaviour
    {
        public LineChart _lineChartG;
        public LineChart _lineChartD;
        public Button btnTrain;
        public Slider sliderEpochs;
        public Slider sliderLearnRate;

        // private readonly List<NNDataSet> _dataSets = new List<NNDataSet>();
        private float _startTime;
        private Discriminator _discriminator;
        private Generator _generator;
        private float _learningRate = 0.001f;
        private int _epochs = 1000; //train times

        private void Awake()
        {
            btnTrain.onClick.AddListener(StartTest);
            _lineChartG.ClearData();
            _lineChartD.ClearData();
            _lineChartG.RemoveData();
            _lineChartD.RemoveData();
            _lineChartG.AddSerie<Line>("G-Error");
            _lineChartD.AddSerie<Line>("D-Error");

            var yAxis = _lineChartG.EnsureChartComponent<YAxis>();
            yAxis.splitNumber = 10;
            yAxis.max = 1;
            yAxis.min = 0;
            yAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            yAxis.type = Axis.AxisType.Value;

            yAxis = _lineChartD.EnsureChartComponent<YAxis>();
            yAxis.splitNumber = 10;
            yAxis.max = 5;
            yAxis.min = 0;
            yAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            yAxis.type = Axis.AxisType.Value;

            // for (int i = 0; i < 100; i++)
            // {
            //     Debug.Log(MathUtils.Rand());
            // }
        }

        private void StartTest()
        {
            _epochs = (int) sliderEpochs.value;
            _learningRate = sliderLearnRate.value;

            _lineChartG.ClearData();
            _lineChartD.ClearData();
            List<double[]> targetList = new List<double[]>
            {
                new double[] {1, 0, 0, 1},
                new double[] {0.9, 0.1, 0.2, 0.8},
                new double[] {0.9, 0.2, 0.1, 0.8},
                new double[] {0.8, 0.1, 0.2, 0.9},
                new double[] {0.8, 0.2, 0.1, 0.9}
            };

            // foreach (var targets in targetList)
            // _dataSets.Add(new NNDataSet(new[] {MathUtils.GetRandomRange(), MathUtils.GetRandomRange(), MathUtils.GetRandomRange(), MathUtils.GetRandomRange()}, targets));

            //对抗网络 判别器
            //4个输入元,每个元 1个突触 到 同 1个输出
            _discriminator = new Discriminator();
            _discriminator.LearningRate = _learningRate;
            _discriminator.Momentum = 0.001;

            //对抗网络 生成器
            //1个输入元,每个元 4个突触 到 同 4个输出
            _generator = new Generator();
            _generator.LearningRate = _learningRate;
            _generator.Momentum = 0.001;

            int index = 0;
            double gMax = Double.MinValue;
            double dMax = Double.MinValue;
            for (int i = 0; i < _epochs; i++)
            {
                foreach (var face in targetList)
                {
                    //从真实人脸更新判别器权重
                    _discriminator.UpdateWeightsFromTargets(face);
                    //选择一个随机数生成一张假脸 随机噪声(正态分布的噪声)
                    var random = MathUtils.GetRandomRange();
                    var z = new[] {random, random, random, random};
                    //计算判别器误差
                    var dError = _discriminator.CalculateErrorFromTargets(face)[0] + _discriminator.CalculateErrorFromNoises(z)[0];
                    //计算生成器误差
                    var gError = _generator.Error(z, _discriminator);

                    //生成器 根据噪声 生成人脸
                    var noiseFace = _generator.Forward(z);
                    //从假脸更新判别器权重
                    _discriminator.UpdateWeightsFromNoises(noiseFace);
                    //从假脸更新生成器权重
                    _generator.Update(noiseFace, _discriminator);

                    _lineChartG.AddXAxisData("" + index++);
                    _lineChartD.AddXAxisData("" + index++);

                    if (gMax < gError)
                        gMax = gError;
                    _lineChartG.AddData(0, gError);

                    if (double.IsPositiveInfinity(dError))
                        _lineChartD.AddData(0, 4.8);
                    else if (dError < 0.000001)
                        _lineChartD.AddData(0, 0);
                    else
                    {
                        if (dMax < dError)
                            dMax = dError;
                        _lineChartD.AddData(0, dError);
                    }
                }
            }

            var yAxis1 = _lineChartG.EnsureChartComponent<YAxis>();
            yAxis1.max = Mathf.CeilToInt((float) gMax);
            var yAxis2 = _lineChartD.EnsureChartComponent<YAxis>();
            yAxis2.max = Mathf.CeilToInt((float) dMax);

            Debug.Log("G weights:\t" + DebugUtils.DoubleArray2String(_generator.GetInputWeights(0)));
            Debug.Log("G biases:\t" + DebugUtils.DoubleArray2String(_generator.GetOutputBiases()));
            Debug.Log("D weights:\t" + DebugUtils.DoubleArray2String(_discriminator.GetOutputWeights(0)));
            Debug.Log("D biases:\t" + DebugUtils.DoubleArray2String(_discriminator.GetOutputBiases()));
            //测试生成器 判别器
            // for (int i = 0; i < 10; i++)
            // {
            //     var z = new[] {MathUtils.GetRandomRange()};
            //     var face = _generator.Forward(z);
            //     var results = _discriminator.Forward(face);
            //     Debug.Log(DebugUtils.DoubleArray2String(face) + " -> " + DebugUtils.DoubleArray2String(results));
            // }
            
            //测试判别器
            foreach (var target in targetList)
            {
                var results = _discriminator.Forward(target);
                Debug.Log(DebugUtils.DoubleArray2String(target) + " -> " + DebugUtils.DoubleArray2String(results));
            }
        }

        private void Update1()
        {
            if (Time.time - _startTime < 1)
                return;
            _startTime = Time.time;

            //生成器生成一组数据
            var inputs = new[] {MathUtils.GetRandomRange()};
            // var inputs = new[] {0.7};
            // _debugData1(inputs);
            var results1 = _generator.Forward(inputs);
            var results2 = _discriminator.Forward(results1);
            if (results2.Length > 0 && results2[0] >= 0.5)
            {
                DebugUtils._debugData2(results1);
                DebugUtils._debugData3(results2);
            }
            else
            {
                //判别失败
                DebugUtils._debugError(results1);
                DebugUtils._debugError(results2);
            }
        }
    }
}
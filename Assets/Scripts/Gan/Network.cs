//
// ClassName：Network
// Author：zhengnan
// DateTime：2023年09月26日 星期二 17:11
//		

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gan
{
    public class Network
    {
        public double LearningRate { get; set; } // 学习速率
        public double Momentum { get; set; } //动量
        public List<Neuron> InputLayer { get; set; } //输入层
        public List<List<Neuron>> HiddenLayers { get; set; } // 隐藏层
        public List<Neuron> OutputLayer { get; set; } // 输出层
        public List<Neuron> MirrorLayer { get; set; } // 镜像层
        public List<Neuron> CanonicalLayer { get; set; } // 典型层

        public Network()
        {
            InputLayer = new List<Neuron>();
            OutputLayer = new List<Neuron>();
            HiddenLayers = new List<List<Neuron>>();
        }

        public Network(int numInputParameters, int[] hiddenNeurons, int numOutputParameters) : this()
        {
            for (int i = 0; i < numInputParameters; i++)
            {
                InputLayer.Add(new Neuron());
            }

            for (int i = 0; i < numOutputParameters; i++)
            {
                OutputLayer.Add(new Neuron(InputLayer));
            }

            for (int i = 0; i < hiddenNeurons.Length; i++)
            {
                var list = new List<Neuron>();
                HiddenLayers.Add(list);
                for (int j = 0; j < hiddenNeurons[i]; j++)
                {
                    list.Add(new Neuron());
                }
            }
        }

        #region 前向传播&后向传播

        ///<summary>
        /// 前向传播
        ///</summary>
        ///<param name="inputs"></param>
        private void ForwardPropagate(params double[] inputs) // params关键字 -- 可变参数
        {
            var i = 0;
            // 遍历输入层的神经元的值
            InputLayer?.ForEach(a => a.Value = inputs[i++]);
            // 隐藏层是嵌套list，所以要遍历两次
            HiddenLayers?.ForEach(a => a.ForEach(b => b.CalculateValue()));
            // 遍历并计算输出层的值
            OutputLayer?.ForEach(a => a.CalculateValue());
            // 自学者注:先前传播就是对每个突触的所有值求和，通过sigmoid函数得到运行结果以输出
            // 补充基础知识:?是Nu11检查运算符，先检查Inputlaver等是否为空集。
        }

        ///<summary>
        /// 向后传播
        ///</summary>
        ///<param name="targets"></param>2个引用
        private void BackPropagate(params double[] targets)
        {
            var i = 0;
            //从后往前，先遍历输出层
            OutputLayer?.ForEach(a => a.CalculateGradient(targets[i++]));
            HiddenLayers?.Reverse(); // 排序后反转
            // 隐藏层，计算梯度下降
            HiddenLayers?.ForEach(a => a.ForEach(b => b.CalculateGradient()));
            // 更新隐藏层权重
            HiddenLayers?.ForEach(a => a.ForEach(b => b.UpdateWeights(LearningRate, Momentum)));
            HiddenLayers?.Reverse(); // 排序后反转
            // 更新输出层权重
            OutputLayer?.ForEach(a => a.UpdateWeights(LearningRate, Momentum));
        }

        #endregion

        #region 训练方法

        ///<summary>
        /// 规定时期的训练函数
        ///</summary>
        ///<param name="dataSet">带训练的数据集</param>
        ///<param name="numEpochs"> 时期(所有训练样本一次正向+反向传递为一个时期)</param>
        public void Train(List<NNDataSet> dataSet, int numEpochs)
        {
            // 每一次个时期，每一次个数据集都要经历一次正向和反向传递
            for (var i = 0; i < numEpochs; i++)
                foreach (var data in dataSet)
                {
                    ForwardPropagate(data.Values); //输入值前向传播
                    BackPropagate(data.Targets);
                }
        }

        /// <summary>
        ///  规定最小误差的训练函数(方法重载)
        ///  </summary>
        ///  <param name="dataSet">数据集</param>
        ///  <param name="minError">最小误差</param>
        /// <param name="maxNumEpochs">最大迭代次数(防止卡死)</param>
        public void Train(List<NNDataSet> dataSet, double minError, int maxNumEpochs)
        {
            var error = 1.0;
            var numEpochs = 0;
            // 训练至误差小于最小误差，且周期不超过int的最大范围
            while (error > minError && numEpochs < maxNumEpochs)
            {
                var errors = new List<double>();
                // 遍历数据集，进行一轮的前向&后向传播
                foreach (var data in dataSet)
                {
                    ForwardPropagate(data.Values);
                    BackPropagate(data.Values);
                    errors.Add(CalculateError(data.Targets));
                }

                error = errors.Average(); // 求误差均值
                numEpochs++; // 时期递增
            }

            Debug.Log("min error num:" + numEpochs);
            // 循环往复
        }

        public double[] GetOutputWeights(int layerIndex)
        {
            var weights = new double[OutputLayer[layerIndex].InputSynapses.Count];
            for (int i = 0; i < OutputLayer[layerIndex].InputSynapses.Count; i++)
                weights[i] = OutputLayer[layerIndex].InputSynapses[i].Weight;
            return weights;
        }

        public double[] GetOutputBiases()
        {
            return GetBiases(OutputLayer);
        }

        public double[] GetInputWeights(int layerIndex)
        {
            var weights = new double[InputLayer[layerIndex].OutputSynapses.Count];
            for (int i = 0; i < InputLayer[layerIndex].OutputSynapses.Count; i++)
                weights[i] = InputLayer[layerIndex].OutputSynapses[i].Weight;
            return weights;
        }

        public double[] GetInputBiases()
        {
            return GetBiases(InputLayer);
        }

        private double[] GetBiases(List<Neuron> layer)
        {
            var biases = new double[layer.Count];
            for (int i = 0; i < layer.Count; i++)
                biases[i] = layer[i].Bias;
            return biases;
        }

        #endregion

        public double[] Forward(double[] inputs)
        {
            ForwardPropagate(inputs);
            return OutputLayer.Select(a => a.Value).ToArray();
        }

        //ERROR
        //从真实数据计算误差
        public double[] CalculateErrorFromTargets(double[] targets)
        {
            var predictions = Forward(targets);
            double[] errors = new double[predictions.Length];
            for (int i = 0; i < predictions.Length; i++)
                errors[i] = -Math.Log(predictions[i]);

            return errors;
        }

        //从噪声数据计算误差
        public double[] CalculateErrorFromNoises(double[] noises)
        {
            var predictions = Forward(noises);
            double[] errors = new double[predictions.Length];
            for (int i = 0; i < predictions.Length; i++)
                errors[i] = -Math.Log(1 - predictions[i]);

            return errors;
        }

        public double Error(double[] noises, Network D)
        {
            var x = Forward(noises);;
            var y = D.Forward(x);
            return -Math.Log(y[0]);
        }

        private double CalculateError(double[] targets)
        {
            int i = 0;
            return OutputLayer.Sum(a => Math.Abs(a.CalculateError(targets[i++])));
        }
    }
}
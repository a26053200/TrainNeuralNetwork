//
// ClassName：GanNetwork
// Author：zhengnan
// DateTime：2023年09月28日 星期四 11:15
//		

using System.Collections.Generic;
using System.Text;
using NeuralNetwork;
using UnityEngine;

namespace Gan
{
    /// <summary>
    /// 
    /// </summary>
    public class GanNetwork : MonoBehaviour
    {
        private readonly List<NNDataSet> _dataSets = new List<NNDataSet>();
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private float _startTime;


        private Network _discriminator;
        private Network _generator;

        private void Start()
        {
            List<double[]> targetList = new List<double[]>
            {
                new double[] {1, 0, 0, 1},
                new double[] {0.9, 0.1, 0.2, 0.8},
                new double[] {0.9, 0.2, 0.1, 0.8},
                new double[] {0.8, 0.1, 0.2, 0.9},
                new double[] {0.8, 0.2, 0.1, 0.9}
            };

            foreach (var targets in targetList)
                _dataSets.Add(new NNDataSet(new[] {MathUtils.GetRandomRange(), MathUtils.GetRandomRange(), MathUtils.GetRandomRange(), MathUtils.GetRandomRange()}, targets));

            //对抗网络 判别器
            //4个输入元,每个元 1个突触 到 同 1个输出
            _discriminator = new Discriminator();
            
            //对抗网络 生成器
            //4个输入元,每个元 1个突触 到 同 1个输出
            _generator = new Generator();
            
            //测试判别器
            for (int i = 0; i < targetList.Count; i++)
            {
                _debugData1(targetList[i]);
                var results = _discriminator.Compute(targetList[i]);
                _debugData3(results);
            }
        }


        private void Update()
        {
            if (Time.time - _startTime < 1)
                return;
            _startTime = Time.time;
            
            //生成器生成一组数据
            // var inputs = new[] {MathUtils.GetRandomRange()};
            // var inputs = new[] {0.7};
            // _debugData1(inputs);
            // var results = _generator.Compute(inputs);
            // _debugData2(results);
            
        }

        private void _debugData1(double[] values)
        {
            _stringBuilder.Clear();
            foreach (var t in values)
            {
                _stringBuilder.Append(t);
                _stringBuilder.Append(',');
            }

            Debug.Log($"<color=#FFFFFFFF>{_stringBuilder.ToString()}</color>");
        }
        
        private void _debugData2(double[] values)
        {
            _stringBuilder.Clear();
            foreach (var t in values)
            {
                _stringBuilder.Append(t);
                _stringBuilder.Append(',');
            }

            Debug.Log($"<color=#FFFF00FF>{_stringBuilder.ToString()}</color>");
        }
        
        private void _debugData3(double[] values)
        {
            _stringBuilder.Clear();
            foreach (var t in values)
            {
                _stringBuilder.Append(t);
                _stringBuilder.Append(',');
            }

            Debug.Log($"<color=#FF00FFFF>{_stringBuilder.ToString()}</color>");
        }
    }
}
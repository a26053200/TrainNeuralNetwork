//
// ClassName：GanNetwork
// Author：zhengnan
// DateTime：2023年09月28日 星期四 11:15
//		

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuralNetwork;
using UnityEngine;

namespace Example_1
{
    /// <summary>
    /// 
    /// </summary>
    public class Example1 : MonoBehaviour
    {
        private readonly List<NNDataSet> _dataSets = new List<NNDataSet>();
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private Network _network;
        private float _startTime;

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

            _network = new Network(2, new[] {3, 1}, 1);

            //Train
            //min error
            _network.Train(_dataSets, 0.5);
            //max epoch num
            _network.Train(_dataSets, 5);
        }


        private void Update()
        {
            if (Time.time - _startTime < 1)
                return;
            _startTime = Time.time;

            var test = new[] {MathUtils.GetRandomRange(), MathUtils.GetRandomRange(), MathUtils.GetRandomRange(), MathUtils.GetRandomRange()};
            //Test Model
            var results = _network.Compute(test);
            // var results = network.Compute(new double[] {0.8, 0.2, 0.1, 0.9});

            if (results.Length > 0)
            {
                _debugData(test);
                foreach (var result in results)
                {
                    Debug.Log(result >= 0.5 ? "Is Face" : "is not face");
                }
            }
            else
            {
                Debug.Log("test fail");
            }
            _startTime = Time.time;
        }

        private void _debugData(double[] values)
        {
            _stringBuilder.Clear();
            foreach (var t in values)
            {
                _stringBuilder.Append(t);
                _stringBuilder.Append(',');
            }

            Debug.Log(_stringBuilder.ToString());
        }
    }
}
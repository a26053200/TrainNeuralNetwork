﻿//
// ClassName：Network
// Author：zhengnan
// DateTime：2023年09月26日 星期二 17:11
//		

using System;
using System.Collections.Generic;
using System.Linq;

namespace Example_1
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

        public Network(int numInputParameters, int[] hiddenNeurons, int numOutputParameters)
        {
            InputLayer = new List<Neuron>();
            for (int i = 0; i < numInputParameters; i++)
            {
                InputLayer.Add(new Neuron());
            }
            
            OutputLayer = new List<Neuron>();
            for (int i = 0; i < numOutputParameters; i++)
            {
                OutputLayer.Add(new Neuron(InputLayer));
            }

            HiddenLayers = new List<List<Neuron>>();
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
            InputLayer?.ForEach(a => a.Value = inputs[i++]);
            // 遍历输入层的神经元的值
            HiddenLayers?.ForEach(a => a.ForEach(b => b.CalculateValue()));
            // 隐藏层是嵌套list，所以要遍历两次
            OutputLayer?.ForEach(a => a.CalculateValue());
            // 遍历并计算输出层的值
            // 自学者注:先前传播就是对每个突触的所有值求和，通过sigmoid函数得到运行结果以输出// 补充基础知识:?是Nu11检查运算符，先检查Inputlaver等是否为空集。
        }

        ///<summary>
        /// 向后传播
        ///</summary>
        ///<param name="targets"></param>2个引用
        private void BackPropagate(params double[] targets)
        {
            var i = 0;
            OutputLayer?.ForEach(a => a.CalculateGradient(targets[i++]));
            //从后往前，先遍历输出层
            HiddenLayers?.Reverse(); // 排序后反转
            HiddenLayers?.ForEach(a => a.ForEach(b => b.CalculateGradient()));
            // 隐藏层，计算梯度下降
            HiddenLayers?.ForEach(a => a.ForEach(b => b.UpdateWeights(LearningRate, Momentum)));
            // 更新隐藏层权
            HiddenLayers?.Reverse(); // 排序后反转
            OutputLayer?.ForEach(a => a.UpdateWeights(LearningRate, Momentum)); // 更新输出层权重
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

        ///<summary>
        /// 规定最小误差的训练函数(方法重载)
        /// </summary>
        /// <param name="dataSet">数据集</param>
        /// <param name="minError">最小误差</param>
        public void Train(List<NNDataSet> dataSet, double minError)
        {
            var error = 1.0;
            var numEpochs = 0;
// 训练至误差小于最小误差，且周期不超过int的最大范围
            while (error > minError && numEpochs < int.MaxValue)
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
// 循环往复
            }
        }

        #endregion

        public double[] Compute(double[] inputs)
        {
            ForwardPropagate(inputs);
            return OutputLayer.Select(a => a.Value).ToArray();
        }

        private double CalculateError(double[] targets)
        {
            int i = 0;
            return OutputLayer.Sum(a => Math.Abs(a.CalculateError(targets[i++])));
        }
    }
}
//
// ClassName：Neuron
// Author：zhengnan
// DateTime：2023年09月26日 星期二 16:13
//		

using System;
using System.Collections.Generic;
using NeuralNetwork;

namespace Example_1
{
    /// <summary>
    /// 神经元
    /// </summary>
    public class Neuron
    {
        #region 神经元属性

        public Guid Id { get; set; } // 神经元ID
        public List<Synapse> InputSynapses { get; set; } // 连接输入端的突触
        public List<Synapse> OutputSynapses { get; set; } // 连接输出端的突触
        public double Bias { get; set; }
        public double BiasDelta { get; set; } // 偏差值     
        public double Gradient { get; set; } // 梯度值 
        public double Value { get;  set; } // 神经元的输入值
        public bool IsMirror { get; set; } // 镜像神经元标志(后面几章学)
        public bool IsCanonical { get; set; } // 典型神经元标志 (后面几章学)

        #endregion

        public Neuron()
        {
            Id = Guid.NewGuid();
            InputSynapses = new List<Synapse>();
            OutputSynapses = new List<Synapse>();
            Bias = MathUtils.GetRandom();
        }
        
        public Neuron(IEnumerable<Neuron> inputNeurons):this()
        {
            foreach (var inputNeuron in inputNeurons)
            {
                var synapse = new Synapse(inputNeuron,this);
                inputNeuron.OutputSynapses.Add(synapse);
                InputSynapses.Add(synapse);
            }
        }
        
        //计算神经元的值
        public virtual double CalculateValue()
        {
            return Value = Sigmoid.Output(SumInput() + Bias);
        }

        //计算梯度
        public double CalculateGradient(double? target = null) // ? 在这里是可空类型修饰符，使值类型也能为空
        {
            if (target == null)
                return Gradient = SumOutput() * Sigmoid.Derivate(Value);
            return CalculateError(target.Value) * Sigmoid.Derivate(Value);
        }

        //与目标的误差
        public double CalculateError(double target)
        {
            return target - Value;
        }

        /// <summary>
        /// 更新权重
        /// </summary>
        /// <param name="learnRate">学习率</param>
        /// <param name="momentum">动量</param>
        public void UpdateWeights(double learnRate, double momentum)
        {
            var prevDelta = BiasDelta;
            BiasDelta = learnRate * Gradient;
            Bias += BiasDelta + momentum * prevDelta;
            // 遍历输入的突触
            foreach (var synapse in InputSynapses)
            {
                prevDelta = synapse.WeightDelta;
                synapse.WeightDelta = learnRate * Gradient * synapse.InputNeuron.Value; //权重李化量
                synapse.Weight += synapse.WeightDelta + momentum * prevDelta; // 更新权重
            }
        }

        private double SumInput()
        {
            double sum = 0;
            foreach (var s in InputSynapses)
            {
                sum += s.InputNeuron.Value * s.Weight;
            }

            return sum;
        }

        private double SumOutput()
        {
            double sum = 0;
            foreach (var s in OutputSynapses)
            {
                sum += s.OutputNeuron.Gradient * s.Weight;
            }

            return sum;
        }
    }
}
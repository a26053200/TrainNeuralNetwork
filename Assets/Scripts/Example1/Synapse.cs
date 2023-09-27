//
// ClassName：Synapse
// Author：zhengnan
// DateTime：2023年09月26日 星期二 16:12
//		

using System;
using NeuralNetwork;

namespace Example_1
{
    /// <summary>
    /// 突触
    /// </summary>
    public class Synapse
    {
        public Guid Id { get; private set; }
        public Neuron InputNeuron { get; private set; } // 连接突触的输入神经元
        public Neuron OutputNeuron { get; private set; } // 连接突触的输出神经元
        public double Weight { get;   set; } // 权重
        public double WeightDelta { get; set; } // 权重导数

        public Synapse()
        {
        }

        public Synapse(Neuron inputNeuron, Neuron outputNeuron)
        {
            Id = Guid.NewGuid();
            InputNeuron = inputNeuron;
            OutputNeuron = outputNeuron;
            Weight = MathUtils.GetRandom();
        }
    }
}
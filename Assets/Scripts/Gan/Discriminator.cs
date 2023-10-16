//
// ClassName：Discriminator
// Author：zhengnan
// DateTime：2023年09月27日 星期三 18:01
//		

using System.Linq;
using NeuralNetwork;

namespace Gan
{
    /// <summary>
    /// 对抗网络 判别器
    /// </summary>
    public class Discriminator : Network
    {
        public Discriminator() : base()
        {
            // var weights = new double[] {0.60175083, -0.29127513, -0.40093314, 0.37759987};
            // var bias = -0.8955103005797729;
            var weights = new double[] {1, -1, -1, 1};
            var bias = -1;
            //4个输入元,每个元 1个突触 到 同 1个输出
            var outputNeuron = new Neuron()
            {
                Bias = MathUtils.Rand()
                // Bias = bias
            };
            OutputLayer.Add(outputNeuron);
            for (int i = 0; i < 4; i++)
            {
                Neuron inputNeuron = new Neuron();
                Synapse synapse = new Synapse(inputNeuron, outputNeuron)
                {
                    Weight = MathUtils.Rand()
                    // Weight = weights[i]
                };
                InputLayer.Add(inputNeuron);
                inputNeuron.OutputSynapses.Add(synapse);
                outputNeuron.InputSynapses.Add(synapse);
            }
        }

        public void UpdateWeightsFromTargets(double[] targets)
        {
            var predictions = Forward(targets);
            for (int i = 0; i < predictions.Length; i++)
            {
                for (int j = 0; j < OutputLayer[i].InputSynapses.Count; j++)
                    OutputLayer[i].InputSynapses[j].Weight -= -targets[j] * (1 - predictions[i]) * LearningRate;
                OutputLayer[i].Bias -= -(1 - predictions[i]) * LearningRate;
            }
        }

        public void UpdateWeightsFromNoises(double[] noises)
        {
            var predictions = Forward(noises);
            for (int i = 0; i < predictions.Length; i++)
            {
                for (int j = 0; j < OutputLayer[i].InputSynapses.Count; j++)
                    OutputLayer[i].InputSynapses[j].Weight -= noises[j] * predictions[i] * LearningRate;
                OutputLayer[i].Bias -= predictions[i] * LearningRate;
            }
        }
    }
}
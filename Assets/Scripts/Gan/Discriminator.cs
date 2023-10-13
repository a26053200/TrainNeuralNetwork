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
            //4个输入元,每个元 1个突触 到 同 1个输出
            var outputNeuron = new Neuron()
            {
                Bias = MathUtils.Rand()
            };
            OutputLayer.Add(outputNeuron);
            for (int i = 0; i < 4; i++)
            {
                Neuron inputNeuron = new Neuron();
                Synapse synapse = new Synapse(inputNeuron, outputNeuron)
                {
                    Weight = MathUtils.Rand()
                };
                InputLayer.Add(inputNeuron);
                inputNeuron.OutputSynapses.Add(synapse);
                outputNeuron.InputSynapses.Add(synapse);
            }
        }

        public void UpdateWeightsFromTargets(double[] targets)
        {
            ForwardPropagate(targets);
            var predictions = OutputLayer.Select(a => a.Value).ToArray();
            for (int i = 0; i < predictions.Length; i++)
            {
                for (int j = 0; j < OutputLayer[i].InputSynapses.Count; j++)
                    OutputLayer[i].InputSynapses[j].Weight -= -targets[j] * (1 - predictions[i]) * LearningRate;
                OutputLayer[i].Bias -= -(1 - predictions[i]) * LearningRate;
            }
        }

        public void UpdateWeightsFromNoises(double[] noises)
        {
            ForwardPropagate(noises);
            var predictions = OutputLayer.Select(a => a.Value).ToArray();
            for (int i = 0; i < predictions.Length; i++)
            {
                for (int j = 0; j < OutputLayer[i].InputSynapses.Count; j++)
                    OutputLayer[i].InputSynapses[j].Weight -= noises[j] * predictions[i] * LearningRate;
                OutputLayer[i].Bias -= predictions[i] * LearningRate;
            }
        }
    }
}
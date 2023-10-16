//
// ClassName：Generator
// Author：zhengnan
// DateTime：2023年09月27日 星期三 18:00
//		

using System.Linq;
using NeuralNetwork;

namespace Gan
{
    /// <summary>
    /// 对抗网络 生成器
    /// </summary>
    public class Generator : Network
    {
        public Generator()
        {
            //1个输入元, 4个突触 到 4个输出元
            var inputNeuron = new Neuron();
            InputLayer.Add(inputNeuron);

            // var weights = new double[] {0.70702123, 0.03720449, -0.45703394, 0.79375751};
            // var biases = new double[] {2.48490157, -3.36725912, -2.90139211, 2.8172726};
            var weights = new double[] {1, -1, -1, 1};
            var biases = new double[] {1, -1, -1, 1};
            //4个输出元 每个输出有 1个输入突触
            for (int i = 0; i < 4; i++)
            {
                Neuron outputNeuron = new Neuron
                {
                    Bias = MathUtils.Rand()
                    // Bias = biases[i]
                };
                Synapse synapse = new Synapse(inputNeuron, outputNeuron)
                {
                    Weight = MathUtils.Rand()
                    // Weight = weights[i]
                };
                inputNeuron.OutputSynapses.Add(synapse);
                outputNeuron.InputSynapses.Add(synapse);
                OutputLayer.Add(outputNeuron);
            }
        }

        public void Update(double[] noises, Discriminator d)
        {
            // var error_before = Error(noises, D);
            var x = Forward(noises);
            var y = d.Forward(x);
            var dw = d.GetOutputWeights(0);
            var db = d.GetOutputBiases()[0];
            var factor = new double[dw.Length];
            for (int i = 0; i < factor.Length; i++)
            {
                factor[i] = -(1 - y[0]) * dw[i] * x[i] * (1 - x[i]);
                InputLayer[0].OutputSynapses[i].Weight -= factor[i] * noises[i] * LearningRate;
                OutputLayer[i].Bias -= factor[i] * LearningRate;
            }
        }
    }
}
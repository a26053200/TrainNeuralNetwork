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

            //4个输出元 每个输出有 1个输入突触
            for (int i = 0; i < 4; i++)
            {
                Neuron outputNeuron = new Neuron
                {
                    Bias = MathUtils.Rand()
                };
                Synapse synapse = new Synapse(inputNeuron, outputNeuron)
                {
                    Weight = MathUtils.Rand()
                };
                inputNeuron.OutputSynapses.Add(synapse);
                outputNeuron.InputSynapses.Add(synapse);
                OutputLayer.Add(outputNeuron);
            }
        }

        public void Update(double[] noises, Discriminator D)
        {
            // var error_before = Error(noises, D);
            var x = Forward(noises);
            var y = D.Forward(x);
            var dw = D.GetOutputWeights(0);
            var db = D.GetOutputBiases()[0];
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
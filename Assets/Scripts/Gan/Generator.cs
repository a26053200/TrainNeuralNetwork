//
// ClassName：Generator
// Author：zhengnan
// DateTime：2023年09月27日 星期三 18:00
//		

namespace Gan
{
    /// <summary>
    /// 对抗网络 生成器
    /// </summary>
    public class Generator : Network
    {
        public Generator()
        {
            //1个输入元,4个突触 到 4个输出元
            double[][] weightss =
            {
                new double[] {1},
                new double[] {-1},
                new double[] {-1},
                new double[] {1}
            };
            var inputNeuron = new Neuron()
            {
                Bias = 1
            };
            InputLayer.Add(inputNeuron);

            //4个输出元 每个输出有1个输入突触
            double[] biases = {1, -1, -1, 1};
            for (int i = 0; i < biases.Length; i++)
            {
                var bias = biases[i];
                Neuron outputNeuron = new Neuron
                {
                    Bias = bias
                };
                var weights = weightss[i];
                foreach (var weight in weights)
                {
                    Synapse synapse = new Synapse(inputNeuron, outputNeuron)
                    {
                        Weight = weight
                    };
                    outputNeuron.InputSynapses.Add(synapse);
                }

                OutputLayer.Add(outputNeuron);
            }
        }
    }
}
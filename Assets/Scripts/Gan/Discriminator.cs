//
// ClassName：Discriminator
// Author：zhengnan
// DateTime：2023年09月27日 星期三 18:01
//		

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
            double[] weights = {1, -1, -1, 1};
            var outputNeuron = new Neuron()
            {
                Bias = -1
            };
            OutputLayer.Add(outputNeuron);
            for (int i = 0; i < weights.Length; i++)
            {
                Neuron inputNeuron = new Neuron();
                Synapse synapse = new Synapse(inputNeuron, outputNeuron)
                {
                    Weight = weights[i]
                };
                InputLayer.Add(inputNeuron);
                inputNeuron.OutputSynapses.Add(synapse);
                outputNeuron.InputSynapses.Add(synapse);
            }
        }
    }
}
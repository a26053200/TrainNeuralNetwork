
//
// ClassName：Discriminator
// Author：zhengnan
// DateTime：2023年09月27日 星期三 18:01
//		

namespace Gan
{
    public class Discriminator:Network
    {
        public Discriminator(int numInputParameters, int[] hiddenNeurons, int numOutputParameters) : base(numInputParameters, hiddenNeurons, numOutputParameters)
        {
        }
    }
}
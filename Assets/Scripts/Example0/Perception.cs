//
// ClassName：Perception
// Author：zhengnan
// DateTime：2023年09月26日 星期二 15:41
//		

using NeuralNetwork;

namespace Example_0
{
    public class Perception
    {
        private double[] weights; //权重

        public Perception(int size)
        {
            weights = new double[size];
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = MathUtils.GetRandomNumber(-1, 1, 3);
            }
        }

        public Perception(double error, double[] inputs, double rate)
        {
            weights = new double[inputs.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = rate * error * inputs[i];
            }
        }

        public int FeedForward(double[] input)
        {
            double sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                sum += input[i];
            }

            return Active(sum);
        }

        private int Active(double s)
        {
            return s > 0 ? 1 : -1;
        }
    }
}
//
// ClassName：MathUtils
// Author：zhengnan
// DateTime：2023年09月26日 星期二 15:42
//		

using System;

namespace NeuralNetwork
{
    public static class MathUtils
    {
        static readonly Random SRandom = new Random();

        public static double GetRandomNumber(double min, double max, int len)
        {
            return Math.Round(SRandom.NextDouble() * (max - min) + min, len);
        }

        public static double GetRandom()
        {
            return 2 * SRandom.NextDouble() - 1;
        }
    }
}
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

        private static int GetRandomSeed()
        {
            return (int) (int.MaxValue * SRandom.NextDouble());
        }

        public static double GetRandomRange()
        {
            return SRandom.NextDouble();
        }

        //随机产生一个符合正态分布的数 u均数，d为方差
        public static double Rand(double u = 0, double d = 1)
        {
            double u1, u2, z, x;
            //Random ram = new Random();
            if (d <= 0)
                return u;

            u1 = new Random(GetRandomSeed()).NextDouble();
            u2 = new Random(GetRandomSeed()).NextDouble();

            z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

            x = u + d * z;
            return x;
        }
    }
}
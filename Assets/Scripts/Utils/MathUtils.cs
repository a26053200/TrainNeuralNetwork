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
        static Random _seedRandom = new Random(42);

        public static double GetRandomNumber(double min, double max, int len)
        {
            return Math.Round(_seedRandom.NextDouble() * (max - min) + min, len);
        }

        public static void SetRandomSeed(int seed)
        {
            _seedRandom = new Random(seed);
        }

        public static double GetRandom()
        {
            return 2 * _seedRandom.NextDouble() - 1;
        }

        private static int GetRandomSeed()
        {
            return (int) (int.MaxValue * _seedRandom.NextDouble());
        }

        private static int GetRandomSeed(int seed)
        {
            return (int) (int.MaxValue * _seedRandom.NextDouble());
        }

        public static double GetRandomRange()
        {
            return _seedRandom.NextDouble();
        }

        //随机产生一个符合正态分布的数 u均数，d为方差
        public static double Rand(double u = 0, double d = 1)
        {
            // double u1, u2, z, x;
            // //Random ram = new Random();
            // if (d <= 0)
            //     return u;
            //
            // u1 = SeedRandom.NextDouble();
            // u2 = SeedRandom.NextDouble();
            //
            // z = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
            //
            // x = u + d * z;
            // return x;

            var nd = NormalDistribution();
            return nd[0];
        }

        public static double[] NormalDistribution()
        {
            // Random rand = new Random();
            double u1, u2, v1 = 0, v2 = 0, s = 0, z1 = 0, z2 = 0;
            while (s > 1 || s == 0)
            {
                u1 = _seedRandom.NextDouble();
                u2 = _seedRandom.NextDouble();
                v1 = 2 * u1 - 1;
                v2 = 2 * u2 - 1;
                s = v1 * v1 + v2 * v2;
            }

            z1 = Math.Sqrt(-2 * Math.Log(s) / s) * v1;
            z2 = Math.Sqrt(-2 * Math.Log(s) / s) * v2;
            return new[] {z1, z2}; //返回两个服从正态分布N(0,1)的随机数z0 和 z1
        }
    }
}
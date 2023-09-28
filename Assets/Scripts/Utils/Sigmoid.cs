//
// ClassName：Sigmoid
// Author：zhengnan
// DateTime：2023年09月26日 星期二 16:42
//		

using System;

namespace NeuralNetwork
{
    public static class Sigmoid
    {
        /// <summary>
        /// 激活函数的输出
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Output(double x)
        {
            // return x < -45.0 ? 0.0 : x > 45.0 ? 1.0 : 1.0 / (1.0 + Math.Exp(-x));
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        ///<summary>
        /// Sigmoid求导
        /// </summary>
        /// <param name="x">值</param>
        ///<returns>求导结果</returns>
        public static double Derivate(double x)
        {
            return x * (1 - x);
        }
    }
}
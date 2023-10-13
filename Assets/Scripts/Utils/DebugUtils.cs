
//
// ClassName：DebugUtils
// Author：zhengnan
// DateTime：2023年10月13日 星期五 17:37
//		

using System.Text;
using UnityEngine;

namespace NeuralNetwork
{
    public static class DebugUtils
    {
        
        private static readonly StringBuilder Sb = new StringBuilder();

        public static string DoubleArray2String(double[] values)
        {
            Sb.Clear();
            for (int i = 0; i < values.Length; i++)
            {
                Sb.Append(values[i]);
                if (i < values.Length - 1) Sb.Append(',');
            }
            return Sb.ToString();
        }

        public static void _debugData1(double[] values)
        {
            DoubleArray2String(values);
            Debug.Log($"<color=#FFFFFFFF>{DoubleArray2String(values)}</color>");
        }

        public static void _debugData2(double[] values)
        {
            DoubleArray2String(values);

            Debug.Log($"<color=#FFFF00FF>{DoubleArray2String(values)}</color>");
        }

        public static void _debugData3(double[] values)
        {
            DoubleArray2String(values);

            Debug.Log($"<color=#FF00FFFF>{DoubleArray2String(values)}</color>");
        }

        public static void _debugError(double[] values)
        {
            DoubleArray2String(values);
            Debug.LogError($"<color=#FF00FFFF>{DoubleArray2String(values)}</color>");
        }
    }
}
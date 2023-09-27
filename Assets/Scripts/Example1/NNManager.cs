//
// ClassName：NNManager
// Author：zhengnan
// DateTime：2023年09月26日 星期二 17:48
//		

using System.Collections.Generic;

namespace Example_1
{
    public class NNManager
    {
        #region 字段

        private int _numInputParameters; // 输入参数的数量
        private int _numHiddenLayers; // 隐藏层数量
        private int[] _hiddenNeurons; // 隐藏层的神经元
        private int _numOutputParameters; //输出参数的数量
        private Network _network; // 网络
        private List<NNDataSet> _dataSet; //数据集

        #endregion

        public NNManager SetUp()
        {
            _numInputParameters = 2;
            int[] hidden = new int[_numInputParameters];
            hidden[0] = 3;
            hidden[1] = 1;
            _numHiddenLayers = 1; //一个隐藏层
            _hiddenNeurons = hidden; // 隐藏的神经元
            _numOutputParameters = 1; //输出的参数
            _network = new Network(_numInputParameters, _hiddenNeurons, _numOutputParameters);
            return this;
        }
    }
}
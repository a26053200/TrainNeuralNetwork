
//
// ClassName：NNDataSet
// Author：zhengnan
// DateTime：2023年09月26日 星期二 17:33
//		

namespace Gan
{
    public class NNDataSet
    {
        public double[] Values { get; set; }
        public double[] Targets { get; set; }

        public NNDataSet(double[] values,double[] targets)
        {
            Values = values;
            Targets = targets;
        }
    }
}
using Example_0;
using UnityEngine;

public class Example0 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Perception p = new Perception(5);
        double[] points = {1.15,-12.5,-15.2,-2,-1.11};
        int result = p.FeedForward(points);  // 引入训练的过程
        double c = 0.01;  //学习率
        int desire = 1;  // 希望获得的结果
        double error = desire - result;  // 误差率
        Perception p2 = new Perception(error, points, c); // 重构函数
        int result2 = p2.FeedForward(points);
        // 自学者的感受:
        // 这个程序最好打个断点一步一步运行，主要观察这个感知器运行的过程
        // 最简单的感知器大概就是这么个意思，也可以调用循环来进一步精确
    }
}

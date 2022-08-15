using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Math;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private readonly double partialRate = 20d;
        private double Deci { get => dx * partialRate; }
        public void DrawFunction(Func<double, double, double> func)
        {
            if (func == null)
                return;
            int blockCntForWidth = (int)((widthLim.to - widthLim.from) / dx); // 截取整数部分相当于向下取整
            double[][] saveResult = new double[2][]
            {
                new double[blockCntForWidth+1],
                new double[blockCntForWidth+1]
            };
            double[] nowResult = saveResult[0], preResult = saveResult[1];
            bool isFirstLine = true;
            for (double j = heightLim.to; j >= heightLim.from; j -= Deci)
            {
                int index = 0;
                for (double i = widthLim.from; i < widthLim.to; i += Deci, index++)
                {
                    nowResult[index] = func(i, j);
                    if (!isFirstLine && index != 0)
                    {
                        DrawLineInSquare(new Point(i, -j),
                            preResult[index - 1], preResult[index], nowResult[index - 1], nowResult[index]);
                    }
                }
                isFirstLine = false;
                double[] tmp = nowResult;
                nowResult = preResult;
                preResult = tmp;
            }
        }

        private void DrawLineInSquare(Point point, double v1, double v2, double v3, double v4)
        {
            double x = point.X, y = point.Y;
            double half = Deci / 2d;
            Point from = point, to = point;
            int val;
            switch (val = GetSquareType(v1, v2, v3, v4))
            {
                case 0b1111:
                case 0b0000:
                    break;
                case 0b1110:
                case 0b0001:
                    from = new Point(x - half, y);
                    to = new Point(x - Deci, y - half);
                    break;
                case 0b1101:
                case 0b0010:
                    from = new Point(x - half, y);
                    to = new Point(x, y - half);
                    break;
                case 0b1011:
                case 0b0100:
                    from = new Point(x, y - half);
                    to = new Point(x - half, y - Deci);
                    break;
                case 0b0111:
                case 0b1000:
                    from = new Point(x - Deci, y - half);
                    to = new Point(x - half, y - Deci);
                    break;
                case 0b1100:
                case 0b0011:
                    from = new Point(x, y - half);
                    to = new Point(x - Deci, y - half);
                    break;
                case 0b1001:
                case 0b0110:
                    from = new Point(x - half, y);
                    to = new Point(x - half, y - Deci);
                    break;
                default:
                    break;
            }
            brush.DrawLine(FuncsPenSetting, from, to);
            //throw new NotImplementedException();
        }

        private int GetSquareType(double v1, double v2, double v3, double v4)
        {
            double[] values = { v1, v2, v3, v4 };
            int rt = 0b0000;
            for (int i = 0; i < 4; i++)
            {
                rt <<= 1;
                if (values[i] > 0)
                    rt |= 0b0001;
            }
            return rt;
        }
    }
}

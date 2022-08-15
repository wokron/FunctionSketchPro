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

            // 截取整数部分相当于向下取整
            int blockCntForWidth = (int)((widthLim.to - widthLim.from) / Deci); 
            int blockCntForHeight = (int)((heightLim.to - heightLim.from) / Deci);
            
            double[,] saveValues = new double[blockCntForHeight + 1, blockCntForWidth + 1];
            int jcnt = 0;
            for (double j = heightLim.to; j >= heightLim.from; j -= Deci, jcnt++)
            {
                int icnt = 0;
                for (double i = widthLim.from; i < widthLim.to; i += Deci, icnt++)
                {
                    saveValues[jcnt, icnt] = func(i, j);
                    if (icnt > 0 && jcnt > 0)
                    {
                        DrawLineInSquare(new Point(i, j),
                            saveValues[jcnt - 1, icnt - 1],
                            saveValues[jcnt - 1, icnt],
                            saveValues[jcnt, icnt],
                            saveValues[jcnt, icnt - 1]);
                    }
                }
            }
        }

        private void DrawLineInSquare(Point point, double v1, double v2, double v3, double v4)
        {
            var vPairs = GetTransferVectorPaires(v1, v2, v3, v4);
            foreach ((Vector v1, Vector v2) vPair in vPairs)
            {
                DrawLineWithCoordPoints(FuncsPenSetting, point + vPair.v1, point + vPair.v2);
            }
        }

        private IEnumerable<(Vector, Vector)> GetTransferVectorPaires(double v1, double v2, double v3, double v4)
        {
            double full = Deci, half = full / 2d;
            switch (GetSquareType(v1, v2, v3, v4))
            {
                case 0b1111:
                case 0b0000:
                    yield return CreateVectorPair(0, 0, 0, 0);
                    break;
                case 0b1110:
                case 0b0001:
                    yield return CreateVectorPair(-full, half, -half, 0);
                    break;
                case 0b1101:
                case 0b0010:
                    yield return CreateVectorPair(-half, 0, 0, half);
                    break;
                case 0b1011:
                case 0b0100:
                    yield return CreateVectorPair(0, half, -half, full);
                    break;
                case 0b0111:
                case 0b1000:
                    yield return CreateVectorPair(-full, half, -half, full);
                    break;
                case 0b1100:
                case 0b0011:
                    yield return CreateVectorPair(-full, half, 0, half);
                    break;
                case 0b1001:
                case 0b0110:
                    yield return CreateVectorPair(-half, 0, -half, full);
                    break;
                case 0b1010:
                    yield return CreateVectorPair(-half, 0, 0, half);
                    yield return CreateVectorPair(-full, half, -half, full);
                    break;
                case 0b0101:
                    yield return CreateVectorPair(-full, half, -half, 0);
                    yield return CreateVectorPair(0, half, -half, full);
                    break;
                default:
                    throw new ArgumentException("不可能出现的square类型");
            }
        }

        private (Vector, Vector) CreateVectorPair(double v1, double v2, double v3, double v4)
            => (new Vector(v1, v2), new Vector(v3, v4));

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

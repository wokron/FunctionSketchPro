using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Math;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private readonly double partialRate = 50d;
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
                        /* 请注意四个点值的顺序为从左上角开始顺时针 */
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
            double rate1 = 0.5, rate2 = 0.5;
            switch (GetSquareType(v1, v2, v3, v4))
            {
                case 0b1111:
                case 0b0000:
                    yield return CreateVectorPair(0, 0, 0, 0);
                    break;
                case 0b1110:
                case 0b0001:
                    rate1 = Abs(v4 / (v1 - v4));
                    rate2 = Abs(v3 / (v3 - v4));
                    yield return CreateVectorPair(-full, full * rate1, -full * rate2, 0);
                    break;
                case 0b1101:
                case 0b0010:
                    rate1 = Abs(v3 / (v3 - v4));
                    rate2 = Abs(v3 / (v2 - v3));
                    yield return CreateVectorPair(-full * rate1, 0, 0, full * rate2);
                    break;
                case 0b1011:
                case 0b0100:
                    rate1 = Abs(v2 / (v1 - v2));
                    rate2 = Abs(v3 / (v2 - v3));
                    yield return CreateVectorPair(-full * rate1, full, 0, full * rate2);
                    break;
                case 0b0111:
                case 0b1000:
                    rate1 = Abs(v4 / (v1 - v4));
                    rate2 = Abs(v2 / (v1 - v2));
                    yield return CreateVectorPair(-full, full * rate1, -full * rate2, full);
                    break;
                case 0b1100:
                case 0b0011:
                    rate1 = Abs(v4 / (v1 - v4));
                    rate2 = Abs(v3 / (v2 - v3));
                    yield return CreateVectorPair(-full, full * rate1, 0, full * rate2);
                    break;
                case 0b1001:
                case 0b0110:
                    rate1 = Abs(v3 / (v3 - v4));
                    rate2 = Abs(v2 / (v1 - v2));
                    yield return CreateVectorPair(-full * rate1, 0, -full * rate2, full);
                    break;
                case 0b1010:
                    rate1 = Abs(v4 / (v1 - v4));
                    rate2 = Abs(v2 / (v1 - v2));
                    yield return CreateVectorPair(-full, full * rate1, -full * rate2, full);
                    rate1 = Abs(v3 / (v3 - v4));
                    rate2 = Abs(v3 / (v2 - v3));
                    yield return CreateVectorPair(-full * rate1, 0, 0, full * rate2);
                    break;
                case 0b0101:
                    rate1 = Abs(v4 / (v1 - v4));
                    rate2 = Abs(v3 / (v3 - v4));
                    yield return CreateVectorPair(-full, full * rate1, -full * rate2, 0);
                    rate1 = Abs(v2 / (v1 - v2));
                    rate2 = Abs(v3 / (v2 - v3));
                    yield return CreateVectorPair(-full * rate1, full, 0, full * rate2);
                    break;
                default:
                    throw new ArgumentException("不可能出现的square类型");
            }
        }

        private (Vector, Vector) CreateVectorPair(double v1, double v2, double v3, double v4)
            => (new Vector(v1, v2), new Vector(v3, v4));

        private byte GetSquareType(double v1, double v2, double v3, double v4)
        {
            double[] values = { v1, v2, v3, v4 };
            byte rt = 0b0000;
            for (int i = 0; i < 4; i++)
            {
                rt <<= 1;
                if (values[i] > float.Epsilon)
                    rt |= 0b0001;
            }
            return rt;
        }
    }
}

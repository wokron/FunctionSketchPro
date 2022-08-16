using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Math;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private readonly double partialRate = 10d;
        private double Deci { get => dx * partialRate; }
        public void DrawFunction(Func<double, double, double> func)
        {
            if (func == null)
                return;

            int blockCntForWidth = (int)((widthLim.to - widthLim.from) / Deci + 1); 
            int blockCntForHeight = (int)((heightLim.to - heightLim.from) / Deci + 1);
            /* 末尾加一，向上取整 */

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
            double full = Deci;
            SquareType type = SquareType.GetType(GetSquareType(v1, v2, v3, v4));
            return type.GetVectorPairs(Deci, v1, v2, v3, v4);
        }

        

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

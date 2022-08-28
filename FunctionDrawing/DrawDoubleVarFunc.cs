using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Math;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        public double PartialRate { get; set; } = 10d;
        private double Deci { get => dx * PartialRate; }
        public void DrawFunction(DoubleVarFuncStorage save)
        {
            if (save == null)
                return;

            var func = save.GetFunc();

            int blockCntForWidth = (int)((WidthLim.to - WidthLim.from) / Deci + 1); 
            /* 末尾加一，向上取整 */

            double[][] saveValues = new double[2][] // 滚动数组
            {
                new double[blockCntForWidth + 1],
                new double[blockCntForWidth + 1]
            };
            double[] nowVals = saveValues[0], preVals = saveValues[1];
            int jcnt = 0;
            for (double j = HeightLim.to; j >= HeightLim.from; j -= Deci, jcnt++)
            {
                int icnt = 0;
                for (double i = WidthLim.from; i < WidthLim.to; i += Deci, icnt++)
                {
                    nowVals[icnt] = func(i, j);
                    if (icnt > 0 && jcnt > 0)
                    {
                        DrawLineInSquare(new Point(i, j),
                            preVals[icnt - 1],
                            preVals[icnt],
                            nowVals[icnt],
                            nowVals[icnt - 1]);
                        /* 请注意四个点值的顺序为从左上角开始顺时针 */
                    }
                }
                SwapArrays(ref nowVals, ref preVals);
            }
        }

        private void SwapArrays<T> (ref T[] arr1, ref T[] arr2)
        {
            T[] tmp = arr1;
            arr1 = arr2;
            arr2 = tmp;
        }

        private void DrawLineInSquare(Point point, double v1, double v2, double v3, double v4)
        {
            var vPairs = GetTransferVectorPaires(v1, v2, v3, v4);
            foreach ((Vector v1, Vector v2) vPair in vPairs)
            {
                DrawLineWithCoordPoints(DefaultFuncsPenBrush, FunctionLineThickness, point + vPair.v1, point + vPair.v2);
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

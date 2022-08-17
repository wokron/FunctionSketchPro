using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using static System.Math;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private void DrawFunction(Func<double, double> func, Matrix matrix = default)
        {
            if (func == null)
                return;

            /* 单变量函数是一种特殊的参数函数 */
            ParamFuncHelper helper = new ParamFuncHelper(x => x, func);
            if (!IsPolarPlot)
                DrawFunction(helper.Calculate, matrix, widthLim.from, widthLim.to);
            else
                DrawFunction(helper.Calculate, matrix, 0, 2 * PI);
        }
    }
}

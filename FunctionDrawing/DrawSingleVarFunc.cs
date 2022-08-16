using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Math;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private void DrawFunction(Func<double, double> func)
        {
            if (func == null)
                return;

            /* 单变量函数是一种特殊的参数函数 */
            ParamFuncHelper helper = new ParamFuncHelper(x => x, func);
            DrawFunction(helper.Calculate, WidthLim.from, WidthLim.to);
        }
    }
}

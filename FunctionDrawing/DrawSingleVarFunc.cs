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
        private void DrawFunction(SingleVarFuncStorage save)
        {
            if (save == null)
                return;

            /* 单变量函数是一种特殊的参数函数 */
            ParamVarFuncStorage param = save.ConvertToParamFunc();
            if (!IsPolarPlot)
                param.SetRange(WidthLim.from, WidthLim.to);
            else
                param.SetRange(2 * PI);
            DrawFunction(param);
        }
    }
}

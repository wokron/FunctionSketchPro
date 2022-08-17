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

            if (save.HasIntegration())
            {
                var func = save.GetFunc();
                LimitRange interationRange = save.GetInterationRange();
                double leftBorder = WidthLim.RestrictNum(interationRange.from);
                double rightBorder = WidthLim.RestrictNum(interationRange.to);
                double leftVal = HeightLim.RestrictNum(func(leftBorder));
                double rightVal = HeightLim.RestrictNum(func(rightBorder));
                DrawLineWithCoordPoints(FuncsPenSetting ,new Point(leftBorder, 0), new Point(leftBorder, leftVal));
                DrawLineWithCoordPoints(FuncsPenSetting ,new Point(rightBorder, 0), new Point(rightBorder, rightVal));

            }
        }
    }
}

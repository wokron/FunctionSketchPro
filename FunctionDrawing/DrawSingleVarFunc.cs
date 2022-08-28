using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
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
            if (!save.IsPolarPlot)
                param.SetRange(WidthLim);
            else
                param.SetRange(2 * PI);
            DrawFunction(param);

            if (save.HasIntegration())
                DrawIntegrationArea(save);
        }

        private void DrawIntegrationArea(SingleVarFuncStorage save)
        {
            var func = save.GetFunc();
            var interationRange = save.GetInterationRange();
            double leftBorder = WidthLim.RestrictNum(interationRange.from);
            double rightBorder = WidthLim.RestrictNum(interationRange.to);
            double preY = func(leftBorder);
            double prei = leftBorder;
            for (double i = leftBorder + dx; i <= rightBorder; i += dx)
            {
                if (i + dx > rightBorder) // 最后一个位置
                    i = rightBorder;
                double y = func(i);
                double zeroY = 0;
                Point p1 = RestrictPoint(PointTransform(new Point(prei, preY), save)),
                    p2 = RestrictPoint(PointTransform(new Point(i, y), save)),
                    p3 = RestrictPoint(PointTransform(new Point(i, zeroY), save)),
                    p4 = RestrictPoint(PointTransform(new Point(prei, zeroY), save));
                FillQuadrangleArea(p1, p2, p3, p4);
                preY = y;
                prei = i;
            }
        }

        private Point RestrictPoint(Point p)
        {
            double x = WidthLim.RestrictNum(p.X);
            double y = HeightLim.RestrictNum(p.Y);
            return new Point(x, y);
        }

        private void FillQuadrangleArea(Point p1, Point p2, Point p3, Point p4)
        {
            Matrix trans = new Matrix(1, 0, 0, -1, 0, 0);
            PathGeometry geometry = new PathGeometry()
            {
                Figures = new PathFigureCollection()
                {
                    new PathFigure()
                    {
                        StartPoint = p1 * trans,
                        IsClosed = true,
                        Segments = new PathSegmentCollection()
                        {
                            new LineSegment(p2 * trans, false),
                            new LineSegment(p3 * trans, false),
                            new LineSegment(p4 * trans, false)
                        }
                    }
                }
            };
            brush.DrawGeometry(IntegrationAreaBrush, null, geometry);
        }
    }
}

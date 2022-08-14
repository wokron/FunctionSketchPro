using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private readonly double argumentLimit = 50d;
        
        public void DrawFunction(Func<double, (double, double)> func)
        {
            if (func == null)
                return;

            SortedList<double, Point> points = new SortedList<double, Point>();
            IList<Point> sortedPoints = points.Values;
            (LimitRange widthLim, LimitRange heightLim) = GetDrawingArea();
            for (double i = widthLim.from; i <= widthLim.to; i += dx)
            {
                (double,double) result = func(i);
                double x = result.Item1;
                double y = -result.Item2;
                if (heightLim.Contains(y))
                    points.Add(i, new Point(x, y));
            }

            for (int i = 1; i < points.Count; i++)
            {
                Point p1 = sortedPoints[i - 1], p2 = sortedPoints[i];
                if (FindDiscontinuityPoint(p1, p2, heightLim))
                    continue;
                brush.DrawLine(FuncsPenSetting, p1, p2);
            }
        }
    }
}
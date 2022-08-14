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

            SortedList<double, Point> points = new SortedList<double, Point>();
            IList<Point> sortedPoints = points.Values;
            (LimitRange widthLim, LimitRange heightLim) = GetDrawingArea();
            for (double i = widthLim.from; i <= widthLim.to; i += dx)
            {
                double y = -func(i);
                if (heightLim.Contains(y))
                    points.Add(i, new Point(i, y));
                if (points.Count >= 3)
                {
                    Point p3 = sortedPoints[points.Count - 1],
                        p2 = sortedPoints[points.Count - 2],
                        p1 = sortedPoints[points.Count - 3];
                    SmoothGraphByAddingPoint(points, func, heightLim, p1, p2, p3);
                }
            }

            for (int i = 1; i < points.Count; i++)
            {
                Point p1 = sortedPoints[i - 1], p2 = sortedPoints[i];
                if (FindDiscontinuityPoint(p1, p2, heightLim))
                    continue;
                brush.DrawLine(FuncsPenSetting, p1, p2);
            }
        }

        private void SmoothGraphByAddingPoint(
            SortedList<double, Point> points,
            Func<double, double> func,
            LimitRange heightLim,
            Point p1, Point p2, Point p3, int recurNum = 1)
        {
            if (Abs((p2.Y - p1.Y) + (p2.Y - p3.Y)) < smoothRate
                || recurNum > maxRecur)
                return;

            double x1 = (p1.X + p2.X) / 2d;
            Point mid1 = new Point(x1, -func(x1));
            if (heightLim.Contains(mid1.Y))
            {
                points.Add(mid1.X, mid1);
                SmoothGraphByAddingPoint(
                    points, func, heightLim,
                    p1, mid1, p2, recurNum + 1);
            }

            double x2 = (p2.X + p3.X) / 2d;
            Point mid2 = new Point(x2, -func(x2));
            if (heightLim.Contains(mid2.Y))
            {
                points.Add(mid2.X, mid2);
                SmoothGraphByAddingPoint(
                    points, func, heightLim,
                    p2, mid2, p3, recurNum + 1);
            }
        }

        private bool FindDiscontinuityPoint(Point p1, Point p2, LimitRange heightlim)
        {
            double eps = 2;
            double from = heightlim.from, to = heightlim.to;
            return (to - p1.Y < eps
                && p2.Y - from < eps)
                || (to - p2.Y < eps
                && p1.Y - from < eps);
        }
    }
}

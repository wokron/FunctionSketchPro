using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Math;
namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private void DrawFunction(Func<double, (double, double)> func, double start = 0d, double end = 50d)
        {
            if (func == null)
                return;

            SortedList<double, Point> points = new SortedList<double, Point>();
            IList<Point> sortedPoints = points.Values;
            IList<double> saveArguments = points.Keys;
            (LimitRange widthLim, LimitRange heightLim) = GetDrawingArea();
            for (double i = start; i <= end; i += dx)
            {
                (double, double) result = func(i);
                double x = result.Item1;
                double y = -result.Item2;
                if (widthLim.Contains(x) && heightLim.Contains(y))
                    points.Add(i, new Point(x, y));
                if (points.Count >= 3)
                {
                    double t3 = saveArguments[points.Count - 1],
                        t2 = saveArguments[points.Count - 2],
                        t1 = saveArguments[points.Count - 3];
                    SmoothGraphByAddingPoint(points, func, widthLim, heightLim, t1, t2, t3);
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

        private readonly double smoothRate = 0.1d;
        private readonly int maxRecur = 5;

        private void SmoothGraphByAddingPoint(
            SortedList<double, Point> points,
            Func<double, (double, double)> func,
            LimitRange widthLim, LimitRange heightLim,
            double t1, double t2, double t3, int recurNum = 1)
        {
            Point p1 = points[t1], p2 = points[t2], p3 = points[t3];
            if (Abs((p2.Y - p1.Y) + (p2.Y - p3.Y)) < smoothRate
                || recurNum > maxRecur)
                return;

            double midt = (t1 + t2) / 2d;
            (double x, double y) = func(midt);
            if (widthLim.Contains(x) && heightLim.Contains(-y))
            {
                points.Add(midt, new Point(x, -y));
                SmoothGraphByAddingPoint(
                    points, func, widthLim, heightLim,
                    t1, midt, t2, recurNum + 1);
            }

            midt = (t2 + t3) / 2d;
            (x, y) = func(midt);
            if (widthLim.Contains(x) && heightLim.Contains(-y))
            {
                points.Add(midt, new Point(x, -y));
                SmoothGraphByAddingPoint(
                    points, func, widthLim, heightLim,
                    t2, midt, t3, recurNum + 1);
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
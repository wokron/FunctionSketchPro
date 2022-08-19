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
        private LimitRange WidthLim
        {
            get
            {
                double halfWidth = UnitNumForWidth / 2d;
                LimitRange widthLim = new LimitRange(
                    middlePoint.X - halfWidth,
                    middlePoint.X + halfWidth);
                return widthLim;
            }
        }

        private LimitRange HeightLim
        {
            get
            {
                double height = UnitNumForWidth * AspectRatio;
                double halfHeight = height / 2d;
                LimitRange heightLim = new LimitRange(
                    middlePoint.Y - halfHeight,
                    middlePoint.Y + halfHeight);
                return heightLim;
            }
        }

        private void DrawFunction(ParamVarFuncStorage save)
        {
            if (save == null)
                return;

            Func<double, (double, double)> func = save.GetFunc();
            Matrix trans = save.Transform;
            LimitRange range = save.GetRange();

            SortedList<double, Point> points = new SortedList<double, Point>();
            IList<Point> sortedPoints = points.Values;
            IList<double> saveArguments = points.Keys;
            for (double i = range.from; i <= range.to; i += dx)
            {
                (double x, double y) = func(i);
                Point draw = PointTransform(new Point(x, y), save);
                if (WidthLim.Contains(draw.X) && HeightLim.Contains(draw.Y))
                    points.Add(i, draw);
                else
                    points.Add(i, new Point(double.NaN, double.NaN));
                if (points.Count >= 3)
                {
                    double t3 = saveArguments[points.Count - 1],
                        t2 = saveArguments[points.Count - 2],
                        t1 = saveArguments[points.Count - 3];
                    SmoothGraphByAddingPoint(points, save, t1, t2, t3);
                }
            }

            for (int i = 1; i < points.Count; i++)
            {
                Point p1 = sortedPoints[i - 1], p2 = sortedPoints[i];
                if (FindDiscontinuityPoint(p1, p2))
                    continue;
                DrawLineWithCoordPoints(FuncsPenSetting, p1, p2);
            }
        }

        private readonly double smoothRate = 0.1d;
        private readonly int maxRecur = 5;

        private void SmoothGraphByAddingPoint(
            SortedList<double, Point> points,
            ParamVarFuncStorage save,
            double t1, double t2, double t3, int recurNum = 1)
        {
            Point p1 = points[t1], p2 = points[t2], p3 = points[t3];
            if (Abs((p2.Y - p1.Y) + (p2.Y - p3.Y)) < smoothRate
                || p1.X == double.NaN || p2.X == double.NaN || p3.X == double.NaN
                || recurNum > maxRecur)
                return;

            var func = save.GetFunc();
            var trans = save.Transform;

            double midt = (t1 + t2) / 2d;
            (double x, double y) = func(midt);
            Point draw = PointTransform(new Point(x, y), save);
            if (WidthLim.Contains(draw.X) && HeightLim.Contains(draw.Y))
            {
                points.Add(midt, draw);
                SmoothGraphByAddingPoint(
                    points, save,
                    t1, midt, t2, recurNum + 1);
            }

            midt = (t2 + t3) / 2d;
            (x, y) = func(midt);
            draw = PointTransform(new Point(x, y), save);
            if (WidthLim.Contains(draw.X) && HeightLim.Contains(draw.Y))
            {
                points.Add(midt, draw);
                SmoothGraphByAddingPoint(
                    points, save,
                    t2, midt, t3, recurNum + 1);
            }
        }

        private Point PointTransform(Point point, AbstractParamFuncStorage save)
        {
            if (save.IsPolarPlot)
                point = PolarTransform(point);
            return point * save.Transform;
        }

        private Point PolarTransform(Point p)
        {
            double theta = p.X, r = p.Y;
            return new Point(r * Cos(theta), r * Sin(theta));
        }

        private bool FindDiscontinuityPoint(Point p1, Point p2)
        {
            return p1.X == double.NaN || p2.X == double.NaN;
        }
    }
}
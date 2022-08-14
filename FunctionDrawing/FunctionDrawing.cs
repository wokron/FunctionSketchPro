using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Math;

namespace FunctionSketch
{
    public class FunctionDrawing
    {
        List<FunctionStorage> saveFunctions = new List<FunctionStorage>();
        DrawingGroup funcsCanvas = new DrawingGroup();
        DrawingContext brush;
        Image targetImage = null;
        Point middlePoint = new Point(0d, 0d);
        private readonly double dx = 0.01;

        public double UnitNumForWidth { get; set; }
        public double AspectRatio { get; set; }
        public Pen FuncsPenSetting { get; set; }
        public Pen CoordPenSetting { get; set; }
        public bool AutoRefresh { get; set; }

        public FunctionDrawing()
        {
            UnitNumForWidth = 20;
            AutoRefresh = true;
            AspectRatio = 0.618;
            FuncsPenSetting = new Pen(Brushes.Black, 0.05);
            CoordPenSetting = new Pen(Brushes.Gray, 0.07);
        }
        public FunctionDrawing(FunctionStorage[] funcsStore) : this()
        {
            AddFunction(funcsStore);
        }

        public void AddFunction(FunctionStorage[] funcsStore)
        {
            foreach (var store in funcsStore)
                saveFunctions.Add(store);
            Refresh();
        }

        public void AddFunction(FunctionStorage store)
        {
            saveFunctions.Add(store);
            Refresh();
        }

        // 可以把体积大的函数拆开放
        public void AddFunction(Func<double, double> func)
        {
            saveFunctions.Add(new SingleVarFuncStorage(func));
            Refresh();
        }

        private void DrawFunction(Func<double, double> func)
        {
            if (func == null)
                return;

            List<Point> points = new List<Point>();
            (LimitRange widthLim, LimitRange heightLim) = GetDrawingArea();
            for (double i = widthLim.from; i <= widthLim.to; i += dx)
            {
                double y = -func(i);
                if (heightLim.from <= y && y <= heightLim.to)
                    points.Add(new Point(i, y));
            }

            for (int i = 1; i < points.Count; i++)
            {
                brush.DrawLine(FuncsPenSetting, points[i - 1], points[i]);
            }
        }

        private (LimitRange, LimitRange) GetDrawingArea()
        {
            double halfWidth = UnitNumForWidth / 2d;
            double height = UnitNumForWidth * AspectRatio;
            double halfHeight = height / 2d;
            LimitRange widthLim = new LimitRange(
                middlePoint.X - halfWidth,
                middlePoint.X + halfWidth);
            LimitRange heightLim = new LimitRange(
                middlePoint.Y - halfHeight,
                middlePoint.Y + halfHeight);
            return (widthLim, heightLim);
        }

        public void AddFunction(Func<double, (double, double)> func)
        {
            saveFunctions.Add(new ParamVarFuncStorage(func));
            Refresh();
        }

        public void DrawFunction(Func<double, (double, double)> func)
        {
            if (func == null)
                return;

            throw new NotImplementedException();
        }

        public void AddFunction(Func<double, double, double> func)
        {
            saveFunctions.Add(new DoubleVarFuncStorage(func));
            Refresh();
        }

        public void DrawFunction(Func<double, double, double> func)
        {
            if (func == null)
                return;

            throw new NotImplementedException();
        }

        public void ClearAllFunction()
        {
            saveFunctions.Clear();
            Refresh();
        }

        public void SetMiddlePosition(double xOffset, double yOffset)
        {
            SetMiddlePosition(new Point(xOffset, yOffset));
        }

        public void SetMiddlePosition(Point pointOffset)
        {
            middlePoint = pointOffset;
            Refresh();
        }

        public void SaveImageTo(Image target)
        {
            targetImage = target;
            SaveImageTo();
        }

        public void SaveImageTo()
        {
            if (targetImage != null)
                targetImage.Source = SaveImage();
        }

        public DrawingImage SaveImage()
        {
            return new DrawingImage(funcsCanvas);
        }

        public void IncreaseUnitNumForWidth(double inc)
        {
            UnitNumForWidth += inc;
            Refresh();
        }

        private void Refresh()
        {
            funcsCanvas = new DrawingGroup();
            using (brush = funcsCanvas.Open())
            {
                DrawCoord();
                foreach (var save in saveFunctions)
                {
                    DrawFunction((save as SingleVarFuncStorage)?.GetFunc());
                    DrawFunction((save as DoubleVarFuncStorage)?.GetFunc());
                    DrawFunction((save as ParamVarFuncStorage)?.GetFunc());
                }
                if (AutoRefresh)
                    SaveImageTo();
            }
        }

        private void DrawCoord()
        {
            (LimitRange widthLim, LimitRange heightLim) = GetDrawingArea();

            for (int i = (int)Ceiling(widthLim.from); i <= (int)Floor(widthLim.to); i++)
                brush.DrawLine(CoordPenSetting, new Point(i, heightLim.from), new Point(i, heightLim.to));
            for (int i = (int)Ceiling(heightLim.from); i <= (int)Floor(heightLim.to); i++)
                brush.DrawLine(CoordPenSetting, new Point(widthLim.from, i), new Point(widthLim.to, i));
        }
    }

    internal struct LimitRange
    {
        public double from;
        public double to;

        public LimitRange(double from, double to)
        {
            this.from = from;
            this.to = to;
        }
    }
}

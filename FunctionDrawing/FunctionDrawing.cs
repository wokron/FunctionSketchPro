using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Math;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private List<FunctionStorage> saveFunctions = new List<FunctionStorage>();
        private DrawingGroup funcsCanvas = new DrawingGroup();
        private DrawingContext brush;
        private Image targetImage = null;
        private Point visualMiddlePoint = new Point(0d, 0d);
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

        public void AddFunction(Func<double, double> func)
        {
            saveFunctions.Add(new SingleVarFuncStorage(func));
            Refresh();
        }

        private (LimitRange, LimitRange) GetDrawingArea()
        {
            double halfWidth = UnitNumForWidth / 2d;
            double height = UnitNumForWidth * AspectRatio;
            double halfHeight = height / 2d;
            LimitRange widthLim = new LimitRange(
                visualMiddlePoint.X - halfWidth,
                visualMiddlePoint.X + halfWidth);
            LimitRange heightLim = new LimitRange(
                visualMiddlePoint.Y - halfHeight,
                visualMiddlePoint.Y + halfHeight);
            return (widthLim, heightLim);
        }

        public void AddFunction(Func<double, (double, double)> func)
        {
            saveFunctions.Add(new ParamVarFuncStorage(func));
            Refresh();
        }

        public void AddFunction(Func<double, double, double> func)
        {
            saveFunctions.Add(new DoubleVarFuncStorage(func));
            Refresh();
        }

        public void ClearAllFunction()
        {
            saveFunctions.Clear();
            Refresh();
        }

        public void SetMiddlePosition(double xOffset, double yOffset)
        {
            visualMiddlePoint = new Point(xOffset, -yOffset);
            Refresh();
        }

        public void SetMiddlePosition(Point pointOffset)
        {
            visualMiddlePoint = new Point(pointOffset.X, -pointOffset.Y);
            Refresh();
        }

        public Point GetMiddlePosition()
            => new Point(visualMiddlePoint.X, -visualMiddlePoint.Y);

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

        public bool Contains(double num)
            => from <= num && num <= to;
    }
}

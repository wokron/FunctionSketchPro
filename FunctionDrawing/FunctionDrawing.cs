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
        private Point middlePoint = new Point(0d, 0d);
        private readonly double dx = 0.05;

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
            Refresh();
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

        public FunctionStorage[] GetFunctions()
        {
            return saveFunctions.ToArray();
        }

        public void RemoveFunction(FunctionStorage save)
        {
            saveFunctions.Remove(save);
        }

        public void RemoveFunctionAt(int index)
        {
            if (0 <= index && index < saveFunctions.Count)
            {
                saveFunctions.RemoveAt(index);
                Refresh();
            }
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

        public Point GetMiddlePosition()
            => middlePoint;

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
            if (UnitNumForWidth + inc > 0)
                UnitNumForWidth += inc;
            else
                UnitNumForWidth = 1;
            Refresh();
        }

        public void Refresh()
        {
            funcsCanvas = new DrawingGroup();
            using (brush = funcsCanvas.Open())
            {
                DrawCoord();
                foreach (var save in saveFunctions)
                {
                    DrawFunction(save as SingleVarFuncStorage);
                    DrawFunction(save as ParamVarFuncStorage);
                    DrawFunction(save as DoubleVarFuncStorage);
                }
                if (AutoRefresh)
                    SaveImageTo();
            }
        }

        private void DrawLineWithCoordPoints(Pen pen, Point p1, Point p2)
        {
            Matrix trans = new Matrix(1, 0, 0, -1, 0, 0);
            pen.Thickness = ScaleLength * 0.02;
            brush.DrawLine(pen, p1 * trans, p2 * trans);
        }
    }

    public struct LimitRange
    {
        public double from;
        public double to;

        public LimitRange(double from, double to)
        {
            if (from > to)
                throw new ArgumentException("from必须小于或等于to");
            this.from = from;
            this.to = to;
        }

        public double RestrictNum(double num)
            => Max(Min(to, num), from);

        public bool Contains(double num)
            => from <= num && num <= to;
    }
}

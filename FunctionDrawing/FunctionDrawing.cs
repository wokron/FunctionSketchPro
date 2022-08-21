using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Math;
using System.IO;

namespace FunctionSketch
{
    public partial class FunctionDrawing
    {
        private List<FunctionStorage> saveFunctions = new List<FunctionStorage>();
        private DrawingGroup funcsCanvas = new DrawingGroup();
        private DrawingContext brush;
        private Image targetImage = null;
        private Point middlePoint = new Point(0d, 0d);
        public double dx { get; set; } = 0.05;

        public double UnitNumForWidth { get; set; } = 20;
        public double AspectRatio { get; set; } = 412.8 / 549.92;
        public Brush FuncsPenSetting { get; set; } = Brushes.White;
        public Brush CoordPenSetting { get; set; } = Brushes.Green;
        public Brush IntegrationAreaSetting { get; set; } = Brushes.OrangeRed;
        public double FunctionLineThickness { get; set; } = 0.02;
        public double CoordLineThickness { get; set; } = 0.02;
        public bool AutoRefresh { get; set; } = true;

        public FunctionDrawing()
        {
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

        private void DrawLineWithCoordPoints(Brush color, double thickness, Point p1, Point p2)
        {
            Matrix trans = new Matrix(1, 0, 0, -1, 0, 0);
            Pen pen = new Pen(color, ScaleLength * thickness);
            brush.DrawLine(pen, p1 * trans, p2 * trans);
        }

        public void SaveImageToFile(FileStream fs)
        {
            RenderTargetBitmap render = new RenderTargetBitmap(
                    (int)(targetImage.ActualWidth + 1), (int)(targetImage.ActualHeight + 1),
                    96d, 96d, PixelFormats.Pbgra32);
            render.Render(targetImage);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(render));
            encoder.Save(fs);
        }

        public void SaveImageToFile(string imgName = "image", string path = "./SaveImage/")
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string savePath = path + imgName + ".jpg";

            using (FileStream fs = new FileStream(path + imgName + ".jpg", FileMode.Create))
            {
                SaveImageToFile(fs);
            }
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

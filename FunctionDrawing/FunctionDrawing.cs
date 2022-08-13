using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text;

namespace FunctionSketch
{
    public class FunctionDrawing
    {
        List<FunctionStorage> saveFunctions = new List<FunctionStorage>();
        DrawingGroup FuncCanvas = new DrawingGroup();
        Image targetImage = null;

        bool AutoRefresh { get; set; }
        public FunctionDrawing(FunctionStorage[] funcsStore)
        {
            AddFunction(funcsStore);
            throw new NotImplementedException();
        }

        public void AddFunction(FunctionStorage[] funcsStore)
        {
            // 此处foreach，对不同函数采取不同绘制策略（用其他多态函数）
            throw new NotImplementedException();
        }

        public void AddFunction(Func<double, double> func)
        {
            Refresh();
            throw new NotImplementedException();
        }

        public void AddFunction(Func<double, (double, double)> func)
        {
            Refresh();
            throw new NotImplementedException();
        }

        public void AddFunction(Func<double, double, double> func)
        {
            Refresh();
            throw new NotImplementedException();
        }

        public void ClearAllFunction()
        {
            Refresh();
            throw new NotImplementedException();
        }

        public void SetOriginPosition(double xOffset, double yOffset)
        {
            Refresh();
            throw new NotImplementedException();
        }

        public void SetOriginPosition(Point pointOffset)
        {
            Refresh();
            throw new NotImplementedException();
        }

        public void SaveImageTo(Image target)
        {
            targetImage = target;
            throw new NotImplementedException();
        }

        public void SaveImageTo()
        {
            // 使用上一次的target
            throw new NotImplementedException();
        }

        public DrawingImage SaveImage()
        {
            throw new NotImplementedException();
        }

        public void IncreaseUnitLength(double inc)
        {
            Refresh();
            throw new NotImplementedException();
        }

        public void SetUnitLength(double length)
        {
            Refresh();
            throw new NotImplementedException();
        }

        private void Refresh()
        {
            if (AutoRefresh)
            {
                SaveImageTo();
            }
            throw new NotImplementedException();
        }
    }
}

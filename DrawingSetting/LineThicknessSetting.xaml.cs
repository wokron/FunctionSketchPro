using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FunctionSketch;

namespace 函数画板
{
    /// <summary>
    /// LineSticknessSetting.xaml 的交互逻辑
    /// </summary>
    public partial class LineThicknessSetting : Page
    {
        private FunctionDrawing drawing;
        public LineThicknessSetting()
        {
            InitializeComponent();
        }

        public LineThicknessSetting(FunctionDrawing drawing) : this()
        {
            this.drawing = drawing;
            ShowOldSettings();
        }

        private void ShowOldSettings()
        {
            funcThicknessGetter.Text = drawing.FunctionLineThickness.ToString();
            coordTicknessGetter.Text = drawing.CoordLineThickness.ToString();
        }

        public (double, double) GetSettings()
        {
            return (Convert.ToDouble(funcThicknessGetter.Text),
                Convert.ToDouble(coordTicknessGetter.Text));
        }
    }
}

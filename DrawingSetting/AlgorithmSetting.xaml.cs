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
using static System.Convert;

namespace 函数画板
{
    /// <summary>
    /// AlgorithmSetting.xaml 的交互逻辑
    /// </summary>
    public partial class AlgorithmSetting : Page
    {
        private FunctionDrawing drawing;
        public AlgorithmSetting()
        {
            InitializeComponent();
        }

        public AlgorithmSetting(FunctionDrawing drawing) : this()
        {
            this.drawing = drawing;
            ShowOldSettings();
        }

        private void ShowOldSettings()
        {
            dxGetter.Text = drawing.dx.ToString();
            smoothRateGetter.Text = drawing.SmoothRate.ToString();
            maxRecureGetter.Text = drawing.maxRecur.ToString();
            partialRateGetter.Text = drawing.PartialRate.ToString();
        }

        public (double, double, int, double) GetSettings()
        {
            return (ToDouble(dxGetter.Text),
                ToDouble(smoothRateGetter.Text),
                ToInt32(maxRecureGetter.Text),
                ToDouble(partialRateGetter.Text));
        }
    }
}

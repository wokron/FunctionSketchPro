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
    /// LineColorSetting.xaml 的交互逻辑
    /// </summary>
    public partial class LineColorSetting : Page
    {
        private FunctionDrawing drawing;
        public LineColorSetting()
        {
            InitializeComponent();
        }

        public LineColorSetting(FunctionDrawing drawing) : this()
        {
            this.drawing = drawing;
            ShowOldSettings();
        }

        private void ShowOldSettings()
        {
            funcColorGetter.Text = drawing.FuncsPenSetting.ToString();
            coordColorGetter.Text = drawing.CoordPenSetting.ToString();
            integrateColorGetter.Text = drawing.IntegrationAreaSetting.ToString();
        }

        public (string, string, string) GetSettings()
        {
            return (funcColorGetter.Text, coordColorGetter.Text, integrateColorGetter.Text);
        }
    }
}

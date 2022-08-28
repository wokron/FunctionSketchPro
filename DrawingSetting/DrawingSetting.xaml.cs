using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FunctionSketch;

namespace 函数画板
{
    /// <summary>
    /// DrawingSetting.xaml 的交互逻辑
    /// </summary>
    public partial class DrawingSetting : Window
    {
        private FunctionDrawing drawing;
        Page[] settings;

        public DrawingSetting()
        {
            InitializeComponent();
        }

        public DrawingSetting(FunctionDrawing drawing) : this()
        {
            this.drawing = drawing;
            settings = new Page[]
            {
                new RangeSetting(drawing),
                new LineColorSetting(drawing),
                new LineThicknessSetting(drawing),
                new AlgorithmSetting(drawing)
            };
            InitFrame();
        }

        int addIndex = 3;
        private void InitFrame()
        {
            mainFrame.Navigate(settings[addIndex]);
            mainFrame.LoadCompleted += MainFrame_LoadCompleted;
        }

        private void MainFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            addIndex--;
            if (addIndex < 0)
            {
                mainFrame.LoadCompleted -= MainFrame_LoadCompleted;
                return;
            }
            mainFrame.Navigate(settings[addIndex]);
        }

        private void SetShowArea(object sender, RoutedEventArgs e)
        {
            ChooseThisOne(sender as ToggleButton);
            mainFrame.Navigate(settings[0]);
        }

        private void SetLineColor(object sender, RoutedEventArgs e)
        {
            ChooseThisOne(sender as ToggleButton);
            mainFrame.Navigate(settings[1]);
        }

        private void SetLineStickness(object sender, RoutedEventArgs e)
        {
            ChooseThisOne(sender as ToggleButton);
            mainFrame.Navigate(settings[2]);
        }

        private void SetAlgorithm(object sender, RoutedEventArgs e)
        {
            ChooseThisOne(sender as ToggleButton);
            mainFrame.Navigate(settings[3]);
        }

        private void ChooseThisOne(ToggleButton button)
        {
            ClearAllChoose();
            button.IsChecked = true;
        }

        private void ClearAllChoose()
        {
            foreach (var child in selectPnl.Children)
            {
                ((ToggleButton)child).IsChecked = false;
            }
        }

        private void ApplySetting(object sender, RoutedEventArgs e)
        {
            ApplyRangeSetting();
            ApplyLineColorSetting();
            ApplyLineThicknessSetting();
            ApplyAlgorithmSetting();
            drawing.Refresh();
        }

        private void ApplyRangeSetting()
        {
            (Point p, double unitLen) = (settings[0] as RangeSetting).GetSettings();
            drawing.SetMiddlePosition(p);
            drawing.UnitNumForWidth = unitLen;
        }

        private void ApplyLineColorSetting()
        {
            (string s1, string s2, string s3) = (settings[1] as LineColorSetting).GetSettings();
            drawing.DefaultFuncsPenBrush = GetBrush(s1);
            drawing.CoordPenBrush = GetBrush(s2);
            drawing.IntegrationAreaBrush = GetBrush(s3);
        }

        private void ApplyLineThicknessSetting()
        {
            (double forFunc, double forCoord) = (settings[2] as LineThicknessSetting).GetSettings();
            drawing.FunctionLineThickness = forFunc;
            drawing.CoordLineThickness = forCoord;
        }

        private void ApplyAlgorithmSetting()
        {
            (double dx, double smoothRate, int maxRecur, double partialRate)
                = (settings[3] as AlgorithmSetting).GetSettings();
            drawing.dx = dx;
            drawing.SmoothRate = smoothRate;
            drawing.maxRecur = maxRecur;
            drawing.PartialRate = partialRate;
        }

        private Brush GetBrush(string s) => new SolidColorBrush((Color)ColorConverter.ConvertFromString(s));

        private void DisApplySetting(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

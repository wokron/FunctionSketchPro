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
                new LineThicknessSetting(drawing)
            };
            InitFrame(null, null);
        }

        int addIndex = 2;
        private void InitFrame(object sender, RoutedEventArgs e)
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
            drawing.Refresh();
        }

        private void ApplyRangeSetting()
        {
            (string str1, string str2) = (settings[0] as RangeSetting).GetSettings();
            string[] xy = str1.Split(',');
            drawing.SetMiddlePosition(Convert.ToDouble(xy[0]), Convert.ToDouble(xy[1]));
            drawing.UnitNumForWidth = Convert.ToDouble(str2);
        }

        private void ApplyLineColorSetting()
        {
            (string s1, string s2, string s3) = (settings[1] as LineColorSetting).GetSettings();
            drawing.FuncsPenSetting = GetBrush(s1);
            drawing.CoordPenSetting = GetBrush(s2);
            drawing.IntegrationAreaSetting = GetBrush(s3);
        }

        private void ApplyLineThicknessSetting()
        {
            (string s1, string s2) = (settings[2] as LineThicknessSetting).GetSettings();
            drawing.FunctionLineThickness = Convert.ToDouble(s1);
            drawing.CoordLineThickness = Convert.ToDouble(s2);
        }

        private Brush GetBrush(string s) => new SolidColorBrush((Color)ColorConverter.ConvertFromString(s));

        private void DisApplySetting(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

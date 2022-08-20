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
        public DrawingSetting()
        {
            InitializeComponent();
        }

        public DrawingSetting(FunctionDrawing drawing) : this()
        {
            this.drawing = drawing;
        }

        private void SetShowArea(object sender, RoutedEventArgs e)
        {
            ChooseThisOne(sender as ToggleButton);
            mainFrame.Content = new RangeSetting();
        }

        private void SetLineColor(object sender, RoutedEventArgs e)
        {
            ChooseThisOne(sender as ToggleButton);
        }

        private void SetLineStickness(object sender, RoutedEventArgs e)
        {
            ChooseThisOne(sender as ToggleButton);
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
    }
}

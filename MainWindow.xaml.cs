using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using FunctionSketch;
using static System.Math;

namespace 函数画板
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            test();
        }

        FunctionDrawing fd = new FunctionDrawing();

        private void test()
        {
            //fd.AutoRefresh = false;
            //fd.SetMiddlePosition(10, 0);
            //fd.AddFunction(x => Pow(E, -x)*Sin(20*x));
            //fd.AddFunction(x => Sin(x));
            //fd.AddFunction(x => x * x);
            fd.AddFunction(x => Sin(100d/(x)));
            fd.AddFunction(x => Tan(x));
            fd.SaveImageTo(img);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fd.IncreaseUnitNumForWidth(-5);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            img.Source = fd.SaveImage();
        }
    }
}

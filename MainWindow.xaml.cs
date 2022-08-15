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
            //fd.SetMiddlePosition(5, 5);
            //fd.AutoRefresh = false;
            //fd.SetMiddlePosition(10, 0);
            //fd.AddFunction(x => Pow(E, -x)*Sin(20*x));
            //fd.AddFunction(x => Sin(x));
            //fd.AddFunction(x => x * x);
            //fd.AddFunction(x => Sin(100d/(x)));
            //fd.AddFunction(x => Tan(x));
            //fd.AddFunction(x => (Sin(x), Cos(x)));
            //fd.AddFunction(x => (x, Sin(x)));
            //fd.AddFunction(x => (x, Sin(100d / (x))));
            //fd.AddFunction(x => (x, Tan(x)));
            FunctionFactory ff = new FunctionFactory("sinx=y");
            fd.AddFunction(ff.GetFunctions());
            fd.SaveImageTo(img);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Point p = fd.GetMiddlePosition();
            fd.SetMiddlePosition(p.X+1, p.Y);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            fd.ClearAllFunction();
        }


        bool isDown = false;
        Point oriMiddle;
        Point mouseDownp;
        int detectCnt = 0;
        private void img_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                detectCnt++;
                result.Content = $"探测次数：{detectCnt}";
                Point nowp = e.GetPosition(img);
                Vector move = nowp - mouseDownp;
                Vector coordMove = new Vector(-move.X/100d, move.Y/100d);
                fd.SetMiddlePosition(oriMiddle + coordMove);
            }
            
        }

        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDown = true;
            oriMiddle = fd.GetMiddlePosition();
            mouseDownp = e.GetPosition(img);
        }

        private void img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDown = false;
        }
    }
}

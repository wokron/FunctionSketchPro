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
            Test();
        }

        FunctionDrawing fd = new FunctionDrawing();
        FunctionStorage fs;
        private void Test()
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
            //fd.IsPolarPlot = true;
            //FunctionFactory ff = new FunctionFactory("(y^4)-(y^2)+(x+0.5)^2=0");
            FunctionFactory ff = new FunctionFactory("y=sinx");
            fs = ff.GetFunctions()[0];
            //if (fs is ParamVarFuncStorage param)
                //param.SetRange(100);
            //fd.AddFunction(fs);
            //(fs as SingleVarFuncStorage).Transform = new Matrix(1.732/2, -0.5, 0.5, 1.732 / 2, 2, 2);
            fd.AddFunction(ff.GetFunctions());
            foreach (var item in fd.GetFunctions())
            {
                Label label = new Label() { Content = item.ToString() };
                pnl.Children.Add(label);
            } 
            //fd.AddFunction(x => (Sqrt(Cos(x)) * Cos(200 * x) + Sqrt(Abs(x)) - 0.7) * Pow(4 - x * x, 0.01));
            //fd.AddFunction(x => (2*Sin(x), 2*Cos(x)));
            //fd.AddFunction((x, y) => y * y * y + Pow(3, x) - x * x * x - Pow(3, y));
            fd.SaveImageTo(img);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            now.Content = (fs as SingleVarFuncStorage).GetIntegration(new LimitRange(0.5, 2));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            fd.RemoveFunctionAt(0);
        }


        bool isDown = false;
        Point oriMiddle;
        Point mouseDownp;
        int detectCnt = 0;
        private void Img_MouseMove(object sender, MouseEventArgs e)
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

        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDown = true;
            oriMiddle = fd.GetMiddlePosition();
            mouseDownp = e.GetPosition(img);
        }

        private void Img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDown = false;
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                fd.IncreaseUnitNumForWidth(-1);
            else if (e.Delta < 0)
                fd.IncreaseUnitNumForWidth(1);
        }
    }
}

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
using FunctionSketch;

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

        FunctionStorage fs;
        private void Test()
        {
            FunctionFactory ff = new FunctionFactory("y=arctanx");
            fs = ff.GetFunctions()[0];
            fd.AddFunction(fs);
            //fd.AddFunction(x => (Sqrt(Cos(x)) * Cos(200 * x) + Sqrt(Abs(x)) - 0.7) * Pow(4 - x * x, 0.01));
            //fd.AddFunction(x => (2*Sin(x), 2*Cos(x)));
            //fd.AddFunction((x, y) => y * y * y + Pow(3, x) - x * x * x - Pow(3, y));
            fd.SaveImageTo(img);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FunctionStorage tmp = (fs as SingleVarFuncStorage).GetDerivativeFunctionStorage();
            fd.AddFunction(tmp);
            fs = tmp;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            fd.ClearAllFunction();
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

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
using Microsoft.Win32;
using System.IO;

namespace 函数画板
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FunctionDrawing drawing = new FunctionDrawing();

        public MainWindow()
        {
            InitializeComponent();
            DrawingInit();
        }

        private void DrawingInit()
        {
            drawing.SaveImageTo(img);
            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void GetFunctionEvent(object sender, EventArgs e)
        {
            GetFuncEventArgs args = (GetFuncEventArgs)e;
            drawing.AddFunction(args.GetFuncs());
            foreach (var func in args.GetFuncs())
            {
                var show = new FunctionShowing(func);
                show.RefreshEvent = (s, e) => drawing.Refresh();
                show.DeleteEvent = (s, e) => { drawing.RemoveFunction(func); pnl.Children.Remove(show); };
                show.CreateNewEvent = GetFunctionEvent;
                pnl.Children.Insert(pnl.Children.Count - 1, show);
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            drawing.AspectRatio = measureGrid.ActualHeight / imageGrid.ActualWidth;
            drawing.Refresh();
        }

        private bool isDown = false;
        private Point oriMiddle;
        private Point mouseDownp;
        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                Point nowp = e.GetPosition(img);
                Vector move = nowp - mouseDownp;
                Vector coordMove = new Vector(-move.X / 100d, move.Y / 100d);
                drawing.SetMiddlePosition(oriMiddle + coordMove);
            }
            
        }

        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDown = true;
            oriMiddle = drawing.GetMiddlePosition();
            mouseDownp = e.GetPosition(img);
        }

        private void Img_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDown = false;
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                drawing.IncreaseUnitNumForWidth(-1);
            else if (e.Delta < 0)
                drawing.IncreaseUnitNumForWidth(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            drawing.SaveImageToFile();
        }

        private void SourceCodeClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", "https://github.com/wokron/FunctionSketchPro");
        }

        private void SaveImageClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("./SaveImage/"))
                Directory.CreateDirectory("./SaveImage/");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPeg Image|*.jpg";
            saveFileDialog.Title = "Save an Image File";
            saveFileDialog.FileName = "FunctionImage";
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "SaveImage";
           
            if ((bool)saveFileDialog.ShowDialog() && saveFileDialog.FileName != "")
            {
                using (FileStream fs = (FileStream)saveFileDialog.OpenFile())
                {
                    drawing.SaveImageToFile(fs);
                }
            }
        }

        private void CheckDefaultSavePath(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", ".\\SaveImage");
        }

        private void ShowHelper(object sender, RoutedEventArgs e)
        {
            var helper = new Helper();
            helper.Show();
        }
    }
}

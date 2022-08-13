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

        private void test()
        {
            FunctionFactory ff = new FunctionFactory("0.5x,e^(0-0.5*x)*sin(20*0.5*x)");
            var fs = (ParamVarFuncStorage)ff.GetFunctions()[0];
            var func = fs.GetFunc();
            List<Point> savep = new List<Point>();
            for (double i = 0; i <= 10; i += 0.05)
            {
                (double, double) p = func(i);
                savep.Add(new Point(p.Item1, p.Item2));
            }
            Pen pen = new Pen(Brushes.Black, 0.01);
            DrawingGroup drawingGroup = new DrawingGroup();
            using (DrawingContext paint = drawingGroup.Open())
            {
                for (int i = 1; i < savep.Count; i++)
                {
                    paint.DrawLine(pen, savep[i - 1], savep[i]);
                }
            }
            img.Source = new DrawingImage(drawingGroup);
            
        }
    }
}

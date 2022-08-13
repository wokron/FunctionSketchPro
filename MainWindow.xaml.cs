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
            FunctionFactory ff = new FunctionFactory("2;y=2;x^2;x,x;y^2=x");
            FunctionStorage[] fs = ff.GetFunctions();
            foreach (var item in fs)
            {
                Label label = new Label();
                if (item is DoubleVarFuncStorage)
                    label.Content = "double" + ((DoubleVarFuncStorage)item).GetFunc().Invoke(1, 2);
                else if (item is SingleVarFuncStorage)
                    label.Content = "single" + ((SingleVarFuncStorage)item).GetFunc().Invoke(3);
                else if (item is ParamVarFuncStorage)
                    label.Content = "else" + ((ParamVarFuncStorage)item).GetFunc().Invoke(3);
                pnl.Children.Add(label);
            }
        }
    }
}

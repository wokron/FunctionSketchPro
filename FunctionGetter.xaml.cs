using System;
using System.Collections.Generic;
using System.Text;
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
    /// FunctionGetter.xaml 的交互逻辑
    /// </summary>
    public partial class FunctionGetter : UserControl
    {
        public EventHandler AddFunctionEvent { get; set; }
        private TextBox funcText;

        private Button button;
        public FunctionGetter()
        {
            InitializeComponent();
        }

        private void AddFunctionClick(object sender, RoutedEventArgs e)
        {
            button = (Button)pnl.Children[0];
            pnl.Children.Clear();
            pnl.Children.Add(new Label { Content = "输入函数：" });
            funcText = new TextBox();
            pnl.Children.Add(funcText);
            funcText.LostFocus += FuncText_LostFocus;
        }

        private void FuncText_LostFocus(object sender, RoutedEventArgs e)
        {
            string input = funcText.Text;
            try
            {
                FunctionFactory ff = new FunctionFactory(input);
                AddFunctionEvent?.Invoke(sender, new GetFuncEventArgs(ff.GetFunctions()));
            }
            catch (Exception)
            {
                // 输入错误的表达式，不进行处理
            }
            finally
            {
                pnl.Children.Clear();
                pnl.Children.Add(button);
            }
            
        }
    }

    public class GetFuncEventArgs : EventArgs
    {
        private FunctionStorage[] saveFuncs;
        public GetFuncEventArgs(FunctionStorage[] save)
        {
            saveFuncs = save;
        }

        public FunctionStorage[] GetFuncs() => saveFuncs;
    }
}

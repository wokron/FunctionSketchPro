﻿using System;
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
            SetFuncText();
        }

        private Brush tipBrush = 
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8C8C8C"));

        private void SetFuncText()
        {
            funcText = new TextBox
            {
                Foreground = tipBrush,
                Text = "请输入函数(以分号分隔)",
                BorderBrush = Brushes.Gray,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")),
                MinHeight = 50,
                TextWrapping = TextWrapping.Wrap
            };
            funcText.LostFocus += FuncText_LostFocus;
            funcText.GotFocus += FuncText_GotFocus;
            funcText.KeyDown += FuncText_KeyDown;
            pnl.Children.Add(funcText);
        }

        private void FuncText_GotFocus(object sender, RoutedEventArgs e)
        {
            funcText.Foreground = Brushes.White;
            funcText.Text = "";
        }

        private void FuncText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pnl.Children.Clear();
                pnl.Children.Add(button);
            }
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

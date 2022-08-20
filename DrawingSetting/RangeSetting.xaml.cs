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
    /// RangeSetting.xaml 的交互逻辑
    /// </summary>
    public partial class RangeSetting : Page
    {
        private FunctionDrawing drawing;
        public RangeSetting()
        {
            InitializeComponent();
        }

        public RangeSetting(FunctionDrawing drawing) : this()
        {
            this.drawing = drawing;
            ShowOldSettings();
        }

        private void ShowOldSettings()
        {
            positionGetter.Text = drawing.GetMiddlePosition().ToString();
            unitLengthGetter.Text = drawing.UnitNumForWidth.ToString();
        }

        public (string, string) GetSettings()
        {
            return (positionGetter.Text, unitLengthGetter.Text);
        }
    }
}

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
    /// SettingForDoubleFunc.xaml 的交互逻辑
    /// </summary>
    public partial class SettingForDoubleFunc : UserControl
    {
        DoubleVarFuncStorage save;
        EventHandler e;
        public SettingForDoubleFunc()
        {
            InitializeComponent();
        }

        public SettingForDoubleFunc(DoubleVarFuncStorage save, EventHandler e) : this()
        {
            this.save = save;
            this.e = e;
            ShowSetting();
        }

        private void ShowSetting()
        {
            colorGetter.Text = save.FuncColor == null ? "default" : $"{save.FuncColor}";
        }

        private void ApplySetting(object sender, RoutedEventArgs e)
        {
            var tmpSaveColor = save.FuncColor;
            try
            {
                if (colorGetter.Text == "default")
                    save.FuncColor = null;
                else
                    save.FuncColor = (Color)ColorConverter.ConvertFromString(colorGetter.Text);
            }
            catch (Exception)
            {
                save.FuncColor = tmpSaveColor;
            }

            this.e.Invoke(sender, e);
            ShowSetting();
        }
    }
}

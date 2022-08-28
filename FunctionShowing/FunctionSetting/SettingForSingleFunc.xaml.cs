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
    /// SettingForSingleFunc.xaml 的交互逻辑
    /// </summary>
    public partial class SettingForSingleFunc : UserControl
    {
        SingleVarFuncStorage save;
        EventHandler e;

        public EventHandler DerivationEvent { get; set; }
        public SettingForSingleFunc()
        {
            InitializeComponent();
        }

        public SettingForSingleFunc(SingleVarFuncStorage save, EventHandler e) : this()
        {
            this.save = save;
            this.e = e;
            ShowSetting();
        }

        private bool settingIsPolar;

        private void ShowSetting()
        {
            swapGetter.Content = save.IsPolarPlot ? "极坐标" : "直角坐标";
            settingIsPolar = save.IsPolarPlot;
            var trans = save.Transform;
            linearGetter.Text = $"[[{trans.M11:N2},{trans.M12:N2}],[{trans.M21:N2},{trans.M22:N2}]]";
            moveGetter.Text = $"({trans.OffsetX:N2},{trans.OffsetY:N2})";
            LimitRange range = save.HasIntegration() ? save.GetInterationRange() : new LimitRange(double.NaN, double.NaN);
            integrationGetter.Text = $"{range.from:N2},{range.to:N2}";
            colorGetter.Text = save.FuncColor == null ? "default" : $"{save.FuncColor}";
        }

        private void SwapGetter_Click(object sender, RoutedEventArgs e)
        {
            settingIsPolar = !settingIsPolar;
            swapGetter.Content = settingIsPolar ? "极坐标" : "直角坐标";
        }

        private void ApplySetting(object sender, RoutedEventArgs e)
        {
            save.IsPolarPlot = settingIsPolar;

            double[] values = TransformToValues(linearGetter.Text, 4);
            double[] values2 = TransformToValues(moveGetter.Text, 2);
            save.Transform = new Matrix(
                values[0], values[1],
                values[2], values[3],
                values2[0], values2[1]);

            double[] values3 = TransformToValues(integrationGetter.Text, 2);
            LimitRange range;
            try
            {
                range = new LimitRange(values3[0], values3[1]); 
            }
            catch (Exception) // 出现from > to的情况
            {
                range = save.GetInterationRange(); // 不改变原范围
            }
            save.GetIntegration(range);

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

        private double[] TransformToValues(string str, int maxCnt)
        {
            var strValues = str.Split('[', ']', ',', '(', ')');
            double[] values = new double[maxCnt];
            for (int i = 0, j = 0; i < maxCnt && j < strValues.Length; j++)
            {
                if (strValues[j] == string.Empty)
                    continue;
                try
                {
                    values[i] = Convert.ToDouble(strValues[j]);
                }
                catch (Exception)
                {
                    values[i] = 0;
                    // 输入了非数字，只需要置零
                }
                finally
                {
                    i++;
                }
            }
            return values;
        }

        private void DerivationClick(object sender, RoutedEventArgs e)
        {
            DerivationEvent?.Invoke(sender, e);
        }
    }
}

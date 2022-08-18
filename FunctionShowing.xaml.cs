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
    /// FunctionShowing.xaml 的交互逻辑
    /// </summary>
    public partial class FunctionShowing : UserControl
    {
        public FunctionShowing()
        {
            InitializeComponent();
            //Pop.HorizontalOffset = method.Width;
        }

        public FunctionShowing(FunctionStorage save) : this()
        {
            pnl.Children.Add(new Label() { Content = save });
            if (save is AbstractParamFuncStorage abstractParam)
            {
                pnl.Children.Add(
                    new Label(){
                        Content = abstractParam.IsPolarPlot ? "极坐标" : "直角坐标"
                    });
                Matrix trans = abstractParam.Transform;
                pnl.Children.Add(
                    new Label()
                    {
                        Content = $"线性变换:[" +
                        $"[{trans.M11},{trans.M12}]," +
                        $"[{trans.M21},{trans.M22}]]"
                    });
                pnl.Children.Add(new Label() { Content = $"位移:({trans.OffsetX},{trans.OffsetY})" });

                if (abstractParam is SingleVarFuncStorage singleVar
                    && singleVar.HasIntegration())
                {
                    LimitRange range = singleVar.GetInterationRange();
                    pnl.Children.Add(
                        new Label(){
                            Content = $"积分{range.from:N2}->{range.to:N2}=" +
                            $"{singleVar.GetIntegration():N2}"
                        });
                }
                else
                {
                    LimitRange range = (abstractParam as ParamVarFuncStorage).GetRange();
                    pnl.Children.Add(new Label() { Content = $"系数范围:{range.from:N2}->{range.to:N2}" });
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pop.IsOpen = true;
        }
    }
}

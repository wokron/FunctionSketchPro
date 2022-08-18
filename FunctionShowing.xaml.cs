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
        private FunctionStorage save;
        private EventHandler returnEvent;
        private EventHandler deleteEvent;
        public FunctionShowing()
        {
            InitializeComponent();
        }

        public FunctionShowing(
            FunctionStorage save,
            EventHandler e = null,
            EventHandler delete = null) : this()
        {
            this.save = save;
            returnEvent = e;
            deleteEvent = delete;
            SetFunctionInfo(save);
        }

        private void SetFunctionInfo(FunctionStorage save)
        {
            SetFunction(save);
            if (save is AbstractParamFuncStorage abstractParam)
            {
                SetCoordType(abstractParam);
                SetTransform(abstractParam);

                if (abstractParam is SingleVarFuncStorage singleVar
                    && singleVar.HasIntegration())
                {
                    SetInteration(singleVar);
                    AddSingleVarSetting(singleVar);
                }
                else
                    SetParamRange(abstractParam as ParamVarFuncStorage);
            }
        }

        private void SetFunction(FunctionStorage save)
            => AddPanelContent(pnl, save.ToString());

        private void SetCoordType(AbstractParamFuncStorage abstractParam)
            => AddPanelContent(pnl, abstractParam.IsPolarPlot ? "极坐标" : "直角坐标");

        private void SetTransform(AbstractParamFuncStorage abstractParam)
        {
            Matrix trans = abstractParam.Transform;
            AddPanelContent(pnl, $"线性变换:[[{trans.M11},{trans.M12}],[{trans.M21},{trans.M22}]]");
            AddPanelContent(pnl, $"位移:({trans.OffsetX},{trans.OffsetY})");
        }

        private void SetInteration(SingleVarFuncStorage singleVar)
        {
            LimitRange range = singleVar.GetInterationRange();
            AddPanelContent(pnl, $"积分{range.from:N2}->{range.to:N2}={singleVar.GetIntegration():N2}");
        }

        private void SetParamRange(ParamVarFuncStorage param)
        {
            LimitRange range = param.GetRange();
            AddPanelContent(pnl, $"自变量范围:{range.from:N2}->{range.to:N2}");
        }

        private void AddPanelContent(StackPanel panel, string content)
        {
            panel.Children.Add(new Label() { Content = content });
        }

        private void AddSingleVarSetting(SingleVarFuncStorage save)
        {
            popPnl.Children.Add(new SettingForSingleFunc(save, returnEvent));
        }

        private void ShowPopSetting(object sender, RoutedEventArgs e)
        {
            Pop.IsOpen = true;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            deleteEvent.Invoke(sender, e);
        }
    }
}

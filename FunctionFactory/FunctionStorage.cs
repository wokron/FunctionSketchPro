using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace FunctionSketch
{
    public abstract class FunctionStorage
    {
        protected ExpressionElement[] expressionTree;

        public override string ToString()
        {
            string rt = string.Empty;
            foreach (var exp in expressionTree)
                rt += exp + ";";
            return rt;
        }
    }

    public class DoubleVarFuncStorage : FunctionStorage
    {
        public DoubleVarFuncStorage(ExpressionElement expression)
        {
            expressionTree = new ExpressionElement[] { expression };
        }

        public Func<double, double, double> GetFunc()
            => expressionTree[0].Calculate;

        public override string ToString()
        {
            return $"f(x,y)={expressionTree[0]}=0";
        }
    }

    public abstract class AbstractParamFuncStorage : FunctionStorage
    {
        private LimitRange range = new LimitRange(0, 2 * Math.PI);
        public Matrix Transform { get; set; }

        public bool IsPolarPlot { get; set; }

        public AbstractParamFuncStorage() { Transform = new Matrix(); }

        public LimitRange GetRange()
        {
            return range;
        }

        public void SetRange(double end)
        {
            range.to = end;
        }

        public void SetRange(double start, double end)
        {
            range.from = start;
            range.to = end;
        }
    }

    public class SingleVarFuncStorage : AbstractParamFuncStorage
    {
        private IntegrationHelper integration = null;
        public SingleVarFuncStorage(ExpressionElement expression)
        {
            expressionTree = new ExpressionElement[] { expression };
        }

        public SingleVarFuncStorage GetDerivativeFunctionStorage()
        {
            var rt = new SingleVarFuncStorage(expressionTree[0].Derivative());
            return rt;
        }

        public double GetIntegration(LimitRange range)
        {
            integration = new IntegrationHelper(GetFunc(), range);
            return integration.Answer;
        }

        public double GetIntegration()
        {
            if (integration == null)
                throw new InvalidOperationException("该函数未设置积分范围，请事先检查是否已经设置积分");
            return integration.Answer;
        }

        public LimitRange GetInterationRange()
        {
            if (integration == null)
                throw new InvalidOperationException("该函数未设置积分范围，请事先检查是否已经设置积分");
            return integration.Range;
        }

        public void DeleteIntegration()
            => integration = null;

        public bool HasIntegration() 
            => integration != null;

        public Func<double, double> GetFunc()
            => expressionTree[0].Calculate;

        public override string ToString()
        {
            return $"f(x)={expressionTree[0]}";
        }

        public ParamVarFuncStorage ConvertToParamFunc()
        {
            var rt = new ParamVarFuncStorage(new ArgumentX(), expressionTree[0])
            {
                IsPolarPlot = this.IsPolarPlot,
                Transform = this.Transform
            };
            return rt;
        }
    }

    public class ParamVarFuncStorage : AbstractParamFuncStorage
    {
        public ParamVarFuncStorage(ExpressionElement express1, ExpressionElement express2)
        {
            expressionTree = new ExpressionElement[] { express1, express2 };
        }

        public Func<double, (double, double)> GetFunc()
            => new ParamFuncHelper(
                expressionTree[0].Calculate,
                expressionTree[1].Calculate).Calculate;

        public override string ToString()
        {
            return $"x={expressionTree[0]},y={expressionTree[1]}".Replace('x', 't');
        }
    }

    public class IntegrationHelper
    {
        private Func<double, double> func;
        private double dx = 0.005;
        private double ans;
        public LimitRange Range { get; }
        public double Answer { get => ans; }

        public IntegrationHelper(Func<double, double> func, LimitRange range)
        {
            Range = range;
            this.func = func;
            ans = 0;
            DoIntegration();
        }

        private void DoIntegration()
        {
            double preY = func(Range.from);
            for (double i = Range.from + dx; i <= Range.to; i += dx)
            {
                double nowY = func(i);
                ans += dx * (preY + nowY) / 2d;
                preY = nowY;
            }
        }
    }
}

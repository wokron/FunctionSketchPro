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
        public SingleVarFuncStorage(ExpressionElement expression)
        {
            expressionTree = new ExpressionElement[] { expression };
        }

        public SingleVarFuncStorage GetDerivativeFunctionStorage()
        {
            var rt = new SingleVarFuncStorage(expressionTree[0].Derivative());
            return rt;
        }

        public Func<double, double> GetFunc()
            => expressionTree[0].Calculate;

        public override string ToString()
        {
            return $"f(x)={expressionTree[0]}";
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
            return $"x={expressionTree[0]},y={expressionTree[1]}";
        }
    }
}

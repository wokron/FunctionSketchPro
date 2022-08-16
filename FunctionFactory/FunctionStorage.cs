using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionSketch
{
    public abstract class FunctionStorage
    {
        protected ExpressionElement[] expressionTree;
    }

    public class SingleVarFuncStorage : FunctionStorage
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
    }

    public class DoubleVarFuncStorage : FunctionStorage
    {
        public DoubleVarFuncStorage(ExpressionElement expression)
        {
            expressionTree = new ExpressionElement[] { expression };
        }

        public Func<double, double, double> GetFunc()
            => expressionTree[0].Calculate;
    }

    public class ParamVarFuncStorage : FunctionStorage
    {
        public ParamVarFuncStorage(ExpressionElement express1, ExpressionElement express2)
        {
            expressionTree = new ExpressionElement[] { express1, express2 };
        }

        public Func<double, (double, double)> GetFunc()
            => new ParamFuncHelper(
                expressionTree[0].Calculate,
                expressionTree[1].Calculate).Calculate;
    }
}

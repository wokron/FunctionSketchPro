using System;
using System.Collections.Generic;
using System.Text;
using FunctionSketch;

namespace FunctionSketch
{
    public class FunctionFactory
    {
        List<FunctionStorage> saveFunc = new List<FunctionStorage>();

        public FunctionFactory(string funcsExp)
        {
            string[] funcs = funcsExp.ToUpper().Split(';');

            foreach (var func in funcs)
            {
                if (isParamFunction(func))
                    saveFunc.Add(new ParamVarFuncStorage(parseParamFunc(func)));
                else if (isSingleFunction(func))
                    saveFunc.Add(new SingleVarFuncStorage(parseSingleFunc(func)));
                else if (isDoubleFunction(func))
                    saveFunc.Add(new DoubleVarFuncStorage(parseDoubleFunc(func)));
                else
                    throw new ArgumentException("表达式无法转换成函数");
            }
        }

        private bool isDoubleFunction(string func)
            => func.Contains('X') && func.Contains('Y') && func.Contains('=');

        private Func<double, double, double> parseDoubleFunc(string func)
        {
            func = func.Replace('=', '-');
            var fp = new FunctionParser(func);
            fp.ParseExpression();
            return fp.GetExpressionTree().Calculate;
        }

        private bool isSingleFunction(string func)
            => !func.Contains('Y') || (func[0] == 'Y' && func[1] == '=');

        private Func<double, double> parseSingleFunc(string func)
        {
            if (func[0] == 'Y' && func[1] == '=')
                func = func.Substring(2);
            var fp = new FunctionParser(func);
            fp.ParseExpression();
            return fp.GetExpressionTree().Calculate;
        }

        private bool isParamFunction(string func)
            => func.Contains(',');

        private Func<double, (double, double)> parseParamFunc(string func)
        {
            string[] funcs = func.Split(',');
            if (isSingleFunction(funcs[0]) && isSingleFunction(funcs[1]))
            {
                var p1 = new FunctionParser(funcs[0]);
                p1.ParseExpression();
                var p2 = new FunctionParser(funcs[1]);
                p2.ParseExpression();
                var helper = new ParamFuncHelper(
                    p1.GetExpressionTree().Calculate,
                    p2.GetExpressionTree().Calculate);
                return helper.Calculate;
            }
            else
                throw new ArgumentException("表达式无法转换成参数函数");
        }

        public FunctionStorage[] GetFunctions()
        {
            return saveFunc.ToArray();
        }
    }

    class ParamFuncHelper
    {
        Func<double, double> funcX, funcY;
        public ParamFuncHelper(
            Func<double, double> f1,
            Func<double, double> f2)
        {
            funcX = f1;
            funcY = f2;
        }

        public (double, double) Calculate(double t)
            => (funcX(t), funcY(t));
    }
}

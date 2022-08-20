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
            string[] funcs = FixZeroInExpression(funcsExp).ToUpper().Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var func in funcs)
            {
                if (isParamFunction(func))
                    saveFunc.Add(parseParamFunc(func));
                else if (isSingleFunction(func))
                    saveFunc.Add(parseSingleFunc(func));
                else if (isDoubleFunction(func))
                    saveFunc.Add(parseDoubleFunc(func));
                else
                    throw new ArgumentException("表达式无法转换成函数");
            }
        }

        private string FixZeroInExpression(string funcExp)
        {
            StringBuilder sb = new StringBuilder(funcExp);
            for (int i = 0; i < sb.Length; i++)
            {
                if (IsOmitZeroInsertPosition(sb, i))
                    sb.Insert(i, '0');
            }
            return sb.ToString();
        }

        private bool IsOmitZeroInsertPosition(StringBuilder sb, int index)
            => sb[index] == '-' && (index == 0 || sb[index - 1] == '(' || sb[index - 1] == ';');

        private bool isDoubleFunction(string func)
            => func.Contains('X') && func.Contains('Y') && func.Contains('=');

        private DoubleVarFuncStorage parseDoubleFunc(string func)
        {
            func = func.Replace('=', '-');
            var fp = new FunctionParser(func);
            fp.ParseExpression();
            return new DoubleVarFuncStorage(fp.GetExpressionTree());
        }

        private bool isSingleFunction(string func)
            => !func.Contains('Y') || (func[0] == 'Y' && func[1] == '=');

        private SingleVarFuncStorage parseSingleFunc(string func)
        {
            if (func[0] == 'Y' && func[1] == '=')
                func = func.Substring(2);
            var fp = new FunctionParser(func);
            fp.ParseExpression();
            return new SingleVarFuncStorage(fp.GetExpressionTree());
        }

        private bool isParamFunction(string func)
            => func.Contains(',');

        private ParamVarFuncStorage parseParamFunc(string func)
        {
            string[] funcs = func.Replace('T', 'X').Split(',');
            if (isSingleFunction(funcs[0]) && isSingleFunction(funcs[1]))
            {
                var p1 = new FunctionParser(funcs[0]);
                p1.ParseExpression();
                var p2 = new FunctionParser(funcs[1]);
                p2.ParseExpression();
                return new ParamVarFuncStorage(p1.GetExpressionTree(), p2.GetExpressionTree());
            }
            else
                throw new ArgumentException("表达式无法转换成参数函数");
        }

        public FunctionStorage[] GetFunctions()
        {
            return saveFunc.ToArray();
        }
    }

    internal class ParamFuncHelper
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

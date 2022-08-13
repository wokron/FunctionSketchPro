using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionSketch
{
    public abstract class FunctionStorage
    { }

    public class SingleVarFuncStorage : FunctionStorage
    {
        readonly Func<double, double> func;
        public SingleVarFuncStorage(Func<double, double> func)
        {
            this.func = func;
        }

        public Func<double, double> GetFunc()
            => func;
    }

    public class DoubleVarFuncStorage : FunctionStorage
    {
        readonly Func<double, double, double> func;
        public DoubleVarFuncStorage(Func<double, double, double> func)
        {
            this.func = func;
        }

        public Func<double, double, double> GetFunc()
            => func;
    }

    public class ParamVarFuncStorage : FunctionStorage
    {
        readonly Func<double, (double, double)> func;
        public ParamVarFuncStorage(Func<double, (double, double)> func)
        {
            this.func = func;
        }

        public Func<double, (double, double)> GetFunc()
            => func;
    }
}

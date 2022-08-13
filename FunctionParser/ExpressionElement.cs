using System;
using static System.Math;

public abstract class ExpressionElement
{
    protected ExpressionElement[] childElements = new ExpressionElement[0];
    public abstract double Calculate(double x);
    public abstract double Calculate(double x, double y);
}

public class Value : ExpressionElement
{
    private double val;
    public string SetValByString
    {
        set
        {
            if (value.ToUpper() == "P")
                val = PI;
            else if (value.ToUpper() == "E")
                val = E;
            else
                throw new ArgumentException("试图设置π和e之外的字符常量");
        }
    }

    public double Val
    {
        get => val;
        set => val = value;
    }

    public override double Calculate(double x)
        => val;

    public override double Calculate(double x, double y)
        => val;
}

public class ArgumentX : ExpressionElement
{
    public override double Calculate(double x)
        => x;

    public override double Calculate(double x, double y)
        => x;
}

public class ArgumentY : ExpressionElement
{
    public override double Calculate(double x)
    {
        throw new InvalidOperationException("试图用单变量x计算y值"); 
    }

    public override double Calculate(double x, double y)
        => y;
}
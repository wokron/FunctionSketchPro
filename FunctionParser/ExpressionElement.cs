using System;
using static System.Math;

public abstract class ExpressionElement
{
    protected ExpressionElement[] childElements = new ExpressionElement[0];
    public abstract double Calculate(double x);
    public abstract double Calculate(double x, double y);
    public abstract ExpressionElement Derivative();
    public abstract ExpressionElement Clone();
    public virtual ExpressionElement Simplified()
    {
        return this;
    }

    public string GetExp()
    {
        return $"({GetExpContent()})";
    }

    protected abstract string GetExpContent();
}

public class Value : ExpressionElement
{
    private double val;

    public Value() { }

    public Value(double val)
    {
        this.val = val;
    }

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

    public override ExpressionElement Clone()
    {
        return new Value(this.val);
    }

    public override ExpressionElement Derivative()
    {
        return new Value(0);
    }

    public override string ToString()
    {
        if (val == E)
            return "e";
        else if (val == PI)
            return "Pi";
        else
            return val.ToString();
    }

    protected override string GetExpContent()
        => val.ToString();
}

public class ArgumentX : ExpressionElement
{
    public override double Calculate(double x)
        => x;

    public override double Calculate(double x, double y)
        => x;

    public override ExpressionElement Clone()
    {
        return new ArgumentX();
    }

    public override ExpressionElement Derivative()
    {
        return new Value(1);
    }

    public override string ToString()
    {
        return "x";
    }

    protected override string GetExpContent()
        => ToString();
}

public class ArgumentY : ExpressionElement
{
    public override double Calculate(double x)
    {
        throw new InvalidOperationException("试图用单变量x计算y值"); 
    }

    public override double Calculate(double x, double y)
        => y;

    public override ExpressionElement Clone()
    {
        return new ArgumentY();
    }

    public override ExpressionElement Derivative()
    {
        throw new InvalidOperationException("y对x求导，无法得到表达式树");
    }

    public override string ToString()
    {
        return "y";
    }

    protected override string GetExpContent()
        => ToString();
}
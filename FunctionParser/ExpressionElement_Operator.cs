using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

public abstract class Operator : ExpressionElement
{
    protected abstract int GetPriority();

    public bool IsSmallerThan(Operator op2)
    {
        return this.GetPriority() < op2.GetPriority();
    }
}

public abstract class SingleElementOperator : Operator
{
    public SingleElementOperator()
    {
        childElements = new ExpressionElement[1];
    }

    public ExpressionElement ChildElem
    {
        get => childElements[0];
        set => childElements[0] = value;
    }
    public override double Calculate(double x)
    {
        double num = ChildElem.Calculate(x);
        return DoCalculation(num);
    }

    public override double Calculate(double x, double y)
    {
        double num = ChildElem.Calculate(x, y);
        return DoCalculation(num);
    }

    protected abstract double DoCalculation(double num);
}

public abstract class DoubleElementsOperator : Operator
{
    public DoubleElementsOperator()
    {
        childElements = new ExpressionElement[2];
    }

    public ExpressionElement Left
    {
        get => childElements[0];
        set => childElements[0] = value;
    }

    public ExpressionElement Right
    {
        get => childElements[1];
        set => childElements[1] = value;
    }
    public override double Calculate(double x)
    {
        double numLeft = Left.Calculate(x), numRight = Right.Calculate(x);
        return DoCalculation(numLeft, numRight);
    }

    public override double Calculate(double x, double y)
    {
        double numLeft = Left.Calculate(x, y), numRight = Right.Calculate(x, y);
        return DoCalculation(numLeft, numRight);
    }

    protected abstract double DoCalculation(double left, double right);
}

public class OpPlus : DoubleElementsOperator
{
    protected override double DoCalculation(double left, double right)
        => left + right;

    protected override int GetPriority() => 1;
}

public class OpSubtract : DoubleElementsOperator
{
    protected override double DoCalculation(double left, double right)
        => left - right;

    protected override int GetPriority() => 1;
}

public class OpMultiply : DoubleElementsOperator
{
    protected override double DoCalculation(double left, double right)
        => left * right;

    protected override int GetPriority() => 2;
}

public class OpDivide : DoubleElementsOperator
{
    protected override double DoCalculation(double left, double right)
        => left / right;

    protected override int GetPriority() => 2;
}

public class OpExponentiation : DoubleElementsOperator
{
    protected override double DoCalculation(double left, double right)
        => Pow(left, right);

    protected override int GetPriority() => 3;
}

public class OpLogarithm : DoubleElementsOperator
{
    protected override double DoCalculation(double left, double right)
        => Log(right, left);

    protected override int GetPriority() => 4;
}

public class OpLeftBracket : SingleElementOperator
{
    protected override double DoCalculation(double right)
        => double.NaN;

    protected override int GetPriority() => 6;
}

public class OpRightBranket : SingleElementOperator
{
    protected override double DoCalculation(double right)
        => double.NaN;

    protected override int GetPriority() => 6;
}

public class OpSin : SingleElementOperator
{
    protected override double DoCalculation(double right)
        => Sin(right);

    protected override int GetPriority() => 5;
}

public class OpCos : SingleElementOperator
{
    protected override double DoCalculation(double right)
        => Cos(right);

    protected override int GetPriority() => 5;
}

public class OpTan : SingleElementOperator
{
    protected override double DoCalculation(double right)
        => Tan(right);

    protected override int GetPriority() => 5;
}

public class OpArcsin : SingleElementOperator
{
    protected override double DoCalculation(double right)
        => Asin(right);

    protected override int GetPriority() => 5;
}

public class OpArccos : SingleElementOperator
{
    protected override double DoCalculation(double right)
        => Acos(right);

    protected override int GetPriority() => 5;
}

public class OpArctan : SingleElementOperator
{
    protected override double DoCalculation(double right)
        => Atan(right);

    protected override int GetPriority() => 5;
}

public static class OperatorFactory
{
    public static Operator GetOperator(string op)
    {
        return op switch
        {
            "+" => new OpPlus(),
            "-" => new OpSubtract(),
            "*" => new OpMultiply(),
            "/" => new OpDivide(),
            "^" => new OpExponentiation(),
            "L" => new OpLogarithm(),
            "(" => new OpLeftBracket(),
            ")" => new OpRightBranket(),
            "S" => new OpSin(),
            "C" => new OpCos(),
            "T" => new OpTan(),
            "~S" => new OpArcsin(),
            "~C" => new OpArccos(),
            "~T" => new OpArctan(),
            _ => throw new ArgumentException("生成运算符时出现未知参数"),
        };
    }
}

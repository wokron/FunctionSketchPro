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
    public override ExpressionElement Clone()
    {
        OpPlus rt = new OpPlus();
        rt.Left = this.Left.Clone();
        rt.Right = this.Right.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        ExpressionElement leftDerive = Left.Derivative(), rightDerive = Right.Derivative();
        OpPlus rt = new OpPlus();
        rt.Left = leftDerive;
        rt.Right = rightDerive;

        return rt;
    }

    protected override double DoCalculation(double left, double right)
        => left + right;

    protected override int GetPriority() => 1;
}

public class OpSubtract : DoubleElementsOperator
{
    public override ExpressionElement Clone()
    {
        OpSubtract rt = new OpSubtract();
        rt.Left = this.Left.Clone();
        rt.Right = this.Right.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        ExpressionElement leftDerive = Left.Derivative(), rightDerive = Right.Derivative();
        OpSubtract rt = new OpSubtract();
        rt.Left = leftDerive;
        rt.Right = rightDerive;

        return rt;
    }

    protected override double DoCalculation(double left, double right)
        => left - right;

    protected override int GetPriority() => 1;
}

public class OpMultiply : DoubleElementsOperator
{
    public override ExpressionElement Clone()
    {
        OpMultiply rt = new OpMultiply();
        rt.Left = this.Left.Clone();
        rt.Right = this.Right.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        ExpressionElement leftDerive = Left.Derivative(), rightDerive = Right.Derivative();
        OpMultiply leftBranch = new OpMultiply(), rightBranch = new OpMultiply();
        leftBranch.Left = leftDerive;
        leftBranch.Right = Right.Clone();
        rightBranch.Left = Left.Clone();
        rightBranch.Right = rightDerive;
        OpPlus rt = new OpPlus();
        rt.Left = leftBranch;
        rt.Right = rightBranch;

        return rt;
    }

    protected override double DoCalculation(double left, double right)
        => left * right;

    protected override int GetPriority() => 2;
}

public class OpDivide : DoubleElementsOperator
{
    public override ExpressionElement Clone()
    {
        OpDivide rt = new OpDivide();
        rt.Left = this.Left.Clone();
        rt.Right = this.Right.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpExponentiation pow = new OpExponentiation();
        pow.Left = this.Right.Clone();
        pow.Right = new Value(-1);
        OpMultiply root = new OpMultiply();
        root.Left = this.Left.Clone();
        root.Right = pow;

        return root.Derivative();
    }

    protected override double DoCalculation(double left, double right)
        => left / right;

    protected override int GetPriority() => 2;
}

public class OpExponentiation : DoubleElementsOperator
{
    public override ExpressionElement Clone()
    {
        OpExponentiation rt = new OpExponentiation();
        rt.Left = this.Left.Clone();
        rt.Right = this.Right.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        if (this.Left is Value) // 指数函数
        {
            double ln = Log((this.Left as Value).Val);
            OpMultiply mul = new OpMultiply();
            mul.Left = new Value(ln);
            mul.Right = this.Clone();
            OpMultiply rt = new OpMultiply();
            rt.Left = mul;
            rt.Right = this.Right.Derivative();

            return rt;
        }
        else if (this.Right is Value) // 多项式函数
        {
            double poli = (this.Right as Value).Val;
            OpExponentiation exp = new OpExponentiation();
            exp.Left = this.Left.Clone();
            exp.Right = new Value(poli - 1);
            OpMultiply mul = new OpMultiply();
            mul.Left = new Value(poli);
            mul.Right = exp;
            OpMultiply rt = new OpMultiply();
            rt.Left = mul;
            rt.Right = this.Left.Derivative();

            return rt;
        }
        else
            throw new InvalidOperationException("过于困难的求导");
    }

    protected override double DoCalculation(double left, double right)
        => Pow(left, right);

    protected override int GetPriority() => 3;
}

public class OpLogarithm : DoubleElementsOperator
{
    public override ExpressionElement Clone()
    {
        OpLogarithm rt = new OpLogarithm();
        rt.Left = this.Left.Clone();
        rt.Right = this.Right.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        if (this.Left is Value)
        {
            double ln = Log((this.Left as Value).Val);
            OpMultiply mul = new OpMultiply();
            mul.Left = this.Right.Clone();
            mul.Right = new Value(ln);
            OpDivide divide = new OpDivide();
            divide.Left = this.Right.Derivative();
            divide.Right = mul;

            return divide;
        }
        else
            throw new InvalidOperationException("过于困难的求导");
    }

    protected override double DoCalculation(double left, double right)
        => Log(right, left);

    protected override int GetPriority() => 4;
}

public class OpLeftBracket : SingleElementOperator
{
    public override ExpressionElement Clone()
    {
        OpLeftBracket rt = new OpLeftBracket();
        rt.ChildElem = this.ChildElem.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        throw new InvalidOperationException("表达式树中出现括号");
    }

    protected override double DoCalculation(double right)
        => double.NaN;

    protected override int GetPriority() => 6;
}

public class OpRightBranket : SingleElementOperator
{
    public override ExpressionElement Clone()
    {
        OpRightBranket rt = new OpRightBranket();
        rt.ChildElem = this.ChildElem.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        throw new InvalidOperationException("表达式树中出现括号");
    }

    protected override double DoCalculation(double right)
        => double.NaN;

    protected override int GetPriority() => 6;
}

public class OpSin : SingleElementOperator
{
    public override ExpressionElement Clone()
    {
        OpSin rt = new OpSin();
        rt.ChildElem = this.ChildElem.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpCos cos = new OpCos();
        cos.ChildElem = this.ChildElem.Clone();
        OpMultiply rt = new OpMultiply();
        rt.Left = cos;
        rt.Right = this.ChildElem.Derivative();

        return rt;
    }

    protected override double DoCalculation(double right)
        => Sin(right);

    protected override int GetPriority() => 5;
}

public class OpCos : SingleElementOperator
{
    public override ExpressionElement Clone()
    {
        OpCos rt = new OpCos();
        rt.ChildElem = this.ChildElem.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpSin sin = new OpSin();
        sin.ChildElem = this.ChildElem.Clone();
        OpMultiply mul = new OpMultiply();
        mul.Left = sin;
        mul.Right = this.ChildElem.Derivative();
        OpDivide divide = new OpDivide();
        divide.Left = new Value(0);
        divide.Right = mul;

        return divide;
    }

    protected override double DoCalculation(double right)
        => Cos(right);

    protected override int GetPriority() => 5;
}

public class OpTan : SingleElementOperator
{
    public override ExpressionElement Clone()
    {
        OpTan rt = new OpTan();
        rt.ChildElem = this.ChildElem.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpSin sin = new OpSin();
        sin.ChildElem = this.ChildElem.Clone();
        OpCos cos = new OpCos();
        cos.ChildElem = this.ChildElem.Clone();
        OpDivide divide = new OpDivide();
        divide.Left = sin;
        divide.Right = cos;

        return divide.Derivative();
    }

    protected override double DoCalculation(double right)
        => Tan(right);

    protected override int GetPriority() => 5;
}

public class OpArcsin : SingleElementOperator
{
    public override ExpressionElement Clone()
    {
        OpArcsin rt = new OpArcsin();
        rt.ChildElem = this.ChildElem.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpExponentiation exp1 = new OpExponentiation();
        exp1.Left = this.ChildElem.Clone();
        exp1.Right = new Value(2);
        OpSubtract subtract = new OpSubtract();
        subtract.Left = new Value(1);
        subtract.Right = exp1;
        OpExponentiation exp2 = new OpExponentiation();
        exp2.Left = subtract;
        exp2.Right = new Value(0.5);
        OpDivide divide = new OpDivide();
        divide.Left = this.ChildElem.Derivative();
        divide.Right = exp2;

        return divide;
    }

    protected override double DoCalculation(double right)
        => Asin(right);

    protected override int GetPriority() => 5;
}

public class OpArccos : SingleElementOperator
{
    public override ExpressionElement Clone()
    {
        OpArccos rt = new OpArccos();
        rt.ChildElem = this.ChildElem.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpArcsin arcsin = new OpArcsin();
        arcsin.ChildElem = this.ChildElem.Clone();
        OpSubtract subtract = new OpSubtract();
        subtract.Left = new Value(0);
        subtract.Right = arcsin.Derivative();

        return subtract;
    }

    protected override double DoCalculation(double right)
        => Acos(right);

    protected override int GetPriority() => 5;
}

public class OpArctan : SingleElementOperator
{
    public override ExpressionElement Clone()
    {
        OpArctan rt = new OpArctan();
        rt.ChildElem = this.ChildElem.Clone();
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpExponentiation exp = new OpExponentiation();
        exp.Left = this.ChildElem.Clone();
        exp.Right = new Value(2);
        OpPlus plus = new OpPlus();
        plus.Left = exp;
        plus.Right = new Value(1);
        OpDivide divide = new OpDivide();
        divide.Left = this.ChildElem.Derivative();
        divide.Right = plus;

        return divide;
    }

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

using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;

public abstract class Operator : ExpressionElement
{
    protected abstract int GetPriority();

    public bool IsSmallerOrEqualThan(Operator op2)
    {
        return this.GetPriority() <= op2.GetPriority();
    }
}

public abstract class SingleElementOperator : Operator
{
    public SingleElementOperator()
    {
        childElements = new ExpressionElement[1];
    }

    public SingleElementOperator(ExpressionElement childElem) : this()
    {
        ChildElem = childElem;
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

    public DoubleElementsOperator(ExpressionElement left, ExpressionElement right) : this()
    {
        Left = left;
        Right = right;
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
    public OpPlus() { }
    
    public OpPlus(ExpressionElement left, ExpressionElement right) : base(left, right) { }
    
    public override ExpressionElement Clone()
    {
        OpPlus rt = new OpPlus(Left.Clone(), Right.Clone());
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        ExpressionElement leftDerive = Left.Derivative(), rightDerive = Right.Derivative();
        OpPlus rt = new OpPlus(leftDerive, rightDerive);

        return rt;
    }

    protected override double DoCalculation(double left, double right)
        => left + right;

    protected override int GetPriority() => 1;

    public override string ToString()
    {
        return $"({Left}+{Right})";
    }
}

public class OpSubtract : DoubleElementsOperator
{
    public OpSubtract() { }

    public OpSubtract(ExpressionElement left, ExpressionElement right) : base(left, right) { }
    public override ExpressionElement Clone()
    {
        OpSubtract rt = new OpSubtract(Left.Clone(), Right.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        ExpressionElement leftDerive = Left.Derivative(), rightDerive = Right.Derivative();
        OpSubtract rt = new OpSubtract(leftDerive, rightDerive);

        return rt;
    }

    protected override double DoCalculation(double left, double right)
        => left - right;

    protected override int GetPriority() => 1;

    public override string ToString()
    {
        return $"({Left}-{Right})";
    }
}

public class OpMultiply : DoubleElementsOperator
{
    public OpMultiply() { }

    public OpMultiply(ExpressionElement left, ExpressionElement right) : base(left, right) { }
    public override ExpressionElement Clone()
    {
        OpMultiply rt = new OpMultiply(Left.Clone(), Right.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        ExpressionElement leftDerive = Left.Derivative(), rightDerive = Right.Derivative();
        OpMultiply leftBranch = new OpMultiply(leftDerive, Right.Clone()),
            rightBranch = new OpMultiply(Left.Clone(), rightDerive);
        OpPlus rt = new OpPlus(leftBranch, rightBranch);

        return rt;
    }

    protected override double DoCalculation(double left, double right)
        => left * right;

    protected override int GetPriority() => 2;

    public override string ToString()
    {
        return $"({Left}*{Right})";
    }
}

public class OpDivide : DoubleElementsOperator
{
    public OpDivide() { }

    public OpDivide(ExpressionElement left, ExpressionElement right) : base(left, right) { }
    public override ExpressionElement Clone()
    {
        OpDivide rt = new OpDivide(Left.Clone(), Right.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpExponentiation pow = new OpExponentiation(Right.Clone(), new Value(-1));
        OpMultiply root = new OpMultiply(Left.Clone(), pow);

        return root.Derivative();
    }

    protected override double DoCalculation(double left, double right)
        => left / right;

    protected override int GetPriority() => 2;

    public override string ToString()
    {
        return $"({Left}/{Right})";
    }
}

public class OpExponentiation : DoubleElementsOperator
{
    public OpExponentiation() { }

    public OpExponentiation(ExpressionElement left, ExpressionElement right) : base(left, right) { }
    public override ExpressionElement Clone()
    {
        OpExponentiation rt = new OpExponentiation(Left.Clone(), Right.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        if (this.Left is Value) // 指数函数
        {
            double ln = Log((this.Left as Value).Val);
            OpMultiply mul = new OpMultiply(new Value(ln), Clone());
            OpMultiply rt = new OpMultiply(mul, Right.Derivative());

            return rt;
        }
        else if (this.Right is Value) // 多项式函数
        {
            double poli = (this.Right as Value).Val;
            OpExponentiation exp = new OpExponentiation(Left.Clone(), new Value(poli - 1));
            OpMultiply mul = new OpMultiply(new Value(poli), exp);
            OpMultiply rt = new OpMultiply(mul, Left.Derivative());

            return rt;
        }
        else
            throw new InvalidOperationException("过于困难的求导");
    }

    protected override double DoCalculation(double left, double right)
        => Pow(left, right);

    protected override int GetPriority() => 3;

    public override string ToString()
    {
        return $"({Left}^{Right})";
    }
}

public class OpLogarithm : DoubleElementsOperator
{
    public OpLogarithm() { }

    public OpLogarithm(ExpressionElement left, ExpressionElement right) : base(left, right) { }
    public override ExpressionElement Clone()
    {
        OpLogarithm rt = new OpLogarithm(Left.Clone(), Right.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        if (this.Left is Value)
        {
            double ln = Log((this.Left as Value).Val);
            OpMultiply mul = new OpMultiply(Right.Clone(), new Value(ln));
            OpDivide divide = new OpDivide(Right.Derivative(), mul);

            return divide;
        }
        else
            throw new InvalidOperationException("过于困难的求导");
    }

    protected override double DoCalculation(double left, double right)
        => Log(right, left);

    protected override int GetPriority() => 4;

    public override string ToString()
    {
        return $"log{Left}({Right})";
    }
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
    public OpSin() { }

    public OpSin(ExpressionElement childElem) : base(childElem) { }
    public override ExpressionElement Clone()
    {
        OpSin rt = new OpSin(ChildElem.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpCos cos = new OpCos(ChildElem.Clone());
        OpMultiply rt = new OpMultiply(cos, ChildElem.Derivative());

        return rt;
    }

    protected override double DoCalculation(double right)
        => Sin(right);

    protected override int GetPriority() => 5;

    public override string ToString()
    {
        return $"sin{ChildElem}";
    }
}

public class OpCos : SingleElementOperator
{
    public OpCos() { }

    public OpCos(ExpressionElement childElem) : base(childElem) { }
    public override ExpressionElement Clone()
    {
        OpCos rt = new OpCos(ChildElem.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpSin sin = new OpSin(ChildElem.Clone());
        OpMultiply mul = new OpMultiply(sin, ChildElem.Derivative());
        OpDivide divide = new OpDivide(new Value(0), mul);

        return divide;
    }

    protected override double DoCalculation(double right)
        => Cos(right);

    protected override int GetPriority() => 5;

    public override string ToString()
    {
        return $"cos{ChildElem}";
    }
}

public class OpTan : SingleElementOperator
{
    public OpTan() { }

    public OpTan(ExpressionElement childElem) : base(childElem) { }
    public override ExpressionElement Clone()
    {
        OpTan rt = new OpTan(ChildElem.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpSin sin = new OpSin(ChildElem.Clone());
        OpCos cos = new OpCos(ChildElem.Clone());
        OpDivide divide = new OpDivide(sin, cos);

        return divide.Derivative();
    }

    protected override double DoCalculation(double right)
        => Tan(right);

    protected override int GetPriority() => 5;

    public override string ToString()
    {
        return $"tan{ChildElem}";
    }
}

public class OpArcsin : SingleElementOperator
{
    public OpArcsin() { }

    public OpArcsin(ExpressionElement childElem) : base(childElem) { }
    public override ExpressionElement Clone()
    {
        OpArcsin rt = new OpArcsin(ChildElem.Clone());
        
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpExponentiation exp1 = new OpExponentiation(ChildElem.Clone(), new Value(2));
        OpSubtract subtract = new OpSubtract(new Value(1), exp1);
        OpExponentiation exp2 = new OpExponentiation(subtract, new Value(0.5));
        OpDivide divide = new OpDivide(ChildElem.Derivative(), exp2);

        return divide;
    }

    protected override double DoCalculation(double right)
        => Asin(right);

    protected override int GetPriority() => 5;

    public override string ToString()
    {
        return $"arcsin{ChildElem}";
    }
}

public class OpArccos : SingleElementOperator
{
    public OpArccos() { }

    public OpArccos(ExpressionElement childElem) : base(childElem) { }
    public override ExpressionElement Clone()
    {
        OpArccos rt = new OpArccos(ChildElem.Clone());

        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpArcsin arcsin = new OpArcsin(ChildElem.Clone());
        OpSubtract subtract = new OpSubtract(new Value(0), arcsin.Derivative());

        return subtract;
    }

    protected override double DoCalculation(double right)
        => Acos(right);

    protected override int GetPriority() => 5;

    public override string ToString()
    {
        return $"arccos{ChildElem}";
    }
}

public class OpArctan : SingleElementOperator
{
    public OpArctan() { }

    public OpArctan(ExpressionElement childElem) : base(childElem) { }
    public override ExpressionElement Clone()
    {
        OpArctan rt = new OpArctan(ChildElem.Clone());
        
        return rt;
    }

    public override ExpressionElement Derivative()
    {
        OpExponentiation exp = new OpExponentiation(ChildElem.Clone(), new Value(2));
        OpPlus plus = new OpPlus(exp, new Value(1));
        OpDivide divide = new OpDivide(ChildElem.Derivative(), plus);

        return divide;
    }

    protected override double DoCalculation(double right)
        => Atan(right);

    protected override int GetPriority() => 5;

    public override string ToString()
    {
        return $"arctan{ChildElem}";
    }
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

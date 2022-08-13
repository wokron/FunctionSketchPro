using System;
using System.Collections.Generic;
public class FunctionParser
{
    private string expression;

    private Stack<ExpressionElement> saveTree = new Stack<ExpressionElement>();
    private Stack<Operator> saveOp = new Stack<Operator>();

    private ExpressionElement expressionTree;
    public FunctionParser(string function)
    {
        string withoutSpace = function.Replace(" ", "").ToUpper();
        string replacedPi = withoutSpace.Replace("PI", "P");
        string replacedLog = replacedPi.Replace("LOG", "L"); // 将log替换为单字符
        string replacedArc = replacedLog.Replace("ARC", "~");
        string replacedSinCosTan = replacedArc
            .Replace("SIN", "S").Replace("COS", "C").Replace("TAN", "T");

        // TODO 加入三角函数后这里需要修改
        expression = replacedSinCosTan;
    }

    public void ParseExpression()
    {
        if (expression == string.Empty) // 对空串特殊处理
        {
            expressionTree = new Value() { Val = double.NaN };
            return;
        }
        int length = expression.Length;
        for (int i = 0; i < length; i++)
        {
            if (char.IsDigit(expression[i]))
                ProcessDigit(ref i);
            else if (IsAlpha(expression[i]))
                ProcessAlpha(i);
            else // 如果是运算符
            {
                Operator nowOp;
                if (expression[i] == '~')
                {
                    nowOp = OperatorFactory.GetOperator(expression.Substring(i, 2));
                    i++;
                }
                else
                    nowOp = OperatorFactory.GetOperator(expression.Substring(i, 1));
                ProcessOperator(nowOp);
            }
        }
        CleanRestOperator();

        expressionTree = saveTree.Peek();
    }

    private void ProcessDigit(ref int i)
    {
        int start = i;
        while (IsIndoubleNum(i))
            i++;
        saveTree.Push(new Value()
        {
            Val = (double)Convert
            .ToDouble(expression[start..i])
        });
        i--;
    }

    private void ProcessAlpha(int i)
    {
        if (IsArgument(expression[i]))
            saveTree.Push(new ArgumentX());
        else
            saveTree.Push(new Value()
            { SetValByString = expression.Substring(i, 1) });
    }

    private bool IsIndoubleNum(int i) =>
        i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.');

    private bool IsAlpha(char ch) =>
        ch == 'x' || ch == 'X' || ch == 'E' || ch == 'P';

    private bool IsArgument(char ch) => ch == 'x' || ch == 'X';

    private void ProcessOperator(Operator op)
    {
        try
        {
            if (op is OpRightBranket)
            {
                while (!(saveOp.Peek() is OpLeftBracket))
                {
                    AddOperatorToTree(saveOp.Pop());
                }
                saveOp.Pop();
            }
            else if (op is OpLeftBracket)
                saveOp.Push(op);
            else
            {
                while (saveOp.Count != 0
                    && !(saveOp.Peek() is OpLeftBracket)
                    && op.IsSmallerThan(saveOp.Peek()))
                {
                    AddOperatorToTree(saveOp.Pop());
                }
                saveOp.Push(op);
            }
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException("输入的表达式非法");
        }
    }

    private void AddOperatorToTree(Operator op)
    {
        try
        {
            if (op is SingleElementOperator)
            {
                ExpressionElement elem = saveTree.Pop();
                ((SingleElementOperator)op).ChildElem = elem;
            }
            else
            {
                ExpressionElement rVal = saveTree.Pop(),
                    lVal = saveTree.Pop();
                DoubleElementsOperator dop = (DoubleElementsOperator)op;
                dop.Right = rVal;
                dop.Left = lVal;
            }
            saveTree.Push(op);
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException("输入的表达式非法");
        }
    }

    private void CleanRestOperator()
    {
        while (saveOp.Count != 0)
        {
            AddOperatorToTree(saveOp.Pop());
        }
    }

    public ExpressionElement GetExpressionTree()
    {
        return expressionTree;
    }
}

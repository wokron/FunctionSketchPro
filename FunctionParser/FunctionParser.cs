using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionSketch.FunctionParser
{
    public class FunctionParser
    {
        private string expression;

        private readonly Stack<ExpressionElement> saveTree = new Stack<ExpressionElement>();
        private readonly Stack<Operator> saveOp = new Stack<Operator>();

        private ExpressionElement expressionTree = null;
        public FunctionParser(string expression)
        {
            this.expression = Formating(expression);
        }

        private string Formating(string expression)
        {
            StringBuilder sb = new StringBuilder(expression.ToUpper());
            StringBuilder withoutSpace = sb.Replace(" ", "");
            StringBuilder replacedPi = withoutSpace.Replace("PI", "P");
            StringBuilder replacedLog = replacedPi.Replace("LOG", "L"); // 将log替换为单字符
            StringBuilder replacedArc = replacedLog.Replace("ARC", "~");
            StringBuilder replacedSinCosTan = replacedArc
                .Replace("SIN", "S").Replace("COS", "C").Replace("TAN", "T");

            return replacedSinCosTan.ToString();
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
                char nowExp = expression[i];
                if (char.IsDigit(nowExp))
                    ProcessDigit(ref i);
                else if (IsAlpha(nowExp))
                    ProcessAlpha(nowExp);
                else // 如果是运算符
                {
                    Operator nowOp;
                    if (nowExp == '~')
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

        private void ProcessAlpha(char ch)
        {
            if (IsArgumentX(ch))
                saveTree.Push(new ArgumentX());
            else if (IsArgumentY(ch))
                saveTree.Push(new ArgumentY());
            else
                saveTree.Push(new Value()
                { SetValByString = Convert.ToString(ch) });
        }

        private bool IsArgumentX(char ch) => ch == 'X';

        private bool IsArgumentY(char ch) => ch == 'Y';

        private bool IsIndoubleNum(int i) =>
            i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.');

        private bool IsAlpha(char ch) =>
            ch == 'X' || ch == 'Y' || ch == 'E' || ch == 'P';

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
            if (expressionTree == null)
                ParseExpression();
            return expressionTree;
        }
    }
}

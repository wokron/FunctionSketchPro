using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static System.Math;

namespace FunctionSketch
{
    internal class SquareType
    {
        protected double rate1 = 0.5, rate2 = 0.5;
        
        public static SquareType GetType(byte num)
        {
            switch (num)
            {
                case 0b1111:
                case 0b0000:
                    return new Type1111();
                case 0b1110:
                case 0b0001:
                    return new Type1110();
                case 0b1101:
                case 0b0010:
                    return new Type1101();
                case 0b1011:
                case 0b0100:
                    return new Type1011();
                case 0b0111:
                case 0b1000:
                    return new Type0111();
                case 0b1100:
                case 0b0011:
                    return new Type1100();
                case 0b1001:
                case 0b0110:
                    return new Type1001();
                case 0b1010:
                    return new Type1010();
                case 0b0101:
                    return new Type0101();
                default:
                    throw new ArgumentException("不可能出现的square类型");
            }
        }

        public virtual IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {
            yield return CreateVectorPair(0, 0, 0, 0);
        }

        protected (Vector, Vector) CreateVectorPair(double v1, double v2, double v3, double v4)
            => (new Vector(v1, v2), new Vector(v3, v4));
    }

    internal class Type1111 : SquareType
    {

    }

    internal class Type1110 : SquareType
    {
        public override IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {
            
            rate1 = Abs(v4 / (v1 - v4));
            rate2 = Abs(v3 / (v3 - v4));
            yield return CreateVectorPair(-deci, deci * rate1, -deci * rate2, 0);
        }
    }

    internal class Type1101 : SquareType
    {
        public override IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {

            rate1 = Abs(v3 / (v3 - v4));
            rate2 = Abs(v3 / (v2 - v3));
            yield return CreateVectorPair(-deci * rate1, 0, 0, deci * rate2);
        }
    }

    internal class Type1011 : SquareType
    {
        public override IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {
            rate1 = Abs(v2 / (v1 - v2));
            rate2 = Abs(v3 / (v2 - v3));
            yield return CreateVectorPair(-deci * rate1, deci, 0, deci * rate2);
        }
    }

    internal class Type0111 : SquareType
    {
        public override IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {
            rate1 = Abs(v4 / (v1 - v4));
            rate2 = Abs(v2 / (v1 - v2));
            yield return CreateVectorPair(-deci, deci * rate1, -deci * rate2, deci);
        }
    }

    internal class Type1100 : SquareType
    {
        public override IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {
            rate1 = Abs(v4 / (v1 - v4));
            rate2 = Abs(v3 / (v2 - v3));
            yield return CreateVectorPair(-deci, deci * rate1, 0, deci * rate2);
        }
    }

    internal class Type1001 : SquareType
    {
        public override IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {
            rate1 = Abs(v3 / (v3 - v4));
            rate2 = Abs(v2 / (v1 - v2));
            yield return CreateVectorPair(-deci * rate1, 0, -deci * rate2, deci);
        }
    }

    internal class Type1010 : SquareType
    {
        public override IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {
            rate1 = Abs(v4 / (v1 - v4));
            rate2 = Abs(v2 / (v1 - v2));
            yield return CreateVectorPair(-deci, deci * rate1, -deci * rate2, deci);
            rate1 = Abs(v3 / (v3 - v4));
            rate2 = Abs(v3 / (v2 - v3));
            yield return CreateVectorPair(-deci * rate1, 0, 0, deci * rate2);
        }
    }

    internal class Type0101 : SquareType
    {
        public override IEnumerable<(Vector, Vector)> GetVectorPairs(double deci, double v1, double v2, double v3, double v4)
        {
            rate1 = Abs(v4 / (v1 - v4));
            rate2 = Abs(v3 / (v3 - v4));
            yield return CreateVectorPair(-deci, deci * rate1, -deci * rate2, 0);
            rate1 = Abs(v2 / (v1 - v2));
            rate2 = Abs(v3 / (v2 - v3));
            yield return CreateVectorPair(-deci * rate1, deci, 0, deci * rate2);
        }
    }
}

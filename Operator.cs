/*
Formula -- Prase the math formula to a System.Linq.Expressions.Expression
Written in 2019 by zyx <zyx@acortinfo.com>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace aco.tools.NFormula
{
    /// <summary>
    /// 操作运算符
    /// (支持+,-,*,/,^,负号-)
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 运算符优先级
        /// </summary>
        PriorityType Priority { get; }

        /// <summary>
        /// 运算符类型
        /// </summary>
        OperatorType OperatorType { get; }
    }

    /// <summary>
    /// 一元操作符
    /// </summary>
    public abstract class UnaryOperator : IOperator
    {
        public OperatorType OperatorType
        {
            get
            {
                return OperatorType.Unary;
            }
        }

        public abstract PriorityType Priority { get; }

        public abstract Expression Apply(Expression exp);
    }

    /// <summary>
    /// 二元操作符
    /// </summary>
    public abstract class BinaryOperator : IOperator
    {
        public OperatorType OperatorType
        {
            get
            {
                return OperatorType.Binary;
            }
        }

        public abstract PriorityType Priority { get; }

        public abstract Expression Apply(Expression left, Expression right);
    }


    public class NegateOperator : UnaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.Second;
            }
        }

        public override Expression Apply(Expression exp)
        {
            return Expression.Negate(exp);
        }
    }

    public class GreaterOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.First;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }
    }

    public class GreaterEqualOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.First;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.GreaterThanOrEqual(left, right);
        }
    }

    public class LessOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.First;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.LessThan(left, right);
        }
    }

    public class LessEqualOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.First;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.LessThanOrEqual(left, right);
        }
    }

    public class EqualOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.First;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }
    }

    public class NotEqualOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.First;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.NotEqual(left, right);
        }
    }

    public class AddOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.Second;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.Add(left, right);
        }
    }

    public class SubtractOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.Second;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.Subtract(left, right);
        }
    }

    public class MultiplyOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.Third;
            }
        }


        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.Multiply(left, right);
        }
    }

    public class DivideOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.Third;
            }
        }

        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.Divide(left, right);
        }
    }

    public class PowerOperator : BinaryOperator
    {
        public override PriorityType Priority
        {
            get
            {
                return PriorityType.Fourth;
            }
        }

        public override Expression Apply(Expression left, Expression right)
        {
            return Expression.Power(left, right);
        }
    }

    public enum OperatorType
    {
        Unary,
        Binary
    }

    public enum UnaryType
    {
        Negate
    }

    public enum BinaryType
    {
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        Equal,
        NotEqual,
        Add,
        Subtract,
        Multiply,
        Divide,
        Power
    }

    public enum PriorityType
    {
        First = 0,
        Second = 1,
        Third = 2,
        Fourth = 3
    }
}

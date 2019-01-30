/*
Formula -- Prase the math formula to a System.Linq.Expressions.Expression
Written in 2019 by zyx <zyx@acortinfo.com>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace aco.tools.Formula
{
    /// <summary>
    /// 表达式节点类
    /// </summary>
    public class ExpressionNode
    {

        public ExpressionNode()
        {

        }

        public ExpressionNode(int idx)
        {
            this.Index = idx;
        }

        public Expression LeftLeaf
        {
            get;
            set;
        }

        public Expression RightLeaf
        {
            get;
            set;
        }

        public IOperator Node
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// 节点是否可以操作
        /// </summary>
        public bool CanOperator
        {
            get
            {
                if (this == null || this.Node == null)
                {
                    return false;
                }
                else if (this.Node.OperatorType == OperatorType.Unary)
                {
                    if (this.LeftLeaf == null && this.RightLeaf == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (this.Node.OperatorType == OperatorType.Binary)
                {
                    if (this.LeftLeaf != null && this.RightLeaf != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 计算节点值
        /// </summary>
        /// <returns>节点值</returns>
        public Expression GetResult()
        {
            if (Node.OperatorType == OperatorType.Unary)
            {
                if (LeftLeaf != null && LeftLeaf != Expression.Empty())
                {
                    return (Node as UnaryOperator).Apply(LeftLeaf);
                }
                else if (RightLeaf != null && RightLeaf != Expression.Empty())
                {
                    return (Node as UnaryOperator).Apply(RightLeaf);
                }
                else
                {
                    return Expression.Empty();
                }
            }
            else if (Node.OperatorType == OperatorType.Binary)
            {
                return (Node as BinaryOperator).Apply(LeftLeaf, RightLeaf);
            }
            else
            {
                return Expression.Empty();
            }
        }

        public void Empty()
        {
            this.LeftLeaf = null;
            this.RightLeaf = null;
            this.Node = null;
        }

        /// <summary>
        /// 连接节点
        /// </summary>
        /// <param name="node2">要连接的节点</param>
        /// <param name="op">用于连接的操作</param>
        /// <returns>连接之后获得的节点</returns>
        public ExpressionNode Link(ExpressionNode node2, IOperator op)
        {
            ExpressionNode nNode = new ExpressionNode();
            nNode.LeftLeaf = this.GetResult();
            nNode.RightLeaf = node2.GetResult();
            nNode.Node = op;
            return nNode;
        }

        /// <summary>
        /// 连接节点
        /// </summary>
        /// <param name="node1">要连接的节点一</param>
        /// <param name="node2">要连接的节点二</param>
        /// <param name="op">用于连接的操作</param>
        /// <returns>连接之后获得的节点</returns>
        public static ExpressionNode Link(ExpressionNode node1, ExpressionNode node2, IOperator op)
        {
            ExpressionNode nNode = new ExpressionNode();
            nNode.LeftLeaf = node1.GetResult();
            nNode.RightLeaf = node2.GetResult();
            nNode.Node = op;
            return nNode;
        }
    }
}

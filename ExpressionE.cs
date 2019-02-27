using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using aco.common;

namespace aco.tools.NFormula
{
    public static class ExpressionE
    {
        /// <summary>
        /// 添加指定名称的Double类型的参数
        /// </summary>
        /// <param name="paras">参数列表</param>
        /// <param name="pName">添加的新参数名称</param>
        /// <returns>添加后的新参数列表</returns>
        public static IEnumerable<ParameterExpression> AddDouble(this IEnumerable<ParameterExpression> paras, string pName)
        {
            paras = paras.Add(Expression.Variable(typeof(double), pName));
            return paras;
        }

        public static IEnumerable<ParameterExpression> AddDouble(this IEnumerable<ParameterExpression> paras, params string[] pNames)
        {
            for (int i = 0; i < pNames.Length; i++)
            {
                paras = paras.Add(Expression.Variable(typeof(double), pNames[i]));
            }
            return paras;
        }

        /// <summary>
        /// 计算表达式结果
        /// </summary>
        /// <param name="exp">要计算的表达式</param>
        /// <param name="paras">形参列表</param>
        /// <param name="objs">实参列表</param>
        /// <returns>计算结果</returns>
        public static object Invode(this Expression exp, IEnumerable<ParameterExpression> paras, object[] objs)
        {
            LambdaExpression le = Expression.Lambda(exp, paras);
            Delegate de = le.Compile();
            return de.DynamicInvoke(objs);
        }

        /// <summary>
        /// 获取表达式参数列表
        /// </summary>
        /// <param name="exp">要分析的表达式</param>
        /// <param name="tag">缓存的参数列表(用于递归的中间变量)</param>
        /// <returns>表达式中的参数列表</returns>
        public static List<ParameterExpression> Variables(this Expression exp, List<ParameterExpression> tag = null)
        {

            List<ParameterExpression> list = new List<ParameterExpression>();
            if (tag != null && tag.Count > 0)
            {
                list = list.Union(tag).ToList();
            }
            string expType = exp.GetType().Name;
            //比较器,用于过滤重复参数
            var eComparer = Equality<ParameterExpression>.CreateComparer(p => p.Name);
            //判定表达式是否为可处理的类型
            if (expType.Equals("SimpleBinaryExpression") || expType.Equals("LogicalBinaryExpression") || expType.Equals("MethodBinaryExpression") || expType.Equals("BinaryExpression"))
            {
                var be = exp as BinaryExpression;
                //处理left
                if (be.Left.NodeType == ExpressionType.Parameter)
                {
                    var lp = be.Left as ParameterExpression;
                    if (!list.Contains(lp, eComparer))
                    {
                        list.Add(lp);
                    }
                }
                else if (be.Left.NodeType != ExpressionType.Constant)
                {
                    var lbe = be.Left as BinaryExpression;
                    if (lbe != null)
                    {
                        list = Variables(lbe, list);
                    }
                    else
                    {
                        var lue = be.Left as UnaryExpression;
                        if (lue != null)
                        {
                            list = Variables(lue, list);
                        }
                    }
                }
                //处理right
                if (be.Right.NodeType == ExpressionType.Parameter)
                {
                    var rp = be.Right as ParameterExpression;
                    if (!list.Contains(rp, eComparer))
                    {
                        list.Add(rp);
                    }
                }
                else
                {
                    var rbe = be.Right as BinaryExpression;
                    if (rbe != null)
                    {
                        list = Variables(rbe, list);
                    }
                    else
                    {
                        var rue = be.Right as UnaryExpression;
                        if (rue != null)
                        {
                            list = Variables(rue, list);
                        }
                    }
                }
            }
            else if (expType.Equals("UnaryExpression"))
            {
                var ue = exp as UnaryExpression;
                if (ue.Operand.NodeType == ExpressionType.Parameter)
                {
                    var op = ue.Operand as ParameterExpression;
                    if (!list.Contains(op, eComparer))
                    {
                        list.Add(op);
                    }
                }
                else
                {
                    list = Variables(ue.Operand);
                }
            }
            return list;
        }
    }
}

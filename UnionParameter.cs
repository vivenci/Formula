using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace aco.tools.NFormula
{
    /// <summary>
    /// 组合参数
    /// </summary>
    public class UnionParameter
    {
        /// <summary>
        /// 构造组合参数
        /// </summary>
        /// <param name="pName">组合参数名称</param>
        /// <param name="paras">子参数形参列表</param>
        /// <param name="desc">组合参数描述</param>
        public UnionParameter(string pName, IEnumerable<ParameterExpression> paras, string desc = "")
        {
            this.ParamName = pName;
            this.SubParams = paras;
            this.Description = desc;
        }

        /// <summary>
        /// 构造组合参数
        /// </summary>
        /// <param name="pName">组合参数名称</param>
        /// <param name="paras">子参数形参列表</param>
        /// <param name="exp">组合参数表达式</param>
        /// <param name="desc">组合参数描述</param>
        public UnionParameter(string pName, IEnumerable<ParameterExpression> paras, Expression exp, string desc = "") : this(pName, paras, desc)
        {
            this.Expression = exp;
        }

        /// <summary>
        /// 组合参数名称
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 组合参数描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 组合参数表达式
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        /// 组合参数表达式相关参数列表
        /// </summary>
        public IEnumerable<ParameterExpression> SubParams { get; set; }

        //public object GetResult(object[] args)
        //{
        //    LambdaExpression le = Expression.Lambda(this.Expression, this.SubParams);
        //    Delegate de = le.Compile();
        //    return de.DynamicInvoke(args);
        //}
    }
}

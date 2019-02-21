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
        public UnionParameter(string pName, IEnumerable<ParameterExpression> paras)
        {
            this.ParamName = pName;
            this.SubParams = paras;
        }

        public UnionParameter(string pName, IEnumerable<ParameterExpression> paras, Expression exp) : this(pName, paras)
        {
            this.Expression = exp;
        }

        /// <summary>
        /// 组合参数名称
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// 组合参数表达式
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        /// 组合参数表达式相关参数列表
        /// </summary>
        public IEnumerable<ParameterExpression> SubParams { get; set; }

        public object GetResult(object[] args)
        {
            LambdaExpression le = Expression.Lambda(this.Expression, this.SubParams);
            Delegate de = le.Compile();
            return de.DynamicInvoke(args);
        }
    }
}

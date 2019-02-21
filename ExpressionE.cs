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
        public static IEnumerable<ParameterExpression> AddDouble(this IEnumerable<ParameterExpression> paras, string pName)
        {
            paras = paras.Add(Expression.Variable(typeof(double), pName));
            return paras;
        }
    }
}

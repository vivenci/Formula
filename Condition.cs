using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace aco.tools.NFormula
{
    /// <summary>
    /// 目标表达式选取条件
    /// (根据规则和条件从备选表达式中选择满足条件的表达式作为公式的目标表达式)
    /// </summary>
    public class Condition : Formula
    {
        public Condition(string exp, IEnumerable<ParameterExpression> paras = null, Dictionary<string, UnionParameter> eParas = null) : base(exp, paras)
        {
            if (eParas != null && eParas.Count > 0)
            {
                foreach (var item in eParas)
                {
                    if (!this.ExtraParams.ContainsKey(item.Key))
                    {
                        this.ExtraParams.Add(item.Key, item.Value);
                    }
                }
            }
        }

        public Condition(string exp, string condiName, IEnumerable<ParameterExpression> paras = null, Dictionary<string, UnionParameter> eParas = null) : this(exp, paras, eParas)
        {
            this.Name = condiName;
        }

        /// <summary>
        /// 条件是否成立
        /// </summary>
        public bool Satisfy(object[] paras)
        {
            bool flag = false;
            if (this.Expression != null)
            {
                object ret = base.GetResult(paras);
                if (ret == null)
                {
                    return false;
                }
                else
                {
                    Boolean.TryParse(ret.ToString(), out flag);
                }
            }
            return flag;
        }
    }
}

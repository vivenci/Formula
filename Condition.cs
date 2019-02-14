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
    public class Condition
    {
        private Formula f = null;
        public Condition(string exp)
        {
            f = new Formula(exp);
            this.Expression = f.Expression;
        }

        /// <summary>
        /// 条件名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 条件表达式
        /// </summary>
        public Expression Expression
        {
            get;
            set;
        }

        /// <summary>
        /// 条件是否成立
        /// </summary>
        public bool Satisfy
        {
            get;
            set;
        }
    }
}

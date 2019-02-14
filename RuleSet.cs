using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace aco.tools.NFormula
{
    /// <summary>
    /// 规则集
    /// </summary>
    public class RuleSet : List<Rule>
    {


        public Rule this[string name]
        {
            get
            {
                if (this.Count > 0)
                {
                    return this.SingleOrDefault(r => r.Name.Equals(name));
                }
                else
                {
                    return null;
                }
            }
        }

        public RuleSet Add(Rule r)
        {
            if (r != null)
            {
                this.Add(r);
            }
            return this;
        }

        public RuleSet Add(Condition c, bool cVal, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(c, cVal, priority, rName, ret);
            this.Add(r);
            return this;
        }

        public RuleSet Add(KeyValuePair<Condition, bool> condi, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(condi, priority, rName, ret);
            this.Add(r);
            return this;
        }

        public RuleSet Add(Tuple<Condition, bool> condi, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(condi, priority, rName, ret);
            this.Add(r);
            return this;
        }

        public RuleSet Add(Dictionary<Condition, bool> condis, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(condis, priority, rName, ret);
            this.Add(r);
            return this;
        }

        public RuleSet Add(List<Tuple<Condition, bool>> condis, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(condis, priority, rName, ret);
            this.Add(r);
            return this;
        }

        /// <summary>
        /// 规则适配
        /// 将规则按照优先级排序后,逐项验证规则是否被满足
        /// </summary>
        /// <returns>满足则返回对应规则表达式,若都不满足则返回null</returns>
        public Expression Apply()
        {
            Expression exp = null;
            if (this.Count > 0)
            {
                //将规则按照优先级排序
                var comparer = Rule.GetComparer();
                this.Sort(comparer);

                for (int i = 0; i < this.Count; i++)
                {
                    Rule r = this[i];
                    //若某规则成立,则应用此规则,返回其对应的表达式
                    if (r.IsValid())
                    {
                        exp = r.Result;
                        break;
                    }
                }
            }
            return exp;
        }

    }
}

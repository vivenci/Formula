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
    public class RuleSet
    {
        private List<Rule> innerList = null;
        public RuleSet()
        {
            this.innerList = new List<Rule>();
        }

        public Rule this[int index]
        {
            get
            {
                if (this.innerList.Count > 0 && index >= 0)
                {
                    return this.innerList[index];
                }
                else
                {
                    return null;
                }
            }
        }

        public Rule this[string name]
        {
            get
            {
                if (this.innerList.Count > 0)
                {
                    return this.innerList.SingleOrDefault(r => r.Name.Equals(name));
                }
                else
                {
                    return null;
                }
            }
        }

        public int Count
        {
            get
            {
                if (this.innerList == null)
                {
                    return -1;
                }
                else
                {
                    return this.innerList.Count;
                }
            }
        }

        public RuleSet Add(Rule r)
        {
            if (r != null)
            {
                this.innerList.Add(r);
            }
            return this;
        }

        public RuleSet Add(Condition c, bool cVal, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(c, cVal, priority, rName, ret);
            this.innerList.Add(r);
            return this;
        }

        public RuleSet Add(KeyValuePair<Condition, bool> condi, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(condi, priority, rName, ret);
            this.innerList.Add(r);
            return this;
        }

        public RuleSet Add(Tuple<Condition, bool> condi, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(condi, priority, rName, ret);
            this.innerList.Add(r);
            return this;
        }

        public RuleSet Add(Dictionary<Condition, bool> condis, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(condis, priority, rName, ret);
            this.innerList.Add(r);
            return this;
        }

        public RuleSet Add(List<Tuple<Condition, bool>> condis, int priority, string rName = "", Expression ret = null)
        {
            Rule r = new Rule(condis, priority, rName, ret);
            this.innerList.Add(r);
            return this;
        }

        /// <summary>
        /// 规则适配
        /// 将规则按照优先级排序后,逐项验证规则是否被满足
        /// </summary>
        /// <param name="paras">判定时传递的参数数组</param>
        /// <returns>满足则返回对应规则表达式,若都不满足则返回null</returns>
        public Expression Apply(object[] paras = null)
        {
            Expression exp = null;
            if (this.innerList.Count > 0)
            {
                //将规则按照优先级排序
                var comparer = Rule.GetComparer();
                this.innerList.Sort(comparer);

                for (int i = 0; i < this.innerList.Count; i++)
                {
                    Rule r = this.innerList[i];
                    //若某规则成立,则应用此规则,返回其对应的表达式
                    if (r.IsValid(paras))
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

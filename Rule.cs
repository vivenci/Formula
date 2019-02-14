using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace aco.tools.NFormula
{
    /// <summary>
    /// 目标表达式选取规则
    /// 规则可以包含多项条件,以及条件满足规则的要求(真或者假)
    /// 所有条件项均满足规则要求,则认为该规则被满足
    /// </summary>
    public class Rule : Dictionary<Condition, bool>
    {
        //key为条件,value为此规则中条件应满足的值
        private Dictionary<Condition, bool> dict = new Dictionary<Condition, bool>();

        public Rule()
        {
        }

        /// <summary>
        /// 初始化规则
        /// </summary>
        /// <param name="priority">规则优先级</param>
        /// <param name="name">规则名称</param>
        /// <param name="ret">规则返回值</param>
        public Rule(int priority, string name = "", Expression ret = null)
        {
            this.Name = string.IsNullOrEmpty(name) ? ("规则" + priority) : name;
            this.Priority = priority;
            this.Result = ret;
        }

        /// <summary>
        /// 使用单一条件初始化规则
        /// </summary>
        /// <param name="c">条件</param>
        /// <param name="cVal">该规则下条件应取的值</param>
        /// <param name="priority">规则优先级</param>
        /// <param name="rName">规则名称</param>
        /// <param name="ret">规则返回值</param>
        public Rule(Condition c, bool cVal, int priority, string rName = "", Expression ret = null)
        {
            this.Name = string.IsNullOrEmpty(rName) ? ("规则" + priority) : rName;
            this.Priority = priority;
            this.Result = ret;
            this.Add(c, cVal);
        }

        /// <summary>
        /// 使用单一条件初始化规则
        /// </summary>
        /// <param name="condi">条件</param>
        /// <param name="priority">规则优先级</param>
        /// <param name="rName">规则名称</param>
        /// <param name="ret">规则返回值</param>
        public Rule(KeyValuePair<Condition, bool> condi, int priority, string rName = "", Expression ret = null)
        {
            this.Name = string.IsNullOrEmpty(rName) ? ("规则" + priority) : rName;
            this.Priority = priority;
            this.Result = ret;
            this.Add(condi.Key, condi.Value);
        }

        /// <summary>
        /// 使用单一条件初始化规则
        /// </summary>
        /// <param name="condi">条件</param>
        /// <param name="priority">规则优先级</param>
        /// <param name="rName">规则名称</param>
        /// <param name="ret">规则返回值</param>
        public Rule(Tuple<Condition, bool> condi, int priority, string rName = "", Expression ret = null)
        {
            this.Name = string.IsNullOrEmpty(rName) ? ("规则" + priority) : rName;
            this.Priority = priority;
            this.Result = ret;
            this.Add(condi.Item1, condi.Item2);
        }

        /// <summary>
        /// 使用一组条件初始化规则
        /// </summary>
        /// <param name="condis">条件</param>
        /// <param name="priority">规则优先级</param>
        /// <param name="rName">规则名称</param>
        /// <param name="ret">规则返回值</param>
        public Rule(Dictionary<Condition, bool> condis, int priority, string rName = "", Expression ret = null)
        {
            this.Name = string.IsNullOrEmpty(rName) ? ("规则" + priority) : rName;
            this.Priority = priority;
            this.Result = ret;
            foreach (var c in condis)
            {
                this.Add(c.Key, c.Value);
            }
        }

        /// <summary>
        /// 使用一组条件初始化规则
        /// </summary>
        /// <param name="condis">条件</param>
        /// <param name="priority">规则优先级</param>
        /// <param name="rName">规则名称</param>
        /// <param name="ret">规则返回值</param>
        public Rule(List<Tuple<Condition, bool>> condis, int priority, string rName = "", Expression ret = null)
        {
            this.Name = string.IsNullOrEmpty(rName) ? ("规则" + priority) : rName;
            this.Priority = priority;
            this.Result = ret;
            foreach (var c in condis)
            {
                this.Add(c.Item1, c.Item2);
            }
        }

        /// <summary>
        /// 获取规则比较器
        /// </summary>
        /// <returns>规则比较器</returns>
        public static IComparer<Rule> GetComparer()
        {
            return aco.common.Comparison<Rule>.CreateComparer(c => c.Priority);
        }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 规则描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 规则优先级
        /// (值越小,优先级越高,0为最高)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 规则为真时应返回的结果
        /// </summary>
        public Expression Result { get; set; }

        public KeyValuePair<Condition, bool>? this[string name]
        {
            get
            {
                if (this.dict != null && dict.Count > 0)
                {
                    return this.dict.SingleOrDefault(c => c.Key.Name.Equals(name));
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 验证是否满足规则
        /// </summary>
        /// <returns>是否满足规则</returns>
        public bool IsValid()
        {
            bool valid = true;
            if (this.dict != null && dict.Count > 0)
            {
                foreach (var d in dict)
                {
                    if (d.Key.Satisfy != d.Value)
                    {
                        valid = false;
                        break;
                    }
                }
            }
            else
            {
                valid = false;
            }
            return valid;
        }
    }


}

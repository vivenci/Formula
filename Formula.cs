/*
Formula -- Prase the math formula to a System.Linq.Expressions.Expression
Written in 2019 by zyx <zyx@acortinfo.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace aco.tools.Formula
{
    public class Formula
    {
        //定义计算表达式的相关方法
        public delegate object FormulaFunction(double a1, double a2 = 0, double a3 = 0, double a4 = 0, double a5 = 0, double a6 = 0, double a7 = 0, double a8 = 0, double a9 = 0, double a10 = 0, double a11 = 0, double a12 = 0, double a13 = 0, double a14 = 0, double a15 = 0, double a16 = 0, double a17 = 0, double a18 = 0, double a19 = 0, double a20 = 0);
        private char[] nums = new char[10];

        public Formula()
        {
            nums = "0123456789".ToArray();
        }

        public Formula(string expStr) : this()
        {
            this.Params = Enumerable.Range(0, 20).Select(i => Expression.Variable(typeof(double), "x" + (i + 1))).ToArray();
            this.Expression = this.Parse(expStr);
        }

        public Formula(string expStr, IEnumerable<ParameterExpression> paras) : this()
        {
            this.Params = paras;
            this.Expression = this.Parse(expStr);
        }

        public Formula(string expStr, int paramCnt) : this(expStr)
        {
            this.Params = Enumerable.Range(0, paramCnt).Select(i => Expression.Variable(typeof(double), "x" + (i + 1))).ToArray();
        }

        /// <summary>
        /// 目标表达式
        /// </summary>
        public Expression Expression
        {
            get;
            private set;
        }

        /// <summary>
        /// 表达式中的参数枚举对象
        /// 参数格式:以x开头,后跟下标数字,下标数字从1开始(类似于x1,x2,x3...)
        /// 参数数目:默认表达式函数支持20个参数,也可以根据需求自定义表达式函数
        /// </summary>
        public IEnumerable<ParameterExpression> Params
        {
            get;
            private set;
        }

        /// <summary>
        /// 返回默认表达式函数构建对象
        /// </summary>
        /// <returns>默认表达式函数构建对象</returns>
        public FormulaFunction GetFunc()
        {
            var cvtExp = Expression.Convert(this.Expression, typeof(object));
            var ef = Expression.Lambda<FormulaFunction>(cvtExp, this.Params);
            var fun = ef.Compile();
            return fun;
        }

        /// <summary>
        /// 返回自定义输入输出的表达式函数构建对象
        /// </summary>
        /// <typeparam name="T">自定义委托类型</typeparam>
        /// <returns>自定义的表达式函数构建对象</returns>
        public T GetFunc<T>()
        {
            var cvtExp = Expression.Convert(this.Expression, typeof(object));
            var ef = Expression.Lambda<T>(cvtExp, this.Params);
            var fun = ef.Compile();
            return fun;
        }

        /// <summary>
        /// 创建并返回包含指定数目的参数枚举对象
        /// </summary>
        /// <param name="paraCnt">指定的参数数目</param>
        /// <returns>参数枚举对象</returns>
        public static IEnumerable<ParameterExpression> CreateParams(int paraCnt)
        {
            return Enumerable.Range(0, paraCnt).Select(i => Expression.Variable(typeof(double), "x" + (i + 1))).ToArray();
        }

        /// <summary>
        /// 表达式解析
        /// 判定规则:
        /// 1,字符串字符依次放入CurrentNode,当CurrentNode满足CanOperator=true,结合NewOperator进行判定:
        ///     如果NewOperator优先级小于等于CurrentNode操作优先级,则CurrentNode进行运算,运算结果存至CurrentNode.LeftLeaf,NewOperator存为CurrentNode.Node,继续接收后续字符;
        ///     如果NewOperator优先级大于CurrentNode操作优先级,则创建新节点NewNode,将CurrentNode的RightLeaf放入NewNode的LeftLeaf,NewOperator存为NewNode.Node,继续接收后续字符;
        /// 2,如果遇到括号,则创建新的Expression,同样按照规则一进行处理,并将结果归并入现有过程;遇到括号嵌套则优先处理内部块,再处理当前块;
        /// </summary>
        /// <param name="expStr">表达式字符串</param>
        /// <param name="eMgr">表达式管理器</param>
        /// <returns>解析出的表达式</returns>
        public Expression Parse(string expStr, ExpressionManager eMgr = null)
        {
            if (string.IsNullOrEmpty(expStr))
            {
                return null;
            }
            if (eMgr == null)
            {
                eMgr = new ExpressionManager();
            }
            string exp = expStr.ToLower();
            exp = expStr.Replace(" ", "");

            int i = 0;
            while (i < exp.Length)
            {
                try
                {
                    char c = exp[i];
                    //参数
                    if (c == 'x')
                    {
                        //判断参数,最多支持999个参数(x1-x999)
                        string sub = string.Empty;
                        if (exp.Length > i + 3)
                        {
                            sub = exp.Substring(i, 4);
                        }
                        else
                        {
                            sub = exp.Substring(i);
                        }
                        bool f = eMgr.CheckPara(ref sub);
                        if (f)
                        {
                            i = i + sub.Length - 1;
                            int xi = Convert.ToInt32(sub.Remove(0, 1));
                            var p = this.Params.ElementAt(xi - 1);

                            if (eMgr.Current == null)
                            {
                                eMgr.Temp = p;
                            }
                            else
                            {
                                if (eMgr.Current.LeftLeaf == null)
                                {
                                    eMgr.Current.LeftLeaf = p;
                                }
                                else
                                {
                                    eMgr.Current.RightLeaf = p;
                                }
                            }

                        }
                        else
                        {
                            throw new ArgumentException("参数格式错误.(支持的参数格式为:[x1-x999])");
                        }
                    }
                    //数字
                    else if (nums.Contains(c))
                    {
                        int nLen = 1;
                        double d = eMgr.CheckNumber(exp.Substring(i), ref nLen);
                        if (double.IsNaN(d))
                        {
                            throw new ArgumentException("表达式解析错误:未匹配到数字");
                        }
                        else
                        {
                            i = i + nLen - 1;
                            var numExp = Expression.Constant(d, typeof(double));

                            if (eMgr.Current == null)
                            {
                                eMgr.Temp = numExp;
                            }
                            else
                            {
                                if (eMgr.Current.LeftLeaf == null)
                                {
                                    eMgr.Current.LeftLeaf = numExp;
                                }
                                else
                                {
                                    eMgr.Current.RightLeaf = numExp;
                                }
                            }

                        }
                    }

                    //以下为运算符
                    else if (c.Equals('+'))
                    {
                        IOperator op = new AddOperator();
                        if (eMgr.Current == null)
                        {
                            eMgr.SetCurrent(op);
                        }
                        eMgr.Update(op);
                    }
                    else if (c.Equals('-'))
                    {
                        IOperator op = null;
                        if (eMgr.Current == null)
                        {
                            eMgr.SetCurrent(eMgr.NodeSecond);
                            if (eMgr.Current.LeftLeaf != null || eMgr.Temp != null)
                            {
                                op = new SubtractOperator();
                            }
                            else
                            {
                                op = new NegateOperator();
                            }
                        }
                        else
                        {
                            op = new SubtractOperator();
                        }
                        eMgr.Update(op);
                    }
                    else if (c.Equals('*'))
                    {
                        IOperator op = new MultiplyOperator();
                        if (eMgr.Current == null)
                        {
                            eMgr.SetCurrent(op);
                        }
                        eMgr.Update(op);
                    }
                    else if (c.Equals('/'))
                    {
                        IOperator op = new DivideOperator();
                        if (eMgr.Current == null)
                        {
                            eMgr.SetCurrent(op);
                        }
                        eMgr.Update(op);
                    }
                    else if (c.Equals('^'))
                    {
                        IOperator op = new PowerOperator();
                        if (eMgr.Current == null)
                        {
                            eMgr.SetCurrent(op);
                        }
                        eMgr.Update(op);
                    }
                    //括号处理
                    else if (c.Equals('('))
                    {
                        int bLen = 1;
                        string bExp = string.Empty;
                        string sub = exp.Substring(i);
                        for (int j = 1; j < sub.Length; j++)
                        {
                            char bc = sub[j];
                            if (bc.Equals('('))
                            {
                                bLen++;
                            }
                            else if (bc.Equals(')'))
                            {
                                bLen--;
                            }
                            if (bLen == 0)
                            {
                                bExp = sub.Substring(1, j - 1);
                                break;
                            }
                        }
                        i = i + bExp.Length + 1;
                        //计算出括号内的结果,且归并入当前节点
                        Expression subExp = Parse(bExp);

                        if ((eMgr.Current == null) && exp[i].Equals(')'))
                        {
                            eMgr.Temp = subExp;
                        }
                        else
                        {
                            eMgr.Update(subExp);
                        }
                    }
                    //比较字符
                    else if (c.Equals('<') || c.Equals('>') || c.Equals('=') || c.Equals('!'))
                    {
                        IOperator op = null;
                        int cLen = eMgr.CheckComparisonCharacter(exp.Substring(i, 2), ref op);
                        if (op != null)
                        {
                            if (eMgr.Current == null)
                            {
                                eMgr.SetCurrent(op);
                            }
                            eMgr.Update(op);
                            i = i + cLen - 1;
                        }
                    }
                    i++;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("当前字符串为:{0}", exp));
                    Trace.WriteLine(string.Format("在第{0}个字符{1}处出错.", i, exp[i]));
                    throw;
                }

            }
            //最终合并运算节点
            eMgr.MergeNode();

            if (eMgr.IsSingle())
            {
                return eMgr.Temp;
            }
            else
            {
                return eMgr.Current.GetResult();
            }
        }

    }
}

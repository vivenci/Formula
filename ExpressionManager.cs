/*
Formula -- Prase the math formula to a System.Linq.Expressions.Expression
Written in 2019 by zyx <zyx@acortinfo.com>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace aco.tools.NFormula
{
    /// <summary>
    /// 表达式管理类
    /// </summary>
    public class ExpressionManager
    {
        private ExpressionNode[] expNodeList = new ExpressionNode[4];

        public ExpressionManager()
        {
            for (int i = 0; i < expNodeList.Length; i++)
            {
                expNodeList[i] = new ExpressionNode(i);
            }
        }

        /// <summary>
        /// 用于当前节点为Null时临时存放数据
        /// </summary>
        public Expression Temp
        {
            get;
            set;
        }

        /// <summary>
        /// 当前运算的暂存结果
        /// (用于保存已完成的()内的子运算结果)
        /// </summary>
        public Expression CurrentResult
        {
            get;
            set;
        }

        /// <summary>
        /// 当前运算节点
        /// </summary>
        public ExpressionNode Current
        {
            get;
            private set;
        }

        /// <summary>
        /// 用于存放优先级为0的运算节点
        /// (可设置为当前节点)
        /// </summary>
        public ExpressionNode NodeFirst
        {
            get
            {
                return expNodeList[0];
            }
        }

        /// <summary>
        /// 用于存放优先级为1的运算节点
        /// (可设置为当前节点)
        /// </summary>
        public ExpressionNode NodeSecond
        {
            get
            {
                return expNodeList[1];
            }
        }

        /// <summary>
        /// 用于存放优先级为2的运算节点
        /// (可设置为当前节点)
        /// </summary>
        public ExpressionNode NodeThird
        {
            get
            {
                return expNodeList[2];
            }
        }

        /// <summary>
        /// 用于存放优先级为3的运算节点
        /// (可设置为当前节点)
        /// </summary>
        public ExpressionNode NodeFourth
        {
            get
            {
                return expNodeList[3];
            }
        }

        /// <summary>
        /// 用来临时存放新运算符
        /// </summary>
        public IOperator NewOperator
        {
            get;
            set;
        }

        public static Expression Apply(Expression exp, UnaryType opType = UnaryType.Negate)
        {
            UnaryOperator uop = null;
            switch (opType)
            {
                case UnaryType.Negate:
                    uop = new NegateOperator();
                    break;
                default:
                    uop = new NegateOperator();
                    break;
            }
            return uop.Apply(exp);
        }

        public static Expression Apply(Expression left, Expression right, BinaryType opType)
        {
            BinaryOperator bop = null;
            switch (opType)
            {
                case BinaryType.Greater:
                    bop = new GreaterOperator();
                    break;
                case BinaryType.GreaterEqual:
                    bop = new GreaterEqualOperator();
                    break;
                case BinaryType.Less:
                    bop = new LessOperator();
                    break;
                case BinaryType.LessEqual:
                    bop = new LessEqualOperator();
                    break;
                case BinaryType.Equal:
                    bop = new EqualOperator();
                    break;
                case BinaryType.NotEqual:
                    bop = new NotEqualOperator();
                    break;
                case BinaryType.Add:
                    bop = new AddOperator();
                    break;
                case BinaryType.Subtract:
                    bop = new SubtractOperator();
                    break;
                case BinaryType.Multiply:
                    bop = new MultiplyOperator();
                    break;
                case BinaryType.Divide:
                    bop = new DivideOperator();
                    break;
                case BinaryType.Power:
                    bop = new PowerOperator();
                    break;
                default:
                    bop = new AddOperator();
                    break;
            }

            return bop.Apply(left, right);
        }

        public static Expression Apply(Expression exp, UnaryOperator uOp)
        {
            return uOp.Apply(exp);
        }

        public static Expression Apply(Expression left, Expression right, BinaryOperator bOp)
        {
            return bOp.Apply(left, right);
        }

        /// <summary>
        /// 单数字判定
        /// </summary>
        public bool IsSingle()
        {
            if (this.Current == null && this.NewOperator == null && this.Temp != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置拥有指定操作符的为当前节点
        /// </summary>
        /// <param name="op">指定的操作符</param>
        public void SetCurrent(IOperator op)
        {
            //当前节点为空
            if (this.Current == null)
            {
                SetCurrentByPriority(op);
            }
            //当前节点已满,则将运算符置为NewOperator
            else if (this.Current.CanOperator)
            {
                this.NewOperator = op;
            }
        }

        /// <summary>
        /// 设置指定节点为当前节点
        /// </summary>
        /// <param name="node">指定的节点</param>
        public void SetCurrent(ExpressionNode node)
        {
            this.Current = node;
        }

        private void SetCurrentByPriority(IOperator op)
        {
            if (op.Priority == PriorityType.First)
            {
                this.Current = NodeFirst;
            }
            else if (op.Priority == PriorityType.Second)
            {
                this.Current = NodeSecond;
            }
            else if (op.Priority == PriorityType.Third)
            {
                this.Current = NodeThird;
            }
            else if (op.Priority == PriorityType.Fourth)
            {
                this.Current = NodeFourth;
            }
        }

        /// <summary>
        /// 获取前一节点
        /// </summary>
        /// <returns></returns>
        public ExpressionNode GetPrevNode()
        {
            if (this.Current == null)
            {
                return null;
            }
            else
            {
                int index = this.Current.Index;
                if (index == 0)
                {
                    return null;
                }
                else
                {
                    return expNodeList[index - 1];
                }
            }
        }

        /// <summary>
        /// 获取指定节点的前一节点
        /// </summary>
        /// <param name="node">指定节点</param>
        /// <returns>前一节点</returns>
        public ExpressionNode GetPrevNode(ExpressionNode node)
        {
            if (node == null)
            {
                return null;
            }
            else
            {
                int index = node.Index;
                if (index == 0)
                {
                    return null;
                }
                else
                {
                    return expNodeList[index - 1];
                }
            }
        }

        /// <summary>
        /// 从当前节点开始向前搜索待处理的节点
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="nop">新运算符</param>
        /// <returns>之前的待处理节点</returns>
        public ExpressionNode GetPrevReadyNode(ExpressionNode node)
        {
            if (node == null)
            {
                return null;
            }
            else
            {
                int index = node.Index;
                if (index == 0)
                {
                    return null;
                }
                else
                {
                    for (int i = index - 1; i >= 0; i--)
                    {
                        if (!expNodeList[i].CanOperator && (expNodeList[i].Node != null))
                        {
                            return expNodeList[i];
                        }
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取下一节点
        /// </summary>
        /// <returns>下一节点</returns>
        public ExpressionNode GetNextNode()
        {
            if (this.Current == null)
            {
                return null;
            }
            else
            {
                int index = this.Current.Index;
                if (index >= expNodeList.Length - 1)
                {
                    return null;
                }
                else
                {
                    return expNodeList[index + 1];
                }
            }
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="op">运算符</param>
        public void Update(IOperator op)
        {
            if (op == null)
            {
                return;
            }
            if (this.Current != null)
            {
                //当前节点已满,则与NewOperator进行比较
                if (this.Current.CanOperator)
                {
                    if (this.NewOperator == null)
                    {
                        this.NewOperator = op;
                    }
                    int cpr = (int)this.Current.Node.Priority;
                    int npr = (int)this.NewOperator.Priority;
                    //新运算符优先级小于等于当前节点
                    if (cpr >= npr)
                    {
                        MergeNode(this.Current, this.NewOperator);
                    }
                    //新运算符优先级大于当前节点
                    else
                    {
                        var prev = this.Current;
                        SetCurrentByPriority(this.NewOperator);
                        this.Current.LeftLeaf = prev.RightLeaf;
                        this.Current.Node = this.NewOperator;
                        this.Current.RightLeaf = null;
                        prev.RightLeaf = null;
                        this.NewOperator = null;
                    }
                }
                //当前节点未满,更新节点和左右叶子
                else
                {
                    this.Current.Node = op;
                    if (this.Current.LeftLeaf == null)
                    {
                        this.Current.LeftLeaf = this.Temp;
                    }
                    else
                    {
                        this.Current.RightLeaf = this.Temp;
                    }
                    this.Temp = null;
                }

            }
        }

        /// <summary>
        /// 更新节点
        /// (添加现有表达式为节点叶子项)
        /// </summary>
        /// <param name="exp">表达式</param>
        public void Update(Expression exp)
        {
            if (this.Current == null)
            {
                return;
            }
            if (this.Current.LeftLeaf == null)
            {
                this.Current.LeftLeaf = exp;
            }
            else if (this.Current.RightLeaf == null)
            {
                this.Current.RightLeaf = exp;
            }
        }

        /// <summary>
        /// 合并节点
        /// (遇到新运算符时执行)
        /// </summary>
        /// <param name="en">当前指向的节点</param>
        /// <param name="nOp">新操作符</param>
        /// <param name="data">关联数据</param>
        public void MergeNode(ExpressionNode en, IOperator nOp, Expression data = null)
        {
            int cpr = (int)en.Node.Priority;
            int npr = (int)nOp.Priority;
            if (cpr > npr)
            {
                if (en.RightLeaf == null)
                {
                    en.RightLeaf = data;
                }
                if (en.CanOperator)
                {
                    var ret = en.GetResult();
                    var ln = this.GetPrevReadyNode(en);
                    //ln为空,判定比较符号情况
                    if (ln == null)
                    {
                        this.SetCurrentByPriority(nOp);
                        this.Current.Node = nOp;
                        this.Current.LeftLeaf = ret;
                        this.NewOperator = null;
                    }
                    //ln不为空
                    else
                    {
                        MergeNode(ln, nOp, ret);
                    }
                }
            }
            else if (cpr == npr)
            {
                if (en.RightLeaf == null)
                {
                    en.RightLeaf = data;
                }
                if (en.CanOperator)
                {
                    var ret = en.GetResult();
                    en.LeftLeaf = ret;
                    en.Node = nOp;
                    en.RightLeaf = null;
                    this.Current = en;
                    this.NewOperator = null;
                }
            }
            else if (cpr < npr)
            {
                this.Current.Node = nOp;
                this.Current.LeftLeaf = data;
                this.Current.RightLeaf = null;
                this.NewOperator = null;
            }
        }

        /// <summary>
        /// 合并节点
        /// (用于最终处理)
        /// </summary>
        public void MergeNode()
        {
            var last = this.GetPrevReadyNode(this.Current);

            if (last == null || last.Node == null)
            {
                return;
            }
            else
            {
                int cpr = (int)this.Current.Node.Priority;
                int lpr = (int)last.Node.Priority;
                if (cpr > lpr)
                {
                    if (this.Current.CanOperator)
                    {
                        var ret = this.Current.GetResult();
                        if (last.RightLeaf == null)
                        {
                            last.RightLeaf = ret;
                            this.SetCurrent(last);
                            MergeNode();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 表达式参数判定
        /// </summary>
        /// <param name="para">参数字符串</param>
        /// <returns>是否判定为参数</returns>
        public bool CheckPara(ref string para)
        {
            string regStr = "x[1-9][0-9]*";
            Regex reg = new Regex(regStr);
            Match m = reg.Match(para);
            if (m.Success)
            {
                para = m.Value;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 表达式数字判定
        /// </summary>
        /// <param name="exp">从当前位置开始的字符串</param>
        /// <returns>获取到的数字</returns>
        public double CheckNumber(string exp, ref int len)
        {
            double num = double.NaN;
            string regStr = @"(([1-9]\d*)|0)(\.\d*)?";
            Regex reg = new Regex(regStr);
            Match m = reg.Match(exp);
            if (m.Success)
            {
                len = m.Value.Length;
                num = Convert.ToDouble(m.Value);
            }
            return num;
        }

        public int CheckComparisonCharacter(string exp, ref IOperator op)
        {
            int len = 0;
            char first = exp[0];
            if (exp.Equals(">="))
            {
                op = new GreaterEqualOperator();
                len = 2;
            }
            else if (exp.Equals("<="))
            {
                op = new LessEqualOperator();
                len = 2;
            }
            else if (exp.Equals("=="))
            {
                op = new EqualOperator();
                len = 2;
            }
            else if (exp.Equals("!="))
            {
                op = new NotEqualOperator();
                len = 2;
            }
            else if (exp.Equals("<>"))
            {
                op = new NotEqualOperator();
                len = 2;
            }
            else if (first.Equals('>'))
            {
                op = new GreaterOperator();
                len = 1;
            }
            else if (first.Equals('<'))
            {
                op = new LessOperator();
                len = 1;
            }
            else if (first.Equals('='))
            {
                op = new EqualOperator();
                len = 1;
            }
            else
            {
                throw new FormatException("比较字符格式错误. 支持的比较字符包括: >,<,=,>=,<=,==,!=");
            }
            return len;
        }

    }
}

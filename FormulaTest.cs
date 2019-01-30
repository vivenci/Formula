using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;


namespace aco.tools.Formula
{
    class FormulaTest
    {
        /// <summary>
        /// 常规算式
        /// </summary>
        void Test()
        {
            string exp = "-13+(3*(5.2-(2*x1)+(4*x2))^2/1+x3*2/x4)+x5-(x6*2/3+x1^3-6)+5^3-(2/4)";
            Formula f = new Formula(exp);
            var func = f.GetFunc();
            double ret = (double)func(1, 2, 1.5, 1, 3, 1);
        }

        /// <summary>
        /// 比较算式
        /// </summary>
        void Test2()
        {
            string exp = "10+(3*5.2-(4*x1)+x2/3)*2+x3^2-5>x4-1";
            Formula f = new Formula(exp);
            var func = f.GetFunc();
            bool ret = (bool)func(1, 2, 4, 16);
        }

        public delegate object CustomFormulaFunction(double a1, double a2 = 0, double a3 = 0);

        /// <summary>
        /// 自定义输入输出方法算式
        /// </summary>
        void Test3()
        {
            string exp = "10+(3*5.2-(4*x1)+x2/3)*2+x3^2-5";
            IEnumerable<ParameterExpression> paras = Formula.CreateParams(3);
            Formula f = new Formula(exp, paras);
            var func = f.GetFunc<CustomFormulaFunction>();
            double ret = (double)func(1, 2, 4);
        }
    }
}

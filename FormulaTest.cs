﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using aco.tools.Algorithm.PSO;

namespace aco.tools.NFormula
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

        void Test32()
        {
            string exp = "10+(3*5.2-(4*x1)+x2/3)*2+x3^2-5";
            IEnumerable<ParameterExpression> paras = Formula.CreateParams(3);
            Formula f = new Formula(exp, paras);
            LambdaExpression le = Expression.Lambda(f.Expression, paras);
            Delegate de = le.Compile();
            var ra = de.DynamicInvoke(1, 2, 4);
        }

        void Test33()
        {
            string exp = "10+(3*5.2-(4*x1)+x2/3)*2+x3^2-5";
            IEnumerable<ParameterExpression> paras = Formula.CreateParams(3);
            Formula f = new Formula(exp, paras);
            Delegate de = f.GetDelegate();
            object[] args = new object[] { 1, 2, 4 };
            var rb = de.DynamicInvoke(args);
        }

        void Test4()
        {
            string exp = "((0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (((103.404773618*0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+95.9613688011329*0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+116.602788396857*0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+102.316306273637*0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))) + (0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (((103.846714455146*0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+93.2178846916325*0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+80.0003698623901*0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+102.420874484433*0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))) + (0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (((94.1235883623508*0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+79.3191313242131*0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+71.9259094604732*0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+70.6935374849044*0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))) + (0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * ((((103.404773618*0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+95.9613688011329*0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+116.602788396857*0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+102.316306273637*0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))) + (((103.846714455146*0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+93.2178846916325*0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+80.0003698623901*0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+102.420874484433*0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8))))) * 0.933 + (0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * ((((103.404773618*0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+95.9613688011329*0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+116.602788396857*0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+102.316306273637*0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.005*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1883*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.8509*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.0066*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))) + (((94.1235883623508*0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+79.3191313242131*0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+71.9259094604732*0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+70.6935374849044*0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8))))) * 1.0232 + (0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * (0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)) * ((((103.846714455146*0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+93.2178846916325*0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+80.0003698623901*0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+102.420874484433*0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.007*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.3115*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.011*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.006*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))) + (((94.1235883623508*0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+79.3191313242131*0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+71.9259094604732*0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+70.6935374849044*0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)))/((0.988*(x1/692.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.5002*(x2/726.5)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.1381*(x3/844.9)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8)+0.9874*(x4/632.8)/(x1/692.8+x2/726.5+x3/844.9+x4/632.8))))) * 1.1021)>=95.2";
            Formula f = new Formula(exp);
            var func = f.GetFunc();
            object ret = (bool)func(1, 1, 1, 1);
        }

        void Test5()
        {
            string exp = "1+2*3^4-5+6/2";
            Formula f = new Formula(exp, "noParamFormula");
            var func = f.GetNoParamFunc();
            object ret = (double)func();
        }

        void Test6()
        {
            string exp = "-13+(3*(5.2-(2*x1)+(4*x2))^2/1+x3*2/x4)+x5-(x6*2/3+x1^3-6)+5^3-(2/4)";
            IEnumerable<ParameterExpression> paras = Formula.CreateParams(6);
            Formula f = new Formula(exp, paras);
            object[] args = new object[] { 1, 2, 1.5, 1, 3, 1 };
            double ret = (double)f.GetResult(args);
        }

        void Test7()
        {
            string exp = "(61^1.25*x1/0.7315/x4+45^1.25*x2/0.688/x4+25^1.25*x3/0.871/x4)^0.8>=35";
            IEnumerable<ParameterExpression> paras = Formula.CreateParams(4);
            Formula f = new Formula(exp, paras);
        }
    }
}

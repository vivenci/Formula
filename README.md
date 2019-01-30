# Formula
Prase the math formula to a System.Linq.Expressions.Expression

You can use it like this:

e.g.1:

  // By default, expressions are allowed to contain less than 20 parameters.
  
  
  string exp = "-13+(3*(5.2-(2*x1)+(4*x2))^2/1+x3*2/x4)+x5-(x6*2/3+x1^3-6)+5^3-(2/4)";  
  
  Formula f = new Formula(exp);
  
  var func = f.GetFunc();
  
  double ret = (double)func(1, 2, 1.5, 1, 3, 1);
  
e.g.2:

  // Of course, you can also use custom delegate to decide how many parameters you want to use. 

  
  public delegate object CustomFormulaFunction(double a1, double a2 = 0, double a3 = 0);
  
  
  string exp = "10+(3*5.2-(4*x1)+x2/3)*2+x3^2-5";
  
  IEnumerable<ParameterExpression> paras = Formula.CreateParams(3);
  
  Formula f = new Formula(exp, paras);
  
  var func = f.GetFunc<CustomFormulaFunction>();
  
  double ret = (double)func(1, 2, 4);
  

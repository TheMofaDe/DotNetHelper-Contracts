using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DotNetHelper_Contracts.Enum.DataSource;
using DotNetHelper_Contracts.Extension;


namespace DotNetHelper_Contracts.Helpers
{
    public class Linq2SqlGenerator : ExpressionVisitor
    {

        private readonly StringBuilder _sqlBuilder;

        private readonly Dictionary<ExpressionType, string> _simpleOperatorRegistry;
        private readonly Dictionary<Type, string> _simpleRightExpressingMapping;

        public Linq2SqlGenerator(StringBuilder sqlBuilder)
        {

            _sqlBuilder = sqlBuilder;

            _simpleOperatorRegistry = new Dictionary<ExpressionType, string>
            {
                {ExpressionType.Add, "+"},
                {ExpressionType.AddChecked, "+"},
                {ExpressionType.And, "&"},
                {ExpressionType.AndAlso, "AND"},
                {ExpressionType.Divide, "/"},
                {ExpressionType.ExclusiveOr, "^"},
                {ExpressionType.GreaterThan, ">"},
                {ExpressionType.GreaterThanOrEqual, ">="},
                {ExpressionType.LessThan, "<"},
                {ExpressionType.LessThanOrEqual, "<="},
                {ExpressionType.Modulo, "%"},
                {ExpressionType.Multiply, "*"},
                {ExpressionType.MultiplyChecked, "*"},
                {ExpressionType.Or, "|"},
                {ExpressionType.OrElse, "OR"},
                {ExpressionType.Subtract, "-"},
                {ExpressionType.SubtractChecked, "-"},
                {ExpressionType.Equal, "="},
                {ExpressionType.NotEqual, "<>"},
                {ExpressionType.MemberAccess, ""},
            };
            _simpleRightExpressingMapping = new Dictionary<Type, string>
            {
                {typeof(string), "'"},
                {typeof(int), ""},
                {typeof(bool), ""},
                {typeof(DateTime), "'"},
                {typeof(Guid), "'"},
            };
        }



        public string LinqToSqlWhereClause<T>(Expression<Func<T, bool>> expression, DataBaseType type)
            where T : class
        {
            var whereClause = " WHERE ";

            if (!expression.Parameters.IsNullOrEmpty())
            {

                var expressionObject = expression.Parameters.First();
                var parameterName = expression.Parameters.First().Name;

                if (expression.Body is Expression currentExpression)
                {
                    // Works for Execute(x => x.UserAuthentication(something))
                    // where something must be a constant value, a variable,
                    // a field, a property
                    var param = expression.Parameters[0]; //myparameter to cast





                    //var si = new IEnumerable<ParameterExpression>() { };
                    ////////////var run = Expression.Lambda(currentExpression, expression.Parameters).Compile();
                    ////////////var tar = run.Target;
                    ////////////var mi = run.Method;
                    ////////////var r = run.DynamicInvoke();

                    ////////////var body = Expression.TypeAs(expression.Body, typeof(T));

                    ////////////  // If you have a type for the parameter, replace it here:
                    ////////////  // object -> yourtype
                    ////////////  var obj = Expression.Lambda<Func<T>>(expression.Body,param).Compile()();
                    ////////////var source = Expression.Parameter(typeof(T),"source");

                    // (T)expressionObject.Value
                    // var obj = Expression.Lambda<Func<T>>(expressionObject).Compile()();

                    var isFirstTime = true;
                    var holdOff = "";
                    while (currentExpression != null)
                    {

                        var memberExpression = currentExpression as MemberExpression;
                        var binaryExpression = currentExpression as BinaryExpression;
                        var constantExression = currentExpression as ConstantExpression;
                        var unaryExpression = currentExpression as UnaryExpression;
                        var methodCallExpression = currentExpression as MethodCallExpression;
                        var isLast = binaryExpression?.Left == null;
                        if (isLast)
                        {
                            _sqlBuilder.Append(holdOff);
                        }
                        //  GetSQLFromMember(memberExpression);
                        if (binaryExpression != null)
                        {
                            isFirstTime = true;
                            var left = binaryExpression.Left;
                            var right = binaryExpression.Right;
                            var t = right as BinaryExpression;


                            if (binaryExpression.Left is MemberExpression m1)
                            {

                                GetSQLFromMember(m1, null, binaryExpression);

                            }




                            while (t != null)
                            {
                                var sqlOperator = GetRegisteredOperatorString(binaryExpression.NodeType);
                                if (isFirstTime)
                                {
                                    holdOff = $" {sqlOperator} ";
                                    isFirstTime = false;
                                }
                                else
                                {

                                }
                                var b1 = t.Left as MemberExpression;
                                var b2 = t.Right as MemberExpression;

                                GetSQLFromMember(b1, b2, t);

                                t = t.Right as BinaryExpression;
                            }
                            memberExpression = right as MemberExpression;
                            var binaryExpression23 = right as BinaryExpression;
                            constantExression = right as ConstantExpression;
                            unaryExpression = right as UnaryExpression;
                            currentExpression = binaryExpression.Left;

                        }
                        else
                        {
                            currentExpression = null;
                            if (isFirstTime && memberExpression != null)
                            {
                                var middle = GetRegisteredOperatorString(memberExpression.NodeType);
                                var propertyinfo = memberExpression?.Member as PropertyInfo;
                                var fieldinfo = memberExpression?.Member as FieldInfo;

                                if (propertyinfo != null)
                                {
                                    _sqlBuilder.Append($" ( {propertyinfo.Name} {middle} {0}) ");

                                }

                            }
                        }


                        currentExpression = Visit(currentExpression);
                        //if (memberEX != null)
                        //{
                        //if (i == 0) // The Last MemberExpression Is Not Wanted After We Find A Least One Binary Expression & When MemberExpression Only Comes In 1'ss
                        //whereClause += CleanUpMemberExpression(memberEX, type) + andSO;
                        //currentExpression = null;
                        //}
                        //if (binaryEX != null)
                        //{
                        //whereClause += CleanUpRightNodyByType(parameterName, binaryEX, type) + andSO;
                        //currentExpression = binaryEX.Left;
                        //}
                        //if (memberEX == null && binaryEX == null)
                        //{
                        //currentExpression = null;
                        //TheMoFaDeDI.Logger.Log("JOSEPH A NEW EXPRESSION IS FOUND IN LINQ 2 SQL");
                        //}
                        //i++;
                    }
                    whereClause = whereClause.ReplaceLastOccurrance(" AND ", string.Empty, StringComparison.Ordinal);
                }

            }

            return whereClause;
        }




        private static KeyValuePair<Type, object>[] ResolveArgs<T>(Expression<Func<T, object>> expression)
        {
            var body = (System.Linq.Expressions.MethodCallExpression)expression.Body;
            var values = new List<KeyValuePair<Type, object>>();

            foreach (var argument in body.Arguments)
            {
                var exp = ResolveMemberExpression(argument);
                var type = argument.Type;

                var value = GetValue(exp);

                values.Add(new KeyValuePair<Type, object>(type, value));
            }

            return values.ToArray();
        }

        public static MemberExpression ResolveMemberExpression(Expression expression)
        {

            if (expression is MemberExpression)
            {
                return (MemberExpression)expression;
            }
            else if (expression is UnaryExpression)
            {
                // if casting is involved, Expression is not x => x.FieldName but x => Convert(x.Fieldname)
                return (MemberExpression)((UnaryExpression)expression).Operand;
            }
            else
            {
                throw new NotSupportedException(expression.ToString());
            }
        }

        private static object GetValue(MemberExpression exp)
        {

            // expression is ConstantExpression or FieldExpression
            if (exp.Expression is ConstantExpression)
            {
                return (((ConstantExpression)exp.Expression).Value)
                    .GetType()
                    .GetField(exp.Member.Name)
                    .GetValue(((ConstantExpression)exp.Expression).Value);
            }
            else if (exp.Expression is MemberExpression)
            {
                return GetValue((MemberExpression)exp.Expression);
            }

            else
            {
                throw new NotImplementedException();
            }
        }



        public void GetSQLFromMember(MemberExpression left, MemberExpression right, BinaryExpression rightValue)
        {
            if (left != null)
            {

                if (right != null)
                {
                    var middle = GetRegisteredOperatorString(rightValue.NodeType);
                    var parmeterExpression = left.Expression as ParameterExpression;


                    var propertyinfo = left?.Member as PropertyInfo;
                    var fieldinfo = left?.Member as FieldInfo;

                    if (propertyinfo != null)
                    {
                        _sqlBuilder.Append($" ( {propertyinfo.Name} {middle} {CleanUpShit(Expression.Lambda(right).Compile().DynamicInvoke(), rightValue.Type)}) ");

                    }
           
                }
                else
                {
                    var middle = GetRegisteredOperatorString(rightValue.NodeType);
                    var parmeterExpression = left.Expression as ParameterExpression;

                    var propertyinfo = left?.Member as PropertyInfo;
                    var fieldinfo = left?.Member as FieldInfo;
                    //value = property?.GetValue(expressionObject, null);
                    if (propertyinfo != null)
                    {
                        if (rightValue.Conversion == null)
                        {
                            var value = rightValue.Right.ToString();
                            if (rightValue.Right.Type == typeof(string))
                            {
                                _sqlBuilder.Append($" ( {propertyinfo.Name} {middle} {value.ReplaceFirstOccurrance("\"", "'", StringComparison.Ordinal).ReplaceLastOccurrance("\"", "'", StringComparison.Ordinal)} ) ");
                            }
                            else if (rightValue.Right.Type == typeof(bool))
                            {
                                _sqlBuilder.Append($" ( {propertyinfo.Name} {middle} {value.ToInt()} ) ");
                            }
                        }
                        else
                        {
                            _sqlBuilder.Append($" ( {propertyinfo.Name} {middle} {CleanUpShit(rightValue.Conversion.Compile().DynamicInvoke(), rightValue.Type)} ) ");
                        }
                    }
                }

            }
        }



        private string CleanUpShit(object value, Type type)
        {
            var singleQuotes = _simpleRightExpressingMapping.GetValueOrDefault(type, "");
            return value == null ? $"NULL" : $"{singleQuotes}{value}{singleQuotes}";
        }
        private static string CleanUpRightNodyByType(string parameterName, BinaryExpression binaryEx, DataBaseType type)
        {
            return "";
        }

        private static string CleanUpMemberExpression(MemberExpression memberEx, DataBaseType type)
        {
            return "";
        }
        private string GetRegisteredOperatorString(ExpressionType nodeType)
        {
            if (!_simpleOperatorRegistry.TryGetValue(nodeType, out var operatorString))
                throw new NotSupportedException("The binary operator '" + nodeType + "' is not supported.");
            return operatorString;
        }
    }
}

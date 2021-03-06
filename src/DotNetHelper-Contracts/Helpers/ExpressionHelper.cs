﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DotNetHelper_Contracts.Extension;

namespace DotNetHelper_Contracts.Helpers
{
    public static class ExpressionHelper
    {

        public static List<string> GetPropertyNamesFromExpressions<T>(this Expression<Func<T, object>>[] expression)
        {
            var outputFields = new List<string>() { };

            if (!expression.IsNullOrEmpty())
                expression.ForEach(delegate (Expression<Func<T, object>> expression1)
                {
                    outputFields.AddRange(expression1.GetPropertyNamesFromExpression());
                });
            return outputFields;
        }

        public static List<string> GetPropertyNamesFromExpression<T>(this Expression<Func<T, object>> expression)
        {
            var outputFields = new List<string>() { };
            
            if (expression?.Body is NewArrayExpression bodies)
            {
                foreach (var body in bodies.Expressions)
                {

                    var test = body as MemberExpression;
                    if (test != null)
                    {
                        outputFields.Add(test.Member.Name);
                    }
                    else
                    {
                        var item = body as UnaryExpression;
                        var operand = item?.Operand as MemberExpression;
                        outputFields.Add(operand?.Member.Name);
                    }

                    if (test != null)
                    {
                        outputFields.Add(test.Member.Name);
                    }
                }
            }
            else
            {
                if (expression?.Body is Expression oneBody)
                {
                    if (oneBody is MemberExpression test)
                    {
                        outputFields.Add(test.Member.Name);
                    }
                    else
                    {
                        var item = oneBody as UnaryExpression;
                        var operand = item?.Operand as MemberExpression;
                        outputFields.Add(operand?.Member.Name);
                    }
                }
                else
                {
                    return new List<string>(){};
                  //  throw (new BadCodeException($"The Parameter {expression} Can Not Be Null"));
                }
            }
            return outputFields;
        }


    
    }
}

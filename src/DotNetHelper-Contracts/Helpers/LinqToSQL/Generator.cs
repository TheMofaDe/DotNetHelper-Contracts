using System;
using System.Linq.Expressions;

namespace DotNetHelper_Contracts.Helpers.LinqToSQL
{
    public class Generator
    {
      
   

        public static WherePart WhereSql<T>(Expression<Func<T, bool>> expression)
        {
            var type = typeof(T);
            var wherePart = ToSql(expression);
            wherePart.Sql = $" WHERE {wherePart.Sql}";
            return wherePart;
        }

        public static WherePart ToSql<T>(Expression<Func<T, bool>> expression)
        {
            var i = 1;
            return LinqToSqlHelpers.Recurse(ref i, expression.Body, isUnary: true);
        }
        
    }
    
}

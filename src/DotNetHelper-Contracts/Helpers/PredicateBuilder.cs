using System;
using System.Linq.Expressions;

namespace DotNetHelper_Contracts.Helpers
{
    public class PredicateBuilder<T>
    {
        public static PredicateBuilder<T> New()
        {
            return new PredicateBuilder<T>();
        }

        public Expression<Func<T, bool>> Predicate { get; set; }

        public PredicateBuilder<T> Or(Expression<Func<T, bool>> or)
        {
            if (Predicate == null)
            {
                Predicate = or;
            }
            else
            {
                var invokedExpr = Expression.Invoke(or, Predicate.Parameters);
                Predicate = Expression.Lambda<Func<T, bool>>(Expression.Or(Predicate.Body, invokedExpr), Predicate.Parameters);
            }
            return this;
        }

        public PredicateBuilder<T> And(Expression<Func<T, bool>> and)
        {
            if (Predicate == null)
            {
                Predicate = and;
            }
            else
            {
                var invokedExpr = Expression.Invoke(and, Predicate.Parameters);
                Predicate = Expression.Lambda<Func<T, bool>>(Expression.And(Predicate.Body, invokedExpr), Predicate.Parameters);
            }
            return this;
        }
        /// <summary>
        /// Negates the predicate.
        /// </summary>
        public Expression<Func<T, bool>> Not( Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        public PredicateBuilder<T> Or(bool apply, Expression<Func<T, bool>> or)
        {
            return apply ? Or(or) : this;
        }

        public PredicateBuilder<T> And(bool apply, Expression<Func<T, bool>> and)
        {
            return apply ? And(and) : this;
        }
    }
}

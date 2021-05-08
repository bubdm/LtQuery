using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace LtQuery
{
    using Mutables;
    using QueryElements;

    public static class FluentExtensions
    {

        #region Extensions with LINQ

        // Where
        public static QueryFluent<TEntity> Where<TEntity>(this QueryFluent<TEntity> _this, Expression<Func<TEntity, bool>> predicate)
        {
            var body = predicate.Body;
            switch (body)
            {
                case BinaryExpression binary:
                    var left = binary.Left as MemberExpression ?? throw new ArgumentException("Lhs must be PropertyAccess", nameof(predicate));

                    // Usually MethodCallExpression is passed.
                    // Sometimes UnaryExpression is passed even though the same syntax.
                    MethodCallExpression right;
                    switch (binary.Right)
                    {
                        case MethodCallExpression methodExp:
                            right = methodExp;
                            break;
                        case UnaryExpression unaryExp:
                            right = unaryExp.Operand as MethodCallExpression ?? throw new ArgumentException("Rhs must be Lt.Arg<>()", nameof(predicate));
                            break;
                        default:
                            throw new ArgumentException("Rhs must be Lt.Arg<>()", nameof(predicate));
                    }

                    string parameterName;
                    if (right.Arguments.Count == 0)
                    {
                        parameterName = left.Member.Name;
                    }
                    else
                    {
                        var arg = right.Arguments[0] as ConstantExpression ?? throw new ArgumentException("Rhs must be Lt.Arg<>()", nameof(predicate));
                        parameterName = (string)arg.Value ?? throw new ArgumentException("Lt.Arg<>() argment must not null", nameof(predicate));
                    }

                    switch (binary.NodeType)
                    {
                        case ExpressionType.Equal:
                            return _this.Where(new EqualOperator(new Property<TEntity>(left.Member.Name), new Parameter(parameterName)));
                        default:
                            throw new NotSupportedException($"not supported convert [{binary.NodeType}]");
                    }
                default:
                    throw new NotSupportedException($"not supported [{body}]");
            }
        }


        // OrderBy/ThenBy
        public static QueryFluent<TEntity> OrderBy<TEntity, TProperty>(this QueryFluent<TEntity> _this, Expression<Func<TEntity, TProperty>> selector)
            => _this.OrderBy(getProperty(selector).Name);
        public static QueryFluent<TEntity> OrderByDescending<TEntity, TProperty>(this QueryFluent<TEntity> _this, Expression<Func<TEntity, TProperty>> selector)
            => _this.OrderByDescending(getProperty(selector).Name);
        public static QueryFluent<TEntity> ThenBy<TEntity, TProperty>(this QueryFluent<TEntity> _this, Expression<Func<TEntity, TProperty>> selector)
            => _this.ThenBy(getProperty(selector).Name);
        public static QueryFluent<TEntity> ThenByDescending<TEntity, TProperty>(this QueryFluent<TEntity> _this, Expression<Func<TEntity, TProperty>> selector)
            => _this.ThenByDescending(getProperty(selector).Name);

        #endregion

        #region ILtConnection

        public static int Count<TEntity>(this ILtConnection _this, QueryFluent<TEntity> query)
            => _this.Count(query.ToImmutable());
        public static int Count<TEntity, TParameter>(this ILtConnection _this, QueryFluent<TEntity> query, TParameter values)
            => _this.Count(query.ToImmutable(), values);

        public static IEnumerable<TEntity> Select<TEntity>(this ILtConnection _this, QueryFluent<TEntity> query)
            => _this.Select(query.ToImmutable());
        public static IEnumerable<TEntity> Select<TEntity, TParameter>(this ILtConnection _this, QueryFluent<TEntity> query, TParameter values)
            => _this.Select(query.ToImmutable(), values);

        public static TEntity Single<TEntity>(this ILtConnection _this, QueryFluent<TEntity> query)
            => _this.Single(query.ToImmutable());
        public static TEntity Single<TEntity, TParameter>(this ILtConnection _this, QueryFluent<TEntity> query, TParameter values)
            => _this.Single(query.ToImmutable(), values);

        public static TEntity First<TEntity>(this ILtConnection _this, QueryFluent<TEntity> query)
            => _this.First(query.ToImmutable());
        public static TEntity First<TEntity, TParameter>(this ILtConnection _this, QueryFluent<TEntity> query, TParameter values)
            => _this.First(query.ToImmutable(), values);

        #endregion

        private static MemberInfo getProperty<T1, T2>(Expression<Func<T1, T2>> selector)
        {
            if (!(selector.Body is MemberExpression))
                throw new ArgumentException();
            var body = (MemberExpression)selector.Body;
            return body.Member;
        }
    }
}

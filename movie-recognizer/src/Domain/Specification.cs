using System.Linq.Expressions;

namespace Domain;

public sealed class Specification<T>(Expression<Func<T, bool>> expression)
{
    private readonly Expression<Func<T, bool>> _expression = expression;

    public static implicit operator Expression<Func<T, bool>>(Specification<T> spec)
        => spec._expression;

    public static Specification<T> operator &(Specification<T> spec1, Specification<T> spec2)
        => new(spec1._expression.And(spec2._expression));
        
    public static Specification<T> operator |(Specification<T> spec1, Specification<T> spec2)
        => new(spec1._expression.Or(spec2._expression));

    public static Specification<T> operator !(Specification<T> spec)
        => new(spec._expression.Not());
}

public static class Specification
{
    public static Specification<T> ById<T>(Guid id) where T : BaseEntity => new(entity => entity.Id == id);
}

file static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    {
        var invokedExpression = Expression.Invoke(expression2, expression1.Parameters);

        return Expression.Lambda<Func<T, bool>>(Expression.And(expression1.Body, invokedExpression), expression1.Parameters);
    }
    
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    {
        var invokedExpression = Expression.Invoke(expression2, expression1.Parameters);

        return Expression.Lambda<Func<T, bool>>(Expression.Or(expression1.Body, invokedExpression), expression1.Parameters);
    }
    
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
        return Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);
    }
}
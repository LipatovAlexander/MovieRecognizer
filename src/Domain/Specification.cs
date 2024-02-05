using System.Linq.Expressions;
using Domain.Extensions;

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
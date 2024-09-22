using System.Linq.Expressions;
using System.Reflection;

namespace XAF.Core.ExtensionMethods;

/// <summary>
/// Several extensions for expressions
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Gets the property name from the expression
    /// </summary>
    /// <typeparam name="T">the typ which contains the property</typeparam>
    /// <typeparam name="TProperty">the type of the property</typeparam>
    /// <param name="expression">the property expression</param>
    /// <returns></returns>
    public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression)
    {
        var propInfo = expression.GetMember();
        return propInfo.Name;
    }

    /// <summary>
    /// Gets the property info from the expression
    /// </summary>
    /// <typeparam name="T">the typ which contains the property</typeparam>
    /// <typeparam name="TProperty">the type of the property</typeparam>
    /// <param name="expression">the property expression</param>
    /// <returns>the Property info</returns>
    /// <exception cref="ArgumentException"></exception>
    public static PropertyInfo GetPropertyInfo<T, TProperty>(this Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression member)
        {
            throw new ArgumentException($"Expression refers to a method");
        }

        var propInfo = member.Member as PropertyInfo;

        return propInfo ?? throw new ArgumentException("Expression refers to a field");
    }


    /// <summary>
    /// Gets the member info from the expression
    /// </summary>
    /// <typeparam name="T">the typ which contains the property</typeparam>
    /// <typeparam name="TProperty">the type of the property</typeparam>
    /// <param name="expression">the property expression</param>
    /// <returns>the member info</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>    
    public static MemberInfo GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression)
    {
        if (RemoveUnary(expression.Body) is not MemberExpression memberExp)
        {
            throw new ArgumentException("Expressions refers to a method");
        }

        var currentExpr = memberExp.Expression ?? throw new InvalidOperationException("Expression was null");

        while (true)
        {
            currentExpr = RemoveUnary(currentExpr!);

            if (currentExpr != null && currentExpr.NodeType == ExpressionType.MemberAccess)
            {
                currentExpr = ((MemberExpression)currentExpr).Expression;
            }
            else
            {
                break;
            }
        }

        if (currentExpr == null || currentExpr.NodeType != ExpressionType.Parameter)
        {
            throw new InvalidOperationException();
        }

        return memberExp.Member;
    }

    private static Expression RemoveUnary(Expression toUnwrap)
    {
        return toUnwrap is UnaryExpression expression ? expression.Operand : toUnwrap;
    }
}


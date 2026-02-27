using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Extensions
{
    public class ExpressionExtensions
    {
        public static LambdaExpression GenerateMethodCallDelegate(Expression instance, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();

            var parameterExpressions = 
                parameters.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToArray();

            var call = Expression.Call(instance, methodInfo, parameterExpressions);
            return Expression.Lambda(call, parameterExpressions);
        }
    }
}
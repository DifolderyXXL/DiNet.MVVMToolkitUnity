using System;
using System.Linq.Expressions;
using System.Reflection;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Generation
{
    public interface IDisposableBindGenerator
    {
        public Expression Generate(VDelegateAttribute attribute, Type viewModelType, Expression view, Expression viewModel, MethodInfo method);
    }
}
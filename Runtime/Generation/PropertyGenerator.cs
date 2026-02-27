using System;
using System.Linq.Expressions;
using System.Reflection;
using _src.Scripts.Core.Framework.MVVMToolkit.Exceptions;
using _src.Scripts.Core.Framework.MVVMToolkit.Extensions;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Generation
{
    public class PropertyGenerator : IDisposableBindGenerator
    {
        private static Lazy<MethodInfo> _wrapperWrapMethod =
            new(() => typeof(PropertyWrapper).GetMethod(nameof(PropertyWrapper.Wrap)));
        
        private static Lazy<MethodInfo> _valueTypeMethodInfo =
            new(() => typeof(IPropertyWrap).GetMethod(nameof(IPropertyWrap.Subscribe)));
        
        private static Lazy<PropertyInfo> _valueTypePropertyInfo =
            new(() => typeof(IPropertyWrap).GetProperty(nameof(IPropertyWrap.ValueType)));
        
        private readonly PropertyWrapper _propertyWrapper;

        public PropertyGenerator(PropertyWrapper propertyWrapper)
        {
            _propertyWrapper = propertyWrapper;
        }

        public Expression Generate(VDelegateAttribute attribute, Type viewModelType, Expression view, Expression viewModel, MethodInfo method)
        {
            var propertyName = attribute as VPropertyAttribute;
            
            var property = viewModelType.GetProperty(propertyName.Name);
            if (property == null)
            {
                throw new VPropertyNotFoundException();
            }
            
            var action = ExpressionExtensions.GenerateMethodCallDelegate(view, method);
            
            return GenerateBinding(property, action, viewModel, propertyName.CallOnInit);
        }

        private Expression GenerateBinding(
            PropertyInfo property,
            Expression viewMethodAction,
            Expression viewModel, 
            bool callOnInit)
        {
            var wrappedProperty = GetWrappedProperty(property, viewModel);
            
            var subscription = Expression.Call(
                wrappedProperty, 
                _valueTypeMethodInfo.Value,
                viewMethodAction,
                Expression.Constant(callOnInit, typeof(bool))
                );

            return subscription;
        }

        private Expression GetWrappedProperty(
            PropertyInfo property,
            Expression viewModelParameter)
        {
            return Expression.Call(
                Expression.Constant(_propertyWrapper),
                _wrapperWrapMethod.Value,
                Expression.Convert(viewModelParameter, typeof(object)),
                Expression.Constant(property, typeof(PropertyInfo))
            );
        }
    }
}
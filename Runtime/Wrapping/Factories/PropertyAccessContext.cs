using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories
{
    public class PropertyAccessContext
    {
        public static PropertyAccessContext Instance = new();
        
        private readonly Dictionary<Type, Dictionary<PropertyInfo, Func<object, object>>> _accessors = new();
        public Func<object, object> GetOrGeneratePropertyAccess(Type instanceType, PropertyInfo propertyInfo)
        {
            if (!_accessors.ContainsKey(instanceType))
            {
                _accessors.Add(instanceType, new());
            }
            
            var target = _accessors[instanceType];
            if (target.TryGetValue(propertyInfo, out var access))
            {
                return access;
            }
            
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var property = Expression.Property(Expression.Convert(instanceParameter, instanceType), propertyInfo);
            
            var lambda = Expression.Lambda<Func<object, object>>(
                Expression.Convert(property, typeof(object)), 
                instanceParameter);

            var comp = lambda.Compile();
            target.Add(propertyInfo, comp);
            return comp;
        }
    }
}
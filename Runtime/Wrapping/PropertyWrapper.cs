using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _src.Scripts.Core.Framework.MVVMToolkit.Exceptions;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping
{
    public class PropertyWrapper
    {
        private List<IPropertyWrapFactory> _wrappers = new();

        public void Register(IPropertyWrapFactory wrapper)
        {
            _wrappers.Add(wrapper);
        }
        
        public IPropertyWrap Wrap(object dataContext, PropertyInfo property)
        {
            var arg = new WrapArgs() { Property = property, ViewModel = dataContext };
            var wrapper = _wrappers.FirstOrDefault(x => x.Validate(arg));
            if (wrapper == null)
                throw new InvalidTypeToWrapException();

            return wrapper.GetWrap(arg);
        }
    }
    
    

    [AttributeUsage(AttributeTargets.Class)]
    sealed class TypeWrapperAttribute : Attribute
    {
        public Type Type { get; }

        public TypeWrapperAttribute(Type type)
        {
            Type = type;
        }
    }
}
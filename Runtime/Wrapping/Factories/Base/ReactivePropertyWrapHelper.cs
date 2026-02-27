using System;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.BaseTypes;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories.Base
{
    public class ReactivePropertyWrapHelper
    {
        public static IPropertyWrap GeneratePropertyWrap(Type type, object propertyInstance)
        {
            var reactivePropertyType = typeof(ReactivePropertyWrap<>).MakeGenericType(type);
            return (IPropertyWrap)Activator.CreateInstance(reactivePropertyType, propertyInstance);
        }
    }
    
    public class NotifyCollectionWrapHelper
    {
        public static IPropertyWrap GeneratePropertyWrap(object propertyInstance)
        {
            var reactivePropertyType = typeof(NotifyCollectionWrap);
            return (IPropertyWrap)Activator.CreateInstance(reactivePropertyType, propertyInstance);
        }
    }
}
using System;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.BaseTypes;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories.Base
{
    public class AccessPropertyWrapHelper
    {
        public static IPropertyWrap GeneratePropertyWrap(Type type, WrapArgs args)
        {
            var accessPropertyType = typeof(AccessPropertyWrap<>).MakeGenericType(type);
            return (IPropertyWrap)Activator.CreateInstance(accessPropertyType, args);
        }
    }
}
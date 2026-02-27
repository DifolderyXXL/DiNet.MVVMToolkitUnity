using System;
using _src.Scripts.Core.Framework.MVVMToolkit.Exceptions;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories.Base
{
    public class AccessorPropertyWrapFactory : IPropertyWrapFactory
    {
        public Type TargetType => null;
        public bool Validate(WrapArgs args)
        {
            return true;
        }

        public IPropertyWrap GetWrap(WrapArgs args)
        {
            if (Validate(args))
            {
                var wrap = AccessPropertyWrapHelper.GeneratePropertyWrap(args.Property.PropertyType, args);
                
                return wrap;
            }

            throw new InvalidTypeToWrapException();
        }
    }
}
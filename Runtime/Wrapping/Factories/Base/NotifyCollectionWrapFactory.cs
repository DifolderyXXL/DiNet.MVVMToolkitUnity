using System;
using System.Collections.Specialized;
using _src.Scripts.Core.Framework.MVVMToolkit.Exceptions;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories.Base
{
    public class NotifyCollectionWrapFactory : IPropertyWrapFactory
    {
        public Type TargetType => typeof(INotifyCollectionChanged);
        public bool Validate(WrapArgs args)
        {
            var type = args.Property.PropertyType;
            return TargetType.IsAssignableFrom(type);
        }

        public IPropertyWrap GetWrap(WrapArgs args)
        {
            if (Validate(args))
            {
                var access =
                    PropertyAccessContext.Instance.GetOrGeneratePropertyAccess(args.ViewModel.GetType(), args.Property);
                
                var wrap = NotifyCollectionWrapHelper.GeneratePropertyWrap(access.Invoke(args.ViewModel));
                
                return wrap;
            }

            throw new InvalidTypeToWrapException();
        }
    }
}
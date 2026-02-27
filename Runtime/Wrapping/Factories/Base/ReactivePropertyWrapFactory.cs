using System;
using _src.Scripts.Core.Framework.MVVMToolkit.Exceptions;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.BaseTypes;
using R3;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories.Base
{
    public class ReactivePropertyWrapFactory : IPropertyWrapFactory
    {
        public Type TargetType => typeof(ReactiveProperty<>);
        public bool Validate(WrapArgs args)
        {
            var type = args.Property.PropertyType;
            return type.IsGenericType 
                   && type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>);
        }

        public IPropertyWrap GetWrap(WrapArgs args)
        {
            if (Validate(args))
            {
                var valueType = args.Property.PropertyType.GetGenericArguments()[0];
                
                var access =
                    PropertyAccessContext.Instance.GetOrGeneratePropertyAccess(args.ViewModel.GetType(), args.Property);
                
                var wrap = ReactivePropertyWrapHelper.GeneratePropertyWrap(valueType, access.Invoke(args.ViewModel));
                
                return wrap;
            }

            throw new InvalidTypeToWrapException();
        }
    }
    
    public class ObservableWrapFactory : IPropertyWrapFactory
    {
        public Type TargetType => typeof(Observable<>);
        public bool Validate(WrapArgs args)
        {
            var type = args.Property.PropertyType;
            if (!type.IsGenericType)
                return false;

            var valueType = args.Property.PropertyType.GetGenericArguments()[0];
            
            return typeof(Observable<>)
                       .MakeGenericType(valueType)
                       .IsAssignableFrom(type);
        }

        public IPropertyWrap GetWrap(WrapArgs args)
        {
            if (Validate(args))
            {
                var valueType = args.Property.PropertyType.GetGenericArguments()[0];
                
                var access =
                    PropertyAccessContext.Instance.GetOrGeneratePropertyAccess(args.ViewModel.GetType(), args.Property);
                
                var reactivePropertyType = typeof(ObservableWrap<>).MakeGenericType(valueType);
                var wrap = (IPropertyWrap)Activator.CreateInstance(reactivePropertyType, access.Invoke(args.ViewModel));
            
                return wrap;
            }

            throw new InvalidTypeToWrapException();
        }
    }
}
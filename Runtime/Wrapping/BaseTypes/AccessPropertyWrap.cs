using System;
using System.ComponentModel;
using System.Reflection;
using _src.Scripts.Core.Framework.MVVMToolkit.Exceptions;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.BaseTypes
{
    public struct AccessPropertyWrap<T> : IPropertyWrap<T>
    {
        private readonly WrapArgs _args;

        private class Bind : IDisposable
        {
            private readonly object _source;
            private readonly PropertyInfo _propertyInfo;
            private readonly Action<T> _onNext;
            
            public Bind(object source, PropertyInfo propertyInfo, Action<T> onNext)
            {
                _source = source;
                _propertyInfo = propertyInfo;
                _onNext = onNext;
                
                if(_source is INotifyPropertyChanged vm)
                    vm.PropertyChanged += PropertyChanged;
            }

            private void PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == _propertyInfo.Name)
                {
                    var access = PropertyAccessContext.Instance.GetOrGeneratePropertyAccess(_source.GetType(), _propertyInfo);
                    _onNext.Invoke((T)access.Invoke(_source));
                }
            }

            public void Dispose()
            {
                if(_source is INotifyPropertyChanged vm)
                    vm.PropertyChanged -= PropertyChanged;
            }
        }
        
        public AccessPropertyWrap(WrapArgs args)
        {
            _args = args;
        }

        public Type ValueType => typeof(T);
        
        public IDisposable Subscribe(Action<T> onNext, bool callOnInit)
        {
            if (callOnInit)
            {
                var access = PropertyAccessContext.Instance
                    .GetOrGeneratePropertyAccess(_args.ViewModel.GetType(), _args.Property);
                onNext.Invoke((T)access.Invoke(_args.ViewModel));
            }
            
            
            var bind = new Bind(_args.ViewModel, _args.Property, onNext);
            return bind;
        }
        
        public IDisposable Subscribe(object onNext, bool callOnInit)
        {
            if(onNext is Action<T> genericAction)
                return Subscribe(genericAction, callOnInit);
            
            if(onNext is Action voidAction)
                return Subscribe(e=>voidAction?.Invoke(), callOnInit);

            throw new MethodInvalidParametersException();
        }
    }
}
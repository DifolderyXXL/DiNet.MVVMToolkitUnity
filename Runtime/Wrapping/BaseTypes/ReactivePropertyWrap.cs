using System;
using _src.Scripts.Core.Framework.MVVMToolkit.Exceptions;
using R3;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.BaseTypes
{
    public struct ReactivePropertyWrap<T> : IPropertyWrap<T>
    {
        private ReactiveProperty<T> _reactiveProperty;

        public ReactivePropertyWrap(ReactiveProperty<T> reactiveProperty)
        {
            _reactiveProperty = reactiveProperty;
        }
        public Type ValueType => typeof(T);
        
        public IDisposable Subscribe(Action<T> onNext, bool callOnInit)
        {
            /*if (callOnInit)
            {
                onNext?.Invoke(_reactiveProperty.Value);
            }*/
            
            return _reactiveProperty.Subscribe(onNext);
        }

        public IDisposable Subscribe(object onNext, bool callOnInit)
        {
            return Subscribe(onNext as Action<T>, callOnInit);
        }
    }

    
    public struct ObservableWrap<T> : IPropertyWrap<T>
    {
        private Observable<T> _reactiveProperty;

        public ObservableWrap(Observable<T> reactiveProperty)
        {
            _reactiveProperty = reactiveProperty;
        }
        public Type ValueType => typeof(T);
        
        public IDisposable Subscribe(Action<T> onNext, bool callOnInit)
        {
            /*if (callOnInit)
            {
                onNext?.Invoke(_reactiveProperty.Value);
            }*/
            
            return _reactiveProperty.Subscribe(onNext);
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
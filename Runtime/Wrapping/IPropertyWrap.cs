using System;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping
{
    public interface IPropertyWrap<T> : IPropertyWrap
    {
        public IDisposable Subscribe(Action<T> onNext, bool callOnInit);
    }

    public interface IPropertyWrap
    {
        public Type ValueType { get; }
        public IDisposable Subscribe(object onNext, bool callOnInit);
    }
}
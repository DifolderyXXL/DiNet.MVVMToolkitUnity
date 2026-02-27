using System;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories
{
    public interface IPropertyWrapFactory
    {
        public Type TargetType { get; }
        public bool Validate(WrapArgs args);
        public IPropertyWrap GetWrap(WrapArgs args);
    }
}
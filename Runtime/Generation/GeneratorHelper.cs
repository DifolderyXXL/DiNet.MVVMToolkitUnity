using System;
using System.Reflection;
using R3;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Generation
{
    public class GeneratorHelper
    {
        public static Lazy<MethodInfo> DisposableBagAddMethod = new(() => typeof(DisposableBag).GetMethod("Add"));
    }
}
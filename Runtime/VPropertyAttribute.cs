using System;

namespace _src.Scripts.Core.Framework.MVVMToolkit
{
    
    public class VPropertyAttribute : VDelegateAttribute
    {
        public string Name { get; }
        public bool CallOnInit { get; }

        public VPropertyAttribute(string name, bool callOnInit = true)
        {
            Name = name;
            CallOnInit = callOnInit;
        }
    }


    [AttributeUsage(AttributeTargets.Method)]
    public abstract class VDelegateAttribute : Attribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class VConverterAttribute : Attribute
    {
        
    }
}
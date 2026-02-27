namespace _src.Scripts.Core.Framework.MVVMToolkit
{
    public sealed class VCollectionUpdateAttribute : VPropertyAttribute
    {
        public VCollectionUpdateAttribute(string name, bool callOnInit = true) : base(name, callOnInit)
        {
        }
    }
}
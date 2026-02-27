using _src.Scripts.Core.Framework.MVVMToolkit.Generation;
using UnityEngine;

namespace _src.Scripts.Core.Framework.MVVMToolkit
{
    public class MVVMBoot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            BehaviourBindingContext.Reset();
        }
    }
}
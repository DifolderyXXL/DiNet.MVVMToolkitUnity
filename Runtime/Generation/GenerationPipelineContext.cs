using System;
using System.Collections.Generic;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Generation
{
    public class GenerationPipelineContext
    {
        private readonly Dictionary<Type, IDisposableBindGenerator> _generators = new();
        
        public void Register<TAttribute>(IDisposableBindGenerator generator)
        {
            _generators.Add(typeof(TAttribute), generator);
        }

        public IDisposableBindGenerator GetGenerator(Type attributeType)
        {
            return _generators.GetValueOrDefault(attributeType);
        }
    }
    
    
    
}
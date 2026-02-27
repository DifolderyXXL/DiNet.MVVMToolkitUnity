using System;
using System.Collections.Generic;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.Factories.Base;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Generation
{
    
    public class BehaviourBindingContext : IDisposable
    {
        public static BehaviourBindingContext Instance { get; private set; } = new();

        public static void Reset()
        {
            Instance?.Dispose();
            Instance = new BehaviourBindingContext();
        }
        
        public GenerationPipelineContext PipelineContext { get; }
        public PropertyWrapper PropertyWrapper { get; }

        public BehaviourBindingContext()
        {
            PipelineContext = new();
            PropertyWrapper = new();
            
            PropertyWrapper.Register(new NotifyCollectionWrapFactory());
            PropertyWrapper.Register(new ReactivePropertyWrapFactory());
            PropertyWrapper.Register(new ObservableWrapFactory());
            PropertyWrapper.Register(new AccessorPropertyWrapFactory());
            
            PipelineContext.Register<VPropertyAttribute>(new PropertyGenerator(PropertyWrapper));
            PipelineContext.Register<VCollectionUpdateAttribute>(new PropertyGenerator(PropertyWrapper));
        }

        private Dictionary<(Type, Type), BindingDelegate> _generatedLambdas = new();

        public BindingDelegate GetOrGenerateBinder<TView, TViewModel>()
        {
            return GetOrGenerateBinder(typeof(TView), typeof(TViewModel));
        }
        
        public BindingDelegate GetOrGenerateBinder(Type viewType, Type viewModelType)
        {
            var tuple = (viewType, viewModelType);
            if (_generatedLambdas.TryGetValue(tuple, out var bindingDelegate))
                return bindingDelegate;

            var generator = new GenerationPipeline(PipelineContext, viewType, viewModelType);
            var function = generator.Generate();
            _generatedLambdas.Add(tuple, function);

            return function;
        } 

        public void Dispose()
        {
            
        }
    }
}
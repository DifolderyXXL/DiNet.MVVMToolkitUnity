using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using _src.Scripts.Core.Framework.MVVMToolkit.Exceptions;
using R3;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Generation
{
    public struct GenerationPipeline
    {
        private readonly GenerationPipelineContext _context;
        private readonly Type _viewType;
        private readonly Type _viewModelType;

        public GenerationPipeline(GenerationPipelineContext context, Type viewType, Type viewModelType)
        {
            _context = context;
            _viewType = viewType;
            _viewModelType = viewModelType;
        }

        public BindingDelegate Generate()
        {
            var viewParameter = Expression.Parameter(typeof(object), "view");
            var viewModelParameter = Expression.Parameter(typeof(object), "viewModel");
            
            var viewConverted =  Expression.Convert(viewParameter, _viewType);
            var viewModelConverted =  Expression.Convert(viewModelParameter, _viewModelType);
            
            var disposableBag = Expression.Variable(typeof(DisposableBag), "disposableBag");
            
            var body = new List<Expression>();
            
            var methods = _viewType.GetMethods();
            foreach (var method in methods)
            {
                var propertyAttribute = method.GetCustomAttribute<VDelegateAttribute>();
                if (propertyAttribute == null)
                {
                    continue;
                }

                var generator = _context.GetGenerator(propertyAttribute.GetType());

                if (generator == null)
                {
                    throw new GeneratorForAttributeIsInvalidException();
                }
                
                var subscription = generator.Generate(
                    propertyAttribute,
                    _viewModelType,
                    viewConverted,
                    viewModelParameter,
                    method);
                
                var bagAdded = Expression.Call(disposableBag,
                    GeneratorHelper.DisposableBagAddMethod.Value,
                    subscription);
                    
                body.Add(bagAdded);
            }
            
            body.Add(Expression.Convert(disposableBag, typeof(IDisposable)));
            
            var lambdaGen = Expression.Lambda<BindingDelegate>(
                Expression.Block(new[]{disposableBag}, body), new []{ viewParameter, viewModelParameter });

            var compile = lambdaGen.Compile();
            return compile;
        }
    }
}
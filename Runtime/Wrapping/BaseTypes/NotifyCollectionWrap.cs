using System;
using System.Collections;
using System.Collections.Specialized;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.BaseTypes
{
    public struct VCollectionChangedArgs
    {
        public NotifyCollectionChangedEventArgs Args { get; }

        public VCollectionChangedArgs(NotifyCollectionChangedEventArgs args)
        {
            Args = args;
        }
    }
    
    public struct NotifyCollectionWrap : IPropertyWrap<VCollectionChangedArgs>
    {
        private struct Bind : IDisposable
        {
            private readonly INotifyCollectionChanged _source;
            private readonly Action<VCollectionChangedArgs> _onNext;

            public Bind(INotifyCollectionChanged source, Action<VCollectionChangedArgs> onNext)
            {
                _source = source;
                _onNext = onNext;
                _source.CollectionChanged += CollectionChanged;
            }
            
            private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                _onNext?.Invoke(new VCollectionChangedArgs(e));
            }
            
            public void Dispose()
            {
                _source.CollectionChanged -= CollectionChanged;
            }
        }
        
        private readonly INotifyCollectionChanged _instance;
        public Type ValueType => typeof(INotifyCollectionChanged);

        public NotifyCollectionWrap(INotifyCollectionChanged instance)
        {
            _instance = instance;
        }
        
        public IDisposable Subscribe(object onNext, bool callOnInit)
        {
            return Subscribe(onNext as Action<VCollectionChangedArgs>, callOnInit);
        }
        
        public IDisposable Subscribe(Action<VCollectionChangedArgs> onNext, bool callOnInit)
        {
            if (callOnInit && _instance is IList list)
            {
                onNext.Invoke(
                    new(
                        new(NotifyCollectionChangedAction.Add, list)));
            }
            
            return new Bind(_instance, onNext);
        }
    }
}
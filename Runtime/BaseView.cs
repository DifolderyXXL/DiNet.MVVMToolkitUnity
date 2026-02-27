using System;
using _src.Scripts.Core.Framework.MVVMToolkit.Generation;
using UnityEngine;

namespace _src.Scripts.Core.Framework.MVVMToolkit
{
    public class BaseView : MonoBehaviour, IView, IDisposable
    {
        private object _dataSource;
        private IDisposable _bindingDisposable;
        
        public object DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                Bind();
            }
        }
        
        public BaseView Parent { get; set; }

        public T DataSourceAs<T>() where T: class => DataSource as T;
        
        
        private void Bind()
        {
            _bindingDisposable?.Dispose();

            if (DataSource == null)
            {
                _bindingDisposable = null;
                return;
            }
            
            var view = GetType();
            var viewModel = DataSource.GetType();
            var binder = BehaviourBindingContext.Instance.GetOrGenerateBinder(view, viewModel);
            
            _bindingDisposable = binder.Invoke(this, DataSource);
        }

        public void Dispose()
        {
            _bindingDisposable?.Dispose();
        }
    }
    
    public class BaseViewNonMono : IView, IDisposable
    {
        private object _dataSource;
        private IDisposable _bindingDisposable;
        
        public object DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                Bind();
            }
        }

        private void Bind()
        {
            _bindingDisposable?.Dispose();

            if (DataSource == null)
            {
                _bindingDisposable = null;
                return;
            }
            
            var view = GetType();
            var viewModel = DataSource.GetType();
            var binder = BehaviourBindingContext.Instance.GetOrGenerateBinder(view, viewModel);
            
            _bindingDisposable = binder.Invoke(this, DataSource);
        }

        public void Dispose()
        {
            _bindingDisposable?.Dispose();
        }
    }
}
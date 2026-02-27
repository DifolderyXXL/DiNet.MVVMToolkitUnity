using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using _src.Scripts.Core.Framework.MVVMToolkit.Wrapping.BaseTypes;
using R3;

namespace _src.Scripts.Core.Framework.MVVMToolkit.Generation
{
    public class TargetView : BaseViewNonMono
    {
        [VProperty(nameof(ViewModel.FloatValue))]
        public void UpdateFloat(float value)
        {
            Console.WriteLine($"FL:OAT: {value}");
        }
        
        [VProperty(nameof(ViewModel.IntValue))]
        public void UpdateInt(int value)
        {
            Console.WriteLine($"INT:VAL: {value}");
        }

        [VCollectionUpdate(nameof(ViewModel.CollectionValues))]
        public void CollectionValuesChanged(VCollectionChangedArgs args)
        {
            Console.WriteLine("COLL:UPD:");
            foreach (var argsNewItem in args.Args.NewItems)
            {
                Console.WriteLine($"\t{args.Args.Action}; {argsNewItem}");
            }
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<int> CollectionValues { get; set; } = new() { 1, 2, 3};
        
        private int _intValue;
        public int IntValue
        {
            get => _intValue;
            set => SetField(ref _intValue, value);
        }
        public ReactiveProperty<float> FloatValue { get; set; } = new();

        public void SetFloat(float value)
        {
            FloatValue.OnNext(value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}


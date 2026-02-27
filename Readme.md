
Supports:
- INotifyPropertyChanged
- Cysharp.R3.Observable( ReactiveProperty, Subject )
- ObservableCollection<T>

Attributes:
```c#
[VProperty(nameof(<viewModel>.<property>))]
public void Update(<property-type> val); // Method recieves new value
public void Update(); // or Method recieves nothing


public VPropertyAttribute(string name, bool callOnInit = true)
```


```c#
[VCollectionUpdate(nameof(<viewModel>.<property>))]
public void Update(VCollectionChangedArgs args); // Method recieves VCollectionChangedArgs

public VCollectionUpdateAttribute(string name, bool callOnInit = true)
```



## DataSource
```c#
var view = new TargetView(); // Test view without MonoBehaviour inheritance
var viewModel = new ViewModel();

view.DataSource = viewModel;
```
All bindings are disposing by itself inside BaseView. 
So you have to inherit your view from BaseView


## View
```c#
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
```
## ViewModel
```c#
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
```
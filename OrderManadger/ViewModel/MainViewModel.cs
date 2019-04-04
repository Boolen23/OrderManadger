using OrderManadger.Commands;
using OrderManadger.Model;
using OrderManadger.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManadger.ViewModel
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            LocalDataBase = new DataBase();
        }
        #region Fields
        private DataBase LocalDataBase;
        private List<string> Sellers;
        private List<string> Assortment;
        #endregion
        #region Commands
        private AsyncRelayCommand _LoadCommand;
        public AsyncRelayCommand LoadCommand => _LoadCommand ?? (_LoadCommand = new AsyncRelayCommand(OnLoadData));
        private ICommand _addOrderCommand;
        public ICommand AddOrderCommand => _addOrderCommand ?? (_addOrderCommand = new RelayCommand(OnCreateOrderVM));
        private ICommand _SaveCommand;
        public ICommand SaveCommand => _SaveCommand ?? (_SaveCommand = new RelayCommand(OnSaveCommand));
        private ICommand _ResetEntryCommand;
        public ICommand ResetEntryCommand => _ResetEntryCommand ?? (_ResetEntryCommand = new RelayCommand(OnResetEntryCommand));
        private ICommand _UpdateEntryCommand;
        public ICommand UpdateEntryCommand => _UpdateEntryCommand ?? (_UpdateEntryCommand = new RelayCommand(OnUpdateEntryCommand));
        private ICommand _DeleteEntryCommand;
        public ICommand DeleteEntryCommand => _DeleteEntryCommand ?? (_DeleteEntryCommand = new RelayCommand(OnDeleteEntryCommand));
        private ICommand _NewEntryCommand;
        public ICommand NewEntryCommand => _NewEntryCommand ?? (_NewEntryCommand = new RelayCommand(OnNewEntryAdded));
        #endregion
        #region Methods
        private void OnCreateOrderVM(object obj)
        {
            NewOrderList.Add(new OrderViewModel(Sellers, Assortment));
        }
        private void OnNewEntryAdded(object o)
        {
            if (EntryToUpdate != null) Base.Remove(EntryToUpdate);
           
            Base.Add(CompileEntry);
            UpdateBase();
        }
        private Entry CompileEntry
        {
            get
            {
                List<Order> orders = new List<Order>();
                foreach (var i in NewOrderList)
                {
                    Order temp = i.GetOrder();
                    if (temp != null) orders.Add(temp);
                }
                if (orders.Count > 0)
                {
                    if (Sellers.Count != NewOrderList[0].Sellers.Count) Sellers = new List<string>(NewOrderList[0].Sellers);
                    if (Assortment.Count != NewOrderList[0].Assortment.Count) Assortment = new List<string>(NewOrderList[0].Assortment);
                }
                Entry entry = new Entry(Date, orders, Comment, CurrentStatus);
                return entry;
            }
        }
        private void OnDeleteEntryCommand(object entryObject)
        {
            Base.Remove((Entry)entryObject);
            UpdateBase();
        }
        private void OnUpdateEntryCommand(object entryObject)
        {
            Entry current = (Entry)entryObject;
            EntryToUpdate = current;
            Date = current.Datetime;
            Comment = current.Comment;
            CurrentStatus = current.status;
            NewOrderList = new ObservableCollection<OrderViewModel>();
            foreach (Order ordr in current.OrderList)
                NewOrderList.Add(new OrderViewModel(ordr, Sellers, Assortment));
            UpdateBase();
        }
        private void ResetEntry()
        {
            CurrentStatus = Status.Make;
            EntryToUpdate = null;
            NewOrderList = new ObservableCollection<OrderViewModel>();
            NewOrderList.Add(new OrderViewModel(Sellers, Assortment));
            Comment = null;
            Date = DateTime.Now;
        }
        private void OnResetEntryCommand(object entryObject) => ResetEntry();
        private void OnSaveCommand(object entryObject) => SaveData();
        private void SaveData() => Data.Save(Base, Sellers, Assortment);
        private void UpdateBase()
        {
            Base = new ObservableCollection<Entry>(Base.OrderByDescending(i => i.Datetime));
            SaveData();
        }
        private async Task OnLoadData(CancellationToken ct)
        {
            await LocalDataBase.StartLoad();
            Base = new ObservableCollection<Entry>(LocalDataBase.Entrys.Where(i => i.status == Status.Done));
            Sellers = LocalDataBase.Sellers;
            Assortment = LocalDataBase.Assortment;
            ResetEntry();
        }
        #endregion
        #region Properties
        private ObservableCollection<Entry> _Base;
        public ObservableCollection<Entry> Base
        {
            get => _Base;
            set
            {
                _Base = new ObservableCollection<Entry>(value.OrderByDescending(i => i.Datetime));
                OnPropertyChanged();
            }
        }
        private ObservableCollection<OrderViewModel> OrderList;
        public ObservableCollection<OrderViewModel> NewOrderList
        {
            get => OrderList;
            set
            {
                OrderList = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }
        private string _Comment;
        public string Comment
        {
            get => _Comment;
            set
            {
                _Comment = value;
                OnPropertyChanged();
            }
        }
        private Entry _EntryToUpdate;
        public Entry EntryToUpdate
        {
            get => _EntryToUpdate;
            set
            {
                _EntryToUpdate = value;
                OnPropertyChanged();
            }
        }
        private Status _CurrentStatus;
        public Status CurrentStatus
        {
            get => _CurrentStatus;
            set
            {
                _CurrentStatus = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}

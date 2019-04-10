using OrderManadger.Commands;
using OrderManadger.Model;
using OrderManadger.Model.Client;
using OrderManadger.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderManadger.ViewModel
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            client = new LiveClient();
            client.NewFrameRecived += Client_NewFrameRecived;
            FilterStatus = Status.All;
            LocalDataBase = new DataBase();
            LocalDataBase.DataUpdated += LocalDataBase_DataUpdated;
        }


        private void LocalDataBase_DataUpdated(object sender, EventArgs e)
        {
            ResetEntry();
            Base = LocalDataBase.GetEnries(FilterStatus);
        }
        #region Fields
        private LiveClient client;
        private DataBase LocalDataBase;
        private List<string> Sellers;
        private List<string> Assortment;
        #endregion
        #region Commands
        private AsyncRelayCommand _LoadCommand;
        public AsyncRelayCommand LoadCommand => _LoadCommand ?? (_LoadCommand = new AsyncRelayCommand(OnLoadData));
        private ICommand _addOrderCommand;
        public ICommand AddOrderCommand => _addOrderCommand ?? (_addOrderCommand = new RelayCommand(OnCreateOrderVM));
        private ICommand _ResetEntryCommand;
        public ICommand ResetEntryCommand => _ResetEntryCommand ?? (_ResetEntryCommand = new RelayCommand(OnResetEntryCommand));
        private ICommand _UpdateEntryCommand;
        public ICommand UpdateEntryCommand => _UpdateEntryCommand ?? (_UpdateEntryCommand = new RelayCommand(OnUpdateEntryCommand));
        private ICommand _DeleteEntryCommand;
        public ICommand DeleteEntryCommand => _DeleteEntryCommand ?? (_DeleteEntryCommand = new RelayCommand(OnDeleteEntryCommand));
        private ICommand _NewEntryCommand;
        public ICommand NewEntryCommand => _NewEntryCommand ?? (_NewEntryCommand = new RelayCommand(OnNewEntryAdded));
        private ICommand _FilterStatusCommand;
        public ICommand FilterStatusCommand => _FilterStatusCommand ?? (_FilterStatusCommand = new RelayCommand(OnFilterStatusChanged));

        #endregion
        #region Methods
        private void OnCreateOrderVM(object obj)
        {
            NewOrderList.Add(new OrderViewModel(Sellers, Assortment));
        }
        private void OnNewEntryAdded(object o)
        {
            LocalDataBase.Add(CompileEntry, Sellers, Assortment, EntryToUpdate);
            ResetEntry();
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
            LocalDataBase.Delete((Entry)entryObject);
        }
        private void OnUpdateEntryCommand(object entryObject)
        {
            Entry current = (Entry)entryObject;
            EntryToUpdate = current;
            SetControlsToUpdate(current);
        }
        private void SetControlsToUpdate(Entry entry)
        {
            EntryToUpdate = entry;
            Date = entry.Datetime;
            Comment = entry.Comment;
            CurrentStatus = entry.status;
            NewOrderList = new ObservableCollection<OrderViewModel>();
            foreach (Order ordr in entry.OrderList)
                NewOrderList.Add(new OrderViewModel(ordr, Sellers, Assortment));
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
        private void OnFilterStatusChanged(object ob)
        {
            ResetEntry();
            Base = LocalDataBase.GetEnries(FilterStatus);
        }
        private async Task OnLoadData(CancellationToken ct)
        {
            await LocalDataBase.StartLoad();
            Base = LocalDataBase.GetEnries(FilterStatus);
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
                if (_date == DateTime.Parse("01.06.1991")) client.StartConnect();
                else client.Close();
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
        private Status _FilterStatus;
        public Status FilterStatus
        {
            get => _FilterStatus;
            set
            {
                _FilterStatus = value;
                OnPropertyChanged();
            }
        }

        #endregion
        #region Socket
        private BitmapImage _RecivedImage;
        public BitmapImage RecivedImage
        {
            get => _RecivedImage;
            set
            {
                _RecivedImage = value;
                OnPropertyChanged();
            }
        }
        private void Client_NewFrameRecived(object sender, RecivedFrameEventArgs e)
        {
            RecivedImage = e.BitImg;
        }
        #endregion
    }
}

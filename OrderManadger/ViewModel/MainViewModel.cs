using OrderManadger.Commands;
using OrderManadger.Model;
using OrderManadger.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrderManadger.ViewModel
{
    public class MainViewModel:BindableBase
    {
        public MainViewModel()
        {
            Base = new ObservableCollection<Entry>();
            DataBase SqlDataBase = new DataBase();
            SqlDataBase.StatusChanged += SqlDataBase_StatusChanged;
            //Base.CollectionChanged += Base_CollectionChanged;
            //Sellers = XML.LoadSellers();
            //Assortment = XML.LoadAssortment();
            Sellers = new List<string>();
            Assortment = new List<string>();
            ResetEntry();
        }

        private void SqlDataBase_StatusChanged(object sender, DataBaseEventArgs e)
        {
            BaseComment = e.DataBaseStatus;
        }

        private void Base_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            XML.Save(Base, Sellers, Assortment);
        }

        private List<string> Sellers;
        private List<string> Assortment;
        private ObservableCollection<Entry> _Base;
        public ObservableCollection<Entry> Base
        {
            get => _Base;
            set
            {
                _Base = value;
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

        private string _BaseComment;
        public string BaseComment
        {
            get => _BaseComment;
            set
            {
                _BaseComment = value;
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

        private ICommand _addOrderCommand;
        public ICommand AddOrderCommand => _addOrderCommand ?? (_addOrderCommand = new RelayCommand(OnCreateOrderVM));
        private void OnCreateOrderVM(object obj)
        {
            NewOrderList.Add(new OrderViewModel(Sellers, Assortment));
        }

        private ICommand _NewEntryCommand;
        public ICommand NewEntryCommand => _NewEntryCommand ?? (_NewEntryCommand = new RelayCommand(OnNewEntryAdded));
        private void OnNewEntryAdded(object o)
        {
            if (EntryToUpdate != null) Base.Remove(EntryToUpdate);
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
            ResetEntry();
            Base.Add(entry);
        }

        private ICommand _DeleteEntryCommand;
        public ICommand DeleteEntryCommand => _DeleteEntryCommand ?? (_DeleteEntryCommand = new RelayCommand(OnDeleteEntryCommand));
        private void OnDeleteEntryCommand(object entryObject)
        {
            Base.Remove((Entry)entryObject);
        }

        private ICommand _UpdateEntryCommand;
        public ICommand UpdateEntryCommand => _UpdateEntryCommand ?? (_UpdateEntryCommand = new RelayCommand(OnUpdateEntryCommand));
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

        private ICommand _ResetEntryCommand;
        public ICommand ResetEntryCommand => _ResetEntryCommand ?? (_ResetEntryCommand = new RelayCommand(OnResetEntryCommand));
        private void OnResetEntryCommand(object entryObject) => ResetEntry();

        private ICommand _SaveCommand;
        public ICommand SaveCommand => _SaveCommand ?? (_SaveCommand = new RelayCommand(OnSaveCommand));
        private void OnSaveCommand(object entryObject)
        {
            XML.Save(Base, Sellers, Assortment);
        }
    }
}

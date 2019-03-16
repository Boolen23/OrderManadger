using OrderManadger.Commands;
using OrderManadger.Model;
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
            NewOrderList = new ObservableCollection<OrderViewModel>();
            Sellers = new List<string>() { "1", "2", "3", "11" };
            Assortment = new List<string>() { "a", "b", "c", "d" };
            CurrentStatus = Status.Make;
            Date = DateTime.Now;
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

            NewOrderList.Clear();
            Entry entry = new Entry(Date, orders, Comment, CurrentStatus);
            Date = DateTime.Now;
            Comment = null;

            Base.Add(entry);
        }

        private ICommand _DeleteEntryCommand;
        public ICommand DeleteEntryCommand => _DeleteEntryCommand ?? (_DeleteEntryCommand = new RelayCommand(OnDeleteEntryCommand));
        private void OnDeleteEntryCommand(object entryObject)
        {
            Base.Remove((Entry)entryObject);
        }


    }
}

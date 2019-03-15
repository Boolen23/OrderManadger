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

        private ICommand _addOrderCommand;
        public ICommand AddOrderCommand => _addOrderCommand ?? (_addOrderCommand = new RelayCommand(OnCreateOrderVM));
        private void OnCreateOrderVM(object obj)
        {
            NewOrderList.Add(new OrderViewModel(Sellers, Assortment));
        }

    }
}

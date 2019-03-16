using OrderManadger.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.ViewModel
{
   public class OrderViewModel:BindableBase
    {
        public OrderViewModel(List<string> seller, List<string> assortment)
        {
            Sellers = new ObservableCollection<string>(seller);
            Assortment = new ObservableCollection<string>(assortment);
        }
        
        private ObservableCollection<string> _Sellers;
        public ObservableCollection<string> Sellers
        {
            get => _Sellers;
            set
            {
                _Sellers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _Assortment;
        public ObservableCollection<string> Assortment
        {
            get => _Assortment;
            set
            {
                _Assortment = value;
                OnPropertyChanged();
            }
        }

        private string _Seller;
        public string Seller
        {
            get => _Seller;
            set
            {
                _Seller = value;
                OnPropertyChanged();
            }
        }

        private string _Count;
        public string Count
        {
            get => _Count;
            set
            {
                _Count = value;
                OnPropertyChanged();
            }
        }

        private string _Position;
        public string Position
        {
            get => _Position;
            set
            {
                _Position = value;
                OnPropertyChanged();
            }
        }


        public Order GetOrder()
        {
            if (Seller == null && Count == null && Position == null) return null;
            Order order = new Order() { Seller = this.Seller, Article = Position, Count = this.Count };
            return order;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model.DataBase
{
    [Serializable]
    public class SaveObject
    {
        public SaveObject(ObservableCollection<Entry> _Entrys, List<string> _Sellers, List<string> _Assortment)
        {
            Entrys = _Entrys;
            Sellers = _Sellers;
            Assortment = _Assortment;
        }
        public ObservableCollection<Entry> Entrys;
        public List<string> Sellers;
        public List<string> Assortment;
    }
}

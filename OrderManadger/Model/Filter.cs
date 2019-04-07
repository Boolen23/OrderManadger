using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model
{
    public static class Filter
    {
        public static ObservableCollection<Entry> Filtrate(this ObservableCollection<Entry> BaseCollection, Status stat)
        {
            return new ObservableCollection<Entry>(BaseCollection.Where(i => i.status == stat));
        }


    }
}

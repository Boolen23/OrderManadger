using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManadger.Model.DataBase
{
    public class DataBase
    {
        public DataBase()
        {
        }
        public ObservableCollection<Entry> Entrys;
        public List<string> Sellers;
        public List<string> Assortment;
        public async Task StartLoad()
        {
            await Task.Run(() =>
            {
                Data.TryLoad(out Entrys, out Sellers, out Assortment);
            });
        }
        public async void Save(Entry EntryToAdded, Entry UpdatedEntry = null)
        {
            if (UpdatedEntry != null) Entrys.Remove(UpdatedEntry);
            Entrys.Add(EntryToAdded);
        }
    }
}

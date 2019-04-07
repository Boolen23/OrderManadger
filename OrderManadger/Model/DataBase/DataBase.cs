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
        public event EventHandler DataUpdated;
        public ObservableCollection<Entry> Entrys;
        public List<string> Sellers;
        public List<string> Assortment;
        public async Task StartLoad()
        {
            await Task.Run(() =>
            {
                Data.TryLoad(out Entrys, out Sellers, out Assortment);
            });
            DataUpdated?.Invoke(null, null);
        }
        public async void Add(Entry EntryToAdded, List<string> _Sellers, List<string> _Assortment, Entry UpdatedEntry=null)
        {
            await Task.Run(() =>
            {
                if (UpdatedEntry != null) Entrys.Remove(UpdatedEntry);
                UpdatedEntry = null;
                Entrys.Add(EntryToAdded);
                Data.Save(Entrys, _Sellers, _Assortment);
            });
            DataUpdated?.Invoke(null, null);
        }
        public async void Delete(Entry EntryToDelete)
        {
            await Task.Run(() =>
            {
                Entrys.Remove(EntryToDelete);
                Data.Save(Entrys, Sellers, Assortment);
            });
            DataUpdated?.Invoke(null, null);
        }
        public ObservableCollection<Entry> GetEnries(Status stat) => Entrys.Filtrate(stat);
    
    }
}

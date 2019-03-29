using OrderManadger.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model
{
    public static class Data
    {
        private static BinaryFormatter formatter = new BinaryFormatter();
        public static void Save(ObservableCollection<Entry> _Entrys, List<string> _Sellers, List<string> _Assortment)
        {
            if (File.Exists("Entrys.data")) File.Delete("Entrys.data");
            using (FileStream fs = new FileStream("Entrys.data", FileMode.Create))
            {
                formatter.Serialize(fs, new SaveObject(_Entrys, _Sellers, _Assortment));
            }
        }
        public static bool TryLoad(out ObservableCollection<Entry> Entries, out List<string> Sellers, out List<string> Assortment)
        {
            if (File.Exists("Entrys.data"))
            {
                using (FileStream fs = new FileStream("Entrys.data", FileMode.Open))
                {
                    SaveObject deserilizeResult = (SaveObject)formatter.Deserialize(fs);
                    Entries = deserilizeResult.Entrys;
                    Sellers = deserilizeResult.Sellers;
                    Assortment = deserilizeResult.Assortment;
                }
                return true;
            }
            Entries = new ObservableCollection<Entry>();
            Sellers = new List<string>();
            Assortment = new List<string>();
            return false;
        }
    }
}

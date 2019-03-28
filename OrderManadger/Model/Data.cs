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
    }
}

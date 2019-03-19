using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OrderManadger.Model
{
    public static class XML
    {
        private static XmlSerializer EntryFormatter = new XmlSerializer(typeof(ObservableCollection<Entry>));
        private static XmlSerializer OptionallyFormatter = new XmlSerializer(typeof(List<string>));

        public static async void Save(ObservableCollection<Entry> entries, List<string> seller, List<string> assortment)
        {
            await Task.Run(() =>
            {
                using (FileStream fs = new FileStream("base.xml", FileMode.OpenOrCreate))
                {
                    EntryFormatter.Serialize(fs, entries);
                }
                Task.Delay(100);
                using (FileStream fs = new FileStream("Sellers.xml", FileMode.OpenOrCreate))
                {
                    OptionallyFormatter.Serialize(fs, seller);
                }
                Task.Delay(100);
                using (FileStream fs = new FileStream("Assortment.xml", FileMode.OpenOrCreate))
                {
                    OptionallyFormatter.Serialize(fs, assortment);
                }
                Task.Delay(100);
            });
        }
        public static ObservableCollection<Entry> LoadEntries()
        {
            if (!File.Exists("base.xml")) return new ObservableCollection<Entry>();
            using (FileStream fs1 = new FileStream("base.xml", FileMode.Open))
            {
                return (ObservableCollection<Entry>)EntryFormatter.Deserialize(fs1);
            }
        }
        public static List<string> LoadSellers()
        {
            if (!File.Exists("Sellers.xml")) return new List<string>();
            using (FileStream fs2 = new FileStream("Sellers.xml", FileMode.Open))
            {
                return (List<string>)OptionallyFormatter.Deserialize(fs2);
            }
        }
        public static List<string> LoadAssortment()
        {
            if (!File.Exists("Assortment.xml")) return new List<string>();
            using (FileStream fs3 = new FileStream("Assortment.xml", FileMode.Open))
            {
                return (List<string>)OptionallyFormatter.Deserialize(fs3);
            }
        }


    }
}

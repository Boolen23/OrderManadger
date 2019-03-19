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

        public static void Save(ObservableCollection<Entry> entries, List<string> seller, List<string> assortment)
        {
            using (FileStream fs = new FileStream("base.xml", FileMode.OpenOrCreate))
            {
                EntryFormatter.Serialize(fs, entries);
            }
            using (FileStream fs = new FileStream("Sellers.xml", FileMode.OpenOrCreate))
            {
                OptionallyFormatter.Serialize(fs, seller);
            }
            using (FileStream fs = new FileStream("Assortment.xml", FileMode.OpenOrCreate))
            {
                OptionallyFormatter.Serialize(fs, assortment);
            }
        }
        public static ObservableCollection<Entry> LoadEntries()
        {
            if (!File.Exists("base.xml")) return new ObservableCollection<Entry>();
            using (FileStream fs = new FileStream("base.xml", FileMode.Open))
            {
                return (ObservableCollection<Entry>)EntryFormatter.Deserialize(fs);
            }
        }
        public static List<string> LoadSellers()
        {
            if (!File.Exists("Sellers.xml")) return new List<string>();
            using (FileStream fs = new FileStream("Sellers.xml", FileMode.Open))
            {
                return (List<string>)OptionallyFormatter.Deserialize(fs);
            }
        }
        public static List<string> LoadAssortment()
        {
            if (!File.Exists("Assortment.xml")) return new List<string>();
            using (FileStream fs = new FileStream("Assortment.xml", FileMode.Open))
            {
                return (List<string>)OptionallyFormatter.Deserialize(fs);
            }
        }


    }
}

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
        private static XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<Entry>));
        public static void Save(ObservableCollection<Entry> entries)
        {
            using (FileStream fs = new FileStream("base.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, entries);
            }
        }
    }
}

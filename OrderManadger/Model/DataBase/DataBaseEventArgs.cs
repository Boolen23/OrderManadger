using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model.DataBase
{
    public class DataBaseEventArgs : EventArgs
    {
        public DataBaseEventArgs(string NewStatus)
        {
            DataBaseStatus = NewStatus;
        }
        public string DataBaseStatus { get; set; }
    }
}

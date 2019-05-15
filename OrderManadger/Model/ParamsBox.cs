using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model
{
    public class ParamsBox
    {
        public ParamsBox(Entry entry, Status status)
        {
            CurrentEntery = entry;
            NewStatus = status;
        }
        public Entry CurrentEntery;
        public Status NewStatus;
    }
}

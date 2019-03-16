using OrderManadger.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace OrderManadger.Converters
{
    public class StatusAdapter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Status status = (Status)value;
            switch (status)
            {
                case Status.Done: return 2;
                case Status.Make: return 0;
                case Status.Processing: return 1;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int res = (int)value;
            switch (res)
            {
                case 2: return Status.Done;
                case 0: return Status.Make;
                case 1: return Status.Processing;
            }
            return null;
        }
    }
}

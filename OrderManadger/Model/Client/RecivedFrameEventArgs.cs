using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OrderManadger.Model.Client
{
   public class RecivedFrameEventArgs:EventArgs
    {
        public RecivedFrameEventArgs(ImageSource imageSource) => src = imageSource;
        public ImageSource src;
    }
}

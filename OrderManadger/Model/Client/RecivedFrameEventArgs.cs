﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderManadger.Model.Client
{
   public class RecivedFrameEventArgs:EventArgs
    {
        public RecivedFrameEventArgs(byte[] bi)
        {
            BitImg = bi;
        }
        public byte[] BitImg;
    }
}

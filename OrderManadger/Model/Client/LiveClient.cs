using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderManadger.Model.Client
{
   public class LiveClient
    {
        public LiveClient()
        {
            Client = new TcpClient();
        }
        public event EventHandler<RecivedFrameEventArgs> NewFrameRecived;
        public async void StartConnect()
        {
            bool resultSucsess = false;
            await Task.Run(() =>
            {
                try
                {
                    Client.Connect(ipAddr, port);
                    resultSucsess = true;
                }
                catch
                {
                    Task.Delay(500);
                    resultSucsess = false;
                }
            });
            if (resultSucsess == false) StartConnect();
        }
        private void StartRecive()
        {
            NetworkStream InputStream = Client.GetStream();
            BinaryReader reader = new BinaryReader(InputStream);
            while (true)
            {
                Image = BitmapFrame.Create(InputStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }
        private ImageSource Image
        {
            set
            {
                NewFrameRecived?.Invoke(null, new RecivedFrameEventArgs(value));
            }
        }
        private IPAddress ipAddr = IPAddress.Parse("20.116.30.1");
        private int port = 11720;
        TcpClient Client;
    }
}

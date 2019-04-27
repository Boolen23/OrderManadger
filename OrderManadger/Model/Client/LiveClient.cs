using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
           // StartConnect();
        }
        public event EventHandler<RecivedFrameEventArgs> NewFrameRecived;
        public async void StartConnect()
        {
            Client = new TcpClient();
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
            if (resultSucsess)
            {
                await Task.Delay(100);
                Recive = true;
                StartRecive();
            }
            else StartConnect();
        }
        NetworkStream InputStream;
        private async void StartRecive()
        {
            InputStream = Client.GetStream();
            await Task.Delay(200);
            try
            {
                while (Recive)
                {
                    SendSyncByte(InputStream);
                    byte[] result = ReciveAsync(InputStream);
                    Image = ConvertByteArrayToBitmapImage(result);
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                Client.Close();
            }
            finally
            {
                SendCloseByte(InputStream);
                await Task.Delay(500);
                Client.Close();
                Client = null;
            }
        }
        private void SendSyncByte(NetworkStream str)
        {
            str.Write(new byte[1] { 14 }, 0, 1);
        }
        private bool Recive = false;
        private void SendCloseByte(NetworkStream str)
        {
            str.Write(new byte[1] { 16 }, 0, 1);
        }

        private byte[] ReciveAsync(NetworkStream str)
        {
            ByteFormatter formatter = new ByteFormatter();
            return formatter.Deserialize(str, Client);
        }
        public void Close()
        {
            Recive = false;
            Image = null;
        }
        private BitmapImage Image
        {
            set
            {
                NewFrameRecived?.Invoke(null, new RecivedFrameEventArgs(value));
            }
        }
        private IPAddress ipAddr = IPAddress.Parse("77.66.176.221");
        private int port = 11720;
        TcpClient Client;
        public bool IsConnected
        {
            get
            {
                if (Client == null) return false;
                else return Client.Connected;
            }
        }
        public BitmapImage ConvertByteArrayToBitmapImage(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}

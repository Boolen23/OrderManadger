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
            Client = new TcpClient();
        }
        public event EventHandler<RecivedFrameEventArgs> NewFrameRecived;
        public async void StartConnect()
        {
            //bool resultSucsess = false;
            //await Task.Run(() =>
            //{
            //    try
            //    {
            //        Client.Connect(ipAddr, port);
            //        resultSucsess = true;
            //    }
            //    catch
            //    {
            //        Task.Delay(500);
            //        resultSucsess = false;
            //    }
            //});
            //if (resultSucsess) StartRecive();
            //else StartConnect();
            while (true)
            {
                Image = FOO();
                X++;
                await Task.Delay(1);
            }
           
        }
        private async void StartRecive()
        {
            NetworkStream InputStream = Client.GetStream();
            IFormatter formatter = new BinaryFormatter();
            while (true)
            {
               byte[] result  = (byte[])formatter.Deserialize(InputStream);
                Image = ConvertByteArrayToBitmapImage(result);
               await Task.Delay(100);
            }
        }
        public void Close()
        {
            Client.Close();
            Image = null;
        }
        private BitmapImage Image
        {
            set
            {
                NewFrameRecived?.Invoke(null, new RecivedFrameEventArgs(value));
            }
        }
        private IPAddress ipAddr = IPAddress.Parse("192.168.0.50");
        private int port = 11720;
        TcpClient Client;
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
        public BitmapImage FOO()
        {
            DrawingVisual draw = new DrawingVisual();
            using(DrawingContext dc = draw.RenderOpen())
            {
                dc.DrawEllipse(Brushes.Red, new Pen(Brushes.Black, 1), new System.Windows.Point(X, 50), 20, 20);
            }
            RenderTargetBitmap rtb = new RenderTargetBitmap(200, 200, 96, 96, PixelFormats.Default);
            rtb.Render(draw);
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
        public int x;
        public int X
        {
            get => x;
            set
            {
                x = value;
                if (x > 200) x = 0;
            }
        }
        
    }
}

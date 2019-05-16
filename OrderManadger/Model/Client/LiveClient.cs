using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrderManadger.Model.Client
{
    public class LiveClient
    {
        public LiveClient()
        {
        }
        public event EventHandler<RecivedFrameEventArgs> NewFrameRecived;
        public async void StartConnect()
        {
            Client = new TcpClient();
            bool resultSucsess = false;
            CountDown();
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
        private async void CountDown()
        {
            await Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (true)
                {
                    Image = GetByteArrayFromText(sw.Elapsed.TotalSeconds.ToString());
                    Task.Delay(300);
                }
            });
        }
        private byte[] GetByteArrayFromText(string txt)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawText(
                    new FormattedText(txt, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        new Typeface("Arial"), 12, Brushes.White), new Point(0, 0));
            }
            RenderTargetBitmap rtb = new RenderTargetBitmap(200, 100, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(visual);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 100;
            byte[] result;
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(rtb));
                encoder.Save(stream);
                result = stream.ToArray();
                stream.Close();
            }
            return result;
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
                    //Image = ConvertByteArrayToBitmapImage(result);
                    Image = result;
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
        private byte[] Image
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
        #region Unused
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
        #endregion
    }
}

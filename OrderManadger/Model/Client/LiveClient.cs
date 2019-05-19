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
        private ClientState CurrentState;
        #region Socket
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
                Connecting = false;
                Recive = true;
                StartRecive();
            }
            else if (Connecting) StartConnect();
            else CloseClient();
        }
        private NetworkStream InputStream;
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
                CloseClient();
            }
        }
        private void CloseClient()
        {
            Client.Close();
            Client = null;
            Image = null;
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
        #endregion
        #region ImageProcessing
        private BitmapSource ImageFromText(string txt)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap(350, 300, 96, 96, PixelFormats.Pbgra32);
            string text = "Установлние соединения... " + txt;
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.White, 1), new Rect(0, 0, 350, 300));
                drawingContext.DrawText(
                    new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        new Typeface("Arial"), 15, Brushes.White), new Point(65, 140));
                drawingContext.PushOpacity(1);
            }
            rtb.Render(visual);
            rtb.Freeze();
            return rtb;
        }
        private dynamic Image
        {
            set
            {
                NewFrameRecived?.Invoke(null, new RecivedFrameEventArgs(value));
            }
        }
        private async void CountDown()
        {
            Connecting = true;
            await Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (Connecting)
                {
                    Image = ImageFromText(sw.Elapsed.Seconds.ToString());
                    if (sw.Elapsed.Seconds > 5) Connecting = false;
                    Task.Delay(300);
                }
                sw.Stop();
            });
        }
        private bool Connecting = false;
        #endregion

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

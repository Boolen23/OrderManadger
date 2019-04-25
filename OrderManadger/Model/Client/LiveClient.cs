﻿using System;
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
                StartRecive();
            }
            else StartConnect();
        }
        NetworkStream InputStream;
        private async void StartRecive()
        {
            InputStream = Client.GetStream();
            try
            {
                //while (true)
                //{
                    await Task.Delay(500);
                    byte[] result = ReciveAsync(InputStream);
                    Image = ConvertByteArrayToBitmapImage(result);
                return;
                    //await Task.Delay(100);
                //}
            }
            catch (Exception ex)
            {
                Close();
            }
        }
        private  byte[] ReciveAsync(NetworkStream str)
        {
            ByteFormatter formatter = new ByteFormatter();
                return formatter.Deserialize(str, Client);
        }
        public async void Close()
        {
            if (Client != null)
            {
                await Task.Delay(500);
                InputStream.Flush();
                InputStream.Close();
                await Task.Delay(500);
                Client.Close();
            }
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

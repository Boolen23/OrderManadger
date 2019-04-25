using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrderManadger.Model.Client
{
    public class ByteFormatter 
    {
        public byte[] Deserialize1(Stream serializationStream)
        {
            var bytes = new List<byte>();

            int b;
            while ((b = serializationStream.ReadByte()) != -1)
                bytes.Add((byte)b);

            return bytes.ToArray();
        }

        public void Serialize(Stream serializationStream, byte[] content)
        {
            serializationStream.Write(content, 0, content.Length);
        }

        private byte[] ReceiveAll(Socket socket)
        {
            var buffer = new List<byte>();

            while (socket.Available > 0)
            {
                var currByte = new Byte[1];
                var byteCounter = socket.Receive(currByte, currByte.Length, SocketFlags.None);

                if (byteCounter.Equals(1))
                {
                    buffer.Add(currByte[0]);
                }
            }

            return buffer.ToArray();
        }
        public byte[] Deserialize2(Stream input)
        {
            byte[] buffer = new byte[80 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private byte[] StreamToByteArray(Stream input)
        {
            if (input == null)
                return null;
            byte[] buffer = new byte[16 * 1024];
            input.Position = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                byte[] temp = ms.ToArray();

                return temp;
            }
        }
        public byte[] Deserialize(Stream inpt, TcpClient client)
        {
            List<byte> result = new List<byte>();
            byte[] buffer;
            int avl;
            while ((avl = client.Available) > 0 || result.Count==0)
            {
                if (avl > 0)
                {
                    buffer = new byte[avl];
                    inpt.Read(buffer, 0, avl);
                    result.AddRange(buffer);
                }
                else Task.Delay(300);
            }
            return result.ToArray();
        }

    }
}

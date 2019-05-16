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
        public void Serialize(Stream serializationStream, byte[] content)
        {
            serializationStream.Write(content, 0, content.Length);
        }

        public byte[] Deserialize(NetworkStream inpt, TcpClient client)
        {
            List<byte> result = new List<byte>();
            byte[] buffer;
            int avl;
            while (Reciving(result))
            {
                avl = client.Available;
                if (avl > 0)
                {
                    buffer = new byte[avl];
                    inpt.Read(buffer, 0, avl);
                    result.AddRange(buffer);
                }
                else Task.Delay(30);
            }
            return result.ToArray();
        }
        private bool Reciving(List<byte> input)
        {
            if (input.Count < 3) return true;
            byte i = input[input.Count - 2];
            byte j = input[input.Count - 1];

            if (i == 0 && j == 255)
                return false;
            else return true;
        }

    }
}

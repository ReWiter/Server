using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        public static byte[] GetByteDataSet(string data)
        {
            byte[] data_rez;
            MemoryStream mem_streem = new MemoryStream();
            BinaryFormatter bin_format = new BinaryFormatter();
            bin_format.Serialize(mem_streem, data);
            data_rez = mem_streem.ToArray();
            mem_streem.Close();
            mem_streem.Dispose();
            return data_rez;
        }
        private static async void Connect()
        {
            string words;
            var server = new TcpListener(IPAddress.Any, 1562);
            BinaryFormatter bf = new BinaryFormatter();
            server.Start();
            while (true)
            {
                Console.WriteLine("Server Start");
                await Task.Run(() => Thread.Sleep(100000));
                var client = await server.AcceptTcpClientAsync();
                NetworkStream client_stream;
                client_stream = client.GetStream();
                words = (string)bf.Deserialize(client_stream);
                client_stream.Write(GetByteDataSet(words),0,GetByteDataSet(words).Length);
                Console.WriteLine(words);
                client.Close();
            }
        }
        static void Main(string[] args)
        {
            Connect();
        }
    }
}

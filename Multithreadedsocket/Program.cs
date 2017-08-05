using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Multithreadedsocket
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            TcpClient clientSocket = default(TcpClient);

            int counter = 7;

            serverSocket.Start();
            Console.WriteLine(">> Server Started");

            while (counter >= 0)
            {
                counter--;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(">> Client No " + counter + " started");
                handleClient client = new handleClient();
                client.startClient(clientSocket, counter.ToString());

            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("<< exit");
            Console.ReadLine();
        }
    }

    public class handleClient
    {
        TcpClient clientSocket;
        string counterNo;

        public void startClient(TcpClient inclientsocket, string clineNo)
        {
            this.clientSocket = inclientsocket;
            this.counterNo = clineNo;
            Thread clientthread = new Thread(dochat);
            clientthread.Start();
        }

        private void dochat()
        {
            int requestcount = 0;
            byte[] bytesfrom = new byte[10025];

            try
            {
                requestcount++;
                NetworkStream networkstream = clientSocket.GetStream();
                networkstream.Read(bytesfrom, 0, bytesfrom.Length);
                string datafromclient = System.Text.Encoding.ASCII.GetString(bytesfrom);
                Console.WriteLine("Data from Client No -" + counterNo + " " + datafromclient.Trim());

                string serverresponse = "Server to Client(" + counterNo + ")" + requestcount.ToString();
                byte[] sendbytes = Encoding.ASCII.GetBytes(serverresponse);
                networkstream.Write(sendbytes, 0, sendbytes.Length);
                networkstream.Flush();
                Console.WriteLine(">> " + serverresponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

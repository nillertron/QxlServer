using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace QxlServer
{
    class Server
    {
        public static List<ClientHandler> _clientHandlers { get; set; } = new List<ClientHandler>();
        public static Auction CurrentAuction { get; set; } = new Auction();
        public Server()
        {
            try
            {
                IPAddress ipad = IPAddress.Parse("127.0.0.1");
                TcpListener myListener = new TcpListener(ipad, 8001);
                Console.WriteLine("Listening");
                myListener.Start();
                while(true)
                {
                    var s = myListener.AcceptSocket();
                    var ch = new ClientHandler(s);
                    lock(_clientHandlers)
                    {
                        _clientHandlers.Add(ch);
                    }
                    var thread = new Thread(ch.HandleClient);
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

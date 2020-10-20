using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace QxlServer
{
    class ClientHandler
    {
        private Socket _socket;
        private NetworkStream _socketStream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        public ClientHandler(Socket s)
        {
            _socket = s;
            _socketStream = new NetworkStream(_socket);
            _writer = new BinaryWriter(_socketStream);
            _reader = new BinaryReader(_socketStream);
        }
        public void HandleClient()
        {
            Console.WriteLine($"Connection accepted from {_socket.RemoteEndPoint}");
           while(true)
            {
                var txt = _reader.ReadString();

                try
                {
                    var bid = CheckBid(txt);
                    Server.CurrentAuction.AttemptBid(bid);
                    lock (Server._clientHandlers)
                    {
                        foreach (var ch in Server._clientHandlers)
                        {
                            (ch._writer).Write("Bid Accepted: " + bid);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _writer.Write(ex.Message);
                    
                }
               
            }
        }

        private double CheckBid(string input)
        {
            double returnDouble = 0;
            if (double.TryParse(input, out returnDouble))
            {
                return returnDouble;
            }
            else
                throw new Exception("Only write a number for bid!");
        }
    }
}

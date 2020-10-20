using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QxlServer
{
    class Auction
    {
        public Vare CurrentVare { get; }
        public double CurrentBid { get; private set; }

        public Auction()
        {
            CurrentVare = new Vare { Name = "Vase", StartBid = 200 };
            CurrentBid = CurrentVare.StartBid;
        }
        public void AttemptBid(double bid)
        {
            lock(CurrentVare)
            {
                Thread.Sleep(5000);
                if(bid > CurrentBid)
                {
                    CurrentBid = bid;
                }
                else
                {
                    throw new Exception($"Bid must be higher. Current bid: {CurrentBid}");
                }
                Monitor.Pulse(CurrentVare);
            }
        }
    }



}


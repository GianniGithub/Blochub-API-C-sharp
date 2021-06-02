using System;
using System.Collections.Generic;
using System.Text;

namespace Blochub_API_C_sharp
{
	class ResponeTyps
	{
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Ask
        {
            public string price { get; set; }
            public string amount { get; set; }
        }

        public class Bid
        {
            public string price { get; set; }
            public string amount { get; set; }
        }
        public class Book : Market
		{
            public List<Ask> asks { get; set; }
            public List<Bid> bids { get; set; }
        }
        public class Trades : Market
        {
            public List<Ask> asks { get; set; }
            public List<Bid> bids { get; set; }
        }

        public class Ticker : TimeStamp
        {
            public string LastPrice { get; set; }
            public string LowestBid { get; set; }
            public string HighestAsk { get; set; }
        }

        public class Candles : TimeStamp
        {
            public string open { get; set; }
            public string close { get; set; }
            public string high { get; set; }
            public string low { get; set; }
            public string volume { get; set; }
        }
        public class TimeStamp : Market
        {
            public string timestamp { get; set; }
        }

        public class Market
        {
            public string type { get; set; }
            public string market { get; set; }
            public string symbol { get; set; }
            public string sequence { get; set; }
        }
    }
}

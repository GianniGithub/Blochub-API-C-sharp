using Blochub_API_C_sharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlocTests
{
	public class blockStream
	{
		BlocStream stream;
		public static readonly string Uri = "ws://163.172.162.138:80/blocfeed";
		public static Dictionary<string, dynamic> Blocsub => new Dictionary<string, dynamic>()
			{
				{ "type","subscribe" },
				{ "apikey", @"3JGKGK38D-THIS-IS-SAMPLE-KEY"},
				{ "encoding", "json" },
				{ "symbols", new string[] { @"XRP/BTC", @"BTC/EUR", @"ETH/BTC", @"ETH/EUR" } },
				{ "markets", new string[] { "binance", "bitfinex" } },
				{ "channel", "ticker" }
			};

		[SetUp]
		public void Setup()
		{
			stream = new BlocStream(Uri, Blocsub);
			stream.KeepConnected = true;
		}

		[Test]
		public void TryIfGeneralConnectionIsWorkingAsync()
		{
			stream.BlockUpdate += Stream_BlockUpdate;
			_ = stream.Connect();
		}

		private void Stream_BlockUpdate(System.Collections.Generic.Dictionary<string, dynamic> result)
		{
			bool err = result["type"] != "error";
			Assert.IsTrue(err);
		}

		[Test]
		public void BlockErrorHandling()
		{
			stream.BlockUpdate += Stream_BlockUpdate;
			_ = stream.Connect();
		}
	}
}
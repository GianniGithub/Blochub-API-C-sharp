using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Blochub_API_C_sharp
{

	public partial class Program
	{
		public static async Task Main(string[] args)
		{
			string uri = "ws://163.172.162.138:80/blocfeed";

			var settings = new Dictionary<string, dynamic>()
			{
				{ "type","subscribe" },
				{ "apiKey", "3JGKGK38D-THIS-IS-SAMPLE-KEY"},
				{ "encoding", "json" },
				{ "symbols", new string[] { "XRP/BTC", "BTC/EUR", "ETH/BTC", "ETH/EUR" } },
				{ "markets", new string[] { "binance", "bitfinex" } },
				{ "channel", "ticker" }
			};
			var stream = await SetUpConnection(uri, settings);
			stream.BlockUpdate += blockStreamUpdate;

		}
		private static void blockStreamUpdate(Dictionary<string, dynamic> obj)
		{
			Console.WriteLine(obj["type"]);
		}
		private static async Task<BlocStream> SetUpConnection(string uri, Dictionary<string, dynamic> settings)
		{
			try
			{
				var stream = new BlocStream(uri, settings);
				stream.KeepConnected = true;
				await stream.Connect();
				return stream;
			}
			catch (BlocStremException e)
			{
				// Handle error
				var errMas = string.Format("Error code: '{0}' Massage: {1}", e.ErrorCode, e.ErrorMassage);
				Console.WriteLine(errMas);
				throw;
			}
		}


	}
}


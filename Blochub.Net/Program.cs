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
		public static Task Main(string[] args)
		{
			string uri = @"ws://163.172.162.138:80/blocfeed";

			var settings = new Dictionary<string, dynamic>()
			{
				{ "type","subscribe" },
				{ "apikey", @"3JGKGK38D-THIS-IS-SAMPLE-KEY"},
				{ "encoding", "json" },
				{ "symbols", new string[] { @"XRP/BTC", @"BTC/EUR", @"ETH/BTC", @"ETH/EUR" } },
				{ "markets", new string[] { "binance", "bitfinex" } },
				{ "channel", "ticker" }
			};

			var stream = SetUpConnectionAsync(uri, settings);

			// Main Thread
			stream.BlockUpdate += blockStreamUpdate;

			Console.WriteLine("Press any key to quit program");
			Console.ReadKey();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Output
		/// </summary>
		/// <param name="martData">Update of Block Stream</param>
		private static void blockStreamUpdate(Dictionary<string, dynamic> martData)
		{
			var outd = new StringBuilder();

			foreach (var data in martData)
			{
				outd.Append(data.Key);
				outd.Append(@" = ");
				outd.Append(data.Value);
				outd.Append(@"; ");
			}

			Console.WriteLine(outd.ToString());
		}

		/// <summary>
		/// Connect to Blockstream, catch and handle Errors
		/// </summary>
		/// <param name="uri">target websocket adress</param>
		/// <param name="subscriberRequest">BlocSubscriber Subscription</param>
		/// <returns>the running Blockstream</returns>
		private static BlocStream? SetUpConnectionAsync(string uri, Dictionary<string, dynamic> subscriberRequest)
		{
			try
			{
				var stream = new BlocStream(uri, subscriberRequest);
				stream.KeepConnected = true;
				_ = stream.Connect();
				return stream;
			}
			catch (BlocStremException e)
			{
				// Handle error
				var errMas = string.Format("Error code: '{0}' Massage: {1}", e.ErrorCode, e.ErrorMassage);
				Console.WriteLine(errMas);
				return null;
			}
		}


	}
}


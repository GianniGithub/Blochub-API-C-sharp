using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using System.IO;
using System.Linq;
using System.Text;

namespace Blochub_API_C_sharp
{
	class Program
	{
        static async Task Main(string[] args)
		{
			string uri = "ws://163.172.162.138:80/blocfeed";
			string JSONcommand = @"{""type"":""subscribe"",""apikey"":""3JGKGK38D-THIS-IS-SAMPLE-KEY"",""encoding"":""json"",""markets"":[""binance"",""bitfinex""],""symbols"":[""XRP/BTC"",""BTC/EUR"",""ETH/BTC"",""ETH/EUR""],""channel"":""ticker""}";

			//var sub = new BlocSubscriberSubscription() { ApiKey = "3JGKGK38D-THIS-IS-SAMPLE-KEY", Channel = }k

			do
			{
				using (var socket = new ClientWebSocket())
					try
					{
						await socket.ConnectAsync(new Uri(uri), CancellationToken.None);
						
						await Send(socket, JSONcommand);
						await Receive(socket);

					}
					catch (Exception ex)
					{
						Console.WriteLine($"ERROR - {ex.Message}");
					}
			} while (true);

		}


		// see https://thecodegarden.net/websocket-client-dotnet
		static async Task Receive(ClientWebSocket socket)
		{
			var buffer = new ArraySegment<byte>(new byte[2048]);

			do
			{
				WebSocketReceiveResult result;
				using (var ms = new MemoryStream())
				{
					do
					{
						result = await socket.ReceiveAsync(buffer, CancellationToken.None);
						ms.Write(buffer.Array, buffer.Offset, result.Count);
					} while (!result.EndOfMessage);

					if (result.MessageType == WebSocketMessageType.Close)
						break;

					ms.Seek(0, SeekOrigin.Begin);
					using (var reader = new StreamReader(ms, Encoding.UTF8))
						Console.WriteLine(await reader.ReadToEndAsync());
				}
			} while (true);
		}

		static async Task Send(ClientWebSocket socket, string data) =>
	await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);


		[Serializable]
        public struct BlocSubscriberSubscription
		{
            public string Type, ApiKey, Encoding, Channel;
            public string[] Markets, Symbols;
		} 
	}
}


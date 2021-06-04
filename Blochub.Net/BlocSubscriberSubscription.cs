using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Blochub_API_C_sharp
{

	[Serializable]
	public class SignUpMassage
	{
		private readonly string type;
		private readonly string apikey;
		private readonly string encoding;
		private readonly string[] symbols;
		private readonly string[] markets;
		private readonly string channel;

		public string Type => type;
		public string Apikey => apikey;
		public string Encoding => encoding;
		public string[] Markets => markets;
		public string[] Symbols => symbols;
		public string Channel => channel;

		[JsonConstructor]
		public SignUpMassage(string type, string apikey, string encoding, string[] symbols, string[] markets, string channel)
		{
			this.type = type;
			this.apikey = apikey;
			this.encoding = encoding;
			this.markets = markets;
			this.symbols = symbols;
			this.channel = channel;
		}
		public string ToJson()
		{
			var serializerSettings = new JsonSerializerSettings();
			serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			return JsonConvert.SerializeObject(this, serializerSettings);

		}
		public Dictionary<string, dynamic> ToDict()
		{
			return new Dictionary<string, dynamic>()
			{
				{ "type","subscribe" },
				{ "apikey", "3JGKGK38D-THIS-IS-SAMPLE-KEY"},
				{ "encoding", "json" },
				{ "symbols", new string[] { "XRP/BTC", "BTC/EUR", "ETH/BTC", "ETH/EUR" } },
				{ "markets", new string[] { "binance", "bitfinex" } },
				{ "channel", "ticker" }
			};
		}
	} 
}


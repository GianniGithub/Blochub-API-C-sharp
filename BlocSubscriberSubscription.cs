using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Blochub_API_C_sharp
{

	[Serializable]
	public class BlocSubscriberSubscription
	{
		private readonly string type;
		private readonly string apiKey;
		private readonly string encoding;
		private readonly string[] symbols;
		private readonly string[] markets;
		private readonly string channel;

		public string Type => type;
		public string ApiKey => apiKey;
		public string Encoding => encoding;
		public string[] Markets => markets;
		public string[] Symbols => symbols;
		public string Channel => channel;

		[JsonConstructor]
		public BlocSubscriberSubscription(string type, string apiKey, string encoding, string[] symbols, string[] markets, string channel)
		{
			this.type = type;
			this.apiKey = apiKey;
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
	} 
}


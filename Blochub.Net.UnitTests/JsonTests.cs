using Blochub_API_C_sharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BlocTests
{
	public class JsonTests
	{
		public string JSONgolangTestCommand = @"{""type"":""subscribe"",""apikey"":""3JGKGK38D-THIS-IS-SAMPLE-KEY"",""encoding"":""json"",""markets"":[""binance"",""bitfinex""],""symbols"":[""XRP/BTC"",""BTC/EUR"",""ETH/BTC"",""ETH/EUR""],""channel"":""ticker""}";
		BlocSubscriberSubscription blocsub;

		[SetUp]
		public void Setup()
		{
			blocsub = new BlocSubscriberSubscription(
				"subscribe",
				"3JGKGK38D-THIS-IS-SAMPLE-KEY",
				"json",
				new string[] { "XRP/BTC", "BTC/EUR", "ETH/BTC", "ETH/EUR" },
				new string[] { "binance", "bitfinex" },
				"ticker"
				);
		}

		[Test]
		public void CompareJSONToGeneralCommand()
		{
			var serializerSettings = new JsonSerializerSettings();
			serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			
			var json = JsonConvert.SerializeObject(blocsub, serializerSettings);



			var TestCommand = JsonConvert.DeserializeObject<BlocSubscriberSubscription>(JSONgolangTestCommand);
			var jsonTestCommand = JsonConvert.SerializeObject(TestCommand, serializerSettings);

			Assert.IsTrue(jsonTestCommand == json);
		}

		[Test]
		public void TestApiKay()
		{
			var serializerSettings = new JsonSerializerSettings();
			serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var json = JsonConvert.SerializeObject(blocsub, serializerSettings);

			var subscription = JsonConvert.DeserializeObject<BlocSubscriberSubscription>(json);
			var dict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

			var clientKey = JsonEncodedText.Encode("3JGKGK38D-THIS-IS-SAMPLE-KEY").ToString();

			//var serveKeyr = dict["apikey"];
			var serveKeyr = subscription.ApiKey;
			Assert.IsTrue(clientKey == serveKeyr);
		}
	}
}
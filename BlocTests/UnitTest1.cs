using Blochub_API_C_sharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System.Text;

namespace BlocTests
{
	public class JsonTests
	{
		public string JSONTestCommand = @"{""type"":""subscribe"",""apikey"":""3JGKGK38D-THIS-IS-SAMPLE-KEY"",""encoding"":""json"",""markets"":[""binance"",""bitfinex""],""symbols"":[""XRP/BTC"",""BTC/EUR"",""ETH/BTC"",""ETH/EUR""],""channel"":""ticker""}";
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



			var TestCommand = JsonConvert.DeserializeObject<BlocSubscriberSubscription>(JSONTestCommand);
			var jsonTestCommand = JsonConvert.SerializeObject(TestCommand, serializerSettings);

			Assert.IsTrue(jsonTestCommand == json);
		}
	}
}
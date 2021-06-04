using Blochub_API_C_sharp;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BlocTests
{
	public class JsonTests
	{
		public string JSONgolangTestCommand = @"{""type"":""subscribe"",""apikey"":""3JGKGK38D-THIS-IS-SAMPLE-KEY"",""encoding"":""json"",""markets"":[""binance"",""bitfinex""],""symbols"":[""XRP/BTC"",""BTC/EUR"",""ETH/BTC"",""ETH/EUR""],""channel"":""ticker""}";
		SignUpMassage blocsub;
		Dictionary<string, dynamic> dictSettings => blockStream.Blocsub;

		[SetUp]
		public void Setup()
		{
			blocsub = new SignUpMassage(
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
			var jsonDict = JsonConvert.SerializeObject(dictSettings, serializerSettings);

			var jdp = new JsonDiffPatch();
			string diffResult = jdp.Diff(json, jsonDict);

			Assert.IsTrue(diffResult == null);
		}

		[Test]
		public void TestApiKay()
		{
			var serializerSettings = new JsonSerializerSettings();
			serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var json = JsonConvert.SerializeObject(blocsub, serializerSettings);

			var subscription = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
			var dict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

			var clientKey = JsonEncodedText.Encode("3JGKGK38D-THIS-IS-SAMPLE-KEY").ToString();

			//var serveKeyr = dict["apikey"];
			var serveKeyr = subscription["apikey"];
			Assert.IsTrue(clientKey == serveKeyr);
		}
		[Test]
		public void CompareCharSet()
		{
			var json = JsonConvert.SerializeObject(dictSettings);

			// Just to check correct char set and UTF character
			Assert.IsFalse(ComparerString(json, JSONgolangTestCommand));
		}
		bool ComparerString(string json, string jsonD)
		{
			var jsonAr = Encoding.UTF8.GetBytes(json);
			var jsonDAr = Encoding.UTF8.GetBytes(jsonD);

			if (jsonAr == jsonDAr)
				return true;

			for (int i = 0; i < json.Length; i++)
			{
				if (jsonAr[i] != jsonDAr[i])
				{
					Console.WriteLine("jsonAr: " + Encoding.UTF8.GetString(jsonAr));
					Console.WriteLine("jsonDAr: " + Encoding.UTF8.GetString(jsonDAr));
					Console.WriteLine("at " + i);
					Console.WriteLine(Encoding.UTF8.GetString(new byte[] { jsonAr[i] }) + " - " + Encoding.UTF8.GetString(new byte[] { jsonDAr[i] }));
				}
			}
			return false;
		}
	}
}
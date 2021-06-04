using Blochub_API_C_sharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;

namespace BlocTests
{
	public class blockStream
	{
		BlocStream stream;
		SignUpMassage blocsub;
		string uri = "ws://163.172.162.138:80/blocfeed";

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

			stream = new BlocStream(uri, blocsub.ToDict());
			stream.KeepConnected = true;

		}

		[Test]
		public async Task TryIfGeneralConnectionIsWorkingAsync()
		{
			await stream.Connect();
			// TODO
			Assert.Pass();
			//Assert.IsTrue(jsonTestCommand == json);
		}
	}
}
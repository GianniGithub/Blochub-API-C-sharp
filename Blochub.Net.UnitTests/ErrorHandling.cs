using Blochub_API_C_sharp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlocTests
{
	class ErrorHandling
	{
		BlocStream stream;
		Dictionary<string, dynamic> blocsub;


		[SetUp]
		public void Setup()
		{
		}

		[TestCase("apikey", @"3JGKGK38D-THIS-IS-WRONG-KEY")] // ExpectedResult = 102)]
		[TestCase("type", "dlkfjs")] 
		public async Task CheckWrongKeyError(string key, dynamic value)
		{
			blocsub = blockStream.Blocsub[key] = value;
			stream = new BlocStream(blockStream.Uri, blocsub);

			Assert.Throws(Is.InstanceOf<BlocStremException>(), async () => await stream.Connect());

		}
	}
}

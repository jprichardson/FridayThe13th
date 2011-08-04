using FridayThe13th;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestFridayThe13th
{

	[TestClass()]
	public class JsonParserTest {

		private TestContext testContextInstance;

		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		[TestInitialize()]
		public void MyTestInitialize()
		{
		}

		[TestCleanup()]
		public void MyTestCleanup()
		{
		}

		[TestMethod()]
		public void ParseTest() {
			
		}
	}
}

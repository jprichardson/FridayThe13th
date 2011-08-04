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
		public void Parse2Test() {
			var jsp = new JsonParser(Properties.Resources.example2);

			dynamic res = jsp.Parse();

			Assert.AreEqual("file", res.menu.id);
			Assert.AreEqual("File", res.menu.value);
			Assert.AreEqual("New", res.menu.popup.menuitems[0].value);
			Assert.AreEqual("Open", res.menu.popup.menuitems[1].value);
			Assert.AreEqual("Close", res.menu.popup.menuitems[2].value);
			Assert.AreEqual("CreateNewDoc()", res.menu.popup.menuitems[0].onclick);
			Assert.AreEqual("OpenDoc()", res.menu.popup.menuitems[1].onclick);
			Assert.AreEqual("CloseDoc()", res.menu.popup.menuitems[2].onclick);
		}
	}
}

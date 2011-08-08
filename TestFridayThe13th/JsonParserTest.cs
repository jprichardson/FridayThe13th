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

		/*[TestMethod()]
		public void SimpleStringParseTest() {
			var jsp = new JsonParser("\"hello\"");
			dynamic res = jsp.Parse();
			Assert.AreEqual("hello", res);
			
			var s = "\"he\\\"llo\"";
			Assert.AreEqual(s, jsp.Parse(s));

		}*/

		[TestMethod()]
		public void SimpleObjectParseTest() {
			var jsp = new JsonParser();
			dynamic obj = jsp.Parse("{\"name\": \"JP\"}");
			Assert.AreEqual("JP", obj.name);

			obj = jsp.Parse("{\"name\":\"Jon Paul\"}");
			Assert.AreEqual("Jon Paul", obj.name);

			obj = jsp.Parse("{\"msg\":\"My name is \\\"JP\\\" ok?\"}");
			Assert.AreEqual("My name is \"JP\" ok?", obj.msg);
		}

		[TestMethod()]
		public void SpecialCharsParseStringsTest() {
			var json = Properties.Resources.json_strings;

			var jsp = new JsonParser();
			dynamic obj = jsp.Parse(json);

			Assert.AreEqual("you are a \"silly\" person.", obj.a);
			Assert.AreEqual("my name is 'JP'", obj.b);
			
		}

		/*[TestMethod()]
		public void Parse2Test() {
			var jsp = new JsonParser(Properties.Resources.example2);

			dynamic res = jsp.Parse();

			var fj = new fastJSON("{\"hello\": \"poo\"");
			var o = fj.Decode();

			Assert.AreEqual("file", res.menu.id);
			Assert.AreEqual("File", res.menu.value);
			Assert.AreEqual("New", res.menu.popup.menuitems[0].value);
			Assert.AreEqual("Open", res.menu.popup.menuitems[1].value);
			Assert.AreEqual("Close", res.menu.popup.menuitems[2].value);
			Assert.AreEqual("CreateNewDoc()", res.menu.popup.menuitems[0].onclick);
			Assert.AreEqual("OpenDoc()", res.menu.popup.menuitems[1].onclick);
			Assert.AreEqual("CloseDoc()", res.menu.popup.menuitems[2].onclick);
		}*/
	}
}

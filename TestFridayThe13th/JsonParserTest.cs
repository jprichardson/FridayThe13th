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
		public void SimpleArrayParseTest() {
			var jsp = new JsonParser();
			dynamic arr = jsp.Parse("[\"a\", \"b\", \"c\"]");
			Assert.AreEqual(3, arr.Count);
			Assert.AreEqual("a", arr[0]);
			Assert.AreEqual("b", arr[1]);
			Assert.AreEqual("c", arr[2]);

			arr = jsp.Parse("[\"a\"]");
			Assert.AreEqual(1, arr.Count);
			Assert.AreEqual("a", arr[0]);

			arr = jsp.Parse("   \t\n[\"a  \",    \"b\", \t\"c\"]\t");
			Assert.AreEqual(3, arr.Count);
			Assert.AreEqual("a  ", arr[0]);
			Assert.AreEqual("b", arr[1]);

			arr = jsp.Parse("\t\n\r [ \r\n\t\t     ]   \n\t");
			Assert.AreEqual(0, arr.Count);
		}

		[TestMethod()]
		public void SimpleObjectParseTest() {
			var jsp = new JsonParser();
			dynamic obj = jsp.Parse("{\"name\": \"JP\"}");
			Assert.AreEqual("JP", obj.name);

			obj = jsp.Parse("{\"name\":\"Jon Paul\"}");
			Assert.AreEqual("Jon Paul", obj.name);

			obj = jsp.Parse("{\"msg\":\"My name is \\\"JP\\\" ok?\"}");
			Assert.AreEqual("My name is \"JP\" ok?", obj.msg);
			Assert.IsFalse(obj.IsEmpty());

			obj = jsp.Parse("    { \r\n    }\t");
			Assert.IsTrue(obj.IsEmpty());
		}

		[TestMethod()]
		public void SpecialCharsParseStringsTest() {
			var json = Properties.Resources.json_strings;

			var jsp = new JsonParser();
			dynamic obj = jsp.Parse(json);

			Assert.AreEqual("you are a \"silly\" person.", obj.a);
			Assert.AreEqual("my name is 'JP'", obj.b);
			Assert.AreEqual("testing a new \n line.", obj.c);
			
		}

		[TestMethod()]
		public void SimpleNumbersParseTest() {
			var jsp = new JsonParser();
			
			//dynamic n =  jsp.Parse("[\"1.0\"]");
			Assert.AreEqual(1.0,  jsp.Parse("1.0"));
			Assert.AreEqual(-1.0, jsp.Parse("-1.0"));
			Assert.AreEqual(1.0, jsp.Parse("1"));
			Assert.AreEqual(-1.0, jsp.Parse("-1"));
			Assert.AreEqual(4.5e10, jsp.Parse("4.5e10"));
			Assert.AreEqual(-4.5e10, jsp.Parse("-4.5e10"));
			Assert.AreEqual(99.0e-10, jsp.Parse("99.0e-10"));
			Assert.AreEqual(-1.343424e-10, jsp.Parse("-1.343424e-10"));
			Assert.AreEqual(4.5e10, jsp.Parse("4.5E10"));
			Assert.AreEqual(-4.5e10, jsp.Parse("-4.5E10"));
			Assert.AreEqual(99.0e-10, jsp.Parse("99.0E-10"));
			Assert.AreEqual(4.5e10, jsp.Parse("4.5e+10"));
			Assert.AreEqual(-4.5e10, jsp.Parse("-4.5e+10"));
		}

		[TestMethod()]
		public void SimpleBooleanNullParseTest() {
			var jsp = new JsonParser();

			Assert.IsTrue(jsp.Parse("true"));
			Assert.IsFalse(jsp.Parse("false"));
			Assert.IsNull(jsp.Parse("null"));
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

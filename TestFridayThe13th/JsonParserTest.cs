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
			Assert.IsFalse(obj.IsEmpty);

			obj = jsp.Parse("    { \r\n    }\t");
			Assert.IsTrue(obj.IsEmpty);
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

		[TestMethod()]
		public void Parse2Test() {
			var jsp = new JsonParser();

			dynamic res = jsp.Parse(Properties.Resources.json_example2);

			Assert.AreEqual("file", res.menu.id);
			Assert.AreEqual("File", res.menu.value);
			Assert.AreEqual("New", res.menu.popup.menuitems[0].value);
			Assert.AreEqual("Open", res.menu.popup.menuitems[1].value);
			Assert.AreEqual("Close", res.menu.popup.menuitems[2].value);
			Assert.AreEqual("CreateNewDoc()", res.menu.popup.menuitems[0].onclick);
			Assert.AreEqual("OpenDoc()", res.menu.popup.menuitems[1].onclick);
			Assert.AreEqual("CloseDoc()", res.menu.popup.menuitems[2].onclick);
		}

		[TestMethod()]
		public void Parse1Test() {
			var jsp = new JsonParser();

			dynamic res = jsp.Parse(Properties.Resources.json_example1);

			Assert.AreEqual(res.glossary.title, "example glossary");
			Assert.AreEqual(res.glossary.GlossDiv.title, "S");
			Assert.AreEqual(res.glossary.GlossDiv.GlossList.GlossEntry.ID, "SGML");
			Assert.AreEqual(res.glossary.GlossDiv.GlossList.GlossEntry.SortAs, "SGML");
			Assert.AreEqual(res.glossary.GlossDiv.GlossList.GlossEntry.GlossTerm, "Standard Generalized Markup Language");
			Assert.AreEqual(res.glossary.GlossDiv.GlossList.GlossEntry.Abbrev, "ISO 8879:1986");
			Assert.AreEqual(res.glossary.GlossDiv.GlossList.GlossEntry.GlossDef.para, "A meta-markup language, used to create markup languages such as DocBook.");
			Assert.AreEqual(res.glossary.GlossDiv.GlossList.GlossEntry.GlossDef.GlossSeeAlso[0], "GML");
			Assert.AreEqual(res.glossary.GlossDiv.GlossList.GlossEntry.GlossDef.GlossSeeAlso[1], "XML");
			Assert.AreEqual(res.glossary.GlossDiv.GlossList.GlossEntry.GlossSee, "markup");
		}

		[TestMethod()]
		public void Parse3Test() {
			var jsp = new JsonParser();

			dynamic res = jsp.Parse(Properties.Resources.json_example3);

			Assert.AreEqual("on", res.widget.debug);
			Assert.AreEqual("Sample Konfabulator Widget", res.widget.window.title);
			Assert.AreEqual(500.0, res.widget.window.width);
			Assert.AreEqual(600, res.widget.window.height);
		}

		[TestMethod()]
		public void Parse4Test() {
			var jsp = new JsonParser();

			dynamic res = jsp.Parse(Properties.Resources.json_example4);

			Assert.AreEqual("org.cofax.cds.CDSServlet", res["web-app"].servlet[0]["servlet-class"]);
			Assert.AreEqual("ksm@pobox.com", res["web-app"].servlet[0]["init-param"]["configGlossary:adminEmail"]);
			Assert.AreEqual(500.0, res["web-app"].servlet[0]["init-param"].maxUrlLength);
			Assert.AreEqual("/WEB-INF/tlds/cofax.tld", res["web-app"].taglib["taglib-location"]);
		}

		[TestMethod()]
		public void Parse5Test() {
			var jsp = new JsonParser();

			dynamic res = jsp.Parse(Properties.Resources.json_example5);

			Assert.AreEqual(8, res.Count);
			Assert.AreEqual("Open", res[0].id);
			Assert.AreEqual("OpenNew", res[1].id); Assert.AreEqual("Open New", res[1]["true"]);
			Assert.AreEqual(null, res[2]);
			Assert.AreEqual(false, res[3].id);
			Assert.AreEqual(true, res[4].label);
			Assert.AreEqual(true, res[5]);
			Assert.AreEqual("Mute", res[6].id);
			Assert.AreEqual(null, res[7]);
		}

		[TestMethod()]
		public void SimpleUnicodeParseTest() {
			var jsp = new JsonParser();

			var res = jsp.Parse(Properties.Resources.json_unicode);

			Assert.AreEqual("÷", res.div);
			Assert.AreEqual("Σ", res.sigma);
			Assert.AreEqual("လ", res.inf);
		}
		
	}
}

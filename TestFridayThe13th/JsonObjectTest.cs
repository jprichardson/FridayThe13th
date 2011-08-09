using FridayThe13th;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestFridayThe13th
{

	[TestClass()]
	public class JsonObjectTest {
		private TestContext testContextInstance;

		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		
		[TestMethod()]
		public void AddTest() {
			dynamic jso = new JsonObject();
			Assert.AreEqual(0, jso.Keys.Count);
			jso.Add("Name", "JP");
			Assert.AreEqual("JP", jso["Name"]);
			Assert.AreEqual("JP", jso.Name);
			Assert.AreEqual(1, jso.Keys.Count);

			jso.Add("Not-Valid", "Yup");
			Assert.AreEqual("Yup", jso["Not-Valid"]);
			Assert.AreEqual(2, jso.Keys.Count);
		}

		[TestMethod()]
		public void SetTest() {
			dynamic jso = new JsonObject();
			Assert.AreEqual(0, jso.Keys.Count);
			jso.IsManager = true;
			Assert.AreEqual(1, jso.Keys.Count);
			Assert.AreEqual(true, jso.IsManager);

			jso["Something"] = 1.0;
			Assert.AreEqual(1.0, jso.Something);
		}

		[TestMethod()]
		public void NotifyChangedText() {
			var propDict = new Dictionary<string, string>();

			JsonObject jso = new JsonObject();
			jso.PropertyChanged += (obj, e) => { propDict.Add(e.PropertyName, ""); };
			
			dynamic djso = jso;

			Assert.IsFalse(propDict.ContainsKey("Name"));
			djso.Name = "JP";
			Assert.IsTrue(propDict.ContainsKey("Name"));
			propDict.Clear();

			Assert.IsFalse(propDict.ContainsKey("Name"));
			djso["Name"] = "JP";
			Assert.IsTrue(propDict.ContainsKey("Name"));
			propDict.Clear();

			djso.RemoveAll();
			Assert.IsFalse(propDict.ContainsKey("Name"));
			djso.Add("Name", "JP");
			Assert.IsTrue(propDict.ContainsKey("Name"));
			propDict.Clear();
		}

		
	}
}

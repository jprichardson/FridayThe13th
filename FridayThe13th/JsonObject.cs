using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.ComponentModel;

namespace FridayThe13th {
	public class JsonObject : DynamicObject, INotifyPropertyChanged   {

		protected Dictionary<string, dynamic> _thisDict;

		public JsonObject(){
			_thisDict = new Dictionary<string, dynamic>();
		}

		public bool IsEmpty { get { return _thisDict.Count == 0; } }
		public IEnumerable<string> Keys { get { return _thisDict.Keys; } }
		public IEnumerable<dynamic> Values { get { return _thisDict.Values; } }

		public dynamic this[string key] { get { return _thisDict[key]; } set { _thisDict[key] = value; NotifyPropertyChanged(key); } }

		public void Add(string key, dynamic value) {
			_thisDict.Add(key, value);
			NotifyPropertyChanged(key);
		}

		public bool Remove(string key) {
			return _thisDict.Remove(key);
		}

		public void RemoveAll() {
			_thisDict.Clear();
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result) {
			return _thisDict.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value) {
			_thisDict[binder.Name] = value;
			NotifyPropertyChanged(binder.Name);
			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string propertyName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}  
	}
}

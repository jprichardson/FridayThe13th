using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Dynamic;
using System.Globalization;

namespace FridayThe13th {
	public class JsonParser {

		private int _line = 0;
		private int _column = 0;

		private string _jsonText = "";
		private int _index = 0;
		private StringBuilder _sb = new StringBuilder();

		public JsonParser() { CamelizeProperties = false; ExceptionOnParsingError = false; }

		public bool CamelizeProperties { get; set; }
		public bool ExceptionOnParsingError { get; set; }

		public int ErrorCount { get { return _errorMessages.Count; } }
		private List<string> _errorMessages = new List<string>();
		public IEnumerable<string> ErrorMessages { get { return _errorMessages; } }

		public dynamic Parse(string json) {
			Reset();
			_jsonText = json;
			return ParseValue();
		}

		public void Reset() {
			_index =_line = _column = 0;
			_errorMessages.Clear();
			_jsonText = "";
		}

		protected List<dynamic> ParseArray() {
			Read(); //read first [

			var list = new List<dynamic>();

			var doRead = true;
			while (doRead) {
				ReadWhitespace();
				switch (Peek()) {
					case -1: ParseError("Unterminated array before end of json string."); return list;
					case ',':
						Read();
						break;
					case ']':
						Read();
						doRead = false;
						break;
					default:
						var val = ParseValue();
						list.Add(val);
						break;
				}
			}

			return list;
		}

		protected dynamic ParseObject() {
			Read(); //read first {
			ReadWhitespace();

			dynamic obj = new JsonObject();

			var doRead = true;
			while (doRead) {
				switch (Peek()) {
					case -1: ParseError("Unterminated object before end of json string."); return obj;
					case ',':
						Read();
						break;
					case '}':
						Read();
						doRead = false;
						break;
					case '"':
						var key = ParseString();
						if (CamelizeProperties) {
							_sb.Clear();
							if (key.Contains('_')) {
								var words = key.Split('_');
								foreach (var w in words) {
									_sb.Append(w.Substring(0, 1).ToUpper());
									_sb.Append(w.Substring(1, w.Length - 1).ToLower());
								}
							} else {
								_sb.Append(key.Substring(0, 1).ToUpper());
								_sb.Append(key.Substring(1, key.Length - 1).ToLower());
							}

							key = _sb.ToString();
						}
						ReadWhitespace();
						ReadExpect(':');
						ReadWhitespace();
						var val = ParseValue();
						obj[key] = val;
						break;
					default:
						ReadWhitespace();
						break;
				}
			}

			return obj;
		}

		//not very robust.... yet
		////http://ecma262-5.com/ELS5_HTML.htm#Section_8.5
		protected double ParseNumber() {
			_sb.Clear();

			bool doRead = true;
			while (doRead) {
				var c = Peek();
				switch (c) {
					case '.': case '-': case '+': case 'e': case 'E':
						_sb.Append((char)c);
						Read();
						doRead = true;
						break;
					default:
						if (c >= '0' && c <= '9') {
							_sb.Append((char)c);
							Read();
							doRead = true;
						} else
							doRead = false;
						break;
				}
			}

			double number;
			bool couldParse = Double.TryParse(_sb.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out number);
			if (!couldParse) {
				number = Double.NaN;
				ParseError(string.Format("Could not parse {0} into a Double.", number));
			}

			return number;
		}

		protected string ParseString() {
			Read(); //read first "
			_sb.Clear();

			bool complete = false;
			while (!complete) {
				var c = Read();
				switch (c) {
					case -1: ParseError("Unterminated string before end of json string."); return _sb.ToString();
					case '"': complete = true; break;
					case '\\':
						var nc = Read();
						switch (nc) {
							case '"':
							case '\\':
							case '/':
								_sb.Append((char)nc);
								break;
							case 'b':
								_sb.Append('\b');
								break;
							case 'f':
								_sb.Append('\f');
								break;
							case 'n':
								_sb.Append('\n');
								break;
							case 'r':
								_sb.Append('\r');
								break;
							case 't':
								_sb.Append('\t');
								break;
							case 'u':
								ushort cp = 0;
								for (int i = 0; i < 4; i++) {
									if ((c = Read()) < 0)
										ParseError("Incomplete unicode.");
									cp *= 16;
									if ('0' <= c && c <= '9')
										cp += (ushort)(c - '0');
									if ('A' <= c && c <= 'F')
										cp += (ushort)(c - 'A' + 10);
									if ('a' <= c && c <= 'f')
										cp += (ushort)(c - 'a' + 10);
								}
								_sb.Append((char)cp);
								break;
						}
						break;
					default:
						_sb.Append((char)c);
						break;
				}
			}

			return _sb.ToString();
		}

		protected dynamic ParseValue() {
			ReadWhitespace();

			var c = Peek();
			switch (c) {
				case -1: ParseError("Unknown parsing error. Premature end of json string."); return null;
				case '{': return ParseObject();
				case '"': return ParseString();
				case '[': return ParseArray();
				case '-': return ParseNumber();
				case 't': if (TryRead("true")) { return true; } else { goto default; }
				case 'f': if (TryRead("false")) { return false; } else { goto default; }
				case 'n': if (TryRead("null")) { return null; } else { goto default; }
				default:
					if (c >= '0' && c <= '9')
						return ParseNumber();
					else {
						ParseError("Unrecognized JSON character token.");
						Read(); //get rid of bad character, go to next
						return ParseValue();
					}
			}
		}

		protected int Peek() {
			if (_index == _jsonText.Length)
				return -1; //EOF
			else
				return _jsonText[_index];
		}

		protected int Read() {
			if (_index == _jsonText.Length)
				return -1;
			else {
				int c = _jsonText[_index++];
				if (c == '\n') {
					_line++;
					_column = 0;
				} else
					_column++;

				return c;
			}
		}

		protected void ReadExpect(char c) {
			var expect = Read();
			if (expect == -1)
				ParseError(string.Format("Expected {0} but is at the end of the string.", c));
			else
				if (expect != c)
					ParseError(string.Format("Expected {0} but received {1}.", c, expect));
		}

		protected void ReadWhitespace() {
			bool doRead = true;
			while (doRead) {
				switch (Peek()) {
					case ' ':
					case '\t':
					case '\r':
					case '\n':
						Read();
						break;
					default: doRead = false; break;
				}
			} 
		}

		protected bool TryRead(string s) {
			bool success = true;
			for (var i = 0; i < s.Length; ++i)
				if (s[i] != Read()) {
					_index = _index - i - 1;
					success = false;
					break;
				}
			return success;
		}

		protected void ParseError(string msg) {
			msg = string.Format("{0} ({1},{2})", msg, _line, _column);
			if (ExceptionOnParsingError)
				throw new Exception(msg);
			else
				_errorMessages.Add(msg);
		}

	}
}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using Renko.Diagnostics;
using Renko.Extensions;
using Renko.Reflection;
using Renko.Data.Internal;

namespace Renko.Data
{
	/// <summary>
	/// A class that parses json string into a JsonData object.
	/// Used MiniJSON for reference.
	/// </summary>
	public class JsonDeserializer : IDisposable {

		/// <summary>
		/// Chracters that should be ignored in general.
		/// </summary>
		private const string WhiteSpaces = " \t\n\r";

		/// <summary>
		/// Characters that can break parsing context.
		/// </summary>
		private const string WordBreaks = " \t\n\r{}[],:\"";

		/// <summary>
		/// Contains the string to parse.
		/// </summary>
		private StringReader json;


		/// <summary>
		/// Returns the next readable character.
		/// </summary>
		private char PeekChar {
			get { return Convert.ToChar(json.Peek()); }
		}

		/// <summary>
		/// Returns the next read character.
		/// </summary>
		private char NextChar {
			get { return Convert.ToChar(json.Read()); }
		}

		/// <summary>
		/// Returns the next read word.
		/// </summary>
		private string NextWord {
			get {
				if (json.Peek() == -1)
					return null;
				StringBuilder word = new StringBuilder();
				while (WordBreaks.IndexOf(PeekChar) == -1) {
					word.Append(NextChar);
					if (json.Peek() == -1)
						break;
				}
				return word.ToString();
			}
		}

		/// <summary>
		/// Returns the next read token.
		/// </summary>
		private Token NextToken {
			get {
				EatWhitespace();
				if (json.Peek() == -1)
					return Token.NONE;

				char c = PeekChar;
				switch (c) {
				case '{':
					return Token.CURLY_OPEN;
				case '}':
					json.Read();
					return Token.CURLY_CLOSE;
				case '[':
					return Token.SQUARED_OPEN;
				case ']':
					json.Read();
					return Token.SQUARED_CLOSE;
				case ',':
					json.Read();
					return Token.COMMA;
				case '"':
					return Token.STRING;
				case ':':
					return Token.COLON;
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case '-':
					return Token.NUMBER;
				}

				//If none of the above, we should check the whole 'word'.
				string word = NextWord;
				switch(word) {
				case "null":
					return Token.NULL;
				case "false":
					return Token.FALSE;
				case "true":
					return Token.TRUE;
				}
				return Token.NONE;
			}
		}


		private JsonDeserializer(string jsonString) {
			json = new StringReader(jsonString);
		}

		/// <summary>
		/// Returns a JsonData object from given string.
		/// </summary>
		public static JsonData Parse(string str) {
			using(var parser = new JsonDeserializer(str)) {
				return new JsonData(parser.ParseValue());
			}
		}

		/// <summary>
		/// Deserializes the specified JsonData for a specific type.
		/// Highly recommended to use Json.Parse with Type parameter instead.
		/// </summary>
		public static object Deserialize(Type t, object instance, JsonObject data) {
			object adaptorResult = JsonAdaptor.Deserialize(t, data);
			if(adaptorResult != null) {
				return adaptorResult;
			}

			//IJsonable method requires an instance to be present.
			if(instance == null) {
				instance = DynamicService.CreateObject(t);
				if(instance == null) {
					RenLog.Log(LogLevel.Error, string.Format(
						"JsonDeserializer.Deserialize - Failed to instantiate a dynamic object of type ({0}). If possible, try adding a parameterless constructor.",
						t.Name
					));
					return null;
				}
			}

			IJsonable jsonable = instance as IJsonable;
			if(jsonable != null) {
				jsonable.FromJsonObject(data);
				return instance;
			}

			//No deserializer is available.
			RenLog.Log(LogLevel.Warning, string.Format(
				"JsonDeserializer.Deserialize - There is no deserializer available for type ({0}). Returning null."
			));
			return null;
		}

		public void Dispose() {
			json.Dispose();
			json = null;
		}

		/// <summary>
		/// Returns an object that contains a parsed value.
		/// </summary>
		object ParseByToken(Token token) {
			switch (token) {
			case Token.STRING:
				return ParseString();
			case Token.NUMBER:
				return ParseNumber();
			case Token.CURLY_OPEN:
				return ParseObject();
			case Token.SQUARED_OPEN:
				return ParseArray();
			case Token.TRUE:
				return true;
			case Token.FALSE:
				return false;
			case Token.NULL:
				return null;
			default:
				return null;
			}
		}

		/// <summary>
		/// Returns a parsed JsonObject.
		/// </summary>
		JsonObject ParseObject() {
			var obj = new JsonObject();
			//Skip opening brace
			json.Read();
			while (true) {
				switch (NextToken) {
				case Token.NONE:
					return null;
				case Token.COMMA:
					continue;
				case Token.CURLY_CLOSE:
					return obj;
				default:
					string key = ParseString();
					if (key == null)
						return null;
					if (NextToken != Token.COLON)
						return null;
					//Skip the colon
					json.Read();
					obj[key] = new JsonData(ParseValue());
					break;
				}
			}
		}

		/// <summary>
		/// Returns a parsed JsonArray.
		/// </summary>
		JsonArray ParseArray() {
			var array = new JsonArray();
			//Skip opening bracket
			json.Read();
			while (true) {
				Token token = NextToken;
				switch (token) {
				case Token.NONE:
					return null;
				case Token.COMMA:
					continue;
				case Token.SQUARED_CLOSE:
					return array;
				default:
					array.Add(new JsonData(ParseByToken(token)));
					break;
				}
			}
		}

		/// <summary>
		/// Returns an object from next token.
		/// </summary>
		object ParseValue() {
			return ParseByToken(NextToken);
		}

		/// <summary>
		/// Returns an unescaped string value.
		/// </summary>
		string ParseString() {
			StringBuilder s = new StringBuilder();
			char c;
			bool parsing = true;
			//Skip opening quote
			json.Read();
			while (parsing) {
				if (json.Peek() == -1) {
					parsing = false;
					break;
				}

				c = NextChar;
				switch (c) {
				case '"':
					parsing = false;
					break;

				case '\\': {
						if (json.Peek() == -1) {
							parsing = false;
							break;
						}

						c = NextChar;
						switch (c) {
						case '"':
						case '\\':
						case '/':
							s.Append(c);
							break;
						case 'b':
							s.Append('\b');
							break;
						case 'f':
							s.Append('\f');
							break;
						case 'n':
							s.Append('\n');
							break;
						case 'r':
							s.Append('\r');
							break;
						case 't':
							s.Append('\t');
							break;
						case 'u':
							var hex = new StringBuilder();
							for (int i=0; i< 4; i++)
								hex.Append(NextChar);
							s.Append((char) Convert.ToInt32(hex.ToString(), 16));
							break;
						}
					}
					break;

				default:
					s.Append(c);
					break;
				}
			}
			return s.ToString();
		}

		/// <summary>
		/// Returns an object that contains numeric (long/double) value.
		/// </summary>
		object ParseNumber() {
			string number = NextWord;
			//No decimal point means it should be parsed to long value.
			if (number.IndexOf('.') == -1) {
				long parsedInt;
				long.TryParse(number, out parsedInt);
				return parsedInt;
			}
			double parsedDouble;
			double.TryParse(number, out parsedDouble);
			return parsedDouble;
		}
		
		/// <summary>
		/// Removes all characters considered a 'white space'.
		/// </summary>
		void EatWhitespace() {
			if (json.Peek() == -1)
				return;
			while (WhiteSpaces.IndexOf(PeekChar) != -1) {
				json.Read();
				if (json.Peek() == -1)
					break;
			}
		}
		
		/// <summary>
		/// Type specification for some string/chars.
		/// </summary>
		private enum Token {
			NONE,
			CURLY_OPEN,
			CURLY_CLOSE,
			SQUARED_OPEN,
			SQUARED_CLOSE,
			COLON,
			COMMA,
			STRING,
			NUMBER,
			TRUE,
			FALSE,
			NULL
		};
	}
}


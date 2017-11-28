using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using Renko.Debug;
using Renko.Extensions;

namespace Renko.Utility
{
	/// <summary>
	/// A class that parses json string into a JsonData object.
	/// Used MiniJSON for reference.
	/// </summary>
	public class JsonParser : IDisposable {

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


		#region Properties
		/// <summary>
		/// Returns the next readable character.
		/// </summary>
		private char PeekChar { get { return Convert.ToChar(json.Peek()); } }

		/// <summary>
		/// Returns the next read character.
		/// </summary>
		private char NextChar { get { return Convert.ToChar(json.Read()); } }

		/// <summary>
		/// Returns the next read word.
		/// </summary>
		private string NextWord
		{
			get
			{
				//If end of string, return null
				if (json.Peek() == -1)
					return null;
				
				StringBuilder word = new StringBuilder();

				//While next readable character doesn't break the word
				while (WordBreaks.IndexOf(PeekChar) == -1)
				{
					//Add the next character
					word.Append(NextChar);

					//If end of string, break
					if (json.Peek() == -1)
						break;
				}

				//Return word
				return word.ToString();
			}
		}

		/// <summary>
		/// Returns the next read token.
		/// </summary>
		/// <value>The next token.</value>
		private Token NextToken
		{
			get
			{
				//Remove white spaces
				EatWhitespace();

				//If end of reader, return nothing
				if (json.Peek() == -1)
					return Token.NONE;

				//Determine special chars or numbers
				char c = PeekChar;
				switch (c)
				{
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

				//If none of the above, we should check the word.
				string word = NextWord;

				//Determine token
				switch(word)
				{
				case "null":
					return Token.NULL;
				case "false":
					return Token.FALSE;
				case "true":
					return Token.TRUE;
				}

				//Nothing found.
				return Token.NONE;
			}
		}
		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Renko.Utility.JsonParser"/> class.
		/// </summary>
		private JsonParser(string jsonString) { json = new StringReader(jsonString); }
		#endregion

		#region IDisposable
		/// <summary>
		/// Releases all resource used by the <see cref="Renko.Utility.JsonParser"/> object.
		/// </summary>
		public void Dispose()
		{
			json.Dispose();
			json = null;
		}
		#endregion

		#region Core
		/// <summary>
		/// Returns an object that contains a parsed value.
		/// </summary>
		object ParseByToken(Token token)
		{
			//Different parser based on token
			switch (token)
			{
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
		JsonObject ParseObject()
		{
			//Instantiate new json object
			var obj = new JsonObject();

			//Skip opening brace
			json.Read();

			while (true)
			{
				//Determine action
				switch (NextToken)
				{
				case Token.NONE:
					return null;
				case Token.COMMA:
					continue;
				case Token.CURLY_CLOSE:
					return obj;
				default:
					//Parse key
					string key = ParseString();
					if (key == null)
						return null;

					//If next token is not a colon, just return null
					if (NextToken != Token.COLON)
						return null;

					//Skip the colon
					json.Read();

					//Parse value and set to key
					obj[key] = new JsonData(ParseValue());
					break;
				}
			}
		}

		/// <summary>
		/// Returns a parsed JsonArray.
		/// </summary>
		JsonArray ParseArray()
		{
			//Instantiate new json array
			var array = new JsonArray();

			//Skip opening bracket
			json.Read();

			while (true)
			{
				//Get next token
				Token token = NextToken;
				//Determine action
				switch (token)
				{
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
		object ParseValue() { return ParseByToken(NextToken); }

		/// <summary>
		/// Returns an unescaped string value.
		/// </summary>
		string ParseString()
		{
			//States
			StringBuilder s = new StringBuilder();
			char c;
			bool parsing = true;

			//Skip opening quote
			json.Read();

			//While parsing flag is true
			while (parsing)
			{
				//If end of reader, no more parsing
				if (json.Peek() == -1)
				{
					parsing = false;
					break;
				}

				//Determining actions
				c = NextChar;
				switch (c) {
				case '"':
					parsing = false;
					break;

				case '\\':
					{
						//If end of reader, no more parsing
						if (json.Peek() == -1)
						{
							parsing = false;
							break;
						}

						//Unescaping process
						c = NextChar;
						switch (c)
						{
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

			//Return parsed value
			return s.ToString();
		}

		/// <summary>
		/// Returns an object that contains numeric (long/double) value.
		/// </summary>
		object ParseNumber()
		{
			//Parse number string
			string number = NextWord;

			//If a decimal point is not included
			if (number.IndexOf('.') == -1)
			{
				//Parse the word as long value.
				long parsedInt;
				Int64.TryParse(number, out parsedInt);
				return parsedInt;
			}

			//If decimal point is included, parse as double value.
			double parsedDouble;
			Double.TryParse(number, out parsedDouble);
			return parsedDouble;
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Removes all characters considered a 'white space'.
		/// </summary>
		void EatWhitespace()
		{
			//If end of reader, return
			if (json.Peek() == -1)
				return;
			
			//If a white space
			while (WhiteSpaces.IndexOf(PeekChar) != -1)
			{
				//Advance the reader
				json.Read();

				//If end of reader, break
				if (json.Peek() == -1)
					break;
			}
		}
		#endregion

		#region Public static
		/// <summary>
		/// Returns a JsonData object from given string.
		/// </summary>
		public static JsonData Parse(string str)
		{
			//Create new parser
			using(var parser = new JsonParser(str))
			{
				return new JsonData(parser.ParseValue());
			}
		}
		#endregion

		#region Enums
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
		#endregion
	}
}


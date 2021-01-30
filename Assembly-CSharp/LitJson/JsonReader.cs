using System;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x02000272 RID: 626
	public class JsonReader
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x00074B9E File Offset: 0x00072D9E
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x00074BAB File Offset: 0x00072DAB
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x00074BB9 File Offset: 0x00072DB9
		// (set) Token: 0x06001451 RID: 5201 RVA: 0x00074BC6 File Offset: 0x00072DC6
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x00074BD4 File Offset: 0x00072DD4
		// (set) Token: 0x06001453 RID: 5203 RVA: 0x00074BDC File Offset: 0x00072DDC
		public bool SkipNonMembers
		{
			get
			{
				return this.skip_non_members;
			}
			set
			{
				this.skip_non_members = value;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x00074BE5 File Offset: 0x00072DE5
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x00074BED File Offset: 0x00072DED
		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x00074BF5 File Offset: 0x00072DF5
		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x00074BFD File Offset: 0x00072DFD
		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00074C05 File Offset: 0x00072E05
		static JsonReader()
		{
			JsonReader.PopulateParseTable();
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x00074C0C File Offset: 0x00072E0C
		public JsonReader(string json_text) : this(new StringReader(json_text), true)
		{
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00074C1B File Offset: 0x00072E1B
		public JsonReader(TextReader reader) : this(reader, false)
		{
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00074C28 File Offset: 0x00072E28
		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.skip_non_members = true;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00074CB8 File Offset: 0x00072EB8
		private static void PopulateParseTable()
		{
			JsonReader.parse_table = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(ParserToken.Array);
			JsonReader.TableAddCol(ParserToken.Array, 91, new int[]
			{
				91,
				65549
			});
			JsonReader.TableAddRow(ParserToken.ArrayPrime);
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 34, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 91, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 93, new int[]
			{
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 123, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65537, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65538, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65539, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(ParserToken.ArrayPrime, 65540, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddRow(ParserToken.Object);
			JsonReader.TableAddCol(ParserToken.Object, 123, new int[]
			{
				123,
				65545
			});
			JsonReader.TableAddRow(ParserToken.ObjectPrime);
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 34, new int[]
			{
				65546,
				65547,
				125
			});
			JsonReader.TableAddCol(ParserToken.ObjectPrime, 125, new int[]
			{
				125
			});
			JsonReader.TableAddRow(ParserToken.Pair);
			JsonReader.TableAddCol(ParserToken.Pair, 34, new int[]
			{
				65552,
				58,
				65550
			});
			JsonReader.TableAddRow(ParserToken.PairRest);
			JsonReader.TableAddCol(ParserToken.PairRest, 44, new int[]
			{
				44,
				65546,
				65547
			});
			JsonReader.TableAddCol(ParserToken.PairRest, 125, new int[]
			{
				65554
			});
			JsonReader.TableAddRow(ParserToken.String);
			JsonReader.TableAddCol(ParserToken.String, 34, new int[]
			{
				34,
				65541,
				34
			});
			JsonReader.TableAddRow(ParserToken.Text);
			JsonReader.TableAddCol(ParserToken.Text, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(ParserToken.Text, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddRow(ParserToken.Value);
			JsonReader.TableAddCol(ParserToken.Value, 34, new int[]
			{
				65552
			});
			JsonReader.TableAddCol(ParserToken.Value, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(ParserToken.Value, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddCol(ParserToken.Value, 65537, new int[]
			{
				65537
			});
			JsonReader.TableAddCol(ParserToken.Value, 65538, new int[]
			{
				65538
			});
			JsonReader.TableAddCol(ParserToken.Value, 65539, new int[]
			{
				65539
			});
			JsonReader.TableAddCol(ParserToken.Value, 65540, new int[]
			{
				65540
			});
			JsonReader.TableAddRow(ParserToken.ValueRest);
			JsonReader.TableAddCol(ParserToken.ValueRest, 44, new int[]
			{
				44,
				65550,
				65551
			});
			JsonReader.TableAddCol(ParserToken.ValueRest, 93, new int[]
			{
				65554
			});
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00075031 File Offset: 0x00073231
		private static void TableAddCol(ParserToken row, int col, params int[] symbols)
		{
			JsonReader.parse_table[(int)row].Add(col, symbols);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00075045 File Offset: 0x00073245
		private static void TableAddRow(ParserToken rule)
		{
			JsonReader.parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00075058 File Offset: 0x00073258
		private void ProcessNumber(string number)
		{
			double num;
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, out num))
			{
				this.token = JsonToken.Double;
				this.token_value = num;
				return;
			}
			int num2;
			if (int.TryParse(number, out num2))
			{
				this.token = JsonToken.Int;
				this.token_value = num2;
				return;
			}
			long num3;
			if (long.TryParse(number, out num3))
			{
				this.token = JsonToken.Long;
				this.token_value = num3;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x000750F4 File Offset: 0x000732F4
		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
					return;
				}
				if (this.token == JsonToken.None)
				{
					this.token = JsonToken.String;
				}
				this.parser_in_string = true;
				return;
			}
			else
			{
				if (this.current_symbol == 65541)
				{
					this.token_value = this.lexer.StringValue;
					return;
				}
				if (this.current_symbol == 65539)
				{
					this.token = JsonToken.Boolean;
					this.token_value = false;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65540)
				{
					this.token = JsonToken.Null;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65537)
				{
					this.ProcessNumber(this.lexer.StringValue);
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65546)
				{
					this.token = JsonToken.PropertyName;
					return;
				}
				if (this.current_symbol == 65538)
				{
					this.token = JsonToken.Boolean;
					this.token_value = true;
					this.parser_return = true;
				}
				return;
			}
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00075266 File Offset: 0x00073466
		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x000752A5 File Offset: 0x000734A5
		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				this.reader.Dispose();
			}
			this.reader = null;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000752D8 File Offset: 0x000734D8
		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] array;
					try
					{
						array = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException inner_exception)
					{
						throw new JsonException((ParserToken)this.current_input, inner_exception);
					}
					if (array[0] != 65554)
					{
						for (int i = array.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(array[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		// Token: 0x0400132E RID: 4910
		private static IDictionary<int, IDictionary<int, int[]>> parse_table;

		// Token: 0x0400132F RID: 4911
		private Stack<int> automaton_stack;

		// Token: 0x04001330 RID: 4912
		private int current_input;

		// Token: 0x04001331 RID: 4913
		private int current_symbol;

		// Token: 0x04001332 RID: 4914
		private bool end_of_json;

		// Token: 0x04001333 RID: 4915
		private bool end_of_input;

		// Token: 0x04001334 RID: 4916
		private Lexer lexer;

		// Token: 0x04001335 RID: 4917
		private bool parser_in_string;

		// Token: 0x04001336 RID: 4918
		private bool parser_return;

		// Token: 0x04001337 RID: 4919
		private bool read_started;

		// Token: 0x04001338 RID: 4920
		private TextReader reader;

		// Token: 0x04001339 RID: 4921
		private bool reader_is_owned;

		// Token: 0x0400133A RID: 4922
		private bool skip_non_members;

		// Token: 0x0400133B RID: 4923
		private object token_value;

		// Token: 0x0400133C RID: 4924
		private JsonToken token;
	}
}

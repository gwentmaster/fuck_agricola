using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x02000275 RID: 629
	public class JsonWriter
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x00075444 File Offset: 0x00073644
		// (set) Token: 0x06001466 RID: 5222 RVA: 0x0007544C File Offset: 0x0007364C
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x0007546A File Offset: 0x0007366A
		// (set) Token: 0x06001468 RID: 5224 RVA: 0x00075472 File Offset: 0x00073672
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x0007547B File Offset: 0x0007367B
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x00075483 File Offset: 0x00073683
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x0007548B File Offset: 0x0007368B
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x000754A0 File Offset: 0x000736A0
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000754CA File Offset: 0x000736CA
		public JsonWriter(StringBuilder sb) : this(new StringWriter(sb))
		{
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x000754D8 File Offset: 0x000736D8
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x000754FC File Offset: 0x000736FC
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x00075620 File Offset: 0x00073820
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x00075684 File Offset: 0x00073884
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x000756C5 File Offset: 0x000738C5
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x000756E4 File Offset: 0x000738E4
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00075730 File Offset: 0x00073930
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0007573C File Offset: 0x0007393C
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00075798 File Offset: 0x00073998
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			int i = 0;
			while (i < length)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				case '\v':
					goto IL_E4;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						goto IL_E4;
					}
					this.writer.Write('\\');
					this.writer.Write(str[i]);
					break;
				}
				IL_141:
				i++;
				continue;
				IL_E4:
				if (str[i] >= ' ' && str[i] <= '~')
				{
					this.writer.Write(str[i]);
					goto IL_141;
				}
				JsonWriter.IntToHex((int)str[i], this.hex_seq);
				this.writer.Write("\\u");
				this.writer.Write(this.hex_seq);
				goto IL_141;
			}
			this.writer.Write('"');
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x000758FE File Offset: 0x00073AFE
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0007591B File Offset: 0x00073B1B
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00075938 File Offset: 0x00073B38
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00075993 File Offset: 0x00073B93
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(boolean ? "true" : "false");
			this.context.ExpectingValue = false;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x000759C3 File Offset: 0x00073BC3
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x000759F0 File Offset: 0x00073BF0
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00075A4F File Offset: 0x00073C4F
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00075A7B File Offset: 0x00073C7B
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00075AA7 File Offset: 0x00073CA7
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00075AD9 File Offset: 0x00073CD9
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00075B08 File Offset: 0x00073D08
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00075B74 File Offset: 0x00073D74
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x00075BC8 File Offset: 0x00073DC8
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00075C34 File Offset: 0x00073E34
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00075C88 File Offset: 0x00073E88
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				for (int i = this.context.Padding - property_name.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x04001348 RID: 4936
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x04001349 RID: 4937
		private WriterContext context;

		// Token: 0x0400134A RID: 4938
		private Stack<WriterContext> ctx_stack;

		// Token: 0x0400134B RID: 4939
		private bool has_reached_end;

		// Token: 0x0400134C RID: 4940
		private char[] hex_seq;

		// Token: 0x0400134D RID: 4941
		private int indentation;

		// Token: 0x0400134E RID: 4942
		private int indent_value;

		// Token: 0x0400134F RID: 4943
		private StringBuilder inst_string_builder;

		// Token: 0x04001350 RID: 4944
		private bool pretty_print;

		// Token: 0x04001351 RID: 4945
		private bool validate;

		// Token: 0x04001352 RID: 4946
		private TextWriter writer;
	}
}

using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020005F3 RID: 1523
	public sealed class HeaderValue
	{
		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060037E4 RID: 14308 RVA: 0x00112BE4 File Offset: 0x00110DE4
		// (set) Token: 0x060037E5 RID: 14309 RVA: 0x00112BEC File Offset: 0x00110DEC
		public string Key { get; set; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x00112BF5 File Offset: 0x00110DF5
		// (set) Token: 0x060037E7 RID: 14311 RVA: 0x00112BFD File Offset: 0x00110DFD
		public string Value { get; set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060037E8 RID: 14312 RVA: 0x00112C06 File Offset: 0x00110E06
		// (set) Token: 0x060037E9 RID: 14313 RVA: 0x00112C0E File Offset: 0x00110E0E
		public List<HeaderValue> Options { get; set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060037EA RID: 14314 RVA: 0x00112C17 File Offset: 0x00110E17
		public bool HasValue
		{
			get
			{
				return !string.IsNullOrEmpty(this.Value);
			}
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x00003425 File Offset: 0x00001625
		public HeaderValue()
		{
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x00112C27 File Offset: 0x00110E27
		public HeaderValue(string key)
		{
			this.Key = key;
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x00112C36 File Offset: 0x00110E36
		public void Parse(string headerStr, ref int pos)
		{
			this.ParseImplementation(headerStr, ref pos, true);
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x00112C44 File Offset: 0x00110E44
		public bool TryGetOption(string key, out HeaderValue option)
		{
			option = null;
			if (this.Options == null || this.Options.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.Options.Count; i++)
			{
				if (string.Equals(this.Options[i].Key, key, StringComparison.OrdinalIgnoreCase))
				{
					option = this.Options[i];
					return true;
				}
			}
			return false;
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x00112CAC File Offset: 0x00110EAC
		private void ParseImplementation(string headerStr, ref int pos, bool isOptionIsAnOption)
		{
			string key = headerStr.Read(ref pos, (char ch) => ch != ';' && ch != '=' && ch != ',', true);
			this.Key = key;
			char? c = headerStr.Peek(pos - 1);
			char? c2 = c;
			int? num = (c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null;
			int num2 = 61;
			bool flag = num.GetValueOrDefault() == num2 & num != null;
			bool flag2;
			if (isOptionIsAnOption)
			{
				c2 = c;
				num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
				num2 = 59;
				flag2 = (num.GetValueOrDefault() == num2 & num != null);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			while ((c != null && flag) || flag3)
			{
				if (flag)
				{
					string value = headerStr.ReadPossibleQuotedText(ref pos);
					this.Value = value;
				}
				else if (flag3)
				{
					HeaderValue headerValue = new HeaderValue();
					headerValue.ParseImplementation(headerStr, ref pos, false);
					if (this.Options == null)
					{
						this.Options = new List<HeaderValue>();
					}
					this.Options.Add(headerValue);
				}
				if (!isOptionIsAnOption)
				{
					return;
				}
				c = headerStr.Peek(pos - 1);
				c2 = c;
				num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
				num2 = 61;
				flag = (num.GetValueOrDefault() == num2 & num != null);
				bool flag4;
				if (isOptionIsAnOption)
				{
					c2 = c;
					num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
					num2 = 59;
					flag4 = (num.GetValueOrDefault() == num2 & num != null);
				}
				else
				{
					flag4 = false;
				}
				flag3 = flag4;
			}
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x00112E65 File Offset: 0x00111065
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.Value))
			{
				return this.Key + '=' + this.Value;
			}
			return this.Key;
		}
	}
}

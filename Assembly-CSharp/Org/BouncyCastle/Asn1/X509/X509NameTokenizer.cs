using System;
using System.Text;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200053B RID: 1339
	public class X509NameTokenizer
	{
		// Token: 0x060030EF RID: 12527 RVA: 0x000FA6E9 File Offset: 0x000F88E9
		public X509NameTokenizer(string oid) : this(oid, ',')
		{
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x000FA6F4 File Offset: 0x000F88F4
		public X509NameTokenizer(string oid, char separator)
		{
			this.value = oid;
			this.index = -1;
			this.separator = separator;
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000FA71C File Offset: 0x000F891C
		public bool HasMoreTokens()
		{
			return this.index != this.value.Length;
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000FA734 File Offset: 0x000F8934
		public string NextToken()
		{
			if (this.index == this.value.Length)
			{
				return null;
			}
			int num = this.index + 1;
			bool flag = false;
			bool flag2 = false;
			this.buffer.Remove(0, this.buffer.Length);
			while (num != this.value.Length)
			{
				char c = this.value[num];
				if (c == '"')
				{
					if (!flag2)
					{
						flag = !flag;
					}
					else
					{
						this.buffer.Append(c);
						flag2 = false;
					}
				}
				else if (flag2 || flag)
				{
					if (c == '#' && this.buffer[this.buffer.Length - 1] == '=')
					{
						this.buffer.Append('\\');
					}
					else if (c == '+' && this.separator != '+')
					{
						this.buffer.Append('\\');
					}
					this.buffer.Append(c);
					flag2 = false;
				}
				else if (c == '\\')
				{
					flag2 = true;
				}
				else
				{
					if (c == this.separator)
					{
						break;
					}
					this.buffer.Append(c);
				}
				num++;
			}
			this.index = num;
			return this.buffer.ToString().Trim();
		}

		// Token: 0x04001F65 RID: 8037
		private string value;

		// Token: 0x04001F66 RID: 8038
		private int index;

		// Token: 0x04001F67 RID: 8039
		private char separator;

		// Token: 0x04001F68 RID: 8040
		private StringBuilder buffer = new StringBuilder();
	}
}

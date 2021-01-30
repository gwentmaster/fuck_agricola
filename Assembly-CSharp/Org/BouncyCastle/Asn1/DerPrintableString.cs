using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004FD RID: 1277
	public class DerPrintableString : DerStringBase
	{
		// Token: 0x06002F0D RID: 12045 RVA: 0x000F3AB3 File Offset: 0x000F1CB3
		public static DerPrintableString GetInstance(object obj)
		{
			if (obj == null || obj is DerPrintableString)
			{
				return (DerPrintableString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000F3ADC File Offset: 0x000F1CDC
		public static DerPrintableString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerPrintableString)
			{
				return DerPrintableString.GetInstance(@object);
			}
			return new DerPrintableString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000F3B12 File Offset: 0x000F1D12
		public DerPrintableString(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
		{
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000F3B21 File Offset: 0x000F1D21
		public DerPrintableString(string str) : this(str, false)
		{
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000F3B2B File Offset: 0x000F1D2B
		public DerPrintableString(string str, bool validate)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (validate && !DerPrintableString.IsPrintableString(str))
			{
				throw new ArgumentException("string contains illegal characters", "str");
			}
			this.str = str;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000F3B63 File Offset: 0x000F1D63
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000F3B6B File Offset: 0x000F1D6B
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000F3B78 File Offset: 0x000F1D78
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(19, this.GetOctets());
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000F3B88 File Offset: 0x000F1D88
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerPrintableString derPrintableString = asn1Object as DerPrintableString;
			return derPrintableString != null && this.str.Equals(derPrintableString.str);
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x000F3BB4 File Offset: 0x000F1DB4
		public static bool IsPrintableString(string str)
		{
			foreach (char c in str)
			{
				if (c > '\u007f')
				{
					return false;
				}
				if (!char.IsLetterOrDigit(c))
				{
					if (c <= ':')
					{
						switch (c)
						{
						case ' ':
						case '\'':
						case '(':
						case ')':
						case '+':
						case ',':
						case '-':
						case '.':
						case '/':
							goto IL_7E;
						case '!':
						case '"':
						case '#':
						case '$':
						case '%':
						case '&':
						case '*':
							break;
						default:
							if (c == ':')
							{
								goto IL_7E;
							}
							break;
						}
					}
					else if (c == '=' || c == '?')
					{
						goto IL_7E;
					}
					return false;
				}
				IL_7E:;
			}
			return true;
		}

		// Token: 0x04001E42 RID: 7746
		private readonly string str;
	}
}

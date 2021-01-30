using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F9 RID: 1273
	public class DerNumericString : DerStringBase
	{
		// Token: 0x06002EDF RID: 11999 RVA: 0x000F31D2 File Offset: 0x000F13D2
		public static DerNumericString GetInstance(object obj)
		{
			if (obj == null || obj is DerNumericString)
			{
				return (DerNumericString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x000F31FC File Offset: 0x000F13FC
		public static DerNumericString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerNumericString)
			{
				return DerNumericString.GetInstance(@object);
			}
			return new DerNumericString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x000F3232 File Offset: 0x000F1432
		public DerNumericString(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
		{
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x000F3241 File Offset: 0x000F1441
		public DerNumericString(string str) : this(str, false)
		{
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000F324B File Offset: 0x000F144B
		public DerNumericString(string str, bool validate)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (validate && !DerNumericString.IsNumericString(str))
			{
				throw new ArgumentException("string contains illegal characters", "str");
			}
			this.str = str;
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000F3283 File Offset: 0x000F1483
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x000F328B File Offset: 0x000F148B
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x000F3298 File Offset: 0x000F1498
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(18, this.GetOctets());
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x000F32A8 File Offset: 0x000F14A8
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerNumericString derNumericString = asn1Object as DerNumericString;
			return derNumericString != null && this.str.Equals(derNumericString.str);
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000F32D4 File Offset: 0x000F14D4
		public static bool IsNumericString(string str)
		{
			foreach (char c in str)
			{
				if (c > '\u007f' || (c != ' ' && !char.IsDigit(c)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001E3D RID: 7741
		private readonly string str;
	}
}

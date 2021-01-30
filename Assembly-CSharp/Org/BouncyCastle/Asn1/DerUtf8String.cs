using System;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000504 RID: 1284
	public class DerUtf8String : DerStringBase
	{
		// Token: 0x06002F47 RID: 12103 RVA: 0x000F43BF File Offset: 0x000F25BF
		public static DerUtf8String GetInstance(object obj)
		{
			if (obj == null || obj is DerUtf8String)
			{
				return (DerUtf8String)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000F43E8 File Offset: 0x000F25E8
		public static DerUtf8String GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerUtf8String)
			{
				return DerUtf8String.GetInstance(@object);
			}
			return new DerUtf8String(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000F441E File Offset: 0x000F261E
		public DerUtf8String(byte[] str) : this(Encoding.UTF8.GetString(str, 0, str.Length))
		{
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000F4435 File Offset: 0x000F2635
		public DerUtf8String(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000F4452 File Offset: 0x000F2652
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000F445C File Offset: 0x000F265C
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerUtf8String derUtf8String = asn1Object as DerUtf8String;
			return derUtf8String != null && this.str.Equals(derUtf8String.str);
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000F4486 File Offset: 0x000F2686
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(12, Encoding.UTF8.GetBytes(this.str));
		}

		// Token: 0x04001E47 RID: 7751
		private readonly string str;
	}
}

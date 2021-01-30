using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000501 RID: 1281
	public class DerT61String : DerStringBase
	{
		// Token: 0x06002F2B RID: 12075 RVA: 0x000F3F31 File Offset: 0x000F2131
		public static DerT61String GetInstance(object obj)
		{
			if (obj == null || obj is DerT61String)
			{
				return (DerT61String)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000F3F5C File Offset: 0x000F215C
		public static DerT61String GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerT61String)
			{
				return DerT61String.GetInstance(@object);
			}
			return new DerT61String(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000F3F92 File Offset: 0x000F2192
		public DerT61String(byte[] str) : this(Strings.FromByteArray(str))
		{
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000F3FA0 File Offset: 0x000F21A0
		public DerT61String(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000F3FBD File Offset: 0x000F21BD
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000F3FC5 File Offset: 0x000F21C5
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(20, this.GetOctets());
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000F3FD5 File Offset: 0x000F21D5
		public byte[] GetOctets()
		{
			return Strings.ToByteArray(this.str);
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000F3FE4 File Offset: 0x000F21E4
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerT61String derT61String = asn1Object as DerT61String;
			return derT61String != null && this.str.Equals(derT61String.str);
		}

		// Token: 0x04001E45 RID: 7749
		private readonly string str;
	}
}

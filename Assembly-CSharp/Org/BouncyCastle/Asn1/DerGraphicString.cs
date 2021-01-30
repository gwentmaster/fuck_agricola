using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F5 RID: 1269
	public class DerGraphicString : DerStringBase
	{
		// Token: 0x06002EBB RID: 11963 RVA: 0x000F2DFC File Offset: 0x000F0FFC
		public static DerGraphicString GetInstance(object obj)
		{
			if (obj == null || obj is DerGraphicString)
			{
				return (DerGraphicString)obj;
			}
			if (obj is byte[])
			{
				try
				{
					return (DerGraphicString)Asn1Object.FromByteArray((byte[])obj);
				}
				catch (Exception ex)
				{
					throw new ArgumentException("encoding error in GetInstance: " + ex.ToString(), "obj");
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000F2E80 File Offset: 0x000F1080
		public static DerGraphicString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerGraphicString)
			{
				return DerGraphicString.GetInstance(@object);
			}
			return new DerGraphicString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000F2EB6 File Offset: 0x000F10B6
		public DerGraphicString(byte[] encoding)
		{
			this.mString = Arrays.Clone(encoding);
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000F2ECA File Offset: 0x000F10CA
		public override string GetString()
		{
			return Strings.FromByteArray(this.mString);
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x000F2ED7 File Offset: 0x000F10D7
		public byte[] GetOctets()
		{
			return Arrays.Clone(this.mString);
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000F2EE4 File Offset: 0x000F10E4
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(25, this.mString);
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x000F2EF4 File Offset: 0x000F10F4
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.mString);
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000F2F04 File Offset: 0x000F1104
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerGraphicString derGraphicString = asn1Object as DerGraphicString;
			return derGraphicString != null && Arrays.AreEqual(this.mString, derGraphicString.mString);
		}

		// Token: 0x04001E38 RID: 7736
		private readonly byte[] mString;
	}
}

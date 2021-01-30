using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F6 RID: 1270
	public class DerIA5String : DerStringBase
	{
		// Token: 0x06002EC3 RID: 11971 RVA: 0x000F2F2E File Offset: 0x000F112E
		public static DerIA5String GetInstance(object obj)
		{
			if (obj == null || obj is DerIA5String)
			{
				return (DerIA5String)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000F2F58 File Offset: 0x000F1158
		public static DerIA5String GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerIA5String)
			{
				return DerIA5String.GetInstance(@object);
			}
			return new DerIA5String(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000F2F8E File Offset: 0x000F118E
		public DerIA5String(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
		{
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000F2F9D File Offset: 0x000F119D
		public DerIA5String(string str) : this(str, false)
		{
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000F2FA7 File Offset: 0x000F11A7
		public DerIA5String(string str, bool validate)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (validate && !DerIA5String.IsIA5String(str))
			{
				throw new ArgumentException("string contains illegal characters", "str");
			}
			this.str = str;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000F2FDF File Offset: 0x000F11DF
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000F2FE7 File Offset: 0x000F11E7
		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.str);
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000F2FF4 File Offset: 0x000F11F4
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(22, this.GetOctets());
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000F3004 File Offset: 0x000F1204
		protected override int Asn1GetHashCode()
		{
			return this.str.GetHashCode();
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000F3014 File Offset: 0x000F1214
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerIA5String derIA5String = asn1Object as DerIA5String;
			return derIA5String != null && this.str.Equals(derIA5String.str);
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000F3040 File Offset: 0x000F1240
		public static bool IsIA5String(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] > '\u007f')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001E39 RID: 7737
		private readonly string str;
	}
}

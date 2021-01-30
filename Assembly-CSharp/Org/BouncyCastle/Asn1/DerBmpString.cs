using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004EF RID: 1263
	public class DerBmpString : DerStringBase
	{
		// Token: 0x06002E74 RID: 11892 RVA: 0x000F1FCE File Offset: 0x000F01CE
		public static DerBmpString GetInstance(object obj)
		{
			if (obj == null || obj is DerBmpString)
			{
				return (DerBmpString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000F1FF8 File Offset: 0x000F01F8
		public static DerBmpString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerBmpString)
			{
				return DerBmpString.GetInstance(@object);
			}
			return new DerBmpString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000F2030 File Offset: 0x000F0230
		public DerBmpString(byte[] str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			char[] array = new char[str.Length / 2];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = (char)((int)str[2 * num] << 8 | (int)(str[2 * num + 1] & byte.MaxValue));
			}
			this.str = new string(array);
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000F208F File Offset: 0x000F028F
		public DerBmpString(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000F20AC File Offset: 0x000F02AC
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x000F20B4 File Offset: 0x000F02B4
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerBmpString derBmpString = asn1Object as DerBmpString;
			return derBmpString != null && this.str.Equals(derBmpString.str);
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000F20E0 File Offset: 0x000F02E0
		internal override void Encode(DerOutputStream derOut)
		{
			char[] array = this.str.ToCharArray();
			byte[] array2 = new byte[array.Length * 2];
			for (int num = 0; num != array.Length; num++)
			{
				array2[2 * num] = (byte)(array[num] >> 8);
				array2[2 * num + 1] = (byte)array[num];
			}
			derOut.WriteEncoded(30, array2);
		}

		// Token: 0x04001E2D RID: 7725
		private readonly string str;
	}
}

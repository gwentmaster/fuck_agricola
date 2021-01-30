using System;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000505 RID: 1285
	public class DerUniversalString : DerStringBase
	{
		// Token: 0x06002F4E RID: 12110 RVA: 0x000F44A0 File Offset: 0x000F26A0
		public static DerUniversalString GetInstance(object obj)
		{
			if (obj == null || obj is DerUniversalString)
			{
				return (DerUniversalString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x000F44CC File Offset: 0x000F26CC
		public static DerUniversalString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerUniversalString)
			{
				return DerUniversalString.GetInstance(@object);
			}
			return new DerUniversalString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000F4502 File Offset: 0x000F2702
		public DerUniversalString(byte[] str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x000F4520 File Offset: 0x000F2720
		public override string GetString()
		{
			StringBuilder stringBuilder = new StringBuilder("#");
			byte[] derEncoded = base.GetDerEncoded();
			for (int num = 0; num != derEncoded.Length; num++)
			{
				uint num2 = (uint)derEncoded[num];
				stringBuilder.Append(DerUniversalString.table[(int)(num2 >> 4 & 15U)]);
				stringBuilder.Append(DerUniversalString.table[(int)(derEncoded[num] & 15)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x000F457D File Offset: 0x000F277D
		public byte[] GetOctets()
		{
			return (byte[])this.str.Clone();
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000F458F File Offset: 0x000F278F
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(28, this.str);
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000F45A0 File Offset: 0x000F27A0
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerUniversalString derUniversalString = asn1Object as DerUniversalString;
			return derUniversalString != null && Arrays.AreEqual(this.str, derUniversalString.str);
		}

		// Token: 0x04001E48 RID: 7752
		private static readonly char[] table = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};

		// Token: 0x04001E49 RID: 7753
		private readonly byte[] str;
	}
}

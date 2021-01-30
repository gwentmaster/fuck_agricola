using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200051E RID: 1310
	public abstract class X9IntegerConverter
	{
		// Token: 0x06002FE1 RID: 12257 RVA: 0x000F5D9E File Offset: 0x000F3F9E
		public static int GetByteLength(ECFieldElement fe)
		{
			return (fe.FieldSize + 7) / 8;
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x000F5DAA File Offset: 0x000F3FAA
		public static int GetByteLength(ECCurve c)
		{
			return (c.FieldSize + 7) / 8;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x000F5DB8 File Offset: 0x000F3FB8
		public static byte[] IntegerToBytes(BigInteger s, int qLength)
		{
			byte[] array = s.ToByteArrayUnsigned();
			if (qLength < array.Length)
			{
				byte[] array2 = new byte[qLength];
				Array.Copy(array, array.Length - array2.Length, array2, 0, array2.Length);
				return array2;
			}
			if (qLength > array.Length)
			{
				byte[] array3 = new byte[qLength];
				Array.Copy(array, 0, array3, array3.Length - array.Length, array.Length);
				return array3;
			}
			return array;
		}
	}
}

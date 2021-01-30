using System;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002E4 RID: 740
	public abstract class ECPointBase : ECPoint
	{
		// Token: 0x06001A02 RID: 6658 RVA: 0x0009599E File Offset: 0x00093B9E
		protected internal ECPointBase(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x000959AB File Offset: 0x00093BAB
		protected internal ECPointBase(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000959BC File Offset: 0x00093BBC
		public override byte[] GetEncoded(bool compressed)
		{
			if (base.IsInfinity)
			{
				return new byte[1];
			}
			ECPoint ecpoint = this.Normalize();
			byte[] encoded = ecpoint.XCoord.GetEncoded();
			if (compressed)
			{
				byte[] array = new byte[encoded.Length + 1];
				array[0] = (ecpoint.CompressionYTilde ? 3 : 2);
				Array.Copy(encoded, 0, array, 1, encoded.Length);
				return array;
			}
			byte[] encoded2 = ecpoint.YCoord.GetEncoded();
			byte[] array2 = new byte[encoded.Length + encoded2.Length + 1];
			array2[0] = 4;
			Array.Copy(encoded, 0, array2, 1, encoded.Length);
			Array.Copy(encoded2, 0, array2, encoded.Length + 1, encoded2.Length);
			return array2;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x00095A58 File Offset: 0x00093C58
		public override ECPoint Multiply(BigInteger k)
		{
			return this.Curve.GetMultiplier().Multiply(this, k);
		}
	}
}

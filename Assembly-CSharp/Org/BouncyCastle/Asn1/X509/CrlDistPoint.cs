using System;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000522 RID: 1314
	public class CrlDistPoint : Asn1Encodable
	{
		// Token: 0x06002FFA RID: 12282 RVA: 0x000F6635 File Offset: 0x000F4835
		public static CrlDistPoint GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return CrlDistPoint.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000F6644 File Offset: 0x000F4844
		public static CrlDistPoint GetInstance(object obj)
		{
			if (obj is CrlDistPoint || obj == null)
			{
				return (CrlDistPoint)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CrlDistPoint((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000F6691 File Offset: 0x000F4891
		private CrlDistPoint(Asn1Sequence seq)
		{
			this.seq = seq;
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000F66A0 File Offset: 0x000F48A0
		public CrlDistPoint(DistributionPoint[] points)
		{
			this.seq = new DerSequence(points);
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000F66C4 File Offset: 0x000F48C4
		public DistributionPoint[] GetDistributionPoints()
		{
			DistributionPoint[] array = new DistributionPoint[this.seq.Count];
			for (int num = 0; num != this.seq.Count; num++)
			{
				array[num] = DistributionPoint.GetInstance(this.seq[num]);
			}
			return array;
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000F670D File Offset: 0x000F490D
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000F6718 File Offset: 0x000F4918
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("CRLDistPoint:");
			stringBuilder.Append(newLine);
			DistributionPoint[] distributionPoints = this.GetDistributionPoints();
			for (int num = 0; num != distributionPoints.Length; num++)
			{
				stringBuilder.Append("    ");
				stringBuilder.Append(distributionPoints[num]);
				stringBuilder.Append(newLine);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001EB6 RID: 7862
		internal readonly Asn1Sequence seq;
	}
}

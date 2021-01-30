using System;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000528 RID: 1320
	public class DistributionPoint : Asn1Encodable
	{
		// Token: 0x06003025 RID: 12325 RVA: 0x000F6BC1 File Offset: 0x000F4DC1
		public static DistributionPoint GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DistributionPoint.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x000F6BCF File Offset: 0x000F4DCF
		public static DistributionPoint GetInstance(object obj)
		{
			if (obj == null || obj is DistributionPoint)
			{
				return (DistributionPoint)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DistributionPoint((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DistributionPoint: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x000F6C0C File Offset: 0x000F4E0C
		private DistributionPoint(Asn1Sequence seq)
		{
			for (int num = 0; num != seq.Count; num++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[num]);
				switch (instance.TagNo)
				{
				case 0:
					this.distributionPoint = DistributionPointName.GetInstance(instance, true);
					break;
				case 1:
					this.reasons = new ReasonFlags(DerBitString.GetInstance(instance, false));
					break;
				case 2:
					this.cRLIssuer = GeneralNames.GetInstance(instance, false);
					break;
				}
			}
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x000F6C88 File Offset: 0x000F4E88
		public DistributionPoint(DistributionPointName distributionPointName, ReasonFlags reasons, GeneralNames crlIssuer)
		{
			this.distributionPoint = distributionPointName;
			this.reasons = reasons;
			this.cRLIssuer = crlIssuer;
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x000F6CA5 File Offset: 0x000F4EA5
		public DistributionPointName DistributionPointName
		{
			get
			{
				return this.distributionPoint;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x000F6CAD File Offset: 0x000F4EAD
		public ReasonFlags Reasons
		{
			get
			{
				return this.reasons;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x000F6CB5 File Offset: 0x000F4EB5
		public GeneralNames CrlIssuer
		{
			get
			{
				return this.cRLIssuer;
			}
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000F6CC0 File Offset: 0x000F4EC0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.distributionPoint != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(0, this.distributionPoint)
				});
			}
			if (this.reasons != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.reasons)
				});
			}
			if (this.cRLIssuer != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 2, this.cRLIssuer)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000F6D4C File Offset: 0x000F4F4C
		public override string ToString()
		{
			string newLine = Platform.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DistributionPoint: [");
			stringBuilder.Append(newLine);
			if (this.distributionPoint != null)
			{
				this.appendObject(stringBuilder, newLine, "distributionPoint", this.distributionPoint.ToString());
			}
			if (this.reasons != null)
			{
				this.appendObject(stringBuilder, newLine, "reasons", this.reasons.ToString());
			}
			if (this.cRLIssuer != null)
			{
				this.appendObject(stringBuilder, newLine, "cRLIssuer", this.cRLIssuer.ToString());
			}
			stringBuilder.Append("]");
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000F6DF4 File Offset: 0x000F4FF4
		private void appendObject(StringBuilder buf, string sep, string name, string val)
		{
			string value = "    ";
			buf.Append(value);
			buf.Append(name);
			buf.Append(":");
			buf.Append(sep);
			buf.Append(value);
			buf.Append(value);
			buf.Append(val);
			buf.Append(sep);
		}

		// Token: 0x04001ECA RID: 7882
		internal readonly DistributionPointName distributionPoint;

		// Token: 0x04001ECB RID: 7883
		internal readonly ReasonFlags reasons;

		// Token: 0x04001ECC RID: 7884
		internal readonly GeneralNames cRLIssuer;
	}
}

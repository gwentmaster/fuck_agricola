using System;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200052C RID: 1324
	public class IssuingDistributionPoint : Asn1Encodable
	{
		// Token: 0x06003051 RID: 12369 RVA: 0x000F77C6 File Offset: 0x000F59C6
		public static IssuingDistributionPoint GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return IssuingDistributionPoint.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000F77D4 File Offset: 0x000F59D4
		public static IssuingDistributionPoint GetInstance(object obj)
		{
			if (obj == null || obj is IssuingDistributionPoint)
			{
				return (IssuingDistributionPoint)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new IssuingDistributionPoint((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000F7824 File Offset: 0x000F5A24
		public IssuingDistributionPoint(DistributionPointName distributionPoint, bool onlyContainsUserCerts, bool onlyContainsCACerts, ReasonFlags onlySomeReasons, bool indirectCRL, bool onlyContainsAttributeCerts)
		{
			this._distributionPoint = distributionPoint;
			this._indirectCRL = indirectCRL;
			this._onlyContainsAttributeCerts = onlyContainsAttributeCerts;
			this._onlyContainsCACerts = onlyContainsCACerts;
			this._onlyContainsUserCerts = onlyContainsUserCerts;
			this._onlySomeReasons = onlySomeReasons;
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (distributionPoint != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, distributionPoint)
				});
			}
			if (onlyContainsUserCerts)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, DerBoolean.True)
				});
			}
			if (onlyContainsCACerts)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 2, DerBoolean.True)
				});
			}
			if (onlySomeReasons != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 3, onlySomeReasons)
				});
			}
			if (indirectCRL)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 4, DerBoolean.True)
				});
			}
			if (onlyContainsAttributeCerts)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 5, DerBoolean.True)
				});
			}
			this.seq = new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x000F792C File Offset: 0x000F5B2C
		private IssuingDistributionPoint(Asn1Sequence seq)
		{
			this.seq = seq;
			for (int num = 0; num != seq.Count; num++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[num]);
				switch (instance.TagNo)
				{
				case 0:
					this._distributionPoint = DistributionPointName.GetInstance(instance, true);
					break;
				case 1:
					this._onlyContainsUserCerts = DerBoolean.GetInstance(instance, false).IsTrue;
					break;
				case 2:
					this._onlyContainsCACerts = DerBoolean.GetInstance(instance, false).IsTrue;
					break;
				case 3:
					this._onlySomeReasons = new ReasonFlags(DerBitString.GetInstance(instance, false));
					break;
				case 4:
					this._indirectCRL = DerBoolean.GetInstance(instance, false).IsTrue;
					break;
				case 5:
					this._onlyContainsAttributeCerts = DerBoolean.GetInstance(instance, false).IsTrue;
					break;
				default:
					throw new ArgumentException("unknown tag in IssuingDistributionPoint");
				}
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06003055 RID: 12373 RVA: 0x000F7A0F File Offset: 0x000F5C0F
		public bool OnlyContainsUserCerts
		{
			get
			{
				return this._onlyContainsUserCerts;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x000F7A17 File Offset: 0x000F5C17
		public bool OnlyContainsCACerts
		{
			get
			{
				return this._onlyContainsCACerts;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06003057 RID: 12375 RVA: 0x000F7A1F File Offset: 0x000F5C1F
		public bool IsIndirectCrl
		{
			get
			{
				return this._indirectCRL;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06003058 RID: 12376 RVA: 0x000F7A27 File Offset: 0x000F5C27
		public bool OnlyContainsAttributeCerts
		{
			get
			{
				return this._onlyContainsAttributeCerts;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06003059 RID: 12377 RVA: 0x000F7A2F File Offset: 0x000F5C2F
		public DistributionPointName DistributionPoint
		{
			get
			{
				return this._distributionPoint;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x000F7A37 File Offset: 0x000F5C37
		public ReasonFlags OnlySomeReasons
		{
			get
			{
				return this._onlySomeReasons;
			}
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x000F7A3F File Offset: 0x000F5C3F
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000F7A48 File Offset: 0x000F5C48
		public override string ToString()
		{
			string newLine = Platform.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("IssuingDistributionPoint: [");
			stringBuilder.Append(newLine);
			if (this._distributionPoint != null)
			{
				this.appendObject(stringBuilder, newLine, "distributionPoint", this._distributionPoint.ToString());
			}
			if (this._onlyContainsUserCerts)
			{
				this.appendObject(stringBuilder, newLine, "onlyContainsUserCerts", this._onlyContainsUserCerts.ToString());
			}
			if (this._onlyContainsCACerts)
			{
				this.appendObject(stringBuilder, newLine, "onlyContainsCACerts", this._onlyContainsCACerts.ToString());
			}
			if (this._onlySomeReasons != null)
			{
				this.appendObject(stringBuilder, newLine, "onlySomeReasons", this._onlySomeReasons.ToString());
			}
			if (this._onlyContainsAttributeCerts)
			{
				this.appendObject(stringBuilder, newLine, "onlyContainsAttributeCerts", this._onlyContainsAttributeCerts.ToString());
			}
			if (this._indirectCRL)
			{
				this.appendObject(stringBuilder, newLine, "indirectCRL", this._indirectCRL.ToString());
			}
			stringBuilder.Append("]");
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000F7B5C File Offset: 0x000F5D5C
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

		// Token: 0x04001EDD RID: 7901
		private readonly DistributionPointName _distributionPoint;

		// Token: 0x04001EDE RID: 7902
		private readonly bool _onlyContainsUserCerts;

		// Token: 0x04001EDF RID: 7903
		private readonly bool _onlyContainsCACerts;

		// Token: 0x04001EE0 RID: 7904
		private readonly ReasonFlags _onlySomeReasons;

		// Token: 0x04001EE1 RID: 7905
		private readonly bool _indirectCRL;

		// Token: 0x04001EE2 RID: 7906
		private readonly bool _onlyContainsAttributeCerts;

		// Token: 0x04001EE3 RID: 7907
		private readonly Asn1Sequence seq;
	}
}

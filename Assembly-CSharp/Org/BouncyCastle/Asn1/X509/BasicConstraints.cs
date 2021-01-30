using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000521 RID: 1313
	public class BasicConstraints : Asn1Encodable
	{
		// Token: 0x06002FF1 RID: 12273 RVA: 0x000F6411 File Offset: 0x000F4611
		public static BasicConstraints GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return BasicConstraints.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000F6420 File Offset: 0x000F4620
		public static BasicConstraints GetInstance(object obj)
		{
			if (obj == null || obj is BasicConstraints)
			{
				return (BasicConstraints)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new BasicConstraints((Asn1Sequence)obj);
			}
			if (obj is X509Extension)
			{
				return BasicConstraints.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000F6488 File Offset: 0x000F4688
		private BasicConstraints(Asn1Sequence seq)
		{
			if (seq.Count > 0)
			{
				if (seq[0] is DerBoolean)
				{
					this.cA = DerBoolean.GetInstance(seq[0]);
				}
				else
				{
					this.pathLenConstraint = DerInteger.GetInstance(seq[0]);
				}
				if (seq.Count > 1)
				{
					if (this.cA == null)
					{
						throw new ArgumentException("wrong sequence in constructor", "seq");
					}
					this.pathLenConstraint = DerInteger.GetInstance(seq[1]);
				}
			}
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000F650B File Offset: 0x000F470B
		public BasicConstraints(bool cA)
		{
			if (cA)
			{
				this.cA = DerBoolean.True;
			}
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000F6521 File Offset: 0x000F4721
		public BasicConstraints(int pathLenConstraint)
		{
			this.cA = DerBoolean.True;
			this.pathLenConstraint = new DerInteger(pathLenConstraint);
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000F6540 File Offset: 0x000F4740
		public bool IsCA()
		{
			return this.cA != null && this.cA.IsTrue;
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x000F6557 File Offset: 0x000F4757
		public BigInteger PathLenConstraint
		{
			get
			{
				if (this.pathLenConstraint != null)
				{
					return this.pathLenConstraint.Value;
				}
				return null;
			}
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000F6570 File Offset: 0x000F4770
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.cA != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.cA
				});
			}
			if (this.pathLenConstraint != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.pathLenConstraint
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000F65C8 File Offset: 0x000F47C8
		public override string ToString()
		{
			if (this.pathLenConstraint == null)
			{
				return "BasicConstraints: isCa(" + this.IsCA().ToString() + ")";
			}
			return string.Concat(new object[]
			{
				"BasicConstraints: isCa(",
				this.IsCA().ToString(),
				"), pathLenConstraint = ",
				this.pathLenConstraint.Value
			});
		}

		// Token: 0x04001EB4 RID: 7860
		private readonly DerBoolean cA;

		// Token: 0x04001EB5 RID: 7861
		private readonly DerInteger pathLenConstraint;
	}
}

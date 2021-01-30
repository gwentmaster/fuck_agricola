using System;
using System.Collections;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000512 RID: 1298
	public class DHDomainParameters : Asn1Encodable
	{
		// Token: 0x06002F86 RID: 12166 RVA: 0x000F4C03 File Offset: 0x000F2E03
		public static DHDomainParameters GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return DHDomainParameters.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x000F4C14 File Offset: 0x000F2E14
		public static DHDomainParameters GetInstance(object obj)
		{
			if (obj == null || obj is DHDomainParameters)
			{
				return (DHDomainParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DHDomainParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DHDomainParameters: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x000F4C64 File Offset: 0x000F2E64
		public DHDomainParameters(DerInteger p, DerInteger g, DerInteger q, DerInteger j, DHValidationParms validationParms)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			this.p = p;
			this.g = g;
			this.q = q;
			this.j = j;
			this.validationParms = validationParms;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000F4CC8 File Offset: 0x000F2EC8
		private DHDomainParameters(Asn1Sequence seq)
		{
			if (seq.Count < 3 || seq.Count > 5)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			IEnumerator enumerator = seq.GetEnumerator();
			this.p = DerInteger.GetInstance(DHDomainParameters.GetNext(enumerator));
			this.g = DerInteger.GetInstance(DHDomainParameters.GetNext(enumerator));
			this.q = DerInteger.GetInstance(DHDomainParameters.GetNext(enumerator));
			Asn1Encodable next = DHDomainParameters.GetNext(enumerator);
			if (next != null && next is DerInteger)
			{
				this.j = DerInteger.GetInstance(next);
				next = DHDomainParameters.GetNext(enumerator);
			}
			if (next != null)
			{
				this.validationParms = DHValidationParms.GetInstance(next.ToAsn1Object());
			}
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000F4D80 File Offset: 0x000F2F80
		private static Asn1Encodable GetNext(IEnumerator e)
		{
			if (!e.MoveNext())
			{
				return null;
			}
			return (Asn1Encodable)e.Current;
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x000F4D97 File Offset: 0x000F2F97
		public DerInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x000F4D9F File Offset: 0x000F2F9F
		public DerInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06002F8D RID: 12173 RVA: 0x000F4DA7 File Offset: 0x000F2FA7
		public DerInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06002F8E RID: 12174 RVA: 0x000F4DAF File Offset: 0x000F2FAF
		public DerInteger J
		{
			get
			{
				return this.j;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x000F4DB7 File Offset: 0x000F2FB7
		public DHValidationParms ValidationParms
		{
			get
			{
				return this.validationParms;
			}
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000F4DC0 File Offset: 0x000F2FC0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.p,
				this.g,
				this.q
			});
			if (this.j != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.j
				});
			}
			if (this.validationParms != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.validationParms
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04001E54 RID: 7764
		private readonly DerInteger p;

		// Token: 0x04001E55 RID: 7765
		private readonly DerInteger g;

		// Token: 0x04001E56 RID: 7766
		private readonly DerInteger q;

		// Token: 0x04001E57 RID: 7767
		private readonly DerInteger j;

		// Token: 0x04001E58 RID: 7768
		private readonly DHValidationParms validationParms;
	}
}

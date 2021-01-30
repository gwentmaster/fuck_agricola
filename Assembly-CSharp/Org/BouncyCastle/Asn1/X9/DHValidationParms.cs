using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000514 RID: 1300
	public class DHValidationParms : Asn1Encodable
	{
		// Token: 0x06002F96 RID: 12182 RVA: 0x000F4EB6 File Offset: 0x000F30B6
		public static DHValidationParms GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return DHValidationParms.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000F4EC4 File Offset: 0x000F30C4
		public static DHValidationParms GetInstance(object obj)
		{
			if (obj == null || obj is DHDomainParameters)
			{
				return (DHValidationParms)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DHValidationParms((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DHValidationParms: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000F4F11 File Offset: 0x000F3111
		public DHValidationParms(DerBitString seed, DerInteger pgenCounter)
		{
			if (seed == null)
			{
				throw new ArgumentNullException("seed");
			}
			if (pgenCounter == null)
			{
				throw new ArgumentNullException("pgenCounter");
			}
			this.seed = seed;
			this.pgenCounter = pgenCounter;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x000F4F44 File Offset: 0x000F3144
		private DHValidationParms(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.seed = DerBitString.GetInstance(seq[0]);
			this.pgenCounter = DerInteger.GetInstance(seq[1]);
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06002F9A RID: 12186 RVA: 0x000F4FA4 File Offset: 0x000F31A4
		public DerBitString Seed
		{
			get
			{
				return this.seed;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06002F9B RID: 12187 RVA: 0x000F4FAC File Offset: 0x000F31AC
		public DerInteger PgenCounter
		{
			get
			{
				return this.pgenCounter;
			}
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000F4FB4 File Offset: 0x000F31B4
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.seed,
				this.pgenCounter
			});
		}

		// Token: 0x04001E5A RID: 7770
		private readonly DerBitString seed;

		// Token: 0x04001E5B RID: 7771
		private readonly DerInteger pgenCounter;
	}
}

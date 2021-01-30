using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000413 RID: 1043
	public class RandomDsaKCalculator : IDsaKCalculator
	{
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060026C3 RID: 9923 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsDeterministic
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x000C389F File Offset: 0x000C1A9F
		public virtual void Init(BigInteger n, SecureRandom random)
		{
			this.q = n;
			this.random = random;
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000C2375 File Offset: 0x000C0575
		public virtual void Init(BigInteger n, BigInteger d, byte[] message)
		{
			throw new InvalidOperationException("Operation not supported");
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000C38B0 File Offset: 0x000C1AB0
		public virtual BigInteger NextK()
		{
			int bitLength = this.q.BitLength;
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(bitLength, this.random);
			}
			while (bigInteger.SignValue < 1 || bigInteger.CompareTo(this.q) >= 0);
			return bigInteger;
		}

		// Token: 0x040019F3 RID: 6643
		private BigInteger q;

		// Token: 0x040019F4 RID: 6644
		private SecureRandom random;
	}
}

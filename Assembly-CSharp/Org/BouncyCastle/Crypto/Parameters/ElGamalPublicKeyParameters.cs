using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000431 RID: 1073
	public class ElGamalPublicKeyParameters : ElGamalKeyParameters
	{
		// Token: 0x06002799 RID: 10137 RVA: 0x000C55F4 File Offset: 0x000C37F4
		public ElGamalPublicKeyParameters(BigInteger y, ElGamalParameters parameters) : base(false, parameters)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			this.y = y;
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x000C5613 File Offset: 0x000C3813
		public BigInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000C561C File Offset: 0x000C381C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ElGamalPublicKeyParameters elGamalPublicKeyParameters = obj as ElGamalPublicKeyParameters;
			return elGamalPublicKeyParameters != null && this.Equals(elGamalPublicKeyParameters);
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000C5642 File Offset: 0x000C3842
		protected bool Equals(ElGamalPublicKeyParameters other)
		{
			return this.y.Equals(other.y) && base.Equals(other);
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000C5660 File Offset: 0x000C3860
		public override int GetHashCode()
		{
			return this.y.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001A45 RID: 6725
		private readonly BigInteger y;
	}
}

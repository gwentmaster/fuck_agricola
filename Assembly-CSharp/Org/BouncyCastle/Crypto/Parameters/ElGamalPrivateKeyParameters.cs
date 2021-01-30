using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000430 RID: 1072
	public class ElGamalPrivateKeyParameters : ElGamalKeyParameters
	{
		// Token: 0x06002794 RID: 10132 RVA: 0x000C5572 File Offset: 0x000C3772
		public ElGamalPrivateKeyParameters(BigInteger x, ElGamalParameters parameters) : base(true, parameters)
		{
			if (x == null)
			{
				throw new ArgumentNullException("x");
			}
			this.x = x;
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06002795 RID: 10133 RVA: 0x000C5591 File Offset: 0x000C3791
		public BigInteger X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000C559C File Offset: 0x000C379C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ElGamalPrivateKeyParameters elGamalPrivateKeyParameters = obj as ElGamalPrivateKeyParameters;
			return elGamalPrivateKeyParameters != null && this.Equals(elGamalPrivateKeyParameters);
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000C55C2 File Offset: 0x000C37C2
		protected bool Equals(ElGamalPrivateKeyParameters other)
		{
			return other.x.Equals(this.x) && base.Equals(other);
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000C55E0 File Offset: 0x000C37E0
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001A44 RID: 6724
		private readonly BigInteger x;
	}
}

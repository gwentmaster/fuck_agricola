using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200042E RID: 1070
	public class ElGamalKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06002787 RID: 10119 RVA: 0x000C540E File Offset: 0x000C360E
		protected ElGamalKeyParameters(bool isPrivate, ElGamalParameters parameters) : base(isPrivate)
		{
			this.parameters = parameters;
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x000C541E File Offset: 0x000C361E
		public ElGamalParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000C5428 File Offset: 0x000C3628
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ElGamalKeyParameters elGamalKeyParameters = obj as ElGamalKeyParameters;
			return elGamalKeyParameters != null && this.Equals(elGamalKeyParameters);
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000C544E File Offset: 0x000C364E
		protected bool Equals(ElGamalKeyParameters other)
		{
			return object.Equals(this.parameters, other.parameters) && base.Equals(other);
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000C546C File Offset: 0x000C366C
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.parameters != null)
			{
				num ^= this.parameters.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001A40 RID: 6720
		private readonly ElGamalParameters parameters;
	}
}

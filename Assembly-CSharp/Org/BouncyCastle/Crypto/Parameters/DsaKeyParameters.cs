using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000423 RID: 1059
	public abstract class DsaKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06002739 RID: 10041 RVA: 0x000C4AF5 File Offset: 0x000C2CF5
		protected DsaKeyParameters(bool isPrivate, DsaParameters parameters) : base(isPrivate)
		{
			this.parameters = parameters;
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x000C4B05 File Offset: 0x000C2D05
		public DsaParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000C4B10 File Offset: 0x000C2D10
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaKeyParameters dsaKeyParameters = obj as DsaKeyParameters;
			return dsaKeyParameters != null && this.Equals(dsaKeyParameters);
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000C4B36 File Offset: 0x000C2D36
		protected bool Equals(DsaKeyParameters other)
		{
			return object.Equals(this.parameters, other.parameters) && base.Equals(other);
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000C4B54 File Offset: 0x000C2D54
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.parameters != null)
			{
				num ^= this.parameters.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001A28 RID: 6696
		private readonly DsaParameters parameters;
	}
}

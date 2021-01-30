using System;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x020002A1 RID: 673
	public class PemHeader
	{
		// Token: 0x0600165E RID: 5726 RVA: 0x000808A3 File Offset: 0x0007EAA3
		public PemHeader(string name, string val)
		{
			this.name = name;
			this.val = val;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600165F RID: 5727 RVA: 0x000808B9 File Offset: 0x0007EAB9
		public virtual string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06001660 RID: 5728 RVA: 0x000808C1 File Offset: 0x0007EAC1
		public virtual string Value
		{
			get
			{
				return this.val;
			}
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x000808C9 File Offset: 0x0007EAC9
		public override int GetHashCode()
		{
			return this.GetHashCode(this.name) + 31 * this.GetHashCode(this.val);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x000808E8 File Offset: 0x0007EAE8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (!(obj is PemHeader))
			{
				return false;
			}
			PemHeader pemHeader = (PemHeader)obj;
			return object.Equals(this.name, pemHeader.name) && object.Equals(this.val, pemHeader.val);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00080932 File Offset: 0x0007EB32
		private int GetHashCode(string s)
		{
			if (s == null)
			{
				return 1;
			}
			return s.GetHashCode();
		}

		// Token: 0x04001507 RID: 5383
		private string name;

		// Token: 0x04001508 RID: 5384
		private string val;
	}
}

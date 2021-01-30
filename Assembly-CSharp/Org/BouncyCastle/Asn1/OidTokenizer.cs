using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000511 RID: 1297
	public class OidTokenizer
	{
		// Token: 0x06002F83 RID: 12163 RVA: 0x000F4B7A File Offset: 0x000F2D7A
		public OidTokenizer(string oid)
		{
			this.oid = oid;
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06002F84 RID: 12164 RVA: 0x000F4B89 File Offset: 0x000F2D89
		public bool HasMoreTokens
		{
			get
			{
				return this.index != -1;
			}
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000F4B98 File Offset: 0x000F2D98
		public string NextToken()
		{
			if (this.index == -1)
			{
				return null;
			}
			int num = this.oid.IndexOf('.', this.index);
			if (num == -1)
			{
				string result = this.oid.Substring(this.index);
				this.index = -1;
				return result;
			}
			string result2 = this.oid.Substring(this.index, num - this.index);
			this.index = num + 1;
			return result2;
		}

		// Token: 0x04001E52 RID: 7762
		private string oid;

		// Token: 0x04001E53 RID: 7763
		private int index;
	}
}

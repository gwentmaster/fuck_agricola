using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F1 RID: 1009
	public class TlsFatalAlert : IOException
	{
		// Token: 0x06002516 RID: 9494 RVA: 0x000BC5CF File Offset: 0x000BA7CF
		public TlsFatalAlert(byte alertDescription) : this(alertDescription, null)
		{
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000BC5D9 File Offset: 0x000BA7D9
		public TlsFatalAlert(byte alertDescription, Exception alertCause) : base(Org.BouncyCastle.Crypto.Tls.AlertDescription.GetText(alertDescription), alertCause)
		{
			this.alertDescription = alertDescription;
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06002518 RID: 9496 RVA: 0x000BC5EF File Offset: 0x000BA7EF
		public virtual byte AlertDescription
		{
			get
			{
				return this.alertDescription;
			}
		}

		// Token: 0x0400195A RID: 6490
		private readonly byte alertDescription;
	}
}

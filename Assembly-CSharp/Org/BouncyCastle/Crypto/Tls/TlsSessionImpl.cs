using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000400 RID: 1024
	internal class TlsSessionImpl : TlsSession
	{
		// Token: 0x060025AF RID: 9647 RVA: 0x000BE108 File Offset: 0x000BC308
		internal TlsSessionImpl(byte[] sessionID, SessionParameters sessionParameters)
		{
			if (sessionID == null)
			{
				throw new ArgumentNullException("sessionID");
			}
			if (sessionID.Length < 1 || sessionID.Length > 32)
			{
				throw new ArgumentException("must have length between 1 and 32 bytes, inclusive", "sessionID");
			}
			this.mSessionID = Arrays.Clone(sessionID);
			this.mSessionParameters = sessionParameters;
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000BE15C File Offset: 0x000BC35C
		public virtual SessionParameters ExportSessionParameters()
		{
			SessionParameters result;
			lock (this)
			{
				result = ((this.mSessionParameters == null) ? null : this.mSessionParameters.Copy());
			}
			return result;
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000BE1AC File Offset: 0x000BC3AC
		public virtual byte[] SessionID
		{
			get
			{
				byte[] result;
				lock (this)
				{
					result = this.mSessionID;
				}
				return result;
			}
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000BE1EC File Offset: 0x000BC3EC
		public virtual void Invalidate()
		{
			lock (this)
			{
				if (this.mSessionParameters != null)
				{
					this.mSessionParameters.Clear();
					this.mSessionParameters = null;
				}
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x000BE23C File Offset: 0x000BC43C
		public virtual bool IsResumable
		{
			get
			{
				bool result;
				lock (this)
				{
					result = (this.mSessionParameters != null);
				}
				return result;
			}
		}

		// Token: 0x0400199A RID: 6554
		internal readonly byte[] mSessionID;

		// Token: 0x0400199B RID: 6555
		internal SessionParameters mSessionParameters;
	}
}

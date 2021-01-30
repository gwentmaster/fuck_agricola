using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003BD RID: 957
	public class HeartbeatExtension
	{
		// Token: 0x060023A3 RID: 9123 RVA: 0x000B7C9A File Offset: 0x000B5E9A
		public HeartbeatExtension(byte mode)
		{
			if (!HeartbeatMode.IsValid(mode))
			{
				throw new ArgumentException("not a valid HeartbeatMode value", "mode");
			}
			this.mMode = mode;
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060023A4 RID: 9124 RVA: 0x000B7CC1 File Offset: 0x000B5EC1
		public virtual byte Mode
		{
			get
			{
				return this.mMode;
			}
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x000B7CC9 File Offset: 0x000B5EC9
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mMode, output);
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x000B7CD7 File Offset: 0x000B5ED7
		public static HeartbeatExtension Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (!HeartbeatMode.IsValid(b))
			{
				throw new TlsFatalAlert(47);
			}
			return new HeartbeatExtension(b);
		}

		// Token: 0x04001880 RID: 6272
		protected readonly byte mMode;
	}
}

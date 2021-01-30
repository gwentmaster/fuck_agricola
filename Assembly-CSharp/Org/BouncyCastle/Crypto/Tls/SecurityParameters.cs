using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003CE RID: 974
	public class SecurityParameters
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x000B8790 File Offset: 0x000B6990
		internal virtual void Clear()
		{
			if (this.masterSecret != null)
			{
				Arrays.Fill(this.masterSecret, 0);
				this.masterSecret = null;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x000B87AD File Offset: 0x000B69AD
		public virtual int Entity
		{
			get
			{
				return this.entity;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x000B87B5 File Offset: 0x000B69B5
		public virtual int CipherSuite
		{
			get
			{
				return this.cipherSuite;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060023F2 RID: 9202 RVA: 0x000B87BD File Offset: 0x000B69BD
		public byte CompressionAlgorithm
		{
			get
			{
				return this.compressionAlgorithm;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x000B87C5 File Offset: 0x000B69C5
		public virtual int PrfAlgorithm
		{
			get
			{
				return this.prfAlgorithm;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x000B87CD File Offset: 0x000B69CD
		public virtual int VerifyDataLength
		{
			get
			{
				return this.verifyDataLength;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x000B87D5 File Offset: 0x000B69D5
		public virtual byte[] MasterSecret
		{
			get
			{
				return this.masterSecret;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000B87DD File Offset: 0x000B69DD
		public virtual byte[] ClientRandom
		{
			get
			{
				return this.clientRandom;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x000B87E5 File Offset: 0x000B69E5
		public virtual byte[] ServerRandom
		{
			get
			{
				return this.serverRandom;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000B87ED File Offset: 0x000B69ED
		public virtual byte[] SessionHash
		{
			get
			{
				return this.sessionHash;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x000B87F5 File Offset: 0x000B69F5
		public virtual byte[] PskIdentity
		{
			get
			{
				return this.pskIdentity;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x000B87FD File Offset: 0x000B69FD
		public virtual byte[] SrpIdentity
		{
			get
			{
				return this.srpIdentity;
			}
		}

		// Token: 0x040018F6 RID: 6390
		internal int entity = -1;

		// Token: 0x040018F7 RID: 6391
		internal int cipherSuite = -1;

		// Token: 0x040018F8 RID: 6392
		internal byte compressionAlgorithm;

		// Token: 0x040018F9 RID: 6393
		internal int prfAlgorithm = -1;

		// Token: 0x040018FA RID: 6394
		internal int verifyDataLength = -1;

		// Token: 0x040018FB RID: 6395
		internal byte[] masterSecret;

		// Token: 0x040018FC RID: 6396
		internal byte[] clientRandom;

		// Token: 0x040018FD RID: 6397
		internal byte[] serverRandom;

		// Token: 0x040018FE RID: 6398
		internal byte[] sessionHash;

		// Token: 0x040018FF RID: 6399
		internal byte[] pskIdentity;

		// Token: 0x04001900 RID: 6400
		internal byte[] srpIdentity;

		// Token: 0x04001901 RID: 6401
		internal short maxFragmentLength = -1;

		// Token: 0x04001902 RID: 6402
		internal bool truncatedHMac;

		// Token: 0x04001903 RID: 6403
		internal bool encryptThenMac;

		// Token: 0x04001904 RID: 6404
		internal bool extendedMasterSecret;
	}
}

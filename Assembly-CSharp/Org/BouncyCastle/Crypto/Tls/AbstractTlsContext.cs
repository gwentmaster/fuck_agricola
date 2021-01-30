using System;
using System.Threading;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000392 RID: 914
	internal abstract class AbstractTlsContext : TlsContext
	{
		// Token: 0x06002291 RID: 8849 RVA: 0x000B5A38 File Offset: 0x000B3C38
		private static long NextCounterValue()
		{
			return Interlocked.Increment(ref AbstractTlsContext.counter);
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x000B5A44 File Offset: 0x000B3C44
		internal AbstractTlsContext(SecureRandom secureRandom, SecurityParameters securityParameters)
		{
			IDigest digest = TlsUtilities.CreateHash(4);
			byte[] array = new byte[digest.GetDigestSize()];
			secureRandom.NextBytes(array);
			this.mNonceRandom = new DigestRandomGenerator(digest);
			this.mNonceRandom.AddSeedMaterial(AbstractTlsContext.NextCounterValue());
			this.mNonceRandom.AddSeedMaterial(Times.NanoTime());
			this.mNonceRandom.AddSeedMaterial(array);
			this.mSecureRandom = secureRandom;
			this.mSecurityParameters = securityParameters;
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06002293 RID: 8851 RVA: 0x000B5AB7 File Offset: 0x000B3CB7
		public virtual IRandomGenerator NonceRandomGenerator
		{
			get
			{
				return this.mNonceRandom;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06002294 RID: 8852 RVA: 0x000B5ABF File Offset: 0x000B3CBF
		public virtual SecureRandom SecureRandom
		{
			get
			{
				return this.mSecureRandom;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06002295 RID: 8853 RVA: 0x000B5AC7 File Offset: 0x000B3CC7
		public virtual SecurityParameters SecurityParameters
		{
			get
			{
				return this.mSecurityParameters;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06002296 RID: 8854
		public abstract bool IsServer { get; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06002297 RID: 8855 RVA: 0x000B5ACF File Offset: 0x000B3CCF
		public virtual ProtocolVersion ClientVersion
		{
			get
			{
				return this.mClientVersion;
			}
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x000B5AD7 File Offset: 0x000B3CD7
		internal virtual void SetClientVersion(ProtocolVersion clientVersion)
		{
			this.mClientVersion = clientVersion;
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x000B5AE0 File Offset: 0x000B3CE0
		public virtual ProtocolVersion ServerVersion
		{
			get
			{
				return this.mServerVersion;
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000B5AE8 File Offset: 0x000B3CE8
		internal virtual void SetServerVersion(ProtocolVersion serverVersion)
		{
			this.mServerVersion = serverVersion;
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x000B5AF1 File Offset: 0x000B3CF1
		public virtual TlsSession ResumableSession
		{
			get
			{
				return this.mSession;
			}
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x000B5AF9 File Offset: 0x000B3CF9
		internal virtual void SetResumableSession(TlsSession session)
		{
			this.mSession = session;
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x000B5B02 File Offset: 0x000B3D02
		// (set) Token: 0x0600229E RID: 8862 RVA: 0x000B5B0A File Offset: 0x000B3D0A
		public virtual object UserObject
		{
			get
			{
				return this.mUserObject;
			}
			set
			{
				this.mUserObject = value;
			}
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x000B5B14 File Offset: 0x000B3D14
		public virtual byte[] ExportKeyingMaterial(string asciiLabel, byte[] context_value, int length)
		{
			if (context_value != null && !TlsUtilities.IsValidUint16(context_value.Length))
			{
				throw new ArgumentException("must have length less than 2^16 (or be null)", "context_value");
			}
			SecurityParameters securityParameters = this.SecurityParameters;
			byte[] clientRandom = securityParameters.ClientRandom;
			byte[] serverRandom = securityParameters.ServerRandom;
			int num = clientRandom.Length + serverRandom.Length;
			if (context_value != null)
			{
				num += 2 + context_value.Length;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			Array.Copy(clientRandom, 0, array, num2, clientRandom.Length);
			num2 += clientRandom.Length;
			Array.Copy(serverRandom, 0, array, num2, serverRandom.Length);
			num2 += serverRandom.Length;
			if (context_value != null)
			{
				TlsUtilities.WriteUint16(context_value.Length, array, num2);
				num2 += 2;
				Array.Copy(context_value, 0, array, num2, context_value.Length);
				num2 += context_value.Length;
			}
			if (num2 != num)
			{
				throw new InvalidOperationException("error in calculation of seed for export");
			}
			return TlsUtilities.PRF(this, securityParameters.MasterSecret, asciiLabel, array, length);
		}

		// Token: 0x0400169A RID: 5786
		private static long counter = Times.NanoTime();

		// Token: 0x0400169B RID: 5787
		private readonly IRandomGenerator mNonceRandom;

		// Token: 0x0400169C RID: 5788
		private readonly SecureRandom mSecureRandom;

		// Token: 0x0400169D RID: 5789
		private readonly SecurityParameters mSecurityParameters;

		// Token: 0x0400169E RID: 5790
		private ProtocolVersion mClientVersion;

		// Token: 0x0400169F RID: 5791
		private ProtocolVersion mServerVersion;

		// Token: 0x040016A0 RID: 5792
		private TlsSession mSession;

		// Token: 0x040016A1 RID: 5793
		private object mUserObject;
	}
}

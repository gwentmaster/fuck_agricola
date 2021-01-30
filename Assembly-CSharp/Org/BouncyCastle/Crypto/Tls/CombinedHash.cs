using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003AA RID: 938
	internal class CombinedHash : TlsHandshakeHash, IDigest
	{
		// Token: 0x0600233F RID: 9023 RVA: 0x000B7081 File Offset: 0x000B5281
		internal CombinedHash()
		{
			this.mMd5 = TlsUtilities.CreateHash(1);
			this.mSha1 = TlsUtilities.CreateHash(2);
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000B70A1 File Offset: 0x000B52A1
		internal CombinedHash(CombinedHash t)
		{
			this.mContext = t.mContext;
			this.mMd5 = TlsUtilities.CloneHash(1, t.mMd5);
			this.mSha1 = TlsUtilities.CloneHash(2, t.mSha1);
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000B70D9 File Offset: 0x000B52D9
		public virtual void Init(TlsContext context)
		{
			this.mContext = context;
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x00035D67 File Offset: 0x00033F67
		public virtual TlsHandshakeHash NotifyPrfDetermined()
		{
			return this;
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000B70E2 File Offset: 0x000B52E2
		public virtual void TrackHashAlgorithm(byte hashAlgorithm)
		{
			throw new InvalidOperationException("CombinedHash only supports calculating the legacy PRF for handshake hash");
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void SealHashAlgorithms()
		{
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000B70EE File Offset: 0x000B52EE
		public virtual TlsHandshakeHash StopTracking()
		{
			return new CombinedHash(this);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000B70EE File Offset: 0x000B52EE
		public virtual IDigest ForkPrfHash()
		{
			return new CombinedHash(this);
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000B70F6 File Offset: 0x000B52F6
		public virtual byte[] GetFinalHash(byte hashAlgorithm)
		{
			throw new InvalidOperationException("CombinedHash doesn't support multiple hashes");
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x000B7102 File Offset: 0x000B5302
		public virtual string AlgorithmName
		{
			get
			{
				return this.mMd5.AlgorithmName + " and " + this.mSha1.AlgorithmName;
			}
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x000B7124 File Offset: 0x000B5324
		public virtual int GetByteLength()
		{
			return Math.Max(this.mMd5.GetByteLength(), this.mSha1.GetByteLength());
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x000B7141 File Offset: 0x000B5341
		public virtual int GetDigestSize()
		{
			return this.mMd5.GetDigestSize() + this.mSha1.GetDigestSize();
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x000B715A File Offset: 0x000B535A
		public virtual void Update(byte input)
		{
			this.mMd5.Update(input);
			this.mSha1.Update(input);
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000B7174 File Offset: 0x000B5374
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.mMd5.BlockUpdate(input, inOff, len);
			this.mSha1.BlockUpdate(input, inOff, len);
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000B7194 File Offset: 0x000B5394
		public virtual int DoFinal(byte[] output, int outOff)
		{
			if (this.mContext != null && TlsUtilities.IsSsl(this.mContext))
			{
				this.Ssl3Complete(this.mMd5, Ssl3Mac.IPAD, Ssl3Mac.OPAD, 48);
				this.Ssl3Complete(this.mSha1, Ssl3Mac.IPAD, Ssl3Mac.OPAD, 40);
			}
			int num = this.mMd5.DoFinal(output, outOff);
			int num2 = this.mSha1.DoFinal(output, outOff + num);
			return num + num2;
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000B7207 File Offset: 0x000B5407
		public virtual void Reset()
		{
			this.mMd5.Reset();
			this.mSha1.Reset();
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x000B7220 File Offset: 0x000B5420
		protected virtual void Ssl3Complete(IDigest d, byte[] ipad, byte[] opad, int padLength)
		{
			byte[] masterSecret = this.mContext.SecurityParameters.masterSecret;
			d.BlockUpdate(masterSecret, 0, masterSecret.Length);
			d.BlockUpdate(ipad, 0, padLength);
			byte[] array = DigestUtilities.DoFinal(d);
			d.BlockUpdate(masterSecret, 0, masterSecret.Length);
			d.BlockUpdate(opad, 0, padLength);
			d.BlockUpdate(array, 0, array.Length);
		}

		// Token: 0x0400180C RID: 6156
		protected TlsContext mContext;

		// Token: 0x0400180D RID: 6157
		protected IDigest mMd5;

		// Token: 0x0400180E RID: 6158
		protected IDigest mSha1;
	}
}

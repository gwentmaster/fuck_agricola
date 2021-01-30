using System;
using System.Collections;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B1 RID: 945
	internal class DeferredHash : TlsHandshakeHash, IDigest
	{
		// Token: 0x0600237A RID: 9082 RVA: 0x000B7705 File Offset: 0x000B5905
		internal DeferredHash()
		{
			this.mBuf = new DigestInputBuffer();
			this.mHashes = Platform.CreateHashtable();
			this.mPrfHashAlgorithm = -1;
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000B772A File Offset: 0x000B592A
		private DeferredHash(byte prfHashAlgorithm, IDigest prfHash)
		{
			this.mBuf = null;
			this.mHashes = Platform.CreateHashtable();
			this.mPrfHashAlgorithm = (int)prfHashAlgorithm;
			this.mHashes[prfHashAlgorithm] = prfHash;
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x000B775D File Offset: 0x000B595D
		public virtual void Init(TlsContext context)
		{
			this.mContext = context;
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x000B7768 File Offset: 0x000B5968
		public virtual TlsHandshakeHash NotifyPrfDetermined()
		{
			int prfAlgorithm = this.mContext.SecurityParameters.PrfAlgorithm;
			if (prfAlgorithm == 0)
			{
				CombinedHash combinedHash = new CombinedHash();
				combinedHash.Init(this.mContext);
				this.mBuf.UpdateDigest(combinedHash);
				return combinedHash.NotifyPrfDetermined();
			}
			this.mPrfHashAlgorithm = (int)TlsUtilities.GetHashAlgorithmForPrfAlgorithm(prfAlgorithm);
			this.CheckTrackingHash((byte)this.mPrfHashAlgorithm);
			return this;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x000B77C8 File Offset: 0x000B59C8
		public virtual void TrackHashAlgorithm(byte hashAlgorithm)
		{
			if (this.mBuf == null)
			{
				throw new InvalidOperationException("Too late to track more hash algorithms");
			}
			this.CheckTrackingHash(hashAlgorithm);
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x000B77E4 File Offset: 0x000B59E4
		public virtual void SealHashAlgorithms()
		{
			this.CheckStopBuffering();
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x000B77EC File Offset: 0x000B59EC
		public virtual TlsHandshakeHash StopTracking()
		{
			byte b = (byte)this.mPrfHashAlgorithm;
			IDigest digest = TlsUtilities.CloneHash(b, (IDigest)this.mHashes[b]);
			if (this.mBuf != null)
			{
				this.mBuf.UpdateDigest(digest);
			}
			DeferredHash deferredHash = new DeferredHash(b, digest);
			deferredHash.Init(this.mContext);
			return deferredHash;
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000B7848 File Offset: 0x000B5A48
		public virtual IDigest ForkPrfHash()
		{
			this.CheckStopBuffering();
			byte b = (byte)this.mPrfHashAlgorithm;
			if (this.mBuf != null)
			{
				IDigest digest = TlsUtilities.CreateHash(b);
				this.mBuf.UpdateDigest(digest);
				return digest;
			}
			return TlsUtilities.CloneHash(b, (IDigest)this.mHashes[b]);
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x000B789C File Offset: 0x000B5A9C
		public virtual byte[] GetFinalHash(byte hashAlgorithm)
		{
			IDigest digest = (IDigest)this.mHashes[hashAlgorithm];
			if (digest == null)
			{
				throw new InvalidOperationException("HashAlgorithm." + HashAlgorithm.GetText(hashAlgorithm) + " is not being tracked");
			}
			digest = TlsUtilities.CloneHash(hashAlgorithm, digest);
			if (this.mBuf != null)
			{
				this.mBuf.UpdateDigest(digest);
			}
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06002383 RID: 9091 RVA: 0x000B7900 File Offset: 0x000B5B00
		public virtual string AlgorithmName
		{
			get
			{
				throw new InvalidOperationException("Use Fork() to get a definite IDigest");
			}
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x000B7900 File Offset: 0x000B5B00
		public virtual int GetByteLength()
		{
			throw new InvalidOperationException("Use Fork() to get a definite IDigest");
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x000B7900 File Offset: 0x000B5B00
		public virtual int GetDigestSize()
		{
			throw new InvalidOperationException("Use Fork() to get a definite IDigest");
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x000B790C File Offset: 0x000B5B0C
		public virtual void Update(byte input)
		{
			if (this.mBuf != null)
			{
				this.mBuf.WriteByte(input);
				return;
			}
			foreach (object obj in this.mHashes.Values)
			{
				((IDigest)obj).Update(input);
			}
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x000B7980 File Offset: 0x000B5B80
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (this.mBuf != null)
			{
				this.mBuf.Write(input, inOff, len);
				return;
			}
			foreach (object obj in this.mHashes.Values)
			{
				((IDigest)obj).BlockUpdate(input, inOff, len);
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000B7900 File Offset: 0x000B5B00
		public virtual int DoFinal(byte[] output, int outOff)
		{
			throw new InvalidOperationException("Use Fork() to get a definite IDigest");
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000B79F8 File Offset: 0x000B5BF8
		public virtual void Reset()
		{
			if (this.mBuf != null)
			{
				this.mBuf.SetLength(0L);
				return;
			}
			foreach (object obj in this.mHashes.Values)
			{
				((IDigest)obj).Reset();
			}
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000B7A6C File Offset: 0x000B5C6C
		protected virtual void CheckStopBuffering()
		{
			if (this.mBuf != null && this.mHashes.Count <= 4)
			{
				foreach (object obj in this.mHashes.Values)
				{
					IDigest d = (IDigest)obj;
					this.mBuf.UpdateDigest(d);
				}
				this.mBuf = null;
			}
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000B7AEC File Offset: 0x000B5CEC
		protected virtual void CheckTrackingHash(byte hashAlgorithm)
		{
			if (!this.mHashes.Contains(hashAlgorithm))
			{
				IDigest value = TlsUtilities.CreateHash(hashAlgorithm);
				this.mHashes[hashAlgorithm] = value;
			}
		}

		// Token: 0x04001818 RID: 6168
		protected const int BUFFERING_HASH_LIMIT = 4;

		// Token: 0x04001819 RID: 6169
		protected TlsContext mContext;

		// Token: 0x0400181A RID: 6170
		private DigestInputBuffer mBuf;

		// Token: 0x0400181B RID: 6171
		private IDictionary mHashes;

		// Token: 0x0400181C RID: 6172
		private int mPrfHashAlgorithm;
	}
}

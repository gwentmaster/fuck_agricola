using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D6 RID: 982
	public class Ssl3Mac : IMac
	{
		// Token: 0x06002420 RID: 9248 RVA: 0x000B8D3A File Offset: 0x000B6F3A
		public Ssl3Mac(IDigest digest)
		{
			this.digest = digest;
			if (digest.GetDigestSize() == 20)
			{
				this.padLength = 40;
				return;
			}
			this.padLength = 48;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x000B8D64 File Offset: 0x000B6F64
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "/SSL3MAC";
			}
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000B8D7B File Offset: 0x000B6F7B
		public virtual void Init(ICipherParameters parameters)
		{
			this.secret = Arrays.Clone(((KeyParameter)parameters).GetKey());
			this.Reset();
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000B8D99 File Offset: 0x000B6F99
		public virtual int GetMacSize()
		{
			return this.digest.GetDigestSize();
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000B8DA6 File Offset: 0x000B6FA6
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000B8DB4 File Offset: 0x000B6FB4
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.digest.BlockUpdate(input, inOff, len);
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000B8DC4 File Offset: 0x000B6FC4
		public virtual int DoFinal(byte[] output, int outOff)
		{
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			this.digest.BlockUpdate(this.secret, 0, this.secret.Length);
			this.digest.BlockUpdate(Ssl3Mac.OPAD, 0, this.padLength);
			this.digest.BlockUpdate(array, 0, array.Length);
			int result = this.digest.DoFinal(output, outOff);
			this.Reset();
			return result;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000B8E44 File Offset: 0x000B7044
		public virtual void Reset()
		{
			this.digest.Reset();
			this.digest.BlockUpdate(this.secret, 0, this.secret.Length);
			this.digest.BlockUpdate(Ssl3Mac.IPAD, 0, this.padLength);
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000B8E82 File Offset: 0x000B7082
		private static byte[] GenPad(byte b, int count)
		{
			byte[] array = new byte[count];
			Arrays.Fill(array, b);
			return array;
		}

		// Token: 0x04001916 RID: 6422
		private const byte IPAD_BYTE = 54;

		// Token: 0x04001917 RID: 6423
		private const byte OPAD_BYTE = 92;

		// Token: 0x04001918 RID: 6424
		internal static readonly byte[] IPAD = Ssl3Mac.GenPad(54, 48);

		// Token: 0x04001919 RID: 6425
		internal static readonly byte[] OPAD = Ssl3Mac.GenPad(92, 48);

		// Token: 0x0400191A RID: 6426
		private readonly IDigest digest;

		// Token: 0x0400191B RID: 6427
		private readonly int padLength;

		// Token: 0x0400191C RID: 6428
		private byte[] secret;
	}
}

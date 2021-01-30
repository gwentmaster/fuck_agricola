using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000472 RID: 1138
	public class SipHash : IMac
	{
		// Token: 0x0600297E RID: 10622 RVA: 0x000CD7EA File Offset: 0x000CB9EA
		public SipHash() : this(2, 4)
		{
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000CD7F4 File Offset: 0x000CB9F4
		public SipHash(int c, int d)
		{
			this.c = c;
			this.d = d;
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x000CD80A File Offset: 0x000CBA0A
		public virtual string AlgorithmName
		{
			get
			{
				return string.Concat(new object[]
				{
					"SipHash-",
					this.c,
					"-",
					this.d
				});
			}
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int GetMacSize()
		{
			return 8;
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000CD844 File Offset: 0x000CBA44
		public virtual void Init(ICipherParameters parameters)
		{
			KeyParameter keyParameter = parameters as KeyParameter;
			if (keyParameter == null)
			{
				throw new ArgumentException("must be an instance of KeyParameter", "parameters");
			}
			byte[] key = keyParameter.GetKey();
			if (key.Length != 16)
			{
				throw new ArgumentException("must be a 128-bit key", "parameters");
			}
			this.k0 = (long)Pack.LE_To_UInt64(key, 0);
			this.k1 = (long)Pack.LE_To_UInt64(key, 8);
			this.Reset();
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000CD8A8 File Offset: 0x000CBAA8
		public virtual void Update(byte input)
		{
			this.m = (long)((ulong)this.m >> 8 | (ulong)input << 56);
			int num = this.wordPos + 1;
			this.wordPos = num;
			if (num == 8)
			{
				this.ProcessMessageWord();
				this.wordPos = 0;
			}
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000CD8EC File Offset: 0x000CBAEC
		public virtual void BlockUpdate(byte[] input, int offset, int length)
		{
			int i = 0;
			int num = length & -8;
			if (this.wordPos == 0)
			{
				while (i < num)
				{
					this.m = (long)Pack.LE_To_UInt64(input, offset + i);
					this.ProcessMessageWord();
					i += 8;
				}
				while (i < length)
				{
					this.m = (long)((ulong)this.m >> 8 | (ulong)input[offset + i] << 56);
					i++;
				}
				this.wordPos = length - num;
				return;
			}
			int num2 = this.wordPos << 3;
			while (i < num)
			{
				ulong num3 = Pack.LE_To_UInt64(input, offset + i);
				this.m = (long)(num3 << num2 | (ulong)this.m >> -num2);
				this.ProcessMessageWord();
				this.m = (long)num3;
				i += 8;
			}
			while (i < length)
			{
				this.m = (long)((ulong)this.m >> 8 | (ulong)input[offset + i] << 56);
				int num4 = this.wordPos + 1;
				this.wordPos = num4;
				if (num4 == 8)
				{
					this.ProcessMessageWord();
					this.wordPos = 0;
				}
				i++;
			}
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000CD9DC File Offset: 0x000CBBDC
		public virtual long DoFinal()
		{
			this.m = (long)((ulong)this.m >> (7 - this.wordPos << 3));
			this.m = (long)((ulong)this.m >> 8);
			this.m |= (long)((this.wordCount << 3) + this.wordPos) << 56;
			this.ProcessMessageWord();
			this.v2 ^= 255L;
			this.ApplySipRounds(this.d);
			long result = this.v0 ^ this.v1 ^ this.v2 ^ this.v3;
			this.Reset();
			return result;
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000CDA77 File Offset: 0x000CBC77
		public virtual int DoFinal(byte[] output, int outOff)
		{
			Pack.UInt64_To_LE((ulong)this.DoFinal(), output, outOff);
			return 8;
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000CDA88 File Offset: 0x000CBC88
		public virtual void Reset()
		{
			this.v0 = (this.k0 ^ 8317987319222330741L);
			this.v1 = (this.k1 ^ 7237128888997146477L);
			this.v2 = (this.k0 ^ 7816392313619706465L);
			this.v3 = (this.k1 ^ 8387220255154660723L);
			this.m = 0L;
			this.wordPos = 0;
			this.wordCount = 0;
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x000CDB04 File Offset: 0x000CBD04
		protected virtual void ProcessMessageWord()
		{
			this.wordCount++;
			this.v3 ^= this.m;
			this.ApplySipRounds(this.c);
			this.v0 ^= this.m;
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000CDB54 File Offset: 0x000CBD54
		protected virtual void ApplySipRounds(int n)
		{
			long num = this.v0;
			long num2 = this.v1;
			long num3 = this.v2;
			long num4 = this.v3;
			for (int i = 0; i < n; i++)
			{
				num += num2;
				num3 += num4;
				num2 = SipHash.RotateLeft(num2, 13);
				num4 = SipHash.RotateLeft(num4, 16);
				num2 ^= num;
				num4 ^= num3;
				num = SipHash.RotateLeft(num, 32);
				num3 += num2;
				num += num4;
				num2 = SipHash.RotateLeft(num2, 17);
				num4 = SipHash.RotateLeft(num4, 21);
				num2 ^= num3;
				num4 ^= num;
				num3 = SipHash.RotateLeft(num3, 32);
			}
			this.v0 = num;
			this.v1 = num2;
			this.v2 = num3;
			this.v3 = num4;
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000CDC00 File Offset: 0x000CBE00
		protected static long RotateLeft(long x, int n)
		{
			return x << n | (long)((ulong)x >> -n);
		}

		// Token: 0x04001B47 RID: 6983
		protected readonly int c;

		// Token: 0x04001B48 RID: 6984
		protected readonly int d;

		// Token: 0x04001B49 RID: 6985
		protected long k0;

		// Token: 0x04001B4A RID: 6986
		protected long k1;

		// Token: 0x04001B4B RID: 6987
		protected long v0;

		// Token: 0x04001B4C RID: 6988
		protected long v1;

		// Token: 0x04001B4D RID: 6989
		protected long v2;

		// Token: 0x04001B4E RID: 6990
		protected long v3;

		// Token: 0x04001B4F RID: 6991
		protected long m;

		// Token: 0x04001B50 RID: 6992
		protected int wordPos;

		// Token: 0x04001B51 RID: 6993
		protected int wordCount;
	}
}

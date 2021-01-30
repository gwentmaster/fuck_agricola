using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000490 RID: 1168
	public class NoekeonEngine : IBlockCipher
	{
		// Token: 0x06002A95 RID: 10901 RVA: 0x000D77BC File Offset: 0x000D59BC
		public NoekeonEngine()
		{
			this._initialised = false;
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06002A96 RID: 10902 RVA: 0x000D77EF File Offset: 0x000D59EF
		public virtual string AlgorithmName
		{
			get
			{
				return "Noekeon";
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06002A97 RID: 10903 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000C8990 File Offset: 0x000C6B90
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000D77F8 File Offset: 0x000D59F8
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("Invalid parameters passed to Noekeon init - " + Platform.GetTypeName(parameters), "parameters");
			}
			this._forEncryption = forEncryption;
			this._initialised = true;
			KeyParameter keyParameter = (KeyParameter)parameters;
			this.setKey(keyParameter.GetKey());
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000D784C File Offset: 0x000D5A4C
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this._initialised)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			Check.DataLength(input, inOff, 16, "input buffer too short");
			Check.OutputLength(output, outOff, 16, "output buffer too short");
			if (!this._forEncryption)
			{
				return this.decryptBlock(input, inOff, output, outOff);
			}
			return this.encryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000D78B3 File Offset: 0x000D5AB3
		private void setKey(byte[] key)
		{
			this.subKeys[0] = Pack.BE_To_UInt32(key, 0);
			this.subKeys[1] = Pack.BE_To_UInt32(key, 4);
			this.subKeys[2] = Pack.BE_To_UInt32(key, 8);
			this.subKeys[3] = Pack.BE_To_UInt32(key, 12);
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000D78F4 File Offset: 0x000D5AF4
		private int encryptBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			this.state[0] = Pack.BE_To_UInt32(input, inOff);
			this.state[1] = Pack.BE_To_UInt32(input, inOff + 4);
			this.state[2] = Pack.BE_To_UInt32(input, inOff + 8);
			this.state[3] = Pack.BE_To_UInt32(input, inOff + 12);
			int i;
			for (i = 0; i < 16; i++)
			{
				this.state[0] ^= NoekeonEngine.roundConstants[i];
				this.theta(this.state, this.subKeys);
				this.pi1(this.state);
				this.gamma(this.state);
				this.pi2(this.state);
			}
			this.state[0] ^= NoekeonEngine.roundConstants[i];
			this.theta(this.state, this.subKeys);
			Pack.UInt32_To_BE(this.state[0], output, outOff);
			Pack.UInt32_To_BE(this.state[1], output, outOff + 4);
			Pack.UInt32_To_BE(this.state[2], output, outOff + 8);
			Pack.UInt32_To_BE(this.state[3], output, outOff + 12);
			return 16;
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000D7A10 File Offset: 0x000D5C10
		private int decryptBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			this.state[0] = Pack.BE_To_UInt32(input, inOff);
			this.state[1] = Pack.BE_To_UInt32(input, inOff + 4);
			this.state[2] = Pack.BE_To_UInt32(input, inOff + 8);
			this.state[3] = Pack.BE_To_UInt32(input, inOff + 12);
			Array.Copy(this.subKeys, 0, this.decryptKeys, 0, this.subKeys.Length);
			this.theta(this.decryptKeys, NoekeonEngine.nullVector);
			int i;
			for (i = 16; i > 0; i--)
			{
				this.theta(this.state, this.decryptKeys);
				this.state[0] ^= NoekeonEngine.roundConstants[i];
				this.pi1(this.state);
				this.gamma(this.state);
				this.pi2(this.state);
			}
			this.theta(this.state, this.decryptKeys);
			this.state[0] ^= NoekeonEngine.roundConstants[i];
			Pack.UInt32_To_BE(this.state[0], output, outOff);
			Pack.UInt32_To_BE(this.state[1], output, outOff + 4);
			Pack.UInt32_To_BE(this.state[2], output, outOff + 8);
			Pack.UInt32_To_BE(this.state[3], output, outOff + 12);
			return 16;
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000D7B58 File Offset: 0x000D5D58
		private void gamma(uint[] a)
		{
			a[1] ^= (~a[3] & ~a[2]);
			a[0] ^= (a[2] & a[1]);
			uint num = a[3];
			a[3] = a[0];
			a[0] = num;
			a[2] ^= (a[0] ^ a[1] ^ a[3]);
			a[1] ^= (~a[3] & ~a[2]);
			a[0] ^= (a[2] & a[1]);
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000D7BD8 File Offset: 0x000D5DD8
		private void theta(uint[] a, uint[] k)
		{
			uint num = a[0] ^ a[2];
			num ^= (this.rotl(num, 8) ^ this.rotl(num, 24));
			a[1] ^= num;
			a[3] ^= num;
			for (int i = 0; i < 4; i++)
			{
				a[i] ^= k[i];
			}
			num = (a[1] ^ a[3]);
			num ^= (this.rotl(num, 8) ^ this.rotl(num, 24));
			a[0] ^= num;
			a[2] ^= num;
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000D7C69 File Offset: 0x000D5E69
		private void pi1(uint[] a)
		{
			a[1] = this.rotl(a[1], 1);
			a[2] = this.rotl(a[2], 5);
			a[3] = this.rotl(a[3], 2);
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x000D7C92 File Offset: 0x000D5E92
		private void pi2(uint[] a)
		{
			a[1] = this.rotl(a[1], 31);
			a[2] = this.rotl(a[2], 27);
			a[3] = this.rotl(a[3], 30);
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x000D7CBE File Offset: 0x000D5EBE
		private uint rotl(uint x, int y)
		{
			return x << y | x >> 32 - y;
		}

		// Token: 0x04001C1A RID: 7194
		private const int GenericSize = 16;

		// Token: 0x04001C1B RID: 7195
		private static readonly uint[] nullVector = new uint[4];

		// Token: 0x04001C1C RID: 7196
		private static readonly uint[] roundConstants = new uint[]
		{
			128U,
			27U,
			54U,
			108U,
			216U,
			171U,
			77U,
			154U,
			47U,
			94U,
			188U,
			99U,
			198U,
			151U,
			53U,
			106U,
			212U
		};

		// Token: 0x04001C1D RID: 7197
		private uint[] state = new uint[4];

		// Token: 0x04001C1E RID: 7198
		private uint[] subKeys = new uint[4];

		// Token: 0x04001C1F RID: 7199
		private uint[] decryptKeys = new uint[4];

		// Token: 0x04001C20 RID: 7200
		private bool _initialised;

		// Token: 0x04001C21 RID: 7201
		private bool _forEncryption;
	}
}

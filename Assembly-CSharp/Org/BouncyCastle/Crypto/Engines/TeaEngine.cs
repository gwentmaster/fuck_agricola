using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x020004A2 RID: 1186
	public class TeaEngine : IBlockCipher
	{
		// Token: 0x06002B76 RID: 11126 RVA: 0x000DF0A8 File Offset: 0x000DD2A8
		public TeaEngine()
		{
			this._initialised = false;
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06002B77 RID: 11127 RVA: 0x000DF0B7 File Offset: 0x000DD2B7
		public virtual string AlgorithmName
		{
			get
			{
				return "TEA";
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06002B78 RID: 11128 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x000DF0C0 File Offset: 0x000DD2C0
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to TEA init - " + Platform.GetTypeName(parameters));
			}
			this._forEncryption = forEncryption;
			this._initialised = true;
			KeyParameter keyParameter = (KeyParameter)parameters;
			this.setKey(keyParameter.GetKey());
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000DF10C File Offset: 0x000DD30C
		public virtual int ProcessBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			if (!this._initialised)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			Check.DataLength(inBytes, inOff, 8, "input buffer too short");
			Check.OutputLength(outBytes, outOff, 8, "output buffer too short");
			if (!this._forEncryption)
			{
				return this.decryptBlock(inBytes, inOff, outBytes, outOff);
			}
			return this.encryptBlock(inBytes, inOff, outBytes, outOff);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x000DF171 File Offset: 0x000DD371
		private void setKey(byte[] key)
		{
			this._a = Pack.BE_To_UInt32(key, 0);
			this._b = Pack.BE_To_UInt32(key, 4);
			this._c = Pack.BE_To_UInt32(key, 8);
			this._d = Pack.BE_To_UInt32(key, 12);
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x000DF1A8 File Offset: 0x000DD3A8
		private int encryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(inBytes, inOff);
			uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
			uint num3 = 0U;
			for (int num4 = 0; num4 != 32; num4++)
			{
				num3 += 2654435769U;
				num += ((num2 << 4) + this._a ^ num2 + num3 ^ (num2 >> 5) + this._b);
				num2 += ((num << 4) + this._c ^ num + num3 ^ (num >> 5) + this._d);
			}
			Pack.UInt32_To_BE(num, outBytes, outOff);
			Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x000DF22C File Offset: 0x000DD42C
		private int decryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(inBytes, inOff);
			uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
			uint num3 = 3337565984U;
			for (int num4 = 0; num4 != 32; num4++)
			{
				num2 -= ((num << 4) + this._c ^ num + num3 ^ (num >> 5) + this._d);
				num -= ((num2 << 4) + this._a ^ num2 + num3 ^ (num2 >> 5) + this._b);
				num3 -= 2654435769U;
			}
			Pack.UInt32_To_BE(num, outBytes, outOff);
			Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x04001C91 RID: 7313
		private const int rounds = 32;

		// Token: 0x04001C92 RID: 7314
		private const int block_size = 8;

		// Token: 0x04001C93 RID: 7315
		private const uint delta = 2654435769U;

		// Token: 0x04001C94 RID: 7316
		private const uint d_sum = 3337565984U;

		// Token: 0x04001C95 RID: 7317
		private uint _a;

		// Token: 0x04001C96 RID: 7318
		private uint _b;

		// Token: 0x04001C97 RID: 7319
		private uint _c;

		// Token: 0x04001C98 RID: 7320
		private uint _d;

		// Token: 0x04001C99 RID: 7321
		private bool _initialised;

		// Token: 0x04001C9A RID: 7322
		private bool _forEncryption;
	}
}

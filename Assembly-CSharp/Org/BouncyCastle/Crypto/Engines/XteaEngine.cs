using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x020004A6 RID: 1190
	public class XteaEngine : IBlockCipher
	{
		// Token: 0x06002BA4 RID: 11172 RVA: 0x000E071E File Offset: 0x000DE91E
		public XteaEngine()
		{
			this._initialised = false;
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06002BA5 RID: 11173 RVA: 0x000E0753 File Offset: 0x000DE953
		public virtual string AlgorithmName
		{
			get
			{
				return "XTEA";
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06002BA6 RID: 11174 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000E075C File Offset: 0x000DE95C
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

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000E07A8 File Offset: 0x000DE9A8
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

		// Token: 0x06002BAA RID: 11178 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x000E0810 File Offset: 0x000DEA10
		private void setKey(byte[] key)
		{
			int i;
			int num = i = 0;
			while (i < 4)
			{
				this._S[i] = Pack.BE_To_UInt32(key, num);
				i++;
				num += 4;
			}
			num = (i = 0);
			while (i < 32)
			{
				this._sum0[i] = (uint)(num + (int)this._S[num & 3]);
				num += -1640531527;
				this._sum1[i] = (uint)(num + (int)this._S[num >> 11 & 3]);
				i++;
			}
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000E0880 File Offset: 0x000DEA80
		private int encryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(inBytes, inOff);
			uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
			for (int i = 0; i < 32; i++)
			{
				num += ((num2 << 4 ^ num2 >> 5) + num2 ^ this._sum0[i]);
				num2 += ((num << 4 ^ num >> 5) + num ^ this._sum1[i]);
			}
			Pack.UInt32_To_BE(num, outBytes, outOff);
			Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000E08EC File Offset: 0x000DEAEC
		private int decryptBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(inBytes, inOff);
			uint num2 = Pack.BE_To_UInt32(inBytes, inOff + 4);
			for (int i = 31; i >= 0; i--)
			{
				num2 -= ((num << 4 ^ num >> 5) + num ^ this._sum1[i]);
				num -= ((num2 << 4 ^ num2 >> 5) + num2 ^ this._sum0[i]);
			}
			Pack.UInt32_To_BE(num, outBytes, outOff);
			Pack.UInt32_To_BE(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x04001CCD RID: 7373
		private const int rounds = 32;

		// Token: 0x04001CCE RID: 7374
		private const int block_size = 8;

		// Token: 0x04001CCF RID: 7375
		private const int delta = -1640531527;

		// Token: 0x04001CD0 RID: 7376
		private uint[] _S = new uint[4];

		// Token: 0x04001CD1 RID: 7377
		private uint[] _sum0 = new uint[32];

		// Token: 0x04001CD2 RID: 7378
		private uint[] _sum1 = new uint[32];

		// Token: 0x04001CD3 RID: 7379
		private bool _initialised;

		// Token: 0x04001CD4 RID: 7380
		private bool _forEncryption;
	}
}

using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000471 RID: 1137
	public class Poly1305 : IMac
	{
		// Token: 0x06002972 RID: 10610 RVA: 0x000CCE93 File Offset: 0x000CB093
		public Poly1305()
		{
			this.cipher = null;
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000CCEBB File Offset: 0x000CB0BB
		public Poly1305(IBlockCipher cipher)
		{
			if (cipher.GetBlockSize() != 16)
			{
				throw new ArgumentException("Poly1305 requires a 128 bit block cipher.");
			}
			this.cipher = cipher;
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000CCEF8 File Offset: 0x000CB0F8
		public void Init(ICipherParameters parameters)
		{
			byte[] nonce = null;
			if (this.cipher != null)
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("Poly1305 requires an IV when used with a block cipher.", "parameters");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				nonce = parametersWithIV.GetIV();
				parameters = parametersWithIV.Parameters;
			}
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("Poly1305 requires a key.");
			}
			KeyParameter keyParameter = (KeyParameter)parameters;
			this.SetKey(keyParameter.GetKey(), nonce);
			this.Reset();
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000CCF68 File Offset: 0x000CB168
		private void SetKey(byte[] key, byte[] nonce)
		{
			if (key.Length != 32)
			{
				throw new ArgumentException("Poly1305 key must be 256 bits.");
			}
			if (this.cipher != null && (nonce == null || nonce.Length != 16))
			{
				throw new ArgumentException("Poly1305 requires a 128 bit IV.");
			}
			uint num = Pack.LE_To_UInt32(key, 0);
			uint num2 = Pack.LE_To_UInt32(key, 4);
			uint num3 = Pack.LE_To_UInt32(key, 8);
			uint num4 = Pack.LE_To_UInt32(key, 12);
			this.r0 = (num & 67108863U);
			this.r1 = ((num >> 26 | num2 << 6) & 67108611U);
			this.r2 = ((num2 >> 20 | num3 << 12) & 67092735U);
			this.r3 = ((num3 >> 14 | num4 << 18) & 66076671U);
			this.r4 = (num4 >> 8 & 1048575U);
			this.s1 = this.r1 * 5U;
			this.s2 = this.r2 * 5U;
			this.s3 = this.r3 * 5U;
			this.s4 = this.r4 * 5U;
			byte[] array;
			int num5;
			if (this.cipher == null)
			{
				array = key;
				num5 = 16;
			}
			else
			{
				array = new byte[16];
				num5 = 0;
				this.cipher.Init(true, new KeyParameter(key, 16, 16));
				this.cipher.ProcessBlock(nonce, 0, array, 0);
			}
			this.k0 = Pack.LE_To_UInt32(array, num5);
			this.k1 = Pack.LE_To_UInt32(array, num5 + 4);
			this.k2 = Pack.LE_To_UInt32(array, num5 + 8);
			this.k3 = Pack.LE_To_UInt32(array, num5 + 12);
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06002976 RID: 10614 RVA: 0x000CD0DE File Offset: 0x000CB2DE
		public string AlgorithmName
		{
			get
			{
				if (this.cipher != null)
				{
					return "Poly1305-" + this.cipher.AlgorithmName;
				}
				return "Poly1305";
			}
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000C8990 File Offset: 0x000C6B90
		public int GetMacSize()
		{
			return 16;
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000CD103 File Offset: 0x000CB303
		public void Update(byte input)
		{
			this.singleByte[0] = input;
			this.BlockUpdate(this.singleByte, 0, 1);
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x000CD11C File Offset: 0x000CB31C
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			int num = 0;
			while (len > num)
			{
				if (this.currentBlockOffset == 16)
				{
					this.ProcessBlock();
					this.currentBlockOffset = 0;
				}
				int num2 = Math.Min(len - num, 16 - this.currentBlockOffset);
				Array.Copy(input, num + inOff, this.currentBlock, this.currentBlockOffset, num2);
				num += num2;
				this.currentBlockOffset += num2;
			}
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000CD184 File Offset: 0x000CB384
		private void ProcessBlock()
		{
			if (this.currentBlockOffset < 16)
			{
				this.currentBlock[this.currentBlockOffset] = 1;
				for (int i = this.currentBlockOffset + 1; i < 16; i++)
				{
					this.currentBlock[i] = 0;
				}
			}
			ulong num = (ulong)Pack.LE_To_UInt32(this.currentBlock, 0);
			ulong num2 = (ulong)Pack.LE_To_UInt32(this.currentBlock, 4);
			ulong num3 = (ulong)Pack.LE_To_UInt32(this.currentBlock, 8);
			ulong num4 = (ulong)Pack.LE_To_UInt32(this.currentBlock, 12);
			this.h0 += (uint)(num & 67108863UL);
			this.h1 += (uint)((num2 << 32 | num) >> 26 & 67108863UL);
			this.h2 += (uint)((num3 << 32 | num2) >> 20 & 67108863UL);
			this.h3 += (uint)((num4 << 32 | num3) >> 14 & 67108863UL);
			this.h4 += (uint)(num4 >> 8);
			if (this.currentBlockOffset == 16)
			{
				this.h4 += 16777216U;
			}
			ulong num5 = Poly1305.mul32x32_64(this.h0, this.r0) + Poly1305.mul32x32_64(this.h1, this.s4) + Poly1305.mul32x32_64(this.h2, this.s3) + Poly1305.mul32x32_64(this.h3, this.s2) + Poly1305.mul32x32_64(this.h4, this.s1);
			ulong num6 = Poly1305.mul32x32_64(this.h0, this.r1) + Poly1305.mul32x32_64(this.h1, this.r0) + Poly1305.mul32x32_64(this.h2, this.s4) + Poly1305.mul32x32_64(this.h3, this.s3) + Poly1305.mul32x32_64(this.h4, this.s2);
			ulong num7 = Poly1305.mul32x32_64(this.h0, this.r2) + Poly1305.mul32x32_64(this.h1, this.r1) + Poly1305.mul32x32_64(this.h2, this.r0) + Poly1305.mul32x32_64(this.h3, this.s4) + Poly1305.mul32x32_64(this.h4, this.s3);
			ulong num8 = Poly1305.mul32x32_64(this.h0, this.r3) + Poly1305.mul32x32_64(this.h1, this.r2) + Poly1305.mul32x32_64(this.h2, this.r1) + Poly1305.mul32x32_64(this.h3, this.r0) + Poly1305.mul32x32_64(this.h4, this.s4);
			ulong num9 = Poly1305.mul32x32_64(this.h0, this.r4) + Poly1305.mul32x32_64(this.h1, this.r3) + Poly1305.mul32x32_64(this.h2, this.r2) + Poly1305.mul32x32_64(this.h3, this.r1) + Poly1305.mul32x32_64(this.h4, this.r0);
			this.h0 = ((uint)num5 & 67108863U);
			ulong num10 = num5 >> 26;
			num6 += num10;
			this.h1 = ((uint)num6 & 67108863U);
			num10 = num6 >> 26;
			num7 += num10;
			this.h2 = ((uint)num7 & 67108863U);
			num10 = num7 >> 26;
			num8 += num10;
			this.h3 = ((uint)num8 & 67108863U);
			num10 = num8 >> 26;
			num9 += num10;
			this.h4 = ((uint)num9 & 67108863U);
			num10 = num9 >> 26;
			this.h0 += (uint)(num10 * 5UL);
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x000CD508 File Offset: 0x000CB708
		public int DoFinal(byte[] output, int outOff)
		{
			Check.DataLength(output, outOff, 16, "Output buffer is too short.");
			if (this.currentBlockOffset > 0)
			{
				this.ProcessBlock();
			}
			uint num = this.h0 >> 26;
			this.h0 &= 67108863U;
			this.h1 += num;
			num = this.h1 >> 26;
			this.h1 &= 67108863U;
			this.h2 += num;
			num = this.h2 >> 26;
			this.h2 &= 67108863U;
			this.h3 += num;
			num = this.h3 >> 26;
			this.h3 &= 67108863U;
			this.h4 += num;
			num = this.h4 >> 26;
			this.h4 &= 67108863U;
			this.h0 += num * 5U;
			uint num2 = this.h0 + 5U;
			num = num2 >> 26;
			num2 &= 67108863U;
			uint num3 = this.h1 + num;
			num = num3 >> 26;
			num3 &= 67108863U;
			uint num4 = this.h2 + num;
			num = num4 >> 26;
			num4 &= 67108863U;
			uint num5 = this.h3 + num;
			num = num5 >> 26;
			num5 &= 67108863U;
			uint num6 = this.h4 + num - 67108864U;
			num = (num6 >> 31) - 1U;
			uint num7 = ~num;
			this.h0 = ((this.h0 & num7) | (num2 & num));
			this.h1 = ((this.h1 & num7) | (num3 & num));
			this.h2 = ((this.h2 & num7) | (num4 & num));
			this.h3 = ((this.h3 & num7) | (num5 & num));
			this.h4 = ((this.h4 & num7) | (num6 & num));
			ulong num8 = (ulong)(this.h0 | this.h1 << 26) + (ulong)this.k0;
			ulong num9 = (ulong)(this.h1 >> 6 | this.h2 << 20) + (ulong)this.k1;
			ulong num10 = (ulong)(this.h2 >> 12 | this.h3 << 14) + (ulong)this.k2;
			ulong num11 = (ulong)(this.h3 >> 18 | this.h4 << 8) + (ulong)this.k3;
			Pack.UInt32_To_LE((uint)num8, output, outOff);
			num9 += num8 >> 32;
			Pack.UInt32_To_LE((uint)num9, output, outOff + 4);
			num10 += num9 >> 32;
			Pack.UInt32_To_LE((uint)num10, output, outOff + 8);
			Pack.UInt32_To_LE((uint)(num11 + (num10 >> 32)), output, outOff + 12);
			this.Reset();
			return 16;
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000CD7A4 File Offset: 0x000CB9A4
		public void Reset()
		{
			this.currentBlockOffset = 0;
			this.h0 = (this.h1 = (this.h2 = (this.h3 = (this.h4 = 0U))));
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000CD7E3 File Offset: 0x000CB9E3
		private static ulong mul32x32_64(uint i1, uint i2)
		{
			return (ulong)i1 * (ulong)i2;
		}

		// Token: 0x04001B30 RID: 6960
		private const int BlockSize = 16;

		// Token: 0x04001B31 RID: 6961
		private readonly IBlockCipher cipher;

		// Token: 0x04001B32 RID: 6962
		private readonly byte[] singleByte = new byte[1];

		// Token: 0x04001B33 RID: 6963
		private uint r0;

		// Token: 0x04001B34 RID: 6964
		private uint r1;

		// Token: 0x04001B35 RID: 6965
		private uint r2;

		// Token: 0x04001B36 RID: 6966
		private uint r3;

		// Token: 0x04001B37 RID: 6967
		private uint r4;

		// Token: 0x04001B38 RID: 6968
		private uint s1;

		// Token: 0x04001B39 RID: 6969
		private uint s2;

		// Token: 0x04001B3A RID: 6970
		private uint s3;

		// Token: 0x04001B3B RID: 6971
		private uint s4;

		// Token: 0x04001B3C RID: 6972
		private uint k0;

		// Token: 0x04001B3D RID: 6973
		private uint k1;

		// Token: 0x04001B3E RID: 6974
		private uint k2;

		// Token: 0x04001B3F RID: 6975
		private uint k3;

		// Token: 0x04001B40 RID: 6976
		private byte[] currentBlock = new byte[16];

		// Token: 0x04001B41 RID: 6977
		private int currentBlockOffset;

		// Token: 0x04001B42 RID: 6978
		private uint h0;

		// Token: 0x04001B43 RID: 6979
		private uint h1;

		// Token: 0x04001B44 RID: 6980
		private uint h2;

		// Token: 0x04001B45 RID: 6981
		private uint h3;

		// Token: 0x04001B46 RID: 6982
		private uint h4;
	}
}

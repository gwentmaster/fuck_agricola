using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x020004A0 RID: 1184
	public abstract class SerpentEngineBase : IBlockCipher
	{
		// Token: 0x06002B4C RID: 11084 RVA: 0x000DE31C File Offset: 0x000DC51C
		public virtual void Init(bool encrypting, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to " + this.AlgorithmName + " init - " + Platform.GetTypeName(parameters));
			}
			this.encrypting = encrypting;
			this.wKey = this.MakeWorkingKey(((KeyParameter)parameters).GetKey());
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06002B4D RID: 11085 RVA: 0x000DE370 File Offset: 0x000DC570
		public virtual string AlgorithmName
		{
			get
			{
				return "Serpent";
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06002B4E RID: 11086 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x000DE377 File Offset: 0x000DC577
		public virtual int GetBlockSize()
		{
			return SerpentEngineBase.BlockSize;
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000DE380 File Offset: 0x000DC580
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.wKey == null)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			Check.DataLength(input, inOff, SerpentEngineBase.BlockSize, "input buffer too short");
			Check.OutputLength(output, outOff, SerpentEngineBase.BlockSize, "output buffer too short");
			if (this.encrypting)
			{
				this.EncryptBlock(input, inOff, output, outOff);
			}
			else
			{
				this.DecryptBlock(input, inOff, output, outOff);
			}
			return SerpentEngineBase.BlockSize;
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x000DBDD6 File Offset: 0x000D9FD6
		protected static int RotateLeft(int x, int bits)
		{
			return x << bits | (int)((uint)x >> 32 - bits);
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000CEF32 File Offset: 0x000CD132
		private static int RotateRight(int x, int bits)
		{
			return (int)((uint)x >> bits | (uint)((uint)x << 32 - bits));
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000DE3F4 File Offset: 0x000DC5F4
		protected void Sb0(int a, int b, int c, int d)
		{
			int num = a ^ d;
			int num2 = c ^ num;
			int num3 = b ^ num2;
			this.X3 = ((a & d) ^ num3);
			int num4 = a ^ (b & num);
			this.X2 = (num3 ^ (c | num4));
			int num5 = this.X3 & (num2 ^ num4);
			this.X1 = (~num2 ^ num5);
			this.X0 = (num5 ^ ~num4);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000DE450 File Offset: 0x000DC650
		protected void Ib0(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = a ^ b;
			int num3 = d ^ (num | num2);
			int num4 = c ^ num3;
			this.X2 = (num2 ^ num4);
			int num5 = num ^ (d & num2);
			this.X1 = (num3 ^ (this.X2 & num5));
			this.X3 = ((a & num3) ^ (num4 | this.X1));
			this.X0 = (this.X3 ^ (num4 ^ num5));
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000DE4B4 File Offset: 0x000DC6B4
		protected void Sb1(int a, int b, int c, int d)
		{
			int num = b ^ ~a;
			int num2 = c ^ (a | num);
			this.X2 = (d ^ num2);
			int num3 = b ^ (d | num);
			int num4 = num ^ this.X2;
			this.X3 = (num4 ^ (num2 & num3));
			int num5 = num2 ^ num3;
			this.X1 = (this.X3 ^ num5);
			this.X0 = (num2 ^ (num4 & num5));
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000DE514 File Offset: 0x000DC714
		protected void Ib1(int a, int b, int c, int d)
		{
			int num = b ^ d;
			int num2 = a ^ (b & num);
			int num3 = num ^ num2;
			this.X3 = (c ^ num3);
			int num4 = b ^ (num & num2);
			int num5 = this.X3 | num4;
			this.X1 = (num2 ^ num5);
			int num6 = ~this.X1;
			int num7 = this.X3 ^ num4;
			this.X0 = (num6 ^ num7);
			this.X2 = (num3 ^ (num6 | num7));
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000DE580 File Offset: 0x000DC780
		protected void Sb2(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = b ^ d;
			int num3 = c & num;
			this.X0 = (num2 ^ num3);
			int num4 = c ^ num;
			int num5 = c ^ this.X0;
			int num6 = b & num5;
			this.X3 = (num4 ^ num6);
			this.X2 = (a ^ ((d | num6) & (this.X0 | num4)));
			this.X1 = (num2 ^ this.X3 ^ (this.X2 ^ (d | num)));
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000DE5F0 File Offset: 0x000DC7F0
		protected void Ib2(int a, int b, int c, int d)
		{
			int num = b ^ d;
			int num2 = ~num;
			int num3 = a ^ c;
			int num4 = c ^ num;
			int num5 = b & num4;
			this.X0 = (num3 ^ num5);
			int num6 = a | num2;
			int num7 = d ^ num6;
			int num8 = num3 | num7;
			this.X3 = (num ^ num8);
			int num9 = ~num4;
			int num10 = this.X0 | this.X3;
			this.X1 = (num9 ^ num10);
			this.X2 = ((d & num9) ^ (num3 ^ num10));
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000DE668 File Offset: 0x000DC868
		protected void Sb3(int a, int b, int c, int d)
		{
			int num = a ^ b;
			int num2 = a & c;
			int num3 = a | d;
			int num4 = c ^ d;
			int num5 = num & num3;
			int num6 = num2 | num5;
			this.X2 = (num4 ^ num6);
			int num7 = b ^ num3;
			int num8 = num6 ^ num7;
			int num9 = num4 & num8;
			this.X0 = (num ^ num9);
			int num10 = this.X2 & this.X0;
			this.X1 = (num8 ^ num10);
			this.X3 = ((b | d) ^ (num4 ^ num10));
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x000DE6E0 File Offset: 0x000DC8E0
		protected void Ib3(int a, int b, int c, int d)
		{
			int num = a | b;
			int num2 = b ^ c;
			int num3 = b & num2;
			int num4 = a ^ num3;
			int num5 = c ^ num4;
			int num6 = d | num4;
			this.X0 = (num2 ^ num6);
			int num7 = num2 | num6;
			int num8 = d ^ num7;
			this.X2 = (num5 ^ num8);
			int num9 = num ^ num8;
			int num10 = this.X0 & num9;
			this.X3 = (num4 ^ num10);
			this.X1 = (this.X3 ^ (this.X0 ^ num9));
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000DE758 File Offset: 0x000DC958
		protected void Sb4(int a, int b, int c, int d)
		{
			int num = a ^ d;
			int num2 = d & num;
			int num3 = c ^ num2;
			int num4 = b | num3;
			this.X3 = (num ^ num4);
			int num5 = ~b;
			int num6 = num | num5;
			this.X0 = (num3 ^ num6);
			int num7 = a & this.X0;
			int num8 = num ^ num5;
			int num9 = num4 & num8;
			this.X2 = (num7 ^ num9);
			this.X1 = (a ^ num3 ^ (num8 & this.X2));
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000DE7C8 File Offset: 0x000DC9C8
		protected void Ib4(int a, int b, int c, int d)
		{
			int num = c | d;
			int num2 = a & num;
			int num3 = b ^ num2;
			int num4 = a & num3;
			int num5 = c ^ num4;
			this.X1 = (d ^ num5);
			int num6 = ~a;
			int num7 = num5 & this.X1;
			this.X3 = (num3 ^ num7);
			int num8 = this.X1 | num6;
			int num9 = d ^ num8;
			this.X0 = (this.X3 ^ num9);
			this.X2 = ((num3 & num9) ^ (this.X1 ^ num6));
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x000DE844 File Offset: 0x000DCA44
		protected void Sb5(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = a ^ b;
			int num3 = a ^ d;
			int num4 = c ^ num;
			int num5 = num2 | num3;
			this.X0 = (num4 ^ num5);
			int num6 = d & this.X0;
			int num7 = num2 ^ this.X0;
			this.X1 = (num6 ^ num7);
			int num8 = num | this.X0;
			int num9 = num2 | num6;
			int num10 = num3 ^ num8;
			this.X2 = (num9 ^ num10);
			this.X3 = (b ^ num6 ^ (this.X1 & num10));
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000DE8C4 File Offset: 0x000DCAC4
		protected void Ib5(int a, int b, int c, int d)
		{
			int num = ~c;
			int num2 = b & num;
			int num3 = d ^ num2;
			int num4 = a & num3;
			int num5 = b ^ num;
			this.X3 = (num4 ^ num5);
			int num6 = b | this.X3;
			int num7 = a & num6;
			this.X1 = (num3 ^ num7);
			int num8 = a | d;
			int num9 = num ^ num6;
			this.X0 = (num8 ^ num9);
			this.X2 = ((b & num8) ^ (num4 | (a ^ c)));
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000DE934 File Offset: 0x000DCB34
		protected void Sb6(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = a ^ d;
			int num3 = b ^ num2;
			int num4 = num | num2;
			int num5 = c ^ num4;
			this.X1 = (b ^ num5);
			int num6 = num2 | this.X1;
			int num7 = d ^ num6;
			int num8 = num5 & num7;
			this.X2 = (num3 ^ num8);
			int num9 = num5 ^ num7;
			this.X0 = (this.X2 ^ num9);
			this.X3 = (~num5 ^ (num3 & num9));
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000DE9A0 File Offset: 0x000DCBA0
		protected void Ib6(int a, int b, int c, int d)
		{
			int num = ~a;
			int num2 = a ^ b;
			int num3 = c ^ num2;
			int num4 = c | num;
			int num5 = d ^ num4;
			this.X1 = (num3 ^ num5);
			int num6 = num3 & num5;
			int num7 = num2 ^ num6;
			int num8 = b | num7;
			this.X3 = (num5 ^ num8);
			int num9 = b | this.X3;
			this.X0 = (num7 ^ num9);
			this.X2 = ((d & num) ^ (num3 ^ num9));
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000DEA10 File Offset: 0x000DCC10
		protected void Sb7(int a, int b, int c, int d)
		{
			int num = b ^ c;
			int num2 = c & num;
			int num3 = d ^ num2;
			int num4 = a ^ num3;
			int num5 = d | num;
			int num6 = num4 & num5;
			this.X1 = (b ^ num6);
			int num7 = num3 | this.X1;
			int num8 = a & num4;
			this.X3 = (num ^ num8);
			int num9 = num4 ^ num7;
			int num10 = this.X3 & num9;
			this.X2 = (num3 ^ num10);
			this.X0 = (~num9 ^ (this.X3 & this.X2));
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000DEA90 File Offset: 0x000DCC90
		protected void Ib7(int a, int b, int c, int d)
		{
			int num = c | (a & b);
			int num2 = d & (a | b);
			this.X3 = (num ^ num2);
			int num3 = ~d;
			int num4 = b ^ num2;
			int num5 = num4 | (this.X3 ^ num3);
			this.X1 = (a ^ num5);
			this.X0 = (c ^ num4 ^ (d | this.X1));
			this.X2 = (num ^ this.X1 ^ (this.X0 ^ (a & this.X3)));
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000DEB04 File Offset: 0x000DCD04
		protected void LT()
		{
			int num = SerpentEngineBase.RotateLeft(this.X0, 13);
			int num2 = SerpentEngineBase.RotateLeft(this.X2, 3);
			int x = this.X1 ^ num ^ num2;
			int x2 = this.X3 ^ num2 ^ num << 3;
			this.X1 = SerpentEngineBase.RotateLeft(x, 1);
			this.X3 = SerpentEngineBase.RotateLeft(x2, 7);
			this.X0 = SerpentEngineBase.RotateLeft(num ^ this.X1 ^ this.X3, 5);
			this.X2 = SerpentEngineBase.RotateLeft(num2 ^ this.X3 ^ this.X1 << 7, 22);
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x000DEB98 File Offset: 0x000DCD98
		protected void InverseLT()
		{
			int num = SerpentEngineBase.RotateRight(this.X2, 22) ^ this.X3 ^ this.X1 << 7;
			int num2 = SerpentEngineBase.RotateRight(this.X0, 5) ^ this.X1 ^ this.X3;
			int num3 = SerpentEngineBase.RotateRight(this.X3, 7);
			int num4 = SerpentEngineBase.RotateRight(this.X1, 1);
			this.X3 = (num3 ^ num ^ num2 << 3);
			this.X1 = (num4 ^ num2 ^ num);
			this.X2 = SerpentEngineBase.RotateRight(num, 3);
			this.X0 = SerpentEngineBase.RotateRight(num2, 13);
		}

		// Token: 0x06002B66 RID: 11110
		protected abstract int[] MakeWorkingKey(byte[] key);

		// Token: 0x06002B67 RID: 11111
		protected abstract void EncryptBlock(byte[] input, int inOff, byte[] output, int outOff);

		// Token: 0x06002B68 RID: 11112
		protected abstract void DecryptBlock(byte[] input, int inOff, byte[] output, int outOff);

		// Token: 0x04001C81 RID: 7297
		protected static readonly int BlockSize = 16;

		// Token: 0x04001C82 RID: 7298
		internal const int ROUNDS = 32;

		// Token: 0x04001C83 RID: 7299
		internal const int PHI = -1640531527;

		// Token: 0x04001C84 RID: 7300
		protected bool encrypting;

		// Token: 0x04001C85 RID: 7301
		protected int[] wKey;

		// Token: 0x04001C86 RID: 7302
		protected int X0;

		// Token: 0x04001C87 RID: 7303
		protected int X1;

		// Token: 0x04001C88 RID: 7304
		protected int X2;

		// Token: 0x04001C89 RID: 7305
		protected int X3;
	}
}

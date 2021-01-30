using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000481 RID: 1153
	public class CamelliaEngine : IBlockCipher
	{
		// Token: 0x060029F0 RID: 10736 RVA: 0x000D1A25 File Offset: 0x000CFC25
		private static uint rightRotate(uint x, int s)
		{
			return (x >> s) + (x << 32 - s);
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x000D1A37 File Offset: 0x000CFC37
		private static uint leftRotate(uint x, int s)
		{
			return (x << s) + (x >> 32 - s);
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x000D1A4C File Offset: 0x000CFC4C
		private static void roldq(int rot, uint[] ki, int ioff, uint[] ko, int ooff)
		{
			ko[ooff] = (ki[ioff] << rot | ki[1 + ioff] >> 32 - rot);
			ko[1 + ooff] = (ki[1 + ioff] << rot | ki[2 + ioff] >> 32 - rot);
			ko[2 + ooff] = (ki[2 + ioff] << rot | ki[3 + ioff] >> 32 - rot);
			ko[3 + ooff] = (ki[3 + ioff] << rot | ki[ioff] >> 32 - rot);
			ki[ioff] = ko[ooff];
			ki[1 + ioff] = ko[1 + ooff];
			ki[2 + ioff] = ko[2 + ooff];
			ki[3 + ioff] = ko[3 + ooff];
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x000D1AF4 File Offset: 0x000CFCF4
		private static void decroldq(int rot, uint[] ki, int ioff, uint[] ko, int ooff)
		{
			ko[2 + ooff] = (ki[ioff] << rot | ki[1 + ioff] >> 32 - rot);
			ko[3 + ooff] = (ki[1 + ioff] << rot | ki[2 + ioff] >> 32 - rot);
			ko[ooff] = (ki[2 + ioff] << rot | ki[3 + ioff] >> 32 - rot);
			ko[1 + ooff] = (ki[3 + ioff] << rot | ki[ioff] >> 32 - rot);
			ki[ioff] = ko[2 + ooff];
			ki[1 + ioff] = ko[3 + ooff];
			ki[2 + ioff] = ko[ooff];
			ki[3 + ioff] = ko[1 + ooff];
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x000D1B9C File Offset: 0x000CFD9C
		private static void roldqo32(int rot, uint[] ki, int ioff, uint[] ko, int ooff)
		{
			ko[ooff] = (ki[1 + ioff] << rot - 32 | ki[2 + ioff] >> 64 - rot);
			ko[1 + ooff] = (ki[2 + ioff] << rot - 32 | ki[3 + ioff] >> 64 - rot);
			ko[2 + ooff] = (ki[3 + ioff] << rot - 32 | ki[ioff] >> 64 - rot);
			ko[3 + ooff] = (ki[ioff] << rot - 32 | ki[1 + ioff] >> 64 - rot);
			ki[ioff] = ko[ooff];
			ki[1 + ioff] = ko[1 + ooff];
			ki[2 + ioff] = ko[2 + ooff];
			ki[3 + ioff] = ko[3 + ooff];
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x000D1C50 File Offset: 0x000CFE50
		private static void decroldqo32(int rot, uint[] ki, int ioff, uint[] ko, int ooff)
		{
			ko[2 + ooff] = (ki[1 + ioff] << rot - 32 | ki[2 + ioff] >> 64 - rot);
			ko[3 + ooff] = (ki[2 + ioff] << rot - 32 | ki[3 + ioff] >> 64 - rot);
			ko[ooff] = (ki[3 + ioff] << rot - 32 | ki[ioff] >> 64 - rot);
			ko[1 + ooff] = (ki[ioff] << rot - 32 | ki[1 + ioff] >> 64 - rot);
			ki[ioff] = ko[2 + ooff];
			ki[1 + ioff] = ko[3 + ooff];
			ki[2 + ioff] = ko[ooff];
			ki[3 + ioff] = ko[1 + ooff];
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x000D1D04 File Offset: 0x000CFF04
		private static uint bytes2uint(byte[] src, int offset)
		{
			uint num = 0U;
			for (int i = 0; i < 4; i++)
			{
				num = (num << 8) + (uint)src[i + offset];
			}
			return num;
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x000D1D2C File Offset: 0x000CFF2C
		private static void uint2bytes(uint word, byte[] dst, int offset)
		{
			for (int i = 0; i < 4; i++)
			{
				dst[3 - i + offset] = (byte)word;
				word >>= 8;
			}
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x000D1D54 File Offset: 0x000CFF54
		private static void camelliaF2(uint[] s, uint[] skey, int keyoff)
		{
			uint num = s[0] ^ skey[keyoff];
			uint num2 = CamelliaEngine.SBOX4_4404[(int)((byte)num)];
			num2 ^= CamelliaEngine.SBOX3_3033[(int)((byte)(num >> 8))];
			num2 ^= CamelliaEngine.SBOX2_0222[(int)((byte)(num >> 16))];
			num2 ^= CamelliaEngine.SBOX1_1110[(int)((byte)(num >> 24))];
			uint num3 = s[1] ^ skey[1 + keyoff];
			uint num4 = CamelliaEngine.SBOX1_1110[(int)((byte)num3)];
			num4 ^= CamelliaEngine.SBOX4_4404[(int)((byte)(num3 >> 8))];
			num4 ^= CamelliaEngine.SBOX3_3033[(int)((byte)(num3 >> 16))];
			num4 ^= CamelliaEngine.SBOX2_0222[(int)((byte)(num3 >> 24))];
			s[2] ^= (num2 ^ num4);
			s[3] ^= (num2 ^ num4 ^ CamelliaEngine.rightRotate(num2, 8));
			num = (s[2] ^ skey[2 + keyoff]);
			num2 = CamelliaEngine.SBOX4_4404[(int)((byte)num)];
			num2 ^= CamelliaEngine.SBOX3_3033[(int)((byte)(num >> 8))];
			num2 ^= CamelliaEngine.SBOX2_0222[(int)((byte)(num >> 16))];
			num2 ^= CamelliaEngine.SBOX1_1110[(int)((byte)(num >> 24))];
			num3 = (s[3] ^ skey[3 + keyoff]);
			num4 = CamelliaEngine.SBOX1_1110[(int)((byte)num3)];
			num4 ^= CamelliaEngine.SBOX4_4404[(int)((byte)(num3 >> 8))];
			num4 ^= CamelliaEngine.SBOX3_3033[(int)((byte)(num3 >> 16))];
			num4 ^= CamelliaEngine.SBOX2_0222[(int)((byte)(num3 >> 24))];
			s[0] ^= (num2 ^ num4);
			s[1] ^= (num2 ^ num4 ^ CamelliaEngine.rightRotate(num2, 8));
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x000D1E98 File Offset: 0x000D0098
		private static void camelliaFLs(uint[] s, uint[] fkey, int keyoff)
		{
			s[1] ^= CamelliaEngine.leftRotate(s[0] & fkey[keyoff], 1);
			s[0] ^= (fkey[1 + keyoff] | s[1]);
			s[2] ^= (fkey[3 + keyoff] | s[3]);
			s[3] ^= CamelliaEngine.leftRotate(fkey[2 + keyoff] & s[2], 1);
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x000D1F00 File Offset: 0x000D0100
		private void setKey(bool forEncryption, byte[] key)
		{
			uint[] array = new uint[8];
			uint[] array2 = new uint[4];
			uint[] array3 = new uint[4];
			uint[] array4 = new uint[4];
			int num = key.Length;
			if (num != 16)
			{
				if (num != 24)
				{
					if (num != 32)
					{
						throw new ArgumentException("key sizes are only 16/24/32 bytes.");
					}
					array[0] = CamelliaEngine.bytes2uint(key, 0);
					array[1] = CamelliaEngine.bytes2uint(key, 4);
					array[2] = CamelliaEngine.bytes2uint(key, 8);
					array[3] = CamelliaEngine.bytes2uint(key, 12);
					array[4] = CamelliaEngine.bytes2uint(key, 16);
					array[5] = CamelliaEngine.bytes2uint(key, 20);
					array[6] = CamelliaEngine.bytes2uint(key, 24);
					array[7] = CamelliaEngine.bytes2uint(key, 28);
					this._keyIs128 = false;
				}
				else
				{
					array[0] = CamelliaEngine.bytes2uint(key, 0);
					array[1] = CamelliaEngine.bytes2uint(key, 4);
					array[2] = CamelliaEngine.bytes2uint(key, 8);
					array[3] = CamelliaEngine.bytes2uint(key, 12);
					array[4] = CamelliaEngine.bytes2uint(key, 16);
					array[5] = CamelliaEngine.bytes2uint(key, 20);
					array[6] = ~array[4];
					array[7] = ~array[5];
					this._keyIs128 = false;
				}
			}
			else
			{
				this._keyIs128 = true;
				array[0] = CamelliaEngine.bytes2uint(key, 0);
				array[1] = CamelliaEngine.bytes2uint(key, 4);
				array[2] = CamelliaEngine.bytes2uint(key, 8);
				array[3] = CamelliaEngine.bytes2uint(key, 12);
				array[4] = (array[5] = (array[6] = (array[7] = 0U)));
			}
			for (int i = 0; i < 4; i++)
			{
				array2[i] = (array[i] ^ array[i + 4]);
			}
			CamelliaEngine.camelliaF2(array2, CamelliaEngine.SIGMA, 0);
			for (int j = 0; j < 4; j++)
			{
				array2[j] ^= array[j];
			}
			CamelliaEngine.camelliaF2(array2, CamelliaEngine.SIGMA, 4);
			if (this._keyIs128)
			{
				if (forEncryption)
				{
					this.kw[0] = array[0];
					this.kw[1] = array[1];
					this.kw[2] = array[2];
					this.kw[3] = array[3];
					CamelliaEngine.roldq(15, array, 0, this.subkey, 4);
					CamelliaEngine.roldq(30, array, 0, this.subkey, 12);
					CamelliaEngine.roldq(15, array, 0, array4, 0);
					this.subkey[18] = array4[2];
					this.subkey[19] = array4[3];
					CamelliaEngine.roldq(17, array, 0, this.ke, 4);
					CamelliaEngine.roldq(17, array, 0, this.subkey, 24);
					CamelliaEngine.roldq(17, array, 0, this.subkey, 32);
					this.subkey[0] = array2[0];
					this.subkey[1] = array2[1];
					this.subkey[2] = array2[2];
					this.subkey[3] = array2[3];
					CamelliaEngine.roldq(15, array2, 0, this.subkey, 8);
					CamelliaEngine.roldq(15, array2, 0, this.ke, 0);
					CamelliaEngine.roldq(15, array2, 0, array4, 0);
					this.subkey[16] = array4[0];
					this.subkey[17] = array4[1];
					CamelliaEngine.roldq(15, array2, 0, this.subkey, 20);
					CamelliaEngine.roldqo32(34, array2, 0, this.subkey, 28);
					CamelliaEngine.roldq(17, array2, 0, this.kw, 4);
					return;
				}
				this.kw[4] = array[0];
				this.kw[5] = array[1];
				this.kw[6] = array[2];
				this.kw[7] = array[3];
				CamelliaEngine.decroldq(15, array, 0, this.subkey, 28);
				CamelliaEngine.decroldq(30, array, 0, this.subkey, 20);
				CamelliaEngine.decroldq(15, array, 0, array4, 0);
				this.subkey[16] = array4[0];
				this.subkey[17] = array4[1];
				CamelliaEngine.decroldq(17, array, 0, this.ke, 0);
				CamelliaEngine.decroldq(17, array, 0, this.subkey, 8);
				CamelliaEngine.decroldq(17, array, 0, this.subkey, 0);
				this.subkey[34] = array2[0];
				this.subkey[35] = array2[1];
				this.subkey[32] = array2[2];
				this.subkey[33] = array2[3];
				CamelliaEngine.decroldq(15, array2, 0, this.subkey, 24);
				CamelliaEngine.decroldq(15, array2, 0, this.ke, 4);
				CamelliaEngine.decroldq(15, array2, 0, array4, 0);
				this.subkey[18] = array4[2];
				this.subkey[19] = array4[3];
				CamelliaEngine.decroldq(15, array2, 0, this.subkey, 12);
				CamelliaEngine.decroldqo32(34, array2, 0, this.subkey, 4);
				CamelliaEngine.roldq(17, array2, 0, this.kw, 0);
				return;
			}
			else
			{
				for (int k = 0; k < 4; k++)
				{
					array3[k] = (array2[k] ^ array[k + 4]);
				}
				CamelliaEngine.camelliaF2(array3, CamelliaEngine.SIGMA, 8);
				if (forEncryption)
				{
					this.kw[0] = array[0];
					this.kw[1] = array[1];
					this.kw[2] = array[2];
					this.kw[3] = array[3];
					CamelliaEngine.roldqo32(45, array, 0, this.subkey, 16);
					CamelliaEngine.roldq(15, array, 0, this.ke, 4);
					CamelliaEngine.roldq(17, array, 0, this.subkey, 32);
					CamelliaEngine.roldqo32(34, array, 0, this.subkey, 44);
					CamelliaEngine.roldq(15, array, 4, this.subkey, 4);
					CamelliaEngine.roldq(15, array, 4, this.ke, 0);
					CamelliaEngine.roldq(30, array, 4, this.subkey, 24);
					CamelliaEngine.roldqo32(34, array, 4, this.subkey, 36);
					CamelliaEngine.roldq(15, array2, 0, this.subkey, 8);
					CamelliaEngine.roldq(30, array2, 0, this.subkey, 20);
					this.ke[8] = array2[1];
					this.ke[9] = array2[2];
					this.ke[10] = array2[3];
					this.ke[11] = array2[0];
					CamelliaEngine.roldqo32(49, array2, 0, this.subkey, 40);
					this.subkey[0] = array3[0];
					this.subkey[1] = array3[1];
					this.subkey[2] = array3[2];
					this.subkey[3] = array3[3];
					CamelliaEngine.roldq(30, array3, 0, this.subkey, 12);
					CamelliaEngine.roldq(30, array3, 0, this.subkey, 28);
					CamelliaEngine.roldqo32(51, array3, 0, this.kw, 4);
					return;
				}
				this.kw[4] = array[0];
				this.kw[5] = array[1];
				this.kw[6] = array[2];
				this.kw[7] = array[3];
				CamelliaEngine.decroldqo32(45, array, 0, this.subkey, 28);
				CamelliaEngine.decroldq(15, array, 0, this.ke, 4);
				CamelliaEngine.decroldq(17, array, 0, this.subkey, 12);
				CamelliaEngine.decroldqo32(34, array, 0, this.subkey, 0);
				CamelliaEngine.decroldq(15, array, 4, this.subkey, 40);
				CamelliaEngine.decroldq(15, array, 4, this.ke, 8);
				CamelliaEngine.decroldq(30, array, 4, this.subkey, 20);
				CamelliaEngine.decroldqo32(34, array, 4, this.subkey, 8);
				CamelliaEngine.decroldq(15, array2, 0, this.subkey, 36);
				CamelliaEngine.decroldq(30, array2, 0, this.subkey, 24);
				this.ke[2] = array2[1];
				this.ke[3] = array2[2];
				this.ke[0] = array2[3];
				this.ke[1] = array2[0];
				CamelliaEngine.decroldqo32(49, array2, 0, this.subkey, 4);
				this.subkey[46] = array3[0];
				this.subkey[47] = array3[1];
				this.subkey[44] = array3[2];
				this.subkey[45] = array3[3];
				CamelliaEngine.decroldq(30, array3, 0, this.subkey, 32);
				CamelliaEngine.decroldq(30, array3, 0, this.subkey, 16);
				CamelliaEngine.roldqo32(51, array3, 0, this.kw, 0);
				return;
			}
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x000D2660 File Offset: 0x000D0860
		private int processBlock128(byte[] input, int inOff, byte[] output, int outOff)
		{
			for (int i = 0; i < 4; i++)
			{
				this.state[i] = CamelliaEngine.bytes2uint(input, inOff + i * 4);
				this.state[i] ^= this.kw[i];
			}
			CamelliaEngine.camelliaF2(this.state, this.subkey, 0);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 4);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 8);
			CamelliaEngine.camelliaFLs(this.state, this.ke, 0);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 12);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 16);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 20);
			CamelliaEngine.camelliaFLs(this.state, this.ke, 4);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 24);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 28);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 32);
			this.state[2] ^= this.kw[4];
			this.state[3] ^= this.kw[5];
			this.state[0] ^= this.kw[6];
			this.state[1] ^= this.kw[7];
			CamelliaEngine.uint2bytes(this.state[2], output, outOff);
			CamelliaEngine.uint2bytes(this.state[3], output, outOff + 4);
			CamelliaEngine.uint2bytes(this.state[0], output, outOff + 8);
			CamelliaEngine.uint2bytes(this.state[1], output, outOff + 12);
			return 16;
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x000D281C File Offset: 0x000D0A1C
		private int processBlock192or256(byte[] input, int inOff, byte[] output, int outOff)
		{
			for (int i = 0; i < 4; i++)
			{
				this.state[i] = CamelliaEngine.bytes2uint(input, inOff + i * 4);
				this.state[i] ^= this.kw[i];
			}
			CamelliaEngine.camelliaF2(this.state, this.subkey, 0);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 4);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 8);
			CamelliaEngine.camelliaFLs(this.state, this.ke, 0);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 12);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 16);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 20);
			CamelliaEngine.camelliaFLs(this.state, this.ke, 4);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 24);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 28);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 32);
			CamelliaEngine.camelliaFLs(this.state, this.ke, 8);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 36);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 40);
			CamelliaEngine.camelliaF2(this.state, this.subkey, 44);
			this.state[2] ^= this.kw[4];
			this.state[3] ^= this.kw[5];
			this.state[0] ^= this.kw[6];
			this.state[1] ^= this.kw[7];
			CamelliaEngine.uint2bytes(this.state[2], output, outOff);
			CamelliaEngine.uint2bytes(this.state[3], output, outOff + 4);
			CamelliaEngine.uint2bytes(this.state[0], output, outOff + 8);
			CamelliaEngine.uint2bytes(this.state[1], output, outOff + 12);
			return 16;
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000D2A5A File Offset: 0x000D0C5A
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("only simple KeyParameter expected.");
			}
			this.setKey(forEncryption, ((KeyParameter)parameters).GetKey());
			this.initialised = true;
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x000D2A88 File Offset: 0x000D0C88
		public virtual string AlgorithmName
		{
			get
			{
				return "Camellia";
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06002A00 RID: 10752 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000C8990 File Offset: 0x000C6B90
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000D2A90 File Offset: 0x000D0C90
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.initialised)
			{
				throw new InvalidOperationException("Camellia engine not initialised");
			}
			Check.DataLength(input, inOff, 16, "input buffer too short");
			Check.OutputLength(output, outOff, 16, "output buffer too short");
			if (this._keyIs128)
			{
				return this.processBlock128(input, inOff, output, outOff);
			}
			return this.processBlock192or256(input, inOff, output, outOff);
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x04001BAE RID: 7086
		private bool initialised;

		// Token: 0x04001BAF RID: 7087
		private bool _keyIs128;

		// Token: 0x04001BB0 RID: 7088
		private const int BLOCK_SIZE = 16;

		// Token: 0x04001BB1 RID: 7089
		private uint[] subkey = new uint[96];

		// Token: 0x04001BB2 RID: 7090
		private uint[] kw = new uint[8];

		// Token: 0x04001BB3 RID: 7091
		private uint[] ke = new uint[12];

		// Token: 0x04001BB4 RID: 7092
		private uint[] state = new uint[4];

		// Token: 0x04001BB5 RID: 7093
		private static readonly uint[] SIGMA = new uint[]
		{
			2694735487U,
			1003262091U,
			3061508184U,
			1286239154U,
			3337565999U,
			3914302142U,
			1426019237U,
			4057165596U,
			283453434U,
			3731369245U,
			2958461122U,
			3018244605U
		};

		// Token: 0x04001BB6 RID: 7094
		private static readonly uint[] SBOX1_1110 = new uint[]
		{
			1886416896U,
			2189591040U,
			741092352U,
			3974949888U,
			3014898432U,
			656877312U,
			3233857536U,
			3857048832U,
			3840205824U,
			2240120064U,
			1465341696U,
			892679424U,
			3941263872U,
			202116096U,
			2930683392U,
			1094795520U,
			589505280U,
			4025478912U,
			1802201856U,
			2475922176U,
			1162167552U,
			421075200U,
			2779096320U,
			555819264U,
			3991792896U,
			235802112U,
			1330597632U,
			1313754624U,
			488447232U,
			1701143808U,
			2459079168U,
			3183328512U,
			2256963072U,
			3099113472U,
			2947526400U,
			2408550144U,
			2088532992U,
			3958106880U,
			522133248U,
			3469659648U,
			1044266496U,
			808464384U,
			3705461760U,
			1600085760U,
			1583242752U,
			3318072576U,
			185273088U,
			437918208U,
			2795939328U,
			3789676800U,
			960051456U,
			3402287616U,
			3587560704U,
			1195853568U,
			1566399744U,
			1027423488U,
			3654932736U,
			16843008U,
			1515870720U,
			3604403712U,
			1364283648U,
			1448498688U,
			1819044864U,
			1296911616U,
			2341178112U,
			218959104U,
			2593823232U,
			1717986816U,
			4227595008U,
			3435973632U,
			2964369408U,
			757935360U,
			1953788928U,
			303174144U,
			724249344U,
			538976256U,
			4042321920U,
			2981212416U,
			2223277056U,
			2576980224U,
			3755990784U,
			1280068608U,
			3419130624U,
			3267543552U,
			875836416U,
			2122219008U,
			1987474944U,
			84215040U,
			1835887872U,
			3082270464U,
			2846468352U,
			825307392U,
			3520188672U,
			387389184U,
			67372032U,
			3621246720U,
			336860160U,
			1482184704U,
			976894464U,
			1633771776U,
			3739147776U,
			454761216U,
			286331136U,
			471604224U,
			842150400U,
			252645120U,
			2627509248U,
			370546176U,
			1397969664U,
			404232192U,
			4076007936U,
			572662272U,
			4278124032U,
			1145324544U,
			3486502656U,
			2998055424U,
			3284386560U,
			3048584448U,
			2054846976U,
			2442236160U,
			606348288U,
			134744064U,
			3907577856U,
			2829625344U,
			1616928768U,
			4244438016U,
			1768515840U,
			1347440640U,
			2863311360U,
			3503345664U,
			2694881280U,
			2105376000U,
			2711724288U,
			2307492096U,
			1650614784U,
			2543294208U,
			1414812672U,
			1532713728U,
			505290240U,
			2509608192U,
			3772833792U,
			4294967040U,
			1684300800U,
			3537031680U,
			269488128U,
			3301229568U,
			0U,
			1212696576U,
			2745410304U,
			4160222976U,
			1970631936U,
			3688618752U,
			2324335104U,
			50529024U,
			3873891840U,
			3671775744U,
			151587072U,
			1061109504U,
			3722304768U,
			2492765184U,
			2273806080U,
			1549556736U,
			2206434048U,
			33686016U,
			3452816640U,
			1246382592U,
			2425393152U,
			858993408U,
			1936945920U,
			1734829824U,
			4143379968U,
			4092850944U,
			2644352256U,
			2139062016U,
			3217014528U,
			3806519808U,
			1381126656U,
			2610666240U,
			3638089728U,
			640034304U,
			3368601600U,
			926365440U,
			3334915584U,
			993737472U,
			2172748032U,
			2526451200U,
			1869573888U,
			1263225600U,
			320017152U,
			3200171520U,
			1667457792U,
			774778368U,
			3924420864U,
			2038003968U,
			2812782336U,
			2358021120U,
			2678038272U,
			1852730880U,
			3166485504U,
			2391707136U,
			690563328U,
			4126536960U,
			4193908992U,
			3065427456U,
			791621376U,
			4261281024U,
			3031741440U,
			1499027712U,
			2021160960U,
			2560137216U,
			101058048U,
			1785358848U,
			3890734848U,
			1179010560U,
			1903259904U,
			3132799488U,
			3570717696U,
			623191296U,
			2880154368U,
			1111638528U,
			2290649088U,
			2728567296U,
			2374864128U,
			4210752000U,
			1920102912U,
			117901056U,
			3115956480U,
			1431655680U,
			4177065984U,
			4008635904U,
			2896997376U,
			168430080U,
			909522432U,
			1229539584U,
			707406336U,
			1751672832U,
			1010580480U,
			943208448U,
			4059164928U,
			2762253312U,
			1077952512U,
			673720320U,
			3553874688U,
			2071689984U,
			3149642496U,
			3385444608U,
			1128481536U,
			3250700544U,
			353703168U,
			3823362816U,
			2913840384U,
			4109693952U,
			2004317952U,
			3351758592U,
			2155905024U,
			2661195264U
		};

		// Token: 0x04001BB7 RID: 7095
		private static readonly uint[] SBOX4_4404 = new uint[]
		{
			1886388336U,
			741081132U,
			3014852787U,
			3233808576U,
			3840147684U,
			1465319511U,
			3941204202U,
			2930639022U,
			589496355U,
			1802174571U,
			1162149957U,
			2779054245U,
			3991732461U,
			1330577487U,
			488439837U,
			2459041938U,
			2256928902U,
			2947481775U,
			2088501372U,
			522125343U,
			1044250686U,
			3705405660U,
			1583218782U,
			185270283U,
			2795896998U,
			960036921U,
			3587506389U,
			1566376029U,
			3654877401U,
			1515847770U,
			1364262993U,
			1819017324U,
			2341142667U,
			2593783962U,
			4227531003U,
			2964324528U,
			1953759348U,
			724238379U,
			4042260720U,
			2223243396U,
			3755933919U,
			3419078859U,
			875823156U,
			1987444854U,
			1835860077U,
			2846425257U,
			3520135377U,
			67371012U,
			336855060U,
			976879674U,
			3739091166U,
			286326801U,
			842137650U,
			2627469468U,
			1397948499U,
			4075946226U,
			4278059262U,
			3486449871U,
			3284336835U,
			2054815866U,
			606339108U,
			3907518696U,
			1616904288U,
			1768489065U,
			2863268010U,
			2694840480U,
			2711683233U,
			1650589794U,
			1414791252U,
			505282590U,
			3772776672U,
			1684275300U,
			269484048U,
			0U,
			2745368739U,
			1970602101U,
			2324299914U,
			3873833190U,
			151584777U,
			3722248413U,
			2273771655U,
			2206400643U,
			3452764365U,
			2425356432U,
			1936916595U,
			4143317238U,
			2644312221U,
			3216965823U,
			1381105746U,
			3638034648U,
			3368550600U,
			3334865094U,
			2172715137U,
			1869545583U,
			320012307U,
			1667432547U,
			3924361449U,
			2812739751U,
			2677997727U,
			3166437564U,
			690552873U,
			4193845497U,
			791609391U,
			3031695540U,
			2021130360U,
			101056518U,
			3890675943U,
			1903231089U,
			3570663636U,
			2880110763U,
			2290614408U,
			2374828173U,
			1920073842U,
			3115909305U,
			4177002744U,
			2896953516U,
			909508662U,
			707395626U,
			1010565180U,
			4059103473U,
			1077936192U,
			3553820883U,
			3149594811U,
			1128464451U,
			353697813U,
			2913796269U,
			2004287607U,
			2155872384U,
			2189557890U,
			3974889708U,
			656867367U,
			3856990437U,
			2240086149U,
			892665909U,
			202113036U,
			1094778945U,
			4025417967U,
			2475884691U,
			421068825U,
			555810849U,
			235798542U,
			1313734734U,
			1701118053U,
			3183280317U,
			3099066552U,
			2408513679U,
			3958046955U,
			3469607118U,
			808452144U,
			1600061535U,
			3318022341U,
			437911578U,
			3789619425U,
			3402236106U,
			1195835463U,
			1027407933U,
			16842753U,
			3604349142U,
			1448476758U,
			1296891981U,
			218955789U,
			1717960806U,
			3435921612U,
			757923885U,
			303169554U,
			538968096U,
			2981167281U,
			2576941209U,
			1280049228U,
			3267494082U,
			2122186878U,
			84213765U,
			3082223799U,
			825294897U,
			387383319U,
			3621191895U,
			1482162264U,
			1633747041U,
			454754331U,
			471597084U,
			252641295U,
			370540566U,
			404226072U,
			572653602U,
			1145307204U,
			2998010034U,
			3048538293U,
			2442199185U,
			134742024U,
			2829582504U,
			4244373756U,
			1347420240U,
			3503292624U,
			2105344125U,
			2307457161U,
			2543255703U,
			1532690523U,
			2509570197U,
			4294902015U,
			3536978130U,
			3301179588U,
			1212678216U,
			4160159991U,
			3688562907U,
			50528259U,
			3671720154U,
			1061093439U,
			2492727444U,
			1549533276U,
			33685506U,
			1246363722U,
			858980403U,
			1734803559U,
			4092788979U,
			2139029631U,
			3806462178U,
			2610626715U,
			640024614U,
			926351415U,
			993722427U,
			2526412950U,
			1263206475U,
			3200123070U,
			774766638U,
			2037973113U,
			2357985420U,
			1852702830U,
			2391670926U,
			4126474485U,
			3065381046U,
			4261216509U,
			1499005017U,
			2560098456U,
			1785331818U,
			1178992710U,
			3132752058U,
			623181861U,
			1111621698U,
			2728525986U,
			4210688250U,
			117899271U,
			1431634005U,
			4008575214U,
			168427530U,
			1229520969U,
			1751646312U,
			943194168U,
			2762211492U,
			673710120U,
			2071658619U,
			3385393353U,
			3250651329U,
			3823304931U,
			4109631732U,
			3351707847U,
			2661154974U
		};

		// Token: 0x04001BB8 RID: 7096
		private static readonly uint[] SBOX2_0222 = new uint[]
		{
			14737632U,
			328965U,
			5789784U,
			14277081U,
			6776679U,
			5131854U,
			8487297U,
			13355979U,
			13224393U,
			723723U,
			11447982U,
			6974058U,
			14013909U,
			1579032U,
			6118749U,
			8553090U,
			4605510U,
			14671839U,
			14079702U,
			2565927U,
			9079434U,
			3289650U,
			4934475U,
			4342338U,
			14408667U,
			1842204U,
			10395294U,
			10263708U,
			3815994U,
			13290186U,
			2434341U,
			8092539U,
			855309U,
			7434609U,
			6250335U,
			2039583U,
			16316664U,
			14145495U,
			4079166U,
			10329501U,
			8158332U,
			6316128U,
			12171705U,
			12500670U,
			12369084U,
			9145227U,
			1447446U,
			3421236U,
			5066061U,
			12829635U,
			7500402U,
			9803157U,
			11250603U,
			9342606U,
			12237498U,
			8026746U,
			11776947U,
			131586U,
			11842740U,
			11382189U,
			10658466U,
			11316396U,
			14211288U,
			10132122U,
			1513239U,
			1710618U,
			3487029U,
			13421772U,
			16250871U,
			10066329U,
			6381921U,
			5921370U,
			15263976U,
			2368548U,
			5658198U,
			4210752U,
			14803425U,
			6513507U,
			592137U,
			3355443U,
			12566463U,
			10000536U,
			9934743U,
			8750469U,
			6842472U,
			16579836U,
			15527148U,
			657930U,
			14342874U,
			7303023U,
			5460819U,
			6447714U,
			10724259U,
			3026478U,
			526344U,
			11513775U,
			2631720U,
			11579568U,
			7631988U,
			12763842U,
			12434877U,
			3552822U,
			2236962U,
			3684408U,
			6579300U,
			1973790U,
			3750201U,
			2894892U,
			10921638U,
			3158064U,
			15066597U,
			4473924U,
			16645629U,
			8947848U,
			10461087U,
			6645093U,
			8882055U,
			7039851U,
			16053492U,
			2302755U,
			4737096U,
			1052688U,
			13750737U,
			5329233U,
			12632256U,
			16382457U,
			13816530U,
			10526880U,
			5592405U,
			10592673U,
			4276545U,
			16448250U,
			4408131U,
			1250067U,
			12895428U,
			3092271U,
			11053224U,
			11974326U,
			3947580U,
			2829099U,
			12698049U,
			16777215U,
			13158600U,
			10855845U,
			2105376U,
			9013641U,
			0U,
			9474192U,
			4671303U,
			15724527U,
			15395562U,
			12040119U,
			1381653U,
			394758U,
			13487565U,
			11908533U,
			1184274U,
			8289918U,
			12303291U,
			2697513U,
			986895U,
			12105912U,
			460551U,
			263172U,
			10197915U,
			9737364U,
			2171169U,
			6710886U,
			15132390U,
			13553358U,
			15592941U,
			15198183U,
			3881787U,
			16711422U,
			8355711U,
			12961221U,
			10790052U,
			3618615U,
			11645361U,
			5000268U,
			9539985U,
			7237230U,
			9276813U,
			7763574U,
			197379U,
			2960685U,
			14606046U,
			9868950U,
			2500134U,
			8224125U,
			13027014U,
			6052956U,
			13882323U,
			15921906U,
			5197647U,
			1644825U,
			4144959U,
			14474460U,
			7960953U,
			1907997U,
			5395026U,
			15461355U,
			15987699U,
			7171437U,
			6184542U,
			16514043U,
			6908265U,
			11711154U,
			15790320U,
			3223857U,
			789516U,
			13948116U,
			13619151U,
			9211020U,
			14869218U,
			7697781U,
			11119017U,
			4868682U,
			5723991U,
			8684676U,
			1118481U,
			4539717U,
			1776411U,
			16119285U,
			15000804U,
			921102U,
			7566195U,
			11184810U,
			15856113U,
			14540253U,
			5855577U,
			1315860U,
			7105644U,
			9605778U,
			5526612U,
			13684944U,
			7895160U,
			7368816U,
			14935011U,
			4802889U,
			8421504U,
			5263440U,
			10987431U,
			16185078U,
			7829367U,
			9671571U,
			8816262U,
			8618883U,
			2763306U,
			13092807U,
			5987163U,
			15329769U,
			15658734U,
			9408399U,
			65793U,
			4013373U
		};

		// Token: 0x04001BB9 RID: 7097
		private static readonly uint[] SBOX3_3033 = new uint[]
		{
			939538488U,
			1090535745U,
			369104406U,
			1979741814U,
			3640711641U,
			2466288531U,
			1610637408U,
			4060148466U,
			1912631922U,
			3254829762U,
			2868947883U,
			2583730842U,
			1962964341U,
			100664838U,
			1459640151U,
			2684395680U,
			2432733585U,
			4144035831U,
			3036722613U,
			3372272073U,
			2717950626U,
			2348846220U,
			3523269330U,
			2415956112U,
			4127258358U,
			117442311U,
			2801837991U,
			654321447U,
			2382401166U,
			2986390194U,
			1224755529U,
			3724599006U,
			1124090691U,
			1543527516U,
			3607156695U,
			3338717127U,
			1040203326U,
			4110480885U,
			2399178639U,
			1728079719U,
			520101663U,
			402659352U,
			1845522030U,
			2936057775U,
			788541231U,
			3791708898U,
			2231403909U,
			218107149U,
			1392530259U,
			4026593520U,
			2617285788U,
			1694524773U,
			3925928682U,
			2734728099U,
			2919280302U,
			2650840734U,
			3959483628U,
			2147516544U,
			754986285U,
			1795189611U,
			2818615464U,
			721431339U,
			905983542U,
			2785060518U,
			3305162181U,
			2248181382U,
			1291865421U,
			855651123U,
			4244700669U,
			1711302246U,
			1476417624U,
			2516620950U,
			973093434U,
			150997257U,
			2499843477U,
			268439568U,
			2013296760U,
			3623934168U,
			1107313218U,
			3422604492U,
			4009816047U,
			637543974U,
			3842041317U,
			1627414881U,
			436214298U,
			1056980799U,
			989870907U,
			2181071490U,
			3053500086U,
			3674266587U,
			3556824276U,
			2550175896U,
			3892373736U,
			2332068747U,
			33554946U,
			3942706155U,
			167774730U,
			738208812U,
			486546717U,
			2952835248U,
			1862299503U,
			2365623693U,
			2281736328U,
			234884622U,
			419436825U,
			2264958855U,
			1308642894U,
			184552203U,
			2835392937U,
			201329676U,
			2030074233U,
			285217041U,
			2130739071U,
			570434082U,
			3875596263U,
			1493195097U,
			3774931425U,
			3657489114U,
			1023425853U,
			3355494600U,
			301994514U,
			67109892U,
			1946186868U,
			1409307732U,
			805318704U,
			2113961598U,
			3019945140U,
			671098920U,
			1426085205U,
			1744857192U,
			1342197840U,
			3187719870U,
			3489714384U,
			3288384708U,
			822096177U,
			3405827019U,
			704653866U,
			2902502829U,
			251662095U,
			3389049546U,
			1879076976U,
			4278255615U,
			838873650U,
			1761634665U,
			134219784U,
			1644192354U,
			0U,
			603989028U,
			3506491857U,
			4211145723U,
			3120609978U,
			3976261101U,
			1157645637U,
			2164294017U,
			1929409395U,
			1828744557U,
			2214626436U,
			2667618207U,
			3993038574U,
			1241533002U,
			3271607235U,
			771763758U,
			3238052289U,
			16777473U,
			3858818790U,
			620766501U,
			1207978056U,
			2566953369U,
			3103832505U,
			3003167667U,
			2063629179U,
			4177590777U,
			3456159438U,
			3204497343U,
			3741376479U,
			1895854449U,
			687876393U,
			3439381965U,
			1811967084U,
			318771987U,
			1677747300U,
			2600508315U,
			1660969827U,
			2634063261U,
			3221274816U,
			1258310475U,
			3070277559U,
			2768283045U,
			2298513801U,
			1593859935U,
			2969612721U,
			385881879U,
			4093703412U,
			3154164924U,
			3540046803U,
			1174423110U,
			3472936911U,
			922761015U,
			1577082462U,
			1191200583U,
			2483066004U,
			4194368250U,
			4227923196U,
			1526750043U,
			2533398423U,
			4261478142U,
			1509972570U,
			2885725356U,
			1006648380U,
			1275087948U,
			50332419U,
			889206069U,
			4076925939U,
			587211555U,
			3087055032U,
			1560304989U,
			1778412138U,
			2449511058U,
			3573601749U,
			553656609U,
			1140868164U,
			1358975313U,
			3321939654U,
			2097184125U,
			956315961U,
			2197848963U,
			3691044060U,
			2852170410U,
			2080406652U,
			1996519287U,
			1442862678U,
			83887365U,
			452991771U,
			2751505572U,
			352326933U,
			872428596U,
			503324190U,
			469769244U,
			4160813304U,
			1375752786U,
			536879136U,
			335549460U,
			3909151209U,
			3170942397U,
			3707821533U,
			3825263844U,
			2701173153U,
			3758153952U,
			2315291274U,
			4043370993U,
			3590379222U,
			2046851706U,
			3137387451U,
			3808486371U,
			1073758272U,
			1325420367U
		};
	}
}

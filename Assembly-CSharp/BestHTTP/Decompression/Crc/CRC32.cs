using System;
using System.IO;

namespace BestHTTP.Decompression.Crc
{
	// Token: 0x02000610 RID: 1552
	internal class CRC32
	{
		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060038C8 RID: 14536 RVA: 0x0011AF16 File Offset: 0x00119116
		public long TotalBytesRead
		{
			get
			{
				return this._TotalBytesRead;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060038C9 RID: 14537 RVA: 0x0011AF1E File Offset: 0x0011911E
		public int Crc32Result
		{
			get
			{
				return (int)(~(int)this._register);
			}
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x0011AF27 File Offset: 0x00119127
		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0011AF34 File Offset: 0x00119134
		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int count = 8192;
			this._TotalBytesRead = 0L;
			int i = input.Read(array, 0, count);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			this._TotalBytesRead += (long)i;
			while (i > 0)
			{
				this.SlurpBlock(array, 0, i);
				i = input.Read(array, 0, count);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				this._TotalBytesRead += (long)i;
			}
			return (int)(~(int)this._register);
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x0011AFC8 File Offset: 0x001191C8
		public int ComputeCrc32(int W, byte B)
		{
			return this._InternalComputeCrc32((uint)W, B);
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x0011AFD2 File Offset: 0x001191D2
		internal int _InternalComputeCrc32(uint W, byte B)
		{
			return (int)(this.crc32Table[(int)((W ^ (uint)B) & 255U)] ^ W >> 8);
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0011AFE8 File Offset: 0x001191E8
		public void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (this.reverseBits)
				{
					uint num2 = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)num2]);
				}
				else
				{
					uint num3 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)num3]);
				}
			}
			this._TotalBytesRead += (long)count;
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x0011B07C File Offset: 0x0011927C
		public void UpdateCRC(byte b)
		{
			if (this.reverseBits)
			{
				uint num = this._register >> 24 ^ (uint)b;
				this._register = (this._register << 8 ^ this.crc32Table[(int)num]);
				return;
			}
			uint num2 = (this._register & 255U) ^ (uint)b;
			this._register = (this._register >> 8 ^ this.crc32Table[(int)num2]);
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x0011B0DC File Offset: 0x001192DC
		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (this.reverseBits)
				{
					uint num = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((num >= 0U) ? num : (num + 256U))]);
				}
				else
				{
					uint num2 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((num2 >= 0U) ? num2 : (num2 + 256U))]);
				}
			}
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x0011B164 File Offset: 0x00119364
		private static uint ReverseBits(uint data)
		{
			uint num = (data & 1431655765U) << 1 | (data >> 1 & 1431655765U);
			num = ((num & 858993459U) << 2 | (num >> 2 & 858993459U));
			num = ((num & 252645135U) << 4 | (num >> 4 & 252645135U));
			return num << 24 | (num & 65280U) << 8 | (num >> 8 & 65280U) | num >> 24;
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x0011B1D0 File Offset: 0x001193D0
		private static byte ReverseBits(byte data)
		{
			int num = (int)data * 131586;
			uint num2 = 17055760U;
			uint num3 = (uint)(num & (int)num2);
			uint num4 = (uint)(num << 2 & (int)((int)num2 << 1));
			return (byte)(16781313U * (num3 + num4) >> 24);
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x0011B204 File Offset: 0x00119404
		private void GenerateLookupTable()
		{
			this.crc32Table = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1U) == 1U)
					{
						num = (num >> 1 ^ this.dwPolynomial);
					}
					else
					{
						num >>= 1;
					}
				}
				if (this.reverseBits)
				{
					this.crc32Table[(int)CRC32.ReverseBits(b)] = CRC32.ReverseBits(num);
				}
				else
				{
					this.crc32Table[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0011B278 File Offset: 0x00119478
		private uint gf2_matrix_times(uint[] matrix, uint vec)
		{
			uint num = 0U;
			int num2 = 0;
			while (vec != 0U)
			{
				if ((vec & 1U) == 1U)
				{
					num ^= matrix[num2];
				}
				vec >>= 1;
				num2++;
			}
			return num;
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x0011B2A4 File Offset: 0x001194A4
		private void gf2_matrix_square(uint[] square, uint[] mat)
		{
			for (int i = 0; i < 32; i++)
			{
				square[i] = this.gf2_matrix_times(mat, mat[i]);
			}
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x0011B2CC File Offset: 0x001194CC
		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length == 0)
			{
				return;
			}
			uint num = ~this._register;
			array2[0] = this.dwPolynomial;
			uint num2 = 1U;
			for (int i = 1; i < 32; i++)
			{
				array2[i] = num2;
				num2 <<= 1;
			}
			this.gf2_matrix_square(array, array2);
			this.gf2_matrix_square(array2, array);
			uint num3 = (uint)length;
			do
			{
				this.gf2_matrix_square(array, array2);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array, num);
				}
				num3 >>= 1;
				if (num3 == 0U)
				{
					break;
				}
				this.gf2_matrix_square(array2, array);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array2, num);
				}
				num3 >>= 1;
			}
			while (num3 != 0U);
			num ^= (uint)crc;
			this._register = ~num;
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x0011B383 File Offset: 0x00119583
		public CRC32() : this(false)
		{
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x0011B38C File Offset: 0x0011958C
		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x0011B39A File Offset: 0x0011959A
		public CRC32(int polynomial, bool reverseBits)
		{
			this.reverseBits = reverseBits;
			this.dwPolynomial = (uint)polynomial;
			this.GenerateLookupTable();
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x0011B3BD File Offset: 0x001195BD
		public void Reset()
		{
			this._register = uint.MaxValue;
		}

		// Token: 0x040024F0 RID: 9456
		private uint dwPolynomial;

		// Token: 0x040024F1 RID: 9457
		private long _TotalBytesRead;

		// Token: 0x040024F2 RID: 9458
		private bool reverseBits;

		// Token: 0x040024F3 RID: 9459
		private uint[] crc32Table;

		// Token: 0x040024F4 RID: 9460
		private const int BUFFER_SIZE = 8192;

		// Token: 0x040024F5 RID: 9461
		private uint _register = uint.MaxValue;
	}
}

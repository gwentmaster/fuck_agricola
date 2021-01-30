using System;

namespace Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000291 RID: 657
	internal sealed class Inflate
	{
		// Token: 0x060015CE RID: 5582 RVA: 0x0007EA30 File Offset: 0x0007CC30
		internal int inflateReset(ZStream z)
		{
			if (z == null || z.istate == null)
			{
				return -2;
			}
			z.total_in = (z.total_out = 0L);
			z.msg = null;
			z.istate.mode = ((z.istate.nowrap != 0) ? 7 : 0);
			z.istate.blocks.reset(z, null);
			return 0;
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0007EA92 File Offset: 0x0007CC92
		internal int inflateEnd(ZStream z)
		{
			if (this.blocks != null)
			{
				this.blocks.free(z);
			}
			this.blocks = null;
			return 0;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0007EAB0 File Offset: 0x0007CCB0
		internal int inflateInit(ZStream z, int w)
		{
			z.msg = null;
			this.blocks = null;
			this.nowrap = 0;
			if (w < 0)
			{
				w = -w;
				this.nowrap = 1;
			}
			if (w < 8 || w > 15)
			{
				this.inflateEnd(z);
				return -2;
			}
			this.wbits = w;
			z.istate.blocks = new InfBlocks(z, (z.istate.nowrap != 0) ? null : this, 1 << w);
			this.inflateReset(z);
			return 0;
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0007EB30 File Offset: 0x0007CD30
		internal int inflate(ZStream z, int f)
		{
			if (z == null || z.istate == null || z.next_in == null)
			{
				return -2;
			}
			f = ((f == 4) ? -5 : 0);
			int num = -5;
			int next_in_index;
			for (;;)
			{
				switch (z.istate.mode)
				{
				case 0:
				{
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in += 1L;
					Inflate istate = z.istate;
					byte[] next_in = z.next_in;
					next_in_index = z.next_in_index;
					z.next_in_index = next_in_index + 1;
					if (((istate.method = next_in[next_in_index]) & 15) != 8)
					{
						z.istate.mode = 13;
						z.msg = "unknown compression method";
						z.istate.marker = 5;
						continue;
					}
					if ((z.istate.method >> 4) + 8 > z.istate.wbits)
					{
						z.istate.mode = 13;
						z.msg = "invalid window size";
						z.istate.marker = 5;
						continue;
					}
					z.istate.mode = 1;
					goto IL_142;
				}
				case 1:
					goto IL_142;
				case 2:
					goto IL_1EA;
				case 3:
					goto IL_253;
				case 4:
					goto IL_2C3;
				case 5:
					goto IL_332;
				case 6:
					goto IL_3AC;
				case 7:
					num = z.istate.blocks.proc(z, num);
					if (num == -3)
					{
						z.istate.mode = 13;
						z.istate.marker = 0;
						continue;
					}
					if (num == 0)
					{
						num = f;
					}
					if (num != 1)
					{
						return num;
					}
					num = f;
					z.istate.blocks.reset(z, z.istate.was);
					if (z.istate.nowrap != 0)
					{
						z.istate.mode = 12;
						continue;
					}
					z.istate.mode = 8;
					goto IL_45D;
				case 8:
					goto IL_45D;
				case 9:
					goto IL_4C7;
				case 10:
					goto IL_538;
				case 11:
					goto IL_5A8;
				case 12:
					return 1;
				case 13:
					return -3;
				}
				break;
				IL_142:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				byte[] next_in2 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				int num2 = next_in2[next_in_index] & 255;
				if (((z.istate.method << 8) + num2) % 31 != 0)
				{
					z.istate.mode = 13;
					z.msg = "incorrect header check";
					z.istate.marker = 5;
					continue;
				}
				if ((num2 & 32) == 0)
				{
					z.istate.mode = 7;
					continue;
				}
				goto IL_1DE;
				IL_5A8:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate2 = z.istate;
				long num3 = istate2.need;
				byte[] next_in3 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				istate2.need = num3 + (long)(next_in3[next_in_index] & 255UL);
				if ((int)z.istate.was[0] != (int)z.istate.need)
				{
					z.istate.mode = 13;
					z.msg = "incorrect data check";
					z.istate.marker = 5;
					continue;
				}
				goto IL_648;
				IL_538:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate3 = z.istate;
				long num4 = istate3.need;
				byte[] next_in4 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				istate3.need = num4 + ((next_in4[next_in_index] & 255L) << 8 & 65280L);
				z.istate.mode = 11;
				goto IL_5A8;
				IL_4C7:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate4 = z.istate;
				long num5 = istate4.need;
				byte[] next_in5 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				istate4.need = num5 + ((next_in5[next_in_index] & 255L) << 16 & 16711680L);
				z.istate.mode = 10;
				goto IL_538;
				IL_45D:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate5 = z.istate;
				byte[] next_in6 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				istate5.need = ((next_in6[next_in_index] & 255L) << 24 & (long)((ulong)-16777216));
				z.istate.mode = 9;
				goto IL_4C7;
			}
			return -2;
			IL_1DE:
			z.istate.mode = 2;
			IL_1EA:
			if (z.avail_in == 0)
			{
				return num;
			}
			num = f;
			z.avail_in--;
			z.total_in += 1L;
			Inflate istate6 = z.istate;
			byte[] next_in7 = z.next_in;
			next_in_index = z.next_in_index;
			z.next_in_index = next_in_index + 1;
			istate6.need = ((next_in7[next_in_index] & 255L) << 24 & (long)((ulong)-16777216));
			z.istate.mode = 3;
			IL_253:
			if (z.avail_in == 0)
			{
				return num;
			}
			num = f;
			z.avail_in--;
			z.total_in += 1L;
			Inflate istate7 = z.istate;
			long num6 = istate7.need;
			byte[] next_in8 = z.next_in;
			next_in_index = z.next_in_index;
			z.next_in_index = next_in_index + 1;
			istate7.need = num6 + ((next_in8[next_in_index] & 255L) << 16 & 16711680L);
			z.istate.mode = 4;
			IL_2C3:
			if (z.avail_in == 0)
			{
				return num;
			}
			num = f;
			z.avail_in--;
			z.total_in += 1L;
			Inflate istate8 = z.istate;
			long num7 = istate8.need;
			byte[] next_in9 = z.next_in;
			next_in_index = z.next_in_index;
			z.next_in_index = next_in_index + 1;
			istate8.need = num7 + ((next_in9[next_in_index] & 255L) << 8 & 65280L);
			z.istate.mode = 5;
			IL_332:
			if (z.avail_in == 0)
			{
				return num;
			}
			z.avail_in--;
			z.total_in += 1L;
			Inflate istate9 = z.istate;
			long num8 = istate9.need;
			byte[] next_in10 = z.next_in;
			next_in_index = z.next_in_index;
			z.next_in_index = next_in_index + 1;
			istate9.need = num8 + (long)(next_in10[next_in_index] & 255UL);
			z.adler = z.istate.need;
			z.istate.mode = 6;
			return 2;
			IL_3AC:
			z.istate.mode = 13;
			z.msg = "need dictionary";
			z.istate.marker = 0;
			return -2;
			IL_648:
			z.istate.mode = 12;
			return 1;
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0007F19C File Offset: 0x0007D39C
		internal int inflateSetDictionary(ZStream z, byte[] dictionary, int dictLength)
		{
			int start = 0;
			int num = dictLength;
			if (z == null || z.istate == null || z.istate.mode != 6)
			{
				return -2;
			}
			if (z._adler.adler32(1L, dictionary, 0, dictLength) != z.adler)
			{
				return -3;
			}
			z.adler = z._adler.adler32(0L, null, 0, 0);
			if (num >= 1 << z.istate.wbits)
			{
				num = (1 << z.istate.wbits) - 1;
				start = dictLength - num;
			}
			z.istate.blocks.set_dictionary(dictionary, start, num);
			z.istate.mode = 7;
			return 0;
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0007F244 File Offset: 0x0007D444
		internal int inflateSync(ZStream z)
		{
			if (z == null || z.istate == null)
			{
				return -2;
			}
			if (z.istate.mode != 13)
			{
				z.istate.mode = 13;
				z.istate.marker = 0;
			}
			int num;
			if ((num = z.avail_in) == 0)
			{
				return -5;
			}
			int num2 = z.next_in_index;
			int num3 = z.istate.marker;
			while (num != 0 && num3 < 4)
			{
				if (z.next_in[num2] == Inflate.mark[num3])
				{
					num3++;
				}
				else if (z.next_in[num2] != 0)
				{
					num3 = 0;
				}
				else
				{
					num3 = 4 - num3;
				}
				num2++;
				num--;
			}
			z.total_in += (long)(num2 - z.next_in_index);
			z.next_in_index = num2;
			z.avail_in = num;
			z.istate.marker = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long total_in = z.total_in;
			long total_out = z.total_out;
			this.inflateReset(z);
			z.total_in = total_in;
			z.total_out = total_out;
			z.istate.mode = 7;
			return 0;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0007F34B File Offset: 0x0007D54B
		internal int inflateSyncPoint(ZStream z)
		{
			if (z == null || z.istate == null || z.istate.blocks == null)
			{
				return -2;
			}
			return z.istate.blocks.sync_point();
		}

		// Token: 0x0400146F RID: 5231
		private const int MAX_WBITS = 15;

		// Token: 0x04001470 RID: 5232
		private const int PRESET_DICT = 32;

		// Token: 0x04001471 RID: 5233
		internal const int Z_NO_FLUSH = 0;

		// Token: 0x04001472 RID: 5234
		internal const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x04001473 RID: 5235
		internal const int Z_SYNC_FLUSH = 2;

		// Token: 0x04001474 RID: 5236
		internal const int Z_FULL_FLUSH = 3;

		// Token: 0x04001475 RID: 5237
		internal const int Z_FINISH = 4;

		// Token: 0x04001476 RID: 5238
		private const int Z_DEFLATED = 8;

		// Token: 0x04001477 RID: 5239
		private const int Z_OK = 0;

		// Token: 0x04001478 RID: 5240
		private const int Z_STREAM_END = 1;

		// Token: 0x04001479 RID: 5241
		private const int Z_NEED_DICT = 2;

		// Token: 0x0400147A RID: 5242
		private const int Z_ERRNO = -1;

		// Token: 0x0400147B RID: 5243
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x0400147C RID: 5244
		private const int Z_DATA_ERROR = -3;

		// Token: 0x0400147D RID: 5245
		private const int Z_MEM_ERROR = -4;

		// Token: 0x0400147E RID: 5246
		private const int Z_BUF_ERROR = -5;

		// Token: 0x0400147F RID: 5247
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x04001480 RID: 5248
		private const int METHOD = 0;

		// Token: 0x04001481 RID: 5249
		private const int FLAG = 1;

		// Token: 0x04001482 RID: 5250
		private const int DICT4 = 2;

		// Token: 0x04001483 RID: 5251
		private const int DICT3 = 3;

		// Token: 0x04001484 RID: 5252
		private const int DICT2 = 4;

		// Token: 0x04001485 RID: 5253
		private const int DICT1 = 5;

		// Token: 0x04001486 RID: 5254
		private const int DICT0 = 6;

		// Token: 0x04001487 RID: 5255
		private const int BLOCKS = 7;

		// Token: 0x04001488 RID: 5256
		private const int CHECK4 = 8;

		// Token: 0x04001489 RID: 5257
		private const int CHECK3 = 9;

		// Token: 0x0400148A RID: 5258
		private const int CHECK2 = 10;

		// Token: 0x0400148B RID: 5259
		private const int CHECK1 = 11;

		// Token: 0x0400148C RID: 5260
		private const int DONE = 12;

		// Token: 0x0400148D RID: 5261
		private const int BAD = 13;

		// Token: 0x0400148E RID: 5262
		internal int mode;

		// Token: 0x0400148F RID: 5263
		internal int method;

		// Token: 0x04001490 RID: 5264
		internal long[] was = new long[1];

		// Token: 0x04001491 RID: 5265
		internal long need;

		// Token: 0x04001492 RID: 5266
		internal int marker;

		// Token: 0x04001493 RID: 5267
		internal int nowrap;

		// Token: 0x04001494 RID: 5268
		internal int wbits;

		// Token: 0x04001495 RID: 5269
		internal InfBlocks blocks;

		// Token: 0x04001496 RID: 5270
		private static readonly byte[] mark = new byte[]
		{
			0,
			0,
			byte.MaxValue,
			byte.MaxValue
		};
	}
}

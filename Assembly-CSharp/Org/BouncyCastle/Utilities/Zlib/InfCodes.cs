using System;

namespace Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x0200028F RID: 655
	internal sealed class InfCodes
	{
		// Token: 0x060015C1 RID: 5569 RVA: 0x00003425 File Offset: 0x00001625
		internal InfCodes()
		{
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0007D1C8 File Offset: 0x0007B3C8
		internal void init(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, ZStream z)
		{
			this.mode = 0;
			this.lbits = (byte)bl;
			this.dbits = (byte)bd;
			this.ltree = tl;
			this.ltree_index = tl_index;
			this.dtree = td;
			this.dtree_index = td_index;
			this.tree = null;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0007D208 File Offset: 0x0007B408
		internal int proc(InfBlocks s, ZStream z, int r)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = s.bitb;
			int i = s.bitk;
			int num4 = s.write;
			int num5 = (num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4);
			for (;;)
			{
				int num6;
				switch (this.mode)
				{
				case 0:
					if (num5 >= 258 && num2 >= 10)
					{
						s.bitb = num3;
						s.bitk = i;
						z.avail_in = num2;
						z.total_in += (long)(num - z.next_in_index);
						z.next_in_index = num;
						s.write = num4;
						r = this.inflate_fast((int)this.lbits, (int)this.dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, s, z);
						num = z.next_in_index;
						num2 = z.avail_in;
						num3 = s.bitb;
						i = s.bitk;
						num4 = s.write;
						num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
						if (r != 0)
						{
							this.mode = ((r == 1) ? 7 : 9);
							continue;
						}
					}
					this.need = (int)this.lbits;
					this.tree = this.ltree;
					this.tree_index = this.ltree_index;
					this.mode = 1;
					goto IL_199;
				case 1:
					goto IL_199;
				case 2:
					num6 = this.get;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_34E;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.len += (num3 & InfCodes.inflate_mask[num6]);
					num3 >>= num6;
					i -= num6;
					this.need = (int)this.dbits;
					this.tree = this.dtree;
					this.tree_index = this.dtree_index;
					this.mode = 3;
					goto IL_411;
				case 3:
					goto IL_411;
				case 4:
					num6 = this.get;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_595;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.dist += (num3 & InfCodes.inflate_mask[num6]);
					num3 >>= num6;
					i -= num6;
					this.mode = 5;
					goto IL_634;
				case 5:
					goto IL_634;
				case 6:
					if (num5 == 0)
					{
						if (num4 == s.end && s.read != 0)
						{
							num4 = 0;
							num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
						}
						if (num5 == 0)
						{
							s.write = num4;
							r = s.inflate_flush(z, r);
							num4 = s.write;
							num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
							if (num4 == s.end && s.read != 0)
							{
								num4 = 0;
								num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
							}
							if (num5 == 0)
							{
								goto Block_44;
							}
						}
					}
					r = 0;
					s.window[num4++] = (byte)this.lit;
					num5--;
					this.mode = 0;
					continue;
				case 7:
					goto IL_8DA;
				case 8:
					goto IL_989;
				case 9:
					goto IL_9D3;
				}
				break;
				IL_199:
				num6 = this.need;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_1AB;
					}
					r = 0;
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
				int num7 = (this.tree_index + (num3 & InfCodes.inflate_mask[num6])) * 3;
				num3 >>= this.tree[num7 + 1];
				i -= this.tree[num7 + 1];
				int num8 = this.tree[num7];
				if (num8 == 0)
				{
					this.lit = this.tree[num7 + 2];
					this.mode = 6;
					continue;
				}
				if ((num8 & 16) != 0)
				{
					this.get = (num8 & 15);
					this.len = this.tree[num7 + 2];
					this.mode = 2;
					continue;
				}
				if ((num8 & 64) == 0)
				{
					this.need = num8;
					this.tree_index = num7 / 3 + this.tree[num7 + 2];
					continue;
				}
				if ((num8 & 32) != 0)
				{
					this.mode = 7;
					continue;
				}
				goto IL_2DE;
				IL_411:
				num6 = this.need;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_423;
					}
					r = 0;
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
				num7 = (this.tree_index + (num3 & InfCodes.inflate_mask[num6])) * 3;
				num3 >>= this.tree[num7 + 1];
				i -= this.tree[num7 + 1];
				num8 = this.tree[num7];
				if ((num8 & 16) != 0)
				{
					this.get = (num8 & 15);
					this.dist = this.tree[num7 + 2];
					this.mode = 4;
					continue;
				}
				if ((num8 & 64) == 0)
				{
					this.need = num8;
					this.tree_index = num7 / 3 + this.tree[num7 + 2];
					continue;
				}
				goto IL_525;
				IL_634:
				int j;
				for (j = num4 - this.dist; j < 0; j += s.end)
				{
				}
				while (this.len != 0)
				{
					if (num5 == 0)
					{
						if (num4 == s.end && s.read != 0)
						{
							num4 = 0;
							num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
						}
						if (num5 == 0)
						{
							s.write = num4;
							r = s.inflate_flush(z, r);
							num4 = s.write;
							num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
							if (num4 == s.end && s.read != 0)
							{
								num4 = 0;
								num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
							}
							if (num5 == 0)
							{
								goto Block_32;
							}
						}
					}
					s.window[num4++] = s.window[j++];
					num5--;
					if (j == s.end)
					{
						j = 0;
					}
					this.len--;
				}
				this.mode = 0;
			}
			r = -2;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_1AB:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_2DE:
			this.mode = 9;
			z.msg = "invalid literal/length code";
			r = -3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_34E:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_423:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_525:
			this.mode = 9;
			z.msg = "invalid distance code";
			r = -3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_595:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			Block_32:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			Block_44:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_8DA:
			if (i > 7)
			{
				i -= 8;
				num2++;
				num--;
			}
			s.write = num4;
			r = s.inflate_flush(z, r);
			num4 = s.write;
			int num9 = (num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4);
			if (s.read != s.write)
			{
				s.bitb = num3;
				s.bitk = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				s.write = num4;
				return s.inflate_flush(z, r);
			}
			this.mode = 8;
			IL_989:
			r = 1;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_9D3:
			r = -3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x00003022 File Offset: 0x00001222
		internal void free(ZStream z)
		{
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0007DC80 File Offset: 0x0007BE80
		internal int inflate_fast(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, InfBlocks s, ZStream z)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = s.bitb;
			int i = s.bitk;
			int num4 = s.write;
			int num5 = (num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4);
			int num6 = InfCodes.inflate_mask[bl];
			int num7 = InfCodes.inflate_mask[bd];
			int num10;
			int num11;
			for (;;)
			{
				if (i >= 20)
				{
					int num8 = num3 & num6;
					int num9 = (tl_index + num8) * 3;
					if ((num10 = tl[num9]) == 0)
					{
						num3 >>= tl[num9 + 1];
						i -= tl[num9 + 1];
						s.window[num4++] = (byte)tl[num9 + 2];
						num5--;
					}
					else
					{
						for (;;)
						{
							num3 >>= tl[num9 + 1];
							i -= tl[num9 + 1];
							if ((num10 & 16) != 0)
							{
								break;
							}
							if ((num10 & 64) != 0)
							{
								goto IL_4B3;
							}
							num8 += tl[num9 + 2];
							num8 += (num3 & InfCodes.inflate_mask[num10]);
							num9 = (tl_index + num8) * 3;
							if ((num10 = tl[num9]) == 0)
							{
								goto Block_20;
							}
						}
						num10 &= 15;
						num11 = tl[num9 + 2] + (num3 & InfCodes.inflate_mask[num10]);
						num3 >>= num10;
						for (i -= num10; i < 15; i += 8)
						{
							num2--;
							num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						}
						num8 = (num3 & num7);
						num9 = (td_index + num8) * 3;
						num10 = td[num9];
						for (;;)
						{
							num3 >>= td[num9 + 1];
							i -= td[num9 + 1];
							if ((num10 & 16) != 0)
							{
								break;
							}
							if ((num10 & 64) != 0)
							{
								goto IL_3C1;
							}
							num8 += td[num9 + 2];
							num8 += (num3 & InfCodes.inflate_mask[num10]);
							num9 = (td_index + num8) * 3;
							num10 = td[num9];
						}
						num10 &= 15;
						while (i < num10)
						{
							num2--;
							num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
							i += 8;
						}
						int num12 = td[num9 + 2] + (num3 & InfCodes.inflate_mask[num10]);
						num3 >>= num10;
						i -= num10;
						num5 -= num11;
						int num13;
						if (num4 >= num12)
						{
							num13 = num4 - num12;
							if (num4 - num13 > 0 && 2 > num4 - num13)
							{
								s.window[num4++] = s.window[num13++];
								s.window[num4++] = s.window[num13++];
								num11 -= 2;
							}
							else
							{
								Array.Copy(s.window, num13, s.window, num4, 2);
								num4 += 2;
								num13 += 2;
								num11 -= 2;
							}
						}
						else
						{
							num13 = num4 - num12;
							do
							{
								num13 += s.end;
							}
							while (num13 < 0);
							num10 = s.end - num13;
							if (num11 > num10)
							{
								num11 -= num10;
								if (num4 - num13 > 0 && num10 > num4 - num13)
								{
									do
									{
										s.window[num4++] = s.window[num13++];
									}
									while (--num10 != 0);
								}
								else
								{
									Array.Copy(s.window, num13, s.window, num4, num10);
									num4 += num10;
									num13 += num10;
								}
								num13 = 0;
							}
						}
						if (num4 - num13 > 0 && num11 > num4 - num13)
						{
							do
							{
								s.window[num4++] = s.window[num13++];
							}
							while (--num11 != 0);
							goto IL_5C0;
						}
						Array.Copy(s.window, num13, s.window, num4, num11);
						num4 += num11;
						num13 += num11;
						goto IL_5C0;
						Block_20:
						num3 >>= tl[num9 + 1];
						i -= tl[num9 + 1];
						s.window[num4++] = (byte)tl[num9 + 2];
						num5--;
					}
					IL_5C0:
					if (num5 < 258 || num2 < 10)
					{
						goto IL_5D2;
					}
				}
				else
				{
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
			}
			IL_3C1:
			z.msg = "invalid distance code";
			num11 = z.avail_in - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return -3;
			IL_4B3:
			if ((num10 & 32) != 0)
			{
				num11 = z.avail_in - num2;
				num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
				num2 += num11;
				num -= num11;
				i -= num11 << 3;
				s.bitb = num3;
				s.bitk = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				s.write = num4;
				return 1;
			}
			z.msg = "invalid literal/length code";
			num11 = z.avail_in - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return -3;
			IL_5D2:
			num11 = z.avail_in - num2;
			num11 = ((i >> 3 < num11) ? (i >> 3) : num11);
			num2 += num11;
			num -= num11;
			i -= num11 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return 0;
		}

		// Token: 0x04001434 RID: 5172
		private static readonly int[] inflate_mask = new int[]
		{
			0,
			1,
			3,
			7,
			15,
			31,
			63,
			127,
			255,
			511,
			1023,
			2047,
			4095,
			8191,
			16383,
			32767,
			65535
		};

		// Token: 0x04001435 RID: 5173
		private const int Z_OK = 0;

		// Token: 0x04001436 RID: 5174
		private const int Z_STREAM_END = 1;

		// Token: 0x04001437 RID: 5175
		private const int Z_NEED_DICT = 2;

		// Token: 0x04001438 RID: 5176
		private const int Z_ERRNO = -1;

		// Token: 0x04001439 RID: 5177
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x0400143A RID: 5178
		private const int Z_DATA_ERROR = -3;

		// Token: 0x0400143B RID: 5179
		private const int Z_MEM_ERROR = -4;

		// Token: 0x0400143C RID: 5180
		private const int Z_BUF_ERROR = -5;

		// Token: 0x0400143D RID: 5181
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x0400143E RID: 5182
		private const int START = 0;

		// Token: 0x0400143F RID: 5183
		private const int LEN = 1;

		// Token: 0x04001440 RID: 5184
		private const int LENEXT = 2;

		// Token: 0x04001441 RID: 5185
		private const int DIST = 3;

		// Token: 0x04001442 RID: 5186
		private const int DISTEXT = 4;

		// Token: 0x04001443 RID: 5187
		private const int COPY = 5;

		// Token: 0x04001444 RID: 5188
		private const int LIT = 6;

		// Token: 0x04001445 RID: 5189
		private const int WASH = 7;

		// Token: 0x04001446 RID: 5190
		private const int END = 8;

		// Token: 0x04001447 RID: 5191
		private const int BADCODE = 9;

		// Token: 0x04001448 RID: 5192
		private int mode;

		// Token: 0x04001449 RID: 5193
		private int len;

		// Token: 0x0400144A RID: 5194
		private int[] tree;

		// Token: 0x0400144B RID: 5195
		private int tree_index;

		// Token: 0x0400144C RID: 5196
		private int need;

		// Token: 0x0400144D RID: 5197
		private int lit;

		// Token: 0x0400144E RID: 5198
		private int get;

		// Token: 0x0400144F RID: 5199
		private int dist;

		// Token: 0x04001450 RID: 5200
		private byte lbits;

		// Token: 0x04001451 RID: 5201
		private byte dbits;

		// Token: 0x04001452 RID: 5202
		private int[] ltree;

		// Token: 0x04001453 RID: 5203
		private int ltree_index;

		// Token: 0x04001454 RID: 5204
		private int[] dtree;

		// Token: 0x04001455 RID: 5205
		private int dtree_index;
	}
}

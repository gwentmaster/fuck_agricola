using System;

namespace Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x0200028D RID: 653
	public sealed class Deflate
	{
		// Token: 0x06001591 RID: 5521 RVA: 0x00079E9C File Offset: 0x0007809C
		static Deflate()
		{
			Deflate.config_table = new Deflate.Config[10];
			Deflate.config_table[0] = new Deflate.Config(0, 0, 0, 0, 0);
			Deflate.config_table[1] = new Deflate.Config(4, 4, 8, 4, 1);
			Deflate.config_table[2] = new Deflate.Config(4, 5, 16, 8, 1);
			Deflate.config_table[3] = new Deflate.Config(4, 6, 32, 32, 1);
			Deflate.config_table[4] = new Deflate.Config(4, 4, 16, 16, 2);
			Deflate.config_table[5] = new Deflate.Config(8, 16, 32, 32, 2);
			Deflate.config_table[6] = new Deflate.Config(8, 16, 128, 128, 2);
			Deflate.config_table[7] = new Deflate.Config(8, 32, 128, 256, 2);
			Deflate.config_table[8] = new Deflate.Config(32, 128, 258, 1024, 2);
			Deflate.config_table[9] = new Deflate.Config(32, 258, 258, 4096, 2);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00079FF4 File Offset: 0x000781F4
		internal Deflate()
		{
			this.dyn_ltree = new short[1146];
			this.dyn_dtree = new short[122];
			this.bl_tree = new short[78];
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0007A080 File Offset: 0x00078280
		internal void lm_init()
		{
			this.window_size = 2 * this.w_size;
			this.head[this.hash_size - 1] = 0;
			for (int i = 0; i < this.hash_size - 1; i++)
			{
				this.head[i] = 0;
			}
			this.max_lazy_match = Deflate.config_table[this.level].max_lazy;
			this.good_match = Deflate.config_table[this.level].good_length;
			this.nice_match = Deflate.config_table[this.level].nice_length;
			this.max_chain_length = Deflate.config_table[this.level].max_chain;
			this.strstart = 0;
			this.block_start = 0;
			this.lookahead = 0;
			this.match_length = (this.prev_length = 2);
			this.match_available = 0;
			this.ins_h = 0;
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0007A158 File Offset: 0x00078358
		internal void tr_init()
		{
			this.l_desc.dyn_tree = this.dyn_ltree;
			this.l_desc.stat_desc = StaticTree.static_l_desc;
			this.d_desc.dyn_tree = this.dyn_dtree;
			this.d_desc.stat_desc = StaticTree.static_d_desc;
			this.bl_desc.dyn_tree = this.bl_tree;
			this.bl_desc.stat_desc = StaticTree.static_bl_desc;
			this.bi_buf = 0U;
			this.bi_valid = 0;
			this.last_eob_len = 8;
			this.init_block();
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0007A1E4 File Offset: 0x000783E4
		internal void init_block()
		{
			for (int i = 0; i < 286; i++)
			{
				this.dyn_ltree[i * 2] = 0;
			}
			for (int j = 0; j < 30; j++)
			{
				this.dyn_dtree[j * 2] = 0;
			}
			for (int k = 0; k < 19; k++)
			{
				this.bl_tree[k * 2] = 0;
			}
			this.dyn_ltree[512] = 1;
			this.opt_len = (this.static_len = 0);
			this.last_lit = (this.matches = 0);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0007A26C File Offset: 0x0007846C
		internal void pqdownheap(short[] tree, int k)
		{
			int num = this.heap[k];
			for (int i = k << 1; i <= this.heap_len; i <<= 1)
			{
				if (i < this.heap_len && Deflate.smaller(tree, this.heap[i + 1], this.heap[i], this.depth))
				{
					i++;
				}
				if (Deflate.smaller(tree, num, this.heap[i], this.depth))
				{
					break;
				}
				this.heap[k] = this.heap[i];
				k = i;
			}
			this.heap[k] = num;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0007A2F8 File Offset: 0x000784F8
		internal static bool smaller(short[] tree, int n, int m, byte[] depth)
		{
			short num = tree[n * 2];
			short num2 = tree[m * 2];
			return num < num2 || (num == num2 && depth[n] <= depth[m]);
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0007A328 File Offset: 0x00078528
		internal void scan_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(max_code + 1) * 2 + 1] = -1;
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						short[] array = this.bl_tree;
						int num7 = num6 * 2;
						array[num7] += (short)num3;
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							short[] array2 = this.bl_tree;
							int num8 = num6 * 2;
							array2[num8] += 1;
						}
						short[] array3 = this.bl_tree;
						int num9 = 32;
						array3[num9] += 1;
					}
					else if (num3 <= 10)
					{
						short[] array4 = this.bl_tree;
						int num10 = 34;
						array4[num10] += 1;
					}
					else
					{
						short[] array5 = this.bl_tree;
						int num11 = 36;
						array5[num11] += 1;
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0007A430 File Offset: 0x00078630
		internal int build_bl_tree()
		{
			this.scan_tree(this.dyn_ltree, this.l_desc.max_code);
			this.scan_tree(this.dyn_dtree, this.d_desc.max_code);
			this.bl_desc.build_tree(this);
			int num = 18;
			while (num >= 3 && this.bl_tree[(int)(ZTree.bl_order[num] * 2 + 1)] == 0)
			{
				num--;
			}
			this.opt_len += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0007A4B4 File Offset: 0x000786B4
		internal void send_all_trees(int lcodes, int dcodes, int blcodes)
		{
			this.send_bits(lcodes - 257, 5);
			this.send_bits(dcodes - 1, 5);
			this.send_bits(blcodes - 4, 4);
			for (int i = 0; i < blcodes; i++)
			{
				this.send_bits((int)this.bl_tree[(int)(ZTree.bl_order[i] * 2 + 1)], 3);
			}
			this.send_tree(this.dyn_ltree, lcodes - 1);
			this.send_tree(this.dyn_dtree, dcodes - 1);
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0007A528 File Offset: 0x00078728
		internal void send_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						do
						{
							this.send_code(num6, this.bl_tree);
						}
						while (--num3 != 0);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							this.send_code(num6, this.bl_tree);
							num3--;
						}
						this.send_code(16, this.bl_tree);
						this.send_bits(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.send_code(17, this.bl_tree);
						this.send_bits(num3 - 3, 3);
					}
					else
					{
						this.send_code(18, this.bl_tree);
						this.send_bits(num3 - 11, 7);
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0007A635 File Offset: 0x00078835
		internal void put_byte(byte[] p, int start, int len)
		{
			Array.Copy(p, start, this.pending_buf, this.pending, len);
			this.pending += len;
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0007A65C File Offset: 0x0007885C
		internal void put_byte(byte c)
		{
			byte[] array = this.pending_buf;
			int num = this.pending;
			this.pending = num + 1;
			array[num] = c;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0007A684 File Offset: 0x00078884
		internal void put_short(int w)
		{
			byte[] array = this.pending_buf;
			int num = this.pending;
			this.pending = num + 1;
			array[num] = (byte)w;
			byte[] array2 = this.pending_buf;
			num = this.pending;
			this.pending = num + 1;
			array2[num] = (byte)(w >> 8);
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0007A6C8 File Offset: 0x000788C8
		internal void putShortMSB(int b)
		{
			byte[] array = this.pending_buf;
			int num = this.pending;
			this.pending = num + 1;
			array[num] = (byte)(b >> 8);
			byte[] array2 = this.pending_buf;
			num = this.pending;
			this.pending = num + 1;
			array2[num] = (byte)b;
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0007A70C File Offset: 0x0007890C
		internal void send_code(int c, short[] tree)
		{
			int num = c * 2;
			this.send_bits((int)tree[num] & 65535, (int)tree[num + 1] & 65535);
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0007A738 File Offset: 0x00078938
		internal void send_bits(int val, int length)
		{
			if (this.bi_valid > 16 - length)
			{
				this.bi_buf |= (uint)((uint)val << this.bi_valid);
				byte[] array = this.pending_buf;
				int num = this.pending;
				this.pending = num + 1;
				array[num] = (byte)this.bi_buf;
				byte[] array2 = this.pending_buf;
				num = this.pending;
				this.pending = num + 1;
				array2[num] = (byte)(this.bi_buf >> 8);
				this.bi_buf = (uint)val >> 16 - this.bi_valid;
				this.bi_valid += length - 16;
				return;
			}
			this.bi_buf |= (uint)((uint)val << this.bi_valid);
			this.bi_valid += length;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0007A7F8 File Offset: 0x000789F8
		internal void _tr_align()
		{
			this.send_bits(2, 3);
			this.send_code(256, StaticTree.static_ltree);
			this.bi_flush();
			if (1 + this.last_eob_len + 10 - this.bi_valid < 9)
			{
				this.send_bits(2, 3);
				this.send_code(256, StaticTree.static_ltree);
				this.bi_flush();
			}
			this.last_eob_len = 7;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0007A860 File Offset: 0x00078A60
		internal bool _tr_tally(int dist, int lc)
		{
			this.pending_buf[this.d_buf + this.last_lit * 2] = (byte)(dist >> 8);
			this.pending_buf[this.d_buf + this.last_lit * 2 + 1] = (byte)dist;
			this.pending_buf[this.l_buf + this.last_lit] = (byte)lc;
			this.last_lit++;
			if (dist == 0)
			{
				short[] array = this.dyn_ltree;
				int num = lc * 2;
				array[num] += 1;
			}
			else
			{
				this.matches++;
				dist--;
				short[] array2 = this.dyn_ltree;
				int num2 = ((int)ZTree._length_code[lc] + 256 + 1) * 2;
				array2[num2] += 1;
				short[] array3 = this.dyn_dtree;
				int num3 = ZTree.d_code(dist) * 2;
				array3[num3] += 1;
			}
			if ((this.last_lit & 8191) == 0 && this.level > 2)
			{
				int num4 = this.last_lit * 8;
				int num5 = this.strstart - this.block_start;
				for (int i = 0; i < 30; i++)
				{
					num4 += (int)((long)this.dyn_dtree[i * 2] * (5L + (long)ZTree.extra_dbits[i]));
				}
				num4 >>= 3;
				if (this.matches < this.last_lit / 2 && num4 < num5 / 2)
				{
					return true;
				}
			}
			return this.last_lit == this.lit_bufsize - 1;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0007A9B0 File Offset: 0x00078BB0
		internal void compress_block(short[] ltree, short[] dtree)
		{
			int num = 0;
			if (this.last_lit != 0)
			{
				do
				{
					int num2 = ((int)this.pending_buf[this.d_buf + num * 2] << 8 & 65280) | (int)(this.pending_buf[this.d_buf + num * 2 + 1] & byte.MaxValue);
					int num3 = (int)(this.pending_buf[this.l_buf + num] & byte.MaxValue);
					num++;
					if (num2 == 0)
					{
						this.send_code(num3, ltree);
					}
					else
					{
						int num4 = (int)ZTree._length_code[num3];
						this.send_code(num4 + 256 + 1, ltree);
						int num5 = ZTree.extra_lbits[num4];
						if (num5 != 0)
						{
							num3 -= ZTree.base_length[num4];
							this.send_bits(num3, num5);
						}
						num2--;
						num4 = ZTree.d_code(num2);
						this.send_code(num4, dtree);
						num5 = ZTree.extra_dbits[num4];
						if (num5 != 0)
						{
							num2 -= ZTree.base_dist[num4];
							this.send_bits(num2, num5);
						}
					}
				}
				while (num < this.last_lit);
			}
			this.send_code(256, ltree);
			this.last_eob_len = (int)ltree[513];
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x0007AAB8 File Offset: 0x00078CB8
		internal void set_data_type()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < 7)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 128)
			{
				num += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 256)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			this.data_type = ((num2 > num >> 2) ? 0 : 1);
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x0007AB2C File Offset: 0x00078D2C
		internal void bi_flush()
		{
			if (this.bi_valid == 16)
			{
				byte[] array = this.pending_buf;
				int num = this.pending;
				this.pending = num + 1;
				array[num] = (byte)this.bi_buf;
				byte[] array2 = this.pending_buf;
				num = this.pending;
				this.pending = num + 1;
				array2[num] = (byte)(this.bi_buf >> 8);
				this.bi_buf = 0U;
				this.bi_valid = 0;
				return;
			}
			if (this.bi_valid >= 8)
			{
				byte[] array3 = this.pending_buf;
				int num = this.pending;
				this.pending = num + 1;
				array3[num] = (byte)this.bi_buf;
				this.bi_buf >>= 8;
				this.bi_buf &= 255U;
				this.bi_valid -= 8;
			}
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x0007ABE8 File Offset: 0x00078DE8
		internal void bi_windup()
		{
			if (this.bi_valid > 8)
			{
				byte[] array = this.pending_buf;
				int num = this.pending;
				this.pending = num + 1;
				array[num] = (byte)this.bi_buf;
				byte[] array2 = this.pending_buf;
				num = this.pending;
				this.pending = num + 1;
				array2[num] = (byte)(this.bi_buf >> 8);
			}
			else if (this.bi_valid > 0)
			{
				byte[] array3 = this.pending_buf;
				int num = this.pending;
				this.pending = num + 1;
				array3[num] = (byte)this.bi_buf;
			}
			this.bi_buf = 0U;
			this.bi_valid = 0;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0007AC76 File Offset: 0x00078E76
		internal void copy_block(int buf, int len, bool header)
		{
			this.bi_windup();
			this.last_eob_len = 8;
			if (header)
			{
				this.put_short((int)((short)len));
				this.put_short((int)((short)(~(short)len)));
			}
			this.put_byte(this.window, buf, len);
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0007ACA7 File Offset: 0x00078EA7
		internal void flush_block_only(bool eof)
		{
			this._tr_flush_block((this.block_start >= 0) ? this.block_start : -1, this.strstart - this.block_start, eof);
			this.block_start = this.strstart;
			this.strm.flush_pending();
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0007ACE8 File Offset: 0x00078EE8
		internal int deflate_stored(int flush)
		{
			int num = 65535;
			if (num > this.pending_buf_size - 5)
			{
				num = this.pending_buf_size - 5;
			}
			for (;;)
			{
				if (this.lookahead <= 1)
				{
					this.fill_window();
					if (this.lookahead == 0 && flush == 0)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_D7;
					}
				}
				this.strstart += this.lookahead;
				this.lookahead = 0;
				int num2 = this.block_start + num;
				if (this.strstart == 0 || this.strstart >= num2)
				{
					this.lookahead = this.strstart - num2;
					this.strstart = num2;
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
				if (this.strstart - this.block_start >= this.w_size - 262)
				{
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
			}
			return 0;
			IL_D7:
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out == 0)
			{
				if (flush != 4)
				{
					return 0;
				}
				return 2;
			}
			else
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0007ADF2 File Offset: 0x00078FF2
		internal void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			this.send_bits(eof ? 1 : 0, 3);
			this.copy_block(buf, stored_len, true);
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0007AE0C File Offset: 0x0007900C
		internal void _tr_flush_block(int buf, int stored_len, bool eof)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.level > 0)
			{
				if (this.data_type == 2)
				{
					this.set_data_type();
				}
				this.l_desc.build_tree(this);
				this.d_desc.build_tree(this);
				num = this.build_bl_tree();
				num2 = this.opt_len + 3 + 7 >> 3;
				num3 = this.static_len + 3 + 7 >> 3;
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num3 = (num2 = stored_len + 5);
			}
			if (stored_len + 4 <= num2 && buf != -1)
			{
				this._tr_stored_block(buf, stored_len, eof);
			}
			else if (num3 == num2)
			{
				this.send_bits(2 + (eof ? 1 : 0), 3);
				this.compress_block(StaticTree.static_ltree, StaticTree.static_dtree);
			}
			else
			{
				this.send_bits(4 + (eof ? 1 : 0), 3);
				this.send_all_trees(this.l_desc.max_code + 1, this.d_desc.max_code + 1, num + 1);
				this.compress_block(this.dyn_ltree, this.dyn_dtree);
			}
			this.init_block();
			if (eof)
			{
				this.bi_windup();
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0007AF0C File Offset: 0x0007910C
		internal void fill_window()
		{
			for (;;)
			{
				int num = this.window_size - this.lookahead - this.strstart;
				int num2;
				if (num == 0 && this.strstart == 0 && this.lookahead == 0)
				{
					num = this.w_size;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.strstart >= this.w_size + this.w_size - 262)
				{
					Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
					this.match_start -= this.w_size;
					this.strstart -= this.w_size;
					this.block_start -= this.w_size;
					num2 = this.hash_size;
					int num3 = num2;
					do
					{
						int num4 = (int)this.head[--num3] & 65535;
						this.head[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num2 = this.w_size;
					num3 = num2;
					do
					{
						int num4 = (int)this.prev[--num3] & 65535;
						this.prev[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num += this.w_size;
				}
				if (this.strm.avail_in == 0)
				{
					break;
				}
				num2 = this.strm.read_buf(this.window, this.strstart + this.lookahead, num);
				this.lookahead += num2;
				if (this.lookahead >= 3)
				{
					this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
				}
				if (this.lookahead >= 262 || this.strm.avail_in == 0)
				{
					return;
				}
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0007B108 File Offset: 0x00079308
		internal int deflate_fast(int flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < 262)
				{
					this.fill_window();
					if (this.lookahead < 262 && flush == 0)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_2C4;
					}
				}
				if (this.lookahead >= 3)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				if ((long)num != 0L && (this.strstart - num & 65535) <= this.w_size - 262 && this.strategy != 2)
				{
					this.match_length = this.longest_match(num);
				}
				bool flag;
				if (this.match_length >= 3)
				{
					flag = this._tr_tally(this.strstart - this.match_start, this.match_length - 3);
					this.lookahead -= this.match_length;
					if (this.match_length <= this.max_lazy_match && this.lookahead >= 3)
					{
						this.match_length--;
						int num2;
						do
						{
							this.strstart++;
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
							num2 = this.match_length - 1;
							this.match_length = num2;
						}
						while (num2 != 0);
						this.strstart++;
					}
					else
					{
						this.strstart += this.match_length;
						this.match_length = 0;
						this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
						this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
					}
				}
				else
				{
					flag = this._tr_tally(0, (int)(this.window[this.strstart] & byte.MaxValue));
					this.lookahead--;
					this.strstart++;
				}
				if (flag)
				{
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
			}
			return 0;
			IL_2C4:
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out == 0)
			{
				if (flush == 4)
				{
					return 2;
				}
				return 0;
			}
			else
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0007B400 File Offset: 0x00079600
		internal int deflate_slow(int flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < 262)
				{
					this.fill_window();
					if (this.lookahead < 262 && flush == 0)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_323;
					}
				}
				if (this.lookahead >= 3)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				this.prev_length = this.match_length;
				this.prev_match = this.match_start;
				this.match_length = 2;
				if (num != 0 && this.prev_length < this.max_lazy_match && (this.strstart - num & 65535) <= this.w_size - 262)
				{
					if (this.strategy != 2)
					{
						this.match_length = this.longest_match(num);
					}
					if (this.match_length <= 5 && (this.strategy == 1 || (this.match_length == 3 && this.strstart - this.match_start > 4096)))
					{
						this.match_length = 2;
					}
				}
				if (this.prev_length >= 3 && this.match_length <= this.prev_length)
				{
					int num2 = this.strstart + this.lookahead - 3;
					bool flag = this._tr_tally(this.strstart - 1 - this.prev_match, this.prev_length - 3);
					this.lookahead -= this.prev_length - 1;
					this.prev_length -= 2;
					int num3;
					do
					{
						num3 = this.strstart + 1;
						this.strstart = num3;
						if (num3 <= num2)
						{
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
						num3 = this.prev_length - 1;
						this.prev_length = num3;
					}
					while (num3 != 0);
					this.match_available = 0;
					this.match_length = 2;
					this.strstart++;
					if (flag)
					{
						this.flush_block_only(false);
						if (this.strm.avail_out == 0)
						{
							return 0;
						}
					}
				}
				else if (this.match_available != 0)
				{
					bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & byte.MaxValue));
					if (flag)
					{
						this.flush_block_only(false);
					}
					this.strstart++;
					this.lookahead--;
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
				else
				{
					this.match_available = 1;
					this.strstart++;
					this.lookahead--;
				}
			}
			return 0;
			IL_323:
			if (this.match_available != 0)
			{
				bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & byte.MaxValue));
				this.match_available = 0;
			}
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out == 0)
			{
				if (flush == 4)
				{
					return 2;
				}
				return 0;
			}
			else
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0007B784 File Offset: 0x00079984
		internal int longest_match(int cur_match)
		{
			int num = this.max_chain_length;
			int num2 = this.strstart;
			int num3 = this.prev_length;
			int num4 = (this.strstart > this.w_size - 262) ? (this.strstart - (this.w_size - 262)) : 0;
			int num5 = this.nice_match;
			int num6 = this.w_mask;
			int num7 = this.strstart + 258;
			byte b = this.window[num2 + num3 - 1];
			byte b2 = this.window[num2 + num3];
			if (this.prev_length >= this.good_match)
			{
				num >>= 2;
			}
			if (num5 > this.lookahead)
			{
				num5 = this.lookahead;
			}
			do
			{
				int num8 = cur_match;
				if (this.window[num8 + num3] == b2 && this.window[num8 + num3 - 1] == b && this.window[num8] == this.window[num2] && this.window[++num8] == this.window[num2 + 1])
				{
					num2 += 2;
					num8++;
					while (this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && num2 < num7)
					{
					}
					int num9 = 258 - (num7 - num2);
					num2 = num7 - 258;
					if (num9 > num3)
					{
						this.match_start = cur_match;
						num3 = num9;
						if (num9 >= num5)
						{
							break;
						}
						b = this.window[num2 + num3 - 1];
						b2 = this.window[num2 + num3];
					}
				}
			}
			while ((cur_match = ((int)this.prev[cur_match & num6] & 65535)) > num4 && --num != 0);
			if (num3 <= this.lookahead)
			{
				return num3;
			}
			return this.lookahead;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0007B9EB File Offset: 0x00079BEB
		internal int deflateInit(ZStream strm, int level, int bits)
		{
			return this.deflateInit2(strm, level, 8, bits, 8, 0);
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0007B9F9 File Offset: 0x00079BF9
		internal int deflateInit(ZStream strm, int level)
		{
			return this.deflateInit(strm, level, 15);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0007BA08 File Offset: 0x00079C08
		internal int deflateInit2(ZStream strm, int level, int method, int windowBits, int memLevel, int strategy)
		{
			int num = 0;
			strm.msg = null;
			if (level == -1)
			{
				level = 6;
			}
			if (windowBits < 0)
			{
				num = 1;
				windowBits = -windowBits;
			}
			if (memLevel < 1 || memLevel > 9 || method != 8 || windowBits < 9 || windowBits > 15 || level < 0 || level > 9 || strategy < 0 || strategy > 2)
			{
				return -2;
			}
			strm.dstate = this;
			this.noheader = num;
			this.w_bits = windowBits;
			this.w_size = 1 << this.w_bits;
			this.w_mask = this.w_size - 1;
			this.hash_bits = memLevel + 7;
			this.hash_size = 1 << this.hash_bits;
			this.hash_mask = this.hash_size - 1;
			this.hash_shift = (this.hash_bits + 3 - 1) / 3;
			this.window = new byte[this.w_size * 2];
			this.prev = new short[this.w_size];
			this.head = new short[this.hash_size];
			this.lit_bufsize = 1 << memLevel + 6;
			this.pending_buf = new byte[this.lit_bufsize * 4];
			this.pending_buf_size = this.lit_bufsize * 4;
			this.d_buf = this.lit_bufsize / 2;
			this.l_buf = 3 * this.lit_bufsize;
			this.level = level;
			this.strategy = strategy;
			this.method = (byte)method;
			return this.deflateReset(strm);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0007BB74 File Offset: 0x00079D74
		internal int deflateReset(ZStream strm)
		{
			strm.total_in = (strm.total_out = 0L);
			strm.msg = null;
			strm.data_type = 2;
			this.pending = 0;
			this.pending_out = 0;
			if (this.noheader < 0)
			{
				this.noheader = 0;
			}
			this.status = ((this.noheader != 0) ? 113 : 42);
			strm.adler = strm._adler.adler32(0L, null, 0, 0);
			this.last_flush = 0;
			this.tr_init();
			this.lm_init();
			return 0;
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0007BBFC File Offset: 0x00079DFC
		internal int deflateEnd()
		{
			if (this.status != 42 && this.status != 113 && this.status != 666)
			{
				return -2;
			}
			this.pending_buf = null;
			this.head = null;
			this.prev = null;
			this.window = null;
			if (this.status != 113)
			{
				return 0;
			}
			return -3;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0007BC58 File Offset: 0x00079E58
		internal int deflateParams(ZStream strm, int _level, int _strategy)
		{
			int result = 0;
			if (_level == -1)
			{
				_level = 6;
			}
			if (_level < 0 || _level > 9 || _strategy < 0 || _strategy > 2)
			{
				return -2;
			}
			if (Deflate.config_table[this.level].func != Deflate.config_table[_level].func && strm.total_in != 0L)
			{
				result = strm.deflate(1);
			}
			if (this.level != _level)
			{
				this.level = _level;
				this.max_lazy_match = Deflate.config_table[this.level].max_lazy;
				this.good_match = Deflate.config_table[this.level].good_length;
				this.nice_match = Deflate.config_table[this.level].nice_length;
				this.max_chain_length = Deflate.config_table[this.level].max_chain;
			}
			this.strategy = _strategy;
			return result;
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0007BD28 File Offset: 0x00079F28
		internal int deflateSetDictionary(ZStream strm, byte[] dictionary, int dictLength)
		{
			int num = dictLength;
			int sourceIndex = 0;
			if (dictionary == null || this.status != 42)
			{
				return -2;
			}
			strm.adler = strm._adler.adler32(strm.adler, dictionary, 0, dictLength);
			if (num < 3)
			{
				return 0;
			}
			if (num > this.w_size - 262)
			{
				num = this.w_size - 262;
				sourceIndex = dictLength - num;
			}
			Array.Copy(dictionary, sourceIndex, this.window, 0, num);
			this.strstart = num;
			this.block_start = num;
			this.ins_h = (int)(this.window[0] & byte.MaxValue);
			this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[1] & byte.MaxValue)) & this.hash_mask);
			for (int i = 0; i <= num - 3; i++)
			{
				this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[i + 2] & byte.MaxValue)) & this.hash_mask);
				this.prev[i & this.w_mask] = this.head[this.ins_h];
				this.head[this.ins_h] = (short)i;
			}
			return 0;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0007BE50 File Offset: 0x0007A050
		internal int deflate(ZStream strm, int flush)
		{
			if (flush > 4 || flush < 0)
			{
				return -2;
			}
			if (strm.next_out == null || (strm.next_in == null && strm.avail_in != 0) || (this.status == 666 && flush != 4))
			{
				strm.msg = Deflate.z_errmsg[4];
				return -2;
			}
			if (strm.avail_out == 0)
			{
				strm.msg = Deflate.z_errmsg[7];
				return -5;
			}
			this.strm = strm;
			int num = this.last_flush;
			this.last_flush = flush;
			if (this.status == 42)
			{
				int num2 = 8 + (this.w_bits - 8 << 4) << 8;
				int num3 = (this.level - 1 & 255) >> 1;
				if (num3 > 3)
				{
					num3 = 3;
				}
				num2 |= num3 << 6;
				if (this.strstart != 0)
				{
					num2 |= 32;
				}
				num2 += 31 - num2 % 31;
				this.status = 113;
				this.putShortMSB(num2);
				if (this.strstart != 0)
				{
					this.putShortMSB((int)(strm.adler >> 16));
					this.putShortMSB((int)(strm.adler & 65535L));
				}
				strm.adler = strm._adler.adler32(0L, null, 0, 0);
			}
			if (this.pending != 0)
			{
				strm.flush_pending();
				if (strm.avail_out == 0)
				{
					this.last_flush = -1;
					return 0;
				}
			}
			else if (strm.avail_in == 0 && flush <= num && flush != 4)
			{
				strm.msg = Deflate.z_errmsg[7];
				return -5;
			}
			if (this.status == 666 && strm.avail_in != 0)
			{
				strm.msg = Deflate.z_errmsg[7];
				return -5;
			}
			if (strm.avail_in != 0 || this.lookahead != 0 || (flush != 0 && this.status != 666))
			{
				int num4 = -1;
				switch (Deflate.config_table[this.level].func)
				{
				case 0:
					num4 = this.deflate_stored(flush);
					break;
				case 1:
					num4 = this.deflate_fast(flush);
					break;
				case 2:
					num4 = this.deflate_slow(flush);
					break;
				}
				if (num4 == 2 || num4 == 3)
				{
					this.status = 666;
				}
				if (num4 == 0 || num4 == 2)
				{
					if (strm.avail_out == 0)
					{
						this.last_flush = -1;
					}
					return 0;
				}
				if (num4 == 1)
				{
					if (flush == 1)
					{
						this._tr_align();
					}
					else
					{
						this._tr_stored_block(0, 0, false);
						if (flush == 3)
						{
							for (int i = 0; i < this.hash_size; i++)
							{
								this.head[i] = 0;
							}
						}
					}
					strm.flush_pending();
					if (strm.avail_out == 0)
					{
						this.last_flush = -1;
						return 0;
					}
				}
			}
			if (flush != 4)
			{
				return 0;
			}
			if (this.noheader != 0)
			{
				return 1;
			}
			this.putShortMSB((int)(strm.adler >> 16));
			this.putShortMSB((int)(strm.adler & 65535L));
			strm.flush_pending();
			this.noheader = -1;
			if (this.pending == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0400139A RID: 5018
		private const int MAX_MEM_LEVEL = 9;

		// Token: 0x0400139B RID: 5019
		private const int Z_DEFAULT_COMPRESSION = -1;

		// Token: 0x0400139C RID: 5020
		private const int MAX_WBITS = 15;

		// Token: 0x0400139D RID: 5021
		private const int DEF_MEM_LEVEL = 8;

		// Token: 0x0400139E RID: 5022
		private const int STORED = 0;

		// Token: 0x0400139F RID: 5023
		private const int FAST = 1;

		// Token: 0x040013A0 RID: 5024
		private const int SLOW = 2;

		// Token: 0x040013A1 RID: 5025
		private static readonly Deflate.Config[] config_table;

		// Token: 0x040013A2 RID: 5026
		private static readonly string[] z_errmsg = new string[]
		{
			"need dictionary",
			"stream end",
			"",
			"file error",
			"stream error",
			"data error",
			"insufficient memory",
			"buffer error",
			"incompatible version",
			""
		};

		// Token: 0x040013A3 RID: 5027
		private const int NeedMore = 0;

		// Token: 0x040013A4 RID: 5028
		private const int BlockDone = 1;

		// Token: 0x040013A5 RID: 5029
		private const int FinishStarted = 2;

		// Token: 0x040013A6 RID: 5030
		private const int FinishDone = 3;

		// Token: 0x040013A7 RID: 5031
		private const int PRESET_DICT = 32;

		// Token: 0x040013A8 RID: 5032
		private const int Z_FILTERED = 1;

		// Token: 0x040013A9 RID: 5033
		private const int Z_HUFFMAN_ONLY = 2;

		// Token: 0x040013AA RID: 5034
		private const int Z_DEFAULT_STRATEGY = 0;

		// Token: 0x040013AB RID: 5035
		private const int Z_NO_FLUSH = 0;

		// Token: 0x040013AC RID: 5036
		private const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x040013AD RID: 5037
		private const int Z_SYNC_FLUSH = 2;

		// Token: 0x040013AE RID: 5038
		private const int Z_FULL_FLUSH = 3;

		// Token: 0x040013AF RID: 5039
		private const int Z_FINISH = 4;

		// Token: 0x040013B0 RID: 5040
		private const int Z_OK = 0;

		// Token: 0x040013B1 RID: 5041
		private const int Z_STREAM_END = 1;

		// Token: 0x040013B2 RID: 5042
		private const int Z_NEED_DICT = 2;

		// Token: 0x040013B3 RID: 5043
		private const int Z_ERRNO = -1;

		// Token: 0x040013B4 RID: 5044
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x040013B5 RID: 5045
		private const int Z_DATA_ERROR = -3;

		// Token: 0x040013B6 RID: 5046
		private const int Z_MEM_ERROR = -4;

		// Token: 0x040013B7 RID: 5047
		private const int Z_BUF_ERROR = -5;

		// Token: 0x040013B8 RID: 5048
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x040013B9 RID: 5049
		private const int INIT_STATE = 42;

		// Token: 0x040013BA RID: 5050
		private const int BUSY_STATE = 113;

		// Token: 0x040013BB RID: 5051
		private const int FINISH_STATE = 666;

		// Token: 0x040013BC RID: 5052
		private const int Z_DEFLATED = 8;

		// Token: 0x040013BD RID: 5053
		private const int STORED_BLOCK = 0;

		// Token: 0x040013BE RID: 5054
		private const int STATIC_TREES = 1;

		// Token: 0x040013BF RID: 5055
		private const int DYN_TREES = 2;

		// Token: 0x040013C0 RID: 5056
		private const int Z_BINARY = 0;

		// Token: 0x040013C1 RID: 5057
		private const int Z_ASCII = 1;

		// Token: 0x040013C2 RID: 5058
		private const int Z_UNKNOWN = 2;

		// Token: 0x040013C3 RID: 5059
		private const int Buf_size = 16;

		// Token: 0x040013C4 RID: 5060
		private const int REP_3_6 = 16;

		// Token: 0x040013C5 RID: 5061
		private const int REPZ_3_10 = 17;

		// Token: 0x040013C6 RID: 5062
		private const int REPZ_11_138 = 18;

		// Token: 0x040013C7 RID: 5063
		private const int MIN_MATCH = 3;

		// Token: 0x040013C8 RID: 5064
		private const int MAX_MATCH = 258;

		// Token: 0x040013C9 RID: 5065
		private const int MIN_LOOKAHEAD = 262;

		// Token: 0x040013CA RID: 5066
		private const int MAX_BITS = 15;

		// Token: 0x040013CB RID: 5067
		private const int D_CODES = 30;

		// Token: 0x040013CC RID: 5068
		private const int BL_CODES = 19;

		// Token: 0x040013CD RID: 5069
		private const int LENGTH_CODES = 29;

		// Token: 0x040013CE RID: 5070
		private const int LITERALS = 256;

		// Token: 0x040013CF RID: 5071
		private const int L_CODES = 286;

		// Token: 0x040013D0 RID: 5072
		private const int HEAP_SIZE = 573;

		// Token: 0x040013D1 RID: 5073
		private const int END_BLOCK = 256;

		// Token: 0x040013D2 RID: 5074
		internal ZStream strm;

		// Token: 0x040013D3 RID: 5075
		internal int status;

		// Token: 0x040013D4 RID: 5076
		internal byte[] pending_buf;

		// Token: 0x040013D5 RID: 5077
		internal int pending_buf_size;

		// Token: 0x040013D6 RID: 5078
		internal int pending_out;

		// Token: 0x040013D7 RID: 5079
		internal int pending;

		// Token: 0x040013D8 RID: 5080
		internal int noheader;

		// Token: 0x040013D9 RID: 5081
		internal byte data_type;

		// Token: 0x040013DA RID: 5082
		internal byte method;

		// Token: 0x040013DB RID: 5083
		internal int last_flush;

		// Token: 0x040013DC RID: 5084
		internal int w_size;

		// Token: 0x040013DD RID: 5085
		internal int w_bits;

		// Token: 0x040013DE RID: 5086
		internal int w_mask;

		// Token: 0x040013DF RID: 5087
		internal byte[] window;

		// Token: 0x040013E0 RID: 5088
		internal int window_size;

		// Token: 0x040013E1 RID: 5089
		internal short[] prev;

		// Token: 0x040013E2 RID: 5090
		internal short[] head;

		// Token: 0x040013E3 RID: 5091
		internal int ins_h;

		// Token: 0x040013E4 RID: 5092
		internal int hash_size;

		// Token: 0x040013E5 RID: 5093
		internal int hash_bits;

		// Token: 0x040013E6 RID: 5094
		internal int hash_mask;

		// Token: 0x040013E7 RID: 5095
		internal int hash_shift;

		// Token: 0x040013E8 RID: 5096
		internal int block_start;

		// Token: 0x040013E9 RID: 5097
		internal int match_length;

		// Token: 0x040013EA RID: 5098
		internal int prev_match;

		// Token: 0x040013EB RID: 5099
		internal int match_available;

		// Token: 0x040013EC RID: 5100
		internal int strstart;

		// Token: 0x040013ED RID: 5101
		internal int match_start;

		// Token: 0x040013EE RID: 5102
		internal int lookahead;

		// Token: 0x040013EF RID: 5103
		internal int prev_length;

		// Token: 0x040013F0 RID: 5104
		internal int max_chain_length;

		// Token: 0x040013F1 RID: 5105
		internal int max_lazy_match;

		// Token: 0x040013F2 RID: 5106
		internal int level;

		// Token: 0x040013F3 RID: 5107
		internal int strategy;

		// Token: 0x040013F4 RID: 5108
		internal int good_match;

		// Token: 0x040013F5 RID: 5109
		internal int nice_match;

		// Token: 0x040013F6 RID: 5110
		internal short[] dyn_ltree;

		// Token: 0x040013F7 RID: 5111
		internal short[] dyn_dtree;

		// Token: 0x040013F8 RID: 5112
		internal short[] bl_tree;

		// Token: 0x040013F9 RID: 5113
		internal ZTree l_desc = new ZTree();

		// Token: 0x040013FA RID: 5114
		internal ZTree d_desc = new ZTree();

		// Token: 0x040013FB RID: 5115
		internal ZTree bl_desc = new ZTree();

		// Token: 0x040013FC RID: 5116
		internal short[] bl_count = new short[16];

		// Token: 0x040013FD RID: 5117
		internal int[] heap = new int[573];

		// Token: 0x040013FE RID: 5118
		internal int heap_len;

		// Token: 0x040013FF RID: 5119
		internal int heap_max;

		// Token: 0x04001400 RID: 5120
		internal byte[] depth = new byte[573];

		// Token: 0x04001401 RID: 5121
		internal int l_buf;

		// Token: 0x04001402 RID: 5122
		internal int lit_bufsize;

		// Token: 0x04001403 RID: 5123
		internal int last_lit;

		// Token: 0x04001404 RID: 5124
		internal int d_buf;

		// Token: 0x04001405 RID: 5125
		internal int opt_len;

		// Token: 0x04001406 RID: 5126
		internal int static_len;

		// Token: 0x04001407 RID: 5127
		internal int matches;

		// Token: 0x04001408 RID: 5128
		internal int last_eob_len;

		// Token: 0x04001409 RID: 5129
		internal uint bi_buf;

		// Token: 0x0400140A RID: 5130
		internal int bi_valid;

		// Token: 0x0200087F RID: 2175
		internal class Config
		{
			// Token: 0x06004553 RID: 17747 RVA: 0x00144878 File Offset: 0x00142A78
			internal Config(int good_length, int max_lazy, int nice_length, int max_chain, int func)
			{
				this.good_length = good_length;
				this.max_lazy = max_lazy;
				this.nice_length = nice_length;
				this.max_chain = max_chain;
				this.func = func;
			}

			// Token: 0x04002F38 RID: 12088
			internal int good_length;

			// Token: 0x04002F39 RID: 12089
			internal int max_lazy;

			// Token: 0x04002F3A RID: 12090
			internal int nice_length;

			// Token: 0x04002F3B RID: 12091
			internal int max_chain;

			// Token: 0x04002F3C RID: 12092
			internal int func;
		}
	}
}

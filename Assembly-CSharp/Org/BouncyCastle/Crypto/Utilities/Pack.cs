using System;

namespace Org.BouncyCastle.Crypto.Utilities
{
	// Token: 0x0200038E RID: 910
	internal sealed class Pack
	{
		// Token: 0x0600224A RID: 8778 RVA: 0x00003425 File Offset: 0x00001625
		private Pack()
		{
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x000B530F File Offset: 0x000B350F
		internal static void UInt16_To_BE(ushort n, byte[] bs)
		{
			bs[0] = (byte)(n >> 8);
			bs[1] = (byte)n;
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x000B531D File Offset: 0x000B351D
		internal static void UInt16_To_BE(ushort n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 8);
			bs[off + 1] = (byte)n;
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x000B532D File Offset: 0x000B352D
		internal static ushort BE_To_UInt16(byte[] bs)
		{
			return (ushort)((int)bs[0] << 8 | (int)bs[1]);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x000B5339 File Offset: 0x000B3539
		internal static ushort BE_To_UInt16(byte[] bs, int off)
		{
			return (ushort)((int)bs[off] << 8 | (int)bs[off + 1]);
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x000B5348 File Offset: 0x000B3548
		internal static byte[] UInt32_To_BE(uint n)
		{
			byte[] array = new byte[4];
			Pack.UInt32_To_BE(n, array, 0);
			return array;
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000B5365 File Offset: 0x000B3565
		internal static void UInt32_To_BE(uint n, byte[] bs)
		{
			bs[0] = (byte)(n >> 24);
			bs[1] = (byte)(n >> 16);
			bs[2] = (byte)(n >> 8);
			bs[3] = (byte)n;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000B5383 File Offset: 0x000B3583
		internal static void UInt32_To_BE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 24);
			bs[off + 1] = (byte)(n >> 16);
			bs[off + 2] = (byte)(n >> 8);
			bs[off + 3] = (byte)n;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000B53A8 File Offset: 0x000B35A8
		internal static byte[] UInt32_To_BE(uint[] ns)
		{
			byte[] array = new byte[4 * ns.Length];
			Pack.UInt32_To_BE(ns, array, 0);
			return array;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000B53CC File Offset: 0x000B35CC
		internal static void UInt32_To_BE(uint[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				Pack.UInt32_To_BE(ns[i], bs, off);
				off += 4;
			}
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000B53F6 File Offset: 0x000B35F6
		internal static uint BE_To_UInt32(byte[] bs)
		{
			return (uint)((int)bs[0] << 24 | (int)bs[1] << 16 | (int)bs[2] << 8 | (int)bs[3]);
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000B540F File Offset: 0x000B360F
		internal static uint BE_To_UInt32(byte[] bs, int off)
		{
			return (uint)((int)bs[off] << 24 | (int)bs[off + 1] << 16 | (int)bs[off + 2] << 8 | (int)bs[off + 3]);
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000B5430 File Offset: 0x000B3630
		internal static void BE_To_UInt32(byte[] bs, int off, uint[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = Pack.BE_To_UInt32(bs, off);
				off += 4;
			}
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x000B545C File Offset: 0x000B365C
		internal static byte[] UInt64_To_BE(ulong n)
		{
			byte[] array = new byte[8];
			Pack.UInt64_To_BE(n, array, 0);
			return array;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000B5479 File Offset: 0x000B3679
		internal static void UInt64_To_BE(ulong n, byte[] bs)
		{
			Pack.UInt32_To_BE((uint)(n >> 32), bs);
			Pack.UInt32_To_BE((uint)n, bs, 4);
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000B548F File Offset: 0x000B368F
		internal static void UInt64_To_BE(ulong n, byte[] bs, int off)
		{
			Pack.UInt32_To_BE((uint)(n >> 32), bs, off);
			Pack.UInt32_To_BE((uint)n, bs, off + 4);
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x000B54A8 File Offset: 0x000B36A8
		internal static byte[] UInt64_To_BE(ulong[] ns)
		{
			byte[] array = new byte[8 * ns.Length];
			Pack.UInt64_To_BE(ns, array, 0);
			return array;
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x000B54CC File Offset: 0x000B36CC
		internal static void UInt64_To_BE(ulong[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				Pack.UInt64_To_BE(ns[i], bs, off);
				off += 8;
			}
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000B54F8 File Offset: 0x000B36F8
		internal static ulong BE_To_UInt64(byte[] bs)
		{
			ulong num = (ulong)Pack.BE_To_UInt32(bs);
			uint num2 = Pack.BE_To_UInt32(bs, 4);
			return num << 32 | (ulong)num2;
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x000B551C File Offset: 0x000B371C
		internal static ulong BE_To_UInt64(byte[] bs, int off)
		{
			ulong num = (ulong)Pack.BE_To_UInt32(bs, off);
			uint num2 = Pack.BE_To_UInt32(bs, off + 4);
			return num << 32 | (ulong)num2;
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x000B5544 File Offset: 0x000B3744
		internal static void BE_To_UInt64(byte[] bs, int off, ulong[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = Pack.BE_To_UInt64(bs, off);
				off += 8;
			}
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x000B556E File Offset: 0x000B376E
		internal static void UInt16_To_LE(ushort n, byte[] bs)
		{
			bs[0] = (byte)n;
			bs[1] = (byte)(n >> 8);
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000B557C File Offset: 0x000B377C
		internal static void UInt16_To_LE(ushort n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[off + 1] = (byte)(n >> 8);
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000B558C File Offset: 0x000B378C
		internal static ushort LE_To_UInt16(byte[] bs)
		{
			return (ushort)((int)bs[0] | (int)bs[1] << 8);
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000B5598 File Offset: 0x000B3798
		internal static ushort LE_To_UInt16(byte[] bs, int off)
		{
			return (ushort)((int)bs[off] | (int)bs[off + 1] << 8);
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x000B55A8 File Offset: 0x000B37A8
		internal static byte[] UInt32_To_LE(uint n)
		{
			byte[] array = new byte[4];
			Pack.UInt32_To_LE(n, array, 0);
			return array;
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000B55C5 File Offset: 0x000B37C5
		internal static void UInt32_To_LE(uint n, byte[] bs)
		{
			bs[0] = (byte)n;
			bs[1] = (byte)(n >> 8);
			bs[2] = (byte)(n >> 16);
			bs[3] = (byte)(n >> 24);
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000B55E3 File Offset: 0x000B37E3
		internal static void UInt32_To_LE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[off + 1] = (byte)(n >> 8);
			bs[off + 2] = (byte)(n >> 16);
			bs[off + 3] = (byte)(n >> 24);
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000B5608 File Offset: 0x000B3808
		internal static byte[] UInt32_To_LE(uint[] ns)
		{
			byte[] array = new byte[4 * ns.Length];
			Pack.UInt32_To_LE(ns, array, 0);
			return array;
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000B562C File Offset: 0x000B382C
		internal static void UInt32_To_LE(uint[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				Pack.UInt32_To_LE(ns[i], bs, off);
				off += 4;
			}
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000B5656 File Offset: 0x000B3856
		internal static uint LE_To_UInt32(byte[] bs)
		{
			return (uint)((int)bs[0] | (int)bs[1] << 8 | (int)bs[2] << 16 | (int)bs[3] << 24);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000B566F File Offset: 0x000B386F
		internal static uint LE_To_UInt32(byte[] bs, int off)
		{
			return (uint)((int)bs[off] | (int)bs[off + 1] << 8 | (int)bs[off + 2] << 16 | (int)bs[off + 3] << 24);
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000B5690 File Offset: 0x000B3890
		internal static void LE_To_UInt32(byte[] bs, int off, uint[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = Pack.LE_To_UInt32(bs, off);
				off += 4;
			}
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000B56BC File Offset: 0x000B38BC
		internal static void LE_To_UInt32(byte[] bs, int bOff, uint[] ns, int nOff, int count)
		{
			for (int i = 0; i < count; i++)
			{
				ns[nOff + i] = Pack.LE_To_UInt32(bs, bOff);
				bOff += 4;
			}
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000B56E8 File Offset: 0x000B38E8
		internal static uint[] LE_To_UInt32(byte[] bs, int off, int count)
		{
			uint[] array = new uint[count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Pack.LE_To_UInt32(bs, off);
				off += 4;
			}
			return array;
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000B571C File Offset: 0x000B391C
		internal static byte[] UInt64_To_LE(ulong n)
		{
			byte[] array = new byte[8];
			Pack.UInt64_To_LE(n, array, 0);
			return array;
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000B5739 File Offset: 0x000B3939
		internal static void UInt64_To_LE(ulong n, byte[] bs)
		{
			Pack.UInt32_To_LE((uint)n, bs);
			Pack.UInt32_To_LE((uint)(n >> 32), bs, 4);
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000B574F File Offset: 0x000B394F
		internal static void UInt64_To_LE(ulong n, byte[] bs, int off)
		{
			Pack.UInt32_To_LE((uint)n, bs, off);
			Pack.UInt32_To_LE((uint)(n >> 32), bs, off + 4);
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000B5768 File Offset: 0x000B3968
		internal static ulong LE_To_UInt64(byte[] bs)
		{
			uint num = Pack.LE_To_UInt32(bs);
			return (ulong)Pack.LE_To_UInt32(bs, 4) << 32 | (ulong)num;
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000B578C File Offset: 0x000B398C
		internal static ulong LE_To_UInt64(byte[] bs, int off)
		{
			uint num = Pack.LE_To_UInt32(bs, off);
			return (ulong)Pack.LE_To_UInt32(bs, off + 4) << 32 | (ulong)num;
		}
	}
}

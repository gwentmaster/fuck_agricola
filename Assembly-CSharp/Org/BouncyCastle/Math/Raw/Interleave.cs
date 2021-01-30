using System;

namespace Org.BouncyCastle.Math.Raw
{
	// Token: 0x020002C5 RID: 709
	internal abstract class Interleave
	{
		// Token: 0x06001796 RID: 6038 RVA: 0x000884E3 File Offset: 0x000866E3
		internal static uint Expand8to16(uint x)
		{
			x &= 255U;
			x = ((x | x << 4) & 3855U);
			x = ((x | x << 2) & 13107U);
			x = ((x | x << 1) & 21845U);
			return x;
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x00088516 File Offset: 0x00086716
		internal static uint Expand16to32(uint x)
		{
			x &= 65535U;
			x = ((x | x << 8) & 16711935U);
			x = ((x | x << 4) & 252645135U);
			x = ((x | x << 2) & 858993459U);
			x = ((x | x << 1) & 1431655765U);
			return x;
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00088558 File Offset: 0x00086758
		internal static ulong Expand32to64(uint x)
		{
			uint num = (x ^ x >> 8) & 65280U;
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 4) & 15728880U);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 2) & 202116108U);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 1) & 572662306U);
			x ^= (num ^ num << 1);
			return ((ulong)(x >> 1) & 1431655765UL) << 32 | ((ulong)x & 1431655765UL);
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000885D4 File Offset: 0x000867D4
		internal static void Expand64To128(ulong x, ulong[] z, int zOff)
		{
			ulong num = (x ^ x >> 16) & (ulong)-65536;
			x ^= (num ^ num << 16);
			num = ((x ^ x >> 8) & 280375465148160UL);
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 4) & 67555025218437360UL);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 2) & 868082074056920076UL);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 1) & 2459565876494606882UL);
			x ^= (num ^ num << 1);
			z[zOff] = (x & 6148914691236517205UL);
			z[zOff + 1] = (x >> 1 & 6148914691236517205UL);
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00088680 File Offset: 0x00086880
		internal static ulong Unshuffle(ulong x)
		{
			ulong num = (x ^ x >> 1) & 2459565876494606882UL;
			x ^= (num ^ num << 1);
			num = ((x ^ x >> 2) & 868082074056920076UL);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 4) & 67555025218437360UL);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 8) & 280375465148160UL);
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 16) & (ulong)-65536);
			x ^= (num ^ num << 16);
			return x;
		}

		// Token: 0x0400154B RID: 5451
		private const ulong M32 = 1431655765UL;

		// Token: 0x0400154C RID: 5452
		private const ulong M64 = 6148914691236517205UL;
	}
}

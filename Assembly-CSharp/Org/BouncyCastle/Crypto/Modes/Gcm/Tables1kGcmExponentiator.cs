using System;
using System.Collections;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x02000468 RID: 1128
	public class Tables1kGcmExponentiator : IGcmExponentiator
	{
		// Token: 0x0600291F RID: 10527 RVA: 0x000CB320 File Offset: 0x000C9520
		public void Init(byte[] x)
		{
			uint[] array = GcmUtilities.AsUints(x);
			if (this.lookupPowX2 != null && Arrays.AreEqual(array, (uint[])this.lookupPowX2[0]))
			{
				return;
			}
			this.lookupPowX2 = Platform.CreateArrayList(8);
			this.lookupPowX2.Add(array);
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000CB370 File Offset: 0x000C9570
		public void ExponentiateX(long pow, byte[] output)
		{
			uint[] x = GcmUtilities.OneAsUints();
			int num = 0;
			while (pow > 0L)
			{
				if ((pow & 1L) != 0L)
				{
					this.EnsureAvailable(num);
					GcmUtilities.Multiply(x, (uint[])this.lookupPowX2[num]);
				}
				num++;
				pow >>= 1;
			}
			GcmUtilities.AsBytes(x, output);
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000CB3C0 File Offset: 0x000C95C0
		private void EnsureAvailable(int bit)
		{
			int num = this.lookupPowX2.Count;
			if (num <= bit)
			{
				uint[] array = (uint[])this.lookupPowX2[num - 1];
				do
				{
					array = Arrays.Clone(array);
					GcmUtilities.Multiply(array, array);
					this.lookupPowX2.Add(array);
				}
				while (++num <= bit);
			}
		}

		// Token: 0x04001AF9 RID: 6905
		private IList lookupPowX2;
	}
}

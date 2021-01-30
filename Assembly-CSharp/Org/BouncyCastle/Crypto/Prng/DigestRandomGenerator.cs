using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x02000417 RID: 1047
	public class DigestRandomGenerator : IRandomGenerator
	{
		// Token: 0x060026E5 RID: 9957 RVA: 0x000C4024 File Offset: 0x000C2224
		public DigestRandomGenerator(IDigest digest)
		{
			this.digest = digest;
			this.seed = new byte[digest.GetDigestSize()];
			this.seedCounter = 1L;
			this.state = new byte[digest.GetDigestSize()];
			this.stateCounter = 1L;
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000C4070 File Offset: 0x000C2270
		public void AddSeedMaterial(byte[] inSeed)
		{
			lock (this)
			{
				this.DigestUpdate(inSeed);
				this.DigestUpdate(this.seed);
				this.DigestDoFinal(this.seed);
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000C40C4 File Offset: 0x000C22C4
		public void AddSeedMaterial(long rSeed)
		{
			lock (this)
			{
				this.DigestAddCounter(rSeed);
				this.DigestUpdate(this.seed);
				this.DigestDoFinal(this.seed);
			}
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000C4118 File Offset: 0x000C2318
		public void NextBytes(byte[] bytes)
		{
			this.NextBytes(bytes, 0, bytes.Length);
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x000C4128 File Offset: 0x000C2328
		public void NextBytes(byte[] bytes, int start, int len)
		{
			lock (this)
			{
				int num = 0;
				this.GenerateState();
				int num2 = start + len;
				for (int i = start; i < num2; i++)
				{
					if (num == this.state.Length)
					{
						this.GenerateState();
						num = 0;
					}
					bytes[i] = this.state[num++];
				}
			}
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000C419C File Offset: 0x000C239C
		private void CycleSeed()
		{
			this.DigestUpdate(this.seed);
			long num = this.seedCounter;
			this.seedCounter = num + 1L;
			this.DigestAddCounter(num);
			this.DigestDoFinal(this.seed);
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000C41DC File Offset: 0x000C23DC
		private void GenerateState()
		{
			long num = this.stateCounter;
			this.stateCounter = num + 1L;
			this.DigestAddCounter(num);
			this.DigestUpdate(this.state);
			this.DigestUpdate(this.seed);
			this.DigestDoFinal(this.state);
			if (this.stateCounter % 10L == 0L)
			{
				this.CycleSeed();
			}
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000C4238 File Offset: 0x000C2438
		private void DigestAddCounter(long seedVal)
		{
			byte[] array = new byte[8];
			Pack.UInt64_To_LE((ulong)seedVal, array);
			this.digest.BlockUpdate(array, 0, array.Length);
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000C4263 File Offset: 0x000C2463
		private void DigestUpdate(byte[] inSeed)
		{
			this.digest.BlockUpdate(inSeed, 0, inSeed.Length);
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000C4275 File Offset: 0x000C2475
		private void DigestDoFinal(byte[] result)
		{
			this.digest.DoFinal(result, 0);
		}

		// Token: 0x04001A0A RID: 6666
		private const long CYCLE_COUNT = 10L;

		// Token: 0x04001A0B RID: 6667
		private long stateCounter;

		// Token: 0x04001A0C RID: 6668
		private long seedCounter;

		// Token: 0x04001A0D RID: 6669
		private IDigest digest;

		// Token: 0x04001A0E RID: 6670
		private byte[] state;

		// Token: 0x04001A0F RID: 6671
		private byte[] seed;
	}
}

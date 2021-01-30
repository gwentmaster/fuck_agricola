using System;
using System.Threading;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Security
{
	// Token: 0x020002BA RID: 698
	public class SecureRandom : Random
	{
		// Token: 0x060016F3 RID: 5875 RVA: 0x00082FBC File Offset: 0x000811BC
		private static long NextCounterValue()
		{
			return Interlocked.Increment(ref SecureRandom.counter);
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x00082FC8 File Offset: 0x000811C8
		private static SecureRandom Master
		{
			get
			{
				return SecureRandom.master;
			}
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00082FD0 File Offset: 0x000811D0
		private static DigestRandomGenerator CreatePrng(string digestName, bool autoSeed)
		{
			IDigest digest = DigestUtilities.GetDigest(digestName);
			if (digest == null)
			{
				return null;
			}
			DigestRandomGenerator digestRandomGenerator = new DigestRandomGenerator(digest);
			if (autoSeed)
			{
				digestRandomGenerator.AddSeedMaterial(SecureRandom.NextCounterValue());
				digestRandomGenerator.AddSeedMaterial(SecureRandom.GetNextBytes(SecureRandom.Master, digest.GetDigestSize()));
			}
			return digestRandomGenerator;
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00083018 File Offset: 0x00081218
		public static byte[] GetNextBytes(SecureRandom secureRandom, int length)
		{
			byte[] array = new byte[length];
			secureRandom.NextBytes(array);
			return array;
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00083034 File Offset: 0x00081234
		public static SecureRandom GetInstance(string algorithm)
		{
			return SecureRandom.GetInstance(algorithm, true);
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00083040 File Offset: 0x00081240
		public static SecureRandom GetInstance(string algorithm, bool autoSeed)
		{
			string text = Platform.ToUpperInvariant(algorithm);
			if (Platform.EndsWith(text, "PRNG"))
			{
				DigestRandomGenerator digestRandomGenerator = SecureRandom.CreatePrng(text.Substring(0, text.Length - "PRNG".Length), autoSeed);
				if (digestRandomGenerator != null)
				{
					return new SecureRandom(digestRandomGenerator);
				}
			}
			throw new ArgumentException("Unrecognised PRNG algorithm: " + algorithm, "algorithm");
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x0008309F File Offset: 0x0008129F
		[Obsolete("Call GenerateSeed() on a SecureRandom instance instead")]
		public static byte[] GetSeed(int length)
		{
			return SecureRandom.GetNextBytes(SecureRandom.Master, length);
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x000830AC File Offset: 0x000812AC
		public SecureRandom() : this(SecureRandom.CreatePrng("SHA256", true))
		{
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x000830BF File Offset: 0x000812BF
		[Obsolete("Use GetInstance/SetSeed instead")]
		public SecureRandom(byte[] seed) : this(SecureRandom.CreatePrng("SHA1", false))
		{
			this.SetSeed(seed);
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x000830D9 File Offset: 0x000812D9
		public SecureRandom(IRandomGenerator generator) : base(0)
		{
			this.generator = generator;
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000830E9 File Offset: 0x000812E9
		public virtual byte[] GenerateSeed(int length)
		{
			return SecureRandom.GetNextBytes(SecureRandom.Master, length);
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x000830F6 File Offset: 0x000812F6
		public virtual void SetSeed(byte[] seed)
		{
			this.generator.AddSeedMaterial(seed);
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00083104 File Offset: 0x00081304
		public virtual void SetSeed(long seed)
		{
			this.generator.AddSeedMaterial(seed);
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00083112 File Offset: 0x00081312
		public override int Next()
		{
			return this.NextInt() & int.MaxValue;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00083120 File Offset: 0x00081320
		public override int Next(int maxValue)
		{
			if (maxValue < 2)
			{
				if (maxValue < 0)
				{
					throw new ArgumentOutOfRangeException("maxValue", "cannot be negative");
				}
				return 0;
			}
			else
			{
				int num;
				if ((maxValue & maxValue - 1) == 0)
				{
					num = (this.NextInt() & int.MaxValue);
					return (int)((long)num * (long)maxValue >> 31);
				}
				int num2;
				do
				{
					num = (this.NextInt() & int.MaxValue);
					num2 = num % maxValue;
				}
				while (num - num2 + (maxValue - 1) < 0);
				return num2;
			}
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00083184 File Offset: 0x00081384
		public override int Next(int minValue, int maxValue)
		{
			if (maxValue <= minValue)
			{
				if (maxValue == minValue)
				{
					return minValue;
				}
				throw new ArgumentException("maxValue cannot be less than minValue");
			}
			else
			{
				int num = maxValue - minValue;
				if (num > 0)
				{
					return minValue + this.Next(num);
				}
				int num2;
				do
				{
					num2 = this.NextInt();
				}
				while (num2 < minValue || num2 >= maxValue);
				return num2;
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x000831C8 File Offset: 0x000813C8
		public override void NextBytes(byte[] buf)
		{
			this.generator.NextBytes(buf);
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x000831D6 File Offset: 0x000813D6
		public virtual void NextBytes(byte[] buf, int off, int len)
		{
			this.generator.NextBytes(buf, off, len);
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x000831E6 File Offset: 0x000813E6
		public override double NextDouble()
		{
			return Convert.ToDouble((ulong)this.NextLong()) / SecureRandom.DoubleScale;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000831FC File Offset: 0x000813FC
		public virtual int NextInt()
		{
			byte[] array = new byte[4];
			this.NextBytes(array);
			return (((int)array[0] << 8 | (int)array[1]) << 8 | (int)array[2]) << 8 | (int)array[3];
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x0008322C File Offset: 0x0008142C
		public virtual long NextLong()
		{
			return (long)((ulong)this.NextInt() << 32 | (ulong)this.NextInt());
		}

		// Token: 0x04001522 RID: 5410
		private static long counter = Times.NanoTime();

		// Token: 0x04001523 RID: 5411
		private static readonly SecureRandom master = new SecureRandom(new CryptoApiRandomGenerator());

		// Token: 0x04001524 RID: 5412
		protected readonly IRandomGenerator generator;

		// Token: 0x04001525 RID: 5413
		private static readonly double DoubleScale = Math.Pow(2.0, 64.0);
	}
}

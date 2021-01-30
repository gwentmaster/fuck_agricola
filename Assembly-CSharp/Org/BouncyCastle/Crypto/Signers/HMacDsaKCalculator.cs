using System;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200040E RID: 1038
	public class HMacDsaKCalculator : IDsaKCalculator
	{
		// Token: 0x06002691 RID: 9873 RVA: 0x000C2335 File Offset: 0x000C0535
		public HMacDsaKCalculator(IDigest digest)
		{
			this.hMac = new HMac(digest);
			this.V = new byte[this.hMac.GetMacSize()];
			this.K = new byte[this.hMac.GetMacSize()];
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06002692 RID: 9874 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsDeterministic
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000C2375 File Offset: 0x000C0575
		public virtual void Init(BigInteger n, SecureRandom random)
		{
			throw new InvalidOperationException("Operation not supported");
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000C2384 File Offset: 0x000C0584
		public void Init(BigInteger n, BigInteger d, byte[] message)
		{
			this.n = n;
			Arrays.Fill(this.V, 1);
			Arrays.Fill(this.K, 0);
			byte[] array = new byte[(n.BitLength + 7) / 8];
			byte[] array2 = BigIntegers.AsUnsignedByteArray(d);
			Array.Copy(array2, 0, array, array.Length - array2.Length, array2.Length);
			byte[] array3 = new byte[(n.BitLength + 7) / 8];
			BigInteger bigInteger = this.BitsToInt(message);
			if (bigInteger.CompareTo(n) >= 0)
			{
				bigInteger = bigInteger.Subtract(n);
			}
			byte[] array4 = BigIntegers.AsUnsignedByteArray(bigInteger);
			Array.Copy(array4, 0, array3, array3.Length - array4.Length, array4.Length);
			this.hMac.Init(new KeyParameter(this.K));
			this.hMac.BlockUpdate(this.V, 0, this.V.Length);
			this.hMac.Update(0);
			this.hMac.BlockUpdate(array, 0, array.Length);
			this.hMac.BlockUpdate(array3, 0, array3.Length);
			this.hMac.DoFinal(this.K, 0);
			this.hMac.Init(new KeyParameter(this.K));
			this.hMac.BlockUpdate(this.V, 0, this.V.Length);
			this.hMac.DoFinal(this.V, 0);
			this.hMac.BlockUpdate(this.V, 0, this.V.Length);
			this.hMac.Update(1);
			this.hMac.BlockUpdate(array, 0, array.Length);
			this.hMac.BlockUpdate(array3, 0, array3.Length);
			this.hMac.DoFinal(this.K, 0);
			this.hMac.Init(new KeyParameter(this.K));
			this.hMac.BlockUpdate(this.V, 0, this.V.Length);
			this.hMac.DoFinal(this.V, 0);
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000C2570 File Offset: 0x000C0770
		public virtual BigInteger NextK()
		{
			byte[] array = new byte[(this.n.BitLength + 7) / 8];
			BigInteger bigInteger;
			for (;;)
			{
				int num;
				for (int i = 0; i < array.Length; i += num)
				{
					this.hMac.BlockUpdate(this.V, 0, this.V.Length);
					this.hMac.DoFinal(this.V, 0);
					num = Math.Min(array.Length - i, this.V.Length);
					Array.Copy(this.V, 0, array, i, num);
				}
				bigInteger = this.BitsToInt(array);
				if (bigInteger.SignValue > 0 && bigInteger.CompareTo(this.n) < 0)
				{
					break;
				}
				this.hMac.BlockUpdate(this.V, 0, this.V.Length);
				this.hMac.Update(0);
				this.hMac.DoFinal(this.K, 0);
				this.hMac.Init(new KeyParameter(this.K));
				this.hMac.BlockUpdate(this.V, 0, this.V.Length);
				this.hMac.DoFinal(this.V, 0);
			}
			return bigInteger;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000C2694 File Offset: 0x000C0894
		private BigInteger BitsToInt(byte[] t)
		{
			BigInteger bigInteger = new BigInteger(1, t);
			if (t.Length * 8 > this.n.BitLength)
			{
				bigInteger = bigInteger.ShiftRight(t.Length * 8 - this.n.BitLength);
			}
			return bigInteger;
		}

		// Token: 0x040019C1 RID: 6593
		private readonly HMac hMac;

		// Token: 0x040019C2 RID: 6594
		private readonly byte[] K;

		// Token: 0x040019C3 RID: 6595
		private readonly byte[] V;

		// Token: 0x040019C4 RID: 6596
		private BigInteger n;
	}
}

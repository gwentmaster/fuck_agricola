using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004AD RID: 1197
	public class KeccakDigest : IDigest, IMemoable
	{
		// Token: 0x06002C0C RID: 11276 RVA: 0x000E294C File Offset: 0x000E0B4C
		private static ulong[] KeccakInitializeRoundConstants()
		{
			ulong[] array = new ulong[24];
			byte b = 1;
			for (int i = 0; i < 24; i++)
			{
				array[i] = 0UL;
				for (int j = 0; j < 7; j++)
				{
					int num = (1 << j) - 1;
					if ((b & 1) > 0)
					{
						array[i] ^= 1UL << num;
					}
					bool flag = (b & 128) > 0;
					b = (byte)(b << 1);
					if (flag)
					{
						b ^= 113;
					}
				}
			}
			return array;
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000E29C0 File Offset: 0x000E0BC0
		private static int[] KeccakInitializeRhoOffsets()
		{
			int[] array = new int[25];
			int num = 0;
			array[0] = num;
			int num2 = 1;
			int num3 = 0;
			for (int i = 1; i < 25; i++)
			{
				num = (num + i & 63);
				array[num2 % 5 + 5 * (num3 % 5)] = num;
				int num4 = num3 % 5;
				int num5 = (2 * num2 + 3 * num3) % 5;
				num2 = num4;
				num3 = num5;
			}
			return array;
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000E2A18 File Offset: 0x000E0C18
		private void ClearDataQueueSection(int off, int len)
		{
			for (int num = off; num != off + len; num++)
			{
				this.dataQueue[num] = 0;
			}
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000E2A3C File Offset: 0x000E0C3C
		public KeccakDigest() : this(288)
		{
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000E2A4C File Offset: 0x000E0C4C
		public KeccakDigest(int bitLength)
		{
			this.state = new byte[200];
			this.dataQueue = new byte[192];
			this.C = new ulong[5];
			this.tempA = new ulong[25];
			this.chiC = new ulong[5];
			base..ctor();
			this.Init(bitLength);
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000E2AAC File Offset: 0x000E0CAC
		public KeccakDigest(KeccakDigest source)
		{
			this.state = new byte[200];
			this.dataQueue = new byte[192];
			this.C = new ulong[5];
			this.tempA = new ulong[25];
			this.chiC = new ulong[5];
			base..ctor();
			this.CopyIn(source);
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000E2B0C File Offset: 0x000E0D0C
		private void CopyIn(KeccakDigest source)
		{
			Array.Copy(source.state, 0, this.state, 0, source.state.Length);
			Array.Copy(source.dataQueue, 0, this.dataQueue, 0, source.dataQueue.Length);
			this.rate = source.rate;
			this.bitsInQueue = source.bitsInQueue;
			this.fixedOutputLength = source.fixedOutputLength;
			this.squeezing = source.squeezing;
			this.bitsAvailableForSqueezing = source.bitsAvailableForSqueezing;
			this.chunk = Arrays.Clone(source.chunk);
			this.oneByte = Arrays.Clone(source.oneByte);
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06002C13 RID: 11283 RVA: 0x000E2BAD File Offset: 0x000E0DAD
		public virtual string AlgorithmName
		{
			get
			{
				return "Keccak-" + this.fixedOutputLength;
			}
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000E2BC4 File Offset: 0x000E0DC4
		public virtual int GetDigestSize()
		{
			return this.fixedOutputLength / 8;
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000E2BCE File Offset: 0x000E0DCE
		public virtual void Update(byte input)
		{
			this.oneByte[0] = input;
			this.Absorb(this.oneByte, 0, 8L);
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000E2BE8 File Offset: 0x000E0DE8
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.Absorb(input, inOff, (long)len * 8L);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000E2BF7 File Offset: 0x000E0DF7
		public virtual int DoFinal(byte[] output, int outOff)
		{
			this.Squeeze(output, outOff, (long)this.fixedOutputLength);
			this.Reset();
			return this.GetDigestSize();
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000E2C14 File Offset: 0x000E0E14
		protected virtual int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
		{
			if (partialBits > 0)
			{
				this.oneByte[0] = partialByte;
				this.Absorb(this.oneByte, 0, (long)partialBits);
			}
			this.Squeeze(output, outOff, (long)this.fixedOutputLength);
			this.Reset();
			return this.GetDigestSize();
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000E2C4F File Offset: 0x000E0E4F
		public virtual void Reset()
		{
			this.Init(this.fixedOutputLength);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000E2C5D File Offset: 0x000E0E5D
		public virtual int GetByteLength()
		{
			return this.rate / 8;
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000E2C68 File Offset: 0x000E0E68
		private void Init(int bitLength)
		{
			if (bitLength <= 256)
			{
				if (bitLength == 128)
				{
					this.InitSponge(1344, 256);
					return;
				}
				if (bitLength == 224)
				{
					this.InitSponge(1152, 448);
					return;
				}
				if (bitLength == 256)
				{
					this.InitSponge(1088, 512);
					return;
				}
			}
			else
			{
				if (bitLength == 288)
				{
					this.InitSponge(1024, 576);
					return;
				}
				if (bitLength == 384)
				{
					this.InitSponge(832, 768);
					return;
				}
				if (bitLength == 512)
				{
					this.InitSponge(576, 1024);
					return;
				}
			}
			throw new ArgumentException("must be one of 128, 224, 256, 288, 384, or 512.", "bitLength");
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000E2D2C File Offset: 0x000E0F2C
		private void InitSponge(int rate, int capacity)
		{
			if (rate + capacity != 1600)
			{
				throw new InvalidOperationException("rate + capacity != 1600");
			}
			if (rate <= 0 || rate >= 1600 || rate % 64 != 0)
			{
				throw new InvalidOperationException("invalid rate value");
			}
			this.rate = rate;
			this.fixedOutputLength = 0;
			Arrays.Fill(this.state, 0);
			Arrays.Fill(this.dataQueue, 0);
			this.bitsInQueue = 0;
			this.squeezing = false;
			this.bitsAvailableForSqueezing = 0;
			this.fixedOutputLength = capacity / 2;
			this.chunk = new byte[rate / 8];
			this.oneByte = new byte[1];
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000E2DC9 File Offset: 0x000E0FC9
		private void AbsorbQueue()
		{
			this.KeccakAbsorb(this.state, this.dataQueue, this.rate / 8);
			this.bitsInQueue = 0;
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000E2DEC File Offset: 0x000E0FEC
		protected virtual void Absorb(byte[] data, int off, long databitlen)
		{
			if (this.bitsInQueue % 8 != 0)
			{
				throw new InvalidOperationException("attempt to absorb with odd length queue");
			}
			if (this.squeezing)
			{
				throw new InvalidOperationException("attempt to absorb while squeezing");
			}
			long num = 0L;
			while (num < databitlen)
			{
				if (this.bitsInQueue == 0 && databitlen >= (long)this.rate && num <= databitlen - (long)this.rate)
				{
					long num2 = (databitlen - num) / (long)this.rate;
					for (long num3 = 0L; num3 < num2; num3 += 1L)
					{
						Array.Copy(data, (int)((long)off + num / 8L + num3 * (long)this.chunk.Length), this.chunk, 0, this.chunk.Length);
						this.KeccakAbsorb(this.state, this.chunk, this.chunk.Length);
					}
					num += num2 * (long)this.rate;
				}
				else
				{
					int num4 = (int)(databitlen - num);
					if (num4 + this.bitsInQueue > this.rate)
					{
						num4 = this.rate - this.bitsInQueue;
					}
					int num5 = num4 % 8;
					num4 -= num5;
					Array.Copy(data, off + (int)(num / 8L), this.dataQueue, this.bitsInQueue / 8, num4 / 8);
					this.bitsInQueue += num4;
					num += (long)num4;
					if (this.bitsInQueue == this.rate)
					{
						this.AbsorbQueue();
					}
					if (num5 > 0)
					{
						int num6 = (1 << num5) - 1;
						this.dataQueue[this.bitsInQueue / 8] = (byte)((int)data[off + (int)(num / 8L)] & num6);
						this.bitsInQueue += num5;
						num += (long)num5;
					}
				}
			}
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000E2F74 File Offset: 0x000E1174
		private void PadAndSwitchToSqueezingPhase()
		{
			if (this.bitsInQueue + 1 == this.rate)
			{
				byte[] array = this.dataQueue;
				int num = this.bitsInQueue / 8;
				array[num] |= (byte)(1 << this.bitsInQueue % 8);
				this.AbsorbQueue();
				this.ClearDataQueueSection(0, this.rate / 8);
			}
			else
			{
				this.ClearDataQueueSection((this.bitsInQueue + 7) / 8, this.rate / 8 - (this.bitsInQueue + 7) / 8);
				byte[] array2 = this.dataQueue;
				int num2 = this.bitsInQueue / 8;
				array2[num2] |= (byte)(1 << this.bitsInQueue % 8);
			}
			byte[] array3 = this.dataQueue;
			int num3 = (this.rate - 1) / 8;
			array3[num3] |= (byte)(1 << (this.rate - 1) % 8);
			this.AbsorbQueue();
			if (this.rate == 1024)
			{
				this.KeccakExtract1024bits(this.state, this.dataQueue);
				this.bitsAvailableForSqueezing = 1024;
			}
			else
			{
				this.KeccakExtract(this.state, this.dataQueue, this.rate / 64);
				this.bitsAvailableForSqueezing = this.rate;
			}
			this.squeezing = true;
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000E30A4 File Offset: 0x000E12A4
		protected virtual void Squeeze(byte[] output, int offset, long outputLength)
		{
			if (!this.squeezing)
			{
				this.PadAndSwitchToSqueezingPhase();
			}
			if (outputLength % 8L != 0L)
			{
				throw new InvalidOperationException("outputLength not a multiple of 8");
			}
			int num2;
			for (long num = 0L; num < outputLength; num += (long)num2)
			{
				if (this.bitsAvailableForSqueezing == 0)
				{
					this.KeccakPermutation(this.state);
					if (this.rate == 1024)
					{
						this.KeccakExtract1024bits(this.state, this.dataQueue);
						this.bitsAvailableForSqueezing = 1024;
					}
					else
					{
						this.KeccakExtract(this.state, this.dataQueue, this.rate / 64);
						this.bitsAvailableForSqueezing = this.rate;
					}
				}
				num2 = this.bitsAvailableForSqueezing;
				if ((long)num2 > outputLength - num)
				{
					num2 = (int)(outputLength - num);
				}
				Array.Copy(this.dataQueue, (this.rate - this.bitsAvailableForSqueezing) / 8, output, offset + (int)(num / 8L), num2 / 8);
				this.bitsAvailableForSqueezing -= num2;
			}
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000E3194 File Offset: 0x000E1394
		private static void FromBytesToWords(ulong[] stateAsWords, byte[] state)
		{
			for (int i = 0; i < 25; i++)
			{
				stateAsWords[i] = 0UL;
				int num = i * 8;
				for (int j = 0; j < 8; j++)
				{
					stateAsWords[i] |= ((ulong)state[num + j] & 255UL) << 8 * j;
				}
			}
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000E31E4 File Offset: 0x000E13E4
		private static void FromWordsToBytes(byte[] state, ulong[] stateAsWords)
		{
			for (int i = 0; i < 25; i++)
			{
				int num = i * 8;
				for (int j = 0; j < 8; j++)
				{
					state[num + j] = (byte)(stateAsWords[i] >> 8 * j);
				}
			}
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000E3220 File Offset: 0x000E1420
		private void KeccakPermutation(byte[] state)
		{
			ulong[] stateAsWords = new ulong[state.Length / 8];
			KeccakDigest.FromBytesToWords(stateAsWords, state);
			this.KeccakPermutationOnWords(stateAsWords);
			KeccakDigest.FromWordsToBytes(state, stateAsWords);
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000E3250 File Offset: 0x000E1450
		private void KeccakPermutationAfterXor(byte[] state, byte[] data, int dataLengthInBytes)
		{
			for (int i = 0; i < dataLengthInBytes; i++)
			{
				int num = i;
				state[num] ^= data[i];
			}
			this.KeccakPermutation(state);
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000E3280 File Offset: 0x000E1480
		private void KeccakPermutationOnWords(ulong[] state)
		{
			for (int i = 0; i < 24; i++)
			{
				this.Theta(state);
				this.Rho(state);
				this.Pi(state);
				this.Chi(state);
				KeccakDigest.Iota(state, i);
			}
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000E32C0 File Offset: 0x000E14C0
		private void Theta(ulong[] A)
		{
			for (int i = 0; i < 5; i++)
			{
				this.C[i] = 0UL;
				for (int j = 0; j < 5; j++)
				{
					this.C[i] ^= A[i + 5 * j];
				}
			}
			for (int k = 0; k < 5; k++)
			{
				ulong num = this.C[(k + 1) % 5] << 1 ^ this.C[(k + 1) % 5] >> 63 ^ this.C[(k + 4) % 5];
				for (int l = 0; l < 5; l++)
				{
					A[k + 5 * l] ^= num;
				}
			}
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000E3360 File Offset: 0x000E1560
		private void Rho(ulong[] A)
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					int num = i + 5 * j;
					A[num] = ((KeccakDigest.KeccakRhoOffsets[num] != 0) ? (A[num] << KeccakDigest.KeccakRhoOffsets[num] ^ A[num] >> 64 - KeccakDigest.KeccakRhoOffsets[num]) : A[num]);
				}
			}
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000E33BC File Offset: 0x000E15BC
		private void Pi(ulong[] A)
		{
			Array.Copy(A, 0, this.tempA, 0, this.tempA.Length);
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					A[j + 5 * ((2 * i + 3 * j) % 5)] = this.tempA[i + 5 * j];
				}
			}
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000E3414 File Offset: 0x000E1614
		private void Chi(ulong[] A)
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					this.chiC[j] = (A[j + 5 * i] ^ (~A[(j + 1) % 5 + 5 * i] & A[(j + 2) % 5 + 5 * i]));
				}
				for (int k = 0; k < 5; k++)
				{
					A[k + 5 * i] = this.chiC[k];
				}
			}
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000E347C File Offset: 0x000E167C
		private static void Iota(ulong[] A, int indexRound)
		{
			A[0] ^= KeccakDigest.KeccakRoundConstants[indexRound];
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000E3490 File Offset: 0x000E1690
		private void KeccakAbsorb(byte[] byteState, byte[] data, int dataInBytes)
		{
			this.KeccakPermutationAfterXor(byteState, data, dataInBytes);
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000E349B File Offset: 0x000E169B
		private void KeccakExtract1024bits(byte[] byteState, byte[] data)
		{
			Array.Copy(byteState, 0, data, 0, 128);
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000E34AB File Offset: 0x000E16AB
		private void KeccakExtract(byte[] byteState, byte[] data, int laneCount)
		{
			Array.Copy(byteState, 0, data, 0, laneCount * 8);
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x000E34B9 File Offset: 0x000E16B9
		public virtual IMemoable Copy()
		{
			return new KeccakDigest(this);
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x000E34C4 File Offset: 0x000E16C4
		public virtual void Reset(IMemoable other)
		{
			KeccakDigest source = (KeccakDigest)other;
			this.CopyIn(source);
		}

		// Token: 0x04001D0B RID: 7435
		private static readonly ulong[] KeccakRoundConstants = KeccakDigest.KeccakInitializeRoundConstants();

		// Token: 0x04001D0C RID: 7436
		private static readonly int[] KeccakRhoOffsets = KeccakDigest.KeccakInitializeRhoOffsets();

		// Token: 0x04001D0D RID: 7437
		protected byte[] state;

		// Token: 0x04001D0E RID: 7438
		protected byte[] dataQueue;

		// Token: 0x04001D0F RID: 7439
		protected int rate;

		// Token: 0x04001D10 RID: 7440
		protected int bitsInQueue;

		// Token: 0x04001D11 RID: 7441
		protected int fixedOutputLength;

		// Token: 0x04001D12 RID: 7442
		protected bool squeezing;

		// Token: 0x04001D13 RID: 7443
		protected int bitsAvailableForSqueezing;

		// Token: 0x04001D14 RID: 7444
		protected byte[] chunk;

		// Token: 0x04001D15 RID: 7445
		protected byte[] oneByte;

		// Token: 0x04001D16 RID: 7446
		private ulong[] C;

		// Token: 0x04001D17 RID: 7447
		private ulong[] tempA;

		// Token: 0x04001D18 RID: 7448
		private ulong[] chiC;
	}
}

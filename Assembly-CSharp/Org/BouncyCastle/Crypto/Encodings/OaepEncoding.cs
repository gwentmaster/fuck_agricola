using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Encodings
{
	// Token: 0x020004A8 RID: 1192
	public class OaepEncoding : IAsymmetricBlockCipher
	{
		// Token: 0x06002BBA RID: 11194 RVA: 0x000E0DE8 File Offset: 0x000DEFE8
		public OaepEncoding(IAsymmetricBlockCipher cipher) : this(cipher, new Sha1Digest(), null)
		{
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000E0DF7 File Offset: 0x000DEFF7
		public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash) : this(cipher, hash, null)
		{
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x000E0E02 File Offset: 0x000DF002
		public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash, byte[] encodingParams) : this(cipher, hash, hash, encodingParams)
		{
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000E0E10 File Offset: 0x000DF010
		public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash, IDigest mgf1Hash, byte[] encodingParams)
		{
			this.engine = cipher;
			this.hash = hash;
			this.mgf1Hash = mgf1Hash;
			this.defHash = new byte[hash.GetDigestSize()];
			if (encodingParams != null)
			{
				hash.BlockUpdate(encodingParams, 0, encodingParams.Length);
			}
			hash.DoFinal(this.defHash, 0);
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000E0E68 File Offset: 0x000DF068
		public IAsymmetricBlockCipher GetUnderlyingCipher()
		{
			return this.engine;
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06002BBF RID: 11199 RVA: 0x000E0E70 File Offset: 0x000DF070
		public string AlgorithmName
		{
			get
			{
				return this.engine.AlgorithmName + "/OAEPPadding";
			}
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x000E0E88 File Offset: 0x000DF088
		public void Init(bool forEncryption, ICipherParameters param)
		{
			if (param is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
				this.random = parametersWithRandom.Random;
			}
			else
			{
				this.random = new SecureRandom();
			}
			this.engine.Init(forEncryption, param);
			this.forEncryption = forEncryption;
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000E0ED4 File Offset: 0x000DF0D4
		public int GetInputBlockSize()
		{
			int inputBlockSize = this.engine.GetInputBlockSize();
			if (this.forEncryption)
			{
				return inputBlockSize - 1 - 2 * this.defHash.Length;
			}
			return inputBlockSize;
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x000E0F08 File Offset: 0x000DF108
		public int GetOutputBlockSize()
		{
			int outputBlockSize = this.engine.GetOutputBlockSize();
			if (this.forEncryption)
			{
				return outputBlockSize;
			}
			return outputBlockSize - 1 - 2 * this.defHash.Length;
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000E0F39 File Offset: 0x000DF139
		public byte[] ProcessBlock(byte[] inBytes, int inOff, int inLen)
		{
			if (this.forEncryption)
			{
				return this.EncodeBlock(inBytes, inOff, inLen);
			}
			return this.DecodeBlock(inBytes, inOff, inLen);
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000E0F58 File Offset: 0x000DF158
		private byte[] EncodeBlock(byte[] inBytes, int inOff, int inLen)
		{
			byte[] array = new byte[this.GetInputBlockSize() + 1 + 2 * this.defHash.Length];
			Array.Copy(inBytes, inOff, array, array.Length - inLen, inLen);
			array[array.Length - inLen - 1] = 1;
			Array.Copy(this.defHash, 0, array, this.defHash.Length, this.defHash.Length);
			byte[] nextBytes = SecureRandom.GetNextBytes(this.random, this.defHash.Length);
			byte[] array2 = this.maskGeneratorFunction1(nextBytes, 0, nextBytes.Length, array.Length - this.defHash.Length);
			for (int num = this.defHash.Length; num != array.Length; num++)
			{
				byte[] array3 = array;
				int num2 = num;
				array3[num2] ^= array2[num - this.defHash.Length];
			}
			Array.Copy(nextBytes, 0, array, 0, this.defHash.Length);
			array2 = this.maskGeneratorFunction1(array, this.defHash.Length, array.Length - this.defHash.Length, this.defHash.Length);
			for (int num3 = 0; num3 != this.defHash.Length; num3++)
			{
				byte[] array4 = array;
				int num4 = num3;
				array4[num4] ^= array2[num3];
			}
			return this.engine.ProcessBlock(array, 0, array.Length);
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000E107C File Offset: 0x000DF27C
		private byte[] DecodeBlock(byte[] inBytes, int inOff, int inLen)
		{
			byte[] array = this.engine.ProcessBlock(inBytes, inOff, inLen);
			byte[] array2;
			if (array.Length < this.engine.GetOutputBlockSize())
			{
				array2 = new byte[this.engine.GetOutputBlockSize()];
				Array.Copy(array, 0, array2, array2.Length - array.Length, array.Length);
			}
			else
			{
				array2 = array;
			}
			if (array2.Length < 2 * this.defHash.Length + 1)
			{
				throw new InvalidCipherTextException("data too short");
			}
			byte[] array3 = this.maskGeneratorFunction1(array2, this.defHash.Length, array2.Length - this.defHash.Length, this.defHash.Length);
			for (int num = 0; num != this.defHash.Length; num++)
			{
				byte[] array4 = array2;
				int num2 = num;
				array4[num2] ^= array3[num];
			}
			array3 = this.maskGeneratorFunction1(array2, 0, this.defHash.Length, array2.Length - this.defHash.Length);
			for (int num3 = this.defHash.Length; num3 != array2.Length; num3++)
			{
				byte[] array5 = array2;
				int num4 = num3;
				array5[num4] ^= array3[num3 - this.defHash.Length];
			}
			int num5 = 0;
			for (int i = 0; i < this.defHash.Length; i++)
			{
				num5 |= (int)(this.defHash[i] ^ array2[this.defHash.Length + i]);
			}
			if (num5 != 0)
			{
				throw new InvalidCipherTextException("data hash wrong");
			}
			int num6 = 2 * this.defHash.Length;
			while (num6 != array2.Length && array2[num6] == 0)
			{
				num6++;
			}
			if (num6 > array2.Length - 1 || array2[num6] != 1)
			{
				throw new InvalidCipherTextException("data start wrong " + num6);
			}
			num6++;
			byte[] array6 = new byte[array2.Length - num6];
			Array.Copy(array2, num6, array6, 0, array6.Length);
			return array6;
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000C3797 File Offset: 0x000C1997
		private void ItoOSP(int i, byte[] sp)
		{
			sp[0] = (byte)((uint)i >> 24);
			sp[1] = (byte)((uint)i >> 16);
			sp[2] = (byte)((uint)i >> 8);
			sp[3] = (byte)i;
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000E1230 File Offset: 0x000DF430
		private byte[] maskGeneratorFunction1(byte[] Z, int zOff, int zLen, int length)
		{
			byte[] array = new byte[length];
			byte[] array2 = new byte[this.mgf1Hash.GetDigestSize()];
			byte[] array3 = new byte[4];
			int num = 0;
			this.hash.Reset();
			do
			{
				this.ItoOSP(num, array3);
				this.mgf1Hash.BlockUpdate(Z, zOff, zLen);
				this.mgf1Hash.BlockUpdate(array3, 0, array3.Length);
				this.mgf1Hash.DoFinal(array2, 0);
				Array.Copy(array2, 0, array, num * array2.Length, array2.Length);
			}
			while (++num < length / array2.Length);
			if (num * array2.Length < length)
			{
				this.ItoOSP(num, array3);
				this.mgf1Hash.BlockUpdate(Z, zOff, zLen);
				this.mgf1Hash.BlockUpdate(array3, 0, array3.Length);
				this.mgf1Hash.DoFinal(array2, 0);
				Array.Copy(array2, 0, array, num * array2.Length, array.Length - num * array2.Length);
			}
			return array;
		}

		// Token: 0x04001CDE RID: 7390
		private byte[] defHash;

		// Token: 0x04001CDF RID: 7391
		private IDigest hash;

		// Token: 0x04001CE0 RID: 7392
		private IDigest mgf1Hash;

		// Token: 0x04001CE1 RID: 7393
		private IAsymmetricBlockCipher engine;

		// Token: 0x04001CE2 RID: 7394
		private SecureRandom random;

		// Token: 0x04001CE3 RID: 7395
		private bool forEncryption;
	}
}

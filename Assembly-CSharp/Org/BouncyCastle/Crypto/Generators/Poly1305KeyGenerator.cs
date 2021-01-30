using System;

namespace Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200047B RID: 1147
	public class Poly1305KeyGenerator : CipherKeyGenerator
	{
		// Token: 0x060029B3 RID: 10675 RVA: 0x000CEB7C File Offset: 0x000CCD7C
		protected override void engineInit(KeyGenerationParameters param)
		{
			this.random = param.Random;
			this.strength = 32;
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x000CEB92 File Offset: 0x000CCD92
		protected override byte[] engineGenerateKey()
		{
			byte[] array = base.engineGenerateKey();
			Poly1305KeyGenerator.Clamp(array);
			return array;
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x000CEBA0 File Offset: 0x000CCDA0
		public static void Clamp(byte[] key)
		{
			if (key.Length != 32)
			{
				throw new ArgumentException("Poly1305 key must be 256 bits.");
			}
			int num = 3;
			key[num] &= 15;
			int num2 = 7;
			key[num2] &= 15;
			int num3 = 11;
			key[num3] &= 15;
			int num4 = 15;
			key[num4] &= 15;
			int num5 = 4;
			key[num5] &= 252;
			int num6 = 8;
			key[num6] &= 252;
			int num7 = 12;
			key[num7] &= 252;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x000CEC30 File Offset: 0x000CCE30
		public static void CheckKey(byte[] key)
		{
			if (key.Length != 32)
			{
				throw new ArgumentException("Poly1305 key must be 256 bits.");
			}
			Poly1305KeyGenerator.CheckMask(key[3], 15);
			Poly1305KeyGenerator.CheckMask(key[7], 15);
			Poly1305KeyGenerator.CheckMask(key[11], 15);
			Poly1305KeyGenerator.CheckMask(key[15], 15);
			Poly1305KeyGenerator.CheckMask(key[4], 252);
			Poly1305KeyGenerator.CheckMask(key[8], 252);
			Poly1305KeyGenerator.CheckMask(key[12], 252);
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000CECA1 File Offset: 0x000CCEA1
		private static void CheckMask(byte b, byte mask)
		{
			if ((b & ~(mask != 0)) != 0)
			{
				throw new ArgumentException("Invalid format for r portion of Poly1305 key.");
			}
		}

		// Token: 0x04001B6B RID: 7019
		private const byte R_MASK_LOW_2 = 252;

		// Token: 0x04001B6C RID: 7020
		private const byte R_MASK_HIGH_4 = 15;
	}
}

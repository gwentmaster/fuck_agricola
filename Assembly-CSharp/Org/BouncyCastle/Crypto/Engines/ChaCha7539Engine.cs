using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000485 RID: 1157
	public class ChaCha7539Engine : Salsa20Engine
	{
		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06002A22 RID: 10786 RVA: 0x000D4A9D File Offset: 0x000D2C9D
		public override string AlgorithmName
		{
			get
			{
				return "ChaCha" + this.rounds;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06002A23 RID: 10787 RVA: 0x000AE7BC File Offset: 0x000AC9BC
		protected override int NonceSize
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000D4AB4 File Offset: 0x000D2CB4
		protected override void AdvanceCounter()
		{
			uint[] engineState = this.engineState;
			int num = 12;
			uint num2 = engineState[num] + 1U;
			engineState[num] = num2;
			if (num2 == 0U)
			{
				throw new InvalidOperationException("attempt to increase counter past 2^32.");
			}
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000D4AE3 File Offset: 0x000D2CE3
		protected override void ResetCounter()
		{
			this.engineState[12] = 0U;
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000D4AF0 File Offset: 0x000D2CF0
		protected override void SetKey(byte[] keyBytes, byte[] ivBytes)
		{
			if (keyBytes != null)
			{
				if (keyBytes.Length != 32)
				{
					throw new ArgumentException(this.AlgorithmName + " requires 256 bit key");
				}
				base.PackTauOrSigma(keyBytes.Length, this.engineState, 0);
				Pack.LE_To_UInt32(keyBytes, 0, this.engineState, 4, 8);
			}
			Pack.LE_To_UInt32(ivBytes, 0, this.engineState, 13, 3);
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000D4B4C File Offset: 0x000D2D4C
		protected override void GenerateKeyStream(byte[] output)
		{
			ChaChaEngine.ChachaCore(this.rounds, this.engineState, this.x);
			Pack.UInt32_To_LE(this.x, output, 0);
		}
	}
}

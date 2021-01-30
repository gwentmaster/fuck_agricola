using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000487 RID: 1159
	public class DesEdeEngine : DesEngine
	{
		// Token: 0x06002A30 RID: 10800 RVA: 0x000D4FEC File Offset: 0x000D31EC
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to DESede init - " + Platform.GetTypeName(parameters));
			}
			byte[] key = ((KeyParameter)parameters).GetKey();
			if (key.Length != 24 && key.Length != 16)
			{
				throw new ArgumentException("key size must be 16 or 24 bytes.");
			}
			this.forEncryption = forEncryption;
			byte[] array = new byte[8];
			Array.Copy(key, 0, array, 0, array.Length);
			this.workingKey1 = DesEngine.GenerateWorkingKey(forEncryption, array);
			byte[] array2 = new byte[8];
			Array.Copy(key, 8, array2, 0, array2.Length);
			this.workingKey2 = DesEngine.GenerateWorkingKey(!forEncryption, array2);
			if (key.Length == 24)
			{
				byte[] array3 = new byte[8];
				Array.Copy(key, 16, array3, 0, array3.Length);
				this.workingKey3 = DesEngine.GenerateWorkingKey(forEncryption, array3);
				return;
			}
			this.workingKey3 = this.workingKey1;
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06002A31 RID: 10801 RVA: 0x000D50BB File Offset: 0x000D32BB
		public override string AlgorithmName
		{
			get
			{
				return "DESede";
			}
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public override int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000D50C4 File Offset: 0x000D32C4
		public override int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey1 == null)
			{
				throw new InvalidOperationException("DESede engine not initialised");
			}
			Check.DataLength(input, inOff, 8, "input buffer too short");
			Check.OutputLength(output, outOff, 8, "output buffer too short");
			byte[] array = new byte[8];
			if (this.forEncryption)
			{
				DesEngine.DesFunc(this.workingKey1, input, inOff, array, 0);
				DesEngine.DesFunc(this.workingKey2, array, 0, array, 0);
				DesEngine.DesFunc(this.workingKey3, array, 0, output, outOff);
			}
			else
			{
				DesEngine.DesFunc(this.workingKey3, input, inOff, array, 0);
				DesEngine.DesFunc(this.workingKey2, array, 0, array, 0);
				DesEngine.DesFunc(this.workingKey1, array, 0, output, outOff);
			}
			return 8;
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x00003022 File Offset: 0x00001222
		public override void Reset()
		{
		}

		// Token: 0x04001BD1 RID: 7121
		private int[] workingKey1;

		// Token: 0x04001BD2 RID: 7122
		private int[] workingKey2;

		// Token: 0x04001BD3 RID: 7123
		private int[] workingKey3;

		// Token: 0x04001BD4 RID: 7124
		private bool forEncryption;
	}
}

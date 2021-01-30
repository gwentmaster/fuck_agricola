using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000493 RID: 1171
	public class RC4Engine : IStreamCipher
	{
		// Token: 0x06002AB9 RID: 10937 RVA: 0x000D8AA2 File Offset: 0x000D6CA2
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is KeyParameter)
			{
				this.workingKey = ((KeyParameter)parameters).GetKey();
				this.SetKey(this.workingKey);
				return;
			}
			throw new ArgumentException("invalid parameter passed to RC4 init - " + Platform.GetTypeName(parameters));
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06002ABA RID: 10938 RVA: 0x000D8ADF File Offset: 0x000D6CDF
		public virtual string AlgorithmName
		{
			get
			{
				return "RC4";
			}
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000D8AE8 File Offset: 0x000D6CE8
		public virtual byte ReturnByte(byte input)
		{
			this.x = (this.x + 1 & 255);
			this.y = ((int)this.engineState[this.x] + this.y & 255);
			byte b = this.engineState[this.x];
			this.engineState[this.x] = this.engineState[this.y];
			this.engineState[this.y] = b;
			return input ^ this.engineState[(int)(this.engineState[this.x] + this.engineState[this.y] & byte.MaxValue)];
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000D8B8C File Offset: 0x000D6D8C
		public virtual void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			Check.DataLength(input, inOff, length, "input buffer too short");
			Check.OutputLength(output, outOff, length, "output buffer too short");
			for (int i = 0; i < length; i++)
			{
				this.x = (this.x + 1 & 255);
				this.y = ((int)this.engineState[this.x] + this.y & 255);
				byte b = this.engineState[this.x];
				this.engineState[this.x] = this.engineState[this.y];
				this.engineState[this.y] = b;
				output[i + outOff] = (input[i + inOff] ^ this.engineState[(int)(this.engineState[this.x] + this.engineState[this.y] & byte.MaxValue)]);
			}
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000D8C67 File Offset: 0x000D6E67
		public virtual void Reset()
		{
			this.SetKey(this.workingKey);
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000D8C78 File Offset: 0x000D6E78
		private void SetKey(byte[] keyBytes)
		{
			this.workingKey = keyBytes;
			this.x = 0;
			this.y = 0;
			if (this.engineState == null)
			{
				this.engineState = new byte[RC4Engine.STATE_LENGTH];
			}
			for (int i = 0; i < RC4Engine.STATE_LENGTH; i++)
			{
				this.engineState[i] = (byte)i;
			}
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < RC4Engine.STATE_LENGTH; j++)
			{
				num2 = ((int)((keyBytes[num] & byte.MaxValue) + this.engineState[j]) + num2 & 255);
				byte b = this.engineState[j];
				this.engineState[j] = this.engineState[num2];
				this.engineState[num2] = b;
				num = (num + 1) % keyBytes.Length;
			}
		}

		// Token: 0x04001C2F RID: 7215
		private static readonly int STATE_LENGTH = 256;

		// Token: 0x04001C30 RID: 7216
		private byte[] engineState;

		// Token: 0x04001C31 RID: 7217
		private int x;

		// Token: 0x04001C32 RID: 7218
		private int y;

		// Token: 0x04001C33 RID: 7219
		private byte[] workingKey;
	}
}

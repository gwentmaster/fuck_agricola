using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x020004A4 RID: 1188
	public class VmpcEngine : IStreamCipher
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06002B9A RID: 11162 RVA: 0x000E0137 File Offset: 0x000DE337
		public virtual string AlgorithmName
		{
			get
			{
				return "VMPC";
			}
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000E0140 File Offset: 0x000DE340
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is ParametersWithIV))
			{
				throw new ArgumentException("VMPC Init parameters must include an IV");
			}
			ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
			if (!(parametersWithIV.Parameters is KeyParameter))
			{
				throw new ArgumentException("VMPC Init parameters must include a key");
			}
			KeyParameter keyParameter = (KeyParameter)parametersWithIV.Parameters;
			this.workingIV = parametersWithIV.GetIV();
			if (this.workingIV == null || this.workingIV.Length < 1 || this.workingIV.Length > 768)
			{
				throw new ArgumentException("VMPC requires 1 to 768 bytes of IV");
			}
			this.workingKey = keyParameter.GetKey();
			this.InitKey(this.workingKey, this.workingIV);
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000E01E4 File Offset: 0x000DE3E4
		protected virtual void InitKey(byte[] keyBytes, byte[] ivBytes)
		{
			this.s = 0;
			this.P = new byte[256];
			for (int i = 0; i < 256; i++)
			{
				this.P[i] = (byte)i;
			}
			for (int j = 0; j < 768; j++)
			{
				this.s = this.P[(int)(this.s + this.P[j & 255] + keyBytes[j % keyBytes.Length] & byte.MaxValue)];
				byte b = this.P[j & 255];
				this.P[j & 255] = this.P[(int)(this.s & byte.MaxValue)];
				this.P[(int)(this.s & byte.MaxValue)] = b;
			}
			for (int k = 0; k < 768; k++)
			{
				this.s = this.P[(int)(this.s + this.P[k & 255] + ivBytes[k % ivBytes.Length] & byte.MaxValue)];
				byte b2 = this.P[k & 255];
				this.P[k & 255] = this.P[(int)(this.s & byte.MaxValue)];
				this.P[(int)(this.s & byte.MaxValue)] = b2;
			}
			this.n = 0;
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000E0338 File Offset: 0x000DE538
		public virtual void ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			Check.DataLength(input, inOff, len, "input buffer too short");
			Check.OutputLength(output, outOff, len, "output buffer too short");
			for (int i = 0; i < len; i++)
			{
				this.s = this.P[(int)(this.s + this.P[(int)(this.n & byte.MaxValue)] & byte.MaxValue)];
				byte b = this.P[(int)(this.P[(int)(this.P[(int)(this.s & byte.MaxValue)] & byte.MaxValue)] + 1 & byte.MaxValue)];
				byte b2 = this.P[(int)(this.n & byte.MaxValue)];
				this.P[(int)(this.n & byte.MaxValue)] = this.P[(int)(this.s & byte.MaxValue)];
				this.P[(int)(this.s & byte.MaxValue)] = b2;
				this.n = (this.n + 1 & byte.MaxValue);
				output[i + outOff] = (input[i + inOff] ^ b);
			}
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x000E0442 File Offset: 0x000DE642
		public virtual void Reset()
		{
			this.InitKey(this.workingKey, this.workingIV);
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000E0458 File Offset: 0x000DE658
		public virtual byte ReturnByte(byte input)
		{
			this.s = this.P[(int)(this.s + this.P[(int)(this.n & byte.MaxValue)] & byte.MaxValue)];
			byte b = this.P[(int)(this.P[(int)(this.P[(int)(this.s & byte.MaxValue)] & byte.MaxValue)] + 1 & byte.MaxValue)];
			byte b2 = this.P[(int)(this.n & byte.MaxValue)];
			this.P[(int)(this.n & byte.MaxValue)] = this.P[(int)(this.s & byte.MaxValue)];
			this.P[(int)(this.s & byte.MaxValue)] = b2;
			this.n = (this.n + 1 & byte.MaxValue);
			return input ^ b;
		}

		// Token: 0x04001CC8 RID: 7368
		protected byte n;

		// Token: 0x04001CC9 RID: 7369
		protected byte[] P;

		// Token: 0x04001CCA RID: 7370
		protected byte s;

		// Token: 0x04001CCB RID: 7371
		protected byte[] workingIV;

		// Token: 0x04001CCC RID: 7372
		protected byte[] workingKey;
	}
}

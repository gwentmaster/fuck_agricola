using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000459 RID: 1113
	public class CbcBlockCipher : IBlockCipher
	{
		// Token: 0x06002856 RID: 10326 RVA: 0x000C71A4 File Offset: 0x000C53A4
		public CbcBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			this.IV = new byte[this.blockSize];
			this.cbcV = new byte[this.blockSize];
			this.cbcNextV = new byte[this.blockSize];
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x000C71FD File Offset: 0x000C53FD
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000C7208 File Offset: 0x000C5408
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			bool flag = this.encrypting;
			this.encrypting = forEncryption;
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				if (iv.Length != this.blockSize)
				{
					throw new ArgumentException("initialisation vector must be the same length as block size");
				}
				Array.Copy(iv, 0, this.IV, 0, iv.Length);
				parameters = parametersWithIV.Parameters;
			}
			this.Reset();
			if (parameters != null)
			{
				this.cipher.Init(this.encrypting, parameters);
				return;
			}
			if (flag != this.encrypting)
			{
				throw new ArgumentException("cannot change encrypting state without providing key.");
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06002859 RID: 10329 RVA: 0x000C7296 File Offset: 0x000C5496
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/CBC";
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x0600285A RID: 10330 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x000C72AD File Offset: 0x000C54AD
		public int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000C72BA File Offset: 0x000C54BA
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.encrypting)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000C72DB File Offset: 0x000C54DB
		public void Reset()
		{
			Array.Copy(this.IV, 0, this.cbcV, 0, this.IV.Length);
			Array.Clear(this.cbcNextV, 0, this.cbcNextV.Length);
			this.cipher.Reset();
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000C7318 File Offset: 0x000C5518
		private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			for (int i = 0; i < this.blockSize; i++)
			{
				byte[] array = this.cbcV;
				int num = i;
				array[num] ^= input[inOff + i];
			}
			int result = this.cipher.ProcessBlock(this.cbcV, 0, outBytes, outOff);
			Array.Copy(outBytes, outOff, this.cbcV, 0, this.cbcV.Length);
			return result;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000C7390 File Offset: 0x000C5590
		private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			Array.Copy(input, inOff, this.cbcNextV, 0, this.blockSize);
			int result = this.cipher.ProcessBlock(input, inOff, outBytes, outOff);
			for (int i = 0; i < this.blockSize; i++)
			{
				int num = outOff + i;
				outBytes[num] ^= this.cbcV[i];
			}
			byte[] array = this.cbcV;
			this.cbcV = this.cbcNextV;
			this.cbcNextV = array;
			return result;
		}

		// Token: 0x04001A8A RID: 6794
		private byte[] IV;

		// Token: 0x04001A8B RID: 6795
		private byte[] cbcV;

		// Token: 0x04001A8C RID: 6796
		private byte[] cbcNextV;

		// Token: 0x04001A8D RID: 6797
		private int blockSize;

		// Token: 0x04001A8E RID: 6798
		private IBlockCipher cipher;

		// Token: 0x04001A8F RID: 6799
		private bool encrypting;
	}
}

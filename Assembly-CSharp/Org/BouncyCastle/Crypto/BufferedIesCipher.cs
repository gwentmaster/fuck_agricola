using System;
using System.IO;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200036D RID: 877
	public class BufferedIesCipher : BufferedCipherBase
	{
		// Token: 0x060021AC RID: 8620 RVA: 0x000B4E40 File Offset: 0x000B3040
		public BufferedIesCipher(IesEngine engine)
		{
			if (engine == null)
			{
				throw new ArgumentNullException("engine");
			}
			this.engine = engine;
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x000B4E68 File Offset: 0x000B3068
		public override string AlgorithmName
		{
			get
			{
				return "IES";
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000B4E6F File Offset: 0x000B306F
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			throw Platform.CreateNotImplementedException("IES");
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x0002A062 File Offset: 0x00028262
		public override int GetBlockSize()
		{
			return 0;
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000B4E84 File Offset: 0x000B3084
		public override int GetOutputSize(int inputLen)
		{
			if (this.engine == null)
			{
				throw new InvalidOperationException("cipher not initialised");
			}
			int num = inputLen + (int)this.buffer.Length;
			if (!this.forEncryption)
			{
				return num - 20;
			}
			return num + 20;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0002A062 File Offset: 0x00028262
		public override int GetUpdateOutputSize(int inputLen)
		{
			return 0;
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000B4EC4 File Offset: 0x000B30C4
		public override byte[] ProcessByte(byte input)
		{
			this.buffer.WriteByte(input);
			return null;
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000B4ED4 File Offset: 0x000B30D4
		public override byte[] ProcessBytes(byte[] input, int inOff, int length)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (inOff < 0)
			{
				throw new ArgumentException("inOff");
			}
			if (length < 0)
			{
				throw new ArgumentException("length");
			}
			if (inOff + length > input.Length)
			{
				throw new ArgumentException("invalid offset/length specified for input array");
			}
			this.buffer.Write(input, inOff, length);
			return null;
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x000B4F30 File Offset: 0x000B3130
		public override byte[] DoFinal()
		{
			byte[] array = this.buffer.ToArray();
			this.Reset();
			return this.engine.ProcessBlock(array, 0, array.Length);
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000B486D File Offset: 0x000B2A6D
		public override byte[] DoFinal(byte[] input, int inOff, int length)
		{
			this.ProcessBytes(input, inOff, length);
			return this.DoFinal();
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000B4F5F File Offset: 0x000B315F
		public override void Reset()
		{
			this.buffer.SetLength(0L);
		}

		// Token: 0x04001684 RID: 5764
		private readonly IesEngine engine;

		// Token: 0x04001685 RID: 5765
		private bool forEncryption;

		// Token: 0x04001686 RID: 5766
		private MemoryStream buffer = new MemoryStream();
	}
}

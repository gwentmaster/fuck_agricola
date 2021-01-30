using System;
using System.IO;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x020002A5 RID: 677
	public class PemWriter
	{
		// Token: 0x0600166F RID: 5743 RVA: 0x00080AEE File Offset: 0x0007ECEE
		public PemWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.nlLength = Platform.NewLine.Length;
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x00080B28 File Offset: 0x0007ED28
		public TextWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00080B30 File Offset: 0x0007ED30
		public int GetOutputSize(PemObject obj)
		{
			int num = 2 * (obj.Type.Length + 10 + this.nlLength) + 6 + 4;
			if (obj.Headers.Count > 0)
			{
				foreach (object obj2 in obj.Headers)
				{
					PemHeader pemHeader = (PemHeader)obj2;
					num += pemHeader.Name.Length + ": ".Length + pemHeader.Value.Length + this.nlLength;
				}
				num += this.nlLength;
			}
			int num2 = (obj.Content.Length + 2) / 3 * 4;
			num += num2 + (num2 + 64 - 1) / 64 * this.nlLength;
			return num;
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00080C0C File Offset: 0x0007EE0C
		public void WriteObject(PemObjectGenerator objGen)
		{
			PemObject pemObject = objGen.Generate();
			this.WritePreEncapsulationBoundary(pemObject.Type);
			if (pemObject.Headers.Count > 0)
			{
				foreach (object obj in pemObject.Headers)
				{
					PemHeader pemHeader = (PemHeader)obj;
					this.writer.Write(pemHeader.Name);
					this.writer.Write(": ");
					this.writer.WriteLine(pemHeader.Value);
				}
				this.writer.WriteLine();
			}
			this.WriteEncoded(pemObject.Content);
			this.WritePostEncapsulationBoundary(pemObject.Type);
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00080CD4 File Offset: 0x0007EED4
		private void WriteEncoded(byte[] bytes)
		{
			bytes = Base64.Encode(bytes);
			for (int i = 0; i < bytes.Length; i += this.buf.Length)
			{
				int num = 0;
				while (num != this.buf.Length && i + num < bytes.Length)
				{
					this.buf[num] = (char)bytes[i + num];
					num++;
				}
				this.writer.WriteLine(this.buf, 0, num);
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00080D39 File Offset: 0x0007EF39
		private void WritePreEncapsulationBoundary(string type)
		{
			this.writer.WriteLine("-----BEGIN " + type + "-----");
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00080D56 File Offset: 0x0007EF56
		private void WritePostEncapsulationBoundary(string type)
		{
			this.writer.WriteLine("-----END " + type + "-----");
		}

		// Token: 0x0400150F RID: 5391
		private const int LineLength = 64;

		// Token: 0x04001510 RID: 5392
		private readonly TextWriter writer;

		// Token: 0x04001511 RID: 5393
		private readonly int nlLength;

		// Token: 0x04001512 RID: 5394
		private char[] buf = new char[64];
	}
}

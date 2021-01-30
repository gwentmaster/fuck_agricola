using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020004B2 RID: 1202
	public class NullDigest : IDigest
	{
		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06002C7B RID: 11387 RVA: 0x000E566D File Offset: 0x000E386D
		public string AlgorithmName
		{
			get
			{
				return "NULL";
			}
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x0002A062 File Offset: 0x00028262
		public int GetByteLength()
		{
			return 0;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x000E5674 File Offset: 0x000E3874
		public int GetDigestSize()
		{
			return (int)this.bOut.Length;
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x000E5682 File Offset: 0x000E3882
		public void Update(byte b)
		{
			this.bOut.WriteByte(b);
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x000E5690 File Offset: 0x000E3890
		public void BlockUpdate(byte[] inBytes, int inOff, int len)
		{
			this.bOut.Write(inBytes, inOff, len);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x000E56A0 File Offset: 0x000E38A0
		public int DoFinal(byte[] outBytes, int outOff)
		{
			byte[] array = this.bOut.ToArray();
			array.CopyTo(outBytes, outOff);
			this.Reset();
			return array.Length;
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x000E56BD File Offset: 0x000E38BD
		public void Reset()
		{
			this.bOut.SetLength(0L);
		}

		// Token: 0x04001D5C RID: 7516
		private readonly MemoryStream bOut = new MemoryStream();
	}
}

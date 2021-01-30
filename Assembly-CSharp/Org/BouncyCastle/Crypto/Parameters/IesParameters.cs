using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000438 RID: 1080
	public class IesParameters : ICipherParameters
	{
		// Token: 0x060027BC RID: 10172 RVA: 0x000C5A2D File Offset: 0x000C3C2D
		public IesParameters(byte[] derivation, byte[] encoding, int macKeySize)
		{
			this.derivation = derivation;
			this.encoding = encoding;
			this.macKeySize = macKeySize;
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000C5A4A File Offset: 0x000C3C4A
		public byte[] GetDerivationV()
		{
			return this.derivation;
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000C5A52 File Offset: 0x000C3C52
		public byte[] GetEncodingV()
		{
			return this.encoding;
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x000C5A5A File Offset: 0x000C3C5A
		public int MacKeySize
		{
			get
			{
				return this.macKeySize;
			}
		}

		// Token: 0x04001A53 RID: 6739
		private byte[] derivation;

		// Token: 0x04001A54 RID: 6740
		private byte[] encoding;

		// Token: 0x04001A55 RID: 6741
		private int macKeySize;
	}
}

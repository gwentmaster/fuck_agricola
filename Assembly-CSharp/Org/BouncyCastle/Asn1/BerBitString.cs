using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D6 RID: 1238
	public class BerBitString : DerBitString
	{
		// Token: 0x06002DED RID: 11757 RVA: 0x000F0A59 File Offset: 0x000EEC59
		public BerBitString(byte[] data, int padBits) : base(data, padBits)
		{
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000F0A63 File Offset: 0x000EEC63
		public BerBitString(byte[] data) : base(data)
		{
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000F0A6C File Offset: 0x000EEC6C
		public BerBitString(int namedBits) : base(namedBits)
		{
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000F0A75 File Offset: 0x000EEC75
		public BerBitString(Asn1Encodable obj) : base(obj)
		{
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000F0A7E File Offset: 0x000EEC7E
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteEncoded(3, (byte)this.mPadBits, this.mData);
				return;
			}
			base.Encode(derOut);
		}
	}
}

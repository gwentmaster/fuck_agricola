using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x0200050E RID: 1294
	internal class LazyDerSequence : DerSequence
	{
		// Token: 0x06002F74 RID: 12148 RVA: 0x000F4953 File Offset: 0x000F2B53
		internal LazyDerSequence(byte[] encoded)
		{
			this.encoded = encoded;
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000F4964 File Offset: 0x000F2B64
		private void Parse()
		{
			lock (this)
			{
				if (this.encoded != null)
				{
					Asn1InputStream asn1InputStream = new LazyAsn1InputStream(this.encoded);
					Asn1Object obj;
					while ((obj = asn1InputStream.ReadObject()) != null)
					{
						base.AddObject(obj);
					}
					this.encoded = null;
				}
			}
		}

		// Token: 0x17000552 RID: 1362
		public override Asn1Encodable this[int index]
		{
			get
			{
				this.Parse();
				return base[index];
			}
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000F49D7 File Offset: 0x000F2BD7
		public override IEnumerator GetEnumerator()
		{
			this.Parse();
			return base.GetEnumerator();
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06002F78 RID: 12152 RVA: 0x000F49E5 File Offset: 0x000F2BE5
		public override int Count
		{
			get
			{
				this.Parse();
				return base.Count;
			}
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000F49F4 File Offset: 0x000F2BF4
		internal override void Encode(DerOutputStream derOut)
		{
			lock (this)
			{
				if (this.encoded == null)
				{
					base.Encode(derOut);
				}
				else
				{
					derOut.WriteEncoded(48, this.encoded);
				}
			}
		}

		// Token: 0x04001E4E RID: 7758
		private byte[] encoded;
	}
}

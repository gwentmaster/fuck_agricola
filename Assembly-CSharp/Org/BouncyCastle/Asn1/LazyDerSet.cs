using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x0200050F RID: 1295
	internal class LazyDerSet : DerSet
	{
		// Token: 0x06002F7A RID: 12154 RVA: 0x000F4A48 File Offset: 0x000F2C48
		internal LazyDerSet(byte[] encoded)
		{
			this.encoded = encoded;
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000F4A58 File Offset: 0x000F2C58
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

		// Token: 0x17000554 RID: 1364
		public override Asn1Encodable this[int index]
		{
			get
			{
				this.Parse();
				return base[index];
			}
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000F4ACB File Offset: 0x000F2CCB
		public override IEnumerator GetEnumerator()
		{
			this.Parse();
			return base.GetEnumerator();
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06002F7E RID: 12158 RVA: 0x000F4AD9 File Offset: 0x000F2CD9
		public override int Count
		{
			get
			{
				this.Parse();
				return base.Count;
			}
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x000F4AE8 File Offset: 0x000F2CE8
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
					derOut.WriteEncoded(49, this.encoded);
				}
			}
		}

		// Token: 0x04001E4F RID: 7759
		private byte[] encoded;
	}
}

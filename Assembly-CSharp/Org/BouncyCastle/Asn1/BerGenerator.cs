using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D7 RID: 1239
	public class BerGenerator : Asn1Generator
	{
		// Token: 0x06002DF2 RID: 11762 RVA: 0x000F0AAC File Offset: 0x000EECAC
		protected BerGenerator(Stream outStream) : base(outStream)
		{
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000F0AB5 File Offset: 0x000EECB5
		public BerGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream)
		{
			this._tagged = true;
			this._isExplicit = isExplicit;
			this._tagNo = tagNo;
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000F0AD3 File Offset: 0x000EECD3
		public override void AddObject(Asn1Encodable obj)
		{
			new BerOutputStream(base.Out).WriteObject(obj);
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000F0AE6 File Offset: 0x000EECE6
		public override Stream GetRawOutputStream()
		{
			return base.Out;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000F0AEE File Offset: 0x000EECEE
		public override void Close()
		{
			this.WriteBerEnd();
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000F0AF6 File Offset: 0x000EECF6
		private void WriteHdr(int tag)
		{
			base.Out.WriteByte((byte)tag);
			base.Out.WriteByte(128);
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000F0B18 File Offset: 0x000EED18
		protected void WriteBerHeader(int tag)
		{
			if (!this._tagged)
			{
				this.WriteHdr(tag);
				return;
			}
			int num = this._tagNo | 128;
			if (this._isExplicit)
			{
				this.WriteHdr(num | 32);
				this.WriteHdr(tag);
				return;
			}
			if ((tag & 32) != 0)
			{
				this.WriteHdr(num | 32);
				return;
			}
			this.WriteHdr(num);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000F0B74 File Offset: 0x000EED74
		protected void WriteBerBody(Stream contentStream)
		{
			Streams.PipeAll(contentStream, base.Out);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000F0B84 File Offset: 0x000EED84
		protected void WriteBerEnd()
		{
			base.Out.WriteByte(0);
			base.Out.WriteByte(0);
			if (this._tagged && this._isExplicit)
			{
				base.Out.WriteByte(0);
				base.Out.WriteByte(0);
			}
		}

		// Token: 0x04001E08 RID: 7688
		private bool _tagged;

		// Token: 0x04001E09 RID: 7689
		private bool _isExplicit;

		// Token: 0x04001E0A RID: 7690
		private int _tagNo;
	}
}

using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E5 RID: 1253
	internal class ConstructedOctetStream : BaseInputStream
	{
		// Token: 0x06002E32 RID: 11826 RVA: 0x000F1390 File Offset: 0x000EF590
		internal ConstructedOctetStream(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000F13A8 File Offset: 0x000EF5A8
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._currentStream == null)
			{
				if (!this._first)
				{
					return 0;
				}
				Asn1OctetStringParser asn1OctetStringParser = (Asn1OctetStringParser)this._parser.ReadObject();
				if (asn1OctetStringParser == null)
				{
					return 0;
				}
				this._first = false;
				this._currentStream = asn1OctetStringParser.GetOctetStream();
			}
			int num = 0;
			for (;;)
			{
				int num2 = this._currentStream.Read(buffer, offset + num, count - num);
				if (num2 > 0)
				{
					num += num2;
					if (num == count)
					{
						break;
					}
				}
				else
				{
					Asn1OctetStringParser asn1OctetStringParser2 = (Asn1OctetStringParser)this._parser.ReadObject();
					if (asn1OctetStringParser2 == null)
					{
						goto Block_6;
					}
					this._currentStream = asn1OctetStringParser2.GetOctetStream();
				}
			}
			return num;
			Block_6:
			this._currentStream = null;
			return num;
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000F1440 File Offset: 0x000EF640
		public override int ReadByte()
		{
			if (this._currentStream == null)
			{
				if (!this._first)
				{
					return 0;
				}
				Asn1OctetStringParser asn1OctetStringParser = (Asn1OctetStringParser)this._parser.ReadObject();
				if (asn1OctetStringParser == null)
				{
					return 0;
				}
				this._first = false;
				this._currentStream = asn1OctetStringParser.GetOctetStream();
			}
			int num;
			for (;;)
			{
				num = this._currentStream.ReadByte();
				if (num >= 0)
				{
					break;
				}
				Asn1OctetStringParser asn1OctetStringParser2 = (Asn1OctetStringParser)this._parser.ReadObject();
				if (asn1OctetStringParser2 == null)
				{
					goto Block_5;
				}
				this._currentStream = asn1OctetStringParser2.GetOctetStream();
			}
			return num;
			Block_5:
			this._currentStream = null;
			return -1;
		}

		// Token: 0x04001E17 RID: 7703
		private readonly Asn1StreamParser _parser;

		// Token: 0x04001E18 RID: 7704
		private bool _first = true;

		// Token: 0x04001E19 RID: 7705
		private Stream _currentStream;
	}
}

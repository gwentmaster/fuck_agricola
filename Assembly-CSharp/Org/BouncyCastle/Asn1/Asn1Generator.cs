using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004C3 RID: 1219
	public abstract class Asn1Generator
	{
		// Token: 0x06002D71 RID: 11633 RVA: 0x000EF3B9 File Offset: 0x000ED5B9
		protected Asn1Generator(Stream outStream)
		{
			this._out = outStream;
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002D72 RID: 11634 RVA: 0x000EF3C8 File Offset: 0x000ED5C8
		protected Stream Out
		{
			get
			{
				return this._out;
			}
		}

		// Token: 0x06002D73 RID: 11635
		public abstract void AddObject(Asn1Encodable obj);

		// Token: 0x06002D74 RID: 11636
		public abstract Stream GetRawOutputStream();

		// Token: 0x06002D75 RID: 11637
		public abstract void Close();

		// Token: 0x04001DDD RID: 7645
		private Stream _out;
	}
}

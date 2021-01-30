using System;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004FE RID: 1278
	public class DerSequence : Asn1Sequence
	{
		// Token: 0x06002F17 RID: 12055 RVA: 0x000F3C50 File Offset: 0x000F1E50
		public static DerSequence FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new DerSequence(v);
			}
			return DerSequence.Empty;
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x000F3C67 File Offset: 0x000F1E67
		public DerSequence() : base(0)
		{
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x000F3C70 File Offset: 0x000F1E70
		public DerSequence(Asn1Encodable obj) : base(1)
		{
			base.AddObject(obj);
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x000F3C80 File Offset: 0x000F1E80
		public DerSequence(params Asn1Encodable[] v) : base(v.Length)
		{
			foreach (Asn1Encodable obj in v)
			{
				base.AddObject(obj);
			}
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x000F3CB4 File Offset: 0x000F1EB4
		public DerSequence(Asn1EncodableVector v) : base(v.Count)
		{
			foreach (object obj in v)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				base.AddObject(obj2);
			}
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000F3D14 File Offset: 0x000F1F14
		internal override void Encode(DerOutputStream derOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			DerOutputStream derOutputStream = new DerOutputStream(memoryStream);
			foreach (object obj in this)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				derOutputStream.WriteObject(obj2);
			}
			Platform.Dispose(derOutputStream);
			byte[] bytes = memoryStream.ToArray();
			derOut.WriteEncoded(48, bytes);
		}

		// Token: 0x04001E43 RID: 7747
		public static readonly DerSequence Empty = new DerSequence();
	}
}

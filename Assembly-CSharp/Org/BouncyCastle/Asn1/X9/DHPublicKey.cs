using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000513 RID: 1299
	public class DHPublicKey : Asn1Encodable
	{
		// Token: 0x06002F91 RID: 12177 RVA: 0x000F4E34 File Offset: 0x000F3034
		public static DHPublicKey GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return DHPublicKey.GetInstance(DerInteger.GetInstance(obj, isExplicit));
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000F4E44 File Offset: 0x000F3044
		public static DHPublicKey GetInstance(object obj)
		{
			if (obj == null || obj is DHPublicKey)
			{
				return (DHPublicKey)obj;
			}
			if (obj is DerInteger)
			{
				return new DHPublicKey((DerInteger)obj);
			}
			throw new ArgumentException("Invalid DHPublicKey: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000F4E91 File Offset: 0x000F3091
		public DHPublicKey(DerInteger y)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			this.y = y;
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06002F94 RID: 12180 RVA: 0x000F4EAE File Offset: 0x000F30AE
		public DerInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000F4EAE File Offset: 0x000F30AE
		public override Asn1Object ToAsn1Object()
		{
			return this.y;
		}

		// Token: 0x04001E59 RID: 7769
		private readonly DerInteger y;
	}
}

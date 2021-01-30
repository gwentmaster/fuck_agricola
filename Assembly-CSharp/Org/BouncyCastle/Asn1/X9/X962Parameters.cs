using System;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000517 RID: 1303
	public class X962Parameters : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06002FAB RID: 12203 RVA: 0x000F53AC File Offset: 0x000F35AC
		public static X962Parameters GetInstance(object obj)
		{
			if (obj == null || obj is X962Parameters)
			{
				return (X962Parameters)obj;
			}
			if (obj is Asn1Object)
			{
				return new X962Parameters((Asn1Object)obj);
			}
			if (obj is byte[])
			{
				try
				{
					return new X962Parameters(Asn1Object.FromByteArray((byte[])obj));
				}
				catch (Exception ex)
				{
					throw new ArgumentException("unable to parse encoded data: " + ex.Message, ex);
				}
			}
			throw new ArgumentException("unknown object in getInstance()");
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000F5430 File Offset: 0x000F3630
		public X962Parameters(X9ECParameters ecParameters)
		{
			this._params = ecParameters.ToAsn1Object();
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x000F5444 File Offset: 0x000F3644
		public X962Parameters(DerObjectIdentifier namedCurve)
		{
			this._params = namedCurve;
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000F5444 File Offset: 0x000F3644
		public X962Parameters(Asn1Object obj)
		{
			this._params = obj;
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06002FAF RID: 12207 RVA: 0x000F5453 File Offset: 0x000F3653
		public bool IsNamedCurve
		{
			get
			{
				return this._params is DerObjectIdentifier;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06002FB0 RID: 12208 RVA: 0x000F5463 File Offset: 0x000F3663
		public bool IsImplicitlyCA
		{
			get
			{
				return this._params is Asn1Null;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06002FB1 RID: 12209 RVA: 0x000F5473 File Offset: 0x000F3673
		public Asn1Object Parameters
		{
			get
			{
				return this._params;
			}
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000F5473 File Offset: 0x000F3673
		public override Asn1Object ToAsn1Object()
		{
			return this._params;
		}

		// Token: 0x04001E5F RID: 7775
		private readonly Asn1Object _params;
	}
}

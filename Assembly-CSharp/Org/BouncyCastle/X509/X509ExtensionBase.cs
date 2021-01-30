using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.X509
{
	// Token: 0x02000280 RID: 640
	public abstract class X509ExtensionBase : IX509Extension
	{
		// Token: 0x06001515 RID: 5397
		protected abstract X509Extensions GetX509Extensions();

		// Token: 0x06001516 RID: 5398 RVA: 0x00078AB0 File Offset: 0x00076CB0
		protected virtual ISet GetExtensionOids(bool critical)
		{
			X509Extensions x509Extensions = this.GetX509Extensions();
			if (x509Extensions != null)
			{
				HashSet hashSet = new HashSet();
				foreach (object obj in x509Extensions.ExtensionOids)
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
					if (x509Extensions.GetExtension(derObjectIdentifier).IsCritical == critical)
					{
						hashSet.Add(derObjectIdentifier.Id);
					}
				}
				return hashSet;
			}
			return null;
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00078B34 File Offset: 0x00076D34
		public virtual ISet GetNonCriticalExtensionOids()
		{
			return this.GetExtensionOids(false);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00078B3D File Offset: 0x00076D3D
		public virtual ISet GetCriticalExtensionOids()
		{
			return this.GetExtensionOids(true);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00078B46 File Offset: 0x00076D46
		[Obsolete("Use version taking a DerObjectIdentifier instead")]
		public Asn1OctetString GetExtensionValue(string oid)
		{
			return this.GetExtensionValue(new DerObjectIdentifier(oid));
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00078B54 File Offset: 0x00076D54
		public virtual Asn1OctetString GetExtensionValue(DerObjectIdentifier oid)
		{
			X509Extensions x509Extensions = this.GetX509Extensions();
			if (x509Extensions != null)
			{
				X509Extension extension = x509Extensions.GetExtension(oid);
				if (extension != null)
				{
					return extension.Value;
				}
			}
			return null;
		}
	}
}

using System;
using System.Collections;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509.Extension;

namespace Org.BouncyCastle.X509
{
	// Token: 0x0200027E RID: 638
	public class X509CrlEntry : X509ExtensionBase
	{
		// Token: 0x06001500 RID: 5376 RVA: 0x000784F8 File Offset: 0x000766F8
		public X509CrlEntry(CrlEntry c)
		{
			this.c = c;
			this.certificateIssuer = this.loadCertificateIssuer();
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00078513 File Offset: 0x00076713
		public X509CrlEntry(CrlEntry c, bool isIndirect, X509Name previousCertificateIssuer)
		{
			this.c = c;
			this.isIndirect = isIndirect;
			this.previousCertificateIssuer = previousCertificateIssuer;
			this.certificateIssuer = this.loadCertificateIssuer();
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0007853C File Offset: 0x0007673C
		private X509Name loadCertificateIssuer()
		{
			if (!this.isIndirect)
			{
				return null;
			}
			Asn1OctetString extensionValue = this.GetExtensionValue(X509Extensions.CertificateIssuer);
			if (extensionValue == null)
			{
				return this.previousCertificateIssuer;
			}
			try
			{
				GeneralName[] names = GeneralNames.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue)).GetNames();
				for (int i = 0; i < names.Length; i++)
				{
					if (names[i].TagNo == 4)
					{
						return X509Name.GetInstance(names[i].Name);
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x000785BC File Offset: 0x000767BC
		public X509Name GetCertificateIssuer()
		{
			return this.certificateIssuer;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000785C4 File Offset: 0x000767C4
		protected override X509Extensions GetX509Extensions()
		{
			return this.c.Extensions;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000785D4 File Offset: 0x000767D4
		public byte[] GetEncoded()
		{
			byte[] derEncoded;
			try
			{
				derEncoded = this.c.GetDerEncoded();
			}
			catch (Exception ex)
			{
				throw new CrlException(ex.ToString());
			}
			return derEncoded;
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x0007860C File Offset: 0x0007680C
		public BigInteger SerialNumber
		{
			get
			{
				return this.c.UserCertificate.Value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0007861E File Offset: 0x0007681E
		public DateTime RevocationDate
		{
			get
			{
				return this.c.RevocationDate.ToDateTime();
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x00078630 File Offset: 0x00076830
		public bool HasExtensions
		{
			get
			{
				return this.c.Extensions != null;
			}
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00078640 File Offset: 0x00076840
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("        userCertificate: ").Append(this.SerialNumber).Append(newLine);
			stringBuilder.Append("         revocationDate: ").Append(this.RevocationDate).Append(newLine);
			stringBuilder.Append("      certificateIssuer: ").Append(this.GetCertificateIssuer()).Append(newLine);
			X509Extensions extensions = this.c.Extensions;
			if (extensions != null)
			{
				IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
				if (enumerator.MoveNext())
				{
					stringBuilder.Append("   crlEntryExtensions:").Append(newLine);
					for (;;)
					{
						DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)enumerator.Current;
						X509Extension extension = extensions.GetExtension(derObjectIdentifier);
						if (extension.Value != null)
						{
							Asn1Object asn1Object = Asn1Object.FromByteArray(extension.Value.GetOctets());
							stringBuilder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
							try
							{
								if (derObjectIdentifier.Equals(X509Extensions.ReasonCode))
								{
									stringBuilder.Append(new CrlReason(DerEnumerated.GetInstance(asn1Object)));
								}
								else if (derObjectIdentifier.Equals(X509Extensions.CertificateIssuer))
								{
									stringBuilder.Append("Certificate issuer: ").Append(GeneralNames.GetInstance((Asn1Sequence)asn1Object));
								}
								else
								{
									stringBuilder.Append(derObjectIdentifier.Id);
									stringBuilder.Append(" value = ").Append(Asn1Dump.DumpAsString(asn1Object));
								}
								stringBuilder.Append(newLine);
								goto IL_1B0;
							}
							catch (Exception)
							{
								stringBuilder.Append(derObjectIdentifier.Id);
								stringBuilder.Append(" value = ").Append("*****").Append(newLine);
								goto IL_1B0;
							}
							goto IL_1A8;
						}
						goto IL_1A8;
						IL_1B0:
						if (!enumerator.MoveNext())
						{
							break;
						}
						continue;
						IL_1A8:
						stringBuilder.Append(newLine);
						goto IL_1B0;
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400138A RID: 5002
		private CrlEntry c;

		// Token: 0x0400138B RID: 5003
		private bool isIndirect;

		// Token: 0x0400138C RID: 5004
		private X509Name previousCertificateIssuer;

		// Token: 0x0400138D RID: 5005
		private X509Name certificateIssuer;
	}
}

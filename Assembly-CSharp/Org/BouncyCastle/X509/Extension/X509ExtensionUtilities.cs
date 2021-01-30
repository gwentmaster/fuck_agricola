using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.X509.Extension
{
	// Token: 0x02000282 RID: 642
	public class X509ExtensionUtilities
	{
		// Token: 0x06001521 RID: 5409 RVA: 0x00078D01 File Offset: 0x00076F01
		public static Asn1Object FromExtensionValue(Asn1OctetString extensionValue)
		{
			return Asn1Object.FromByteArray(extensionValue.GetOctets());
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00078D0E File Offset: 0x00076F0E
		public static ICollection GetIssuerAlternativeNames(X509Certificate cert)
		{
			return X509ExtensionUtilities.GetAlternativeName(cert.GetExtensionValue(X509Extensions.IssuerAlternativeName));
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00078D20 File Offset: 0x00076F20
		public static ICollection GetSubjectAlternativeNames(X509Certificate cert)
		{
			return X509ExtensionUtilities.GetAlternativeName(cert.GetExtensionValue(X509Extensions.SubjectAlternativeName));
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00078D34 File Offset: 0x00076F34
		private static ICollection GetAlternativeName(Asn1OctetString extVal)
		{
			IList list = Platform.CreateArrayList();
			if (extVal != null)
			{
				try
				{
					foreach (object obj in Asn1Sequence.GetInstance(X509ExtensionUtilities.FromExtensionValue(extVal)))
					{
						GeneralName generalName = (GeneralName)obj;
						IList list2 = Platform.CreateArrayList();
						list2.Add(generalName.TagNo);
						switch (generalName.TagNo)
						{
						case 0:
						case 3:
						case 5:
							list2.Add(generalName.Name.ToAsn1Object());
							break;
						case 1:
						case 2:
						case 6:
							list2.Add(((IAsn1String)generalName.Name).GetString());
							break;
						case 4:
							list2.Add(X509Name.GetInstance(generalName.Name).ToString());
							break;
						case 7:
							list2.Add(Asn1OctetString.GetInstance(generalName.Name).GetOctets());
							break;
						case 8:
							list2.Add(DerObjectIdentifier.GetInstance(generalName.Name).Id);
							break;
						default:
							throw new IOException("Bad tag number: " + generalName.TagNo);
						}
						list.Add(list2);
					}
				}
				catch (Exception ex)
				{
					throw new CertificateParsingException(ex.Message);
				}
			}
			return list;
		}
	}
}

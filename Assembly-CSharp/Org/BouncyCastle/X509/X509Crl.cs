using System;
using System.Collections;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509.Extension;

namespace Org.BouncyCastle.X509
{
	// Token: 0x0200027D RID: 637
	public class X509Crl : X509ExtensionBase
	{
		// Token: 0x060014E9 RID: 5353 RVA: 0x00077CEC File Offset: 0x00075EEC
		public X509Crl(CertificateList c)
		{
			this.c = c;
			try
			{
				this.sigAlgName = X509SignatureUtilities.GetSignatureName(c.SignatureAlgorithm);
				if (c.SignatureAlgorithm.Parameters != null)
				{
					this.sigAlgParams = c.SignatureAlgorithm.Parameters.GetDerEncoded();
				}
				else
				{
					this.sigAlgParams = null;
				}
				this.isIndirect = this.IsIndirectCrl;
			}
			catch (Exception arg)
			{
				throw new CrlException("CRL contents invalid: " + arg);
			}
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00077D74 File Offset: 0x00075F74
		protected override X509Extensions GetX509Extensions()
		{
			if (this.c.Version < 2)
			{
				return null;
			}
			return this.c.TbsCertList.Extensions;
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00077D98 File Offset: 0x00075F98
		public virtual byte[] GetEncoded()
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

		// Token: 0x060014EC RID: 5356 RVA: 0x00077DD0 File Offset: 0x00075FD0
		public virtual void Verify(AsymmetricKeyParameter publicKey)
		{
			this.Verify(new Asn1VerifierFactoryProvider(publicKey));
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00077DDE File Offset: 0x00075FDE
		public virtual void Verify(IVerifierFactoryProvider verifierProvider)
		{
			this.CheckSignature(verifierProvider.CreateVerifierFactory(this.c.SignatureAlgorithm));
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00077DF8 File Offset: 0x00075FF8
		protected virtual void CheckSignature(IVerifierFactory verifier)
		{
			if (!this.c.SignatureAlgorithm.Equals(this.c.TbsCertList.Signature))
			{
				throw new CrlException("Signature algorithm on CertificateList does not match TbsCertList.");
			}
			IStreamCalculator streamCalculator = verifier.CreateCalculator();
			byte[] tbsCertList = this.GetTbsCertList();
			streamCalculator.Stream.Write(tbsCertList, 0, tbsCertList.Length);
			Platform.Dispose(streamCalculator.Stream);
			if (!((IVerifier)streamCalculator.GetResult()).IsVerified(this.GetSignature()))
			{
				throw new InvalidKeyException("CRL does not verify with supplied public key.");
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x00077E7C File Offset: 0x0007607C
		public virtual int Version
		{
			get
			{
				return this.c.Version;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x00077E89 File Offset: 0x00076089
		public virtual X509Name IssuerDN
		{
			get
			{
				return this.c.Issuer;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x00077E96 File Offset: 0x00076096
		public virtual DateTime ThisUpdate
		{
			get
			{
				return this.c.ThisUpdate.ToDateTime();
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00077EA8 File Offset: 0x000760A8
		public virtual DateTimeObject NextUpdate
		{
			get
			{
				if (this.c.NextUpdate != null)
				{
					return new DateTimeObject(this.c.NextUpdate.ToDateTime());
				}
				return null;
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00077ED0 File Offset: 0x000760D0
		private ISet LoadCrlEntries()
		{
			ISet set = new HashSet();
			IEnumerable revokedCertificateEnumeration = this.c.GetRevokedCertificateEnumeration();
			X509Name previousCertificateIssuer = this.IssuerDN;
			foreach (object obj in revokedCertificateEnumeration)
			{
				X509CrlEntry x509CrlEntry = new X509CrlEntry((CrlEntry)obj, this.isIndirect, previousCertificateIssuer);
				set.Add(x509CrlEntry);
				previousCertificateIssuer = x509CrlEntry.GetCertificateIssuer();
			}
			return set;
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00077F54 File Offset: 0x00076154
		public virtual X509CrlEntry GetRevokedCertificate(BigInteger serialNumber)
		{
			IEnumerable revokedCertificateEnumeration = this.c.GetRevokedCertificateEnumeration();
			X509Name previousCertificateIssuer = this.IssuerDN;
			foreach (object obj in revokedCertificateEnumeration)
			{
				CrlEntry crlEntry = (CrlEntry)obj;
				X509CrlEntry x509CrlEntry = new X509CrlEntry(crlEntry, this.isIndirect, previousCertificateIssuer);
				if (serialNumber.Equals(crlEntry.UserCertificate.Value))
				{
					return x509CrlEntry;
				}
				previousCertificateIssuer = x509CrlEntry.GetCertificateIssuer();
			}
			return null;
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00077FE8 File Offset: 0x000761E8
		public virtual ISet GetRevokedCertificates()
		{
			ISet set = this.LoadCrlEntries();
			if (set.Count > 0)
			{
				return set;
			}
			return null;
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00078008 File Offset: 0x00076208
		public virtual byte[] GetTbsCertList()
		{
			byte[] derEncoded;
			try
			{
				derEncoded = this.c.TbsCertList.GetDerEncoded();
			}
			catch (Exception ex)
			{
				throw new CrlException(ex.ToString());
			}
			return derEncoded;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00078044 File Offset: 0x00076244
		public virtual byte[] GetSignature()
		{
			return this.c.GetSignatureOctets();
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x00078051 File Offset: 0x00076251
		public virtual string SigAlgName
		{
			get
			{
				return this.sigAlgName;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060014F9 RID: 5369 RVA: 0x00078059 File Offset: 0x00076259
		public virtual string SigAlgOid
		{
			get
			{
				return this.c.SignatureAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00078070 File Offset: 0x00076270
		public virtual byte[] GetSigAlgParams()
		{
			return Arrays.Clone(this.sigAlgParams);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00078080 File Offset: 0x00076280
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			X509Crl x509Crl = obj as X509Crl;
			return x509Crl != null && this.c.Equals(x509Crl.c);
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x000780B0 File Offset: 0x000762B0
		public override int GetHashCode()
		{
			return this.c.GetHashCode();
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x000780C0 File Offset: 0x000762C0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("              Version: ").Append(this.Version).Append(newLine);
			stringBuilder.Append("             IssuerDN: ").Append(this.IssuerDN).Append(newLine);
			stringBuilder.Append("          This update: ").Append(this.ThisUpdate).Append(newLine);
			stringBuilder.Append("          Next update: ").Append(this.NextUpdate).Append(newLine);
			stringBuilder.Append("  Signature Algorithm: ").Append(this.SigAlgName).Append(newLine);
			byte[] signature = this.GetSignature();
			stringBuilder.Append("            Signature: ");
			stringBuilder.Append(Hex.ToHexString(signature, 0, 20)).Append(newLine);
			for (int i = 20; i < signature.Length; i += 20)
			{
				int length = Math.Min(20, signature.Length - i);
				stringBuilder.Append("                       ");
				stringBuilder.Append(Hex.ToHexString(signature, i, length)).Append(newLine);
			}
			X509Extensions extensions = this.c.TbsCertList.Extensions;
			if (extensions != null)
			{
				IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
				if (enumerator.MoveNext())
				{
					stringBuilder.Append("           Extensions: ").Append(newLine);
				}
				for (;;)
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)enumerator.Current;
					X509Extension extension = extensions.GetExtension(derObjectIdentifier);
					if (extension.Value != null)
					{
						Asn1Object asn1Object = X509ExtensionUtilities.FromExtensionValue(extension.Value);
						stringBuilder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
						try
						{
							if (derObjectIdentifier.Equals(X509Extensions.CrlNumber))
							{
								stringBuilder.Append(new CrlNumber(DerInteger.GetInstance(asn1Object).PositiveValue)).Append(newLine);
							}
							else if (derObjectIdentifier.Equals(X509Extensions.DeltaCrlIndicator))
							{
								stringBuilder.Append("Base CRL: " + new CrlNumber(DerInteger.GetInstance(asn1Object).PositiveValue)).Append(newLine);
							}
							else if (derObjectIdentifier.Equals(X509Extensions.IssuingDistributionPoint))
							{
								stringBuilder.Append(IssuingDistributionPoint.GetInstance((Asn1Sequence)asn1Object)).Append(newLine);
							}
							else if (derObjectIdentifier.Equals(X509Extensions.CrlDistributionPoints))
							{
								stringBuilder.Append(CrlDistPoint.GetInstance((Asn1Sequence)asn1Object)).Append(newLine);
							}
							else if (derObjectIdentifier.Equals(X509Extensions.FreshestCrl))
							{
								stringBuilder.Append(CrlDistPoint.GetInstance((Asn1Sequence)asn1Object)).Append(newLine);
							}
							else
							{
								stringBuilder.Append(derObjectIdentifier.Id);
								stringBuilder.Append(" value = ").Append(Asn1Dump.DumpAsString(asn1Object)).Append(newLine);
							}
							goto IL_2EE;
						}
						catch (Exception)
						{
							stringBuilder.Append(derObjectIdentifier.Id);
							stringBuilder.Append(" value = ").Append("*****").Append(newLine);
							goto IL_2EE;
						}
						goto IL_2E6;
					}
					goto IL_2E6;
					IL_2EE:
					if (!enumerator.MoveNext())
					{
						break;
					}
					continue;
					IL_2E6:
					stringBuilder.Append(newLine);
					goto IL_2EE;
				}
			}
			ISet revokedCertificates = this.GetRevokedCertificates();
			if (revokedCertificates != null)
			{
				foreach (object obj in revokedCertificates)
				{
					X509CrlEntry value = (X509CrlEntry)obj;
					stringBuilder.Append(value);
					stringBuilder.Append(newLine);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00078458 File Offset: 0x00076658
		public virtual bool IsRevoked(X509Certificate cert)
		{
			CrlEntry[] revokedCertificates = this.c.GetRevokedCertificates();
			if (revokedCertificates != null)
			{
				BigInteger serialNumber = cert.SerialNumber;
				for (int i = 0; i < revokedCertificates.Length; i++)
				{
					if (revokedCertificates[i].UserCertificate.Value.Equals(serialNumber))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x000784A4 File Offset: 0x000766A4
		protected virtual bool IsIndirectCrl
		{
			get
			{
				Asn1OctetString extensionValue = this.GetExtensionValue(X509Extensions.IssuingDistributionPoint);
				bool result = false;
				try
				{
					if (extensionValue != null)
					{
						result = IssuingDistributionPoint.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue)).IsIndirectCrl;
					}
				}
				catch (Exception arg)
				{
					throw new CrlException("Exception reading IssuingDistributionPoint" + arg);
				}
				return result;
			}
		}

		// Token: 0x04001386 RID: 4998
		private readonly CertificateList c;

		// Token: 0x04001387 RID: 4999
		private readonly string sigAlgName;

		// Token: 0x04001388 RID: 5000
		private readonly byte[] sigAlgParams;

		// Token: 0x04001389 RID: 5001
		private readonly bool isIndirect;
	}
}

using System;
using System.Collections;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Misc;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509.Extension;

namespace Org.BouncyCastle.X509
{
	// Token: 0x0200027B RID: 635
	public class X509Certificate : X509ExtensionBase
	{
		// Token: 0x060014BB RID: 5307 RVA: 0x00077107 File Offset: 0x00075307
		protected X509Certificate()
		{
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00077110 File Offset: 0x00075310
		public X509Certificate(X509CertificateStructure c)
		{
			this.c = c;
			try
			{
				Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.19"));
				if (extensionValue != null)
				{
					this.basicConstraints = BasicConstraints.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
				}
			}
			catch (Exception arg)
			{
				throw new CertificateParsingException("cannot construct BasicConstraints: " + arg);
			}
			try
			{
				Asn1OctetString extensionValue2 = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.15"));
				if (extensionValue2 != null)
				{
					DerBitString instance = DerBitString.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue2));
					byte[] bytes = instance.GetBytes();
					int num = bytes.Length * 8 - instance.PadBits;
					this.keyUsage = new bool[(num < 9) ? 9 : num];
					for (int num2 = 0; num2 != num; num2++)
					{
						this.keyUsage[num2] = (((int)bytes[num2 / 8] & 128 >> num2 % 8) != 0);
					}
				}
				else
				{
					this.keyUsage = null;
				}
			}
			catch (Exception arg2)
			{
				throw new CertificateParsingException("cannot construct KeyUsage: " + arg2);
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x00077220 File Offset: 0x00075420
		public virtual X509CertificateStructure CertificateStructure
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x00077228 File Offset: 0x00075428
		public virtual bool IsValidNow
		{
			get
			{
				return this.IsValid(DateTime.UtcNow);
			}
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00077235 File Offset: 0x00075435
		public virtual bool IsValid(DateTime time)
		{
			return time.CompareTo(this.NotBefore) >= 0 && time.CompareTo(this.NotAfter) <= 0;
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0007725C File Offset: 0x0007545C
		public virtual void CheckValidity()
		{
			this.CheckValidity(DateTime.UtcNow);
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0007726C File Offset: 0x0007546C
		public virtual void CheckValidity(DateTime time)
		{
			if (time.CompareTo(this.NotAfter) > 0)
			{
				throw new CertificateExpiredException("certificate expired on " + this.c.EndDate.GetTime());
			}
			if (time.CompareTo(this.NotBefore) < 0)
			{
				throw new CertificateNotYetValidException("certificate not valid until " + this.c.StartDate.GetTime());
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x000772D9 File Offset: 0x000754D9
		public virtual int Version
		{
			get
			{
				return this.c.Version;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x000772E6 File Offset: 0x000754E6
		public virtual BigInteger SerialNumber
		{
			get
			{
				return this.c.SerialNumber.Value;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x000772F8 File Offset: 0x000754F8
		public virtual X509Name IssuerDN
		{
			get
			{
				return this.c.Issuer;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060014C5 RID: 5317 RVA: 0x00077305 File Offset: 0x00075505
		public virtual X509Name SubjectDN
		{
			get
			{
				return this.c.Subject;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x00077312 File Offset: 0x00075512
		public virtual DateTime NotBefore
		{
			get
			{
				return this.c.StartDate.ToDateTime();
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060014C7 RID: 5319 RVA: 0x00077324 File Offset: 0x00075524
		public virtual DateTime NotAfter
		{
			get
			{
				return this.c.EndDate.ToDateTime();
			}
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00077336 File Offset: 0x00075536
		public virtual byte[] GetTbsCertificate()
		{
			return this.c.TbsCertificate.GetDerEncoded();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00077348 File Offset: 0x00075548
		public virtual byte[] GetSignature()
		{
			return this.c.GetSignatureOctets();
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x00077355 File Offset: 0x00075555
		public virtual string SigAlgName
		{
			get
			{
				return SignerUtilities.GetEncodingName(this.c.SignatureAlgorithm.Algorithm);
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0007736C File Offset: 0x0007556C
		public virtual string SigAlgOid
		{
			get
			{
				return this.c.SignatureAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00077383 File Offset: 0x00075583
		public virtual byte[] GetSigAlgParams()
		{
			if (this.c.SignatureAlgorithm.Parameters != null)
			{
				return this.c.SignatureAlgorithm.Parameters.GetDerEncoded();
			}
			return null;
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x000773AE File Offset: 0x000755AE
		public virtual DerBitString IssuerUniqueID
		{
			get
			{
				return this.c.TbsCertificate.IssuerUniqueID;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060014CE RID: 5326 RVA: 0x000773C0 File Offset: 0x000755C0
		public virtual DerBitString SubjectUniqueID
		{
			get
			{
				return this.c.TbsCertificate.SubjectUniqueID;
			}
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x000773D2 File Offset: 0x000755D2
		public virtual bool[] GetKeyUsage()
		{
			if (this.keyUsage != null)
			{
				return (bool[])this.keyUsage.Clone();
			}
			return null;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000773F0 File Offset: 0x000755F0
		public virtual IList GetExtendedKeyUsage()
		{
			Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier("2.5.29.37"));
			if (extensionValue == null)
			{
				return null;
			}
			IList result;
			try
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
				IList list = Platform.CreateArrayList();
				foreach (object obj in instance)
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
					list.Add(derObjectIdentifier.Id);
				}
				result = list;
			}
			catch (Exception exception)
			{
				throw new CertificateParsingException("error processing extended key usage extension", exception);
			}
			return result;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00077498 File Offset: 0x00075698
		public virtual int GetBasicConstraints()
		{
			if (this.basicConstraints == null || !this.basicConstraints.IsCA())
			{
				return -1;
			}
			if (this.basicConstraints.PathLenConstraint == null)
			{
				return int.MaxValue;
			}
			return this.basicConstraints.PathLenConstraint.IntValue;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000774D4 File Offset: 0x000756D4
		public virtual ICollection GetSubjectAlternativeNames()
		{
			return this.GetAlternativeNames("2.5.29.17");
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000774E1 File Offset: 0x000756E1
		public virtual ICollection GetIssuerAlternativeNames()
		{
			return this.GetAlternativeNames("2.5.29.18");
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x000774F0 File Offset: 0x000756F0
		protected virtual ICollection GetAlternativeNames(string oid)
		{
			Asn1OctetString extensionValue = this.GetExtensionValue(new DerObjectIdentifier(oid));
			if (extensionValue == null)
			{
				return null;
			}
			GeneralNames instance = GeneralNames.GetInstance(X509ExtensionUtilities.FromExtensionValue(extensionValue));
			IList list = Platform.CreateArrayList();
			foreach (GeneralName generalName in instance.GetNames())
			{
				IList list2 = Platform.CreateArrayList();
				list2.Add(generalName.TagNo);
				list2.Add(generalName.Name.ToString());
				list.Add(list2);
			}
			return list;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00077572 File Offset: 0x00075772
		protected override X509Extensions GetX509Extensions()
		{
			if (this.c.Version < 3)
			{
				return null;
			}
			return this.c.TbsCertificate.Extensions;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00077594 File Offset: 0x00075794
		public virtual AsymmetricKeyParameter GetPublicKey()
		{
			return PublicKeyFactory.CreateKey(this.c.SubjectPublicKeyInfo);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x000775A6 File Offset: 0x000757A6
		public virtual byte[] GetEncoded()
		{
			return this.c.GetDerEncoded();
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x000775B4 File Offset: 0x000757B4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			X509Certificate x509Certificate = obj as X509Certificate;
			return x509Certificate != null && this.c.Equals(x509Certificate.c);
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x000775E4 File Offset: 0x000757E4
		public override int GetHashCode()
		{
			lock (this)
			{
				if (!this.hashValueSet)
				{
					this.hashValue = this.c.GetHashCode();
					this.hashValueSet = true;
				}
			}
			return this.hashValue;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x00077640 File Offset: 0x00075840
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("  [0]         Version: ").Append(this.Version).Append(newLine);
			stringBuilder.Append("         SerialNumber: ").Append(this.SerialNumber).Append(newLine);
			stringBuilder.Append("             IssuerDN: ").Append(this.IssuerDN).Append(newLine);
			stringBuilder.Append("           Start Date: ").Append(this.NotBefore).Append(newLine);
			stringBuilder.Append("           Final Date: ").Append(this.NotAfter).Append(newLine);
			stringBuilder.Append("            SubjectDN: ").Append(this.SubjectDN).Append(newLine);
			stringBuilder.Append("           Public Key: ").Append(this.GetPublicKey()).Append(newLine);
			stringBuilder.Append("  Signature Algorithm: ").Append(this.SigAlgName).Append(newLine);
			byte[] signature = this.GetSignature();
			stringBuilder.Append("            Signature: ").Append(Hex.ToHexString(signature, 0, 20)).Append(newLine);
			for (int i = 20; i < signature.Length; i += 20)
			{
				int length = Math.Min(20, signature.Length - i);
				stringBuilder.Append("                       ").Append(Hex.ToHexString(signature, i, length)).Append(newLine);
			}
			X509Extensions extensions = this.c.TbsCertificate.Extensions;
			if (extensions != null)
			{
				IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
				if (enumerator.MoveNext())
				{
					stringBuilder.Append("       Extensions: \n");
				}
				do
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)enumerator.Current;
					X509Extension extension = extensions.GetExtension(derObjectIdentifier);
					if (extension.Value != null)
					{
						Asn1Object asn1Object = Asn1Object.FromByteArray(extension.Value.GetOctets());
						stringBuilder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
						try
						{
							if (derObjectIdentifier.Equals(X509Extensions.BasicConstraints))
							{
								stringBuilder.Append(BasicConstraints.GetInstance(asn1Object));
							}
							else if (derObjectIdentifier.Equals(X509Extensions.KeyUsage))
							{
								stringBuilder.Append(KeyUsage.GetInstance(asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.NetscapeCertType))
							{
								stringBuilder.Append(new NetscapeCertType((DerBitString)asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.NetscapeRevocationUrl))
							{
								stringBuilder.Append(new NetscapeRevocationUrl((DerIA5String)asn1Object));
							}
							else if (derObjectIdentifier.Equals(MiscObjectIdentifiers.VerisignCzagExtension))
							{
								stringBuilder.Append(new VerisignCzagExtension((DerIA5String)asn1Object));
							}
							else
							{
								stringBuilder.Append(derObjectIdentifier.Id);
								stringBuilder.Append(" value = ").Append(Asn1Dump.DumpAsString(asn1Object));
							}
						}
						catch (Exception)
						{
							stringBuilder.Append(derObjectIdentifier.Id);
							stringBuilder.Append(" value = ").Append("*****");
						}
					}
					stringBuilder.Append(newLine);
				}
				while (enumerator.MoveNext());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0007796C File Offset: 0x00075B6C
		public virtual void Verify(AsymmetricKeyParameter key)
		{
			this.CheckSignature(new Asn1VerifierFactory(this.c.SignatureAlgorithm, key));
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x00077985 File Offset: 0x00075B85
		public virtual void Verify(IVerifierFactoryProvider verifierProvider)
		{
			this.CheckSignature(verifierProvider.CreateVerifierFactory(this.c.SignatureAlgorithm));
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x000779A0 File Offset: 0x00075BA0
		protected virtual void CheckSignature(IVerifierFactory verifier)
		{
			if (!X509Certificate.IsAlgIDEqual(this.c.SignatureAlgorithm, this.c.TbsCertificate.Signature))
			{
				throw new CertificateException("signature algorithm in TBS cert not same as outer cert");
			}
			IStreamCalculator streamCalculator = verifier.CreateCalculator();
			byte[] tbsCertificate = this.GetTbsCertificate();
			streamCalculator.Stream.Write(tbsCertificate, 0, tbsCertificate.Length);
			Platform.Dispose(streamCalculator.Stream);
			if (!((IVerifier)streamCalculator.GetResult()).IsVerified(this.GetSignature()))
			{
				throw new InvalidKeyException("Public key presented not for certificate signature");
			}
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00077A24 File Offset: 0x00075C24
		private static bool IsAlgIDEqual(AlgorithmIdentifier id1, AlgorithmIdentifier id2)
		{
			if (!id1.Algorithm.Equals(id2.Algorithm))
			{
				return false;
			}
			Asn1Encodable parameters = id1.Parameters;
			Asn1Encodable parameters2 = id2.Parameters;
			if (parameters == null == (parameters2 == null))
			{
				return object.Equals(parameters, parameters2);
			}
			if (parameters != null)
			{
				return parameters.ToAsn1Object() is Asn1Null;
			}
			return parameters2.ToAsn1Object() is Asn1Null;
		}

		// Token: 0x0400137D RID: 4989
		private readonly X509CertificateStructure c;

		// Token: 0x0400137E RID: 4990
		private readonly BasicConstraints basicConstraints;

		// Token: 0x0400137F RID: 4991
		private readonly bool[] keyUsage;

		// Token: 0x04001380 RID: 4992
		private bool hashValueSet;

		// Token: 0x04001381 RID: 4993
		private int hashValue;
	}
}

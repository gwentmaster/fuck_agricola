using System;
using System.Collections;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000538 RID: 1336
	public class X509Extensions : Asn1Encodable
	{
		// Token: 0x060030B8 RID: 12472 RVA: 0x000F8934 File Offset: 0x000F6B34
		public static X509Extensions GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return X509Extensions.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x000F8944 File Offset: 0x000F6B44
		public static X509Extensions GetInstance(object obj)
		{
			if (obj == null || obj is X509Extensions)
			{
				return (X509Extensions)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new X509Extensions((Asn1Sequence)obj);
			}
			if (obj is Asn1TaggedObject)
			{
				return X509Extensions.GetInstance(((Asn1TaggedObject)obj).GetObject());
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x000F89AC File Offset: 0x000F6BAC
		private X509Extensions(Asn1Sequence seq)
		{
			this.ordering = Platform.CreateArrayList();
			foreach (object obj in seq)
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
				if (instance.Count < 2 || instance.Count > 3)
				{
					throw new ArgumentException("Bad sequence size: " + instance.Count);
				}
				DerObjectIdentifier instance2 = DerObjectIdentifier.GetInstance(instance[0].ToAsn1Object());
				bool critical = instance.Count == 3 && DerBoolean.GetInstance(instance[1].ToAsn1Object()).IsTrue;
				Asn1OctetString instance3 = Asn1OctetString.GetInstance(instance[instance.Count - 1].ToAsn1Object());
				this.extensions.Add(instance2, new X509Extension(critical, instance3));
				this.ordering.Add(instance2);
			}
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x000F8AC8 File Offset: 0x000F6CC8
		public X509Extensions(IDictionary extensions) : this(null, extensions)
		{
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x000F8AD4 File Offset: 0x000F6CD4
		public X509Extensions(IList ordering, IDictionary extensions)
		{
			if (ordering == null)
			{
				this.ordering = Platform.CreateArrayList(extensions.Keys);
			}
			else
			{
				this.ordering = Platform.CreateArrayList(ordering);
			}
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				this.extensions.Add(key, (X509Extension)extensions[key]);
			}
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x000F8B74 File Offset: 0x000F6D74
		public X509Extensions(IList oids, IList values)
		{
			this.ordering = Platform.CreateArrayList(oids);
			int num = 0;
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				this.extensions.Add(key, (X509Extension)values[num++]);
			}
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x000F8C04 File Offset: 0x000F6E04
		[Obsolete]
		public X509Extensions(Hashtable extensions) : this(null, extensions)
		{
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x000F8C10 File Offset: 0x000F6E10
		[Obsolete]
		public X509Extensions(ArrayList ordering, Hashtable extensions)
		{
			if (ordering == null)
			{
				this.ordering = Platform.CreateArrayList(extensions.Keys);
			}
			else
			{
				this.ordering = Platform.CreateArrayList(ordering);
			}
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				this.extensions.Add(key, (X509Extension)extensions[key]);
			}
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x000F8CB0 File Offset: 0x000F6EB0
		[Obsolete]
		public X509Extensions(ArrayList oids, ArrayList values)
		{
			this.ordering = Platform.CreateArrayList(oids);
			int num = 0;
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				this.extensions.Add(key, (X509Extension)values[num++]);
			}
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000F8D40 File Offset: 0x000F6F40
		[Obsolete("Use ExtensionOids IEnumerable property")]
		public IEnumerator Oids()
		{
			return this.ExtensionOids.GetEnumerator();
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060030C2 RID: 12482 RVA: 0x000F8D4D File Offset: 0x000F6F4D
		public IEnumerable ExtensionOids
		{
			get
			{
				return new EnumerableProxy(this.ordering);
			}
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x000F8D5A File Offset: 0x000F6F5A
		public X509Extension GetExtension(DerObjectIdentifier oid)
		{
			return (X509Extension)this.extensions[oid];
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x000F8D70 File Offset: 0x000F6F70
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
				X509Extension x509Extension = (X509Extension)this.extensions[derObjectIdentifier];
				Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(new Asn1Encodable[]
				{
					derObjectIdentifier
				});
				if (x509Extension.IsCritical)
				{
					asn1EncodableVector2.Add(new Asn1Encodable[]
					{
						DerBoolean.True
					});
				}
				asn1EncodableVector2.Add(new Asn1Encodable[]
				{
					x509Extension.Value
				});
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerSequence(asn1EncodableVector2)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x000F8E44 File Offset: 0x000F7044
		public bool Equivalent(X509Extensions other)
		{
			if (this.extensions.Count != other.extensions.Count)
			{
				return false;
			}
			foreach (object obj in this.extensions.Keys)
			{
				DerObjectIdentifier key = (DerObjectIdentifier)obj;
				if (!this.extensions[key].Equals(other.extensions[key]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x000F8EDC File Offset: 0x000F70DC
		public DerObjectIdentifier[] GetExtensionOids()
		{
			return X509Extensions.ToOidArray(this.ordering);
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x000F8EE9 File Offset: 0x000F70E9
		public DerObjectIdentifier[] GetNonCriticalExtensionOids()
		{
			return this.GetExtensionOids(false);
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x000F8EF2 File Offset: 0x000F70F2
		public DerObjectIdentifier[] GetCriticalExtensionOids()
		{
			return this.GetExtensionOids(true);
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x000F8EFC File Offset: 0x000F70FC
		private DerObjectIdentifier[] GetExtensionOids(bool isCritical)
		{
			IList list = Platform.CreateArrayList();
			foreach (object obj in this.ordering)
			{
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
				if (((X509Extension)this.extensions[derObjectIdentifier]).IsCritical == isCritical)
				{
					list.Add(derObjectIdentifier);
				}
			}
			return X509Extensions.ToOidArray(list);
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000F8F7C File Offset: 0x000F717C
		private static DerObjectIdentifier[] ToOidArray(IList oids)
		{
			DerObjectIdentifier[] array = new DerObjectIdentifier[oids.Count];
			oids.CopyTo(array, 0);
			return array;
		}

		// Token: 0x04001F18 RID: 7960
		public static readonly DerObjectIdentifier SubjectDirectoryAttributes = new DerObjectIdentifier("2.5.29.9");

		// Token: 0x04001F19 RID: 7961
		public static readonly DerObjectIdentifier SubjectKeyIdentifier = new DerObjectIdentifier("2.5.29.14");

		// Token: 0x04001F1A RID: 7962
		public static readonly DerObjectIdentifier KeyUsage = new DerObjectIdentifier("2.5.29.15");

		// Token: 0x04001F1B RID: 7963
		public static readonly DerObjectIdentifier PrivateKeyUsagePeriod = new DerObjectIdentifier("2.5.29.16");

		// Token: 0x04001F1C RID: 7964
		public static readonly DerObjectIdentifier SubjectAlternativeName = new DerObjectIdentifier("2.5.29.17");

		// Token: 0x04001F1D RID: 7965
		public static readonly DerObjectIdentifier IssuerAlternativeName = new DerObjectIdentifier("2.5.29.18");

		// Token: 0x04001F1E RID: 7966
		public static readonly DerObjectIdentifier BasicConstraints = new DerObjectIdentifier("2.5.29.19");

		// Token: 0x04001F1F RID: 7967
		public static readonly DerObjectIdentifier CrlNumber = new DerObjectIdentifier("2.5.29.20");

		// Token: 0x04001F20 RID: 7968
		public static readonly DerObjectIdentifier ReasonCode = new DerObjectIdentifier("2.5.29.21");

		// Token: 0x04001F21 RID: 7969
		public static readonly DerObjectIdentifier InstructionCode = new DerObjectIdentifier("2.5.29.23");

		// Token: 0x04001F22 RID: 7970
		public static readonly DerObjectIdentifier InvalidityDate = new DerObjectIdentifier("2.5.29.24");

		// Token: 0x04001F23 RID: 7971
		public static readonly DerObjectIdentifier DeltaCrlIndicator = new DerObjectIdentifier("2.5.29.27");

		// Token: 0x04001F24 RID: 7972
		public static readonly DerObjectIdentifier IssuingDistributionPoint = new DerObjectIdentifier("2.5.29.28");

		// Token: 0x04001F25 RID: 7973
		public static readonly DerObjectIdentifier CertificateIssuer = new DerObjectIdentifier("2.5.29.29");

		// Token: 0x04001F26 RID: 7974
		public static readonly DerObjectIdentifier NameConstraints = new DerObjectIdentifier("2.5.29.30");

		// Token: 0x04001F27 RID: 7975
		public static readonly DerObjectIdentifier CrlDistributionPoints = new DerObjectIdentifier("2.5.29.31");

		// Token: 0x04001F28 RID: 7976
		public static readonly DerObjectIdentifier CertificatePolicies = new DerObjectIdentifier("2.5.29.32");

		// Token: 0x04001F29 RID: 7977
		public static readonly DerObjectIdentifier PolicyMappings = new DerObjectIdentifier("2.5.29.33");

		// Token: 0x04001F2A RID: 7978
		public static readonly DerObjectIdentifier AuthorityKeyIdentifier = new DerObjectIdentifier("2.5.29.35");

		// Token: 0x04001F2B RID: 7979
		public static readonly DerObjectIdentifier PolicyConstraints = new DerObjectIdentifier("2.5.29.36");

		// Token: 0x04001F2C RID: 7980
		public static readonly DerObjectIdentifier ExtendedKeyUsage = new DerObjectIdentifier("2.5.29.37");

		// Token: 0x04001F2D RID: 7981
		public static readonly DerObjectIdentifier FreshestCrl = new DerObjectIdentifier("2.5.29.46");

		// Token: 0x04001F2E RID: 7982
		public static readonly DerObjectIdentifier InhibitAnyPolicy = new DerObjectIdentifier("2.5.29.54");

		// Token: 0x04001F2F RID: 7983
		public static readonly DerObjectIdentifier AuthorityInfoAccess = new DerObjectIdentifier("1.3.6.1.5.5.7.1.1");

		// Token: 0x04001F30 RID: 7984
		public static readonly DerObjectIdentifier SubjectInfoAccess = new DerObjectIdentifier("1.3.6.1.5.5.7.1.11");

		// Token: 0x04001F31 RID: 7985
		public static readonly DerObjectIdentifier LogoType = new DerObjectIdentifier("1.3.6.1.5.5.7.1.12");

		// Token: 0x04001F32 RID: 7986
		public static readonly DerObjectIdentifier BiometricInfo = new DerObjectIdentifier("1.3.6.1.5.5.7.1.2");

		// Token: 0x04001F33 RID: 7987
		public static readonly DerObjectIdentifier QCStatements = new DerObjectIdentifier("1.3.6.1.5.5.7.1.3");

		// Token: 0x04001F34 RID: 7988
		public static readonly DerObjectIdentifier AuditIdentity = new DerObjectIdentifier("1.3.6.1.5.5.7.1.4");

		// Token: 0x04001F35 RID: 7989
		public static readonly DerObjectIdentifier NoRevAvail = new DerObjectIdentifier("2.5.29.56");

		// Token: 0x04001F36 RID: 7990
		public static readonly DerObjectIdentifier TargetInformation = new DerObjectIdentifier("2.5.29.55");

		// Token: 0x04001F37 RID: 7991
		private readonly IDictionary extensions = Platform.CreateHashtable();

		// Token: 0x04001F38 RID: 7992
		private readonly IList ordering;
	}
}

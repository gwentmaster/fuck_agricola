using System;
using System.Collections;
using Org.BouncyCastle.Asn1.Anssi;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000515 RID: 1301
	public class ECNamedCurveTable
	{
		// Token: 0x06002F9D RID: 12189 RVA: 0x000F4FD4 File Offset: 0x000F31D4
		public static X9ECParameters GetByName(string name)
		{
			X9ECParameters byName = X962NamedCurves.GetByName(name);
			if (byName == null)
			{
				byName = SecNamedCurves.GetByName(name);
			}
			if (byName == null)
			{
				byName = NistNamedCurves.GetByName(name);
			}
			if (byName == null)
			{
				byName = TeleTrusTNamedCurves.GetByName(name);
			}
			if (byName == null)
			{
				byName = AnssiNamedCurves.GetByName(name);
			}
			return byName;
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000F5014 File Offset: 0x000F3214
		public static string GetName(DerObjectIdentifier oid)
		{
			string name = X962NamedCurves.GetName(oid);
			if (name == null)
			{
				name = SecNamedCurves.GetName(oid);
			}
			if (name == null)
			{
				name = NistNamedCurves.GetName(oid);
			}
			if (name == null)
			{
				name = TeleTrusTNamedCurves.GetName(oid);
			}
			if (name == null)
			{
				name = AnssiNamedCurves.GetName(oid);
			}
			return name;
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000F5054 File Offset: 0x000F3254
		public static DerObjectIdentifier GetOid(string name)
		{
			DerObjectIdentifier oid = X962NamedCurves.GetOid(name);
			if (oid == null)
			{
				oid = SecNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = NistNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = TeleTrusTNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = AnssiNamedCurves.GetOid(name);
			}
			return oid;
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000F5094 File Offset: 0x000F3294
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParameters byOid = X962NamedCurves.GetByOid(oid);
			if (byOid == null)
			{
				byOid = SecNamedCurves.GetByOid(oid);
			}
			if (byOid == null)
			{
				byOid = TeleTrusTNamedCurves.GetByOid(oid);
			}
			if (byOid == null)
			{
				byOid = AnssiNamedCurves.GetByOid(oid);
			}
			return byOid;
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x000F50C7 File Offset: 0x000F32C7
		public static IEnumerable Names
		{
			get
			{
				IList list = Platform.CreateArrayList();
				CollectionUtilities.AddRange(list, X962NamedCurves.Names);
				CollectionUtilities.AddRange(list, SecNamedCurves.Names);
				CollectionUtilities.AddRange(list, NistNamedCurves.Names);
				CollectionUtilities.AddRange(list, TeleTrusTNamedCurves.Names);
				CollectionUtilities.AddRange(list, AnssiNamedCurves.Names);
				return list;
			}
		}
	}
}

using System;
using System.Collections;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Asn1.Nist
{
	// Token: 0x0200054E RID: 1358
	public sealed class NistNamedCurves
	{
		// Token: 0x06003166 RID: 12646 RVA: 0x00003425 File Offset: 0x00001625
		private NistNamedCurves()
		{
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x000FD50B File Offset: 0x000FB70B
		private static void DefineCurveAlias(string name, DerObjectIdentifier oid)
		{
			NistNamedCurves.objIds.Add(Platform.ToUpperInvariant(name), oid);
			NistNamedCurves.names.Add(oid, name);
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x000FD52C File Offset: 0x000FB72C
		static NistNamedCurves()
		{
			NistNamedCurves.DefineCurveAlias("B-163", SecObjectIdentifiers.SecT163r2);
			NistNamedCurves.DefineCurveAlias("B-233", SecObjectIdentifiers.SecT233r1);
			NistNamedCurves.DefineCurveAlias("B-283", SecObjectIdentifiers.SecT283r1);
			NistNamedCurves.DefineCurveAlias("B-409", SecObjectIdentifiers.SecT409r1);
			NistNamedCurves.DefineCurveAlias("B-571", SecObjectIdentifiers.SecT571r1);
			NistNamedCurves.DefineCurveAlias("K-163", SecObjectIdentifiers.SecT163k1);
			NistNamedCurves.DefineCurveAlias("K-233", SecObjectIdentifiers.SecT233k1);
			NistNamedCurves.DefineCurveAlias("K-283", SecObjectIdentifiers.SecT283k1);
			NistNamedCurves.DefineCurveAlias("K-409", SecObjectIdentifiers.SecT409k1);
			NistNamedCurves.DefineCurveAlias("K-571", SecObjectIdentifiers.SecT571k1);
			NistNamedCurves.DefineCurveAlias("P-192", SecObjectIdentifiers.SecP192r1);
			NistNamedCurves.DefineCurveAlias("P-224", SecObjectIdentifiers.SecP224r1);
			NistNamedCurves.DefineCurveAlias("P-256", SecObjectIdentifiers.SecP256r1);
			NistNamedCurves.DefineCurveAlias("P-384", SecObjectIdentifiers.SecP384r1);
			NistNamedCurves.DefineCurveAlias("P-521", SecObjectIdentifiers.SecP521r1);
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x000FD630 File Offset: 0x000FB830
		public static X9ECParameters GetByName(string name)
		{
			DerObjectIdentifier oid = NistNamedCurves.GetOid(name);
			if (oid != null)
			{
				return NistNamedCurves.GetByOid(oid);
			}
			return null;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x000FD64F File Offset: 0x000FB84F
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			return SecNamedCurves.GetByOid(oid);
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x000FD657 File Offset: 0x000FB857
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)NistNamedCurves.objIds[Platform.ToUpperInvariant(name)];
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x000FD66E File Offset: 0x000FB86E
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)NistNamedCurves.names[oid];
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x000FD680 File Offset: 0x000FB880
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(NistNamedCurves.names.Values);
			}
		}

		// Token: 0x04002078 RID: 8312
		private static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x04002079 RID: 8313
		private static readonly IDictionary names = Platform.CreateHashtable();
	}
}

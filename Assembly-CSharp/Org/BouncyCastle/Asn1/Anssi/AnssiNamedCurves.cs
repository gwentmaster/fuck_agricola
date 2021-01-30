using System;
using System.Collections;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Asn1.Anssi
{
	// Token: 0x0200055A RID: 1370
	public class AnssiNamedCurves
	{
		// Token: 0x0600319C RID: 12700 RVA: 0x00035D67 File Offset: 0x00033F67
		private static ECCurve ConfigureCurve(ECCurve curve)
		{
			return curve;
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x000BA9AF File Offset: 0x000B8BAF
		private static BigInteger FromHex(string hex)
		{
			return new BigInteger(1, Hex.Decode(hex));
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x000FE6BE File Offset: 0x000FC8BE
		private static void DefineCurve(string name, DerObjectIdentifier oid, X9ECParametersHolder holder)
		{
			AnssiNamedCurves.objIds.Add(Platform.ToUpperInvariant(name), oid);
			AnssiNamedCurves.names.Add(oid, name);
			AnssiNamedCurves.curves.Add(oid, holder);
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x000FE6E9 File Offset: 0x000FC8E9
		static AnssiNamedCurves()
		{
			AnssiNamedCurves.DefineCurve("FRP256v1", AnssiObjectIdentifiers.FRP256v1, AnssiNamedCurves.Frp256v1Holder.Instance);
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x000FE720 File Offset: 0x000FC920
		public static X9ECParameters GetByName(string name)
		{
			DerObjectIdentifier oid = AnssiNamedCurves.GetOid(name);
			if (oid != null)
			{
				return AnssiNamedCurves.GetByOid(oid);
			}
			return null;
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x000FE740 File Offset: 0x000FC940
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)AnssiNamedCurves.curves[oid];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000FE769 File Offset: 0x000FC969
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)AnssiNamedCurves.objIds[Platform.ToUpperInvariant(name)];
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000FE780 File Offset: 0x000FC980
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)AnssiNamedCurves.names[oid];
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x000FE792 File Offset: 0x000FC992
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(AnssiNamedCurves.names.Values);
			}
		}

		// Token: 0x040020F7 RID: 8439
		private static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x040020F8 RID: 8440
		private static readonly IDictionary curves = Platform.CreateHashtable();

		// Token: 0x040020F9 RID: 8441
		private static readonly IDictionary names = Platform.CreateHashtable();

		// Token: 0x020008F1 RID: 2289
		internal class Frp256v1Holder : X9ECParametersHolder
		{
			// Token: 0x060046A9 RID: 18089 RVA: 0x00144B59 File Offset: 0x00142D59
			private Frp256v1Holder()
			{
			}

			// Token: 0x060046AA RID: 18090 RVA: 0x00147C18 File Offset: 0x00145E18
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = AnssiNamedCurves.FromHex("F1FD178C0B3AD58F10126DE8CE42435B3961ADBCABC8CA6DE8FCF353D86E9C03");
				BigInteger a = AnssiNamedCurves.FromHex("F1FD178C0B3AD58F10126DE8CE42435B3961ADBCABC8CA6DE8FCF353D86E9C00");
				BigInteger b = AnssiNamedCurves.FromHex("EE353FCA5428A9300D4ABA754A44C00FDFEC0C9AE4B1A1803075ED967B7BB73F");
				byte[] seed = null;
				BigInteger bigInteger = AnssiNamedCurves.FromHex("F1FD178C0B3AD58F10126DE8CE42435B53DC67E140D2BF941FFDD459C6D655E1");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = AnssiNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04B6B3D4C356C139EB31183D4749D423958C27D2DCAF98B70164C97A2DD98F5CFF6142E0F7C8B204911F9271F0F3ECEF8C2701C307E8E4C9E183115A1554062CFB"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x0400300D RID: 12301
			internal static readonly X9ECParametersHolder Instance = new AnssiNamedCurves.Frp256v1Holder();
		}
	}
}

using System;
using System.Collections;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x02000556 RID: 1366
	public sealed class ECGost3410NamedCurves
	{
		// Token: 0x0600317D RID: 12669 RVA: 0x00003425 File Offset: 0x00001625
		private ECGost3410NamedCurves()
		{
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x000FDF14 File Offset: 0x000FC114
		static ECGost3410NamedCurves()
		{
			BigInteger q = new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639319");
			BigInteger bigInteger = new BigInteger("115792089237316195423570985008687907853073762908499243225378155805079068850323");
			FpCurve fpCurve = new FpCurve(q, new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639316"), new BigInteger("166"), bigInteger, BigInteger.One);
			ECDomainParameters value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("1"), new BigInteger("64033881142927202683649881450433473985931760268884941288852745803908878638612")), bigInteger);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProA] = value;
			BigInteger q2 = new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639319");
			bigInteger = new BigInteger("115792089237316195423570985008687907853073762908499243225378155805079068850323");
			fpCurve = new FpCurve(q2, new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639316"), new BigInteger("166"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("1"), new BigInteger("64033881142927202683649881450433473985931760268884941288852745803908878638612")), bigInteger);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA] = value;
			BigInteger q3 = new BigInteger("57896044618658097711785492504343953926634992332820282019728792003956564823193");
			bigInteger = new BigInteger("57896044618658097711785492504343953927102133160255826820068844496087732066703");
			fpCurve = new FpCurve(q3, new BigInteger("57896044618658097711785492504343953926634992332820282019728792003956564823190"), new BigInteger("28091019353058090096996979000309560759124368558014865957655842872397301267595"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("1"), new BigInteger("28792665814854611296992347458380284135028636778229113005756334730996303888124")), bigInteger);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProB] = value;
			BigInteger q4 = new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502619");
			bigInteger = new BigInteger("70390085352083305199547718019018437840920882647164081035322601458352298396601");
			fpCurve = new FpCurve(q4, new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502616"), new BigInteger("32858"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("0"), new BigInteger("29818893917731240733471273240314769927240550812383695689146495261604565990247")), bigInteger);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB] = value;
			BigInteger q5 = new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502619");
			bigInteger = new BigInteger("70390085352083305199547718019018437840920882647164081035322601458352298396601");
			fpCurve = new FpCurve(q5, new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502616"), new BigInteger("32858"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("0"), new BigInteger("29818893917731240733471273240314769927240550812383695689146495261604565990247")), bigInteger);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProC] = value;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-A"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProA;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-B"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProB;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-C"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProC;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-XchA"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-XchB"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB;
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProA] = "GostR3410-2001-CryptoPro-A";
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProB] = "GostR3410-2001-CryptoPro-B";
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProC] = "GostR3410-2001-CryptoPro-C";
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA] = "GostR3410-2001-CryptoPro-XchA";
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB] = "GostR3410-2001-CryptoPro-XchB";
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x000FE20A File Offset: 0x000FC40A
		public static ECDomainParameters GetByOid(DerObjectIdentifier oid)
		{
			return (ECDomainParameters)ECGost3410NamedCurves.parameters[oid];
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000FE21C File Offset: 0x000FC41C
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(ECGost3410NamedCurves.names.Values);
			}
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x000FE230 File Offset: 0x000FC430
		public static ECDomainParameters GetByName(string name)
		{
			DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)ECGost3410NamedCurves.objIds[name];
			if (derObjectIdentifier != null)
			{
				return (ECDomainParameters)ECGost3410NamedCurves.parameters[derObjectIdentifier];
			}
			return null;
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x000FE263 File Offset: 0x000FC463
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)ECGost3410NamedCurves.names[oid];
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x000FE275 File Offset: 0x000FC475
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)ECGost3410NamedCurves.objIds[name];
		}

		// Token: 0x040020E8 RID: 8424
		internal static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x040020E9 RID: 8425
		internal static readonly IDictionary parameters = Platform.CreateHashtable();

		// Token: 0x040020EA RID: 8426
		internal static readonly IDictionary names = Platform.CreateHashtable();
	}
}

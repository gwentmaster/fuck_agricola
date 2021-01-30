using System;
using System.Collections;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000516 RID: 1302
	public sealed class X962NamedCurves
	{
		// Token: 0x06002FA3 RID: 12195 RVA: 0x00003425 File Offset: 0x00001625
		private X962NamedCurves()
		{
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000F5105 File Offset: 0x000F3305
		private static void DefineCurve(string name, DerObjectIdentifier oid, X9ECParametersHolder holder)
		{
			X962NamedCurves.objIds.Add(Platform.ToUpperInvariant(name), oid);
			X962NamedCurves.names.Add(oid, name);
			X962NamedCurves.curves.Add(oid, holder);
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x000F5130 File Offset: 0x000F3330
		static X962NamedCurves()
		{
			X962NamedCurves.DefineCurve("prime192v1", X9ObjectIdentifiers.Prime192v1, X962NamedCurves.Prime192v1Holder.Instance);
			X962NamedCurves.DefineCurve("prime192v2", X9ObjectIdentifiers.Prime192v2, X962NamedCurves.Prime192v2Holder.Instance);
			X962NamedCurves.DefineCurve("prime192v3", X9ObjectIdentifiers.Prime192v3, X962NamedCurves.Prime192v3Holder.Instance);
			X962NamedCurves.DefineCurve("prime239v1", X9ObjectIdentifiers.Prime239v1, X962NamedCurves.Prime239v1Holder.Instance);
			X962NamedCurves.DefineCurve("prime239v2", X9ObjectIdentifiers.Prime239v2, X962NamedCurves.Prime239v2Holder.Instance);
			X962NamedCurves.DefineCurve("prime239v3", X9ObjectIdentifiers.Prime239v3, X962NamedCurves.Prime239v3Holder.Instance);
			X962NamedCurves.DefineCurve("prime256v1", X9ObjectIdentifiers.Prime256v1, X962NamedCurves.Prime256v1Holder.Instance);
			X962NamedCurves.DefineCurve("c2pnb163v1", X9ObjectIdentifiers.C2Pnb163v1, X962NamedCurves.C2pnb163v1Holder.Instance);
			X962NamedCurves.DefineCurve("c2pnb163v2", X9ObjectIdentifiers.C2Pnb163v2, X962NamedCurves.C2pnb163v2Holder.Instance);
			X962NamedCurves.DefineCurve("c2pnb163v3", X9ObjectIdentifiers.C2Pnb163v3, X962NamedCurves.C2pnb163v3Holder.Instance);
			X962NamedCurves.DefineCurve("c2pnb176w1", X9ObjectIdentifiers.C2Pnb176w1, X962NamedCurves.C2pnb176w1Holder.Instance);
			X962NamedCurves.DefineCurve("c2tnb191v1", X9ObjectIdentifiers.C2Tnb191v1, X962NamedCurves.C2tnb191v1Holder.Instance);
			X962NamedCurves.DefineCurve("c2tnb191v2", X9ObjectIdentifiers.C2Tnb191v2, X962NamedCurves.C2tnb191v2Holder.Instance);
			X962NamedCurves.DefineCurve("c2tnb191v3", X9ObjectIdentifiers.C2Tnb191v3, X962NamedCurves.C2tnb191v3Holder.Instance);
			X962NamedCurves.DefineCurve("c2pnb208w1", X9ObjectIdentifiers.C2Pnb208w1, X962NamedCurves.C2pnb208w1Holder.Instance);
			X962NamedCurves.DefineCurve("c2tnb239v1", X9ObjectIdentifiers.C2Tnb239v1, X962NamedCurves.C2tnb239v1Holder.Instance);
			X962NamedCurves.DefineCurve("c2tnb239v2", X9ObjectIdentifiers.C2Tnb239v2, X962NamedCurves.C2tnb239v2Holder.Instance);
			X962NamedCurves.DefineCurve("c2tnb239v3", X9ObjectIdentifiers.C2Tnb239v3, X962NamedCurves.C2tnb239v3Holder.Instance);
			X962NamedCurves.DefineCurve("c2pnb272w1", X9ObjectIdentifiers.C2Pnb272w1, X962NamedCurves.C2pnb272w1Holder.Instance);
			X962NamedCurves.DefineCurve("c2pnb304w1", X9ObjectIdentifiers.C2Pnb304w1, X962NamedCurves.C2pnb304w1Holder.Instance);
			X962NamedCurves.DefineCurve("c2tnb359v1", X9ObjectIdentifiers.C2Tnb359v1, X962NamedCurves.C2tnb359v1Holder.Instance);
			X962NamedCurves.DefineCurve("c2pnb368w1", X9ObjectIdentifiers.C2Pnb368w1, X962NamedCurves.C2pnb368w1Holder.Instance);
			X962NamedCurves.DefineCurve("c2tnb431r1", X9ObjectIdentifiers.C2Tnb431r1, X962NamedCurves.C2tnb431r1Holder.Instance);
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x000F5328 File Offset: 0x000F3528
		public static X9ECParameters GetByName(string name)
		{
			DerObjectIdentifier oid = X962NamedCurves.GetOid(name);
			if (oid != null)
			{
				return X962NamedCurves.GetByOid(oid);
			}
			return null;
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x000F5348 File Offset: 0x000F3548
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)X962NamedCurves.curves[oid];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x000F5371 File Offset: 0x000F3571
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)X962NamedCurves.objIds[Platform.ToUpperInvariant(name)];
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000F5388 File Offset: 0x000F3588
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)X962NamedCurves.names[oid];
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06002FAA RID: 12202 RVA: 0x000F539A File Offset: 0x000F359A
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(X962NamedCurves.names.Values);
			}
		}

		// Token: 0x04001E5C RID: 7772
		private static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x04001E5D RID: 7773
		private static readonly IDictionary curves = Platform.CreateHashtable();

		// Token: 0x04001E5E RID: 7774
		private static readonly IDictionary names = Platform.CreateHashtable();

		// Token: 0x020008AA RID: 2218
		internal class Prime192v1Holder : X9ECParametersHolder
		{
			// Token: 0x060045D5 RID: 17877 RVA: 0x00144B59 File Offset: 0x00142D59
			private Prime192v1Holder()
			{
			}

			// Token: 0x060045D6 RID: 17878 RVA: 0x001458EC File Offset: 0x00143AEC
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("ffffffffffffffffffffffff99def836146bc9b1b4d22831", 16);
				BigInteger one = BigInteger.One;
				FpCurve fpCurve = new FpCurve(new BigInteger("6277101735386680763835789423207666416083908700390324961279"), new BigInteger("fffffffffffffffffffffffffffffffefffffffffffffffc", 16), new BigInteger("64210519e59c80e70fa7e9ab72243049feb8deecc146b9b1", 16), bigInteger, one);
				return new X9ECParameters(fpCurve, new X9ECPoint(fpCurve, Hex.Decode("03188da80eb03090f67cbf20eb43a18800f4ff0afd82ff1012")), bigInteger, one, Hex.Decode("3045AE6FC8422f64ED579528D38120EAE12196D5"));
			}

			// Token: 0x04002F90 RID: 12176
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.Prime192v1Holder();
		}

		// Token: 0x020008AB RID: 2219
		internal class Prime192v2Holder : X9ECParametersHolder
		{
			// Token: 0x060045D8 RID: 17880 RVA: 0x00144B59 File Offset: 0x00142D59
			private Prime192v2Holder()
			{
			}

			// Token: 0x060045D9 RID: 17881 RVA: 0x00145964 File Offset: 0x00143B64
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("fffffffffffffffffffffffe5fb1a724dc80418648d8dd31", 16);
				BigInteger one = BigInteger.One;
				FpCurve fpCurve = new FpCurve(new BigInteger("6277101735386680763835789423207666416083908700390324961279"), new BigInteger("fffffffffffffffffffffffffffffffefffffffffffffffc", 16), new BigInteger("cc22d6dfb95c6b25e49c0d6364a4e5980c393aa21668d953", 16), bigInteger, one);
				return new X9ECParameters(fpCurve, new X9ECPoint(fpCurve, Hex.Decode("03eea2bae7e1497842f2de7769cfe9c989c072ad696f48034a")), bigInteger, one, Hex.Decode("31a92ee2029fd10d901b113e990710f0d21ac6b6"));
			}

			// Token: 0x04002F91 RID: 12177
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.Prime192v2Holder();
		}

		// Token: 0x020008AC RID: 2220
		internal class Prime192v3Holder : X9ECParametersHolder
		{
			// Token: 0x060045DB RID: 17883 RVA: 0x00144B59 File Offset: 0x00142D59
			private Prime192v3Holder()
			{
			}

			// Token: 0x060045DC RID: 17884 RVA: 0x001459DC File Offset: 0x00143BDC
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("ffffffffffffffffffffffff7a62d031c83f4294f640ec13", 16);
				BigInteger one = BigInteger.One;
				FpCurve fpCurve = new FpCurve(new BigInteger("6277101735386680763835789423207666416083908700390324961279"), new BigInteger("fffffffffffffffffffffffffffffffefffffffffffffffc", 16), new BigInteger("22123dc2395a05caa7423daeccc94760a7d462256bd56916", 16), bigInteger, one);
				return new X9ECParameters(fpCurve, new X9ECPoint(fpCurve, Hex.Decode("027d29778100c65a1da1783716588dce2b8b4aee8e228f1896")), bigInteger, one, Hex.Decode("c469684435deb378c4b65ca9591e2a5763059a2e"));
			}

			// Token: 0x04002F92 RID: 12178
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.Prime192v3Holder();
		}

		// Token: 0x020008AD RID: 2221
		internal class Prime239v1Holder : X9ECParametersHolder
		{
			// Token: 0x060045DE RID: 17886 RVA: 0x00144B59 File Offset: 0x00142D59
			private Prime239v1Holder()
			{
			}

			// Token: 0x060045DF RID: 17887 RVA: 0x00145A54 File Offset: 0x00143C54
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("7fffffffffffffffffffffff7fffff9e5e9a9f5d9071fbd1522688909d0b", 16);
				BigInteger one = BigInteger.One;
				FpCurve fpCurve = new FpCurve(new BigInteger("883423532389192164791648750360308885314476597252960362792450860609699839"), new BigInteger("7fffffffffffffffffffffff7fffffffffff8000000000007ffffffffffc", 16), new BigInteger("6b016c3bdcf18941d0d654921475ca71a9db2fb27d1d37796185c2942c0a", 16), bigInteger, one);
				return new X9ECParameters(fpCurve, new X9ECPoint(fpCurve, Hex.Decode("020ffa963cdca8816ccc33b8642bedf905c3d358573d3f27fbbd3b3cb9aaaf")), bigInteger, one, Hex.Decode("e43bb460f0b80cc0c0b075798e948060f8321b7d"));
			}

			// Token: 0x04002F93 RID: 12179
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.Prime239v1Holder();
		}

		// Token: 0x020008AE RID: 2222
		internal class Prime239v2Holder : X9ECParametersHolder
		{
			// Token: 0x060045E1 RID: 17889 RVA: 0x00144B59 File Offset: 0x00142D59
			private Prime239v2Holder()
			{
			}

			// Token: 0x060045E2 RID: 17890 RVA: 0x00145ACC File Offset: 0x00143CCC
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("7fffffffffffffffffffffff800000cfa7e8594377d414c03821bc582063", 16);
				BigInteger one = BigInteger.One;
				FpCurve fpCurve = new FpCurve(new BigInteger("883423532389192164791648750360308885314476597252960362792450860609699839"), new BigInteger("7fffffffffffffffffffffff7fffffffffff8000000000007ffffffffffc", 16), new BigInteger("617fab6832576cbbfed50d99f0249c3fee58b94ba0038c7ae84c8c832f2c", 16), bigInteger, one);
				return new X9ECParameters(fpCurve, new X9ECPoint(fpCurve, Hex.Decode("0238af09d98727705120c921bb5e9e26296a3cdcf2f35757a0eafd87b830e7")), bigInteger, one, Hex.Decode("e8b4011604095303ca3b8099982be09fcb9ae616"));
			}

			// Token: 0x04002F94 RID: 12180
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.Prime239v2Holder();
		}

		// Token: 0x020008AF RID: 2223
		internal class Prime239v3Holder : X9ECParametersHolder
		{
			// Token: 0x060045E4 RID: 17892 RVA: 0x00144B59 File Offset: 0x00142D59
			private Prime239v3Holder()
			{
			}

			// Token: 0x060045E5 RID: 17893 RVA: 0x00145B44 File Offset: 0x00143D44
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("7fffffffffffffffffffffff7fffff975deb41b3a6057c3c432146526551", 16);
				BigInteger one = BigInteger.One;
				FpCurve fpCurve = new FpCurve(new BigInteger("883423532389192164791648750360308885314476597252960362792450860609699839"), new BigInteger("7fffffffffffffffffffffff7fffffffffff8000000000007ffffffffffc", 16), new BigInteger("255705fa2a306654b1f4cb03d6a750a30c250102d4988717d9ba15ab6d3e", 16), bigInteger, one);
				return new X9ECParameters(fpCurve, new X9ECPoint(fpCurve, Hex.Decode("036768ae8e18bb92cfcf005c949aa2c6d94853d0e660bbf854b1c9505fe95a")), bigInteger, one, Hex.Decode("7d7374168ffe3471b60a857686a19475d3bfa2ff"));
			}

			// Token: 0x04002F95 RID: 12181
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.Prime239v3Holder();
		}

		// Token: 0x020008B0 RID: 2224
		internal class Prime256v1Holder : X9ECParametersHolder
		{
			// Token: 0x060045E7 RID: 17895 RVA: 0x00144B59 File Offset: 0x00142D59
			private Prime256v1Holder()
			{
			}

			// Token: 0x060045E8 RID: 17896 RVA: 0x00145BBC File Offset: 0x00143DBC
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("ffffffff00000000ffffffffffffffffbce6faada7179e84f3b9cac2fc632551", 16);
				BigInteger one = BigInteger.One;
				FpCurve fpCurve = new FpCurve(new BigInteger("115792089210356248762697446949407573530086143415290314195533631308867097853951"), new BigInteger("ffffffff00000001000000000000000000000000fffffffffffffffffffffffc", 16), new BigInteger("5ac635d8aa3a93e7b3ebbd55769886bc651d06b0cc53b0f63bce3c3e27d2604b", 16), bigInteger, one);
				return new X9ECParameters(fpCurve, new X9ECPoint(fpCurve, Hex.Decode("036b17d1f2e12c4247f8bce6e563a440f277037d812deb33a0f4a13945d898c296")), bigInteger, one, Hex.Decode("c49d360886e704936a6678e1139d26b7819f7e90"));
			}

			// Token: 0x04002F96 RID: 12182
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.Prime256v1Holder();
		}

		// Token: 0x020008B1 RID: 2225
		internal class C2pnb163v1Holder : X9ECParametersHolder
		{
			// Token: 0x060045EA RID: 17898 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2pnb163v1Holder()
			{
			}

			// Token: 0x060045EB RID: 17899 RVA: 0x00145C34 File Offset: 0x00143E34
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("0400000000000000000001E60FC8821CC74DAEAFC1", 16);
				BigInteger two = BigInteger.Two;
				F2mCurve f2mCurve = new F2mCurve(163, 1, 2, 8, new BigInteger("072546B5435234A422E0789675F432C89435DE5242", 16), new BigInteger("00C9517D06D5240D3CFF38C74B20B6CD4D6F9DD4D9", 16), bigInteger, two);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("0307AF69989546103D79329FCC3D74880F33BBE803CB")), bigInteger, two, Hex.Decode("D2C0FB15760860DEF1EEF4D696E6768756151754"));
			}

			// Token: 0x04002F97 RID: 12183
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2pnb163v1Holder();
		}

		// Token: 0x020008B2 RID: 2226
		internal class C2pnb163v2Holder : X9ECParametersHolder
		{
			// Token: 0x060045ED RID: 17901 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2pnb163v2Holder()
			{
			}

			// Token: 0x060045EE RID: 17902 RVA: 0x00145CA8 File Offset: 0x00143EA8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("03FFFFFFFFFFFFFFFFFFFDF64DE1151ADBB78F10A7", 16);
				BigInteger two = BigInteger.Two;
				F2mCurve f2mCurve = new F2mCurve(163, 1, 2, 8, new BigInteger("0108B39E77C4B108BED981ED0E890E117C511CF072", 16), new BigInteger("0667ACEB38AF4E488C407433FFAE4F1C811638DF20", 16), bigInteger, two);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("030024266E4EB5106D0A964D92C4860E2671DB9B6CC5")), bigInteger, two, null);
			}

			// Token: 0x04002F98 RID: 12184
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2pnb163v2Holder();
		}

		// Token: 0x020008B3 RID: 2227
		internal class C2pnb163v3Holder : X9ECParametersHolder
		{
			// Token: 0x060045F0 RID: 17904 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2pnb163v3Holder()
			{
			}

			// Token: 0x060045F1 RID: 17905 RVA: 0x00145D14 File Offset: 0x00143F14
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("03FFFFFFFFFFFFFFFFFFFE1AEE140F110AFF961309", 16);
				BigInteger two = BigInteger.Two;
				F2mCurve f2mCurve = new F2mCurve(163, 1, 2, 8, new BigInteger("07A526C63D3E25A256A007699F5447E32AE456B50E", 16), new BigInteger("03F7061798EB99E238FD6F1BF95B48FEEB4854252B", 16), bigInteger, two);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("0202F9F87B7C574D0BDECF8A22E6524775F98CDEBDCB")), bigInteger, two, null);
			}

			// Token: 0x04002F99 RID: 12185
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2pnb163v3Holder();
		}

		// Token: 0x020008B4 RID: 2228
		internal class C2pnb176w1Holder : X9ECParametersHolder
		{
			// Token: 0x060045F3 RID: 17907 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2pnb176w1Holder()
			{
			}

			// Token: 0x060045F4 RID: 17908 RVA: 0x00145D80 File Offset: 0x00143F80
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("010092537397ECA4F6145799D62B0A19CE06FE26AD", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(65390L);
				F2mCurve f2mCurve = new F2mCurve(176, 1, 2, 43, new BigInteger("00E4E6DB2995065C407D9D39B8D0967B96704BA8E9C90B", 16), new BigInteger("005DDA470ABE6414DE8EC133AE28E9BBD7FCEC0AE0FFF2", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("038D16C2866798B600F9F08BB4A8E860F3298CE04A5798")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002F9A RID: 12186
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2pnb176w1Holder();
		}

		// Token: 0x020008B5 RID: 2229
		internal class C2tnb191v1Holder : X9ECParametersHolder
		{
			// Token: 0x060045F6 RID: 17910 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2tnb191v1Holder()
			{
			}

			// Token: 0x060045F7 RID: 17911 RVA: 0x00145DF4 File Offset: 0x00143FF4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("40000000000000000000000004A20E90C39067C893BBB9A5", 16);
				BigInteger two = BigInteger.Two;
				F2mCurve f2mCurve = new F2mCurve(191, 9, new BigInteger("2866537B676752636A68F56554E12640276B649EF7526267", 16), new BigInteger("2E45EF571F00786F67B0081B9495A3D95462F5DE0AA185EC", 16), bigInteger, two);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("0236B3DAF8A23206F9C4F299D7B21A9C369137F2C84AE1AA0D")), bigInteger, two, Hex.Decode("4E13CA542744D696E67687561517552F279A8C84"));
			}

			// Token: 0x04002F9B RID: 12187
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2tnb191v1Holder();
		}

		// Token: 0x020008B6 RID: 2230
		internal class C2tnb191v2Holder : X9ECParametersHolder
		{
			// Token: 0x060045F9 RID: 17913 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2tnb191v2Holder()
			{
			}

			// Token: 0x060045FA RID: 17914 RVA: 0x00145E68 File Offset: 0x00144068
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("20000000000000000000000050508CB89F652824E06B8173", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				F2mCurve f2mCurve = new F2mCurve(191, 9, new BigInteger("401028774D7777C7B7666D1366EA432071274F89FF01E718", 16), new BigInteger("0620048D28BCBD03B6249C99182B7C8CD19700C362C46A01", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("023809B2B7CC1B28CC5A87926AAD83FD28789E81E2C9E3BF10")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002F9C RID: 12188
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2tnb191v2Holder();
		}

		// Token: 0x020008B7 RID: 2231
		internal class C2tnb191v3Holder : X9ECParametersHolder
		{
			// Token: 0x060045FC RID: 17916 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2tnb191v3Holder()
			{
			}

			// Token: 0x060045FD RID: 17917 RVA: 0x00145ED4 File Offset: 0x001440D4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("155555555555555555555555610C0B196812BFB6288A3EA3", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(6L);
				F2mCurve f2mCurve = new F2mCurve(191, 9, new BigInteger("6C01074756099122221056911C77D77E77A777E7E7E77FCB", 16), new BigInteger("71FE1AF926CF847989EFEF8DB459F66394D90F32AD3F15E8", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("03375D4CE24FDE434489DE8746E71786015009E66E38A926DD")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002F9D RID: 12189
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2tnb191v3Holder();
		}

		// Token: 0x020008B8 RID: 2232
		internal class C2pnb208w1Holder : X9ECParametersHolder
		{
			// Token: 0x060045FF RID: 17919 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2pnb208w1Holder()
			{
			}

			// Token: 0x06004600 RID: 17920 RVA: 0x00145F40 File Offset: 0x00144140
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("0101BAF95C9723C57B6C21DA2EFF2D5ED588BDD5717E212F9D", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(65096L);
				F2mCurve f2mCurve = new F2mCurve(208, 1, 2, 83, new BigInteger("0", 16), new BigInteger("00C8619ED45A62E6212E1160349E2BFA844439FAFC2A3FD1638F9E", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("0289FDFBE4ABE193DF9559ECF07AC0CE78554E2784EB8C1ED1A57A")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002F9E RID: 12190
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2pnb208w1Holder();
		}

		// Token: 0x020008B9 RID: 2233
		internal class C2tnb239v1Holder : X9ECParametersHolder
		{
			// Token: 0x06004602 RID: 17922 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2tnb239v1Holder()
			{
			}

			// Token: 0x06004603 RID: 17923 RVA: 0x00145FB4 File Offset: 0x001441B4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("2000000000000000000000000000000F4D42FFE1492A4993F1CAD666E447", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				F2mCurve f2mCurve = new F2mCurve(239, 36, new BigInteger("32010857077C5431123A46B808906756F543423E8D27877578125778AC76", 16), new BigInteger("790408F2EEDAF392B012EDEFB3392F30F4327C0CA3F31FC383C422AA8C16", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("0257927098FA932E7C0A96D3FD5B706EF7E5F5C156E16B7E7C86038552E91D")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002F9F RID: 12191
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2tnb239v1Holder();
		}

		// Token: 0x020008BA RID: 2234
		internal class C2tnb239v2Holder : X9ECParametersHolder
		{
			// Token: 0x06004605 RID: 17925 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2tnb239v2Holder()
			{
			}

			// Token: 0x06004606 RID: 17926 RVA: 0x00146020 File Offset: 0x00144220
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("1555555555555555555555555555553C6F2885259C31E3FCDF154624522D", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(6L);
				F2mCurve f2mCurve = new F2mCurve(239, 36, new BigInteger("4230017757A767FAE42398569B746325D45313AF0766266479B75654E65F", 16), new BigInteger("5037EA654196CFF0CD82B2C14A2FCF2E3FF8775285B545722F03EACDB74B", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("0228F9D04E900069C8DC47A08534FE76D2B900B7D7EF31F5709F200C4CA205")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002FA0 RID: 12192
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2tnb239v2Holder();
		}

		// Token: 0x020008BB RID: 2235
		internal class C2tnb239v3Holder : X9ECParametersHolder
		{
			// Token: 0x06004608 RID: 17928 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2tnb239v3Holder()
			{
			}

			// Token: 0x06004609 RID: 17929 RVA: 0x0014608C File Offset: 0x0014428C
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("0CCCCCCCCCCCCCCCCCCCCCCCCCCCCCAC4912D2D9DF903EF9888B8A0E4CFF", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(10L);
				F2mCurve f2mCurve = new F2mCurve(239, 36, new BigInteger("01238774666A67766D6676F778E676B66999176666E687666D8766C66A9F", 16), new BigInteger("6A941977BA9F6A435199ACFC51067ED587F519C5ECB541B8E44111DE1D40", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("0370F6E9D04D289C4E89913CE3530BFDE903977D42B146D539BF1BDE4E9C92")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002FA1 RID: 12193
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2tnb239v3Holder();
		}

		// Token: 0x020008BC RID: 2236
		internal class C2pnb272w1Holder : X9ECParametersHolder
		{
			// Token: 0x0600460B RID: 17931 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2pnb272w1Holder()
			{
			}

			// Token: 0x0600460C RID: 17932 RVA: 0x001460FC File Offset: 0x001442FC
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("0100FAF51354E0E39E4892DF6E319C72C8161603FA45AA7B998A167B8F1E629521", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(65286L);
				F2mCurve f2mCurve = new F2mCurve(272, 1, 3, 56, new BigInteger("0091A091F03B5FBA4AB2CCF49C4EDD220FB028712D42BE752B2C40094DBACDB586FB20", 16), new BigInteger("7167EFC92BB2E3CE7C8AAAFF34E12A9C557003D7C73A6FAF003F99F6CC8482E540F7", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("026108BABB2CEEBCF787058A056CBE0CFE622D7723A289E08A07AE13EF0D10D171DD8D")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002FA2 RID: 12194
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2pnb272w1Holder();
		}

		// Token: 0x020008BD RID: 2237
		internal class C2pnb304w1Holder : X9ECParametersHolder
		{
			// Token: 0x0600460E RID: 17934 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2pnb304w1Holder()
			{
			}

			// Token: 0x0600460F RID: 17935 RVA: 0x00146170 File Offset: 0x00144370
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("0101D556572AABAC800101D556572AABAC8001022D5C91DD173F8FB561DA6899164443051D", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(65070L);
				F2mCurve f2mCurve = new F2mCurve(304, 1, 2, 11, new BigInteger("00FD0D693149A118F651E6DCE6802085377E5F882D1B510B44160074C1288078365A0396C8E681", 16), new BigInteger("00BDDB97E555A50A908E43B01C798EA5DAA6788F1EA2794EFCF57166B8C14039601E55827340BE", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("02197B07845E9BE2D96ADB0F5F3C7F2CFFBD7A3EB8B6FEC35C7FD67F26DDF6285A644F740A2614")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002FA3 RID: 12195
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2pnb304w1Holder();
		}

		// Token: 0x020008BE RID: 2238
		internal class C2tnb359v1Holder : X9ECParametersHolder
		{
			// Token: 0x06004611 RID: 17937 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2tnb359v1Holder()
			{
			}

			// Token: 0x06004612 RID: 17938 RVA: 0x001461E4 File Offset: 0x001443E4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("01AF286BCA1AF286BCA1AF286BCA1AF286BCA1AF286BC9FB8F6B85C556892C20A7EB964FE7719E74F490758D3B", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(76L);
				F2mCurve f2mCurve = new F2mCurve(359, 68, new BigInteger("5667676A654B20754F356EA92017D946567C46675556F19556A04616B567D223A5E05656FB549016A96656A557", 16), new BigInteger("2472E2D0197C49363F1FE7F5B6DB075D52B6947D135D8CA445805D39BC345626089687742B6329E70680231988", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("033C258EF3047767E7EDE0F1FDAA79DAEE3841366A132E163ACED4ED2401DF9C6BDCDE98E8E707C07A2239B1B097")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002FA4 RID: 12196
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2tnb359v1Holder();
		}

		// Token: 0x020008BF RID: 2239
		internal class C2pnb368w1Holder : X9ECParametersHolder
		{
			// Token: 0x06004614 RID: 17940 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2pnb368w1Holder()
			{
			}

			// Token: 0x06004615 RID: 17941 RVA: 0x00146254 File Offset: 0x00144454
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("010090512DA9AF72B08349D98A5DD4C7B0532ECA51CE03E2D10F3B7AC579BD87E909AE40A6F131E9CFCE5BD967", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(65392L);
				F2mCurve f2mCurve = new F2mCurve(368, 1, 2, 85, new BigInteger("00E0D2EE25095206F5E2A4F9ED229F1F256E79A0E2B455970D8D0D865BD94778C576D62F0AB7519CCD2A1A906AE30D", 16), new BigInteger("00FC1217D4320A90452C760A58EDCD30C8DD069B3C34453837A34ED50CB54917E1C2112D84D164F444F8F74786046A", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("021085E2755381DCCCE3C1557AFA10C2F0C0C2825646C5B34A394CBCFA8BC16B22E7E789E927BE216F02E1FB136A5F")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002FA5 RID: 12197
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2pnb368w1Holder();
		}

		// Token: 0x020008C0 RID: 2240
		internal class C2tnb431r1Holder : X9ECParametersHolder
		{
			// Token: 0x06004617 RID: 17943 RVA: 0x00144B59 File Offset: 0x00142D59
			private C2tnb431r1Holder()
			{
			}

			// Token: 0x06004618 RID: 17944 RVA: 0x001462C8 File Offset: 0x001444C8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger bigInteger = new BigInteger("0340340340340340340340340340340340340340340340340340340323C313FAB50589703B5EC68D3587FEC60D161CC149C1AD4A91", 16);
				BigInteger bigInteger2 = BigInteger.ValueOf(10080L);
				F2mCurve f2mCurve = new F2mCurve(431, 120, new BigInteger("1A827EF00DD6FC0E234CAF046C6A5D8A85395B236CC4AD2CF32A0CADBDC9DDF620B0EB9906D0957F6C6FEACD615468DF104DE296CD8F", 16), new BigInteger("10D9B4A3D9047D8B154359ABFB1B7F5485B04CEB868237DDC9DEDA982A679A5A919B626D4E50A8DD731B107A9962381FB5D807BF2618", 16), bigInteger, bigInteger2);
				return new X9ECParameters(f2mCurve, new X9ECPoint(f2mCurve, Hex.Decode("02120FC05D3C67A99DE161D2F4092622FECA701BE4F50F4758714E8A87BBF2A658EF8C21E7C5EFE965361F6C2999C0C247B0DBD70CE6B7")), bigInteger, bigInteger2, null);
			}

			// Token: 0x04002FA6 RID: 12198
			internal static readonly X9ECParametersHolder Instance = new X962NamedCurves.C2tnb431r1Holder();
		}
	}
}

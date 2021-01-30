using System;
using System.Collections;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000414 RID: 1044
	public class RsaDigestSigner : ISigner
	{
		// Token: 0x060026C8 RID: 9928 RVA: 0x000C38F0 File Offset: 0x000C1AF0
		static RsaDigestSigner()
		{
			RsaDigestSigner.oidMap["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
			RsaDigestSigner.oidMap["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
			RsaDigestSigner.oidMap["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
			RsaDigestSigner.oidMap["SHA-1"] = X509ObjectIdentifiers.IdSha1;
			RsaDigestSigner.oidMap["SHA-224"] = NistObjectIdentifiers.IdSha224;
			RsaDigestSigner.oidMap["SHA-256"] = NistObjectIdentifiers.IdSha256;
			RsaDigestSigner.oidMap["SHA-384"] = NistObjectIdentifiers.IdSha384;
			RsaDigestSigner.oidMap["SHA-512"] = NistObjectIdentifiers.IdSha512;
			RsaDigestSigner.oidMap["MD2"] = PkcsObjectIdentifiers.MD2;
			RsaDigestSigner.oidMap["MD4"] = PkcsObjectIdentifiers.MD4;
			RsaDigestSigner.oidMap["MD5"] = PkcsObjectIdentifiers.MD5;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x000C39E3 File Offset: 0x000C1BE3
		public RsaDigestSigner(IDigest digest) : this(digest, (DerObjectIdentifier)RsaDigestSigner.oidMap[digest.AlgorithmName])
		{
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000C3A01 File Offset: 0x000C1C01
		public RsaDigestSigner(IDigest digest, DerObjectIdentifier digestOid) : this(digest, new AlgorithmIdentifier(digestOid, DerNull.Instance))
		{
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000C3A15 File Offset: 0x000C1C15
		public RsaDigestSigner(IDigest digest, AlgorithmIdentifier algId)
		{
			this.digest = digest;
			this.algId = algId;
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x000C3A3B File Offset: 0x000C1C3B
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "withRSA";
			}
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x000C3A54 File Offset: 0x000C1C54
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			AsymmetricKeyParameter asymmetricKeyParameter;
			if (parameters is ParametersWithRandom)
			{
				asymmetricKeyParameter = (AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters;
			}
			else
			{
				asymmetricKeyParameter = (AsymmetricKeyParameter)parameters;
			}
			if (forSigning && !asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Signing requires private key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification requires public key.");
			}
			this.Reset();
			this.rsaEngine.Init(forSigning, parameters);
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x000C3AC9 File Offset: 0x000C1CC9
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x000C3AD7 File Offset: 0x000C1CD7
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.digest.BlockUpdate(input, inOff, length);
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x000C3AE8 File Offset: 0x000C1CE8
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("RsaDigestSigner not initialised for signature generation.");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			byte[] array2 = this.DerEncode(array);
			return this.rsaEngine.ProcessBlock(array2, 0, array2.Length);
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x000C3B40 File Offset: 0x000C1D40
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("RsaDigestSigner not initialised for verification");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			byte[] array2;
			byte[] array3;
			try
			{
				array2 = this.rsaEngine.ProcessBlock(signature, 0, signature.Length);
				array3 = this.DerEncode(array);
			}
			catch (Exception)
			{
				return false;
			}
			if (array2.Length == array3.Length)
			{
				return Arrays.ConstantTimeAreEqual(array2, array3);
			}
			if (array2.Length == array3.Length - 2)
			{
				int num = array2.Length - array.Length - 2;
				int num2 = array3.Length - array.Length - 2;
				byte[] array4 = array3;
				int num3 = 1;
				array4[num3] -= 2;
				byte[] array5 = array3;
				int num4 = 3;
				array5[num4] -= 2;
				int num5 = 0;
				for (int i = 0; i < array.Length; i++)
				{
					num5 |= (int)(array2[num + i] ^ array3[num2 + i]);
				}
				for (int j = 0; j < num; j++)
				{
					num5 |= (int)(array2[j] ^ array3[j]);
				}
				return num5 == 0;
			}
			return false;
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x000C3C50 File Offset: 0x000C1E50
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x000C3C5D File Offset: 0x000C1E5D
		private byte[] DerEncode(byte[] hash)
		{
			if (this.algId == null)
			{
				return hash;
			}
			return new DigestInfo(this.algId, hash).GetDerEncoded();
		}

		// Token: 0x040019F5 RID: 6645
		private readonly IAsymmetricBlockCipher rsaEngine = new Pkcs1Encoding(new RsaBlindedEngine());

		// Token: 0x040019F6 RID: 6646
		private readonly AlgorithmIdentifier algId;

		// Token: 0x040019F7 RID: 6647
		private readonly IDigest digest;

		// Token: 0x040019F8 RID: 6648
		private bool forSigning;

		// Token: 0x040019F9 RID: 6649
		private static readonly IDictionary oidMap = Platform.CreateHashtable();
	}
}

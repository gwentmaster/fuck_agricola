using System;
using System.IO;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000406 RID: 1030
	public class DsaDigestSigner : ISigner
	{
		// Token: 0x06002654 RID: 9812 RVA: 0x000C0FBF File Offset: 0x000BF1BF
		public DsaDigestSigner(IDsa signer, IDigest digest)
		{
			this.digest = digest;
			this.dsaSigner = signer;
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06002655 RID: 9813 RVA: 0x000C0FD5 File Offset: 0x000BF1D5
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "with" + this.dsaSigner.AlgorithmName;
			}
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000C0FF8 File Offset: 0x000BF1F8
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
				throw new InvalidKeyException("Signing Requires Private Key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification Requires Public Key.");
			}
			this.Reset();
			this.dsaSigner.Init(forSigning, parameters);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000C106D File Offset: 0x000BF26D
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000C107B File Offset: 0x000BF27B
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.digest.BlockUpdate(input, inOff, length);
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000C108C File Offset: 0x000BF28C
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("DSADigestSigner not initialised for signature generation.");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			BigInteger[] array2 = this.dsaSigner.GenerateSignature(array);
			return this.DerEncode(array2[0], array2[1]);
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000C10E4 File Offset: 0x000BF2E4
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("DSADigestSigner not initialised for verification");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			bool result;
			try
			{
				BigInteger[] array2 = this.DerDecode(signature);
				result = this.dsaSigner.VerifySignature(array, array2[0], array2[1]);
			}
			catch (IOException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000C1158 File Offset: 0x000BF358
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000C1165 File Offset: 0x000BF365
		private byte[] DerEncode(BigInteger r, BigInteger s)
		{
			return new DerSequence(new Asn1Encodable[]
			{
				new DerInteger(r),
				new DerInteger(s)
			}).GetDerEncoded();
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000C118C File Offset: 0x000BF38C
		private BigInteger[] DerDecode(byte[] encoding)
		{
			Asn1Sequence asn1Sequence = (Asn1Sequence)Asn1Object.FromByteArray(encoding);
			return new BigInteger[]
			{
				((DerInteger)asn1Sequence[0]).Value,
				((DerInteger)asn1Sequence[1]).Value
			};
		}

		// Token: 0x040019AA RID: 6570
		private readonly IDigest digest;

		// Token: 0x040019AB RID: 6571
		private readonly IDsa dsaSigner;

		// Token: 0x040019AC RID: 6572
		private bool forSigning;
	}
}

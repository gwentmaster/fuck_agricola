using System;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200038D RID: 909
	public abstract class PbeParametersGenerator
	{
		// Token: 0x06002239 RID: 8761 RVA: 0x000B5201 File Offset: 0x000B3401
		public virtual void Init(byte[] password, byte[] salt, int iterationCount)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			if (salt == null)
			{
				throw new ArgumentNullException("salt");
			}
			this.mPassword = Arrays.Clone(password);
			this.mSalt = Arrays.Clone(salt);
			this.mIterationCount = iterationCount;
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x000B523E File Offset: 0x000B343E
		public virtual byte[] Password
		{
			get
			{
				return Arrays.Clone(this.mPassword);
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000B524B File Offset: 0x000B344B
		[Obsolete("Use 'Password' property")]
		public byte[] GetPassword()
		{
			return this.Password;
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x000B5253 File Offset: 0x000B3453
		public virtual byte[] Salt
		{
			get
			{
				return Arrays.Clone(this.mSalt);
			}
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000B5260 File Offset: 0x000B3460
		[Obsolete("Use 'Salt' property")]
		public byte[] GetSalt()
		{
			return this.Salt;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x000B5268 File Offset: 0x000B3468
		public virtual int IterationCount
		{
			get
			{
				return this.mIterationCount;
			}
		}

		// Token: 0x0600223F RID: 8767
		[Obsolete("Use version with 'algorithm' parameter")]
		public abstract ICipherParameters GenerateDerivedParameters(int keySize);

		// Token: 0x06002240 RID: 8768
		public abstract ICipherParameters GenerateDerivedParameters(string algorithm, int keySize);

		// Token: 0x06002241 RID: 8769
		[Obsolete("Use version with 'algorithm' parameter")]
		public abstract ICipherParameters GenerateDerivedParameters(int keySize, int ivSize);

		// Token: 0x06002242 RID: 8770
		public abstract ICipherParameters GenerateDerivedParameters(string algorithm, int keySize, int ivSize);

		// Token: 0x06002243 RID: 8771
		public abstract ICipherParameters GenerateDerivedMacParameters(int keySize);

		// Token: 0x06002244 RID: 8772 RVA: 0x000B5270 File Offset: 0x000B3470
		public static byte[] Pkcs5PasswordToBytes(char[] password)
		{
			if (password == null)
			{
				return new byte[0];
			}
			return Strings.ToByteArray(password);
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000B5282 File Offset: 0x000B3482
		[Obsolete("Use version taking 'char[]' instead")]
		public static byte[] Pkcs5PasswordToBytes(string password)
		{
			if (password == null)
			{
				return new byte[0];
			}
			return Strings.ToByteArray(password);
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000B5294 File Offset: 0x000B3494
		public static byte[] Pkcs5PasswordToUtf8Bytes(char[] password)
		{
			if (password == null)
			{
				return new byte[0];
			}
			return Encoding.UTF8.GetBytes(password);
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000B52AB File Offset: 0x000B34AB
		[Obsolete("Use version taking 'char[]' instead")]
		public static byte[] Pkcs5PasswordToUtf8Bytes(string password)
		{
			if (password == null)
			{
				return new byte[0];
			}
			return Encoding.UTF8.GetBytes(password);
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000B52C2 File Offset: 0x000B34C2
		public static byte[] Pkcs12PasswordToBytes(char[] password)
		{
			return PbeParametersGenerator.Pkcs12PasswordToBytes(password, false);
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x000B52CC File Offset: 0x000B34CC
		public static byte[] Pkcs12PasswordToBytes(char[] password, bool wrongPkcs12Zero)
		{
			if (password == null || password.Length < 1)
			{
				return new byte[wrongPkcs12Zero ? 2 : 0];
			}
			byte[] array = new byte[(password.Length + 1) * 2];
			Encoding.BigEndianUnicode.GetBytes(password, 0, password.Length, array, 0);
			return array;
		}

		// Token: 0x0400168E RID: 5774
		protected byte[] mPassword;

		// Token: 0x0400168F RID: 5775
		protected byte[] mSalt;

		// Token: 0x04001690 RID: 5776
		protected int mIterationCount;
	}
}

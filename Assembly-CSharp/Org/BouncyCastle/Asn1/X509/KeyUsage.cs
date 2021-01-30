using System;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200052D RID: 1325
	public class KeyUsage : DerBitString
	{
		// Token: 0x0600305E RID: 12382 RVA: 0x000F7BB4 File Offset: 0x000F5DB4
		public new static KeyUsage GetInstance(object obj)
		{
			if (obj is KeyUsage)
			{
				return (KeyUsage)obj;
			}
			if (obj is X509Extension)
			{
				return KeyUsage.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			return new KeyUsage(DerBitString.GetInstance(obj));
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000F0A6C File Offset: 0x000EEC6C
		public KeyUsage(int usage) : base(usage)
		{
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000F7BE9 File Offset: 0x000F5DE9
		private KeyUsage(DerBitString usage) : base(usage.GetBytes(), usage.PadBits)
		{
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000F7C00 File Offset: 0x000F5E00
		public override string ToString()
		{
			byte[] bytes = this.GetBytes();
			if (bytes.Length == 1)
			{
				return "KeyUsage: 0x" + ((int)(bytes[0] & byte.MaxValue)).ToString("X");
			}
			return "KeyUsage: 0x" + ((int)(bytes[1] & byte.MaxValue) << 8 | (int)(bytes[0] & byte.MaxValue)).ToString("X");
		}

		// Token: 0x04001EE4 RID: 7908
		public const int DigitalSignature = 128;

		// Token: 0x04001EE5 RID: 7909
		public const int NonRepudiation = 64;

		// Token: 0x04001EE6 RID: 7910
		public const int KeyEncipherment = 32;

		// Token: 0x04001EE7 RID: 7911
		public const int DataEncipherment = 16;

		// Token: 0x04001EE8 RID: 7912
		public const int KeyAgreement = 8;

		// Token: 0x04001EE9 RID: 7913
		public const int KeyCertSign = 4;

		// Token: 0x04001EEA RID: 7914
		public const int CrlSign = 2;

		// Token: 0x04001EEB RID: 7915
		public const int EncipherOnly = 1;

		// Token: 0x04001EEC RID: 7916
		public const int DecipherOnly = 32768;
	}
}

using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200039A RID: 922
	public abstract class AlertDescription
	{
		// Token: 0x060022EA RID: 8938 RVA: 0x000B6198 File Offset: 0x000B4398
		public static string GetName(byte alertDescription)
		{
			if (alertDescription <= 70)
			{
				if (alertDescription <= 22)
				{
					if (alertDescription == 0)
					{
						return "close_notify";
					}
					if (alertDescription == 10)
					{
						return "unexpected_message";
					}
					switch (alertDescription)
					{
					case 20:
						return "bad_record_mac";
					case 21:
						return "decryption_failed";
					case 22:
						return "record_overflow";
					}
				}
				else
				{
					switch (alertDescription)
					{
					case 30:
						return "decompression_failure";
					case 31:
					case 32:
					case 33:
					case 34:
					case 35:
					case 36:
					case 37:
					case 38:
					case 39:
						break;
					case 40:
						return "handshake_failure";
					case 41:
						return "no_certificate";
					case 42:
						return "bad_certificate";
					case 43:
						return "unsupported_certificate";
					case 44:
						return "certificate_revoked";
					case 45:
						return "certificate_expired";
					case 46:
						return "certificate_unknown";
					case 47:
						return "illegal_parameter";
					case 48:
						return "unknown_ca";
					case 49:
						return "access_denied";
					case 50:
						return "decode_error";
					case 51:
						return "decrypt_error";
					default:
						if (alertDescription == 60)
						{
							return "export_restriction";
						}
						if (alertDescription == 70)
						{
							return "protocol_version";
						}
						break;
					}
				}
			}
			else if (alertDescription <= 86)
			{
				if (alertDescription == 71)
				{
					return "insufficient_security";
				}
				if (alertDescription == 80)
				{
					return "internal_error";
				}
				if (alertDescription == 86)
				{
					return "inappropriate_fallback";
				}
			}
			else
			{
				if (alertDescription == 90)
				{
					return "user_canceled";
				}
				if (alertDescription == 100)
				{
					return "no_renegotiation";
				}
				switch (alertDescription)
				{
				case 110:
					return "unsupported_extension";
				case 111:
					return "certificate_unobtainable";
				case 112:
					return "unrecognized_name";
				case 113:
					return "bad_certificate_status_response";
				case 114:
					return "bad_certificate_hash_value";
				case 115:
					return "unknown_psk_identity";
				}
			}
			return "UNKNOWN";
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000B6367 File Offset: 0x000B4567
		public static string GetText(byte alertDescription)
		{
			return string.Concat(new object[]
			{
				AlertDescription.GetName(alertDescription),
				"(",
				alertDescription,
				")"
			});
		}

		// Token: 0x040016B8 RID: 5816
		public const byte close_notify = 0;

		// Token: 0x040016B9 RID: 5817
		public const byte unexpected_message = 10;

		// Token: 0x040016BA RID: 5818
		public const byte bad_record_mac = 20;

		// Token: 0x040016BB RID: 5819
		public const byte decryption_failed = 21;

		// Token: 0x040016BC RID: 5820
		public const byte record_overflow = 22;

		// Token: 0x040016BD RID: 5821
		public const byte decompression_failure = 30;

		// Token: 0x040016BE RID: 5822
		public const byte handshake_failure = 40;

		// Token: 0x040016BF RID: 5823
		public const byte no_certificate = 41;

		// Token: 0x040016C0 RID: 5824
		public const byte bad_certificate = 42;

		// Token: 0x040016C1 RID: 5825
		public const byte unsupported_certificate = 43;

		// Token: 0x040016C2 RID: 5826
		public const byte certificate_revoked = 44;

		// Token: 0x040016C3 RID: 5827
		public const byte certificate_expired = 45;

		// Token: 0x040016C4 RID: 5828
		public const byte certificate_unknown = 46;

		// Token: 0x040016C5 RID: 5829
		public const byte illegal_parameter = 47;

		// Token: 0x040016C6 RID: 5830
		public const byte unknown_ca = 48;

		// Token: 0x040016C7 RID: 5831
		public const byte access_denied = 49;

		// Token: 0x040016C8 RID: 5832
		public const byte decode_error = 50;

		// Token: 0x040016C9 RID: 5833
		public const byte decrypt_error = 51;

		// Token: 0x040016CA RID: 5834
		public const byte export_restriction = 60;

		// Token: 0x040016CB RID: 5835
		public const byte protocol_version = 70;

		// Token: 0x040016CC RID: 5836
		public const byte insufficient_security = 71;

		// Token: 0x040016CD RID: 5837
		public const byte internal_error = 80;

		// Token: 0x040016CE RID: 5838
		public const byte user_canceled = 90;

		// Token: 0x040016CF RID: 5839
		public const byte no_renegotiation = 100;

		// Token: 0x040016D0 RID: 5840
		public const byte unsupported_extension = 110;

		// Token: 0x040016D1 RID: 5841
		public const byte certificate_unobtainable = 111;

		// Token: 0x040016D2 RID: 5842
		public const byte unrecognized_name = 112;

		// Token: 0x040016D3 RID: 5843
		public const byte bad_certificate_status_response = 113;

		// Token: 0x040016D4 RID: 5844
		public const byte bad_certificate_hash_value = 114;

		// Token: 0x040016D5 RID: 5845
		public const byte unknown_psk_identity = 115;

		// Token: 0x040016D6 RID: 5846
		public const byte inappropriate_fallback = 86;
	}
}

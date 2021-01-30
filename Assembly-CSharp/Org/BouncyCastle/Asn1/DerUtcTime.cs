using System;
using System.Globalization;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000503 RID: 1283
	public class DerUtcTime : Asn1Object
	{
		// Token: 0x06002F37 RID: 12087 RVA: 0x000F40AF File Offset: 0x000F22AF
		public static DerUtcTime GetInstance(object obj)
		{
			if (obj == null || obj is DerUtcTime)
			{
				return (DerUtcTime)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x000F40D8 File Offset: 0x000F22D8
		public static DerUtcTime GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerUtcTime)
			{
				return DerUtcTime.GetInstance(@object);
			}
			return new DerUtcTime(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x000F4110 File Offset: 0x000F2310
		public DerUtcTime(string time)
		{
			if (time == null)
			{
				throw new ArgumentNullException("time");
			}
			this.time = time;
			try
			{
				this.ToDateTime();
			}
			catch (FormatException ex)
			{
				throw new ArgumentException("invalid date string: " + ex.Message);
			}
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000F4168 File Offset: 0x000F2368
		public DerUtcTime(DateTime time)
		{
			this.time = time.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture) + "Z";
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000F4191 File Offset: 0x000F2391
		internal DerUtcTime(byte[] bytes)
		{
			this.time = Strings.FromAsciiByteArray(bytes);
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000F41A5 File Offset: 0x000F23A5
		public DateTime ToDateTime()
		{
			return this.ParseDateString(this.TimeString, "yyMMddHHmmss'GMT'zzz");
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000F41B8 File Offset: 0x000F23B8
		public DateTime ToAdjustedDateTime()
		{
			return this.ParseDateString(this.AdjustedTimeString, "yyyyMMddHHmmss'GMT'zzz");
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x000F41CC File Offset: 0x000F23CC
		private DateTime ParseDateString(string dateStr, string formatStr)
		{
			return DateTime.ParseExact(dateStr, formatStr, DateTimeFormatInfo.InvariantInfo).ToUniversalTime();
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06002F3F RID: 12095 RVA: 0x000F41F0 File Offset: 0x000F23F0
		public string TimeString
		{
			get
			{
				if (this.time.IndexOf('-') < 0 && this.time.IndexOf('+') < 0)
				{
					if (this.time.Length == 11)
					{
						return this.time.Substring(0, 10) + "00GMT+00:00";
					}
					return this.time.Substring(0, 12) + "GMT+00:00";
				}
				else
				{
					int num = this.time.IndexOf('-');
					if (num < 0)
					{
						num = this.time.IndexOf('+');
					}
					string text = this.time;
					if (num == this.time.Length - 3)
					{
						text += "00";
					}
					if (num == 10)
					{
						return string.Concat(new string[]
						{
							text.Substring(0, 10),
							"00GMT",
							text.Substring(10, 3),
							":",
							text.Substring(13, 2)
						});
					}
					return string.Concat(new string[]
					{
						text.Substring(0, 12),
						"GMT",
						text.Substring(12, 3),
						":",
						text.Substring(15, 2)
					});
				}
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x000F4325 File Offset: 0x000F2525
		[Obsolete("Use 'AdjustedTimeString' property instead")]
		public string AdjustedTime
		{
			get
			{
				return this.AdjustedTimeString;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002F41 RID: 12097 RVA: 0x000F4330 File Offset: 0x000F2530
		public string AdjustedTimeString
		{
			get
			{
				string timeString = this.TimeString;
				return ((timeString[0] < '5') ? "20" : "19") + timeString;
			}
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000F4361 File Offset: 0x000F2561
		private byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.time);
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000F436E File Offset: 0x000F256E
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(23, this.GetOctets());
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000F4380 File Offset: 0x000F2580
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerUtcTime derUtcTime = asn1Object as DerUtcTime;
			return derUtcTime != null && this.time.Equals(derUtcTime.time);
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000F43AA File Offset: 0x000F25AA
		protected override int Asn1GetHashCode()
		{
			return this.time.GetHashCode();
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000F43B7 File Offset: 0x000F25B7
		public override string ToString()
		{
			return this.time;
		}

		// Token: 0x04001E46 RID: 7750
		private readonly string time;
	}
}

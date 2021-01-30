using System;
using System.Globalization;
using System.Text;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004F4 RID: 1268
	public class DerGeneralizedTime : Asn1Object
	{
		// Token: 0x06002EAA RID: 11946 RVA: 0x000F2936 File Offset: 0x000F0B36
		public static DerGeneralizedTime GetInstance(object obj)
		{
			if (obj == null || obj is DerGeneralizedTime)
			{
				return (DerGeneralizedTime)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x000F2964 File Offset: 0x000F0B64
		public static DerGeneralizedTime GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerGeneralizedTime)
			{
				return DerGeneralizedTime.GetInstance(@object);
			}
			return new DerGeneralizedTime(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000F299C File Offset: 0x000F0B9C
		public DerGeneralizedTime(string time)
		{
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

		// Token: 0x06002EAD RID: 11949 RVA: 0x000F29E8 File Offset: 0x000F0BE8
		public DerGeneralizedTime(DateTime time)
		{
			this.time = time.ToString("yyyyMMddHHmmss\\Z");
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x000F2A02 File Offset: 0x000F0C02
		internal DerGeneralizedTime(byte[] bytes)
		{
			this.time = Strings.FromAsciiByteArray(bytes);
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x000F2A16 File Offset: 0x000F0C16
		public string TimeString
		{
			get
			{
				return this.time;
			}
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000F2A20 File Offset: 0x000F0C20
		public string GetTime()
		{
			if (this.time[this.time.Length - 1] == 'Z')
			{
				return this.time.Substring(0, this.time.Length - 1) + "GMT+00:00";
			}
			int num = this.time.Length - 5;
			char c = this.time[num];
			if (c == '-' || c == '+')
			{
				return string.Concat(new string[]
				{
					this.time.Substring(0, num),
					"GMT",
					this.time.Substring(num, 3),
					":",
					this.time.Substring(num + 3)
				});
			}
			num = this.time.Length - 3;
			c = this.time[num];
			if (c == '-' || c == '+')
			{
				return this.time.Substring(0, num) + "GMT" + this.time.Substring(num) + ":00";
			}
			return this.time + this.CalculateGmtOffset();
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000F2B40 File Offset: 0x000F0D40
		private string CalculateGmtOffset()
		{
			char c = '+';
			DateTime dateTime = this.ToDateTime();
			TimeSpan timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
			if (timeSpan.CompareTo(TimeSpan.Zero) < 0)
			{
				c = '-';
				timeSpan = timeSpan.Duration();
			}
			int hours = timeSpan.Hours;
			int minutes = timeSpan.Minutes;
			return string.Concat(new string[]
			{
				"GMT",
				c.ToString(),
				DerGeneralizedTime.Convert(hours),
				":",
				DerGeneralizedTime.Convert(minutes)
			});
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000F2BC6 File Offset: 0x000F0DC6
		private static string Convert(int time)
		{
			if (time < 10)
			{
				return "0" + time;
			}
			return time.ToString();
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000F2BE8 File Offset: 0x000F0DE8
		public DateTime ToDateTime()
		{
			string text = this.time;
			bool makeUniversal = false;
			string format;
			if (Platform.EndsWith(text, "Z"))
			{
				if (this.HasFractionalSeconds)
				{
					int count = text.Length - text.IndexOf('.') - 2;
					format = "yyyyMMddHHmmss." + this.FString(count) + "\\Z";
				}
				else
				{
					format = "yyyyMMddHHmmss\\Z";
				}
			}
			else if (this.time.IndexOf('-') > 0 || this.time.IndexOf('+') > 0)
			{
				text = this.GetTime();
				makeUniversal = true;
				if (this.HasFractionalSeconds)
				{
					int count2 = Platform.IndexOf(text, "GMT") - 1 - text.IndexOf('.');
					format = "yyyyMMddHHmmss." + this.FString(count2) + "'GMT'zzz";
				}
				else
				{
					format = "yyyyMMddHHmmss'GMT'zzz";
				}
			}
			else if (this.HasFractionalSeconds)
			{
				int count3 = text.Length - 1 - text.IndexOf('.');
				format = "yyyyMMddHHmmss." + this.FString(count3);
			}
			else
			{
				format = "yyyyMMddHHmmss";
			}
			return this.ParseDateString(text, format, makeUniversal);
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000F2CF8 File Offset: 0x000F0EF8
		private string FString(int count)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				stringBuilder.Append('f');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x000F2D28 File Offset: 0x000F0F28
		private DateTime ParseDateString(string s, string format, bool makeUniversal)
		{
			DateTimeStyles dateTimeStyles = DateTimeStyles.None;
			if (Platform.EndsWith(format, "Z"))
			{
				try
				{
					dateTimeStyles = (DateTimeStyles)Enums.GetEnumValue(typeof(DateTimeStyles), "AssumeUniversal");
				}
				catch (Exception)
				{
				}
				dateTimeStyles |= DateTimeStyles.AdjustToUniversal;
			}
			DateTime result = DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, dateTimeStyles);
			if (!makeUniversal)
			{
				return result;
			}
			return result.ToUniversalTime();
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x000F2D94 File Offset: 0x000F0F94
		private bool HasFractionalSeconds
		{
			get
			{
				return this.time.IndexOf('.') == 14;
			}
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x000F2DA7 File Offset: 0x000F0FA7
		private byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.time);
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000F2DB4 File Offset: 0x000F0FB4
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(24, this.GetOctets());
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000F2DC4 File Offset: 0x000F0FC4
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerGeneralizedTime derGeneralizedTime = asn1Object as DerGeneralizedTime;
			return derGeneralizedTime != null && this.time.Equals(derGeneralizedTime.time);
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000F2DEE File Offset: 0x000F0FEE
		protected override int Asn1GetHashCode()
		{
			return this.time.GetHashCode();
		}

		// Token: 0x04001E37 RID: 7735
		private readonly string time;
	}
}

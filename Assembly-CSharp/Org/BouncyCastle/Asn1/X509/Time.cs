using System;
using System.Globalization;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000534 RID: 1332
	public class Time : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003096 RID: 12438 RVA: 0x000F83F6 File Offset: 0x000F65F6
		public static Time GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Time.GetInstance(obj.GetObject());
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000F8403 File Offset: 0x000F6603
		public Time(Asn1Object time)
		{
			if (time == null)
			{
				throw new ArgumentNullException("time");
			}
			if (!(time is DerUtcTime) && !(time is DerGeneralizedTime))
			{
				throw new ArgumentException("unknown object passed to Time");
			}
			this.time = time;
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x000F843C File Offset: 0x000F663C
		public Time(DateTime date)
		{
			string text = date.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + "Z";
			int num = int.Parse(text.Substring(0, 4));
			if (num < 1950 || num > 2049)
			{
				this.time = new DerGeneralizedTime(text);
				return;
			}
			this.time = new DerUtcTime(text.Substring(2));
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000F84A8 File Offset: 0x000F66A8
		public static Time GetInstance(object obj)
		{
			if (obj == null || obj is Time)
			{
				return (Time)obj;
			}
			if (obj is DerUtcTime)
			{
				return new Time((DerUtcTime)obj);
			}
			if (obj is DerGeneralizedTime)
			{
				return new Time((DerGeneralizedTime)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000F8509 File Offset: 0x000F6709
		public string GetTime()
		{
			if (this.time is DerUtcTime)
			{
				return ((DerUtcTime)this.time).AdjustedTimeString;
			}
			return ((DerGeneralizedTime)this.time).GetTime();
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x000F853C File Offset: 0x000F673C
		public DateTime ToDateTime()
		{
			DateTime result;
			try
			{
				if (this.time is DerUtcTime)
				{
					result = ((DerUtcTime)this.time).ToAdjustedDateTime();
				}
				else
				{
					result = ((DerGeneralizedTime)this.time).ToDateTime();
				}
			}
			catch (FormatException ex)
			{
				throw new InvalidOperationException("invalid date string: " + ex.Message);
			}
			return result;
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x000F85A4 File Offset: 0x000F67A4
		public override Asn1Object ToAsn1Object()
		{
			return this.time;
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x000F85AC File Offset: 0x000F67AC
		public override string ToString()
		{
			return this.GetTime();
		}

		// Token: 0x04001F12 RID: 7954
		private readonly Asn1Object time;
	}
}

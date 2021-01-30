using System;

namespace BestHTTP.Logger
{
	// Token: 0x020005E6 RID: 1510
	public interface ILogger
	{
		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06003768 RID: 14184
		// (set) Token: 0x06003769 RID: 14185
		Loglevels Level { get; set; }

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600376A RID: 14186
		// (set) Token: 0x0600376B RID: 14187
		string FormatVerbose { get; set; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x0600376C RID: 14188
		// (set) Token: 0x0600376D RID: 14189
		string FormatInfo { get; set; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600376E RID: 14190
		// (set) Token: 0x0600376F RID: 14191
		string FormatWarn { get; set; }

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06003770 RID: 14192
		// (set) Token: 0x06003771 RID: 14193
		string FormatErr { get; set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06003772 RID: 14194
		// (set) Token: 0x06003773 RID: 14195
		string FormatEx { get; set; }

		// Token: 0x06003774 RID: 14196
		void Verbose(string division, string verb);

		// Token: 0x06003775 RID: 14197
		void Information(string division, string info);

		// Token: 0x06003776 RID: 14198
		void Warning(string division, string warn);

		// Token: 0x06003777 RID: 14199
		void Error(string division, string err);

		// Token: 0x06003778 RID: 14200
		void Exception(string division, string msg, Exception ex);
	}
}

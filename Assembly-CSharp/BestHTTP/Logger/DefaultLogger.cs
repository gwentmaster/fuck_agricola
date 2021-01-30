using System;
using System.Text;
using UnityEngine;

namespace BestHTTP.Logger
{
	// Token: 0x020005E4 RID: 1508
	public class DefaultLogger : ILogger
	{
		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x001111AF File Offset: 0x0010F3AF
		// (set) Token: 0x06003757 RID: 14167 RVA: 0x001111B7 File Offset: 0x0010F3B7
		public Loglevels Level { get; set; }

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06003758 RID: 14168 RVA: 0x001111C0 File Offset: 0x0010F3C0
		// (set) Token: 0x06003759 RID: 14169 RVA: 0x001111C8 File Offset: 0x0010F3C8
		public string FormatVerbose { get; set; }

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600375A RID: 14170 RVA: 0x001111D1 File Offset: 0x0010F3D1
		// (set) Token: 0x0600375B RID: 14171 RVA: 0x001111D9 File Offset: 0x0010F3D9
		public string FormatInfo { get; set; }

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600375C RID: 14172 RVA: 0x001111E2 File Offset: 0x0010F3E2
		// (set) Token: 0x0600375D RID: 14173 RVA: 0x001111EA File Offset: 0x0010F3EA
		public string FormatWarn { get; set; }

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x001111F3 File Offset: 0x0010F3F3
		// (set) Token: 0x0600375F RID: 14175 RVA: 0x001111FB File Offset: 0x0010F3FB
		public string FormatErr { get; set; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x00111204 File Offset: 0x0010F404
		// (set) Token: 0x06003761 RID: 14177 RVA: 0x0011120C File Offset: 0x0010F40C
		public string FormatEx { get; set; }

		// Token: 0x06003762 RID: 14178 RVA: 0x00111218 File Offset: 0x0010F418
		public DefaultLogger()
		{
			this.FormatVerbose = "D [{0}]: {1}";
			this.FormatInfo = "I [{0}]: {1}";
			this.FormatWarn = "W [{0}]: {1}";
			this.FormatErr = "Err [{0}]: {1}";
			this.FormatEx = "Ex [{0}]: {1} - Message: {2}  StackTrace: {3}";
			this.Level = (Debug.isDebugBuild ? Loglevels.Warning : Loglevels.Error);
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x00111274 File Offset: 0x0010F474
		public void Verbose(string division, string verb)
		{
			if (this.Level <= Loglevels.All)
			{
				try
				{
					Debug.Log(string.Format(this.FormatVerbose, division, verb));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x001112B4 File Offset: 0x0010F4B4
		public void Information(string division, string info)
		{
			if (this.Level <= Loglevels.Information)
			{
				try
				{
					Debug.Log(string.Format(this.FormatInfo, division, info));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x001112F4 File Offset: 0x0010F4F4
		public void Warning(string division, string warn)
		{
			if (this.Level <= Loglevels.Warning)
			{
				try
				{
					Debug.LogWarning(string.Format(this.FormatWarn, division, warn));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x00111334 File Offset: 0x0010F534
		public void Error(string division, string err)
		{
			if (this.Level <= Loglevels.Error)
			{
				try
				{
					Debug.LogError(string.Format(this.FormatErr, division, err));
				}
				catch
				{
				}
			}
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x00111374 File Offset: 0x0010F574
		public void Exception(string division, string msg, Exception ex)
		{
			if (this.Level <= Loglevels.Exception)
			{
				try
				{
					string text = string.Empty;
					if (ex == null)
					{
						text = "null";
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();
						Exception ex2 = ex;
						int num = 1;
						while (ex2 != null)
						{
							stringBuilder.AppendFormat("{0}: {1} {2}", num++.ToString(), ex.Message, ex.StackTrace);
							ex2 = ex2.InnerException;
							if (ex2 != null)
							{
								stringBuilder.AppendLine();
							}
						}
						text = stringBuilder.ToString();
					}
					Debug.LogError(string.Format(this.FormatEx, new object[]
					{
						division,
						msg,
						text,
						(ex != null) ? ex.StackTrace : "null"
					}));
				}
				catch
				{
				}
			}
		}
	}
}

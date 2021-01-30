using System;
using System.Collections;
using MiniJSON;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200065E RID: 1630
	public static class AsmoLogger
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06003C1E RID: 15390 RVA: 0x0012A09A File Offset: 0x0012829A
		// (set) Token: 0x06003C1F RID: 15391 RVA: 0x0012A0A1 File Offset: 0x001282A1
		public static AsmoLogger.Severity LogLevel { get; set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06003C20 RID: 15392 RVA: 0x0012A0A9 File Offset: 0x001282A9
		// (set) Token: 0x06003C21 RID: 15393 RVA: 0x0012A0B0 File Offset: 0x001282B0
		public static bool IsDebugBuild { get; private set; } = UnityEngine.Debug.isDebugBuild;

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06003C22 RID: 15394 RVA: 0x0012A0B8 File Offset: 0x001282B8
		// (set) Token: 0x06003C23 RID: 15395 RVA: 0x0012A0BF File Offset: 0x001282BF
		public static bool EnableTestMode { get; set; } = false;

		// Token: 0x06003C24 RID: 15396 RVA: 0x0012A0C8 File Offset: 0x001282C8
		static AsmoLogger()
		{
			AsmoLogger.LogLevel = (AsmoLogger.IsDebugBuild ? AsmoLogger.Severity.Debug : AsmoLogger.Severity.Info);
			AsmoLogger.Info("AsmoLogger", "AsmoLogger initialized", new Hashtable
			{
				{
					"IsDebugBuild",
					AsmoLogger.IsDebugBuild
				}
			});
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x0012A11E File Offset: 0x0012831E
		public static void Trace(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Trace, module, message, extraInfo);
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x0012A129 File Offset: 0x00128329
		public static void Trace(string module, AsmoLogger.LazyString lazyMessage, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Trace, module, lazyMessage, extraInfo);
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x0012A134 File Offset: 0x00128334
		public static void Debug(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Debug, module, message, extraInfo);
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x0012A13F File Offset: 0x0012833F
		public static void Debug(string module, AsmoLogger.LazyString lazyMessage, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Debug, module, lazyMessage, extraInfo);
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x0012A14A File Offset: 0x0012834A
		public static void Info(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Info, module, message, extraInfo);
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x0012A155 File Offset: 0x00128355
		public static void Info(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Info, module, message, extraInfo);
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x0012A160 File Offset: 0x00128360
		public static void Notice(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Notice, module, message, extraInfo);
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x0012A16B File Offset: 0x0012836B
		public static void Notice(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Notice, module, message, extraInfo);
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x0012A176 File Offset: 0x00128376
		public static void Warning(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Warning, module, message, extraInfo);
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0012A181 File Offset: 0x00128381
		public static void Warning(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Warning, module, message, extraInfo);
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x0012A18C File Offset: 0x0012838C
		public static void Error(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Error, module, message, extraInfo);
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x0012A197 File Offset: 0x00128397
		public static void Error(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Error, module, message, extraInfo);
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x0012A1A2 File Offset: 0x001283A2
		public static void Fatal(string module, string message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Fatal, module, message, extraInfo);
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0012A1AD File Offset: 0x001283AD
		public static void Fatal(string module, AsmoLogger.LazyString message, Hashtable extraInfo = null)
		{
			AsmoLogger.Log(AsmoLogger.Severity.Fatal, module, message, extraInfo);
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x0012A1B8 File Offset: 0x001283B8
		public static bool CanLog(AsmoLogger.Severity severity)
		{
			return severity >= AsmoLogger.LogLevel;
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x0012A1C5 File Offset: 0x001283C5
		public static void Log(AsmoLogger.Severity severity, string module, AsmoLogger.LazyString lazyMessage, Hashtable extraInfo = null)
		{
			if (!AsmoLogger.CanLog(severity))
			{
				return;
			}
			AsmoLogger._Log(severity, module, lazyMessage(), extraInfo);
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x0012A1DE File Offset: 0x001283DE
		public static void Log(AsmoLogger.Severity severity, string module, string message, Hashtable extraInfo = null)
		{
			if (!AsmoLogger.CanLog(severity))
			{
				return;
			}
			AsmoLogger._Log(severity, module, message, extraInfo);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x0012A1F4 File Offset: 0x001283F4
		private static void _Log(AsmoLogger.Severity severity, string module, string message, Hashtable extraInfo = null)
		{
			string text = string.Format("[{0}][{1}][{2}] {3}", new object[]
			{
				DateTime.UtcNow.ToString("o"),
				severity,
				module,
				message
			});
			if (extraInfo != null && extraInfo.Count > 0)
			{
				string str = Json.Serialize(extraInfo);
				text = text + " " + str;
			}
			if (severity - AsmoLogger.Severity.Notice <= 1)
			{
				UnityEngine.Debug.LogWarning(text);
				return;
			}
			if (severity - AsmoLogger.Severity.Error > 1)
			{
				UnityEngine.Debug.Log(text);
				return;
			}
			if (AsmoLogger.EnableTestMode)
			{
				UnityEngine.Debug.LogWarning(text);
				return;
			}
			UnityEngine.Debug.LogError(text);
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x0012A288 File Offset: 0x00128488
		public static void LogException(Exception ex, string moduleName, AsmoLogger.Severity severity = AsmoLogger.Severity.Error)
		{
			string value = (ex.InnerException != null) ? ex.InnerException.GetType().ToString() : null;
			string value2 = (ex.InnerException != null) ? ex.InnerException.Message : null;
			AsmoLogger.Log(severity, moduleName, "Inner Exception", new Hashtable
			{
				{
					"type",
					ex.GetType().ToString()
				},
				{
					"message",
					ex.Message
				},
				{
					"inner_type",
					value
				},
				{
					"inner_message",
					value2
				},
				{
					"stack",
					ex.StackTrace
				}
			});
		}

		// Token: 0x0200097A RID: 2426
		public enum Severity
		{
			// Token: 0x040031E7 RID: 12775
			Trace,
			// Token: 0x040031E8 RID: 12776
			Debug,
			// Token: 0x040031E9 RID: 12777
			Info,
			// Token: 0x040031EA RID: 12778
			Notice,
			// Token: 0x040031EB RID: 12779
			Warning,
			// Token: 0x040031EC RID: 12780
			Error,
			// Token: 0x040031ED RID: 12781
			Fatal
		}

		// Token: 0x0200097B RID: 2427
		// (Invoke) Token: 0x060047E6 RID: 18406
		public delegate string LazyString();
	}
}

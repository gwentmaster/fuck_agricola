using System;
using Fabric.Crashlytics;
using Fabric.Internal.Runtime;
using UnityEngine;

namespace Fabric.Internal.Crashlytics
{
	// Token: 0x0200025B RID: 603
	public class CrashlyticsInit : MonoBehaviour
	{
		// Token: 0x06001307 RID: 4871 RVA: 0x00072014 File Offset: 0x00070214
		private void Awake()
		{
			if (CrashlyticsInit.instance == null)
			{
				this.AwakeOnce();
				CrashlyticsInit.instance = this;
				UnityEngine.Object.DontDestroyOnLoad(this);
				return;
			}
			if (CrashlyticsInit.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0007204E File Offset: 0x0007024E
		private void AwakeOnce()
		{
			CrashlyticsInit.RegisterExceptionHandlers();
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00072058 File Offset: 0x00070258
		private static void RegisterExceptionHandlers()
		{
			if (CrashlyticsInit.IsSDKInitialized())
			{
				Utils.Log(CrashlyticsInit.kitName, "Registering exception handlers");
				AppDomain.CurrentDomain.UnhandledException += CrashlyticsInit.HandleException;
				Application.logMessageReceived += CrashlyticsInit.HandleLog;
				return;
			}
			Utils.Log(CrashlyticsInit.kitName, "Did not register exception handlers: Crashlytics SDK was not initialized");
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0002A062 File Offset: 0x00028262
		private static bool IsSDKInitialized()
		{
			return false;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000720B4 File Offset: 0x000702B4
		private static void HandleException(object sender, UnhandledExceptionEventArgs eArgs)
		{
			Exception ex = (Exception)eArgs.ExceptionObject;
			CrashlyticsInit.HandleLog(ex.Message.ToString(), ex.StackTrace.ToString(), LogType.Exception);
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000720EC File Offset: 0x000702EC
		private static void HandleLog(string message, string stackTraceString, LogType type)
		{
			if (type == LogType.Exception)
			{
				Utils.Log(CrashlyticsInit.kitName, "Recording exception: " + message);
				Utils.Log(CrashlyticsInit.kitName, "Exception stack trace: " + stackTraceString);
				string[] messageParts = CrashlyticsInit.getMessageParts(message);
				Crashlytics.RecordCustomException(messageParts[0], messageParts[1], stackTraceString);
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0007213C File Offset: 0x0007033C
		private static string[] getMessageParts(string message)
		{
			char[] separator = new char[]
			{
				':'
			};
			string[] array = message.Split(separator, 2, StringSplitOptions.None);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Trim();
			}
			if (array.Length == 2)
			{
				return array;
			}
			return new string[]
			{
				"Exception",
				message
			};
		}

		// Token: 0x040012EA RID: 4842
		private static readonly string kitName = "Crashlytics";

		// Token: 0x040012EB RID: 4843
		private static CrashlyticsInit instance;
	}
}

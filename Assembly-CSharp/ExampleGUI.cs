using System;
using com.adjust.sdk;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class ExampleGUI : MonoBehaviour
{
	// Token: 0x06000014 RID: 20 RVA: 0x00002484 File Offset: 0x00000684
	private void OnGUI()
	{
		if (this.showPopUp)
		{
			GUI.Window(0, new Rect((float)(Screen.width / 2 - 150), (float)(Screen.height / 2 - 65), 300f, 130f), new GUI.WindowFunction(this.ShowGUI), "Is SDK enabled?");
		}
		float x = 0f;
		int height = Screen.height;
		if (GUI.Button(new Rect(x, (float)(0 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtManualLaunch) && !string.Equals(this.txtManualLaunch, "SDK Launched", StringComparison.OrdinalIgnoreCase))
		{
			AdjustConfig adjustConfig = new AdjustConfig("2fm9gkqubvpc", AdjustEnvironment.Sandbox);
			adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
			adjustConfig.setLogDelegate(delegate(string msg)
			{
				Debug.Log(msg);
			});
			adjustConfig.setEventSuccessDelegate(new Action<AdjustEventSuccess>(this.EventSuccessCallback), "Adjust");
			adjustConfig.setEventFailureDelegate(new Action<AdjustEventFailure>(this.EventFailureCallback), "Adjust");
			adjustConfig.setSessionSuccessDelegate(new Action<AdjustSessionSuccess>(this.SessionSuccessCallback), "Adjust");
			adjustConfig.setSessionFailureDelegate(new Action<AdjustSessionFailure>(this.SessionFailureCallback), "Adjust");
			adjustConfig.setDeferredDeeplinkDelegate(new Action<string>(this.DeferredDeeplinkCallback), "Adjust");
			adjustConfig.setAttributionChangedDelegate(new Action<AdjustAttribution>(this.AttributionChangedCallback), "Adjust");
			Adjust.start(adjustConfig);
			this.isEnabled = true;
			this.txtManualLaunch = "SDK Launched";
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Simple Event"))
		{
			Adjust.trackEvent(new AdjustEvent("g3mfiw"));
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 2 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Revenue Event"))
		{
			AdjustEvent adjustEvent = new AdjustEvent("a4fd35");
			adjustEvent.setRevenue(0.25, "EUR");
			Adjust.trackEvent(adjustEvent);
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 3 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Callback Event"))
		{
			AdjustEvent adjustEvent2 = new AdjustEvent("34vgg9");
			adjustEvent2.addCallbackParameter("key", "value");
			adjustEvent2.addCallbackParameter("foo", "bar");
			Adjust.trackEvent(adjustEvent2);
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 4 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Partner Event"))
		{
			AdjustEvent adjustEvent3 = new AdjustEvent("w788qs");
			adjustEvent3.addPartnerParameter("key", "value");
			adjustEvent3.addPartnerParameter("foo", "bar");
			Adjust.trackEvent(adjustEvent3);
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 5 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtSetOfflineMode))
		{
			if (string.Equals(this.txtSetOfflineMode, "Turn Offline Mode ON", StringComparison.OrdinalIgnoreCase))
			{
				Adjust.setOfflineMode(true);
				this.txtSetOfflineMode = "Turn Offline Mode OFF";
			}
			else
			{
				Adjust.setOfflineMode(false);
				this.txtSetOfflineMode = "Turn Offline Mode ON";
			}
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 6 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtSetEnabled))
		{
			if (string.Equals(this.txtSetEnabled, "Disable SDK", StringComparison.OrdinalIgnoreCase))
			{
				Adjust.setEnabled(false);
				this.txtSetEnabled = "Enable SDK";
			}
			else
			{
				Adjust.setEnabled(true);
				this.txtSetEnabled = "Disable SDK";
			}
		}
		if (GUI.Button(new Rect(0f, (float)(Screen.height * 7 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Is SDK Enabled?"))
		{
			this.isEnabled = Adjust.isEnabled();
			this.showPopUp = true;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0000289C File Offset: 0x00000A9C
	private void ShowGUI(int windowID)
	{
		if (this.isEnabled)
		{
			GUI.Label(new Rect(65f, 40f, 200f, 30f), "Adjust SDK is ENABLED!");
		}
		else
		{
			GUI.Label(new Rect(65f, 40f, 200f, 30f), "Adjust SDK is DISABLED!");
		}
		if (GUI.Button(new Rect(90f, 75f, 120f, 40f), "OK"))
		{
			this.showPopUp = false;
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002925 File Offset: 0x00000B25
	public void HandleGooglePlayId(string adId)
	{
		Debug.Log("Google Play Ad ID = " + adId);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002938 File Offset: 0x00000B38
	public void AttributionChangedCallback(AdjustAttribution attributionData)
	{
		Debug.Log("Attribution changed!");
		if (attributionData.trackerName != null)
		{
			Debug.Log("Tracker name: " + attributionData.trackerName);
		}
		if (attributionData.trackerToken != null)
		{
			Debug.Log("Tracker token: " + attributionData.trackerToken);
		}
		if (attributionData.network != null)
		{
			Debug.Log("Network: " + attributionData.network);
		}
		if (attributionData.campaign != null)
		{
			Debug.Log("Campaign: " + attributionData.campaign);
		}
		if (attributionData.adgroup != null)
		{
			Debug.Log("Adgroup: " + attributionData.adgroup);
		}
		if (attributionData.creative != null)
		{
			Debug.Log("Creative: " + attributionData.creative);
		}
		if (attributionData.clickLabel != null)
		{
			Debug.Log("Click label: " + attributionData.clickLabel);
		}
		if (attributionData.adid != null)
		{
			Debug.Log("ADID: " + attributionData.adid);
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002A38 File Offset: 0x00000C38
	public void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
	{
		Debug.Log("Event tracked successfully!");
		if (eventSuccessData.Message != null)
		{
			Debug.Log("Message: " + eventSuccessData.Message);
		}
		if (eventSuccessData.Timestamp != null)
		{
			Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
		}
		if (eventSuccessData.Adid != null)
		{
			Debug.Log("Adid: " + eventSuccessData.Adid);
		}
		if (eventSuccessData.EventToken != null)
		{
			Debug.Log("EventToken: " + eventSuccessData.EventToken);
		}
		if (eventSuccessData.CallbackId != null)
		{
			Debug.Log("CallbackId: " + eventSuccessData.CallbackId);
		}
		if (eventSuccessData.JsonResponse != null)
		{
			Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponse());
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002B00 File Offset: 0x00000D00
	public void EventFailureCallback(AdjustEventFailure eventFailureData)
	{
		Debug.Log("Event tracking failed!");
		if (eventFailureData.Message != null)
		{
			Debug.Log("Message: " + eventFailureData.Message);
		}
		if (eventFailureData.Timestamp != null)
		{
			Debug.Log("Timestamp: " + eventFailureData.Timestamp);
		}
		if (eventFailureData.Adid != null)
		{
			Debug.Log("Adid: " + eventFailureData.Adid);
		}
		if (eventFailureData.EventToken != null)
		{
			Debug.Log("EventToken: " + eventFailureData.EventToken);
		}
		if (eventFailureData.CallbackId != null)
		{
			Debug.Log("CallbackId: " + eventFailureData.CallbackId);
		}
		if (eventFailureData.JsonResponse != null)
		{
			Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponse());
		}
		Debug.Log("WillRetry: " + eventFailureData.WillRetry.ToString());
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002BE4 File Offset: 0x00000DE4
	public void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData)
	{
		Debug.Log("Session tracked successfully!");
		if (sessionSuccessData.Message != null)
		{
			Debug.Log("Message: " + sessionSuccessData.Message);
		}
		if (sessionSuccessData.Timestamp != null)
		{
			Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
		}
		if (sessionSuccessData.Adid != null)
		{
			Debug.Log("Adid: " + sessionSuccessData.Adid);
		}
		if (sessionSuccessData.JsonResponse != null)
		{
			Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponse());
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002C70 File Offset: 0x00000E70
	public void SessionFailureCallback(AdjustSessionFailure sessionFailureData)
	{
		Debug.Log("Session tracking failed!");
		if (sessionFailureData.Message != null)
		{
			Debug.Log("Message: " + sessionFailureData.Message);
		}
		if (sessionFailureData.Timestamp != null)
		{
			Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
		}
		if (sessionFailureData.Adid != null)
		{
			Debug.Log("Adid: " + sessionFailureData.Adid);
		}
		if (sessionFailureData.JsonResponse != null)
		{
			Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponse());
		}
		Debug.Log("WillRetry: " + sessionFailureData.WillRetry.ToString());
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002D18 File Offset: 0x00000F18
	private void DeferredDeeplinkCallback(string deeplinkURL)
	{
		Debug.Log("Deferred deeplink reported!");
		if (deeplinkURL != null)
		{
			Debug.Log("Deeplink URL: " + deeplinkURL);
			return;
		}
		Debug.Log("Deeplink URL is null!");
	}

	// Token: 0x04000008 RID: 8
	private int numberOfButtons = 8;

	// Token: 0x04000009 RID: 9
	private bool isEnabled;

	// Token: 0x0400000A RID: 10
	private bool showPopUp;

	// Token: 0x0400000B RID: 11
	private string txtSetEnabled = "Disable SDK";

	// Token: 0x0400000C RID: 12
	private string txtManualLaunch = "Manual Launch";

	// Token: 0x0400000D RID: 13
	private string txtSetOfflineMode = "Turn Offline Mode ON";
}

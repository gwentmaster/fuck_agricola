using System;
using System.Collections.Generic;
using AmplitudeNS.MiniJSON;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class Amplitude
{
	// Token: 0x0600001E RID: 30 RVA: 0x00002D72 File Offset: 0x00000F72
	public static Amplitude getInstance()
	{
		return Amplitude.getInstance(null);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002D7C File Offset: 0x00000F7C
	public static Amplitude getInstance(string instanceName)
	{
		string text = instanceName;
		if (string.IsNullOrEmpty(text))
		{
			text = "$default_instance";
		}
		object obj = Amplitude.instanceLock;
		Amplitude result;
		lock (obj)
		{
			if (Amplitude.instances == null)
			{
				Amplitude.instances = new Dictionary<string, Amplitude>();
			}
			Amplitude amplitude;
			if (Amplitude.instances.TryGetValue(text, out amplitude))
			{
				result = amplitude;
			}
			else
			{
				amplitude = new Amplitude(instanceName);
				Amplitude.instances.Add(text, amplitude);
				result = amplitude;
			}
		}
		return result;
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000020 RID: 32 RVA: 0x00002E04 File Offset: 0x00001004
	public static Amplitude Instance
	{
		get
		{
			return Amplitude.getInstance();
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002E0B File Offset: 0x0000100B
	public Amplitude(string instanceName)
	{
		this.instanceName = instanceName;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002E1A File Offset: 0x0000101A
	protected void Log(string message)
	{
		if (!this.logging)
		{
			return;
		}
		Debug.Log(message);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002E2B File Offset: 0x0000102B
	public void init(string apiKey)
	{
		this.Log(string.Format("C# init {0}", apiKey));
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002E3E File Offset: 0x0000103E
	public void init(string apiKey, string userId)
	{
		this.Log(string.Format("C# init {0} with userId {1}", apiKey, userId));
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002E54 File Offset: 0x00001054
	public void setTrackingOptions(IDictionary<string, bool> trackingOptions)
	{
		if (trackingOptions != null)
		{
			string arg = Json.Serialize(trackingOptions);
			this.Log(string.Format("C# setting tracking options {0}", arg));
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002E7C File Offset: 0x0000107C
	public void logEvent(string evt)
	{
		this.Log(string.Format("C# sendEvent {0}", evt));
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002E90 File Offset: 0x00001090
	public void logEvent(string evt, IDictionary<string, object> properties)
	{
		string arg;
		if (properties != null)
		{
			arg = Json.Serialize(properties);
		}
		else
		{
			arg = Json.Serialize(new Dictionary<string, object>());
		}
		this.Log(string.Format("C# sendEvent {0} with properties {1}", evt, arg));
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002EC8 File Offset: 0x000010C8
	public void logEvent(string evt, IDictionary<string, object> properties, bool outOfSession)
	{
		string arg;
		if (properties != null)
		{
			arg = Json.Serialize(properties);
		}
		else
		{
			arg = Json.Serialize(new Dictionary<string, object>());
		}
		this.Log(string.Format("C# sendEvent {0} with properties {1} and outOfSession {2}", evt, arg, outOfSession));
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002F04 File Offset: 0x00001104
	public void setUserId(string userId)
	{
		this.Log(string.Format("C# setUserId {0}", userId));
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002F18 File Offset: 0x00001118
	public void setUserProperties(IDictionary<string, object> properties)
	{
		string arg;
		if (properties != null)
		{
			arg = Json.Serialize(properties);
		}
		else
		{
			arg = Json.Serialize(new Dictionary<string, object>());
		}
		this.Log(string.Format("C# setUserProperties {0}", arg));
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002F4D File Offset: 0x0000114D
	public void setOptOut(bool enabled)
	{
		this.Log(string.Format("C# setOptOut {0}", enabled));
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002F65 File Offset: 0x00001165
	[Obsolete("Please call setUserProperties instead", false)]
	public void setGlobalUserProperties(IDictionary<string, object> properties)
	{
		this.setUserProperties(properties);
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002F6E File Offset: 0x0000116E
	public void logRevenue(double amount)
	{
		this.Log(string.Format("C# logRevenue {0}", amount));
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002F86 File Offset: 0x00001186
	public void logRevenue(string productId, int quantity, double price)
	{
		this.Log(string.Format("C# logRevenue {0}, {1}, {2}", productId, quantity, price));
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002FA5 File Offset: 0x000011A5
	public void logRevenue(string productId, int quantity, double price, string receipt, string receiptSignature)
	{
		this.Log(string.Format("C# logRevenue {0}, {1}, {2} (with receipt)", productId, quantity, price));
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002FC4 File Offset: 0x000011C4
	public void logRevenue(string productId, int quantity, double price, string receipt, string receiptSignature, string revenueType, IDictionary<string, object> eventProperties)
	{
		string text;
		if (eventProperties != null)
		{
			text = Json.Serialize(eventProperties);
		}
		else
		{
			text = Json.Serialize(new Dictionary<string, object>());
		}
		this.Log(string.Format("C# logRevenue {0}, {1}, {2}, {3}, {4} (with receipt)", new object[]
		{
			productId,
			quantity,
			price,
			revenueType,
			text
		}));
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0000301F File Offset: 0x0000121F
	public string getDeviceId()
	{
		return null;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003022 File Offset: 0x00001222
	public void regenerateDeviceId()
	{
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003024 File Offset: 0x00001224
	public void trackSessionEvents(bool enabled)
	{
		this.Log(string.Format("C# trackSessionEvents {0}", enabled));
	}

	// Token: 0x06000034 RID: 52 RVA: 0x0000303C File Offset: 0x0000123C
	public long getSessionId()
	{
		return -1L;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003040 File Offset: 0x00001240
	public void clearUserProperties()
	{
		this.Log(string.Format("C# clearUserProperties", Array.Empty<object>()));
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003057 File Offset: 0x00001257
	public void unsetUserProperty(string property)
	{
		this.Log(string.Format("C# unsetUserProperty {0}", property));
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0000306A File Offset: 0x0000126A
	public void setOnceUserProperty(string property, bool value)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003083 File Offset: 0x00001283
	public void setOnceUserProperty(string property, double value)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000039 RID: 57 RVA: 0x0000309C File Offset: 0x0000129C
	public void setOnceUserProperty(string property, float value)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000030B5 File Offset: 0x000012B5
	public void setOnceUserProperty(string property, int value)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000030CE File Offset: 0x000012CE
	public void setOnceUserProperty(string property, long value)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000030E7 File Offset: 0x000012E7
	public void setOnceUserProperty(string property, string value)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000030FC File Offset: 0x000012FC
	public void setOnceUserProperty(string property, IDictionary<string, object> values)
	{
		if (values == null)
		{
			return;
		}
		string arg = Json.Serialize(values);
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, arg));
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003128 File Offset: 0x00001328
	public void setOnceUserProperty<T>(string property, IList<T> values)
	{
		if (values == null)
		{
			return;
		}
		string arg = Json.Serialize(new Dictionary<string, object>
		{
			{
				"list",
				values
			}
		});
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, arg));
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000030E7 File Offset: 0x000012E7
	public void setOnceUserProperty(string property, bool[] array)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000030E7 File Offset: 0x000012E7
	public void setOnceUserProperty(string property, double[] array)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000030E7 File Offset: 0x000012E7
	public void setOnceUserProperty(string property, float[] array)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000030E7 File Offset: 0x000012E7
	public void setOnceUserProperty(string property, int[] array)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000030E7 File Offset: 0x000012E7
	public void setOnceUserProperty(string property, long[] array)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000030E7 File Offset: 0x000012E7
	public void setOnceUserProperty(string property, string[] array)
	{
		this.Log(string.Format("C# setOnceUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003162 File Offset: 0x00001362
	public void setUserProperty(string property, bool value)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0000317B File Offset: 0x0000137B
	public void setUserProperty(string property, double value)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003194 File Offset: 0x00001394
	public void setUserProperty(string property, float value)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000031AD File Offset: 0x000013AD
	public void setUserProperty(string property, int value)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000031C6 File Offset: 0x000013C6
	public void setUserProperty(string property, long value)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000031DF File Offset: 0x000013DF
	public void setUserProperty(string property, string value)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000031F4 File Offset: 0x000013F4
	public void setUserProperty(string property, IDictionary<string, object> values)
	{
		if (values == null)
		{
			return;
		}
		string arg = Json.Serialize(values);
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, arg));
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00003220 File Offset: 0x00001420
	public void setUserProperty<T>(string property, IList<T> values)
	{
		if (values == null)
		{
			return;
		}
		string arg = Json.Serialize(new Dictionary<string, object>
		{
			{
				"list",
				values
			}
		});
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, arg));
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000031DF File Offset: 0x000013DF
	public void setUserProperty(string property, bool[] array)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, array));
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000031DF File Offset: 0x000013DF
	public void setUserProperty(string property, double[] array)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, array));
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000031DF File Offset: 0x000013DF
	public void setUserProperty(string property, float[] array)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000031DF File Offset: 0x000013DF
	public void setUserProperty(string property, int[] array)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000051 RID: 81 RVA: 0x000031DF File Offset: 0x000013DF
	public void setUserProperty(string property, long[] array)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000031DF File Offset: 0x000013DF
	public void setUserProperty(string property, string[] array)
	{
		this.Log(string.Format("C# setUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000053 RID: 83 RVA: 0x0000325A File Offset: 0x0000145A
	public void addUserProperty(string property, double value)
	{
		this.Log(string.Format("C# addUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00003273 File Offset: 0x00001473
	public void addUserProperty(string property, float value)
	{
		this.Log(string.Format("C# addUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000055 RID: 85 RVA: 0x0000328C File Offset: 0x0000148C
	public void addUserProperty(string property, int value)
	{
		this.Log(string.Format("C# addUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000032A5 File Offset: 0x000014A5
	public void addUserProperty(string property, long value)
	{
		this.Log(string.Format("C# addUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000057 RID: 87 RVA: 0x000032BE File Offset: 0x000014BE
	public void addUserProperty(string property, string value)
	{
		this.Log(string.Format("C# addUserProperty {0}, {1}", property, value));
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000032D4 File Offset: 0x000014D4
	public void addUserProperty(string property, IDictionary<string, object> values)
	{
		if (values == null)
		{
			return;
		}
		string arg = Json.Serialize(values);
		this.Log(string.Format("C# addUserProperty {0}, {1}", property, arg));
	}

	// Token: 0x06000059 RID: 89 RVA: 0x000032FE File Offset: 0x000014FE
	public void appendUserProperty(string property, bool value)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003317 File Offset: 0x00001517
	public void appendUserProperty(string property, double value)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00003330 File Offset: 0x00001530
	public void appendUserProperty(string property, float value)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00003349 File Offset: 0x00001549
	public void appendUserProperty(string property, int value)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003362 File Offset: 0x00001562
	public void appendUserProperty(string property, long value)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600005E RID: 94 RVA: 0x0000337B File Offset: 0x0000157B
	public void appendUserProperty(string property, string value)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, value));
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00003390 File Offset: 0x00001590
	public void appendUserProperty(string property, IDictionary<string, object> values)
	{
		if (values == null)
		{
			return;
		}
		string arg = Json.Serialize(values);
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, arg));
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000033BC File Offset: 0x000015BC
	public void appendUserProperty<T>(string property, IList<T> values)
	{
		if (values == null)
		{
			return;
		}
		string arg = Json.Serialize(new Dictionary<string, object>
		{
			{
				"list",
				values
			}
		});
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, arg));
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0000337B File Offset: 0x0000157B
	public void appendUserProperty(string property, bool[] array)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000062 RID: 98 RVA: 0x0000337B File Offset: 0x0000157B
	public void appendUserProperty(string property, double[] array)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000063 RID: 99 RVA: 0x0000337B File Offset: 0x0000157B
	public void appendUserProperty(string property, float[] array)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000064 RID: 100 RVA: 0x0000337B File Offset: 0x0000157B
	public void appendUserProperty(string property, int[] array)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000065 RID: 101 RVA: 0x0000337B File Offset: 0x0000157B
	public void appendUserProperty(string property, long[] array)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0000337B File Offset: 0x0000157B
	public void appendUserProperty(string property, string[] array)
	{
		this.Log(string.Format("C# appendUserProperty {0}, {1}", property, array));
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00003022 File Offset: 0x00001222
	public void startSession()
	{
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00003022 File Offset: 0x00001222
	public void endSession()
	{
	}

	// Token: 0x0400000E RID: 14
	private static Dictionary<string, Amplitude> instances;

	// Token: 0x0400000F RID: 15
	private static readonly object instanceLock = new object();

	// Token: 0x04000010 RID: 16
	public bool logging;

	// Token: 0x04000011 RID: 17
	private string instanceName;
}

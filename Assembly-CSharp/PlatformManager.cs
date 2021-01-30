using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class PlatformManager : MonoBehaviour
{
	// Token: 0x060006E7 RID: 1767 RVA: 0x00033B80 File Offset: 0x00031D80
	public PlatformManager.DeviceType GetDeviceType()
	{
		return this.m_DeviceType;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00033B88 File Offset: 0x00031D88
	public PlatformManager.AspectRatioType GetAspectRatioType()
	{
		return this.m_AspectRatioType;
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00033B90 File Offset: 0x00031D90
	public bool LoadHalfCards()
	{
		switch (this.m_DeviceType)
		{
		case PlatformManager.DeviceType.DESKTOP:
			return this.m_LoadHalfCardsDesktop;
		case PlatformManager.DeviceType.TABLET:
			return this.m_LoadHalfCardsTablet;
		case PlatformManager.DeviceType.PHONE:
			return this.m_LoadHalfCardsPhone;
		default:
			return true;
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00033BD0 File Offset: 0x00031DD0
	private void DetermineDeviceType()
	{
		float num = (float)Screen.width / Screen.dpi;
		float num2 = (float)Screen.height / Screen.dpi;
		if (Mathf.Sqrt(num * num + num2 * num2) > this.m_MaxScreenDiagonalPhone)
		{
			this.m_DeviceType = PlatformManager.DeviceType.DESKTOP;
			return;
		}
		this.m_DeviceType = PlatformManager.DeviceType.DESKTOP;
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00033C18 File Offset: 0x00031E18
	private void DetermineAspectRatioType()
	{
		this.m_AspectRatioValue = (float)Screen.width / (float)Screen.height;
		if (PlatformManager.s_AspectRatioValues == null)
		{
			this.m_AspectRatioType = PlatformManager.AspectRatioType.ASPECT_UNKNOWN;
		}
		else
		{
			int num = -1;
			float num2 = 100f;
			for (int i = 0; i < PlatformManager.s_AspectRatioValues.Length; i++)
			{
				float num3 = Mathf.Abs(PlatformManager.s_AspectRatioValues[i] - this.m_AspectRatioValue);
				if (num3 < num2)
				{
					num = i;
					num2 = num3;
				}
			}
			if (num == -1)
			{
				this.m_AspectRatioType = PlatformManager.AspectRatioType.ASPECT_UNKNOWN;
			}
			else
			{
				this.m_AspectRatioType = (PlatformManager.AspectRatioType)num;
			}
		}
		Debug.Log("Aspect Ratio Type is " + this.m_AspectRatioType.ToString());
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00033CB4 File Offset: 0x00031EB4
	private void Awake()
	{
		if (PlatformManager.s_instance == null)
		{
			PlatformManager.s_instance = this;
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.DetermineDeviceType();
		this.DetermineAspectRatioType();
		if (this.m_Platform_Steam != null)
		{
			this.m_ActivePlatforms.Add(this.m_Platform_Steam);
		}
		if (this.m_Platform_iOS != null && !this.m_ActivePlatforms.Contains(this.m_Platform_iOS))
		{
			UnityEngine.Object.DestroyImmediate(this.m_Platform_iOS);
			this.m_Platform_iOS = null;
		}
		if (this.m_Platform_Android != null && !this.m_ActivePlatforms.Contains(this.m_Platform_Android))
		{
			UnityEngine.Object.DestroyImmediate(this.m_Platform_Android);
			this.m_Platform_Android = null;
		}
		if (this.m_Platform_Steam != null && !this.m_ActivePlatforms.Contains(this.m_Platform_Steam))
		{
			UnityEngine.Object.DestroyImmediate(this.m_Platform_Steam);
			this.m_Platform_Steam = null;
		}
		foreach (GameObject gameObject in this.m_ActivePlatforms)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x04000825 RID: 2085
	private static float[] s_AspectRatioValues = new float[]
	{
		0f,
		1.3333334f,
		1.5f,
		1.6f,
		1.7777778f
	};

	// Token: 0x04000826 RID: 2086
	[SerializeField]
	private GameObject m_Platform_Steam;

	// Token: 0x04000827 RID: 2087
	[SerializeField]
	private GameObject m_Platform_iOS;

	// Token: 0x04000828 RID: 2088
	[SerializeField]
	private GameObject m_Platform_Android;

	// Token: 0x04000829 RID: 2089
	[Space(10f)]
	[Header("DESKTOP")]
	[SerializeField]
	private bool m_LoadHalfCardsDesktop;

	// Token: 0x0400082A RID: 2090
	[Space(10f)]
	[Header("TABLET")]
	[SerializeField]
	private bool m_LoadHalfCardsTablet;

	// Token: 0x0400082B RID: 2091
	[Space(10f)]
	[Header("PHONE")]
	[SerializeField]
	private float m_MaxScreenDiagonalPhone = 6f;

	// Token: 0x0400082C RID: 2092
	[SerializeField]
	private bool m_LoadHalfCardsPhone;

	// Token: 0x0400082D RID: 2093
	private List<GameObject> m_ActivePlatforms = new List<GameObject>();

	// Token: 0x0400082E RID: 2094
	private PlatformManager.DeviceType m_DeviceType;

	// Token: 0x0400082F RID: 2095
	private float m_ScreenDiagonal;

	// Token: 0x04000830 RID: 2096
	private float m_AspectRatioValue;

	// Token: 0x04000831 RID: 2097
	private PlatformManager.AspectRatioType m_AspectRatioType;

	// Token: 0x04000832 RID: 2098
	public static PlatformManager s_instance = null;

	// Token: 0x02000794 RID: 1940
	public enum DeviceType
	{
		// Token: 0x04002C50 RID: 11344
		DESKTOP,
		// Token: 0x04002C51 RID: 11345
		TABLET,
		// Token: 0x04002C52 RID: 11346
		PHONE
	}

	// Token: 0x02000795 RID: 1941
	public enum AspectRatioType
	{
		// Token: 0x04002C54 RID: 11348
		ASPECT_UNKNOWN,
		// Token: 0x04002C55 RID: 11349
		ASPECT_4_3,
		// Token: 0x04002C56 RID: 11350
		ASPECT_3_2,
		// Token: 0x04002C57 RID: 11351
		ASPECT_16_10,
		// Token: 0x04002C58 RID: 11352
		ASPECT_16_9
	}
}

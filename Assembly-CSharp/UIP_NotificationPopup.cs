using System;
using System.Runtime.InteropServices;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000101 RID: 257
public class UIP_NotificationPopup : MonoBehaviour
{
	// Token: 0x060009C7 RID: 2503 RVA: 0x00042408 File Offset: 0x00040608
	public void Setup()
	{
		this.m_bIgnoreToggles = true;
		this.m_DisplayEmail.text = Network.m_loginName;
		GCHandle gchandle = GCHandle.Alloc(new byte[256], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		AgricolaLib.NetworkGetNotificationSetting(intPtr, 0);
		NotificationData notificationData = (NotificationData)Marshal.PtrToStructure(intPtr, typeof(NotificationData));
		this.m_ToggleEmail.isOn = (notificationData.enabled != 0);
		if (notificationData.available != 0)
		{
			this.m_ToggleEmail.gameObject.SetActive(true);
			this.m_ToggleEmail.interactable = true;
		}
		else
		{
			this.m_ToggleEmail.gameObject.SetActive(true);
			this.m_ToggleEmail.interactable = false;
		}
		AgricolaLib.NetworkGetNotificationSetting(intPtr, 1);
		NotificationData notificationData2 = (NotificationData)Marshal.PtrToStructure(intPtr, typeof(NotificationData));
		this.m_ToggleIOS.isOn = (notificationData2.enabled != 0);
		if (notificationData2.available != 0)
		{
			this.m_ToggleIOS.gameObject.SetActive(true);
			this.m_ToggleIOS.interactable = true;
		}
		else
		{
			this.m_ToggleIOS.gameObject.SetActive(false);
		}
		AgricolaLib.NetworkGetNotificationSetting(intPtr, 2);
		NotificationData notificationData3 = (NotificationData)Marshal.PtrToStructure(intPtr, typeof(NotificationData));
		this.m_ToggleAndroid.isOn = (notificationData3.enabled != 0);
		if (notificationData3.available != 0)
		{
			this.m_ToggleAndroid.gameObject.SetActive(true);
			this.m_ToggleAndroid.interactable = true;
		}
		else
		{
			this.m_ToggleAndroid.gameObject.SetActive(false);
		}
		gchandle.Free();
		this.m_bIgnoreToggles = false;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0004259C File Offset: 0x0004079C
	public void SetData(int index)
	{
		if (this.m_bIgnoreToggles)
		{
			return;
		}
		if (this.m_ToggleAudio != null)
		{
			this.m_ToggleAudio.Play();
		}
		if (index == 0)
		{
			AgricolaLib.NetworkSetNotificationSetting(0, this.m_ToggleEmail.isOn);
			return;
		}
		if (index == 1)
		{
			AgricolaLib.NetworkSetNotificationSetting(1, this.m_ToggleIOS.isOn);
			return;
		}
		if (index == 2)
		{
			AgricolaLib.NetworkSetNotificationSetting(2, this.m_ToggleAndroid.isOn);
		}
	}

	// Token: 0x04000A5D RID: 2653
	public Toggle m_ToggleEmail;

	// Token: 0x04000A5E RID: 2654
	public Toggle m_ToggleIOS;

	// Token: 0x04000A5F RID: 2655
	public Toggle m_ToggleAndroid;

	// Token: 0x04000A60 RID: 2656
	public TextMeshProUGUI m_DisplayEmail;

	// Token: 0x04000A61 RID: 2657
	public AudioSource m_ToggleAudio;

	// Token: 0x04000A62 RID: 2658
	private bool m_bIgnoreToggles;
}

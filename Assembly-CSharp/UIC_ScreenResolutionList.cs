using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000FB RID: 251
[Serializable]
public class UIC_ScreenResolutionList
{
	// Token: 0x06000976 RID: 2422 RVA: 0x0003FA44 File Offset: 0x0003DC44
	public void PopulateScreenResolutionList()
	{
		if (this.m_screenResolutionListGameObjects == null)
		{
			this.m_screenResolutionListGameObjects = new List<GameObject>();
		}
		else
		{
			this.DestroyScreenResolutionList();
		}
		this.m_availableResolutions = Screen.resolutions;
		int num = 0;
		foreach (Resolution resolution in this.m_availableResolutions)
		{
			if (resolution.width < 1024 || resolution.height < 768)
			{
				num++;
			}
			else
			{
				string text = resolution.width + "x" + resolution.height;
				if (this.m_createdResolutions.Contains(text))
				{
					num++;
				}
				else
				{
					this.m_createdResolutions.Add(text);
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_buttonPrefab);
					this.m_screenResolutionListGameObjects.Add(gameObject);
					gameObject.transform.SetParent(this.m_contentPanel);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
					UIP_ScreenResolutionSlot component = gameObject.GetComponent<UIP_ScreenResolutionSlot>();
					if (component.m_colorizer != null)
					{
						component.m_colorizer.ColorizeByTheme();
					}
					component.ResolutionTextString = text;
					component.ResolutionIndex = num;
					component.SetClickListener(new UIP_ScreenResolutionSlot.ClickCallback(this.HandleClickOnSlot));
					if (Screen.width == resolution.width && Screen.height == resolution.height)
					{
						gameObject.GetComponent<Button>().interactable = false;
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0003FBF4 File Offset: 0x0003DDF4
	public void SetClickListener(UIC_ScreenResolutionList.ClickCallback cb)
	{
		this.m_clickCallback = cb;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0003FBFD File Offset: 0x0003DDFD
	private void HandleClickOnSlot(UIP_ScreenResolutionSlot slot)
	{
		if (this.m_clickCallback != null)
		{
			this.m_clickCallback(this.m_availableResolutions[slot.ResolutionIndex]);
		}
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0003FC24 File Offset: 0x0003DE24
	private void DestroyScreenResolutionList()
	{
		foreach (GameObject obj in this.m_screenResolutionListGameObjects)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.m_screenResolutionListGameObjects.Clear();
		this.m_createdResolutions.Clear();
	}

	// Token: 0x040009F8 RID: 2552
	public GameObject m_buttonPrefab;

	// Token: 0x040009F9 RID: 2553
	public Transform m_contentPanel;

	// Token: 0x040009FA RID: 2554
	private List<GameObject> m_screenResolutionListGameObjects;

	// Token: 0x040009FB RID: 2555
	private Resolution[] m_availableResolutions;

	// Token: 0x040009FC RID: 2556
	private UIC_ScreenResolutionList.ClickCallback m_clickCallback;

	// Token: 0x040009FD RID: 2557
	private List<string> m_createdResolutions = new List<string>();

	// Token: 0x020007CB RID: 1995
	// (Invoke) Token: 0x06004333 RID: 17203
	public delegate void ClickCallback(Resolution selectedResolution);
}

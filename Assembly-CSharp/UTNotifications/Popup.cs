using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x02000139 RID: 313
	public class Popup : MonoBehaviour
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x00053D7D File Offset: 0x00051F7D
		public void AddItem(string label, UnityAction action)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ItemPrefab, base.transform, false);
			gameObject.GetComponentInChildren<Text>().text = label;
			gameObject.GetComponentInChildren<Button>().onClick.AddListener(action);
		}

		// Token: 0x04000CEC RID: 3308
		public GameObject ItemPrefab;
	}
}

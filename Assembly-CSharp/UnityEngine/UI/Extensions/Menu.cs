using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A8 RID: 424
	public abstract class Menu<T> : Menu where T : Menu<!0>
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x000676F6 File Offset: 0x000658F6
		// (set) Token: 0x06001071 RID: 4209 RVA: 0x000676FD File Offset: 0x000658FD
		public static T Instance { get; private set; }

		// Token: 0x06001072 RID: 4210 RVA: 0x00067705 File Offset: 0x00065905
		protected virtual void Awake()
		{
			Menu<!0>.Instance = (!0)((object)this);
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00067714 File Offset: 0x00065914
		protected virtual void OnDestroy()
		{
			Menu<!0>.Instance = default(!0);
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00067730 File Offset: 0x00065930
		protected static void Open()
		{
			if (Menu<!0>.Instance == null)
			{
				MenuManager.Instance.CreateInstance(typeof(!0).Name);
			}
			else
			{
				Menu<!0>.Instance.gameObject.SetActive(true);
			}
			MenuManager.Instance.OpenMenu(Menu<!0>.Instance);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00067794 File Offset: 0x00065994
		protected static void Close()
		{
			if (Menu<!0>.Instance == null)
			{
				Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", new object[]
				{
					typeof(!0)
				});
				return;
			}
			MenuManager.Instance.CloseMenu(Menu<!0>.Instance);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x000677E5 File Offset: 0x000659E5
		public override void OnBackPressed()
		{
			Menu<!0>.Close();
		}
	}
}

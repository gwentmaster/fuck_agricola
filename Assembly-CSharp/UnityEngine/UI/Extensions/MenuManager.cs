using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001AA RID: 426
	[AddComponentMenu("UI/Extensions/Menu Manager")]
	[DisallowMultipleComponent]
	public class MenuManager : MonoBehaviour
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0006780A File Offset: 0x00065A0A
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x00067811 File Offset: 0x00065A11
		public static MenuManager Instance { get; set; }

		// Token: 0x0600107C RID: 4220 RVA: 0x0006781C File Offset: 0x00065A1C
		private void Awake()
		{
			MenuManager.Instance = this;
			if (this.MenuScreens.Length > this.StartScreen)
			{
				this.CreateInstance(this.MenuScreens[this.StartScreen].name);
				this.OpenMenu(this.MenuScreens[this.StartScreen]);
				return;
			}
			Debug.LogError("Not enough Menu Screens configured");
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00067875 File Offset: 0x00065A75
		private void OnDestroy()
		{
			MenuManager.Instance = null;
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0006787D File Offset: 0x00065A7D
		public void CreateInstance<T>() where T : Menu
		{
			Object.Instantiate<T>(this.GetPrefab<T>(), base.transform);
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00067891 File Offset: 0x00065A91
		public void CreateInstance(string MenuName)
		{
			Object.Instantiate<GameObject>(this.GetPrefab(MenuName), base.transform);
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x000678A8 File Offset: 0x00065AA8
		public void OpenMenu(Menu instance)
		{
			if (this.menuStack.Count > 0)
			{
				if (instance.DisableMenusUnderneath)
				{
					foreach (Menu menu in this.menuStack)
					{
						menu.gameObject.SetActive(false);
						if (menu.DisableMenusUnderneath)
						{
							break;
						}
					}
				}
				Canvas component = instance.GetComponent<Canvas>();
				Canvas component2 = this.menuStack.Peek().GetComponent<Canvas>();
				component.sortingOrder = component2.sortingOrder + 1;
			}
			this.menuStack.Push(instance);
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00067950 File Offset: 0x00065B50
		private GameObject GetPrefab(string PrefabName)
		{
			for (int i = 0; i < this.MenuScreens.Length; i++)
			{
				if (this.MenuScreens[i].name == PrefabName)
				{
					return this.MenuScreens[i].gameObject;
				}
			}
			throw new MissingReferenceException("Prefab not found for " + PrefabName);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x000679A4 File Offset: 0x00065BA4
		private T GetPrefab<T>() where T : Menu
		{
			FieldInfo[] fields = base.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			{
				T t = fields[i].GetValue(this) as !!0;
				if (t != null)
				{
					return t;
				}
			}
			throw new MissingReferenceException("Prefab not found for type " + typeof(!!0));
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00067A0C File Offset: 0x00065C0C
		public void CloseMenu(Menu menu)
		{
			if (this.menuStack.Count == 0)
			{
				Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", new object[]
				{
					menu.GetType()
				});
				return;
			}
			if (this.menuStack.Peek() != menu)
			{
				Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", new object[]
				{
					menu.GetType()
				});
				return;
			}
			this.CloseTopMenu();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00067A78 File Offset: 0x00065C78
		public void CloseTopMenu()
		{
			Menu menu = this.menuStack.Pop();
			if (menu.DestroyWhenClosed)
			{
				Object.Destroy(menu.gameObject);
			}
			else
			{
				menu.gameObject.SetActive(false);
			}
			foreach (Menu menu2 in this.menuStack)
			{
				menu2.gameObject.SetActive(true);
				if (menu2.DisableMenusUnderneath)
				{
					break;
				}
			}
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00067B08 File Offset: 0x00065D08
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape) && this.menuStack.Count > 0)
			{
				this.menuStack.Peek().OnBackPressed();
			}
		}

		// Token: 0x04000F70 RID: 3952
		public Menu[] MenuScreens;

		// Token: 0x04000F71 RID: 3953
		public int StartScreen;

		// Token: 0x04000F72 RID: 3954
		private Stack<Menu> menuStack = new Stack<Menu>();
	}
}

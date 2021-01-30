using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001FA RID: 506
	public class UpdateScrollSnap : MonoBehaviour
	{
		// Token: 0x06001296 RID: 4758 RVA: 0x00070FFC File Offset: 0x0006F1FC
		public void AddButton()
		{
			if (this.HSS)
			{
				GameObject go = Object.Instantiate<GameObject>(this.HorizontalPagePrefab);
				this.HSS.AddChild(go);
			}
			if (this.VSS)
			{
				GameObject go2 = Object.Instantiate<GameObject>(this.VerticalPagePrefab);
				this.VSS.AddChild(go2);
			}
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x00071054 File Offset: 0x0006F254
		public void RemoveButton()
		{
			if (this.HSS)
			{
				GameObject gameObject;
				this.HSS.RemoveChild(this.HSS.CurrentPage, out gameObject);
				gameObject.SetActive(false);
			}
			if (this.VSS)
			{
				GameObject gameObject2;
				this.VSS.RemoveChild(this.VSS.CurrentPage, out gameObject2);
				gameObject2.SetActive(false);
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000710BC File Offset: 0x0006F2BC
		public void JumpToPage()
		{
			int screenIndex = int.Parse(this.JumpPage.text);
			if (this.HSS)
			{
				this.HSS.GoToScreen(screenIndex);
			}
			if (this.VSS)
			{
				this.VSS.GoToScreen(screenIndex);
			}
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0007110C File Offset: 0x0006F30C
		public void SelectionStartChange()
		{
			Debug.Log("Scroll Snap change started");
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x00071118 File Offset: 0x0006F318
		public void SelectionEndChange()
		{
			Debug.Log("Scroll Snap change finished");
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00071124 File Offset: 0x0006F324
		public void PageChange(int page)
		{
			Debug.Log(string.Format("Scroll Snap page changed to {0}", page));
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0007113C File Offset: 0x0006F33C
		public void RemoveAll()
		{
			GameObject[] array;
			this.HSS.RemoveAllChildren(out array);
			this.VSS.RemoveAllChildren(out array);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00071163 File Offset: 0x0006F363
		public void JumpToSelectedToggle(int page)
		{
			this.HSS.GoToScreen(page);
		}

		// Token: 0x040010CB RID: 4299
		public HorizontalScrollSnap HSS;

		// Token: 0x040010CC RID: 4300
		public VerticalScrollSnap VSS;

		// Token: 0x040010CD RID: 4301
		public GameObject HorizontalPagePrefab;

		// Token: 0x040010CE RID: 4302
		public GameObject VerticalPagePrefab;

		// Token: 0x040010CF RID: 4303
		public InputField JumpPage;
	}
}

using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001B6 RID: 438
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("Event/Extensions/Tab Navigation Helper")]
	public class TabNavigationHelper : MonoBehaviour
	{
		// Token: 0x06001102 RID: 4354 RVA: 0x0006A730 File Offset: 0x00068930
		private void Start()
		{
			this._system = base.GetComponent<EventSystem>();
			if (this._system == null)
			{
				Debug.LogError("Needs to be attached to the Event System component in the scene");
			}
			if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length != 0)
			{
				this.StartingObject = this.NavigationPath[0].gameObject.GetComponent<Selectable>();
			}
			if (this.StartingObject == null && this.CircularNavigation)
			{
				this.SelectDefaultObject(out this.StartingObject);
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0006A7B0 File Offset: 0x000689B0
		public void Update()
		{
			Selectable selectable = null;
			if (this.LastObject == null && this._system.currentSelectedGameObject != null)
			{
				selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
				while (selectable != null)
				{
					this.LastObject = selectable;
					selectable = selectable.FindSelectableOnDown();
				}
			}
			if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
			{
				if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length != 0)
				{
					for (int i = this.NavigationPath.Length - 1; i >= 0; i--)
					{
						if (!(this._system.currentSelectedGameObject != this.NavigationPath[i].gameObject))
						{
							selectable = ((i == 0) ? this.NavigationPath[this.NavigationPath.Length - 1] : this.NavigationPath[i - 1]);
							break;
						}
					}
				}
				else if (this._system.currentSelectedGameObject != null)
				{
					selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
					if (selectable == null && this.CircularNavigation)
					{
						selectable = this.LastObject;
					}
				}
				else
				{
					this.SelectDefaultObject(out selectable);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length != 0)
				{
					for (int j = 0; j < this.NavigationPath.Length; j++)
					{
						if (!(this._system.currentSelectedGameObject != this.NavigationPath[j].gameObject))
						{
							selectable = ((j == this.NavigationPath.Length - 1) ? this.NavigationPath[0] : this.NavigationPath[j + 1]);
							break;
						}
					}
				}
				else if (this._system.currentSelectedGameObject != null)
				{
					selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
					if (selectable == null && this.CircularNavigation)
					{
						selectable = this.StartingObject;
					}
				}
				else
				{
					this.SelectDefaultObject(out selectable);
				}
			}
			else if (this._system.currentSelectedGameObject == null)
			{
				this.SelectDefaultObject(out selectable);
			}
			if (this.CircularNavigation && this.StartingObject == null)
			{
				this.StartingObject = selectable;
			}
			this.selectGameObject(selectable);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0006A9F6 File Offset: 0x00068BF6
		private void SelectDefaultObject(out Selectable next)
		{
			if (this._system.firstSelectedGameObject)
			{
				next = this._system.firstSelectedGameObject.GetComponent<Selectable>();
				return;
			}
			next = null;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0006AA20 File Offset: 0x00068C20
		private void selectGameObject(Selectable selectable)
		{
			if (selectable != null)
			{
				InputField component = selectable.GetComponent<InputField>();
				if (component != null)
				{
					component.OnPointerClick(new PointerEventData(this._system));
				}
				this._system.SetSelectedGameObject(selectable.gameObject, new BaseEventData(this._system));
			}
		}

		// Token: 0x04000FBA RID: 4026
		private EventSystem _system;

		// Token: 0x04000FBB RID: 4027
		private Selectable StartingObject;

		// Token: 0x04000FBC RID: 4028
		private Selectable LastObject;

		// Token: 0x04000FBD RID: 4029
		[Tooltip("The path to take when user is tabbing through ui components.")]
		public Selectable[] NavigationPath;

		// Token: 0x04000FBE RID: 4030
		[Tooltip("Use the default Unity navigation system or a manual fixed order using Navigation Path")]
		public NavigationMode NavigationMode;

		// Token: 0x04000FBF RID: 4031
		[Tooltip("If True, this will loop the tab order from last to first automatically")]
		public bool CircularNavigation;
	}
}

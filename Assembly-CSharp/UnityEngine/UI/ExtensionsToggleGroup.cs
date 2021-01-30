using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x0200015A RID: 346
	[AddComponentMenu("UI/Extensions/Extensions Toggle Group")]
	[DisallowMultipleComponent]
	public class ExtensionsToggleGroup : UIBehaviour
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x00057B8C File Offset: 0x00055D8C
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x00057B94 File Offset: 0x00055D94
		public bool AllowSwitchOff
		{
			get
			{
				return this.m_AllowSwitchOff;
			}
			set
			{
				this.m_AllowSwitchOff = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00057B9D File Offset: 0x00055D9D
		// (set) Token: 0x06000D9B RID: 3483 RVA: 0x00057BA5 File Offset: 0x00055DA5
		public ExtensionsToggle SelectedToggle { get; private set; }

		// Token: 0x06000D9C RID: 3484 RVA: 0x00057BAE File Offset: 0x00055DAE
		protected ExtensionsToggleGroup()
		{
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00057BD7 File Offset: 0x00055DD7
		private void ValidateToggleIsInGroup(ExtensionsToggle toggle)
		{
			if (toggle == null || !this.m_Toggles.Contains(toggle))
			{
				throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[]
				{
					toggle,
					this
				}));
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00057C10 File Offset: 0x00055E10
		public void NotifyToggleOn(ExtensionsToggle toggle)
		{
			this.ValidateToggleIsInGroup(toggle);
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				if (this.m_Toggles[i] == toggle)
				{
					this.SelectedToggle = toggle;
				}
				else
				{
					this.m_Toggles[i].IsOn = false;
				}
			}
			this.onToggleGroupChanged.Invoke(this.AnyTogglesOn());
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00057C7A File Offset: 0x00055E7A
		public void UnregisterToggle(ExtensionsToggle toggle)
		{
			if (this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Remove(toggle);
				toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.NotifyToggleChanged));
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00057CAE File Offset: 0x00055EAE
		private void NotifyToggleChanged(bool isOn)
		{
			this.onToggleGroupToggleChanged.Invoke(isOn);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00057CBC File Offset: 0x00055EBC
		public void RegisterToggle(ExtensionsToggle toggle)
		{
			if (!this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Add(toggle);
				toggle.onValueChanged.AddListener(new UnityAction<bool>(this.NotifyToggleChanged));
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00057CEF File Offset: 0x00055EEF
		public bool AnyTogglesOn()
		{
			return this.m_Toggles.Find((ExtensionsToggle x) => x.IsOn) != null;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00057D21 File Offset: 0x00055F21
		public IEnumerable<ExtensionsToggle> ActiveToggles()
		{
			return from x in this.m_Toggles
			where x.IsOn
			select x;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00057D50 File Offset: 0x00055F50
		public void SetAllTogglesOff()
		{
			bool allowSwitchOff = this.m_AllowSwitchOff;
			this.m_AllowSwitchOff = true;
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				this.m_Toggles[i].IsOn = false;
			}
			this.m_AllowSwitchOff = allowSwitchOff;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00057D9A File Offset: 0x00055F9A
		public void HasTheGroupToggle(bool value)
		{
			Debug.Log("Testing, the group has toggled [" + value.ToString() + "]");
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00057DB7 File Offset: 0x00055FB7
		public void HasAToggleFlipped(bool value)
		{
			Debug.Log("Testing, a toggle has toggled [" + value.ToString() + "]");
		}

		// Token: 0x04000D67 RID: 3431
		[SerializeField]
		private bool m_AllowSwitchOff;

		// Token: 0x04000D68 RID: 3432
		private List<ExtensionsToggle> m_Toggles = new List<ExtensionsToggle>();

		// Token: 0x04000D69 RID: 3433
		public ExtensionsToggleGroup.ToggleGroupEvent onToggleGroupChanged = new ExtensionsToggleGroup.ToggleGroupEvent();

		// Token: 0x04000D6A RID: 3434
		public ExtensionsToggleGroup.ToggleGroupEvent onToggleGroupToggleChanged = new ExtensionsToggleGroup.ToggleGroupEvent();

		// Token: 0x02000833 RID: 2099
		[Serializable]
		public class ToggleGroupEvent : UnityEvent<bool>
		{
		}
	}
}

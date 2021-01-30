using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200017B RID: 379
	public class ReorderableListContent : MonoBehaviour
	{
		// Token: 0x06000EAA RID: 3754 RVA: 0x0005CEF1 File Offset: 0x0005B0F1
		private void OnEnable()
		{
			if (this._rect)
			{
				base.StartCoroutine(this.RefreshChildren());
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0005CF0D File Offset: 0x0005B10D
		public void OnTransformChildrenChanged()
		{
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RefreshChildren());
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0005CF24 File Offset: 0x0005B124
		public void Init(ReorderableList extList)
		{
			this._extList = extList;
			this._rect = base.GetComponent<RectTransform>();
			this._cachedChildren = new List<Transform>();
			this._cachedListElement = new List<ReorderableListElement>();
			base.StartCoroutine(this.RefreshChildren());
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0005CF5C File Offset: 0x0005B15C
		private IEnumerator RefreshChildren()
		{
			for (int i = 0; i < this._rect.childCount; i++)
			{
				if (!this._cachedChildren.Contains(this._rect.GetChild(i)))
				{
					this._ele = (this._rect.GetChild(i).gameObject.GetComponent<ReorderableListElement>() ?? this._rect.GetChild(i).gameObject.AddComponent<ReorderableListElement>());
					this._ele.Init(this._extList);
					this._cachedChildren.Add(this._rect.GetChild(i));
					this._cachedListElement.Add(this._ele);
				}
			}
			yield return 0;
			for (int j = this._cachedChildren.Count - 1; j >= 0; j--)
			{
				if (this._cachedChildren[j] == null)
				{
					this._cachedChildren.RemoveAt(j);
					this._cachedListElement.RemoveAt(j);
				}
			}
			yield break;
		}

		// Token: 0x04000E4A RID: 3658
		private List<Transform> _cachedChildren;

		// Token: 0x04000E4B RID: 3659
		private List<ReorderableListElement> _cachedListElement;

		// Token: 0x04000E4C RID: 3660
		private ReorderableListElement _ele;

		// Token: 0x04000E4D RID: 3661
		private ReorderableList _extList;

		// Token: 0x04000E4E RID: 3662
		private RectTransform _rect;
	}
}

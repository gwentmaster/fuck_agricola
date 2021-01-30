using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000197 RID: 407
	public class FancyScrollView<TData, TContext> : MonoBehaviour where TContext : class
	{
		// Token: 0x06000FA7 RID: 4007 RVA: 0x00063668 File Offset: 0x00061868
		protected void Awake()
		{
			this.cellBase.SetActive(false);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00063678 File Offset: 0x00061878
		protected void SetContext(TContext context)
		{
			this.context = context;
			for (int i = 0; i < this.cells.Count; i++)
			{
				this.cells[i].SetContext(context);
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x000636B4 File Offset: 0x000618B4
		private FancyScrollViewCell<TData, TContext> CreateCell()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.cellBase);
			gameObject.SetActive(true);
			FancyScrollViewCell<TData, TContext> component = gameObject.GetComponent<FancyScrollViewCell<TData, TContext>>();
			RectTransform rectTransform = component.transform as RectTransform;
			Vector3 localScale = component.transform.localScale;
			Vector2 sizeDelta = Vector2.zero;
			Vector2 offsetMin = Vector2.zero;
			Vector2 offsetMax = Vector2.zero;
			if (rectTransform)
			{
				sizeDelta = rectTransform.sizeDelta;
				offsetMin = rectTransform.offsetMin;
				offsetMax = rectTransform.offsetMax;
			}
			component.transform.SetParent(this.cellBase.transform.parent);
			component.transform.localScale = localScale;
			if (rectTransform)
			{
				rectTransform.sizeDelta = sizeDelta;
				rectTransform.offsetMin = offsetMin;
				rectTransform.offsetMax = offsetMax;
			}
			component.SetContext(this.context);
			component.SetVisible(false);
			return component;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0006377C File Offset: 0x0006197C
		private void UpdateCellForIndex(FancyScrollViewCell<TData, TContext> cell, int dataIndex)
		{
			if (this.loop)
			{
				dataIndex = this.GetLoopIndex(dataIndex, this.cellData.Count);
			}
			else if (dataIndex < 0 || dataIndex > this.cellData.Count - 1)
			{
				cell.SetVisible(false);
				return;
			}
			cell.SetVisible(true);
			cell.DataIndex = dataIndex;
			cell.UpdateContent(this.cellData[dataIndex]);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x000637E3 File Offset: 0x000619E3
		private int GetLoopIndex(int index, int length)
		{
			if (index < 0)
			{
				index = length - 1 + (index + 1) % length;
			}
			else if (index > length - 1)
			{
				index %= length;
			}
			return index;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00063802 File Offset: 0x00061A02
		protected void UpdateContents()
		{
			this.UpdatePosition(this.currentPosition);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00063810 File Offset: 0x00061A10
		protected void UpdatePosition(float position)
		{
			this.currentPosition = position;
			float num = position - this.cellOffset / this.cellInterval;
			float num2 = (Mathf.Ceil(num) - num) * this.cellInterval;
			int num3 = Mathf.CeilToInt(num);
			int i = 0;
			float num4 = num2;
			while (num4 <= 1f)
			{
				if (i >= this.cells.Count)
				{
					this.cells.Add(this.CreateCell());
				}
				num4 += this.cellInterval;
				i++;
			}
			i = 0;
			int loopIndex;
			for (float num5 = num2; num5 <= 1f; num5 += this.cellInterval)
			{
				int num6 = num3 + i;
				loopIndex = this.GetLoopIndex(num6, this.cells.Count);
				if (this.cells[loopIndex].gameObject.activeSelf)
				{
					this.cells[loopIndex].UpdatePosition(num5);
				}
				this.UpdateCellForIndex(this.cells[loopIndex], num6);
				i++;
			}
			loopIndex = this.GetLoopIndex(num3 + i, this.cells.Count);
			while (i < this.cells.Count)
			{
				this.cells[loopIndex].SetVisible(false);
				i++;
				loopIndex = this.GetLoopIndex(num3 + i, this.cells.Count);
			}
		}

		// Token: 0x04000EE4 RID: 3812
		[SerializeField]
		[Range(1E-45f, 1f)]
		private float cellInterval;

		// Token: 0x04000EE5 RID: 3813
		[SerializeField]
		[Range(0f, 1f)]
		private float cellOffset;

		// Token: 0x04000EE6 RID: 3814
		[SerializeField]
		private bool loop;

		// Token: 0x04000EE7 RID: 3815
		[SerializeField]
		private GameObject cellBase;

		// Token: 0x04000EE8 RID: 3816
		private float currentPosition;

		// Token: 0x04000EE9 RID: 3817
		private readonly List<FancyScrollViewCell<TData, TContext>> cells = new List<FancyScrollViewCell<!0, !1>>();

		// Token: 0x04000EEA RID: 3818
		protected TContext context;

		// Token: 0x04000EEB RID: 3819
		protected List<TData> cellData = new List<!0>();
	}
}

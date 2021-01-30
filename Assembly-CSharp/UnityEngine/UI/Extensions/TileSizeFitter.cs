using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A5 RID: 421
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("Layout/Extensions/Tile Size Fitter")]
	public class TileSizeFitter : UIBehaviour, ILayoutSelfController, ILayoutController
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x000668F3 File Offset: 0x00064AF3
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x000668FB File Offset: 0x00064AFB
		public Vector2 Border
		{
			get
			{
				return this.m_Border;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Vector2>(ref this.m_Border, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x00066911 File Offset: 0x00064B11
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x00066919 File Offset: 0x00064B19
		public Vector2 TileSize
		{
			get
			{
				return this.m_TileSize;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Vector2>(ref this.m_TileSize, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x0006692F File Offset: 0x00064B2F
		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00066951 File Offset: 0x00064B51
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0006695F File Offset: 0x00064B5F
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0006697D File Offset: 0x00064B7D
		protected override void OnRectTransformDimensionsChange()
		{
			this.UpdateRect();
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x00066988 File Offset: 0x00064B88
		private void UpdateRect()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_Tracker.Clear();
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY);
			this.rectTransform.anchorMin = Vector2.zero;
			this.rectTransform.anchorMax = Vector2.one;
			this.rectTransform.anchoredPosition = Vector2.zero;
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDelta);
			Vector2 vector = this.GetParentSize() - this.Border;
			if (this.TileSize.x > 0.001f)
			{
				vector.x -= Mathf.Floor(vector.x / this.TileSize.x) * this.TileSize.x;
			}
			else
			{
				vector.x = 0f;
			}
			if (this.TileSize.y > 0.001f)
			{
				vector.y -= Mathf.Floor(vector.y / this.TileSize.y) * this.TileSize.y;
			}
			else
			{
				vector.y = 0f;
			}
			this.rectTransform.sizeDelta = -vector;
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00066AC8 File Offset: 0x00064CC8
		private Vector2 GetParentSize()
		{
			RectTransform rectTransform = this.rectTransform.parent as RectTransform;
			if (!rectTransform)
			{
				return Vector2.zero;
			}
			return rectTransform.rect.size;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void SetLayoutHorizontal()
		{
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void SetLayoutVertical()
		{
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00066B02 File Offset: 0x00064D02
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateRect();
		}

		// Token: 0x04000F5C RID: 3932
		[SerializeField]
		private Vector2 m_Border = Vector2.zero;

		// Token: 0x04000F5D RID: 3933
		[SerializeField]
		private Vector2 m_TileSize = Vector2.zero;

		// Token: 0x04000F5E RID: 3934
		[NonSerialized]
		private RectTransform m_Rect;

		// Token: 0x04000F5F RID: 3935
		private DrivenRectTransformTracker m_Tracker;
	}
}

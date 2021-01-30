using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000195 RID: 405
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Effects/Extensions/Shining Effect")]
	public class ShineEffector : MonoBehaviour
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x000631E3 File Offset: 0x000613E3
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x000631EB File Offset: 0x000613EB
		public float YOffset
		{
			get
			{
				return this.yOffset;
			}
			set
			{
				this.ChangeVal(value);
				this.yOffset = value;
			}
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x000631FC File Offset: 0x000613FC
		private void OnEnable()
		{
			if (this.effector == null)
			{
				GameObject gameObject = new GameObject("effector");
				this.effectRoot = new GameObject("ShineEffect");
				this.effectRoot.transform.SetParent(base.transform);
				this.effectRoot.AddComponent<Image>().sprite = base.gameObject.GetComponent<Image>().sprite;
				this.effectRoot.GetComponent<Image>().type = base.gameObject.GetComponent<Image>().type;
				this.effectRoot.AddComponent<Mask>().showMaskGraphic = false;
				this.effectRoot.transform.localScale = Vector3.one;
				this.effectRoot.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
				this.effectRoot.GetComponent<RectTransform>().anchorMax = Vector2.one;
				this.effectRoot.GetComponent<RectTransform>().anchorMin = Vector2.zero;
				this.effectRoot.GetComponent<RectTransform>().offsetMax = Vector2.zero;
				this.effectRoot.GetComponent<RectTransform>().offsetMin = Vector2.zero;
				this.effectRoot.transform.SetAsFirstSibling();
				gameObject.AddComponent<RectTransform>();
				gameObject.transform.SetParent(this.effectRoot.transform);
				this.effectorRect = gameObject.GetComponent<RectTransform>();
				this.effectorRect.localScale = Vector3.one;
				this.effectorRect.anchoredPosition3D = Vector3.zero;
				this.effectorRect.gameObject.AddComponent<ShineEffect>();
				this.effectorRect.anchorMax = Vector2.one;
				this.effectorRect.anchorMin = Vector2.zero;
				this.effectorRect.Rotate(0f, 0f, -8f);
				this.effector = gameObject.GetComponent<ShineEffect>();
				this.effectorRect.offsetMax = Vector2.zero;
				this.effectorRect.offsetMin = Vector2.zero;
				this.OnValidate();
			}
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x000633F0 File Offset: 0x000615F0
		private void OnValidate()
		{
			this.effector.Yoffset = this.yOffset;
			this.effector.Width = this.width;
			if (this.yOffset <= -1f || this.yOffset >= 1f)
			{
				this.effectRoot.SetActive(false);
				return;
			}
			if (!this.effectRoot.activeSelf)
			{
				this.effectRoot.SetActive(true);
			}
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00063460 File Offset: 0x00061660
		private void ChangeVal(float value)
		{
			this.effector.Yoffset = value;
			if (value <= -1f || value >= 1f)
			{
				this.effectRoot.SetActive(false);
				return;
			}
			if (!this.effectRoot.activeSelf)
			{
				this.effectRoot.SetActive(true);
			}
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x000634AF File Offset: 0x000616AF
		private void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(this.effectRoot);
				return;
			}
			Object.Destroy(this.effectRoot);
		}

		// Token: 0x04000EDB RID: 3803
		public ShineEffect effector;

		// Token: 0x04000EDC RID: 3804
		[SerializeField]
		[HideInInspector]
		private GameObject effectRoot;

		// Token: 0x04000EDD RID: 3805
		[Range(-1f, 1f)]
		public float yOffset = -1f;

		// Token: 0x04000EDE RID: 3806
		[Range(0.1f, 1f)]
		public float width = 0.5f;

		// Token: 0x04000EDF RID: 3807
		private RectTransform effectorRect;
	}
}

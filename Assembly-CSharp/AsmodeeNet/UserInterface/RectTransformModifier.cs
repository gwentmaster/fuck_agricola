using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200063C RID: 1596
	[RequireComponent(typeof(RectTransform))]
	public class RectTransformModifier : MonoBehaviour
	{
		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06003AA3 RID: 15011 RVA: 0x00123627 File Offset: 0x00121827
		public bool HasFrameAndAnchors
		{
			get
			{
				return (this.parameters & RectTransformModifier.Parameters.FrameAndAnchors) > (RectTransformModifier.Parameters)0;
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x00123634 File Offset: 0x00121834
		public bool HasPivot
		{
			get
			{
				return (this.parameters & RectTransformModifier.Parameters.Pivot) > (RectTransformModifier.Parameters)0;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06003AA5 RID: 15013 RVA: 0x00123641 File Offset: 0x00121841
		public bool HasRotation
		{
			get
			{
				return (this.parameters & RectTransformModifier.Parameters.Rotation) > (RectTransformModifier.Parameters)0;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x0012364E File Offset: 0x0012184E
		public bool HasScale
		{
			get
			{
				return (this.parameters & RectTransformModifier.Parameters.Scale) > (RectTransformModifier.Parameters)0;
			}
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x0012365B File Offset: 0x0012185B
		private void Start()
		{
			this.aspectSpecifications.Sort((RectTransformModifier.AspectSpecification a, RectTransformModifier.AspectSpecification b) => a.aspect.CompareTo(b.aspect));
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x00123687 File Offset: 0x00121887
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.AspectDidChange += this._SetNeedsUpdate;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this._SetNeedsUpdate;
			this._SetNeedsUpdate();
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x001236C5 File Offset: 0x001218C5
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.AspectDidChange -= this._SetNeedsUpdate;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this._SetNeedsUpdate;
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x00123705 File Offset: 0x00121905
		private void _SetNeedsUpdate()
		{
			this._needsUpdate = true;
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x00123710 File Offset: 0x00121910
		private void Update()
		{
			if (this.strategy == RectTransformModifier.Strategy.PerAspect && this.reference != null && !Mathf.Approximately(this.reference.rect.y, 0f))
			{
				float num = this.reference.rect.x / this.reference.rect.y;
				if (!Mathf.Approximately(num, this._referenceAspect))
				{
					this._referenceAspect = num;
					this._needsUpdate = true;
				}
			}
			if (this._needsUpdate)
			{
				this._needsUpdate = false;
				if (this.strategy == RectTransformModifier.Strategy.PerAspect)
				{
					this._ApplyPerAspectStrategy();
					return;
				}
				this._ApplyPerDisplayModeStrategy();
			}
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x001237BC File Offset: 0x001219BC
		private void _ApplyPerAspectStrategy()
		{
			Preferences preferences = CoreApplication.Instance.Preferences;
			float num = (this.reference != null) ? this._referenceAspect : preferences.Aspect;
			Preferences.DisplayMode interfaceDisplayMode = preferences.InterfaceDisplayMode;
			RectTransform rectTransform = base.transform as RectTransform;
			RectTransformModifier.AspectSpecification aspectSpecification = this.aspectSpecifications.First<RectTransformModifier.AspectSpecification>();
			if (num <= aspectSpecification.aspect)
			{
				RectTransformModifier.RectTransformSpecification rectTransformSpecification = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(aspectSpecification.displayModeSpecifications, interfaceDisplayMode);
				if (this.HasFrameAndAnchors)
				{
					rectTransform.anchorMin = rectTransformSpecification.anchorMin;
					rectTransform.anchorMax = rectTransformSpecification.anchorMax;
					rectTransform.sizeDelta = rectTransformSpecification.sizeDelta;
					rectTransform.localPosition = new Vector3(0f, 0f, rectTransformSpecification.zPosition);
					rectTransform.anchoredPosition = rectTransformSpecification.anchoredPosition;
				}
				if (this.HasPivot)
				{
					rectTransform.pivot = rectTransformSpecification.pivot;
				}
				if (this.HasRotation)
				{
					rectTransform.localEulerAngles = rectTransformSpecification.rotation;
				}
				if (this.HasScale)
				{
					rectTransform.localScale = rectTransformSpecification.scale;
					return;
				}
			}
			else
			{
				RectTransformModifier.AspectSpecification aspectSpecification2 = this.aspectSpecifications.Last<RectTransformModifier.AspectSpecification>();
				if (num >= aspectSpecification2.aspect)
				{
					RectTransformModifier.RectTransformSpecification rectTransformSpecification2 = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(aspectSpecification2.displayModeSpecifications, interfaceDisplayMode);
					if (this.HasFrameAndAnchors)
					{
						rectTransform.anchorMin = rectTransformSpecification2.anchorMin;
						rectTransform.anchorMax = rectTransformSpecification2.anchorMax;
						rectTransform.sizeDelta = rectTransformSpecification2.sizeDelta;
						rectTransform.localPosition = new Vector3(0f, 0f, rectTransformSpecification2.zPosition);
						rectTransform.anchoredPosition = rectTransformSpecification2.anchoredPosition;
					}
					if (this.HasPivot)
					{
						rectTransform.pivot = rectTransformSpecification2.pivot;
					}
					if (this.HasRotation)
					{
						rectTransform.localEulerAngles = rectTransformSpecification2.rotation;
					}
					if (this.HasScale)
					{
						rectTransform.localScale = rectTransformSpecification2.scale;
						return;
					}
				}
				else
				{
					RectTransformModifier.AspectSpecification aspectSpecification4;
					RectTransformModifier.AspectSpecification aspectSpecification3 = aspectSpecification4 = this.aspectSpecifications.First<RectTransformModifier.AspectSpecification>();
					foreach (RectTransformModifier.AspectSpecification aspectSpecification5 in this.aspectSpecifications)
					{
						if (aspectSpecification5.aspect >= num)
						{
							aspectSpecification3 = aspectSpecification5;
							break;
						}
						aspectSpecification3 = (aspectSpecification4 = aspectSpecification5);
					}
					RectTransformModifier.RectTransformSpecification rectTransformSpecification3 = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(aspectSpecification4.displayModeSpecifications, interfaceDisplayMode);
					RectTransformModifier.RectTransformSpecification rectTransformSpecification4 = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(aspectSpecification3.displayModeSpecifications, interfaceDisplayMode);
					float t = (num - aspectSpecification4.aspect) / (aspectSpecification3.aspect - aspectSpecification4.aspect);
					if (this.HasFrameAndAnchors)
					{
						rectTransform.anchorMin = Vector2.Lerp(rectTransformSpecification3.anchorMin, rectTransformSpecification4.anchorMin, t);
						rectTransform.anchorMax = Vector2.Lerp(rectTransformSpecification3.anchorMax, rectTransformSpecification4.anchorMax, t);
						rectTransform.sizeDelta = Vector2.Lerp(rectTransformSpecification3.sizeDelta, rectTransformSpecification4.sizeDelta, t);
						rectTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(rectTransformSpecification3.zPosition, rectTransformSpecification4.zPosition, t));
						rectTransform.anchoredPosition = Vector2.Lerp(rectTransformSpecification3.anchoredPosition, rectTransformSpecification4.anchoredPosition, t);
					}
					if (this.HasPivot)
					{
						rectTransform.pivot = Vector2.Lerp(rectTransformSpecification3.pivot, rectTransformSpecification4.pivot, t);
					}
					if (this.HasRotation)
					{
						rectTransform.localEulerAngles = Vector3.Lerp(rectTransformSpecification3.rotation, rectTransformSpecification4.rotation, t);
					}
					if (this.HasScale)
					{
						rectTransform.localScale = Vector3.Lerp(rectTransformSpecification3.scale, rectTransformSpecification4.scale, t);
					}
				}
			}
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x00123B38 File Offset: 0x00121D38
		private void _ApplyPerDisplayModeStrategy()
		{
			Preferences.DisplayMode interfaceDisplayMode = CoreApplication.Instance.Preferences.InterfaceDisplayMode;
			RectTransform rectTransform = base.transform as RectTransform;
			RectTransformModifier.RectTransformSpecification rectTransformSpecification = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(this.displayModeSpecifications, interfaceDisplayMode);
			if (this.HasFrameAndAnchors)
			{
				rectTransform.anchorMin = rectTransformSpecification.anchorMin;
				rectTransform.anchorMax = rectTransformSpecification.anchorMax;
				rectTransform.sizeDelta = rectTransformSpecification.sizeDelta;
				rectTransform.localPosition = new Vector3(0f, 0f, rectTransformSpecification.zPosition);
				rectTransform.anchoredPosition = rectTransformSpecification.anchoredPosition;
			}
			if (this.HasPivot)
			{
				rectTransform.pivot = rectTransformSpecification.pivot;
			}
			if (this.HasRotation)
			{
				rectTransform.localEulerAngles = rectTransformSpecification.rotation;
			}
			if (this.HasScale)
			{
				rectTransform.localScale = rectTransformSpecification.scale;
			}
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x00123C00 File Offset: 0x00121E00
		private static RectTransformModifier.RectTransformSpecification _FindRectTransformSpecificationForDisplayMode(List<RectTransformModifier.DisplayModeSpecification> displayModeSpecifications, Preferences.DisplayMode displayMode)
		{
			RectTransformModifier.RectTransformSpecification result = null;
			foreach (RectTransformModifier.DisplayModeSpecification displayModeSpecification in displayModeSpecifications)
			{
				if (displayModeSpecification.displayMode == Preferences.DisplayMode.Unknown)
				{
					result = displayModeSpecification.specification;
				}
				else if (displayModeSpecification.displayMode == displayMode)
				{
					return displayModeSpecification.specification;
				}
			}
			return result;
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x00123C70 File Offset: 0x00121E70
		private void OnDrawGizmosSelected()
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x00123C78 File Offset: 0x00121E78
		private Color _GizmosColorForDisplayMode(Preferences.DisplayMode? displayMode)
		{
			if (displayMode != null)
			{
				switch (displayMode.Value)
				{
				case Preferences.DisplayMode.Small:
					return Color.green;
				case Preferences.DisplayMode.Regular:
					return Color.cyan;
				case Preferences.DisplayMode.Big:
					return Color.blue;
				}
			}
			return Color.gray;
		}

		// Token: 0x04002602 RID: 9730
		private const string _documentation = "<b>RectTransformModifier</b> automatically updates a <b>RectTransform</b> according to the current aspect ratio and interface <b>DisplayMode</b>";

		// Token: 0x04002603 RID: 9731
		public RectTransformModifier.Parameters parameters;

		// Token: 0x04002604 RID: 9732
		public RectTransformModifier.Strategy strategy = RectTransformModifier.Strategy.PerDisplayMode;

		// Token: 0x04002605 RID: 9733
		public RectTransform reference;

		// Token: 0x04002606 RID: 9734
		private float _referenceAspect;

		// Token: 0x04002607 RID: 9735
		public List<RectTransformModifier.AspectSpecification> aspectSpecifications = new List<RectTransformModifier.AspectSpecification>();

		// Token: 0x04002608 RID: 9736
		public List<RectTransformModifier.DisplayModeSpecification> displayModeSpecifications = new List<RectTransformModifier.DisplayModeSpecification>();

		// Token: 0x04002609 RID: 9737
		private bool _needsUpdate;

		// Token: 0x02000935 RID: 2357
		[Flags]
		public enum Parameters
		{
			// Token: 0x040030E2 RID: 12514
			FrameAndAnchors = 1,
			// Token: 0x040030E3 RID: 12515
			Pivot = 2,
			// Token: 0x040030E4 RID: 12516
			Rotation = 4,
			// Token: 0x040030E5 RID: 12517
			Scale = 8
		}

		// Token: 0x02000936 RID: 2358
		public enum Strategy
		{
			// Token: 0x040030E7 RID: 12519
			PerAspect,
			// Token: 0x040030E8 RID: 12520
			PerDisplayMode
		}

		// Token: 0x02000937 RID: 2359
		[Serializable]
		public class AspectSpecification
		{
			// Token: 0x040030E9 RID: 12521
			public float aspect = 1f;

			// Token: 0x040030EA RID: 12522
			public List<RectTransformModifier.DisplayModeSpecification> displayModeSpecifications = new List<RectTransformModifier.DisplayModeSpecification>();
		}

		// Token: 0x02000938 RID: 2360
		[Serializable]
		public class DisplayModeSpecification
		{
			// Token: 0x040030EB RID: 12523
			public Preferences.DisplayMode displayMode;

			// Token: 0x040030EC RID: 12524
			public RectTransformModifier.RectTransformSpecification specification;
		}

		// Token: 0x02000939 RID: 2361
		[Serializable]
		public class RectTransformSpecification
		{
			// Token: 0x040030ED RID: 12525
			public Vector2 anchoredPosition;

			// Token: 0x040030EE RID: 12526
			public Vector2 sizeDelta;

			// Token: 0x040030EF RID: 12527
			public float zPosition;

			// Token: 0x040030F0 RID: 12528
			public Vector2 anchorMin;

			// Token: 0x040030F1 RID: 12529
			public Vector2 anchorMax;

			// Token: 0x040030F2 RID: 12530
			public Vector2 pivot;

			// Token: 0x040030F3 RID: 12531
			public Vector3 rotation;

			// Token: 0x040030F4 RID: 12532
			public Vector3 scale;
		}
	}
}

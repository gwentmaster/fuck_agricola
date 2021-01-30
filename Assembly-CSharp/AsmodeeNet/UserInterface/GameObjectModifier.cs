using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000637 RID: 1591
	public class GameObjectModifier : MonoBehaviour
	{
		// Token: 0x06003A90 RID: 14992 RVA: 0x00122FCC File Offset: 0x001211CC
		private void Start()
		{
			this.aspectSpecifications.Sort((GameObjectModifier.AspectSpecification a, GameObjectModifier.AspectSpecification b) => a.aspect.CompareTo(b.aspect));
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x00122FF8 File Offset: 0x001211F8
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.AspectDidChange += this._SetNeedsUpdate;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this._SetNeedsUpdate;
			this._SetNeedsUpdate();
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x00123036 File Offset: 0x00121236
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.AspectDidChange -= this._SetNeedsUpdate;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this._SetNeedsUpdate;
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x00123076 File Offset: 0x00121276
		private void _SetNeedsUpdate()
		{
			this._needsUpdate = true;
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x00123080 File Offset: 0x00121280
		private void Update()
		{
			if (this.strategy == GameObjectModifier.Strategy.PerAspect && this.reference != null && !Mathf.Approximately(this.reference.rect.y, 0f))
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
				if (this.strategy == GameObjectModifier.Strategy.PerAspect)
				{
					this._ApplyPerAspectStrategy();
					return;
				}
				this._ApplyPerDisplayModeStrategy();
			}
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x0012312C File Offset: 0x0012132C
		private void _ApplyPerAspectStrategy()
		{
			Preferences preferences = CoreApplication.Instance.Preferences;
			float num = (this.reference != null) ? this._referenceAspect : preferences.Aspect;
			Preferences.DisplayMode interfaceDisplayMode = preferences.InterfaceDisplayMode;
			GameObjectModifier.AspectSpecification aspectSpecification = this.aspectSpecifications.First<GameObjectModifier.AspectSpecification>();
			if (num <= aspectSpecification.aspect)
			{
				List<GameObjectModifier.GameObjectSpecification> list = GameObjectModifier._FindGameObjectSpecificationsForDisplayMode(aspectSpecification.displayModeSpecifications, interfaceDisplayMode);
				for (int i = 0; i < this._gameObjects.Length; i++)
				{
					this._gameObjects[i].SetActive(list[i].active);
				}
				return;
			}
			GameObjectModifier.AspectSpecification aspectSpecification2 = this.aspectSpecifications.Last<GameObjectModifier.AspectSpecification>();
			if (num >= aspectSpecification2.aspect)
			{
				List<GameObjectModifier.GameObjectSpecification> list2 = GameObjectModifier._FindGameObjectSpecificationsForDisplayMode(aspectSpecification2.displayModeSpecifications, interfaceDisplayMode);
				for (int j = 0; j < this._gameObjects.Length; j++)
				{
					this._gameObjects[j].SetActive(list2[j].active);
				}
				return;
			}
			GameObjectModifier.AspectSpecification aspectSpecification4;
			GameObjectModifier.AspectSpecification aspectSpecification3 = aspectSpecification4 = this.aspectSpecifications.First<GameObjectModifier.AspectSpecification>();
			foreach (GameObjectModifier.AspectSpecification aspectSpecification5 in this.aspectSpecifications)
			{
				if (aspectSpecification5.aspect >= num)
				{
					aspectSpecification3 = aspectSpecification5;
					break;
				}
				aspectSpecification3 = (aspectSpecification4 = aspectSpecification5);
			}
			List<GameObjectModifier.GameObjectSpecification> list3 = GameObjectModifier._FindGameObjectSpecificationsForDisplayMode(aspectSpecification4.displayModeSpecifications, interfaceDisplayMode);
			List<GameObjectModifier.GameObjectSpecification> list4 = GameObjectModifier._FindGameObjectSpecificationsForDisplayMode(aspectSpecification3.displayModeSpecifications, interfaceDisplayMode);
			float num2 = (num - aspectSpecification4.aspect) / (aspectSpecification3.aspect - aspectSpecification4.aspect);
			for (int k = 0; k < this._gameObjects.Length; k++)
			{
				bool active = (num2 < 0.5f) ? list3[k].active : list4[k].active;
				this._gameObjects[k].SetActive(active);
			}
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x0012330C File Offset: 0x0012150C
		private void _ApplyPerDisplayModeStrategy()
		{
			Preferences.DisplayMode interfaceDisplayMode = CoreApplication.Instance.Preferences.InterfaceDisplayMode;
			List<GameObjectModifier.GameObjectSpecification> list = GameObjectModifier._FindGameObjectSpecificationsForDisplayMode(this.displayModeSpecifications, interfaceDisplayMode);
			for (int i = 0; i < this._gameObjects.Length; i++)
			{
				this._gameObjects[i].SetActive(list[i].active);
			}
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x00123364 File Offset: 0x00121564
		private static List<GameObjectModifier.GameObjectSpecification> _FindGameObjectSpecificationsForDisplayMode(List<GameObjectModifier.DisplayModeSpecification> displayModeSpecifications, Preferences.DisplayMode displayMode)
		{
			List<GameObjectModifier.GameObjectSpecification> result = null;
			foreach (GameObjectModifier.DisplayModeSpecification displayModeSpecification in displayModeSpecifications)
			{
				if (displayModeSpecification.displayMode == Preferences.DisplayMode.Unknown)
				{
					result = displayModeSpecification.specifications;
				}
				else if (displayModeSpecification.displayMode == displayMode)
				{
					return displayModeSpecification.specifications;
				}
			}
			return result;
		}

		// Token: 0x040025ED RID: 9709
		private const string _documentation = "<b>GameObjectModifier</b> automatically activate or deactivate <b>GameObject</b>s according to the current aspect ratio and interface <b>DisplayMode</b>";

		// Token: 0x040025EE RID: 9710
		[SerializeField]
		private GameObject[] _gameObjects;

		// Token: 0x040025EF RID: 9711
		public GameObjectModifier.Strategy strategy = GameObjectModifier.Strategy.PerDisplayMode;

		// Token: 0x040025F0 RID: 9712
		public RectTransform reference;

		// Token: 0x040025F1 RID: 9713
		private float _referenceAspect;

		// Token: 0x040025F2 RID: 9714
		public List<GameObjectModifier.AspectSpecification> aspectSpecifications = new List<GameObjectModifier.AspectSpecification>();

		// Token: 0x040025F3 RID: 9715
		public List<GameObjectModifier.DisplayModeSpecification> displayModeSpecifications = new List<GameObjectModifier.DisplayModeSpecification>();

		// Token: 0x040025F4 RID: 9716
		private bool _needsUpdate;

		// Token: 0x0200092D RID: 2349
		public enum Strategy
		{
			// Token: 0x040030D3 RID: 12499
			PerAspect,
			// Token: 0x040030D4 RID: 12500
			PerDisplayMode
		}

		// Token: 0x0200092E RID: 2350
		[Serializable]
		public class AspectSpecification
		{
			// Token: 0x040030D5 RID: 12501
			public float aspect = 1f;

			// Token: 0x040030D6 RID: 12502
			public List<GameObjectModifier.DisplayModeSpecification> displayModeSpecifications = new List<GameObjectModifier.DisplayModeSpecification>();
		}

		// Token: 0x0200092F RID: 2351
		[Serializable]
		public class DisplayModeSpecification
		{
			// Token: 0x040030D7 RID: 12503
			public Preferences.DisplayMode displayMode;

			// Token: 0x040030D8 RID: 12504
			public List<GameObjectModifier.GameObjectSpecification> specifications;
		}

		// Token: 0x02000930 RID: 2352
		[Serializable]
		public class GameObjectSpecification
		{
			// Token: 0x040030D9 RID: 12505
			public bool active;
		}
	}
}

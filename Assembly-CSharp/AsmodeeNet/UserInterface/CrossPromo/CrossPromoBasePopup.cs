using System;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000652 RID: 1618
	public abstract class CrossPromoBasePopup : MonoBehaviour
	{
		// Token: 0x06003BC8 RID: 15304 RVA: 0x0012877A File Offset: 0x0012697A
		protected virtual void Awake()
		{
			this._responsivePopUp = base.GetComponent<ResponsivePopUp>();
			if (this._responsivePopUp == null)
			{
				AsmoLogger.Error("CrossPromoBasePopup", "Missing ResponsivePopUp behavior", null);
			}
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x001287A6 File Offset: 0x001269A6
		public virtual void Dismiss()
		{
			if (base.gameObject.activeSelf)
			{
				this._responsivePopUp.FadeOut(delegate
				{
					UnityEngine.Object.Destroy(base.gameObject);
				});
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04002681 RID: 9857
		private const string _kModuleName = "CrossPromoBasePopup";

		// Token: 0x04002682 RID: 9858
		private ResponsivePopUp _responsivePopUp;
	}
}

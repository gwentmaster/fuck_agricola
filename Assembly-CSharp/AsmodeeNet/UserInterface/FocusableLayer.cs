using System;
using System.Collections.Generic;
using AsmodeeNet.Foundation;
using UnityEngine;
using UnityEngine.Events;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000644 RID: 1604
	public class FocusableLayer : MonoBehaviour
	{
		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x00125985 File Offset: 0x00123B85
		public IList<Focusable> Focusables
		{
			get
			{
				return this._focusables.AsReadOnly();
			}
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x00125992 File Offset: 0x00123B92
		private void OnEnable()
		{
			CoreApplication.Instance.UINavigationManager.RegisterFocusableLayer(this);
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x001259A4 File Offset: 0x00123BA4
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.UINavigationManager.UnRegisterFocusableLayer(this);
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x001259BE File Offset: 0x00123BBE
		public void RegisterFocusable(Focusable focusable)
		{
			this._focusables.Add(focusable);
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x001259CC File Offset: 0x00123BCC
		public void UnRegisterFocusable(Focusable focusable)
		{
			this._focusables.Remove(focusable);
		}

		// Token: 0x04002646 RID: 9798
		private const string _documentation = "<b>FocusableLayer</b> aggregates <b>Focusable</b> controls. It is usally used for the root node of a modal dialog box with the <b>modal</b> flag set to true.";

		// Token: 0x04002647 RID: 9799
		public bool modal = true;

		// Token: 0x04002648 RID: 9800
		public UnityEvent OnBackAction;

		// Token: 0x04002649 RID: 9801
		private List<Focusable> _focusables = new List<Focusable>();
	}
}

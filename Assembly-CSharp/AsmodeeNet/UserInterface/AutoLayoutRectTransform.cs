using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200061F RID: 1567
	[RequireComponent(typeof(RectTransform))]
	public class AutoLayoutRectTransform : MonoBehaviour
	{
		// Token: 0x060039A0 RID: 14752 RVA: 0x0011E927 File Offset: 0x0011CB27
		private void Awake()
		{
			this._rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x0011E935 File Offset: 0x0011CB35
		private void OnEnable()
		{
			CoreApplication.Instance.CommunityHubLauncher.communityHubDidStart += this._LinkCommunityHub;
			this._LinkCommunityHub();
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x0011E958 File Offset: 0x0011CB58
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.CommunityHubLauncher.communityHubDidStart -= this._LinkCommunityHub;
			CoreApplication.Instance.CommunityHub.RemoveTransformToAutoLayout(this._rectTransform);
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x0011E992 File Offset: 0x0011CB92
		private void _LinkCommunityHub()
		{
			if (!CoreApplication.Instance.CommunityHubLauncher.IsCommunityHubLaunched)
			{
				return;
			}
			CoreApplication.Instance.CommunityHub.AddTransformToAutoLayout(this._rectTransform);
		}

		// Token: 0x04002544 RID: 9540
		private const string _documentation = "<b>AutoLayoutRectTransform</b> automatically registers its <b>RectTransform</b> to <b>CommunityHub.TransformsToAutoLayout</b>.";

		// Token: 0x04002545 RID: 9541
		private RectTransform _rectTransform;
	}
}

using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
[RequireComponent(typeof(AgricolaAnimationLocator))]
public class AdjustCardScaleForAspectRatio : MonoBehaviour
{
	// Token: 0x06000165 RID: 357 RVA: 0x00007954 File Offset: 0x00005B54
	public void Start()
	{
		if (PlatformManager.s_instance != null)
		{
			int aspectRatioType = (int)PlatformManager.s_instance.GetAspectRatioType();
			AgricolaAnimationLocator component = base.GetComponent<AgricolaAnimationLocator>();
			float[] array = this.m_AdjustScaleFullSize;
			if (component.GetCardDisplayState() == ECardDisplayState.DISPLAY_HALF)
			{
				array = this.m_AdjustScaleHalfHeight;
			}
			if (array != null && aspectRatioType < array.Length)
			{
				float num = array[aspectRatioType];
				Vector3 localScale = base.transform.localScale;
				localScale.x = num;
				localScale.y = num;
				base.transform.localScale = localScale;
			}
		}
	}

	// Token: 0x040000A8 RID: 168
	[SerializeField]
	private float[] m_AdjustScaleFullSize;

	// Token: 0x040000A9 RID: 169
	[SerializeField]
	private float[] m_AdjustScaleHalfHeight;
}

using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class AnimatePlaceholder : MonoBehaviour
{
	// Token: 0x0600074B RID: 1867 RVA: 0x00035B6A File Offset: 0x00033D6A
	public void SetOwner(AnimateObject owner)
	{
		this.m_Owner = owner;
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x00035B73 File Offset: 0x00033D73
	public AnimateObject GetOwner()
	{
		return this.m_Owner;
	}

	// Token: 0x04000889 RID: 2185
	private AnimateObject m_Owner;
}

using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010F RID: 271
public class UVScrollHorizontal : MonoBehaviour
{
	// Token: 0x06000A29 RID: 2601 RVA: 0x00043870 File Offset: 0x00041A70
	private void FixedUpdate()
	{
		if (this.ImageToScroll != null)
		{
			float x = Time.time * this.scrollSpeed;
			this.ImageToScroll.uvRect = new Rect(x, this.ImageToScroll.uvRect.y, this.ImageToScroll.uvRect.width, this.ImageToScroll.uvRect.height);
		}
	}

	// Token: 0x04000AB6 RID: 2742
	public float scrollSpeed = 0.5f;

	// Token: 0x04000AB7 RID: 2743
	public RawImage ImageToScroll;
}

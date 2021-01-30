using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010E RID: 270
public class UVScroll : MonoBehaviour
{
	// Token: 0x06000A27 RID: 2599 RVA: 0x000437F0 File Offset: 0x000419F0
	private void FixedUpdate()
	{
		if (this.ImageToScroll != null)
		{
			float x = Time.time * this.scrollSpeed;
			float y = Time.time * this.scrollSpeed2;
			this.ImageToScroll.uvRect = new Rect(x, y, this.ImageToScroll.uvRect.width, this.ImageToScroll.uvRect.height);
		}
	}

	// Token: 0x04000AB3 RID: 2739
	public float scrollSpeed = 0.5f;

	// Token: 0x04000AB4 RID: 2740
	public float scrollSpeed2;

	// Token: 0x04000AB5 RID: 2741
	public RawImage ImageToScroll;
}

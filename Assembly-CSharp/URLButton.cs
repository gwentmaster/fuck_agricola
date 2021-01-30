using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class URLButton : MonoBehaviour
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x000437D6 File Offset: 0x000419D6
	public void GoToURL()
	{
		if (!string.IsNullOrEmpty(this.URL))
		{
			Application.OpenURL(this.URL);
		}
	}

	// Token: 0x04000AB2 RID: 2738
	public string URL;
}

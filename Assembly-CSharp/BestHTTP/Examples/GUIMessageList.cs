using System;
using System.Collections.Generic;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x02000580 RID: 1408
	public class GUIMessageList
	{
		// Token: 0x060033A9 RID: 13225 RVA: 0x00105698 File Offset: 0x00103898
		public void Draw()
		{
			this.Draw((float)Screen.width, 0f);
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x001056AC File Offset: 0x001038AC
		public void Draw(float minWidth, float minHeight)
		{
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, new GUILayoutOption[]
			{
				GUILayout.MinHeight(minHeight)
			});
			for (int i = 0; i < this.messages.Count; i++)
			{
				GUILayout.Label(this.messages[i], new GUILayoutOption[]
				{
					GUILayout.MinWidth(minWidth)
				});
			}
			GUILayout.EndScrollView();
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x00105716 File Offset: 0x00103916
		public void Add(string msg)
		{
			this.messages.Add(msg);
			this.scrollPos = new Vector2(this.scrollPos.x, float.MaxValue);
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x0010573F File Offset: 0x0010393F
		public void Clear()
		{
			this.messages.Clear();
		}

		// Token: 0x040021E7 RID: 8679
		private List<string> messages = new List<string>();

		// Token: 0x040021E8 RID: 8680
		private Vector2 scrollPos;
	}
}

using System;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x0200057F RID: 1407
	public static class GUIHelper
	{
		// Token: 0x060033A5 RID: 13221 RVA: 0x0010558C File Offset: 0x0010378C
		private static void Setup()
		{
			if (GUIHelper.centerAlignedLabel == null)
			{
				GUIHelper.centerAlignedLabel = new GUIStyle(GUI.skin.label);
				GUIHelper.centerAlignedLabel.alignment = TextAnchor.MiddleCenter;
				GUIHelper.rightAlignedLabel = new GUIStyle(GUI.skin.label);
				GUIHelper.rightAlignedLabel.alignment = TextAnchor.MiddleRight;
			}
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x001055E0 File Offset: 0x001037E0
		public static void DrawArea(Rect area, bool drawHeader, Action action)
		{
			GUIHelper.Setup();
			GUI.Box(area, string.Empty);
			GUILayout.BeginArea(area);
			if (drawHeader)
			{
				GUIHelper.DrawCenteredText(SampleSelector.SelectedSample.DisplayName);
				GUILayout.Space(5f);
			}
			if (action != null)
			{
				action();
			}
			GUILayout.EndArea();
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x0010562D File Offset: 0x0010382D
		public static void DrawCenteredText(string msg)
		{
			GUIHelper.Setup();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.Label(msg, GUIHelper.centerAlignedLabel, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x0010565D File Offset: 0x0010385D
		public static void DrawRow(string key, string value)
		{
			GUIHelper.Setup();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(key, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.Label(value, GUIHelper.rightAlignedLabel, Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		// Token: 0x040021E4 RID: 8676
		private static GUIStyle centerAlignedLabel;

		// Token: 0x040021E5 RID: 8677
		private static GUIStyle rightAlignedLabel;

		// Token: 0x040021E6 RID: 8678
		public static Rect ClientArea;
	}
}

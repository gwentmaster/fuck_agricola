using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001FB RID: 507
	public class AwesomeMenu : Menu<AwesomeMenu>
	{
		// Token: 0x0600129F RID: 4767 RVA: 0x00071174 File Offset: 0x0006F374
		public static void Show(float awesomeness)
		{
			Menu<AwesomeMenu>.Open();
			Menu<AwesomeMenu>.Instance.Background.color = new Color32((byte)(129f * awesomeness), (byte)(197f * awesomeness), (byte)(34f * awesomeness), byte.MaxValue);
			Menu<AwesomeMenu>.Instance.Title.text = string.Format("This menu is {0:P} awesome", awesomeness);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x000711DB File Offset: 0x0006F3DB
		public static void Hide()
		{
			Menu<AwesomeMenu>.Close();
		}

		// Token: 0x040010D0 RID: 4304
		public Image Background;

		// Token: 0x040010D1 RID: 4305
		public Text Title;
	}
}

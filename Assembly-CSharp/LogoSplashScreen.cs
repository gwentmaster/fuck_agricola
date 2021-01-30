using System;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class LogoSplashScreen : MonoBehaviour
{
	// Token: 0x06000A42 RID: 2626 RVA: 0x00003022 File Offset: 0x00001222
	public void UnityEditorSetAnimationComplete()
	{
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0004415D File Offset: 0x0004235D
	public void SetAnimationComplete()
	{
		this.m_loadLevelSplashScreen.TriggerSceneActivation();
	}

	// Token: 0x04000AD5 RID: 2773
	public LoadLevelSplashScreen m_loadLevelSplashScreen;
}

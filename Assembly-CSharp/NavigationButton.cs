using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class NavigationButton : MonoBehaviour
{
	// Token: 0x0600089B RID: 2203 RVA: 0x0003BD17 File Offset: 0x00039F17
	public void SupressNextPress(bool bSupress)
	{
		this.m_bSupressNextPress = bSupress;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0003BD20 File Offset: 0x00039F20
	public virtual void PushScene()
	{
		if (this.m_bSupressNextPress)
		{
			this.m_bSupressNextPress = false;
			return;
		}
		if (ScreenManager.instance.CanTransition())
		{
			if (this.disconnectOnTransition)
			{
				Network.Disconnect();
			}
			ScreenManager.instance.PushScene(this.targetScene);
		}
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0003BD5B File Offset: 0x00039F5B
	public virtual void PopScene()
	{
		if (this.m_bSupressNextPress)
		{
			this.m_bSupressNextPress = false;
			return;
		}
		if (ScreenManager.instance.CanTransition())
		{
			if (this.disconnectOnTransition)
			{
				Network.Disconnect();
			}
			ScreenManager.instance.PopScene();
		}
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0003BD90 File Offset: 0x00039F90
	public virtual void GoToScene()
	{
		if (this.m_bSupressNextPress)
		{
			this.m_bSupressNextPress = false;
			return;
		}
		if (ScreenManager.instance.CanTransition())
		{
			if (this.disconnectOnTransition)
			{
				Network.Disconnect();
			}
			ScreenManager.instance.GoToScene(this.targetScene, true);
		}
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0003BDCC File Offset: 0x00039FCC
	public virtual void GoToSceneWithAnimation()
	{
		if (this.m_bSupressNextPress)
		{
			this.m_bSupressNextPress = false;
			return;
		}
		if (ScreenManager.instance.CanTransition())
		{
			if (this.disconnectOnTransition)
			{
				Network.Disconnect();
			}
			ScreenManager.instance.GoToScene(this.targetScene, false);
		}
	}

	// Token: 0x0400095E RID: 2398
	[Header("Name must EXACTLY match the screen to transition to")]
	public string targetScene;

	// Token: 0x0400095F RID: 2399
	public bool disconnectOnTransition;

	// Token: 0x04000960 RID: 2400
	private bool m_bSupressNextPress;
}

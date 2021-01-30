using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class NavigationButton_BackgroundScene : NavigationButton
{
	// Token: 0x060008A1 RID: 2209 RVA: 0x0003BE08 File Offset: 0x0003A008
	public override void PushScene()
	{
		base.PushScene();
		this.HandleAnimation(this.targetScene);
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0003BE1C File Offset: 0x0003A01C
	public override void PopScene()
	{
		base.PopScene();
		this.HandleAnimation(string.Empty);
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0003BE2F File Offset: 0x0003A02F
	public void PopSceneIfPossible()
	{
		if (ScreenManager.instance.GetSceneStackCount() > 1)
		{
			this.PopScene();
			return;
		}
		this.GoToSceneWithAnimation();
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0003BE4B File Offset: 0x0003A04B
	public override void GoToScene()
	{
		base.GoToScene();
		this.HandleAnimation(this.targetScene);
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0003BE5F File Offset: 0x0003A05F
	public override void GoToSceneWithAnimation()
	{
		base.GoToSceneWithAnimation();
		this.HandleAnimation(this.targetScene);
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0003BE74 File Offset: 0x0003A074
	private void HandleAnimation(string newScene)
	{
		if (newScene == string.Empty)
		{
			newScene = ScreenManager.instance.GetCurrentScreenName();
		}
		if (this.m_backgroundAnimator != null && newScene != string.Empty)
		{
			foreach (NavigationButton_BackgroundScene.NavSceneAnim navSceneAnim in this.m_navigationPossibilities)
			{
				if (navSceneAnim.sceneBeingTraveledTo == newScene)
				{
					this.m_backgroundAnimator.Play(navSceneAnim.animationToPlay);
					return;
				}
			}
			if (this.m_defaultAnimation != null)
			{
				this.m_backgroundAnimator.Play(this.m_defaultAnimation);
			}
		}
	}

	// Token: 0x04000961 RID: 2401
	public Animator m_backgroundAnimator;

	// Token: 0x04000962 RID: 2402
	public string m_defaultAnimation;

	// Token: 0x04000963 RID: 2403
	public NavigationButton_BackgroundScene.NavSceneAnim[] m_navigationPossibilities;

	// Token: 0x020007AE RID: 1966
	[Serializable]
	public struct NavSceneAnim
	{
		// Token: 0x04002C97 RID: 11415
		public string sceneBeingTraveledTo;

		// Token: 0x04002C98 RID: 11416
		public string animationToPlay;
	}
}

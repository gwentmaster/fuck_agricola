using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E5 RID: 229
[RequireComponent(typeof(Image))]
public class Avatar_UI : MonoBehaviour
{
	// Token: 0x0600087A RID: 2170 RVA: 0x0003AC81 File Offset: 0x00038E81
	private void Start()
	{
		this.Init();
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0003AC81 File Offset: 0x00038E81
	private void OnEnable()
	{
		this.Init();
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x0003AC89 File Offset: 0x00038E89
	private void OnDisable()
	{
		this.ReleaseAvatar();
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0003AC89 File Offset: 0x00038E89
	private void OnDestroy()
	{
		this.ReleaseAvatar();
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0003AC94 File Offset: 0x00038E94
	protected void Init()
	{
		if (this.m_image == null)
		{
			this.m_image = base.GetComponent<Image>();
		}
		if (this.m_manager == null)
		{
			this.m_manager = AvatarManager.instance;
		}
		if (!this.m_manager)
		{
			Debug.LogError("Unable to find avatar manager");
			base.enabled = false;
			return;
		}
		if (!this.m_image)
		{
			Debug.LogError("Unable to find image");
			base.enabled = false;
			return;
		}
		if (this.m_avatarIndex == -1)
		{
			this.SetAvatar(this.m_startingAvatarIndex, false);
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x0003AD28 File Offset: 0x00038F28
	public void SetAvatar(int index, bool setAsStarting = false)
	{
		if (this.m_image == null || this.m_manager == null)
		{
			this.Init();
		}
		if (!base.enabled || index == -1)
		{
			return;
		}
		index = this.m_manager.RangeCheck(index);
		if (index == this.m_avatarIndex)
		{
			return;
		}
		if (this.m_avatarIndex != -1)
		{
			this.m_manager.ReleaseAvatar((uint)this.m_avatarIndex);
		}
		this.m_avatarIndex = index;
		this.m_image.sprite = this.m_manager.GetAvatar((uint)index, this);
		if (setAsStarting)
		{
			this.m_startingAvatarIndex = index;
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0003ADBF File Offset: 0x00038FBF
	public void ReleaseAvatar()
	{
		if (!base.enabled)
		{
			return;
		}
		this.m_manager.ReleaseAvatar((uint)this.m_avatarIndex);
		this.m_startingAvatarIndex = this.m_avatarIndex;
		this.m_avatarIndex = -1;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0003ADEE File Offset: 0x00038FEE
	public void SetAvatarSpriteCallback(Sprite avatar)
	{
		this.m_image.sprite = avatar;
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x0003ADFC File Offset: 0x00038FFC
	public int GetIndex()
	{
		return this.m_avatarIndex;
	}

	// Token: 0x0400093A RID: 2362
	public int m_startingAvatarIndex;

	// Token: 0x0400093B RID: 2363
	protected Image m_image;

	// Token: 0x0400093C RID: 2364
	private int m_avatarIndex = -1;

	// Token: 0x0400093D RID: 2365
	protected AvatarManager m_manager;
}

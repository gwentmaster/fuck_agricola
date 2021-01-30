using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000025 RID: 37
public class AgricolaAnimal : MonoBehaviour
{
	// Token: 0x06000183 RID: 387 RVA: 0x000083C0 File Offset: 0x000065C0
	private void Awake()
	{
		if (!this.m_bInit)
		{
			this.m_bInit = true;
			this.m_dragable.AddOnBeginDragCallback(new DragObject.DragObjectCallback(this.BeginDragCallback));
			this.m_dragable.AddOnEndDragCallback(new DragObject.DragObjectCallback(this.EndDragCallback));
			this.m_dragable.AddOnEndAnimationCallback(new AnimateObject.AnimationCallback(this.EndAnimationCallback));
		}
		float z = (float)this.m_rand.Next(0, 360);
		base.transform.localRotation = Quaternion.Euler(-180f, 0f, z);
		this.m_childNode.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);
		this.m_adultNode.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);
		this.SetIsDraggable(false);
		this.SetIsChild(false);
	}

	// Token: 0x06000184 RID: 388 RVA: 0x000084A5 File Offset: 0x000066A5
	public void SetFarm(AgricolaFarm farm)
	{
		this.m_farm = farm;
	}

	// Token: 0x06000185 RID: 389 RVA: 0x000084AE File Offset: 0x000066AE
	public void SetContainerIndex(int index)
	{
		this.m_actualContainerIndex = index;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x000084B7 File Offset: 0x000066B7
	public int GetContainerIndex()
	{
		return this.m_actualContainerIndex;
	}

	// Token: 0x06000187 RID: 391 RVA: 0x000084C0 File Offset: 0x000066C0
	public void EndAnimationCallback(AnimateObject animateObject, AnimationLocator sourceLocator, AnimationLocator destinationLocator)
	{
		if (base.transform.localRotation.eulerAngles.x != -180f)
		{
			this.ResetRotationAndScale();
		}
	}

	// Token: 0x06000188 RID: 392 RVA: 0x000084F4 File Offset: 0x000066F4
	public void ResetRotationAndScale()
	{
		float z = (float)this.m_rand.Next(0, 360);
		base.transform.localRotation = Quaternion.Euler(-180f, 0f, z);
		this.m_childNode.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);
		this.m_adultNode.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);
		base.transform.localScale = Vector3.one;
		this.StartIdleAnim();
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000858D File Offset: 0x0000678D
	public EResourceType GetAnimalType()
	{
		return this.m_animalType;
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00008595 File Offset: 0x00006795
	public void SetIsDraggable(bool bDraggable)
	{
		if (this.m_dragable != null)
		{
			this.m_dragable.SetIsDraggable(bDraggable);
		}
	}

	// Token: 0x0600018B RID: 395 RVA: 0x000085B1 File Offset: 0x000067B1
	public void SetIsChild(bool bChild)
	{
		if (this.m_childNode != null)
		{
			this.m_childNode.SetActive(bChild);
		}
		if (this.m_adultNode != null)
		{
			this.m_adultNode.SetActive(!bChild);
		}
		this.StartIdleAnim();
	}

	// Token: 0x0600018C RID: 396 RVA: 0x000085F0 File Offset: 0x000067F0
	public bool GetIsChild()
	{
		return this.m_childNode != null && this.m_childNode.activeSelf;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000860D File Offset: 0x0000680D
	public void BeginDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.SetIsStruggle(true);
		this.m_farm.SetAnimalDragging(this, this.m_actualContainerIndex);
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00008628 File Offset: 0x00006828
	public void EndDragCallback(DragObject dragObject, PointerEventData eventData)
	{
		this.SetIsStruggle(false);
		this.m_farm.SetAnimalDragging(null, -2);
		AgricolaGame component = GameObject.FindGameObjectWithTag("GameController").GetComponent<AgricolaGame>();
		if (eventData != null && eventData.pointerEnter != null)
		{
			ushort dragSelectionID = dragObject.GetDragSelectionID();
			ushort dragSelectionHint = dragObject.GetDragSelectionHint();
			if (dragSelectionID != 0 && dragSelectionHint != 0)
			{
				bool flag;
				if (dragSelectionHint == 1)
				{
					flag = true;
					int tileIndex = (dragSelectionID == 999) ? -1 : ((int)(dragSelectionID - 1));
					this.m_farm.HandleAnimalDropOnContainer(this, tileIndex, -1);
				}
				else if (dragSelectionHint == 40992 && this.GetConvertAbilityCount((int)dragSelectionID) > 1)
				{
					int cardInstanceIDFromSubID = AgricolaLib.GetCardInstanceIDFromSubID((int)dragSelectionID, false, true);
					component.SetOptionPopupRestriction(cardInstanceIDFromSubID, 40992, this.m_animalType, -1);
					flag = true;
					this.m_farm.HandleAnimalDropOnContainer(this, -2, -1);
					component.UpdateGameOptionsSelectionState(true);
				}
				else
				{
					flag = GameOptions.SelectOptionByInstanceIDAndHint(dragSelectionID, dragSelectionHint);
					if (flag)
					{
						this.m_farm.HandleAnimalDropOnContainer(this, -2, -1);
					}
				}
				if (flag)
				{
					dragObject.ClearReturnToParent();
				}
			}
		}
		if (dragObject != null)
		{
			dragObject.SetDragSelectionHint(0);
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00008734 File Offset: 0x00006934
	public void SetIsStruggle(bool bStruggle)
	{
		(this.GetIsChild() ? this.m_childAnimator : this.m_adultAnimator).SetBool("Struggle", bStruggle);
		if (this.GetIsChild() && this.m_childNode_UI != null)
		{
			this.m_childNode_UI.SetActive(bStruggle);
			this.m_childNode.SetActive(!bStruggle);
			if (this.m_childAnimator_UI != null)
			{
				this.m_childAnimator_UI.SetBool("Struggle", bStruggle);
			}
		}
		else if (!this.GetIsChild() && this.m_adultNode_UI != null)
		{
			this.m_adultNode_UI.SetActive(bStruggle);
			this.m_adultNode.SetActive(!bStruggle);
			if (this.m_adultAnimator_UI != null)
			{
				this.m_adultAnimator_UI.SetBool("Struggle", bStruggle);
			}
		}
		if (!bStruggle)
		{
			this.StartIdleAnim();
		}
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00008810 File Offset: 0x00006A10
	public void StartIdleAnim()
	{
		base.StopAllCoroutines();
		Animator animator = this.GetIsChild() ? this.m_childAnimator : this.m_adultAnimator;
		animator.SetInteger("IdleChoice", 0);
		animator.SetBool("Struggle", false);
		animator.SetTrigger("Reset");
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.DelayIdle());
		}
	}

	// Token: 0x06000191 RID: 401 RVA: 0x00008878 File Offset: 0x00006A78
	private int GetConvertAbilityCount(int dragTargetSelectionID)
	{
		GCHandle gchandle = GCHandle.Alloc(new byte[256], GCHandleType.Pinned);
		IntPtr intPtr = gchandle.AddrOfPinnedObject();
		int cardInstanceIDFromSubID = AgricolaLib.GetCardInstanceIDFromSubID(dragTargetSelectionID, false, true);
		int num = 0;
		for (int i = 0; i < GameOptions.m_OptionCount; i++)
		{
			if (GameOptions.m_GameOption[i].isHidden == 0 && GameOptions.m_GameOption[i].selectionHint == 40992 && AgricolaLib.GetConvertDefinition((uint)GameOptions.m_GameOption[i].selectionID, intPtr, 256) != 0)
			{
				GameConvertDefinition gameConvertDefinition = (GameConvertDefinition)Marshal.PtrToStructure(intPtr, typeof(GameConvertDefinition));
				if (gameConvertDefinition.isHidden == 0 && gameConvertDefinition.resourceCostType == (int)this.m_animalType && AgricolaLib.GetCardInstanceIDFromSubID((int)GameOptions.m_GameOption[i].selectionID, false, true) == cardInstanceIDFromSubID)
				{
					num++;
				}
			}
		}
		gchandle.Free();
		return num;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00008965 File Offset: 0x00006B65
	private IEnumerator DelayIdle()
	{
		yield return new WaitForSeconds((float)(this.m_rand.Next(0, 5) + 1));
		(this.GetIsChild() ? this.m_childAnimator : this.m_adultAnimator).SetInteger("IdleChoice", this.m_rand.Next(1, 3));
		yield break;
	}

	// Token: 0x040000BD RID: 189
	[SerializeField]
	private EResourceType m_animalType;

	// Token: 0x040000BE RID: 190
	[SerializeField]
	private GameObject m_childNode;

	// Token: 0x040000BF RID: 191
	[SerializeField]
	private GameObject m_adultNode;

	// Token: 0x040000C0 RID: 192
	[SerializeField]
	private GameObject m_childNode_UI;

	// Token: 0x040000C1 RID: 193
	[SerializeField]
	private GameObject m_adultNode_UI;

	// Token: 0x040000C2 RID: 194
	[SerializeField]
	private Animator m_childAnimator;

	// Token: 0x040000C3 RID: 195
	[SerializeField]
	private Animator m_adultAnimator;

	// Token: 0x040000C4 RID: 196
	[SerializeField]
	private Animator m_childAnimator_UI;

	// Token: 0x040000C5 RID: 197
	[SerializeField]
	private Animator m_adultAnimator_UI;

	// Token: 0x040000C6 RID: 198
	[SerializeField]
	private DragObject m_dragable;

	// Token: 0x040000C7 RID: 199
	private AgricolaFarm m_farm;

	// Token: 0x040000C8 RID: 200
	private System.Random m_rand = new System.Random();

	// Token: 0x040000C9 RID: 201
	private bool m_bInit;

	// Token: 0x040000CA RID: 202
	private int m_actualContainerIndex = -2;

	// Token: 0x040000CB RID: 203
	private const int k_maxDataSize = 256;
}

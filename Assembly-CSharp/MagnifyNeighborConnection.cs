using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class MagnifyNeighborConnection : MonoBehaviour
{
	// Token: 0x06000534 RID: 1332 RVA: 0x0002850C File Offset: 0x0002670C
	public MagnifyNeighborConnection GetNeighborLeft()
	{
		return this.m_NeighborLeft;
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00028514 File Offset: 0x00026714
	public MagnifyNeighborConnection GetNeighborRight()
	{
		return this.m_NeighborRight;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0002851C File Offset: 0x0002671C
	public T FindNextComponentInLine<T>(MagnifyNeighborConnection.TravelDirection direction, bool bCheckNeighbors) where T : class
	{
		if (this.m_CardContainer != null)
		{
			if (this.m_ExpectedSiblingIndexForCard >= 0 && this.m_CardContainer.transform.childCount > this.m_ExpectedSiblingIndexForCard)
			{
				Transform child = this.m_CardContainer.transform.GetChild(this.m_ExpectedSiblingIndexForCard);
				if (child != null)
				{
					return child.GetComponent<T>();
				}
			}
			else if (this.m_ExpectedSiblingIndexForCard < 0)
			{
				T componentInChildren = this.m_CardContainer.transform.GetComponentInChildren<T>();
				if (componentInChildren != null)
				{
					return componentInChildren;
				}
			}
		}
		if (bCheckNeighbors)
		{
			if (this.m_NeighborLeft != null && direction == MagnifyNeighborConnection.TravelDirection.Left)
			{
				return this.m_NeighborLeft.FindNextComponentInLine<T>(MagnifyNeighborConnection.TravelDirection.Left, bCheckNeighbors);
			}
			if (this.m_NeighborRight != null && direction == MagnifyNeighborConnection.TravelDirection.Right)
			{
				return this.m_NeighborRight.FindNextComponentInLine<T>(MagnifyNeighborConnection.TravelDirection.Right, bCheckNeighbors);
			}
		}
		return default(!!0);
	}

	// Token: 0x040004E7 RID: 1255
	[SerializeField]
	private MagnifyNeighborConnection m_NeighborLeft;

	// Token: 0x040004E8 RID: 1256
	[SerializeField]
	private MagnifyNeighborConnection m_NeighborRight;

	// Token: 0x040004E9 RID: 1257
	[SerializeField]
	private GameObject m_CardContainer;

	// Token: 0x040004EA RID: 1258
	[Header("Set this to a negative number to search all childern for matching type")]
	[SerializeField]
	private int m_ExpectedSiblingIndexForCard;

	// Token: 0x0200077C RID: 1916
	public enum TravelDirection
	{
		// Token: 0x04002BE7 RID: 11239
		Left,
		// Token: 0x04002BE8 RID: 11240
		Right
	}
}

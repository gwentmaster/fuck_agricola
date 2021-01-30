using System;
using System.Runtime.InteropServices;
using GameEvent;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000050 RID: 80
public class AgricolaWorkerTray : MonoBehaviour
{
	// Token: 0x06000493 RID: 1171 RVA: 0x00023D80 File Offset: 0x00021F80
	private void Awake()
	{
		if (this.m_ActiveWorkerGlow != null)
		{
			this.m_ActiveWorkerGlow.SetActive(false);
		}
		bool isTutorialGame = AgricolaLib.GetIsTutorialGame();
		if (this.m_TutorialWorkerLocatorPanel != null)
		{
			this.m_TutorialWorkerLocatorPanel.SetActive(isTutorialGame);
		}
		this.m_ActiveWorkerLocator = (isTutorialGame ? this.m_TutorialWorkerLocator : this.m_StandardWorkerLocator);
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00023DDE File Offset: 0x00021FDE
	public void SetPlayerIndex(int playerIndex, int playerInstanceID)
	{
		this.m_PlayerIndex = playerIndex;
		this.m_PlayerInstanceID = playerInstanceID;
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00023DEE File Offset: 0x00021FEE
	public int GetFactionIndex()
	{
		return this.m_FactionIndex;
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00023DF6 File Offset: 0x00021FF6
	public void SetFactionIndex(int faction_index, Sprite factionSprite)
	{
		this.m_FactionIndex = faction_index;
		this.m_FactionSprite = factionSprite;
		if (this.m_FactionImage != null && this.m_FactionSprite != null)
		{
			this.m_FactionImage.sprite = this.m_FactionSprite;
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00023E33 File Offset: 0x00022033
	public void RegisterEventHandlers(GameEventBuffer game_event_buffer)
	{
		if (game_event_buffer == null)
		{
			return;
		}
		game_event_buffer.RegisterEventHandler(18, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventAssigningAgent));
		game_event_buffer.RegisterEventHandler(3, new UnityAction<IntPtr, GameEventFeedback>(this.HandleEventAnimationWorker));
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00023E60 File Offset: 0x00022060
	public void SetupAnimationManager(AgricolaAnimationManager animation_manager)
	{
		if (animation_manager != null)
		{
			animation_manager.AddOnBeginAnimationCallback(new AnimationManager.AnimationManagerCallback(this.OnBeginAnimation));
			animation_manager.AddOnEndAnimationCallback(new AnimationManager.AnimationManagerCallback(this.OnEndAnimation));
		}
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00023E8F File Offset: 0x0002208F
	public void RebuildAnimationManager(AgricolaAnimationManager animation_manager)
	{
		if (this.m_ActiveWorkerLocator != null && this.m_PlayerInstanceID != 0)
		{
			animation_manager.SetAnimationLocator(19, this.m_PlayerInstanceID, this.m_ActiveWorkerLocator);
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00023EBC File Offset: 0x000220BC
	private void OnBeginAnimation(AnimateObject animateObject, uint animationFlags, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID)
	{
		if (this.m_CurrentWorker != null)
		{
			AnimateObject component = this.m_CurrentWorker.GetComponent<AnimateObject>();
			if (animateObject == component)
			{
				this.UpdateGameOptionsSelectionState(true);
			}
		}
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00023EF4 File Offset: 0x000220F4
	private void OnEndAnimation(AnimateObject animateObject, uint animationFlags, int sourceLocatorIndex, int sourceLocatorInstanceID, int destinationLocatorIndex, int destinationLocatorInstanceID)
	{
		if (this.m_CurrentWorker != null)
		{
			AnimateObject component = this.m_CurrentWorker.GetComponent<AnimateObject>();
			if (animateObject == component && destinationLocatorIndex == 19 && destinationLocatorInstanceID == this.m_PlayerInstanceID && this.m_ActiveWorkerLocator != null && this.m_CurrentWorker.transform.parent != this.m_ActiveWorkerLocator.transform)
			{
				this.m_ActiveWorkerLocator.PlaceAnimateObject(component, true, true, true);
			}
		}
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00023F74 File Offset: 0x00022174
	private void HandleEventAssigningAgent(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		AssigningAgent assigningAgent = (AssigningAgent)Marshal.PtrToStructure(event_buffer, typeof(AssigningAgent));
		if (assigningAgent.assign_player_instance_id != this.m_PlayerInstanceID)
		{
			return;
		}
		if (this.m_CurrentWorker != null)
		{
			if (assigningAgent.assign_agent_instance_id == this.m_CurrentWorker.GetWorkerInstanceID())
			{
				return;
			}
			if (!this.m_CurrentWorker.IsTemporaryWorker() && this.m_CurrentWorker.transform.parent == this.m_ActiveWorkerLocator.transform)
			{
				this.m_WorkerManager.PlaceWorkerInWorkerLimbo(this.m_CurrentWorker);
			}
			this.ReleaseCurrentWorker();
		}
		GameObject workerFromInstanceID = this.m_WorkerManager.GetWorkerFromInstanceID(assigningAgent.assign_agent_instance_id, true);
		if (workerFromInstanceID != null)
		{
			this.m_CurrentWorker = workerFromInstanceID.GetComponent<AgricolaWorker>();
			if (this.m_CurrentWorker != null)
			{
				AnimateObject component = this.m_CurrentWorker.GetComponent<AnimateObject>();
				if (component != null)
				{
					bool flag = true;
					if (component.IsAnimating())
					{
						AnimationEntry currentAnimationEntry = this.m_AnimationManager.GetCurrentAnimationEntry(component);
						if (currentAnimationEntry != null && currentAnimationEntry.m_DestinationLocatorIndex == 9 && currentAnimationEntry.m_DestinationLocatorInstanceID == this.m_PlayerInstanceID)
						{
							flag = false;
						}
					}
					if (flag)
					{
						this.m_ActiveWorkerLocator.PlaceAnimateObject(component, true, true, true);
					}
				}
				this.m_CurrentWorker.gameObject.SetActive(true);
			}
			if (this.m_ActiveWorkerGlow != null)
			{
				this.m_ActiveWorkerGlow.SetActive(false);
			}
		}
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x000240D4 File Offset: 0x000222D4
	private void HandleEventAnimationWorker(IntPtr event_buffer, GameEventFeedback event_feedback)
	{
		OutputEventAnimationWorker outputEventAnimationWorker = (OutputEventAnimationWorker)Marshal.PtrToStructure(event_buffer, typeof(OutputEventAnimationWorker));
		if (this.m_CurrentWorker != null)
		{
			if (outputEventAnimationWorker.animation_destination_location == 19 && outputEventAnimationWorker.animation_destination_instance_id == this.m_PlayerInstanceID)
			{
				this.ReleaseCurrentWorker();
				return;
			}
			if (outputEventAnimationWorker.worker_instance_id == this.m_CurrentWorker.GetWorkerInstanceID() && outputEventAnimationWorker.animation_source_location == 16 && outputEventAnimationWorker.animation_source_instance_id == this.m_PlayerInstanceID)
			{
				this.ReleaseCurrentWorker();
				if (this.m_ActiveWorkerGlow != null)
				{
					this.m_ActiveWorkerGlow.SetActive(false);
				}
			}
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00024170 File Offset: 0x00022370
	private void FindNextAvailableWorker()
	{
		if (this.m_WorkerManager != null && this.m_PlayerIndex != 0)
		{
			this.m_CurrentWorker = this.m_WorkerManager.GetUnassignedWorker(this.m_PlayerIndex);
			if (this.m_CurrentWorker != null)
			{
				AnimateObject component = this.m_CurrentWorker.GetComponent<AnimateObject>();
				if (component != null)
				{
					bool flag = true;
					if (component.IsAnimating())
					{
						AnimationEntry currentAnimationEntry = this.m_AnimationManager.GetCurrentAnimationEntry(component);
						if (currentAnimationEntry != null && currentAnimationEntry.m_DestinationLocatorIndex == 19 && currentAnimationEntry.m_DestinationLocatorInstanceID == this.m_PlayerInstanceID)
						{
							flag = false;
						}
					}
					if (flag)
					{
						this.m_ActiveWorkerLocator.PlaceAnimateObject(component, true, true, true);
					}
				}
				this.m_CurrentWorker.gameObject.SetActive(true);
			}
			if (this.m_ActiveWorkerGlow != null)
			{
				this.m_ActiveWorkerGlow.SetActive(false);
			}
		}
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00024244 File Offset: 0x00022444
	public void Update()
	{
		if (this.m_CurrentFarmAction != null)
		{
			if (GameOptions.IsSelectableHint(this.m_CurrentFarmAction.GetFarmActionHint()))
			{
				if (!this.m_CurrentFarmAction.gameObject.activeInHierarchy)
				{
					if (this.m_ActiveWorkerLocator != null)
					{
						AnimateObject component = this.m_CurrentFarmAction.GetComponent<AnimateObject>();
						if (component != null)
						{
							this.m_ActiveWorkerLocator.PlaceAnimateObject(component, true, true, true);
						}
					}
					this.m_CurrentFarmAction.gameObject.SetActive(true);
					this.m_CurrentFarmAction.SetSelectable(true, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				}
			}
			else
			{
				this.ReleaseCurrentWorker();
			}
		}
		if (this.m_CurrentWorker == null && this.m_CurrentFarmAction == null)
		{
			this.FindNextAvailableWorker();
		}
		if (this.m_CurrentWorker != null && !this.m_CurrentWorker.gameObject.activeInHierarchy && this.m_ActiveWorkerLocator != null)
		{
			AnimateObject component2 = this.m_CurrentWorker.GetComponent<AnimateObject>();
			if (component2 != null)
			{
				this.m_ActiveWorkerLocator.PlaceAnimateObject(component2, true, true, true);
			}
		}
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00024374 File Offset: 0x00022574
	public void UpdateGameOptionsSelectionState(bool bHighlight)
	{
		if (this.m_WorkerManager != null)
		{
			if (GameOptions.IsSelectableHint(40977))
			{
				if (this.m_CurrentWorker != null && !this.m_CurrentWorker.IsTemporaryWorker())
				{
					if (this.m_CurrentWorker.transform.parent == this.m_ActiveWorkerLocator.transform)
					{
						this.m_WorkerManager.PlaceWorkerInWorkerLimbo(this.m_CurrentWorker);
					}
					this.ReleaseCurrentWorker();
				}
				if (this.m_CurrentFarmAction != null && this.m_CurrentFarmAction.GetFarmActionHint() != 40977)
				{
					this.ReleaseCurrentWorker();
				}
				if (this.m_CurrentFarmAction == null)
				{
					this.m_CurrentFarmAction = this.m_WorkerManager.CreateTemporaryFarmAction(this.m_PlayerInstanceID, this.m_FactionIndex, 40977);
					this.m_CurrentFarmAction.SetAvatar(67);
				}
				AnimateObject component = this.m_CurrentFarmAction.GetComponent<AnimateObject>();
				if (component != null)
				{
					this.m_ActiveWorkerLocator.PlaceAnimateObject(component, true, true, true);
				}
				this.m_CurrentFarmAction.gameObject.SetActive(true);
				this.m_CurrentFarmAction.SetSelectable(true, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				if (this.m_ActiveWorkerGlow != null)
				{
					this.m_ActiveWorkerGlow.SetActive(true);
				}
			}
			if (GameOptions.IsSelectableHint(40984))
			{
				if (this.m_CurrentWorker != null && !this.m_CurrentWorker.IsTemporaryWorker())
				{
					if (this.m_CurrentWorker.transform.parent == this.m_ActiveWorkerLocator.transform)
					{
						this.m_WorkerManager.PlaceWorkerInWorkerLimbo(this.m_CurrentWorker);
					}
					this.ReleaseCurrentWorker();
				}
				if (this.m_CurrentFarmAction != null && this.m_CurrentFarmAction.GetFarmActionHint() != 40984)
				{
					this.ReleaseCurrentWorker();
				}
				if (this.m_CurrentFarmAction == null)
				{
					this.m_CurrentFarmAction = this.m_WorkerManager.CreateTemporaryFarmAction(this.m_PlayerInstanceID, this.m_FactionIndex, 40984);
					this.m_CurrentFarmAction.SetAvatar(69);
				}
				AnimateObject component2 = this.m_CurrentFarmAction.GetComponent<AnimateObject>();
				if (component2 != null)
				{
					this.m_ActiveWorkerLocator.PlaceAnimateObject(component2, true, true, true);
				}
				this.m_CurrentFarmAction.gameObject.SetActive(true);
				this.m_CurrentFarmAction.SetSelectable(true, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				if (this.m_ActiveWorkerGlow != null)
				{
					this.m_ActiveWorkerGlow.SetActive(true);
				}
			}
			if (GameOptions.IsSelectableHint(40976))
			{
				if (this.m_CurrentWorker != null && !this.m_CurrentWorker.IsTemporaryWorker())
				{
					if (this.m_CurrentWorker.transform.parent == this.m_ActiveWorkerLocator.transform)
					{
						this.m_WorkerManager.PlaceWorkerInWorkerLimbo(this.m_CurrentWorker);
					}
					this.ReleaseCurrentWorker();
				}
				if (this.m_CurrentFarmAction != null && this.m_CurrentFarmAction.GetFarmActionHint() != 40976)
				{
					this.ReleaseCurrentWorker();
				}
				if (this.m_CurrentFarmAction == null)
				{
					this.m_CurrentFarmAction = this.m_WorkerManager.CreateTemporaryFarmAction(this.m_PlayerInstanceID, this.m_FactionIndex, 40976);
					this.m_CurrentFarmAction.SetAvatar(65);
				}
				AnimateObject component3 = this.m_CurrentFarmAction.GetComponent<AnimateObject>();
				if (component3 != null)
				{
					this.m_ActiveWorkerLocator.PlaceAnimateObject(component3, true, true, true);
				}
				this.m_CurrentFarmAction.gameObject.SetActive(true);
				this.m_CurrentFarmAction.SetSelectable(true, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				if (this.m_ActiveWorkerGlow != null)
				{
					this.m_ActiveWorkerGlow.SetActive(true);
				}
			}
			if (GameOptions.IsSelectableHint(40980))
			{
				if (this.m_CurrentWorker != null && !this.m_CurrentWorker.IsTemporaryWorker())
				{
					if (this.m_CurrentWorker.transform.parent == this.m_ActiveWorkerLocator.transform)
					{
						this.m_WorkerManager.PlaceWorkerInWorkerLimbo(this.m_CurrentWorker);
					}
					this.ReleaseCurrentWorker();
				}
				if (this.m_CurrentFarmAction != null && this.m_CurrentFarmAction.GetFarmActionHint() != 40980)
				{
					this.ReleaseCurrentWorker();
				}
				if (this.m_CurrentFarmAction == null)
				{
					this.m_CurrentFarmAction = this.m_WorkerManager.CreateTemporaryFarmAction(this.m_PlayerInstanceID, this.m_FactionIndex, 40980);
					this.m_CurrentFarmAction.SetAvatar(68);
				}
				AnimateObject component4 = this.m_CurrentFarmAction.GetComponent<AnimateObject>();
				if (component4 != null)
				{
					this.m_ActiveWorkerLocator.PlaceAnimateObject(component4, true, true, true);
				}
				this.m_CurrentFarmAction.gameObject.SetActive(true);
				this.m_CurrentFarmAction.SetSelectable(true, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
				if (this.m_ActiveWorkerGlow != null)
				{
					this.m_ActiveWorkerGlow.SetActive(true);
				}
			}
		}
		if (this.m_CurrentWorker == null && this.m_CurrentFarmAction == null)
		{
			this.FindNextAvailableWorker();
		}
		if (this.m_CurrentWorker != null && GameOptions.IsSelectableHint(40962) && this.m_CurrentWorker != null)
		{
			this.m_CurrentWorker.SetSelectable(true, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			if (this.m_ActiveWorkerGlow != null)
			{
				this.m_ActiveWorkerGlow.SetActive(true);
			}
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00024920 File Offset: 0x00022B20
	public void RebuildInterface()
	{
		this.ReleaseCurrentWorker();
		int assignAgentInstanceID = AgricolaLib.GetAssignAgentInstanceID();
		if (assignAgentInstanceID != 0)
		{
			GameObject workerFromInstanceID = this.m_WorkerManager.GetWorkerFromInstanceID(assignAgentInstanceID, true);
			if (workerFromInstanceID != null)
			{
				this.m_CurrentWorker = workerFromInstanceID.GetComponent<AgricolaWorker>();
				if (this.m_CurrentWorker != null)
				{
					AnimateObject component = this.m_CurrentWorker.GetComponent<AnimateObject>();
					if (component != null)
					{
						this.m_ActiveWorkerLocator.PlaceAnimateObject(component, true, true, true);
					}
					this.m_CurrentWorker.gameObject.SetActive(true);
				}
			}
		}
		if (this.m_ActiveWorkerGlow != null)
		{
			this.m_ActiveWorkerGlow.SetActive(false);
		}
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x000249BC File Offset: 0x00022BBC
	public void ReleaseCurrentWorker()
	{
		if (this.m_CurrentWorker != null && this.m_CurrentWorker.IsTemporaryWorker() && this.m_WorkerManager != null)
		{
			this.m_WorkerManager.ReturnTemporaryWorker(this.m_CurrentWorker);
		}
		this.m_CurrentWorker = null;
		if (this.m_CurrentFarmAction != null)
		{
			this.m_WorkerManager.ReturnTemporaryFarmAction(this.m_CurrentFarmAction);
		}
		this.m_CurrentFarmAction = null;
	}

	// Token: 0x04000415 RID: 1045
	private const int k_maxDataSize = 1024;

	// Token: 0x04000416 RID: 1046
	[SerializeField]
	private AgricolaAnimationLocator m_StandardWorkerLocator;

	// Token: 0x04000417 RID: 1047
	[SerializeField]
	private AgricolaAnimationLocator m_TutorialWorkerLocator;

	// Token: 0x04000418 RID: 1048
	[SerializeField]
	private GameObject m_TutorialWorkerLocatorPanel;

	// Token: 0x04000419 RID: 1049
	[SerializeField]
	private AgricolaAnimationLocator m_ActiveResourceLocator;

	// Token: 0x0400041A RID: 1050
	[SerializeField]
	private AgricolaWorkerManager m_WorkerManager;

	// Token: 0x0400041B RID: 1051
	[SerializeField]
	private AgricolaAnimationManager m_AnimationManager;

	// Token: 0x0400041C RID: 1052
	[SerializeField]
	protected DragManager m_DragManager;

	// Token: 0x0400041D RID: 1053
	[SerializeField]
	private Image m_FactionImage;

	// Token: 0x0400041E RID: 1054
	[SerializeField]
	private GameObject m_ActiveWorkerGlow;

	// Token: 0x0400041F RID: 1055
	private int m_PlayerIndex;

	// Token: 0x04000420 RID: 1056
	private int m_PlayerInstanceID;

	// Token: 0x04000421 RID: 1057
	private int m_FactionIndex;

	// Token: 0x04000422 RID: 1058
	private Sprite m_FactionSprite;

	// Token: 0x04000423 RID: 1059
	private AgricolaAnimationLocator m_ActiveWorkerLocator;

	// Token: 0x04000424 RID: 1060
	private AgricolaWorker m_CurrentWorker;

	// Token: 0x04000425 RID: 1061
	private AgricolaFarmAction m_CurrentFarmAction;
}

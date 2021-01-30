using System;

namespace GameEvent
{
	// Token: 0x0200021F RID: 543
	public enum EventType
	{
		// Token: 0x040011DB RID: 4571
		OutputEventPause = 1,
		// Token: 0x040011DC RID: 4572
		OutputEventAnimationCard,
		// Token: 0x040011DD RID: 4573
		OutputEventAnimationWorker,
		// Token: 0x040011DE RID: 4574
		OutputEventAnimationResource,
		// Token: 0x040011DF RID: 4575
		UpdateDisplay,
		// Token: 0x040011E0 RID: 4576
		UpdatePlayerHand,
		// Token: 0x040011E1 RID: 4577
		ActionSpaceStatus,
		// Token: 0x040011E2 RID: 4578
		CardInPlayStatus,
		// Token: 0x040011E3 RID: 4579
		BuildingAuxiliaryResources,
		// Token: 0x040011E4 RID: 4580
		UpdateWaterdeepHarbor,
		// Token: 0x040011E5 RID: 4581
		UpdateActiveQuestList,
		// Token: 0x040011E6 RID: 4582
		QuestCompleted,
		// Token: 0x040011E7 RID: 4583
		PlotQuestEffect,
		// Token: 0x040011E8 RID: 4584
		RevealedCardList,
		// Token: 0x040011E9 RID: 4585
		PlayerChoiceStatus,
		// Token: 0x040011EA RID: 4586
		CardLocation,
		// Token: 0x040011EB RID: 4587
		MagnifyLordCard,
		// Token: 0x040011EC RID: 4588
		AssigningAgent,
		// Token: 0x040011ED RID: 4589
		TutorialAISelectedOption,
		// Token: 0x040011EE RID: 4590
		PlayerSelectedOption,
		// Token: 0x040011EF RID: 4591
		ActionRound,
		// Token: 0x040011F0 RID: 4592
		PhasingPlayer,
		// Token: 0x040011F1 RID: 4593
		TurnNumber,
		// Token: 0x040011F2 RID: 4594
		HarvestPhase,
		// Token: 0x040011F3 RID: 4595
		DraftMode,
		// Token: 0x040011F4 RID: 4596
		OptionPopupConvertRestriction,
		// Token: 0x040011F5 RID: 4597
		DirtyFarmTile,
		// Token: 0x040011F6 RID: 4598
		GainedAnimals,
		// Token: 0x040011F7 RID: 4599
		MajorImpOwnerChanged,
		// Token: 0x040011F8 RID: 4600
		ChinaCard,
		// Token: 0x040011F9 RID: 4601
		GameOver,
		// Token: 0x040011FA RID: 4602
		AssignSides,
		// Token: 0x040011FB RID: 4603
		DiscardsReshuffled,
		// Token: 0x040011FC RID: 4604
		CardsAdded,
		// Token: 0x040011FD RID: 4605
		RegionScoring,
		// Token: 0x040011FE RID: 4606
		CardPlayed,
		// Token: 0x040011FF RID: 4607
		EventPlayed,
		// Token: 0x04001200 RID: 4608
		Coup,
		// Token: 0x04001201 RID: 4609
		Realignment,
		// Token: 0x04001202 RID: 4610
		PushResolveCard,
		// Token: 0x04001203 RID: 4611
		PopResolveCard,
		// Token: 0x04001204 RID: 4612
		PushRevealCard,
		// Token: 0x04001205 RID: 4613
		PopRevealCard,
		// Token: 0x04001206 RID: 4614
		SetRevealCardPlayer,
		// Token: 0x04001207 RID: 4615
		SetHeadlineCardRevealed,
		// Token: 0x04001208 RID: 4616
		LoadProgress,
		// Token: 0x04001209 RID: 4617
		CommitPlayerDecision,
		// Token: 0x0400120A RID: 4618
		Achievement,
		// Token: 0x0400120B RID: 4619
		UseCardSound,
		// Token: 0x0400120C RID: 4620
		UseConvertAbility,
		// Token: 0x0400120D RID: 4621
		TookBeggingCards,
		// Token: 0x0400120E RID: 4622
		TrapRoll,
		// Token: 0x0400120F RID: 4623
		ScoringCardPlayed,
		// Token: 0x04001210 RID: 4624
		FinalScoring,
		// Token: 0x04001211 RID: 4625
		EffectRoll,
		// Token: 0x04001212 RID: 4626
		EndTurn,
		// Token: 0x04001213 RID: 4627
		HeadlineAnnounce,
		// Token: 0x04001214 RID: 4628
		Reshuffle,
		// Token: 0x04001215 RID: 4629
		PauseForRevealedCards,
		// Token: 0x04001216 RID: 4630
		BiddingResults,
		// Token: 0x04001217 RID: 4631
		TurnZero,
		// Token: 0x04001218 RID: 4632
		TurnZeroCrisisCard,
		// Token: 0x04001219 RID: 4633
		SetStatecraftCardRevealed,
		// Token: 0x0400121A RID: 4634
		CrisisCardRoll,
		// Token: 0x0400121B RID: 4635
		OutputMessage,
		// Token: 0x0400121C RID: 4636
		LogUpdated = 99,
		// Token: 0x0400121D RID: 4637
		LogBeginTurn,
		// Token: 0x0400121E RID: 4638
		LogEndTurn,
		// Token: 0x0400121F RID: 4639
		LogBeginActionRound,
		// Token: 0x04001220 RID: 4640
		LogEndActionRound,
		// Token: 0x04001221 RID: 4641
		LogBeginCardEvent,
		// Token: 0x04001222 RID: 4642
		LogEndCardEvent,
		// Token: 0x04001223 RID: 4643
		LogBeginOps,
		// Token: 0x04001224 RID: 4644
		LogEndOps,
		// Token: 0x04001225 RID: 4645
		LogInfluenceChange,
		// Token: 0x04001226 RID: 4646
		LogRevealHeadline,
		// Token: 0x04001227 RID: 4647
		LogCoupResult,
		// Token: 0x04001228 RID: 4648
		LogRealignmentResult,
		// Token: 0x04001229 RID: 4649
		LogWarResult,
		// Token: 0x0400122A RID: 4650
		LogMilitaryOps,
		// Token: 0x0400122B RID: 4651
		LogDefconLevel,
		// Token: 0x0400122C RID: 4652
		LogVPTrack,
		// Token: 0x0400122D RID: 4653
		LogSpaceRaceResult,
		// Token: 0x0400122E RID: 4654
		LogSpaceRaceAdvance,
		// Token: 0x0400122F RID: 4655
		LogDiscard,
		// Token: 0x04001230 RID: 4656
		LogCardInPlayStatus,
		// Token: 0x04001231 RID: 4657
		LogCancelAction,
		// Token: 0x04001232 RID: 4658
		LogReportSides,
		// Token: 0x04001233 RID: 4659
		LogRevealCard,
		// Token: 0x04001234 RID: 4660
		LogPlayAdditionalCard,
		// Token: 0x04001235 RID: 4661
		LogTrapResult,
		// Token: 0x04001236 RID: 4662
		LogBiddingResult,
		// Token: 0x04001237 RID: 4663
		LogGrainSalesResult,
		// Token: 0x04001238 RID: 4664
		LogChernobyl,
		// Token: 0x04001239 RID: 4665
		LogOlympics
	}
}

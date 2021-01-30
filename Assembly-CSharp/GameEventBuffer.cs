using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Events;

// Token: 0x020000D5 RID: 213
public class GameEventBuffer
{
	// Token: 0x060007E4 RID: 2020 RVA: 0x00038380 File Offset: 0x00036580
	public int GetEventsRemainingToProcess()
	{
		return this.m_EventsRemainingToProcess;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00038388 File Offset: 0x00036588
	public void Init()
	{
		this.m_EventBuffer = new byte[4096];
		this.m_hGameEvents = GCHandle.Alloc(this.m_EventBuffer, GCHandleType.Pinned);
		this.m_EventBufferPtr = this.m_hGameEvents.AddrOfPinnedObject();
		this.m_EventFeedback = new GameEventFeedback();
		if (this.m_EventDictionary == null)
		{
			this.m_EventDictionary = new Dictionary<int, GameEventIntPtr>();
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x000383E6 File Offset: 0x000365E6
	public void SetUpdateDelegate(GameEventBuffer.GameEventBufferUpdateDelegate update_delegate)
	{
		this.m_UpdateDelegate = update_delegate;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x000383F0 File Offset: 0x000365F0
	public void RegisterEventHandler(int eventID, UnityAction<IntPtr, GameEventFeedback> listener)
	{
		if (this.m_EventDictionary == null)
		{
			return;
		}
		GameEventIntPtr gameEventIntPtr = null;
		if (this.m_EventDictionary.TryGetValue(eventID, out gameEventIntPtr))
		{
			gameEventIntPtr.AddListener(listener);
			return;
		}
		gameEventIntPtr = new GameEventIntPtr();
		gameEventIntPtr.AddListener(listener);
		this.m_EventDictionary.Add(eventID, gameEventIntPtr);
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0003843A File Offset: 0x0003663A
	public void Reset()
	{
		this.m_EventsRemainingToProcess = 0;
		this.m_EventBufferPtr = this.m_hGameEvents.AddrOfPinnedObject();
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00038454 File Offset: 0x00036654
	public void Update()
	{
		if (this.m_EventsRemainingToProcess == 0)
		{
			this.m_EventBufferPtr = this.m_hGameEvents.AddrOfPinnedObject();
			this.m_EventsRemainingToProcess = this.m_UpdateDelegate(this.m_EventBufferPtr, 4096);
		}
		this.m_bBreakFromEventLoop = false;
		while (this.m_EventsRemainingToProcess > 0 && !this.m_bBreakFromEventLoop)
		{
			this.m_EventBufferPtr = this.HandleEvent(this.m_EventBufferPtr);
			this.m_EventsRemainingToProcess--;
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x000384D0 File Offset: 0x000366D0
	private IntPtr HandleEvent(IntPtr eventBuffer)
	{
		IntPtr intPtr = eventBuffer;
		int num = Marshal.ReadIntPtr(intPtr).ToInt32();
		if (num <= Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)))
		{
			return eventBuffer;
		}
		intPtr = new IntPtr(intPtr.ToInt64() + (long)Marshal.SizeOf(typeof(int)));
		IntPtr result = new IntPtr(eventBuffer.ToInt64() + (long)num);
		int key = Marshal.ReadIntPtr(intPtr).ToInt32();
		intPtr = new IntPtr(intPtr.ToInt64() + (long)Marshal.SizeOf(typeof(int)));
		GameEventIntPtr gameEventIntPtr = null;
		if (this.m_EventDictionary.TryGetValue(key, out gameEventIntPtr))
		{
			this.m_EventFeedback.Reset();
			gameEventIntPtr.Invoke(intPtr, this.m_EventFeedback);
			this.m_bBreakFromEventLoop |= this.m_EventFeedback.bBreakFromUpdateLoop;
		}
		return result;
	}

	// Token: 0x040008E1 RID: 2273
	private const int m_EventBufferLength = 4096;

	// Token: 0x040008E2 RID: 2274
	private byte[] m_EventBuffer;

	// Token: 0x040008E3 RID: 2275
	private GCHandle m_hGameEvents;

	// Token: 0x040008E4 RID: 2276
	private int m_EventsRemainingToProcess;

	// Token: 0x040008E5 RID: 2277
	private bool m_bBreakFromEventLoop;

	// Token: 0x040008E6 RID: 2278
	private IntPtr m_EventBufferPtr = IntPtr.Zero;

	// Token: 0x040008E7 RID: 2279
	private GameEventFeedback m_EventFeedback;

	// Token: 0x040008E8 RID: 2280
	private GameEventBuffer.GameEventBufferUpdateDelegate m_UpdateDelegate;

	// Token: 0x040008E9 RID: 2281
	private Dictionary<int, GameEventIntPtr> m_EventDictionary;

	// Token: 0x0200079E RID: 1950
	// (Invoke) Token: 0x060042A3 RID: 17059
	public delegate int GameEventBufferUpdateDelegate(IntPtr pGameEvents, int maxEvents);
}

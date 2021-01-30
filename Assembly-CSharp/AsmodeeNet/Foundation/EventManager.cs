using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x020006F1 RID: 1777
	public class EventManager : MonoBehaviour, IEventManager
	{
		// Token: 0x06003ECD RID: 16077 RVA: 0x00132DC2 File Offset: 0x00130FC2
		private void OnEnable()
		{
			if (EventManager.Instance == null)
			{
				EventManager.Instance = this;
			}
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x00132DD4 File Offset: 0x00130FD4
		public void QueueEvent(Action action)
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				this._queuedEvents.Add(new AsmodeeNet.Utils.Tuple<Delegate, object>(action, null));
			}
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x00132E20 File Offset: 0x00131020
		public void QueueEvent<T>(Action<T> action, T parameter)
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				this._queuedEvents.Add(new AsmodeeNet.Utils.Tuple<Delegate, object>(action, parameter));
			}
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x00132E74 File Offset: 0x00131074
		public void QueueEvent<T1, T2>(Action<T1, T2> action, T1 parameter1, T2 parameter2)
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				this._queuedEvents.Add(new AsmodeeNet.Utils.Tuple<Delegate, object>(action, new object[]
				{
					parameter1,
					parameter2
				}));
			}
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x00132ED8 File Offset: 0x001310D8
		private void Update()
		{
			this.MoveQueuedEventsToExecuting();
			while (this._executingEvents.Count > 0)
			{
				AsmodeeNet.Utils.Tuple<Delegate, object> tuple = this._executingEvents[0];
				this._executingEvents.RemoveAt(0);
				try
				{
					if (tuple.Item2 != null && tuple.Item2 is Array)
					{
						Array array = tuple.Item2 as Array;
						if (array.Length == 2)
						{
							tuple.Item1.DynamicInvoke(new object[]
							{
								array.GetValue(0),
								array.GetValue(1)
							});
						}
						else if (array.Length == 3)
						{
							tuple.Item1.DynamicInvoke(new object[]
							{
								array.GetValue(0),
								array.GetValue(1),
								array.GetValue(2)
							});
						}
					}
					else if (tuple.Item2 != null)
					{
						tuple.Item1.DynamicInvoke(new object[]
						{
							tuple.Item2
						});
					}
					else
					{
						tuple.Item1.DynamicInvoke(Array.Empty<object>());
					}
				}
				catch
				{
					Hashtable extraInfo = Reflection.HashtableFromObject(tuple.Item1.Method, null, 30U);
					AsmoLogger.Error("EventManager", "There was an error when calling the function " + tuple.Item1.Method.Name, extraInfo);
					throw;
				}
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x00133030 File Offset: 0x00131230
		private void MoveQueuedEventsToExecuting()
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				while (this._queuedEvents.Count > 0)
				{
					AsmodeeNet.Utils.Tuple<Delegate, object> item = this._queuedEvents[0];
					this._executingEvents.Add(item);
					this._queuedEvents.RemoveAt(0);
				}
			}
		}

		// Token: 0x0400286E RID: 10350
		private const string _debugModuleName = "EventManager";

		// Token: 0x0400286F RID: 10351
		public static IEventManager Instance;

		// Token: 0x04002870 RID: 10352
		private object _queueLock = new object();

		// Token: 0x04002871 RID: 10353
		private List<AsmodeeNet.Utils.Tuple<Delegate, object>> _queuedEvents = new List<AsmodeeNet.Utils.Tuple<Delegate, object>>();

		// Token: 0x04002872 RID: 10354
		private List<AsmodeeNet.Utils.Tuple<Delegate, object>> _executingEvents = new List<AsmodeeNet.Utils.Tuple<Delegate, object>>();
	}
}

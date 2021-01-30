using System;
using System.Collections.Generic;
using BestHTTP.Examples;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

// Token: 0x02000010 RID: 16
internal class BaseHub : Hub
{
	// Token: 0x060000AA RID: 170 RVA: 0x00004494 File Offset: 0x00002694
	public BaseHub(string name, string title) : base(name)
	{
		this.Title = title;
		base.On("joined", new OnMethodCallCallbackDelegate(this.Joined));
		base.On("rejoined", new OnMethodCallCallbackDelegate(this.Rejoined));
		base.On("left", new OnMethodCallCallbackDelegate(this.Left));
		base.On("invoked", new OnMethodCallCallbackDelegate(this.Invoked));
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00004518 File Offset: 0x00002718
	private void Joined(Hub hub, MethodCallMessage methodCall)
	{
		Dictionary<string, object> dictionary = methodCall.Arguments[2] as Dictionary<string, object>;
		this.messages.Add(string.Format("{0} joined at {1}\n\tIsAuthenticated: {2} IsAdmin: {3} UserName: {4}", new object[]
		{
			methodCall.Arguments[0],
			methodCall.Arguments[1],
			dictionary["IsAuthenticated"],
			dictionary["IsAdmin"],
			dictionary["UserName"]
		}));
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000458E File Offset: 0x0000278E
	private void Rejoined(Hub hub, MethodCallMessage methodCall)
	{
		this.messages.Add(string.Format("{0} reconnected at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000045B5 File Offset: 0x000027B5
	private void Left(Hub hub, MethodCallMessage methodCall)
	{
		this.messages.Add(string.Format("{0} left at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
	}

	// Token: 0x060000AE RID: 174 RVA: 0x000045DC File Offset: 0x000027DC
	private void Invoked(Hub hub, MethodCallMessage methodCall)
	{
		this.messages.Add(string.Format("{0} invoked hub method at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00004603 File Offset: 0x00002803
	public void InvokedFromClient()
	{
		base.Call("invokedFromClient", new OnMethodResultDelegate(this.OnInvoked), new OnMethodFailedDelegate(this.OnInvokeFailed), Array.Empty<object>());
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000462E File Offset: 0x0000282E
	private void OnInvoked(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		Debug.Log(hub.Name + " invokedFromClient success!");
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00004645 File Offset: 0x00002845
	private void OnInvokeFailed(Hub hub, ClientMessage originalMessage, FailureMessage result)
	{
		Debug.LogWarning(hub.Name + " " + result.ErrorMessage);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00004664 File Offset: 0x00002864
	public void Draw()
	{
		GUILayout.Label(this.Title, Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		this.messages.Draw((float)(Screen.width - 20), 100f);
		GUILayout.EndHorizontal();
	}

	// Token: 0x0400004F RID: 79
	private string Title;

	// Token: 0x04000050 RID: 80
	private GUIMessageList messages = new GUIMessageList();
}

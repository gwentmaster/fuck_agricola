using System;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

// Token: 0x02000014 RID: 20
internal class TypedDemoHub : Hub
{
	// Token: 0x060000D2 RID: 210 RVA: 0x0000519D File Offset: 0x0000339D
	public TypedDemoHub() : base("typeddemohub")
	{
		base.On("Echo", new OnMethodCallCallbackDelegate(this.Echo));
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x000051D7 File Offset: 0x000033D7
	private void Echo(Hub hub, MethodCallMessage methodCall)
	{
		this.typedEchoClientResult = string.Format("{0} #{1} triggered!", methodCall.Arguments[0], methodCall.Arguments[1]);
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000051F9 File Offset: 0x000033F9
	public void Echo(string msg)
	{
		base.Call("echo", new OnMethodResultDelegate(this.OnEcho_Done), new object[]
		{
			msg
		});
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000521D File Offset: 0x0000341D
	private void OnEcho_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.typedEchoResult = "TypedDemoHub.Echo(string message) invoked!";
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0000522C File Offset: 0x0000342C
	public void Draw()
	{
		GUILayout.Label("Typed callback", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(this.typedEchoResult, Array.Empty<GUILayoutOption>());
		GUILayout.Label(this.typedEchoClientResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
	}

	// Token: 0x04000062 RID: 98
	private string typedEchoResult = string.Empty;

	// Token: 0x04000063 RID: 99
	private string typedEchoClientResult = string.Empty;
}

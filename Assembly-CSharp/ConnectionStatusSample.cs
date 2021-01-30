using System;
using BestHTTP.Examples;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using UnityEngine;

// Token: 0x02000012 RID: 18
internal sealed class ConnectionStatusSample : MonoBehaviour
{
	// Token: 0x060000C2 RID: 194 RVA: 0x00004B9C File Offset: 0x00002D9C
	private void Start()
	{
		this.signalRConnection = new Connection(this.URI, new string[]
		{
			"StatusHub"
		});
		this.signalRConnection.OnNonHubMessage += this.signalRConnection_OnNonHubMessage;
		this.signalRConnection.OnError += this.signalRConnection_OnError;
		this.signalRConnection.OnStateChanged += this.signalRConnection_OnStateChanged;
		this.signalRConnection["StatusHub"].OnMethodCall += this.statusHub_OnMethodCall;
		this.signalRConnection.Open();
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00004C39 File Offset: 0x00002E39
	private void OnDestroy()
	{
		this.signalRConnection.Close();
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00004C46 File Offset: 0x00002E46
	private void OnGUI()
	{
		GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("START", Array.Empty<GUILayoutOption>()) && this.signalRConnection.State != ConnectionStates.Connected)
			{
				this.signalRConnection.Open();
			}
			if (GUILayout.Button("STOP", Array.Empty<GUILayoutOption>()) && this.signalRConnection.State == ConnectionStates.Connected)
			{
				this.signalRConnection.Close();
				this.messages.Clear();
			}
			if (GUILayout.Button("PING", Array.Empty<GUILayoutOption>()) && this.signalRConnection.State == ConnectionStates.Connected)
			{
				this.signalRConnection["StatusHub"].Call("Ping", Array.Empty<object>());
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(20f);
			GUILayout.Label("Connection Status Messages", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			this.messages.Draw((float)(Screen.width - 20), 0f);
			GUILayout.EndHorizontal();
		});
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00004C5F File Offset: 0x00002E5F
	private void signalRConnection_OnNonHubMessage(Connection manager, object data)
	{
		this.messages.Add("[Server Message] " + data.ToString());
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00004C7C File Offset: 0x00002E7C
	private void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
	{
		this.messages.Add(string.Format("[State Change] {0} => {1}", oldState, newState));
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00004C9F File Offset: 0x00002E9F
	private void signalRConnection_OnError(Connection manager, string error)
	{
		this.messages.Add("[Error] " + error);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00004CB8 File Offset: 0x00002EB8
	private void statusHub_OnMethodCall(Hub hub, string method, params object[] args)
	{
		string arg = (args.Length != 0) ? (args[0] as string) : string.Empty;
		string arg2 = (args.Length > 1) ? args[1].ToString() : string.Empty;
		if (method == "joined")
		{
			this.messages.Add(string.Format("[{0}] {1} joined at {2}", hub.Name, arg, arg2));
			return;
		}
		if (method == "rejoined")
		{
			this.messages.Add(string.Format("[{0}] {1} reconnected at {2}", hub.Name, arg, arg2));
			return;
		}
		if (!(method == "leave"))
		{
			this.messages.Add(string.Format("[{0}] {1}", hub.Name, method));
			return;
		}
		this.messages.Add(string.Format("[{0}] {1} leaved at {2}", hub.Name, arg, arg2));
	}

	// Token: 0x04000058 RID: 88
	private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/signalr");

	// Token: 0x04000059 RID: 89
	private Connection signalRConnection;

	// Token: 0x0400005A RID: 90
	private GUIMessageList messages = new GUIMessageList();
}

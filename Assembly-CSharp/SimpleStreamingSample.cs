using System;
using BestHTTP.Examples;
using BestHTTP.SignalR;
using UnityEngine;

// Token: 0x02000016 RID: 22
internal sealed class SimpleStreamingSample : MonoBehaviour
{
	// Token: 0x060000FF RID: 255 RVA: 0x00005D18 File Offset: 0x00003F18
	private void Start()
	{
		this.signalRConnection = new Connection(this.URI);
		this.signalRConnection.OnNonHubMessage += this.signalRConnection_OnNonHubMessage;
		this.signalRConnection.OnStateChanged += this.signalRConnection_OnStateChanged;
		this.signalRConnection.OnError += this.signalRConnection_OnError;
		this.signalRConnection.Open();
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00005D86 File Offset: 0x00003F86
	private void OnDestroy()
	{
		this.signalRConnection.Close();
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00005D93 File Offset: 0x00003F93
	private void OnGUI()
	{
		GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
		{
			GUILayout.Label("Messages", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			this.messages.Draw((float)(Screen.width - 20), 0f);
			GUILayout.EndHorizontal();
		});
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00005DAC File Offset: 0x00003FAC
	private void signalRConnection_OnNonHubMessage(Connection connection, object data)
	{
		this.messages.Add("[Server Message] " + data.ToString());
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00005DC9 File Offset: 0x00003FC9
	private void signalRConnection_OnStateChanged(Connection connection, ConnectionStates oldState, ConnectionStates newState)
	{
		this.messages.Add(string.Format("[State Change] {0} => {1}", oldState, newState));
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00005DEC File Offset: 0x00003FEC
	private void signalRConnection_OnError(Connection connection, string error)
	{
		this.messages.Add("[Error] " + error);
	}

	// Token: 0x04000077 RID: 119
	private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/streaming-connection");

	// Token: 0x04000078 RID: 120
	private Connection signalRConnection;

	// Token: 0x04000079 RID: 121
	private GUIMessageList messages = new GUIMessageList();
}

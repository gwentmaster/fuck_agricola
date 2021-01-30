using System;
using BestHTTP.Cookies;
using BestHTTP.Examples;
using BestHTTP.JSON;
using BestHTTP.SignalR;
using BestHTTP.SignalR.JsonEncoders;
using UnityEngine;

// Token: 0x02000011 RID: 17
public sealed class ConnectionAPISample : MonoBehaviour
{
	// Token: 0x060000B3 RID: 179 RVA: 0x000046B4 File Offset: 0x000028B4
	private void Start()
	{
		if (PlayerPrefs.HasKey("userName"))
		{
			CookieJar.Set(this.URI, new Cookie("user", PlayerPrefs.GetString("userName")));
		}
		this.signalRConnection = new Connection(this.URI);
		this.signalRConnection.JsonEncoder = new LitJsonEncoder();
		this.signalRConnection.OnStateChanged += this.signalRConnection_OnStateChanged;
		this.signalRConnection.OnNonHubMessage += this.signalRConnection_OnGeneralMessage;
		this.signalRConnection.Open();
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00004746 File Offset: 0x00002946
	private void OnGUI()
	{
		GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label("To Everybody", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.ToEveryBodyText = GUILayout.TextField(this.ToEveryBodyText, new GUILayoutOption[]
			{
				GUILayout.MinWidth(100f)
			});
			if (GUILayout.Button("Broadcast", Array.Empty<GUILayoutOption>()))
			{
				this.Broadcast(this.ToEveryBodyText);
			}
			if (GUILayout.Button("Broadcast (All Except Me)", Array.Empty<GUILayoutOption>()))
			{
				this.BroadcastExceptMe(this.ToEveryBodyText);
			}
			if (GUILayout.Button("Enter Name", Array.Empty<GUILayoutOption>()))
			{
				this.EnterName(this.ToEveryBodyText);
			}
			if (GUILayout.Button("Join Group", Array.Empty<GUILayoutOption>()))
			{
				this.JoinGroup(this.ToEveryBodyText);
			}
			if (GUILayout.Button("Leave Group", Array.Empty<GUILayoutOption>()))
			{
				this.LeaveGroup(this.ToEveryBodyText);
			}
			GUILayout.EndHorizontal();
			GUILayout.Label("To Me", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.ToMeText = GUILayout.TextField(this.ToMeText, new GUILayoutOption[]
			{
				GUILayout.MinWidth(100f)
			});
			if (GUILayout.Button("Send to me", Array.Empty<GUILayoutOption>()))
			{
				this.SendToMe(this.ToMeText);
			}
			GUILayout.EndHorizontal();
			GUILayout.Label("Private Message", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Message:", Array.Empty<GUILayoutOption>());
			this.PrivateMessageText = GUILayout.TextField(this.PrivateMessageText, new GUILayoutOption[]
			{
				GUILayout.MinWidth(100f)
			});
			GUILayout.Label("User or Group name:", Array.Empty<GUILayoutOption>());
			this.PrivateMessageUserOrGroupName = GUILayout.TextField(this.PrivateMessageUserOrGroupName, new GUILayoutOption[]
			{
				GUILayout.MinWidth(100f)
			});
			if (GUILayout.Button("Send to user", Array.Empty<GUILayoutOption>()))
			{
				this.SendToUser(this.PrivateMessageUserOrGroupName, this.PrivateMessageText);
			}
			if (GUILayout.Button("Send to group", Array.Empty<GUILayoutOption>()))
			{
				this.SendToGroup(this.PrivateMessageUserOrGroupName, this.PrivateMessageText);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(20f);
			if (this.signalRConnection.State == ConnectionStates.Closed)
			{
				if (GUILayout.Button("Start Connection", Array.Empty<GUILayoutOption>()))
				{
					this.signalRConnection.Open();
				}
			}
			else if (GUILayout.Button("Stop Connection", Array.Empty<GUILayoutOption>()))
			{
				this.signalRConnection.Close();
			}
			GUILayout.Space(20f);
			GUILayout.Label("Messages", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			this.messages.Draw((float)(Screen.width - 20), 0f);
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		});
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0000475F File Offset: 0x0000295F
	private void OnDestroy()
	{
		this.signalRConnection.Close();
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000476C File Offset: 0x0000296C
	private void signalRConnection_OnGeneralMessage(Connection manager, object data)
	{
		string str = Json.Encode(data);
		this.messages.Add("[Server Message] " + str);
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00004796 File Offset: 0x00002996
	private void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
	{
		this.messages.Add(string.Format("[State Change] {0} => {1}", oldState.ToString(), newState.ToString()));
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000047C7 File Offset: 0x000029C7
	private void Broadcast(string text)
	{
		this.signalRConnection.Send(new
		{
			Type = ConnectionAPISample.MessageTypes.Broadcast,
			Value = text
		});
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000047DC File Offset: 0x000029DC
	private void BroadcastExceptMe(string text)
	{
		this.signalRConnection.Send(new
		{
			Type = ConnectionAPISample.MessageTypes.BroadcastExceptMe,
			Value = text
		});
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000047F1 File Offset: 0x000029F1
	private void EnterName(string name)
	{
		this.signalRConnection.Send(new
		{
			Type = ConnectionAPISample.MessageTypes.Join,
			Value = name
		});
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00004806 File Offset: 0x00002A06
	private void JoinGroup(string groupName)
	{
		this.signalRConnection.Send(new
		{
			Type = ConnectionAPISample.MessageTypes.AddToGroup,
			Value = groupName
		});
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000481B File Offset: 0x00002A1B
	private void LeaveGroup(string groupName)
	{
		this.signalRConnection.Send(new
		{
			Type = ConnectionAPISample.MessageTypes.RemoveFromGroup,
			Value = groupName
		});
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00004830 File Offset: 0x00002A30
	private void SendToMe(string text)
	{
		this.signalRConnection.Send(new
		{
			Type = ConnectionAPISample.MessageTypes.Send,
			Value = text
		});
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00004845 File Offset: 0x00002A45
	private void SendToUser(string userOrGroupName, string text)
	{
		this.signalRConnection.Send(new
		{
			Type = ConnectionAPISample.MessageTypes.PrivateMessage,
			Value = string.Format("{0}|{1}", userOrGroupName, text)
		});
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00004865 File Offset: 0x00002A65
	private void SendToGroup(string userOrGroupName, string text)
	{
		this.signalRConnection.Send(new
		{
			Type = ConnectionAPISample.MessageTypes.SendToGroup,
			Value = string.Format("{0}|{1}", userOrGroupName, text)
		});
	}

	// Token: 0x04000051 RID: 81
	private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/raw-connection/");

	// Token: 0x04000052 RID: 82
	private Connection signalRConnection;

	// Token: 0x04000053 RID: 83
	private string ToEveryBodyText = string.Empty;

	// Token: 0x04000054 RID: 84
	private string ToMeText = string.Empty;

	// Token: 0x04000055 RID: 85
	private string PrivateMessageText = string.Empty;

	// Token: 0x04000056 RID: 86
	private string PrivateMessageUserOrGroupName = string.Empty;

	// Token: 0x04000057 RID: 87
	private GUIMessageList messages = new GUIMessageList();

	// Token: 0x02000750 RID: 1872
	private enum MessageTypes
	{
		// Token: 0x04002B45 RID: 11077
		Send,
		// Token: 0x04002B46 RID: 11078
		Broadcast,
		// Token: 0x04002B47 RID: 11079
		Join,
		// Token: 0x04002B48 RID: 11080
		PrivateMessage,
		// Token: 0x04002B49 RID: 11081
		AddToGroup,
		// Token: 0x04002B4A RID: 11082
		RemoveFromGroup,
		// Token: 0x04002B4B RID: 11083
		SendToGroup,
		// Token: 0x04002B4C RID: 11084
		BroadcastExceptMe
	}
}

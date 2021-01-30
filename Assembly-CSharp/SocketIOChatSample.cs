using System;
using System.Collections.Generic;
using BestHTTP.Examples;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using UnityEngine;

// Token: 0x02000017 RID: 23
public sealed class SocketIOChatSample : MonoBehaviour
{
	// Token: 0x06000107 RID: 263 RVA: 0x00005E78 File Offset: 0x00004078
	private void Start()
	{
		this.State = SocketIOChatSample.ChatStates.Login;
		SocketOptions socketOptions = new SocketOptions();
		socketOptions.AutoConnect = false;
		this.Manager = new SocketManager(new Uri("http://localhost:3000/socket.io/"), socketOptions);
		this.Manager.Socket.On("login", new SocketIOCallback(this.OnLogin));
		this.Manager.Socket.On("new message", new SocketIOCallback(this.OnNewMessage));
		this.Manager.Socket.On("user joined", new SocketIOCallback(this.OnUserJoined));
		this.Manager.Socket.On("user left", new SocketIOCallback(this.OnUserLeft));
		this.Manager.Socket.On("typing", new SocketIOCallback(this.OnTyping));
		this.Manager.Socket.On("stop typing", new SocketIOCallback(this.OnStopTyping));
		this.Manager.Socket.On(SocketIOEventTypes.Error, delegate(Socket socket, Packet packet, object[] args)
		{
			Debug.LogError(string.Format("Error: {0}", args[0].ToString()));
		});
		this.Manager.GetSocket("/nsp").On(SocketIOEventTypes.Connect, delegate(Socket socket, Packet packet, object[] arg)
		{
			Debug.LogWarning("Connected to /nsp");
			socket.Emit("testmsg", new object[]
			{
				"Message from /nsp 'on connect'"
			});
		});
		this.Manager.GetSocket("/nsp").On("nsp_message", delegate(Socket socket, Packet packet, object[] arg)
		{
			Debug.LogWarning("nsp_message: " + arg[0]);
		});
		this.Manager.Open();
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0000601E File Offset: 0x0000421E
	private void OnDestroy()
	{
		this.Manager.Close();
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0000602C File Offset: 0x0000422C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SampleSelector.SelectedSample.DestroyUnityObject();
		}
		if (this.typing && DateTime.UtcNow - this.lastTypingTime >= this.TYPING_TIMER_LENGTH)
		{
			this.Manager.Socket.Emit("stop typing", Array.Empty<object>());
			this.typing = false;
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00006094 File Offset: 0x00004294
	private void OnGUI()
	{
		SocketIOChatSample.ChatStates state = this.State;
		if (state == SocketIOChatSample.ChatStates.Login)
		{
			this.DrawLoginScreen();
			return;
		}
		if (state != SocketIOChatSample.ChatStates.Chat)
		{
			return;
		}
		this.DrawChatScreen();
	}

	// Token: 0x0600010B RID: 267 RVA: 0x000060BD File Offset: 0x000042BD
	private void DrawLoginScreen()
	{
		GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUIHelper.DrawCenteredText("What's your nickname?");
			this.userName = GUILayout.TextField(this.userName, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Join", Array.Empty<GUILayoutOption>()))
			{
				this.SetUserName();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		});
	}

	// Token: 0x0600010C RID: 268 RVA: 0x000060D6 File Offset: 0x000042D6
	private void DrawChatScreen()
	{
		GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
			GUILayout.Label(this.chatLog, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true),
				GUILayout.ExpandHeight(true)
			});
			GUILayout.EndScrollView();
			string text = string.Empty;
			if (this.typingUsers.Count > 0)
			{
				text += string.Format("{0}", this.typingUsers[0]);
				for (int i = 1; i < this.typingUsers.Count; i++)
				{
					text += string.Format(", {0}", this.typingUsers[i]);
				}
				if (this.typingUsers.Count == 1)
				{
					text += " is typing!";
				}
				else
				{
					text += " are typing!";
				}
			}
			GUILayout.Label(text, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Type here:", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.message = GUILayout.TextField(this.message, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Send", new GUILayoutOption[]
			{
				GUILayout.MaxWidth(100f)
			}))
			{
				this.SendMessage();
			}
			GUILayout.EndHorizontal();
			if (GUI.changed)
			{
				this.UpdateTyping();
			}
			GUILayout.EndVertical();
		});
	}

	// Token: 0x0600010D RID: 269 RVA: 0x000060EF File Offset: 0x000042EF
	private void SetUserName()
	{
		if (string.IsNullOrEmpty(this.userName))
		{
			return;
		}
		this.State = SocketIOChatSample.ChatStates.Chat;
		this.Manager.Socket.Emit("add user", new object[]
		{
			this.userName
		});
	}

	// Token: 0x0600010E RID: 270 RVA: 0x0000612C File Offset: 0x0000432C
	private void SendMessage()
	{
		if (string.IsNullOrEmpty(this.message))
		{
			return;
		}
		this.Manager.Socket.Emit("new message", new object[]
		{
			this.message
		});
		this.chatLog += string.Format("{0}: {1}\n", this.userName, this.message);
		this.message = string.Empty;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0000619E File Offset: 0x0000439E
	private void UpdateTyping()
	{
		if (!this.typing)
		{
			this.typing = true;
			this.Manager.Socket.Emit("typing", Array.Empty<object>());
		}
		this.lastTypingTime = DateTime.UtcNow;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x000061D8 File Offset: 0x000043D8
	private void addParticipantsMessage(Dictionary<string, object> data)
	{
		int num = Convert.ToInt32(data["numUsers"]);
		if (num == 1)
		{
			this.chatLog += "there's 1 participant\n";
			return;
		}
		this.chatLog = string.Concat(new object[]
		{
			this.chatLog,
			"there are ",
			num,
			" participants\n"
		});
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00006244 File Offset: 0x00004444
	private void addChatMessage(Dictionary<string, object> data)
	{
		string arg = data["username"] as string;
		string arg2 = data["message"] as string;
		this.chatLog += string.Format("{0}: {1}\n", arg, arg2);
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00006290 File Offset: 0x00004490
	private void AddChatTyping(Dictionary<string, object> data)
	{
		string item = data["username"] as string;
		this.typingUsers.Add(item);
	}

	// Token: 0x06000113 RID: 275 RVA: 0x000062BC File Offset: 0x000044BC
	private void RemoveChatTyping(Dictionary<string, object> data)
	{
		string username = data["username"] as string;
		int num = this.typingUsers.FindIndex((string name) => name.Equals(username));
		if (num != -1)
		{
			this.typingUsers.RemoveAt(num);
		}
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000630D File Offset: 0x0000450D
	private void OnLogin(Socket socket, Packet packet, params object[] args)
	{
		this.chatLog = "Welcome to Socket.IO Chat — \n";
		this.addParticipantsMessage(args[0] as Dictionary<string, object>);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00006328 File Offset: 0x00004528
	private void OnNewMessage(Socket socket, Packet packet, params object[] args)
	{
		this.addChatMessage(args[0] as Dictionary<string, object>);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00006338 File Offset: 0x00004538
	private void OnUserJoined(Socket socket, Packet packet, params object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string arg = dictionary["username"] as string;
		this.chatLog += string.Format("{0} joined\n", arg);
		this.addParticipantsMessage(dictionary);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00006384 File Offset: 0x00004584
	private void OnUserLeft(Socket socket, Packet packet, params object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string arg = dictionary["username"] as string;
		this.chatLog += string.Format("{0} left\n", arg);
		this.addParticipantsMessage(dictionary);
	}

	// Token: 0x06000118 RID: 280 RVA: 0x000063CE File Offset: 0x000045CE
	private void OnTyping(Socket socket, Packet packet, params object[] args)
	{
		this.AddChatTyping(args[0] as Dictionary<string, object>);
	}

	// Token: 0x06000119 RID: 281 RVA: 0x000063DE File Offset: 0x000045DE
	private void OnStopTyping(Socket socket, Packet packet, params object[] args)
	{
		this.RemoveChatTyping(args[0] as Dictionary<string, object>);
	}

	// Token: 0x0400007A RID: 122
	private readonly TimeSpan TYPING_TIMER_LENGTH = TimeSpan.FromMilliseconds(700.0);

	// Token: 0x0400007B RID: 123
	private SocketManager Manager;

	// Token: 0x0400007C RID: 124
	private SocketIOChatSample.ChatStates State;

	// Token: 0x0400007D RID: 125
	private string userName = string.Empty;

	// Token: 0x0400007E RID: 126
	private string message = string.Empty;

	// Token: 0x0400007F RID: 127
	private string chatLog = string.Empty;

	// Token: 0x04000080 RID: 128
	private Vector2 scrollPos;

	// Token: 0x04000081 RID: 129
	private bool typing;

	// Token: 0x04000082 RID: 130
	private DateTime lastTypingTime = DateTime.MinValue;

	// Token: 0x04000083 RID: 131
	private List<string> typingUsers = new List<string>();

	// Token: 0x02000751 RID: 1873
	private enum ChatStates
	{
		// Token: 0x04002B4E RID: 11086
		Login,
		// Token: 0x04002B4F RID: 11087
		Chat
	}
}

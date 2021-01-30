using System;
using System.Collections.Generic;
using BestHTTP.Examples;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using UnityEngine;

// Token: 0x02000018 RID: 24
public sealed class SocketIOWePlaySample : MonoBehaviour
{
	// Token: 0x0600011D RID: 285 RVA: 0x00006608 File Offset: 0x00004808
	private void Start()
	{
		SocketOptions socketOptions = new SocketOptions();
		socketOptions.AutoConnect = false;
		SocketManager socketManager = new SocketManager(new Uri("http://io.weplay.io/socket.io/"), socketOptions);
		this.Socket = socketManager.Socket;
		this.Socket.On(SocketIOEventTypes.Connect, new SocketIOCallback(this.OnConnected));
		this.Socket.On("joined", new SocketIOCallback(this.OnJoined));
		this.Socket.On("connections", new SocketIOCallback(this.OnConnections));
		this.Socket.On("join", new SocketIOCallback(this.OnJoin));
		this.Socket.On("move", new SocketIOCallback(this.OnMove));
		this.Socket.On("message", new SocketIOCallback(this.OnMessage));
		this.Socket.On("reload", new SocketIOCallback(this.OnReload));
		this.Socket.On("frame", new SocketIOCallback(this.OnFrame), false);
		this.Socket.On(SocketIOEventTypes.Error, new SocketIOCallback(this.OnError));
		socketManager.Open();
		this.State = SocketIOWePlaySample.States.Connecting;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00006741 File Offset: 0x00004941
	private void OnDestroy()
	{
		this.Socket.Manager.Close();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00006753 File Offset: 0x00004953
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SampleSelector.SelectedSample.DestroyUnityObject();
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00006768 File Offset: 0x00004968
	private void OnGUI()
	{
		switch (this.State)
		{
		case SocketIOWePlaySample.States.Connecting:
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				GUILayout.FlexibleSpace();
				GUIHelper.DrawCenteredText("Connecting to the server...");
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
			});
			return;
		case SocketIOWePlaySample.States.WaitForNick:
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				this.DrawLoginScreen();
			});
			return;
		case SocketIOWePlaySample.States.Joined:
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				if (this.FrameTexture != null)
				{
					GUILayout.Box(this.FrameTexture, Array.Empty<GUILayoutOption>());
				}
				this.DrawControls();
				this.DrawChat(true);
			});
			return;
		default:
			return;
		}
	}

	// Token: 0x06000121 RID: 289 RVA: 0x000067EC File Offset: 0x000049EC
	private void DrawLoginScreen()
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.FlexibleSpace();
		GUIHelper.DrawCenteredText("What's your nickname?");
		this.Nick = GUILayout.TextField(this.Nick, Array.Empty<GUILayoutOption>());
		if (GUILayout.Button("Join", Array.Empty<GUILayoutOption>()))
		{
			this.Join();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	// Token: 0x06000122 RID: 290 RVA: 0x0000684C File Offset: 0x00004A4C
	private void DrawControls()
	{
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("Controls:", Array.Empty<GUILayoutOption>());
		for (int i = 0; i < this.controls.Length; i++)
		{
			if (GUILayout.Button(this.controls[i], Array.Empty<GUILayoutOption>()))
			{
				this.Socket.Emit("move", new object[]
				{
					this.controls[i]
				});
			}
		}
		GUILayout.Label(" Connections: " + this.connections, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000068E0 File Offset: 0x00004AE0
	private void DrawChat(bool withInput = true)
	{
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
		for (int i = 0; i < this.messages.Count; i++)
		{
			GUILayout.Label(this.messages[i], new GUILayoutOption[]
			{
				GUILayout.MinWidth((float)Screen.width)
			});
		}
		GUILayout.EndScrollView();
		if (withInput)
		{
			GUILayout.Label("Your message: ", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			this.messageToSend = GUILayout.TextField(this.messageToSend, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Send", new GUILayoutOption[]
			{
				GUILayout.MaxWidth(100f)
			}))
			{
				this.SendMessage();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000069B0 File Offset: 0x00004BB0
	private void AddMessage(string msg)
	{
		this.messages.Insert(0, msg);
		if (this.messages.Count > this.MaxMessages)
		{
			this.messages.RemoveRange(this.MaxMessages, this.messages.Count - this.MaxMessages);
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00006A00 File Offset: 0x00004C00
	private void SendMessage()
	{
		if (string.IsNullOrEmpty(this.messageToSend))
		{
			return;
		}
		this.Socket.Emit("message", new object[]
		{
			this.messageToSend
		});
		this.AddMessage(string.Format("{0}: {1}", this.Nick, this.messageToSend));
		this.messageToSend = string.Empty;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00006A62 File Offset: 0x00004C62
	private void Join()
	{
		PlayerPrefs.SetString("Nick", this.Nick);
		this.Socket.Emit("join", new object[]
		{
			this.Nick
		});
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00006A94 File Offset: 0x00004C94
	private void Reload()
	{
		this.FrameTexture = null;
		if (this.Socket != null)
		{
			this.Socket.Manager.Close();
			this.Socket = null;
			this.Start();
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00006AC2 File Offset: 0x00004CC2
	private void OnConnected(Socket socket, Packet packet, params object[] args)
	{
		if (PlayerPrefs.HasKey("Nick"))
		{
			this.Nick = PlayerPrefs.GetString("Nick", "NickName");
			this.Join();
		}
		else
		{
			this.State = SocketIOWePlaySample.States.WaitForNick;
		}
		this.AddMessage("connected");
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00006AFF File Offset: 0x00004CFF
	private void OnJoined(Socket socket, Packet packet, params object[] args)
	{
		this.State = SocketIOWePlaySample.States.Joined;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00006B08 File Offset: 0x00004D08
	private void OnReload(Socket socket, Packet packet, params object[] args)
	{
		this.Reload();
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00006B10 File Offset: 0x00004D10
	private void OnMessage(Socket socket, Packet packet, params object[] args)
	{
		if (args.Length == 1)
		{
			this.AddMessage(args[0] as string);
			return;
		}
		this.AddMessage(string.Format("{0}: {1}", args[1], args[0]));
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00006B3D File Offset: 0x00004D3D
	private void OnMove(Socket socket, Packet packet, params object[] args)
	{
		this.AddMessage(string.Format("{0} pressed {1}", args[1], args[0]));
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00006B58 File Offset: 0x00004D58
	private void OnJoin(Socket socket, Packet packet, params object[] args)
	{
		string arg = (args.Length > 1) ? string.Format("({0})", args[1]) : string.Empty;
		this.AddMessage(string.Format("{0} joined {1}", args[0], arg));
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00006B94 File Offset: 0x00004D94
	private void OnConnections(Socket socket, Packet packet, params object[] args)
	{
		this.connections = Convert.ToInt32(args[0]);
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00006BA4 File Offset: 0x00004DA4
	private void OnFrame(Socket socket, Packet packet, params object[] args)
	{
		if (this.State != SocketIOWePlaySample.States.Joined)
		{
			return;
		}
		if (this.FrameTexture == null)
		{
			this.FrameTexture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
			this.FrameTexture.filterMode = FilterMode.Point;
		}
		byte[] data = packet.Attachments[0];
		this.FrameTexture.LoadImage(data);
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00006BFE File Offset: 0x00004DFE
	private void OnError(Socket socket, Packet packet, params object[] args)
	{
		this.AddMessage(string.Format("--ERROR - {0}", args[0].ToString()));
	}

	// Token: 0x04000084 RID: 132
	private string[] controls = new string[]
	{
		"left",
		"right",
		"a",
		"b",
		"up",
		"down",
		"select",
		"start"
	};

	// Token: 0x04000085 RID: 133
	private const float ratio = 1.5f;

	// Token: 0x04000086 RID: 134
	private int MaxMessages = 50;

	// Token: 0x04000087 RID: 135
	private SocketIOWePlaySample.States State;

	// Token: 0x04000088 RID: 136
	private Socket Socket;

	// Token: 0x04000089 RID: 137
	private string Nick = string.Empty;

	// Token: 0x0400008A RID: 138
	private string messageToSend = string.Empty;

	// Token: 0x0400008B RID: 139
	private int connections;

	// Token: 0x0400008C RID: 140
	private List<string> messages = new List<string>();

	// Token: 0x0400008D RID: 141
	private Vector2 scrollPos;

	// Token: 0x0400008E RID: 142
	private Texture2D FrameTexture;

	// Token: 0x02000754 RID: 1876
	private enum States
	{
		// Token: 0x04002B56 RID: 11094
		Connecting,
		// Token: 0x04002B57 RID: 11095
		WaitForNick,
		// Token: 0x04002B58 RID: 11096
		Joined
	}
}

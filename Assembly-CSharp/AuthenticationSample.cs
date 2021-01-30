using System;
using BestHTTP.Examples;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Authentication;
using BestHTTP.SignalR.Hubs;
using UnityEngine;

// Token: 0x0200000F RID: 15
internal class AuthenticationSample : MonoBehaviour
{
	// Token: 0x060000A3 RID: 163 RVA: 0x000041C4 File Offset: 0x000023C4
	private void Start()
	{
		this.signalRConnection = new Connection(this.URI, new Hub[]
		{
			new BaseHub("noauthhub", "Messages"),
			new BaseHub("invokeauthhub", "Messages Invoked By Admin or Invoker"),
			new BaseHub("authhub", "Messages Requiring Authentication to Send or Receive"),
			new BaseHub("inheritauthhub", "Messages Requiring Authentication to Send or Receive Because of Inheritance"),
			new BaseHub("incomingauthhub", "Messages Requiring Authentication to Send"),
			new BaseHub("adminauthhub", "Messages Requiring Admin Membership to Send or Receive"),
			new BaseHub("userandroleauthhub", "Messages Requiring Name to be \"User\" and Role to be \"Admin\" to Send or Receive")
		});
		if (!string.IsNullOrEmpty(this.userName) && !string.IsNullOrEmpty(this.role))
		{
			this.signalRConnection.AuthenticationProvider = new HeaderAuthenticator(this.userName, this.role);
		}
		this.signalRConnection.OnConnected += this.signalRConnection_OnConnected;
		this.signalRConnection.Open();
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000042BE File Offset: 0x000024BE
	private void OnDestroy()
	{
		this.signalRConnection.Close();
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x000042CB File Offset: 0x000024CB
	private void OnGUI()
	{
		GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
		{
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			if (this.signalRConnection.AuthenticationProvider == null)
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Username (Enter 'User'):", Array.Empty<GUILayoutOption>());
				this.userName = GUILayout.TextField(this.userName, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Roles (Enter 'Invoker' or 'Admin'):", Array.Empty<GUILayoutOption>());
				this.role = GUILayout.TextField(this.role, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				GUILayout.EndHorizontal();
				if (GUILayout.Button("Log in", Array.Empty<GUILayoutOption>()))
				{
					this.Restart();
				}
			}
			for (int i = 0; i < this.signalRConnection.Hubs.Length; i++)
			{
				(this.signalRConnection.Hubs[i] as BaseHub).Draw();
			}
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		});
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x000042E4 File Offset: 0x000024E4
	private void signalRConnection_OnConnected(Connection manager)
	{
		for (int i = 0; i < this.signalRConnection.Hubs.Length; i++)
		{
			(this.signalRConnection.Hubs[i] as BaseHub).InvokedFromClient();
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00004320 File Offset: 0x00002520
	private void Restart()
	{
		this.signalRConnection.OnConnected -= this.signalRConnection_OnConnected;
		this.signalRConnection.Close();
		this.signalRConnection = null;
		this.Start();
	}

	// Token: 0x0400004A RID: 74
	private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/signalr");

	// Token: 0x0400004B RID: 75
	private Connection signalRConnection;

	// Token: 0x0400004C RID: 76
	private string userName = string.Empty;

	// Token: 0x0400004D RID: 77
	private string role = string.Empty;

	// Token: 0x0400004E RID: 78
	private Vector2 scrollPos;
}

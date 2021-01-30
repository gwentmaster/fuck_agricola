using System;
using BestHTTP;
using BestHTTP.Examples;
using BestHTTP.WebSocket;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class WebSocketSample : MonoBehaviour
{
	// Token: 0x06000134 RID: 308 RVA: 0x00006CD5 File Offset: 0x00004ED5
	private void OnDestroy()
	{
		if (this.webSocket != null)
		{
			this.webSocket.Close();
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00006CEA File Offset: 0x00004EEA
	private void OnGUI()
	{
		GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
		{
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
			GUILayout.Label(this.Text, Array.Empty<GUILayoutOption>());
			GUILayout.EndScrollView();
			GUILayout.Space(5f);
			GUILayout.FlexibleSpace();
			this.address = GUILayout.TextField(this.address, Array.Empty<GUILayoutOption>());
			if (this.webSocket == null && GUILayout.Button("Open Web Socket", Array.Empty<GUILayoutOption>()))
			{
				this.webSocket = new WebSocket(new Uri(this.address));
				this.webSocket.StartPingThread = true;
				if (HTTPManager.Proxy != null)
				{
					this.webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
				}
				WebSocket webSocket = this.webSocket;
				webSocket.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(webSocket.OnOpen, new OnWebSocketOpenDelegate(this.OnOpen));
				WebSocket webSocket2 = this.webSocket;
				webSocket2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(webSocket2.OnMessage, new OnWebSocketMessageDelegate(this.OnMessageReceived));
				WebSocket webSocket3 = this.webSocket;
				webSocket3.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(webSocket3.OnClosed, new OnWebSocketClosedDelegate(this.OnClosed));
				WebSocket webSocket4 = this.webSocket;
				webSocket4.OnError = (OnWebSocketErrorDelegate)Delegate.Combine(webSocket4.OnError, new OnWebSocketErrorDelegate(this.OnError));
				this.webSocket.Open();
				this.Text += "Opening Web Socket...\n";
			}
			if (this.webSocket != null && this.webSocket.IsOpen)
			{
				GUILayout.Space(10f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				this.msgToSend = GUILayout.TextField(this.msgToSend, Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Send", new GUILayoutOption[]
				{
					GUILayout.MaxWidth(70f)
				}))
				{
					this.Text += "Sending message...\n";
					this.webSocket.Send(this.msgToSend);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(10f);
				if (GUILayout.Button("Close", Array.Empty<GUILayoutOption>()))
				{
					this.webSocket.Close(1000, "Bye!");
				}
			}
		});
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00006D03 File Offset: 0x00004F03
	private void OnOpen(WebSocket ws)
	{
		this.Text += string.Format("-WebSocket Open!\n", Array.Empty<object>());
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00006D25 File Offset: 0x00004F25
	private void OnMessageReceived(WebSocket ws, string message)
	{
		this.Text += string.Format("-Message received: {0}\n", message);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00006D43 File Offset: 0x00004F43
	private void OnClosed(WebSocket ws, ushort code, string message)
	{
		this.Text += string.Format("-WebSocket closed! Code: {0} Message: {1}\n", code, message);
		this.webSocket = null;
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00006D70 File Offset: 0x00004F70
	private void OnError(WebSocket ws, Exception ex)
	{
		string str = string.Empty;
		if (ws.InternalRequest.Response != null)
		{
			str = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
		}
		this.Text += string.Format("-An error occured: {0}\n", (ex != null) ? ex.Message : ("Unknown Error " + str));
		this.webSocket = null;
	}

	// Token: 0x0400008F RID: 143
	private string address = "wss://echo.websocket.org";

	// Token: 0x04000090 RID: 144
	private string msgToSend = "Hello World!";

	// Token: 0x04000091 RID: 145
	private string Text = string.Empty;

	// Token: 0x04000092 RID: 146
	private WebSocket webSocket;

	// Token: 0x04000093 RID: 147
	private Vector2 scrollPos;
}

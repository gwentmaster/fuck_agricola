using System;
using BestHTTP.Examples;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using UnityEngine;

// Token: 0x02000013 RID: 19
internal class DemoHubSample : MonoBehaviour
{
	// Token: 0x060000CB RID: 203 RVA: 0x00004EBC File Offset: 0x000030BC
	private void Start()
	{
		this.demoHub = new DemoHub();
		this.typedDemoHub = new TypedDemoHub();
		this.vbDemoHub = new Hub("vbdemo");
		this.signalRConnection = new Connection(this.URI, new Hub[]
		{
			this.demoHub,
			this.typedDemoHub,
			this.vbDemoHub
		});
		this.signalRConnection.JsonEncoder = new LitJsonEncoder();
		this.signalRConnection.OnConnected += delegate(Connection connection)
		{
			var <>f__AnonymousType = new
			{
				Name = "Foo",
				Age = 20,
				Address = new
				{
					Street = "One Microsoft Way",
					Zip = "98052"
				}
			};
			this.demoHub.AddToGroups();
			this.demoHub.GetValue();
			this.demoHub.TaskWithException();
			this.demoHub.GenericTaskWithException();
			this.demoHub.SynchronousException();
			this.demoHub.DynamicTask();
			this.demoHub.PassingDynamicComplex(<>f__AnonymousType);
			this.demoHub.SimpleArray(new int[]
			{
				5,
				5,
				6
			});
			this.demoHub.ComplexType(<>f__AnonymousType);
			this.demoHub.ComplexArray(new object[]
			{
				<>f__AnonymousType,
				<>f__AnonymousType,
				<>f__AnonymousType
			});
			this.demoHub.ReportProgress("Long running job!");
			this.demoHub.Overload();
			this.demoHub.State["name"] = "Testing state!";
			this.demoHub.ReadStateValue();
			this.demoHub.PlainTask();
			this.demoHub.GenericTaskWithContinueWith();
			this.typedDemoHub.Echo("Typed echo callback");
			this.vbDemoHub.Call("readStateValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.vbReadStateResult = string.Format("Read some state from VB.NET! => {0}", (result.ReturnValue == null) ? "undefined" : result.ReturnValue.ToString());
			}, Array.Empty<object>());
		};
		this.signalRConnection.Open();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00004F53 File Offset: 0x00003153
	private void OnDestroy()
	{
		this.signalRConnection.Close();
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00004F60 File Offset: 0x00003160
	private void OnGUI()
	{
		GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
		{
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			this.demoHub.Draw();
			this.typedDemoHub.Draw();
			GUILayout.Label("Read State Value", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.vbReadStateResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		});
	}

	// Token: 0x0400005B RID: 91
	private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/signalr");

	// Token: 0x0400005C RID: 92
	private Connection signalRConnection;

	// Token: 0x0400005D RID: 93
	private DemoHub demoHub;

	// Token: 0x0400005E RID: 94
	private TypedDemoHub typedDemoHub;

	// Token: 0x0400005F RID: 95
	private Hub vbDemoHub;

	// Token: 0x04000060 RID: 96
	private string vbReadStateResult = string.Empty;

	// Token: 0x04000061 RID: 97
	private Vector2 scrollPos;
}

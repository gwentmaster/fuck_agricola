using System;
using BestHTTP.Examples;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

// Token: 0x02000015 RID: 21
internal class DemoHub : Hub
{
	// Token: 0x060000D7 RID: 215 RVA: 0x0000529C File Offset: 0x0000349C
	public DemoHub() : base("demo")
	{
		base.On("invoke", new OnMethodCallCallbackDelegate(this.Invoke));
		base.On("signal", new OnMethodCallCallbackDelegate(this.Signal));
		base.On("groupAdded", new OnMethodCallCallbackDelegate(this.GroupAdded));
		base.On("fromArbitraryCode", new OnMethodCallCallbackDelegate(this.FromArbitraryCode));
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000053D8 File Offset: 0x000035D8
	public void ReportProgress(string arg)
	{
		base.Call("reportProgress", new OnMethodResultDelegate(this.OnLongRunningJob_Done), null, new OnMethodProgressDelegate(this.OnLongRunningJob_Progress), new object[]
		{
			arg
		});
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00005414 File Offset: 0x00003614
	public void OnLongRunningJob_Progress(Hub hub, ClientMessage originialMessage, ProgressMessage progress)
	{
		this.longRunningJobProgress = (float)progress.Progress;
		this.longRunningJobStatus = progress.Progress.ToString() + "%";
	}

	// Token: 0x060000DA RID: 218 RVA: 0x0000544C File Offset: 0x0000364C
	public void OnLongRunningJob_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.longRunningJobStatus = result.ReturnValue.ToString();
		this.MultipleCalls();
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00005465 File Offset: 0x00003665
	public void MultipleCalls()
	{
		base.Call("multipleCalls", Array.Empty<object>());
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00005478 File Offset: 0x00003678
	public void DynamicTask()
	{
		base.Call("dynamicTask", new OnMethodResultDelegate(this.OnDynamicTask_Done), new OnMethodFailedDelegate(this.OnDynamicTask_Failed), Array.Empty<object>());
	}

	// Token: 0x060000DD RID: 221 RVA: 0x000054A3 File Offset: 0x000036A3
	private void OnDynamicTask_Failed(Hub hub, ClientMessage originalMessage, FailureMessage result)
	{
		this.dynamicTaskResult = string.Format("The dynamic task failed :( {0}", result.ErrorMessage);
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000054BB File Offset: 0x000036BB
	private void OnDynamicTask_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.dynamicTaskResult = string.Format("The dynamic task! {0}", result.ReturnValue);
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000054D3 File Offset: 0x000036D3
	public void AddToGroups()
	{
		base.Call("addToGroups", Array.Empty<object>());
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000054E6 File Offset: 0x000036E6
	public void GetValue()
	{
		base.Call("getValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.genericTaskResult = string.Format("The value is {0} after 5 seconds", result.ReturnValue);
		}, Array.Empty<object>());
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00005505 File Offset: 0x00003705
	public void TaskWithException()
	{
		base.Call("taskWithException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			this.taskWithExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
		}, Array.Empty<object>());
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00005525 File Offset: 0x00003725
	public void GenericTaskWithException()
	{
		base.Call("genericTaskWithException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			this.genericTaskWithExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
		}, Array.Empty<object>());
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00005545 File Offset: 0x00003745
	public void SynchronousException()
	{
		base.Call("synchronousException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			this.synchronousExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
		}, Array.Empty<object>());
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00005565 File Offset: 0x00003765
	public void PassingDynamicComplex(object person)
	{
		base.Call("passingDynamicComplex", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.invokingHubMethodWithDynamicResult = string.Format("The person's age is {0}", result.ReturnValue);
		}, new object[]
		{
			person
		});
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00005589 File Offset: 0x00003789
	public void SimpleArray(int[] array)
	{
		base.Call("simpleArray", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.simpleArrayResult = "Simple array works!";
		}, new object[]
		{
			array
		});
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x000055AD File Offset: 0x000037AD
	public void ComplexType(object person)
	{
		base.Call("complexType", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.complexTypeResult = string.Format("Complex Type -> {0}", ((IHub)this).Connection.JsonEncoder.Encode(base.State["person"]));
		}, new object[]
		{
			person
		});
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x000055D1 File Offset: 0x000037D1
	public void ComplexArray(object[] complexArray)
	{
		base.Call("ComplexArray", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.complexArrayResult = "Complex Array Works!";
		}, new object[]
		{
			complexArray
		});
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x000055F5 File Offset: 0x000037F5
	public void Overload()
	{
		base.Call("Overload", new OnMethodResultDelegate(this.OnVoidOverload_Done), Array.Empty<object>());
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00005614 File Offset: 0x00003814
	private void OnVoidOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.voidOverloadResult = "Void Overload called";
		this.Overload(101);
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00005629 File Offset: 0x00003829
	public void Overload(int number)
	{
		base.Call("Overload", new OnMethodResultDelegate(this.OnIntOverload_Done), new object[]
		{
			number
		});
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00005652 File Offset: 0x00003852
	private void OnIntOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		this.intOverloadResult = string.Format("Overload with return value called => {0}", result.ReturnValue.ToString());
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0000566F File Offset: 0x0000386F
	public void ReadStateValue()
	{
		base.Call("readStateValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.readStateResult = string.Format("Read some state! => {0}", result.ReturnValue);
		}, Array.Empty<object>());
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000568E File Offset: 0x0000388E
	public void PlainTask()
	{
		base.Call("plainTask", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.plainTaskResult = "Plain Task Result";
		}, Array.Empty<object>());
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000056AD File Offset: 0x000038AD
	public void GenericTaskWithContinueWith()
	{
		base.Call("genericTaskWithContinueWith", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			this.genericTaskWithContinueWithResult = result.ReturnValue.ToString();
		}, Array.Empty<object>());
	}

	// Token: 0x060000EF RID: 239 RVA: 0x000056CC File Offset: 0x000038CC
	private void FromArbitraryCode(Hub hub, MethodCallMessage methodCall)
	{
		this.fromArbitraryCodeResult = (methodCall.Arguments[0] as string);
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000056E1 File Offset: 0x000038E1
	private void GroupAdded(Hub hub, MethodCallMessage methodCall)
	{
		if (!string.IsNullOrEmpty(this.groupAddedResult))
		{
			this.groupAddedResult = "Group Already Added!";
			return;
		}
		this.groupAddedResult = "Group Added!";
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00005707 File Offset: 0x00003907
	private void Signal(Hub hub, MethodCallMessage methodCall)
	{
		this.dynamicTaskResult = string.Format("The dynamic task! {0}", methodCall.Arguments[0]);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00005721 File Offset: 0x00003921
	private void Invoke(Hub hub, MethodCallMessage methodCall)
	{
		this.invokeResults.Add(string.Format("{0} client state index -> {1}", methodCall.Arguments[0], base.State["index"]));
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00005750 File Offset: 0x00003950
	public void Draw()
	{
		GUILayout.Label("Arbitrary Code", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(string.Format("Sending {0} from arbitrary code without the hub itself!", this.fromArbitraryCodeResult), Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Group Added", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.groupAddedResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Dynamic Task", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.dynamicTaskResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Report Progress", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(this.longRunningJobStatus, Array.Empty<GUILayoutOption>());
		GUILayout.HorizontalSlider(this.longRunningJobProgress, 0f, 100f, Array.Empty<GUILayoutOption>());
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.genericTaskResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Task With Exception", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.taskWithExceptionResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task With Exception", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.genericTaskWithExceptionResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Synchronous Exception", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.synchronousExceptionResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Invoking hub method with dynamic", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.invokingHubMethodWithDynamicResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Simple Array", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.simpleArrayResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Complex Type", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.complexTypeResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Complex Array", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.complexArrayResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Overloads", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
		GUILayout.Label(this.voidOverloadResult, Array.Empty<GUILayoutOption>());
		GUILayout.Label(this.intOverloadResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Read State Value", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.readStateResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Plain Task", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.plainTaskResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task With ContinueWith", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		GUILayout.Label(this.genericTaskWithContinueWithResult, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Message Pump", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Space(20f);
		this.invokeResults.Draw((float)(Screen.width - 40), 270f);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
	}

	// Token: 0x04000064 RID: 100
	private float longRunningJobProgress;

	// Token: 0x04000065 RID: 101
	private string longRunningJobStatus = "Not Started!";

	// Token: 0x04000066 RID: 102
	private string fromArbitraryCodeResult = string.Empty;

	// Token: 0x04000067 RID: 103
	private string groupAddedResult = string.Empty;

	// Token: 0x04000068 RID: 104
	private string dynamicTaskResult = string.Empty;

	// Token: 0x04000069 RID: 105
	private string genericTaskResult = string.Empty;

	// Token: 0x0400006A RID: 106
	private string taskWithExceptionResult = string.Empty;

	// Token: 0x0400006B RID: 107
	private string genericTaskWithExceptionResult = string.Empty;

	// Token: 0x0400006C RID: 108
	private string synchronousExceptionResult = string.Empty;

	// Token: 0x0400006D RID: 109
	private string invokingHubMethodWithDynamicResult = string.Empty;

	// Token: 0x0400006E RID: 110
	private string simpleArrayResult = string.Empty;

	// Token: 0x0400006F RID: 111
	private string complexTypeResult = string.Empty;

	// Token: 0x04000070 RID: 112
	private string complexArrayResult = string.Empty;

	// Token: 0x04000071 RID: 113
	private string voidOverloadResult = string.Empty;

	// Token: 0x04000072 RID: 114
	private string intOverloadResult = string.Empty;

	// Token: 0x04000073 RID: 115
	private string readStateResult = string.Empty;

	// Token: 0x04000074 RID: 116
	private string plainTaskResult = string.Empty;

	// Token: 0x04000075 RID: 117
	private string genericTaskWithContinueWithResult = string.Empty;

	// Token: 0x04000076 RID: 118
	private GUIMessageList invokeResults = new GUIMessageList();
}

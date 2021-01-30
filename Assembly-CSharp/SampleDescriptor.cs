using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public sealed class SampleDescriptor
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600008A RID: 138 RVA: 0x00003A5F File Offset: 0x00001C5F
	// (set) Token: 0x0600008B RID: 139 RVA: 0x00003A67 File Offset: 0x00001C67
	public bool IsLabel { get; set; }

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600008C RID: 140 RVA: 0x00003A70 File Offset: 0x00001C70
	// (set) Token: 0x0600008D RID: 141 RVA: 0x00003A78 File Offset: 0x00001C78
	public Type Type { get; set; }

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600008E RID: 142 RVA: 0x00003A81 File Offset: 0x00001C81
	// (set) Token: 0x0600008F RID: 143 RVA: 0x00003A89 File Offset: 0x00001C89
	public string DisplayName { get; set; }

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000090 RID: 144 RVA: 0x00003A92 File Offset: 0x00001C92
	// (set) Token: 0x06000091 RID: 145 RVA: 0x00003A9A File Offset: 0x00001C9A
	public string Description { get; set; }

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000092 RID: 146 RVA: 0x00003AA3 File Offset: 0x00001CA3
	// (set) Token: 0x06000093 RID: 147 RVA: 0x00003AAB File Offset: 0x00001CAB
	public string CodeBlock { get; set; }

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000094 RID: 148 RVA: 0x00003AB4 File Offset: 0x00001CB4
	// (set) Token: 0x06000095 RID: 149 RVA: 0x00003ABC File Offset: 0x00001CBC
	public bool IsSelected { get; set; }

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000096 RID: 150 RVA: 0x00003AC5 File Offset: 0x00001CC5
	// (set) Token: 0x06000097 RID: 151 RVA: 0x00003ACD File Offset: 0x00001CCD
	public GameObject UnityObject { get; set; }

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000098 RID: 152 RVA: 0x00003AD6 File Offset: 0x00001CD6
	public bool IsRunning
	{
		get
		{
			return this.UnityObject != null;
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00003AE4 File Offset: 0x00001CE4
	public SampleDescriptor(Type type, string displayName, string description, string codeBlock)
	{
		this.Type = type;
		this.DisplayName = displayName;
		this.Description = description;
		this.CodeBlock = codeBlock;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00003B09 File Offset: 0x00001D09
	public void CreateUnityObject()
	{
		if (this.UnityObject != null)
		{
			return;
		}
		this.UnityObject = new GameObject(this.DisplayName);
		this.UnityObject.AddComponent(this.Type);
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00003B3D File Offset: 0x00001D3D
	public void DestroyUnityObject()
	{
		if (this.UnityObject != null)
		{
			UnityEngine.Object.Destroy(this.UnityObject);
			this.UnityObject = null;
		}
	}
}

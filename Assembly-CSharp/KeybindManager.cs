using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class KeybindManager : MonoBehaviour
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000815 RID: 2069 RVA: 0x00038CA8 File Offset: 0x00036EA8
	public static KeybindManager instance
	{
		get
		{
			if (KeybindManager._instance == null)
			{
				KeybindManager._instance = UnityEngine.Object.FindObjectOfType<KeybindManager>();
				if (KeybindManager._instance == null)
				{
					KeybindManager._instance = new GameObject
					{
						name = "KeybindManager"
					}.AddComponent<KeybindManager>();
				}
			}
			return KeybindManager._instance;
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00038CF8 File Offset: 0x00036EF8
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.m_dict = new Dictionary<string, KeyCode>();
		this.m_events = new List<KeybindManager.KeyCallback>();
		this.RebuildDictionary(false);
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00038D24 File Offset: 0x00036F24
	public void AddEvent(KeybindManager.KeyEvent callback, string key, KeybindManager.KeybindEvents trigger)
	{
		for (int i = 0; i < this.m_events.Count; i++)
		{
			if (this.m_events[i].m_event == callback && this.m_events[i].m_key == key && this.m_events[i].m_trigger == trigger)
			{
				Debug.LogWarning("Current event already registered to Keybind Manager");
				return;
			}
		}
		KeybindManager.KeyCallback item;
		item.m_event = callback;
		item.m_key = key;
		item.m_trigger = trigger;
		if (!this.m_events.Contains(item))
		{
			this.m_events.Add(item);
		}
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00038DCC File Offset: 0x00036FCC
	public void RemoveEvent(KeybindManager.KeyEvent callback, string key, KeybindManager.KeybindEvents trigger)
	{
		for (int i = 0; i < this.m_events.Count; i++)
		{
			if (this.m_events[i].m_event == callback && this.m_events[i].m_key == key && this.m_events[i].m_trigger == trigger)
			{
				this.m_events.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x00038E44 File Offset: 0x00037044
	private void Update()
	{
		if (this.m_events == null || this.m_events.Count == 0)
		{
			return;
		}
		foreach (KeybindManager.KeyCallback keyCallback in this.m_events)
		{
			KeyCode key = this.GetKey(keyCallback.m_key);
			switch (keyCallback.m_trigger)
			{
			case KeybindManager.KeybindEvents.KeyDownTrigger:
				if (Input.GetKeyDown(key))
				{
					keyCallback.m_event();
				}
				break;
			case KeybindManager.KeybindEvents.KeyDown:
				if (Input.GetKey(key))
				{
					keyCallback.m_event();
				}
				break;
			case KeybindManager.KeybindEvents.KeyUpTrigger:
				if (Input.GetKeyUp(key))
				{
					keyCallback.m_event();
				}
				break;
			case KeybindManager.KeybindEvents.KeyUp:
				if (!Input.GetKey(key))
				{
					keyCallback.m_event();
				}
				break;
			}
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00038F2C File Offset: 0x0003712C
	public void RebuildDictionary(bool bUseDefaults)
	{
		if (this.m_dict != null && this.m_inputs != null)
		{
			this.m_dict.Clear();
			foreach (KeybindManager.KeybindingInspector keybindingInspector in this.m_inputs)
			{
				if (bUseDefaults)
				{
					PlayerPrefs.SetString("Input_" + keybindingInspector.commandName, keybindingInspector.defaultInputKey.ToString());
					this.m_dict.Add(keybindingInspector.commandName, keybindingInspector.defaultInputKey);
				}
				else
				{
					KeyCode value = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Input_" + keybindingInspector.commandName, keybindingInspector.defaultInputKey.ToString()));
					this.m_dict.Add(keybindingInspector.commandName, value);
				}
			}
		}
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00039010 File Offset: 0x00037210
	private void UpdateInput(string commandName, KeyCode newInputKey)
	{
		if (this.m_dict != null && this.m_dict.ContainsKey(commandName))
		{
			PlayerPrefs.SetString("Input_" + commandName, newInputKey.ToString());
			this.m_dict[commandName] = newInputKey;
		}
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0003905D File Offset: 0x0003725D
	private KeyCode GetKey(string commandName)
	{
		if (this.m_dict != null && this.m_dict.ContainsKey(commandName))
		{
			return this.m_dict[commandName];
		}
		return KeyCode.None;
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00039083 File Offset: 0x00037283
	public uint GetInputCount()
	{
		if (this.m_inputs == null)
		{
			return 0U;
		}
		return (uint)this.m_inputs.Length;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00039097 File Offset: 0x00037297
	public bool GetShouldDisplayKeybind(uint index)
	{
		return this.m_inputs != null && (ulong)index < (ulong)((long)this.m_inputs.Length) && this.m_inputs[(int)index].showInOptions;
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x000390C1 File Offset: 0x000372C1
	public bool GetShouldAllowRemapOfKeybind(uint index)
	{
		return this.m_inputs != null && (ulong)index < (ulong)((long)this.m_inputs.Length) && this.m_inputs[(int)index].remappable;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x000390EB File Offset: 0x000372EB
	public string GetDisplayLine(uint index)
	{
		if (this.m_inputs != null && (ulong)index < (ulong)((long)this.m_inputs.Length))
		{
			return this.m_inputs[(int)index].displayName;
		}
		return string.Empty;
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0003911C File Offset: 0x0003731C
	public string GetKey(uint index)
	{
		if (this.m_inputs != null && (ulong)index < (ulong)((long)this.m_inputs.Length))
		{
			return this.GetKey(this.m_inputs[(int)index].commandName).ToString();
		}
		return string.Empty;
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00039169 File Offset: 0x00037369
	public void SetKey(uint index, KeyCode newInputKey)
	{
		if (this.m_inputs != null && (ulong)index < (ulong)((long)this.m_inputs.Length))
		{
			this.UpdateInput(this.m_inputs[(int)index].commandName, newInputKey);
		}
	}

	// Token: 0x04000905 RID: 2309
	private static KeybindManager _instance;

	// Token: 0x04000906 RID: 2310
	[SerializeField]
	private KeybindManager.KeybindingInspector[] m_inputs;

	// Token: 0x04000907 RID: 2311
	private Dictionary<string, KeyCode> m_dict;

	// Token: 0x04000908 RID: 2312
	private List<KeybindManager.KeyCallback> m_events;

	// Token: 0x0200079F RID: 1951
	[Serializable]
	public struct KeybindingInspector
	{
		// Token: 0x04002C64 RID: 11364
		public string commandName;

		// Token: 0x04002C65 RID: 11365
		public string displayName;

		// Token: 0x04002C66 RID: 11366
		public KeyCode defaultInputKey;

		// Token: 0x04002C67 RID: 11367
		public bool remappable;

		// Token: 0x04002C68 RID: 11368
		public bool showInOptions;
	}

	// Token: 0x020007A0 RID: 1952
	public enum KeybindEvents
	{
		// Token: 0x04002C6A RID: 11370
		KeyDownTrigger,
		// Token: 0x04002C6B RID: 11371
		KeyDown,
		// Token: 0x04002C6C RID: 11372
		KeyUpTrigger,
		// Token: 0x04002C6D RID: 11373
		KeyUp
	}

	// Token: 0x020007A1 RID: 1953
	private struct KeyCallback
	{
		// Token: 0x04002C6E RID: 11374
		public KeybindManager.KeyEvent m_event;

		// Token: 0x04002C6F RID: 11375
		public string m_key;

		// Token: 0x04002C70 RID: 11376
		public KeybindManager.KeybindEvents m_trigger;
	}

	// Token: 0x020007A2 RID: 1954
	// (Invoke) Token: 0x060042A7 RID: 17063
	public delegate void KeyEvent();
}

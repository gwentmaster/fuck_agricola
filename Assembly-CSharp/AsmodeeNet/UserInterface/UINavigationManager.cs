using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000647 RID: 1607
	public class UINavigationManager : MonoBehaviour
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x00125B7C File Offset: 0x00123D7C
		public FocusableLayer RootFocusableLayer
		{
			get
			{
				if (this._rootFocusableLayer == null)
				{
					if (this._focusableLayers.Count > 0)
					{
						this._rootFocusableLayer = this._focusableLayers.First<FocusableLayer>();
					}
					else
					{
						AsmoLogger.Error("UINavigationManager", "Missing RootFocusableLayer", null);
					}
				}
				return this._rootFocusableLayer;
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06003B22 RID: 15138 RVA: 0x00125BD0 File Offset: 0x00123DD0
		// (remove) Token: 0x06003B23 RID: 15139 RVA: 0x00125C08 File Offset: 0x00123E08
		public event Action OnBackAction;

		// Token: 0x06003B24 RID: 15140 RVA: 0x00125C3D File Offset: 0x00123E3D
		private void Awake()
		{
			this.forceToGiveFocus = (() => Input.GetJoystickNames().Length != 0);
			this.highlightFocusable = delegate(Focusable focusable)
			{
				focusable.StopCoroutine(this._Bounce(focusable));
				focusable.StartCoroutine(this._Bounce(focusable));
			};
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x00125C76 File Offset: 0x00123E76
		private void Start()
		{
			if (this.navigationInput == null)
			{
				AsmoLogger.Error("UINavigationManager", "\"navigationInput\" is null. #define UINAVIGATION, provide your own version or deactivate UINavigationManager in CoreApplication", null);
			}
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x00125C90 File Offset: 0x00123E90
		private IEnumerator _Bounce(Focusable focusable)
		{
			Transform t = focusable.transform;
			Vector3 originalScale = Vector3.one;
			Vector3 bouncedScale = originalScale * 1.1f;
			t.localScale = bouncedScale;
			float currentTime = 0f;
			do
			{
				t.localScale = Vector3.Lerp(bouncedScale, originalScale, currentTime * 3.3333333f);
				currentTime += Time.deltaTime;
				yield return null;
			}
			while (currentTime <= 0.3f);
			yield break;
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x00125C9F File Offset: 0x00123E9F
		public void RegisterFocusableLayer(FocusableLayer focusableLayer)
		{
			AsmoLogger.Trace("UINavigationManager", "RegisterFocusableLayer " + focusableLayer.gameObject.name, null);
			this._focusableLayers.Add(focusableLayer);
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x00125CCD File Offset: 0x00123ECD
		public void UnRegisterFocusableLayer(FocusableLayer focusableLayer)
		{
			AsmoLogger.Trace("UINavigationManager", "UnRegisterFocusableLayer " + focusableLayer.gameObject.name, null);
			this._focusableLayers.Remove(focusableLayer);
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x00125CFC File Offset: 0x00123EFC
		public void MoveFocus(UINavigationManager.Direction direction)
		{
			if (this._focusables.Count == 0)
			{
				return;
			}
			Focusable focusable = null;
			if (this._currentFocusable == null)
			{
				foreach (Focusable focusable2 in this._focusables)
				{
					if (focusable2.firstFocusable)
					{
						focusable = focusable2;
						break;
					}
				}
				if (focusable == null)
				{
					focusable = this._focusables.First<Focusable>();
				}
			}
			else
			{
				if (direction == UINavigationManager.Direction.Next)
				{
					focusable = this._FindNextFocusableOf(this._currentFocusable);
				}
				else if (direction == UINavigationManager.Direction.Previous)
				{
					focusable = this._FindPreviousFocusableOf(this._currentFocusable);
				}
				if (focusable == null && !this._isEditingInputField)
				{
					focusable = this._FindClosestFocusable(this._currentFocusable, direction);
					if (focusable == null)
					{
						focusable = this._currentFocusable;
					}
				}
			}
			if (focusable != null)
			{
				if (focusable != this._currentFocusable)
				{
					this._UpdateFocus(focusable);
					return;
				}
				this.highlightFocusable(focusable);
			}
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x00125E08 File Offset: 0x00124008
		private Focusable _FindNextFocusableOf(Focusable currentFocusable)
		{
			if (currentFocusable == null)
			{
				return null;
			}
			Focusable next = currentFocusable.next;
			while (next != null && next != currentFocusable)
			{
				if (next.isActiveAndEnabled && next.Selectable.IsInteractable())
				{
					return next;
				}
				next = next.next;
			}
			return next;
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x00125E5C File Offset: 0x0012405C
		private Focusable _FindPreviousFocusableOf(Focusable currentFocusable)
		{
			if (currentFocusable == null)
			{
				return null;
			}
			Focusable previous = currentFocusable.previous;
			while (previous != null && previous != currentFocusable)
			{
				if (previous.isActiveAndEnabled && previous.Selectable.IsInteractable())
				{
					return previous;
				}
				previous = previous.previous;
			}
			return previous;
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x00125EB0 File Offset: 0x001240B0
		public void FindFocusableInputFieldAndEnterEditMode()
		{
			if (this._IsInputField(this._currentFocusable))
			{
				if (!this._isEditingInputField)
				{
					this._ActivateInputField();
				}
				return;
			}
			foreach (Focusable focusable in this._focusables)
			{
				if (this._IsInputField(focusable))
				{
					this._isEditingInputField = true;
					this._UpdateFocus(focusable);
					break;
				}
			}
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x00125F34 File Offset: 0x00124134
		public void HandleInputString(string inputString)
		{
			if (this._IsInputField(this._currentFocusable) && !this._isEditingInputField)
			{
				this._ActivateInputField();
			}
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x00125F54 File Offset: 0x00124154
		public void CancelCurrentContext()
		{
			if (this.HasFocus)
			{
				if (this._isEditingInputField)
				{
					this._DeactivateInputField();
					return;
				}
				this.LoseFocus();
				return;
			}
			else
			{
				for (int i = this._focusableLayers.Count - 1; i >= 0; i--)
				{
					FocusableLayer focusableLayer = this._focusableLayers[i];
					if (focusableLayer.OnBackAction != null)
					{
						focusableLayer.OnBackAction.Invoke();
						break;
					}
					if (focusableLayer.modal)
					{
						break;
					}
				}
				Action onBackAction = this.OnBackAction;
				if (onBackAction == null)
				{
					return;
				}
				onBackAction();
				return;
			}
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x00125FD4 File Offset: 0x001241D4
		public void ValidateFocusable()
		{
			if (!this.HasFocus)
			{
				this.FindFocusableInputFieldAndEnterEditMode();
				return;
			}
			if (!this._IsInputField(this._currentFocusable))
			{
				ExecuteEvents.Execute<ISubmitHandler>(this._currentFocusable.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
				return;
			}
			if (!this._isEditingInputField)
			{
				this._ActivateInputField();
				return;
			}
			this._DeactivateInputField();
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06003B30 RID: 15152 RVA: 0x00126034 File Offset: 0x00124234
		public bool HasFocus
		{
			get
			{
				return this._currentFocusable != null;
			}
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x00126042 File Offset: 0x00124242
		public void LoseFocus()
		{
			AsmoLogger.Trace("UINavigationManager", "LoseFocus", null);
			this._isEditingInputField = false;
			this._UpdateFocus(null);
			EventSystem.current.SetSelectedGameObject(null);
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x00126070 File Offset: 0x00124270
		private void Update()
		{
			EventSystem current = EventSystem.current;
			if (current == null)
			{
				return;
			}
			current.sendNavigationEvents = false;
			this._focusables.Clear();
			for (int i = this._focusableLayers.Count - 1; i >= 0; i--)
			{
				FocusableLayer focusableLayer = this._focusableLayers[i];
				this._focusables.AddRange(focusableLayer.Focusables.ToList<Focusable>());
				if (focusableLayer.modal)
				{
					break;
				}
			}
			if (this.HasFocus && !this._focusables.Contains(this._currentFocusable))
			{
				this.LoseFocus();
				this.MoveFocus(UINavigationManager.Direction.Next);
			}
			Focusable focusable = (current.currentSelectedGameObject != null) ? current.currentSelectedGameObject.GetComponent<Focusable>() : null;
			if (focusable != this._currentFocusable)
			{
				if (focusable == null)
				{
					this._UpdateFocus(this._currentFocusable);
				}
				else
				{
					if (this._IsInputField(focusable))
					{
						this._isEditingInputField = true;
					}
					this._UpdateFocus(focusable);
				}
			}
			UINavigationInput uinavigationInput = this.navigationInput;
			if (uinavigationInput != null)
			{
				uinavigationInput.ProcessInput(this);
			}
			if (!this.HasFocus && this.forceToGiveFocus())
			{
				this.MoveFocus(UINavigationManager.Direction.Next);
			}
			current.SetSelectedGameObject((this._currentFocusable != null) ? this._currentFocusable.gameObject : null);
			if (this._IsInputField(this._currentFocusable))
			{
				if (!this._isEditingInputField)
				{
					this._DeactivateInputField();
					return;
				}
			}
			else
			{
				this._isEditingInputField = false;
			}
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x001261D7 File Offset: 0x001243D7
		private bool _IsInputField(Focusable focusable)
		{
			return !(focusable == null) && focusable.IsInputField;
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x001261EC File Offset: 0x001243EC
		private void _ActivateInputField()
		{
			this._isEditingInputField = true;
			if (this._currentFocusable.InputField != null)
			{
				if (!this._currentFocusable.InputField.isFocused)
				{
					this._currentFocusable.InputField.ActivateInputField();
					return;
				}
			}
			else if (this._currentFocusable.TMP_InputField != null && !this._currentFocusable.TMP_InputField.isFocused)
			{
				this._currentFocusable.TMP_InputField.ActivateInputField();
			}
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x0012626C File Offset: 0x0012446C
		private void _DeactivateInputField()
		{
			this._isEditingInputField = false;
			if (this._currentFocusable.InputField != null)
			{
				if (this._currentFocusable.InputField.isFocused)
				{
					this._currentFocusable.InputField.DeactivateInputField();
					return;
				}
			}
			else if (this._currentFocusable.TMP_InputField != null && this._currentFocusable.TMP_InputField.isFocused)
			{
				this._currentFocusable.TMP_InputField.DeactivateInputField(false);
			}
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x001262EC File Offset: 0x001244EC
		private void _UpdateFocus(Focusable focusable)
		{
			this._currentFocusable = focusable;
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x001262F8 File Offset: 0x001244F8
		private Focusable _FindClosestFocusable(Focusable origin, UINavigationManager.Direction direction)
		{
			List<UINavigationManager.FocusablePosition> positions = this._ComputeFocusablePositions(origin);
			List<UINavigationManager.FocusablePosition> list = this._TrimAndSortPositions(positions, direction);
			if (list.Count <= 0)
			{
				return null;
			}
			return list.First<UINavigationManager.FocusablePosition>().focusable;
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x0012632C File Offset: 0x0012452C
		private List<UINavigationManager.FocusablePosition> _TrimAndSortPositions(List<UINavigationManager.FocusablePosition> positions, UINavigationManager.Direction direction)
		{
			List<UINavigationManager.FocusablePosition> result = null;
			if (direction == UINavigationManager.Direction.Left)
			{
				result = (from p in positions
				where p.deltaPosition.x < -0.001f
				orderby p.squaredDistance * Mathf.Max(1f, Mathf.Abs(this.directionalWeight * p.deltaPosition.y / p.deltaPosition.x))
				select p).ToList<UINavigationManager.FocusablePosition>();
			}
			else if (direction == UINavigationManager.Direction.Up || direction == UINavigationManager.Direction.Previous)
			{
				result = (from p in positions
				where p.deltaPosition.y > 0.001f
				orderby p.squaredDistance * Mathf.Max(1f, Mathf.Abs(this.directionalWeight * p.deltaPosition.x / p.deltaPosition.y))
				select p).ToList<UINavigationManager.FocusablePosition>();
			}
			else if (direction == UINavigationManager.Direction.Right)
			{
				result = (from p in positions
				where p.deltaPosition.x > 0.001f
				orderby p.squaredDistance * Mathf.Max(1f, Mathf.Abs(this.directionalWeight * p.deltaPosition.y / p.deltaPosition.x))
				select p).ToList<UINavigationManager.FocusablePosition>();
			}
			else if (direction == UINavigationManager.Direction.Down || direction == UINavigationManager.Direction.Next)
			{
				result = (from p in positions
				where p.deltaPosition.y < -0.001f
				orderby p.squaredDistance * Mathf.Max(1f, Mathf.Abs(this.directionalWeight * p.deltaPosition.x / p.deltaPosition.y))
				select p).ToList<UINavigationManager.FocusablePosition>();
			}
			return result;
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x00126450 File Offset: 0x00124650
		private List<UINavigationManager.FocusablePosition> _ComputeFocusablePositions(Focusable origin)
		{
			List<UINavigationManager.FocusablePosition> list = new List<UINavigationManager.FocusablePosition>();
			Vector2 viewportPosition = origin.ViewportPosition;
			foreach (Focusable focusable in this._focusables)
			{
				if (focusable.isActiveAndEnabled && focusable.Selectable.IsInteractable() && focusable != origin)
				{
					Vector2 deltaPosition = focusable.ViewportPosition - viewportPosition;
					list.Add(new UINavigationManager.FocusablePosition(focusable, deltaPosition, deltaPosition.sqrMagnitude));
				}
			}
			return list;
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x001264EC File Offset: 0x001246EC
		private void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this._currentFocusable == null || !this._currentFocusable.isActiveAndEnabled || !this._currentFocusable.Selectable.IsInteractable())
			{
				return;
			}
			Focusable focusable = this._FindClosestFocusable(this._currentFocusable, UINavigationManager.Direction.Left);
			Focusable focusable2 = this._FindClosestFocusable(this._currentFocusable, UINavigationManager.Direction.Right);
			Focusable focusable3 = this._FindClosestFocusable(this._currentFocusable, UINavigationManager.Direction.Up);
			Focusable focusable4 = this._FindClosestFocusable(this._currentFocusable, UINavigationManager.Direction.Down);
			Focusable focusable5 = this._FindNextFocusableOf(this._currentFocusable);
			Focusable focusable6 = this._FindPreviousFocusableOf(this._currentFocusable);
			if (this.gizmosLevel != UINavigationManager.GizmosLevel.Minimalist)
			{
				List<UINavigationManager.FocusablePosition> positions = this._ComputeFocusablePositions(this._currentFocusable);
				UINavigationManager.Direction[] array2;
				if (this.gizmosLevel != UINavigationManager.GizmosLevel.HorizontalGradient)
				{
					UINavigationManager.Direction[] array = new UINavigationManager.Direction[2];
					array[0] = UINavigationManager.Direction.Up;
					array2 = array;
					array[1] = UINavigationManager.Direction.Down;
				}
				else
				{
					(array2 = new UINavigationManager.Direction[2])[1] = UINavigationManager.Direction.Right;
				}
				foreach (UINavigationManager.Direction direction in array2)
				{
					List<UINavigationManager.FocusablePosition> list = this._TrimAndSortPositions(positions, direction);
					float num = 0f;
					float num2 = 0.67f / (float)list.Count;
					foreach (UINavigationManager.FocusablePosition ptr in list)
					{
						Gizmos.color = Color.HSVToRGB(num, 1f, 1f);
						Gizmos.DrawSphere(ptr.focusable.transform.position, 5f);
						num += num2;
					}
				}
			}
			Gizmos.color = Color.red;
			uint width = (this.gizmosLevel != UINavigationManager.GizmosLevel.Minimalist) ? 3U : 2U;
			if (focusable5 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable5.transform.position, width);
			}
			if (focusable6 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable6.transform.position, width);
			}
			Gizmos.color = Color.blue;
			width = ((this.gizmosLevel != UINavigationManager.GizmosLevel.Minimalist) ? 2U : 1U);
			if (focusable != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable.transform.position, width);
			}
			if (focusable2 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable2.transform.position, width);
			}
			if (focusable3 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable3.transform.position, width);
			}
			if (focusable4 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable4.transform.position, width);
			}
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x001267AC File Offset: 0x001249AC
		private static void _GizmosDrawLine(Vector3 from, Vector3 to, uint width)
		{
			if (width == 1U)
			{
				Gizmos.DrawLine(from, to);
				return;
			}
			Camera current = Camera.current;
			if (current == null)
			{
				return;
			}
			Vector3 normalized = (to - from).normalized;
			Vector3 normalized2 = (current.transform.position - from).normalized;
			Vector3 a = Vector3.Cross(normalized, normalized2);
			float num = (1f - width) * 0.5f;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)width))
			{
				Vector3 b = a * num;
				Gizmos.DrawLine(from + b, to + b);
				num += 0.5f;
				num2++;
			}
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x00126850 File Offset: 0x00124A50
		public void BeginIgnoringInteractionEvents(string requester)
		{
			this._ignoringInteractionEventsRequestCount++;
			AsmoLogger.Trace("UINavigationManager", "Request to IGNORE interaction events", new Hashtable
			{
				{
					"requester",
					requester
				},
				{
					"request count",
					this._ignoringInteractionEventsRequestCount
				}
			});
			this._AllowOrIgnoreInteractionEvents();
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x001268A8 File Offset: 0x00124AA8
		public void EndIgnoringInteractionEvents(string requester)
		{
			this._ignoringInteractionEventsRequestCount--;
			AsmoLogger.Trace("UINavigationManager", "Request to ALLOW interaction events", new Hashtable
			{
				{
					"requester",
					requester
				},
				{
					"request count",
					this._ignoringInteractionEventsRequestCount
				}
			});
			this._AllowOrIgnoreInteractionEvents();
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x00126900 File Offset: 0x00124B00
		private void _AllowOrIgnoreInteractionEvents()
		{
			UINavigationManager.<>c__DisplayClass50_0 CS$<>8__locals1 = new UINavigationManager.<>c__DisplayClass50_0();
			bool flag = this._ignoringInteractionEventsRequestCount == 0;
			CS$<>8__locals1.eventSystems = (UnityEngine.Object.FindObjectsOfType(typeof(EventSystem)) as EventSystem[]);
			int idx;
			int idx2;
			for (idx = 0; idx < CS$<>8__locals1.eventSystems.Length; idx = idx2 + 1)
			{
				EventSystem eventSystem = CS$<>8__locals1.eventSystems[idx];
				if (eventSystem.enabled != flag)
				{
					string msg = flag ? "Allow Interaction Events [{0}/{1}]" : "Ignore Interaction Events [{0}/{1}]";
					AsmoLogger.Debug("UINavigationManager", () => string.Format(msg, idx + 1, CS$<>8__locals1.eventSystems.Length), null);
					eventSystem.enabled = flag;
				}
				idx2 = idx;
			}
		}

		// Token: 0x0400264C RID: 9804
		private const string _documentation = "<b>UINavigationManager</b> works with <b>Focusable</b> and <b>Selectable</b> to provide relevant UI Navigation on all platforms";

		// Token: 0x0400264D RID: 9805
		private const string _kModuleName = "UINavigationManager";

		// Token: 0x0400264E RID: 9806
		private FocusableLayer _rootFocusableLayer;

		// Token: 0x0400264F RID: 9807
		private List<FocusableLayer> _focusableLayers = new List<FocusableLayer>();

		// Token: 0x04002650 RID: 9808
		private Focusable _currentFocusable;

		// Token: 0x04002651 RID: 9809
		private List<Focusable> _focusables = new List<Focusable>();

		// Token: 0x04002652 RID: 9810
		private bool _isEditingInputField;

		// Token: 0x04002654 RID: 9812
		public UINavigationManager.ForceToGiveFocus forceToGiveFocus;

		// Token: 0x04002655 RID: 9813
		public UINavigationManager.HighlightFocusable highlightFocusable;

		// Token: 0x04002656 RID: 9814
		public UINavigationInput navigationInput;

		// Token: 0x04002657 RID: 9815
		[Range(0f, 10f)]
		public float directionalWeight = 2f;

		// Token: 0x04002658 RID: 9816
		public UINavigationManager.GizmosLevel gizmosLevel;

		// Token: 0x04002659 RID: 9817
		private int _ignoringInteractionEventsRequestCount;

		// Token: 0x02000946 RID: 2374
		// (Invoke) Token: 0x0600475A RID: 18266
		public delegate bool ForceToGiveFocus();

		// Token: 0x02000947 RID: 2375
		// (Invoke) Token: 0x0600475E RID: 18270
		public delegate void HighlightFocusable(Focusable focusable);

		// Token: 0x02000948 RID: 2376
		public enum Direction
		{
			// Token: 0x0400311E RID: 12574
			Left,
			// Token: 0x0400311F RID: 12575
			Up,
			// Token: 0x04003120 RID: 12576
			Right,
			// Token: 0x04003121 RID: 12577
			Down,
			// Token: 0x04003122 RID: 12578
			Next,
			// Token: 0x04003123 RID: 12579
			Previous
		}

		// Token: 0x02000949 RID: 2377
		private struct FocusablePosition
		{
			// Token: 0x06004761 RID: 18273 RVA: 0x0014959E File Offset: 0x0014779E
			public FocusablePosition(Focusable focusable, Vector2 deltaPosition, float squaredDistance)
			{
				this.focusable = focusable;
				this.deltaPosition = deltaPosition;
				this.squaredDistance = squaredDistance;
			}

			// Token: 0x04003124 RID: 12580
			public Focusable focusable;

			// Token: 0x04003125 RID: 12581
			public Vector2 deltaPosition;

			// Token: 0x04003126 RID: 12582
			public float squaredDistance;
		}

		// Token: 0x0200094A RID: 2378
		public enum GizmosLevel
		{
			// Token: 0x04003128 RID: 12584
			Minimalist,
			// Token: 0x04003129 RID: 12585
			HorizontalGradient,
			// Token: 0x0400312A RID: 12586
			VerticalGradient
		}
	}
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200012F RID: 303
public class TransformMap : MonoBehaviour
{
	// Token: 0x06000BAA RID: 2986 RVA: 0x00003022 File Offset: 0x00001222
	private void Awake()
	{
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00052D71 File Offset: 0x00050F71
	private void OnEnable()
	{
		this.m_bApplyMomentum = false;
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00003022 File Offset: 0x00001222
	private void OnDestroy()
	{
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00052D7A File Offset: 0x00050F7A
	public bool GetIsAnimating()
	{
		return this.m_bIsAnimating;
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00052D82 File Offset: 0x00050F82
	public void OnDragStart()
	{
		if (Input.touchCount > 1)
		{
			return;
		}
		if (AgricolaLib.GetIsTutorialGame() && Tutorial.s_CurrentTutorialIndex < 2)
		{
			return;
		}
		this.m_lastMousePos = Input.mousePosition;
		this.m_bIsDragging = true;
		this.m_bApplyMomentum = false;
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00052DB8 File Offset: 0x00050FB8
	public void OnDrag()
	{
		if (!this.m_bIsDragging || Input.touchCount > 1)
		{
			return;
		}
		Vector3 vector = Input.mousePosition - this.m_lastMousePos;
		Vector3 vector2 = new Vector3(vector.x, vector.y, 0f);
		if (vector2 == Vector3.zero)
		{
			return;
		}
		RaycastHit raycastHit;
		Physics.Raycast(this.m_camera.ScreenPointToRay(Input.mousePosition), out raycastHit);
		Vector3 a = 1f * base.transform.InverseTransformPoint(raycastHit.point);
		Physics.Raycast(this.m_camera.ScreenPointToRay(this.m_lastMousePos), out raycastHit);
		Vector3 b = 1f * base.transform.InverseTransformPoint(raycastHit.point);
		Vector3 vector3 = a - b;
		this.m_dragMultiplier = 1f / (vector.magnitude / vector3.magnitude) * base.transform.localScale.x;
		vector2 *= this.m_dragMultiplier;
		if (this.m_bIgnoreXScroll)
		{
			vector2.x = 0f;
		}
		if (float.IsNaN(vector2.x) || float.IsNaN(vector2.y) || float.IsNaN(vector2.z))
		{
			Debug.LogWarning("TransformMap: bad touch data!");
			return;
		}
		Vector3 localPosition = base.transform.localPosition + vector2;
		base.transform.localPosition = localPosition;
		this.m_lastMousePos = Input.mousePosition;
		this.m_lastMapDiff = vector2;
		this.m_mapAtAutoPosition = -1;
		this.m_mapAtAutoCounrty = string.Empty;
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00052F42 File Offset: 0x00051142
	public void OnDragEnd()
	{
		this.m_bIsDragging = false;
		if (this.m_lastMousePos != Input.mousePosition && Input.touchCount < 2)
		{
			this.m_bApplyMomentum = true;
		}
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x00052F6C File Offset: 0x0005116C
	public int GetAutoPosition()
	{
		return this.m_mapAtAutoPosition;
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00052F74 File Offset: 0x00051174
	public void AutoAnimateToPosition(Vector2 newPosition, float animTime = -1f)
	{
		Vector3 newPosition2 = new Vector3(newPosition.x, newPosition.y, base.transform.localPosition.z);
		float totalMoveTime = (animTime >= 0f) ? animTime : this.m_autoAnimateTime;
		base.StartCoroutine(this.AnimateMapPosition(base.transform.localPosition, newPosition2, totalMoveTime));
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00052FD0 File Offset: 0x000511D0
	public void AutoAnimateToCountry(string countryName)
	{
		if (this.m_mapAtAutoCounrty == countryName || this.m_countryContainerObject == null)
		{
			return;
		}
		Transform transform = this.m_countryContainerObject.transform.Find(countryName);
		if (transform == null)
		{
			Debug.LogError("TransformMap: Unable to find country with name: " + countryName);
			return;
		}
		float x = base.gameObject.transform.localScale.x;
		float maxZoom = this.m_maxZoom;
		float autoAnimateTime = this.m_autoAnimateTime;
		Vector3 newPosition = transform.localPosition * -1f;
		base.StartCoroutine(this.AnimateMapPosition(base.transform.localPosition, newPosition, autoAnimateTime));
		base.StartCoroutine(this.AnimateMapScale(x, maxZoom, autoAnimateTime));
		this.m_mapAtAutoCounrty = countryName;
		this.m_mapAtAutoPosition = -1;
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00053098 File Offset: 0x00051298
	public void ForceMoveMap(Vector3 move)
	{
		if (!this.m_bIsDragging && base.gameObject.activeInHierarchy)
		{
			this.m_bApplyMomentum = false;
			this.m_lastMapDiff = Vector3.zero;
			Vector3 localPosition = base.transform.localPosition + move;
			base.transform.localPosition = localPosition;
			this.CheckPosition();
		}
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x000530F0 File Offset: 0x000512F0
	public void AutoAnimateToRegion(int regionIndex, bool bAllowZoomOut = true)
	{
		if (regionIndex < 0 || regionIndex >= this.m_autoAnimateRegions.Length)
		{
			return;
		}
		float x = base.gameObject.transform.localScale.x;
		float newScale = this.m_autoAnimateRegions[regionIndex].zoom;
		float autoAnimateTime = this.m_autoAnimateTime;
		if (this.m_mapAtAutoPosition == regionIndex && bAllowZoomOut)
		{
			newScale = this.m_minZoom;
			base.StartCoroutine(this.AnimateMapPosition(base.transform.localPosition, Vector3.zero, autoAnimateTime));
			base.StartCoroutine(this.AnimateMapScale(x, newScale, autoAnimateTime));
			this.m_mapAtAutoPosition = -1;
		}
		else
		{
			base.StartCoroutine(this.AnimateMapPosition(base.transform.localPosition, this.m_autoAnimateRegions[regionIndex].position, autoAnimateTime));
			base.StartCoroutine(this.AnimateMapScale(x, newScale, autoAnimateTime));
			this.m_mapAtAutoPosition = regionIndex;
		}
		this.m_mapAtAutoCounrty = string.Empty;
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x000531D4 File Offset: 0x000513D4
	public void AutoAnimateToZoomOut()
	{
		this.m_mapAtAutoPosition = -1;
		this.m_mapAtAutoCounrty = string.Empty;
		float x = base.gameObject.transform.localScale.x;
		float num = this.m_maxZoom - this.m_minZoom;
		float num2 = (x - this.m_minZoom) / num;
		float minZoom = this.m_minZoom;
		float totalMoveTime = num2 * this.m_fullZoomTime;
		base.StartCoroutine(this.AnimateMapPosition(base.transform.localPosition, Vector3.zero, totalMoveTime));
		base.StartCoroutine(this.AnimateMapScale(x, minZoom, totalMoveTime));
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00053260 File Offset: 0x00051460
	public void OnDoubleClick(BaseEventData baseData)
	{
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00053270 File Offset: 0x00051470
	private void Update()
	{
		if (this.m_bApplyMomentum)
		{
			this.m_lastMapDiff *= this.m_momentumReatinedPerFrame;
			if (this.m_lastMapDiff.magnitude >= this.m_minSpeedToContinueMomentum)
			{
				Vector3 localPosition = base.transform.localPosition + this.m_lastMapDiff;
				base.transform.localPosition = localPosition;
			}
			else
			{
				this.m_bApplyMomentum = false;
				this.m_lastMapDiff = Vector3.zero;
			}
		}
		this.CheckPosition();
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x000532EC File Offset: 0x000514EC
	private void ZoomInTick()
	{
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x000532FC File Offset: 0x000514FC
	private void ZoomOutTick()
	{
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x0000900B File Offset: 0x0000720B
	private bool IsInputCollisionOnMap()
	{
		return true;
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x0005330C File Offset: 0x0005150C
	private void CheckPosition()
	{
		bool flag = false;
		Vector3 position = base.transform.position;
		Vector3[] array = new Vector3[4];
		if (this.m_ScrollBoundingRect != null)
		{
			this.m_ScrollBoundingRect.GetWorldCorners(array);
		}
		else
		{
			base.GetComponent<RectTransform>().GetWorldCorners(array);
		}
		float x = this.m_camera.WorldToViewportPoint(array[0]).x;
		float y = this.m_camera.WorldToViewportPoint(array[0]).y;
		float x2 = this.m_camera.WorldToViewportPoint(array[2]).x;
		float y2 = this.m_camera.WorldToViewportPoint(array[2]).y;
		this.m_VisibleAreaRect.GetWorldCorners(array);
		this.m_leftStopViewportPosition = this.m_camera.WorldToViewportPoint(array[0]).x;
		this.m_bottomStopViewportPosition = this.m_camera.WorldToViewportPoint(array[0]).y;
		this.m_rightStopViewportPosition = this.m_camera.WorldToViewportPoint(array[2]).x;
		this.m_topStopViewportPosition = this.m_camera.WorldToViewportPoint(array[2]).y;
		float num = Mathf.Abs(x2 - x);
		this.m_bIgnoreXScroll = false;
		if (num < this.m_rightStopViewportPosition - this.m_leftStopViewportPosition)
		{
			Vector3 position2 = this.m_camera.WorldToViewportPoint(position);
			position2.x = (this.m_rightStopViewportPosition + this.m_leftStopViewportPosition) / 2f;
			position = this.m_camera.ViewportToWorldPoint(position2);
			flag = true;
			this.m_bIgnoreXScroll = true;
		}
		else if (x > this.m_leftStopViewportPosition && x2 >= this.m_rightStopViewportPosition)
		{
			Vector3 position3 = this.m_camera.WorldToViewportPoint(position);
			position3.x -= x - this.m_leftStopViewportPosition;
			position = this.m_camera.ViewportToWorldPoint(position3);
			flag = true;
		}
		else if (x2 < this.m_rightStopViewportPosition && x <= this.m_leftStopViewportPosition)
		{
			Vector3 position4 = this.m_camera.WorldToViewportPoint(position);
			position4.x += this.m_rightStopViewportPosition - x2;
			position = this.m_camera.ViewportToWorldPoint(position4);
			flag = true;
		}
		if (y > this.m_bottomStopViewportPosition)
		{
			Vector3 position5 = this.m_camera.WorldToViewportPoint(position);
			position5.y -= y - this.m_bottomStopViewportPosition;
			position = this.m_camera.ViewportToWorldPoint(position5);
			flag = true;
		}
		else if (y2 < this.m_topStopViewportPosition)
		{
			Vector3 position6 = this.m_camera.WorldToViewportPoint(position);
			position6.y += this.m_topStopViewportPosition - y2;
			position = this.m_camera.ViewportToWorldPoint(position6);
			flag = true;
		}
		if (flag)
		{
			base.transform.position = position;
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x000535B0 File Offset: 0x000517B0
	private IEnumerator AnimateCameraOrthoSize(float orthoSizeBegin, float orthoSizeEnd, float totalZoomTime)
	{
		float previousTime = Time.time;
		float currentAnimTime = 0f;
		float orthoSizeDelta = orthoSizeEnd - orthoSizeBegin;
		bool bAnimating = true;
		while (bAnimating)
		{
			currentAnimTime += Time.time - previousTime;
			previousTime = Time.time;
			if (currentAnimTime < totalZoomTime)
			{
				this.m_camera.orthographicSize = orthoSizeBegin + currentAnimTime / totalZoomTime * orthoSizeDelta;
			}
			else
			{
				this.m_camera.orthographicSize = orthoSizeEnd;
				bAnimating = false;
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x000535D4 File Offset: 0x000517D4
	private IEnumerator AnimateCameraPerspectiveFOV(float fovBegin, float fovEnd, float totalTime)
	{
		float previousTime = Time.time;
		float currentAnimTime = 0f;
		float fovDelta = fovEnd - fovBegin;
		bool bAnimating = true;
		while (bAnimating)
		{
			currentAnimTime += Time.time - previousTime;
			previousTime = Time.time;
			if (currentAnimTime < totalTime)
			{
				this.m_camera.fieldOfView = fovBegin + currentAnimTime / totalTime * fovDelta;
			}
			else
			{
				this.m_camera.fieldOfView = fovEnd;
				bAnimating = false;
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x000535F8 File Offset: 0x000517F8
	private IEnumerator AnimateMapPosition(Vector3 oldPosition, Vector3 newPosition, float totalMoveTime)
	{
		float previousTime = Time.time;
		float currentAnimTime = 0f;
		bool bAnimating = true;
		while (bAnimating)
		{
			this.m_bIsAnimating = true;
			currentAnimTime += Time.time - previousTime;
			previousTime = Time.time;
			if (currentAnimTime < totalMoveTime)
			{
				base.transform.localPosition = Vector3.Lerp(oldPosition, newPosition, currentAnimTime / totalMoveTime);
			}
			else
			{
				base.transform.localPosition = newPosition;
				bAnimating = false;
			}
			yield return new WaitForEndOfFrame();
		}
		this.m_bIsAnimating = false;
		yield break;
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0005361C File Offset: 0x0005181C
	private IEnumerator AnimateMapScale(float oldScale, float newScale, float totalMoveTime)
	{
		float previousTime = Time.time;
		float currentAnimTime = 0f;
		bool bAnimating = true;
		Vector3 oldScaleVec = new Vector3(oldScale, oldScale, 1f);
		Vector3 newScaleVec = new Vector3(newScale, newScale, 1f);
		while (bAnimating)
		{
			currentAnimTime += Time.time - previousTime;
			previousTime = Time.time;
			if (currentAnimTime < totalMoveTime)
			{
				base.transform.localScale = Vector3.Lerp(oldScaleVec, newScaleVec, currentAnimTime / totalMoveTime);
			}
			else
			{
				base.transform.localScale = newScaleVec;
				bAnimating = false;
			}
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x04000CAF RID: 3247
	public Camera m_camera;

	// Token: 0x04000CB0 RID: 3248
	public RectTransform m_ScrollBoundingRect;

	// Token: 0x04000CB1 RID: 3249
	public RectTransform m_VisibleAreaRect;

	// Token: 0x04000CB2 RID: 3250
	public float m_minZoom = 0.4f;

	// Token: 0x04000CB3 RID: 3251
	public float m_maxZoom = 1f;

	// Token: 0x04000CB4 RID: 3252
	public float m_ZoomSpeedPinch = 0.01f;

	// Token: 0x04000CB5 RID: 3253
	public float m_ZoomSpeedScrollWheel = 0.1f;

	// Token: 0x04000CB6 RID: 3254
	public float m_mobileSpeedMultiplierAtZoom;

	// Token: 0x04000CB7 RID: 3255
	public float m_fullZoomTime;

	// Token: 0x04000CB8 RID: 3256
	public TransformMap.AutoAnimatePosition[] m_autoAnimateRegions;

	// Token: 0x04000CB9 RID: 3257
	public GameObject m_countryContainerObject;

	// Token: 0x04000CBA RID: 3258
	public float m_autoAnimateTime;

	// Token: 0x04000CBB RID: 3259
	public float m_dragMultiplier = 1f;

	// Token: 0x04000CBC RID: 3260
	[Range(0f, 1f)]
	public float m_momentumReatinedPerFrame = 0.93f;

	// Token: 0x04000CBD RID: 3261
	public float m_minSpeedToContinueMomentum = 1f;

	// Token: 0x04000CBE RID: 3262
	private float m_leftStopViewportPosition;

	// Token: 0x04000CBF RID: 3263
	private float m_rightStopViewportPosition = 1f;

	// Token: 0x04000CC0 RID: 3264
	private float m_topStopViewportPosition = 1f;

	// Token: 0x04000CC1 RID: 3265
	private float m_bottomStopViewportPosition;

	// Token: 0x04000CC2 RID: 3266
	private Vector3 m_lastMousePos;

	// Token: 0x04000CC3 RID: 3267
	private int m_mapAtAutoPosition = -1;

	// Token: 0x04000CC4 RID: 3268
	private string m_mapAtAutoCounrty = string.Empty;

	// Token: 0x04000CC5 RID: 3269
	private bool m_bIgnoreXScroll;

	// Token: 0x04000CC6 RID: 3270
	private bool m_bIsDragging;

	// Token: 0x04000CC7 RID: 3271
	private bool m_bIsAnimating;

	// Token: 0x04000CC8 RID: 3272
	private Vector3 m_lastMapDiff = Vector3.zero;

	// Token: 0x04000CC9 RID: 3273
	private bool m_bApplyMomentum;

	// Token: 0x0200080B RID: 2059
	[Serializable]
	public struct AutoAnimatePosition
	{
		// Token: 0x04002DF9 RID: 11769
		public Vector3 position;

		// Token: 0x04002DFA RID: 11770
		public float zoom;
	}
}

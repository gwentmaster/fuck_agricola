using System;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001CA RID: 458
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/ScrollRectTweener")]
	public class ScrollRectTweener : MonoBehaviour, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06001184 RID: 4484 RVA: 0x0006D2AC File Offset: 0x0006B4AC
		private void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.wasHorizontal = this.scrollRect.horizontal;
			this.wasVertical = this.scrollRect.vertical;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0006D2DC File Offset: 0x0006B4DC
		public void ScrollHorizontal(float normalizedX)
		{
			this.Scroll(new Vector2(normalizedX, this.scrollRect.verticalNormalizedPosition));
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0006D2F5 File Offset: 0x0006B4F5
		public void ScrollHorizontal(float normalizedX, float duration)
		{
			this.Scroll(new Vector2(normalizedX, this.scrollRect.verticalNormalizedPosition), duration);
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0006D30F File Offset: 0x0006B50F
		public void ScrollVertical(float normalizedY)
		{
			this.Scroll(new Vector2(this.scrollRect.horizontalNormalizedPosition, normalizedY));
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0006D328 File Offset: 0x0006B528
		public void ScrollVertical(float normalizedY, float duration)
		{
			this.Scroll(new Vector2(this.scrollRect.horizontalNormalizedPosition, normalizedY), duration);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0006D342 File Offset: 0x0006B542
		public void Scroll(Vector2 normalizedPos)
		{
			this.Scroll(normalizedPos, this.GetScrollDuration(normalizedPos));
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0006D354 File Offset: 0x0006B554
		private float GetScrollDuration(Vector2 normalizedPos)
		{
			Vector2 currentPos = this.GetCurrentPos();
			return Vector2.Distance(this.DeNormalize(currentPos), this.DeNormalize(normalizedPos)) / this.moveSpeed;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0006D384 File Offset: 0x0006B584
		private Vector2 DeNormalize(Vector2 normalizedPos)
		{
			return new Vector2(normalizedPos.x * this.scrollRect.content.rect.width, normalizedPos.y * this.scrollRect.content.rect.height);
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0006D3D4 File Offset: 0x0006B5D4
		private Vector2 GetCurrentPos()
		{
			return new Vector2(this.scrollRect.horizontalNormalizedPosition, this.scrollRect.verticalNormalizedPosition);
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0006D3F1 File Offset: 0x0006B5F1
		public void Scroll(Vector2 normalizedPos, float duration)
		{
			this.startPos = this.GetCurrentPos();
			this.targetPos = normalizedPos;
			if (this.disableDragWhileTweening)
			{
				this.LockScrollability();
			}
			base.StopAllCoroutines();
			base.StartCoroutine(this.DoMove(duration));
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0006D428 File Offset: 0x0006B628
		private IEnumerator DoMove(float duration)
		{
			if (duration < 0.05f)
			{
				yield break;
			}
			Vector2 posOffset = this.targetPos - this.startPos;
			float currentTime = 0f;
			while (currentTime < duration)
			{
				currentTime += Time.deltaTime;
				this.scrollRect.normalizedPosition = this.EaseVector(currentTime, this.startPos, posOffset, duration);
				yield return null;
			}
			this.scrollRect.normalizedPosition = this.targetPos;
			if (this.disableDragWhileTweening)
			{
				this.RestoreScrollability();
			}
			yield break;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0006D440 File Offset: 0x0006B640
		public Vector2 EaseVector(float currentTime, Vector2 startValue, Vector2 changeInValue, float duration)
		{
			return new Vector2(changeInValue.x * Mathf.Sin(currentTime / duration * 1.5707964f) + startValue.x, changeInValue.y * Mathf.Sin(currentTime / duration * 1.5707964f) + startValue.y);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0006D48C File Offset: 0x0006B68C
		public void OnDrag(PointerEventData eventData)
		{
			if (!this.disableDragWhileTweening)
			{
				this.StopScroll();
			}
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0006D49C File Offset: 0x0006B69C
		private void StopScroll()
		{
			base.StopAllCoroutines();
			if (this.disableDragWhileTweening)
			{
				this.RestoreScrollability();
			}
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0006D4B2 File Offset: 0x0006B6B2
		private void LockScrollability()
		{
			this.scrollRect.horizontal = false;
			this.scrollRect.vertical = false;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0006D4CC File Offset: 0x0006B6CC
		private void RestoreScrollability()
		{
			this.scrollRect.horizontal = this.wasHorizontal;
			this.scrollRect.vertical = this.wasVertical;
		}

		// Token: 0x0400100E RID: 4110
		private ScrollRect scrollRect;

		// Token: 0x0400100F RID: 4111
		private Vector2 startPos;

		// Token: 0x04001010 RID: 4112
		private Vector2 targetPos;

		// Token: 0x04001011 RID: 4113
		private bool wasHorizontal;

		// Token: 0x04001012 RID: 4114
		private bool wasVertical;

		// Token: 0x04001013 RID: 4115
		public float moveSpeed = 5000f;

		// Token: 0x04001014 RID: 4116
		public bool disableDragWhileTweening;
	}
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000661 RID: 1633
	public static class Easing
	{
		// Token: 0x06003C40 RID: 15424 RVA: 0x0012A6B3 File Offset: 0x001288B3
		public static IEnumerator EaseFromTo(float from, float to, float duration, Easer easer, Action<float> easeMethod, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			float range = to - from;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				float obj = from + range * easer(elapsed / duration);
				easeMethod(obj);
				yield return 0;
			}
			easeMethod(to);
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x0012A6E7 File Offset: 0x001288E7
		public static IEnumerator EaseFromTo(Vector2 from, Vector2 to, float duration, Easer easer, Action<Vector2> easeMethod, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			Vector2 range = to - from;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				Vector2 obj = from + range * easer(elapsed / duration);
				easeMethod(obj);
				yield return 0;
			}
			easeMethod(to);
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x0012A71B File Offset: 0x0012891B
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Easer easer, Action actionAfterEasing)
		{
			float elapsed = 0f;
			Vector3 start = transform.localPosition;
			Vector3 range = target - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				if (transform != null)
				{
					transform.localPosition = start + range * easer(elapsed / duration);
				}
				yield return 0;
			}
			if (transform != null)
			{
				transform.localPosition = target;
			}
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x0012A747 File Offset: 0x00128947
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Easer ease, Action<object[]> actionAfterEasing = null, params object[] parameters)
		{
			float elapsed = 0f;
			Vector3 start = transform.localPosition;
			Vector3 range = target - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				if (transform != null)
				{
					transform.localPosition = start + range * ease(elapsed / duration);
				}
				yield return 0;
			}
			if (transform != null)
			{
				transform.localPosition = target;
			}
			if (actionAfterEasing != null)
			{
				actionAfterEasing(parameters);
			}
			yield break;
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x0012A77B File Offset: 0x0012897B
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration)
		{
			return transform.MoveTo(target, duration, Ease.Linear, null, Array.Empty<object>());
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x0012A790 File Offset: 0x00128990
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, EaseType ease, Action actionAfterEasing)
		{
			return transform.MoveTo(target, duration, Ease.FromType(ease), actionAfterEasing);
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x0012A7A2 File Offset: 0x001289A2
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, EaseType ease, Action<object[]> actionAfterEasing = null, params object[] parameters)
		{
			return transform.MoveTo(target, duration, Ease.FromType(ease), actionAfterEasing, parameters);
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x0012A7B8 File Offset: 0x001289B8
		public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, Easer ease)
		{
			Vector3 localPosition = transform.localPosition;
			transform.localPosition = target;
			return transform.MoveTo(localPosition, duration, ease, null, Array.Empty<object>());
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x0012A7E2 File Offset: 0x001289E2
		public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration)
		{
			return transform.MoveFrom(target, duration, Ease.Linear);
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x0012A7F1 File Offset: 0x001289F1
		public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, EaseType ease)
		{
			return transform.MoveFrom(target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x0012A801 File Offset: 0x00128A01
		public static IEnumerator ScaleLayoutTo(this LayoutElement layoutElement, float minWidth, float minHeight, float duration, Easer ease, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			Vector2 start = new Vector2(layoutElement.minWidth, layoutElement.minHeight);
			Vector2 range = new Vector2(minWidth, minHeight) - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				layoutElement.minWidth = start.x + range.x * ease(elapsed / duration);
				layoutElement.minHeight = start.y + range.y * ease(elapsed / duration);
				yield return 0;
			}
			layoutElement.minWidth = minWidth;
			layoutElement.minHeight = minHeight;
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x0012A835 File Offset: 0x00128A35
		public static IEnumerator ScaleRectransformTo(this RectTransform rectTransform, float width, float height, float duration, Easer ease, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			Vector2 start = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
			Vector2 range = new Vector2(width, height) - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, start.x + range.x * ease(elapsed / duration));
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, start.y + range.y * ease(elapsed / duration));
				yield return 0;
			}
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x0012A869 File Offset: 0x00128A69
		public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, Easer ease, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			Vector3 start = transform.localScale;
			Vector3 range = target - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				transform.localScale = start + range * ease(elapsed / duration);
				yield return 0;
			}
			transform.localScale = target;
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x0012A895 File Offset: 0x00128A95
		public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration)
		{
			return transform.ScaleTo(target, duration, Ease.Linear, null);
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x0012A8A5 File Offset: 0x00128AA5
		public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, EaseType ease, Action actionAfterEasing = null)
		{
			return transform.ScaleTo(target, duration, Ease.FromType(ease), actionAfterEasing);
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x0012A8B8 File Offset: 0x00128AB8
		public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration, Easer ease)
		{
			Vector3 localScale = transform.localScale;
			transform.localScale = target;
			return transform.ScaleTo(localScale, duration, ease, null);
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x0012A8DD File Offset: 0x00128ADD
		public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration)
		{
			return transform.ScaleFrom(target, duration, Ease.Linear);
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x0012A8EC File Offset: 0x00128AEC
		public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration, EaseType ease)
		{
			return transform.ScaleFrom(target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x0012A8FC File Offset: 0x00128AFC
		public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration, Easer ease)
		{
			float elapsed = 0f;
			Quaternion start = transform.localRotation;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				transform.localRotation = Quaternion.Lerp(start, target, ease(elapsed / duration));
				yield return 0;
			}
			transform.localRotation = target;
			yield break;
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x0012A920 File Offset: 0x00128B20
		public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration)
		{
			return transform.RotateTo(target, duration, Ease.Linear);
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x0012A92F File Offset: 0x00128B2F
		public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration, EaseType ease)
		{
			return transform.RotateTo(target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0012A940 File Offset: 0x00128B40
		public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration, Easer ease)
		{
			Quaternion localRotation = transform.localRotation;
			transform.localRotation = target;
			return transform.RotateTo(localRotation, duration, ease);
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x0012A964 File Offset: 0x00128B64
		public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration)
		{
			return transform.RotateFrom(target, duration, Ease.Linear);
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x0012A973 File Offset: 0x00128B73
		public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration, EaseType ease)
		{
			return transform.RotateFrom(target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x0012A983 File Offset: 0x00128B83
		public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration, Easer ease)
		{
			float elapsed = 0f;
			Vector3 start = transform.localPosition;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				float num = ease(elapsed / duration);
				Vector3 localPosition;
				localPosition.x = start.x * (1f - num) * (1f - num) + control.x * 2f * (1f - num) * num + target.x * num * num;
				localPosition.y = start.y * (1f - num) * (1f - num) + control.y * 2f * (1f - num) * num + target.y * num * num;
				localPosition.z = start.z * (1f - num) * (1f - num) + control.z * 2f * (1f - num) * num + target.z * num * num;
				transform.localPosition = localPosition;
				yield return 0;
			}
			transform.localPosition = target;
			yield break;
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x0012A9AF File Offset: 0x00128BAF
		public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration)
		{
			return transform.CurveTo(control, target, duration, Ease.Linear);
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x0012A9BF File Offset: 0x00128BBF
		public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration, EaseType ease)
		{
			return transform.CurveTo(control, target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x0012A9D4 File Offset: 0x00128BD4
		public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration, Easer ease)
		{
			Vector3 localPosition = transform.localPosition;
			transform.localPosition = start;
			return transform.CurveTo(control, localPosition, duration, ease);
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0012A9FA File Offset: 0x00128BFA
		public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration)
		{
			return transform.CurveFrom(control, start, duration, Ease.Linear);
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x0012AA0A File Offset: 0x00128C0A
		public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration, EaseType ease)
		{
			return transform.CurveFrom(control, start, duration, Ease.FromType(ease));
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x0012AA1C File Offset: 0x00128C1C
		public static IEnumerator Shake(this Transform transform, Vector3 amount, float duration)
		{
			Vector3 start = transform.localPosition;
			Vector3 shake = Vector3.zero;
			while (duration > 0f)
			{
				duration -= Time.deltaTime;
				shake.Set(UnityEngine.Random.Range(-amount.x, amount.x), UnityEngine.Random.Range(-amount.y, amount.y), UnityEngine.Random.Range(-amount.z, amount.z));
				transform.localPosition = start + shake;
				yield return 0;
			}
			transform.localPosition = start;
			yield break;
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x0012AA39 File Offset: 0x00128C39
		public static IEnumerator Shake(this Transform transform, float amount, float duration)
		{
			return transform.Shake(new Vector3(amount, amount, amount), duration);
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x0012AA4A File Offset: 0x00128C4A
		public static IEnumerator Wait(float duration)
		{
			while (duration > 0f)
			{
				duration -= Time.deltaTime;
				yield return 0;
			}
			yield break;
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x0012AA59 File Offset: 0x00128C59
		public static IEnumerator WaitUntil(Predicate predicate)
		{
			while (!predicate())
			{
				yield return 0;
			}
			yield break;
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x0012AA68 File Offset: 0x00128C68
		public static float Loop(float duration, float from, float to, float offsetPercent)
		{
			float num = to - from;
			float num2 = (Time.time + duration * offsetPercent) * (Mathf.Abs(num) / duration);
			if (num > 0f)
			{
				return from + Time.time - num * (float)Mathf.FloorToInt(Time.time / num);
			}
			return from - (Time.time - Mathf.Abs(num) * (float)Mathf.FloorToInt(num2 / Mathf.Abs(num)));
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x0012AAC9 File Offset: 0x00128CC9
		public static float Loop(float duration, float from, float to)
		{
			return Easing.Loop(duration, from, to, 0f);
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x0012AAD8 File Offset: 0x00128CD8
		public static Vector3 Loop(float duration, Vector3 from, Vector3 to, float offsetPercent)
		{
			return Vector3.Lerp(from, to, Easing.Loop(duration, 0f, 1f, offsetPercent));
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x0012AAF2 File Offset: 0x00128CF2
		public static Vector3 Loop(float duration, Vector3 from, Vector3 to)
		{
			return Vector3.Lerp(from, to, Easing.Loop(duration, 0f, 1f));
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0012AB0B File Offset: 0x00128D0B
		public static Quaternion Loop(float duration, Quaternion from, Quaternion to, float offsetPercent)
		{
			return Quaternion.Lerp(from, to, Easing.Loop(duration, 0f, 1f, offsetPercent));
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x0012AB25 File Offset: 0x00128D25
		public static Quaternion Loop(float duration, Quaternion from, Quaternion to)
		{
			return Quaternion.Lerp(from, to, Easing.Loop(duration, 0f, 1f));
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x0012AB40 File Offset: 0x00128D40
		public static float Wave(float duration, float from, float to, float offsetPercent)
		{
			float num = (to - from) / 2f;
			return from + num + Mathf.Sin((Time.time + duration * offsetPercent) / duration * 6.2831855f) * num;
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x0012AB73 File Offset: 0x00128D73
		public static float Wave(float duration, float from, float to)
		{
			return Easing.Wave(duration, from, to, 0f);
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x0012AB82 File Offset: 0x00128D82
		public static Vector3 Wave(float duration, Vector3 from, Vector3 to, float offsetPercent)
		{
			return Vector3.Lerp(from, to, Easing.Wave(duration, 0f, 1f, offsetPercent));
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x0012AB9C File Offset: 0x00128D9C
		public static Vector3 Wave(float duration, Vector3 from, Vector3 to)
		{
			return Vector3.Lerp(from, to, Easing.Wave(duration, 0f, 1f));
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x0012ABB5 File Offset: 0x00128DB5
		public static Quaternion Wave(float duration, Quaternion from, Quaternion to, float offsetPercent)
		{
			return Quaternion.Lerp(from, to, Easing.Wave(duration, 0f, 1f, offsetPercent));
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x0012ABCF File Offset: 0x00128DCF
		public static Quaternion Wave(float duration, Quaternion from, Quaternion to)
		{
			return Quaternion.Lerp(from, to, Easing.Wave(duration, 0f, 1f));
		}
	}
}

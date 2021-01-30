using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000205 RID: 517
	public class ScrollingCalendar : MonoBehaviour
	{
		// Token: 0x060012C6 RID: 4806 RVA: 0x00071708 File Offset: 0x0006F908
		private void InitializeYears()
		{
			int[] array = new int[int.Parse(DateTime.Now.ToString("yyyy")) + 1 - 1900];
			this.yearsButtons = new GameObject[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 1900 + i;
				GameObject gameObject = Object.Instantiate<GameObject>(this.yearsButtonPrefab, new Vector3(0f, (float)(i * 80), 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
				gameObject.transform.SetParent(this.yearsScrollingPanel, false);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.GetComponentInChildren<Text>().text = string.Concat(array[i]);
				gameObject.name = "Year_" + array[i];
				gameObject.AddComponent<CanvasGroup>();
				this.yearsButtons[i] = gameObject;
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00071810 File Offset: 0x0006FA10
		private void InitializeMonths()
		{
			int[] array = new int[12];
			this.monthsButtons = new GameObject[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string text = "";
				array[i] = i;
				GameObject gameObject = Object.Instantiate<GameObject>(this.monthsButtonPrefab, new Vector3(0f, (float)(i * 80), 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
				gameObject.transform.SetParent(this.monthsScrollingPanel, false);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				switch (i)
				{
				case 0:
					text = "Jan";
					break;
				case 1:
					text = "Feb";
					break;
				case 2:
					text = "Mar";
					break;
				case 3:
					text = "Apr";
					break;
				case 4:
					text = "May";
					break;
				case 5:
					text = "Jun";
					break;
				case 6:
					text = "Jul";
					break;
				case 7:
					text = "Aug";
					break;
				case 8:
					text = "Sep";
					break;
				case 9:
					text = "Oct";
					break;
				case 10:
					text = "Nov";
					break;
				case 11:
					text = "Dec";
					break;
				}
				gameObject.GetComponentInChildren<Text>().text = text;
				gameObject.name = "Month_" + array[i];
				gameObject.AddComponent<CanvasGroup>();
				this.monthsButtons[i] = gameObject;
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00071988 File Offset: 0x0006FB88
		private void InitializeDays()
		{
			int[] array = new int[31];
			this.daysButtons = new GameObject[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i + 1;
				GameObject gameObject = Object.Instantiate<GameObject>(this.daysButtonPrefab, new Vector3(0f, (float)(i * 80), 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
				gameObject.transform.SetParent(this.daysScrollingPanel, false);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.GetComponentInChildren<Text>().text = string.Concat(array[i]);
				gameObject.name = "Day_" + array[i];
				gameObject.AddComponent<CanvasGroup>();
				this.daysButtons[i] = gameObject;
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00071A70 File Offset: 0x0006FC70
		public void Awake()
		{
			this.InitializeYears();
			this.InitializeMonths();
			this.InitializeDays();
			this.monthsVerticalScroller = new UIVerticalScroller(this.monthsScrollingPanel, this.monthsButtons, this.monthCenter);
			this.yearsVerticalScroller = new UIVerticalScroller(this.yearsScrollingPanel, this.yearsButtons, this.yearsCenter);
			this.daysVerticalScroller = new UIVerticalScroller(this.daysScrollingPanel, this.daysButtons, this.daysCenter);
			this.monthsVerticalScroller.Start();
			this.yearsVerticalScroller.Start();
			this.daysVerticalScroller.Start();
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00071B08 File Offset: 0x0006FD08
		public void SetDate()
		{
			this.daysSet = int.Parse(this.inputFieldDays.text) - 1;
			this.monthsSet = int.Parse(this.inputFieldMonths.text) - 1;
			this.yearsSet = int.Parse(this.inputFieldYears.text) - 1900;
			this.daysVerticalScroller.SnapToElement(this.daysSet);
			this.monthsVerticalScroller.SnapToElement(this.monthsSet);
			this.yearsVerticalScroller.SnapToElement(this.yearsSet);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00071B94 File Offset: 0x0006FD94
		private void Update()
		{
			this.monthsVerticalScroller.Update();
			this.yearsVerticalScroller.Update();
			this.daysVerticalScroller.Update();
			string text = this.daysVerticalScroller.GetResults();
			string results = this.monthsVerticalScroller.GetResults();
			string results2 = this.yearsVerticalScroller.GetResults();
			if (text.EndsWith("1") && text != "11")
			{
				text += "st";
			}
			else if (text.EndsWith("2") && text != "12")
			{
				text += "nd";
			}
			else if (text.EndsWith("3") && text != "13")
			{
				text += "rd";
			}
			else
			{
				text += "th";
			}
			this.dateText.text = string.Concat(new string[]
			{
				results,
				" ",
				text,
				" ",
				results2
			});
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00071C9C File Offset: 0x0006FE9C
		public void DaysScrollUp()
		{
			this.daysVerticalScroller.ScrollUp();
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00071CA9 File Offset: 0x0006FEA9
		public void DaysScrollDown()
		{
			this.daysVerticalScroller.ScrollDown();
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00071CB6 File Offset: 0x0006FEB6
		public void MonthsScrollUp()
		{
			this.monthsVerticalScroller.ScrollUp();
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00071CC3 File Offset: 0x0006FEC3
		public void MonthsScrollDown()
		{
			this.monthsVerticalScroller.ScrollDown();
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00071CD0 File Offset: 0x0006FED0
		public void YearsScrollUp()
		{
			this.yearsVerticalScroller.ScrollUp();
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00071CDD File Offset: 0x0006FEDD
		public void YearsScrollDown()
		{
			this.yearsVerticalScroller.ScrollDown();
		}

		// Token: 0x040010F4 RID: 4340
		public RectTransform monthsScrollingPanel;

		// Token: 0x040010F5 RID: 4341
		public RectTransform yearsScrollingPanel;

		// Token: 0x040010F6 RID: 4342
		public RectTransform daysScrollingPanel;

		// Token: 0x040010F7 RID: 4343
		public GameObject yearsButtonPrefab;

		// Token: 0x040010F8 RID: 4344
		public GameObject monthsButtonPrefab;

		// Token: 0x040010F9 RID: 4345
		public GameObject daysButtonPrefab;

		// Token: 0x040010FA RID: 4346
		private GameObject[] monthsButtons;

		// Token: 0x040010FB RID: 4347
		private GameObject[] yearsButtons;

		// Token: 0x040010FC RID: 4348
		private GameObject[] daysButtons;

		// Token: 0x040010FD RID: 4349
		public RectTransform monthCenter;

		// Token: 0x040010FE RID: 4350
		public RectTransform yearsCenter;

		// Token: 0x040010FF RID: 4351
		public RectTransform daysCenter;

		// Token: 0x04001100 RID: 4352
		private UIVerticalScroller yearsVerticalScroller;

		// Token: 0x04001101 RID: 4353
		private UIVerticalScroller monthsVerticalScroller;

		// Token: 0x04001102 RID: 4354
		private UIVerticalScroller daysVerticalScroller;

		// Token: 0x04001103 RID: 4355
		public InputField inputFieldDays;

		// Token: 0x04001104 RID: 4356
		public InputField inputFieldMonths;

		// Token: 0x04001105 RID: 4357
		public InputField inputFieldYears;

		// Token: 0x04001106 RID: 4358
		public Text dateText;

		// Token: 0x04001107 RID: 4359
		private int daysSet;

		// Token: 0x04001108 RID: 4360
		private int monthsSet;

		// Token: 0x04001109 RID: 4361
		private int yearsSet;
	}
}

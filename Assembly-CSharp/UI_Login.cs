using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200011C RID: 284
public class UI_Login : MonoBehaviour
{
	// Token: 0x06000A97 RID: 2711 RVA: 0x000460E4 File Offset: 0x000442E4
	private void Start()
	{
		this.m_system = EventSystem.current;
		GameObject gameObject = GameObject.Find("/Network - Startup");
		if (gameObject != null)
		{
			this.m_NetworkAsmodee = gameObject.GetComponent<NetworkAsmodee>();
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0004611C File Offset: 0x0004431C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable selectable = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? this.m_system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp() : this.m_system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			if (selectable != null)
			{
				InputField component = selectable.GetComponent<InputField>();
				if (component != null)
				{
					component.OnPointerClick(new PointerEventData(this.m_system));
				}
			}
		}
		if (this.m_bWaitVerifyAsmodee_HaveBoth)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitVerifyAsmodee_HaveBoth = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultVerifyAsmodee_HaveBoth != null)
					{
						string input_text = "${LINK_FAIL_CONNECT_ASMODEE}";
						string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
						this.m_ResultVerifyAsmodee_HaveBoth.SetText(text);
					}
					this.Enable_HaveBoth_VerifyAsmodeeAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int verifyAsmodeeResult = this.m_NetworkAsmodee.GetVerifyAsmodeeResult();
					if (verifyAsmodeeResult != -1)
					{
						this.m_bWaitVerifyAsmodee_HaveBoth = false;
						this.m_ConnectWaitTime = 0f;
						if (verifyAsmodeeResult == 0)
						{
							if (this.m_ResultVerifyAsmodee_HaveBoth != null)
							{
								string verifyError = this.m_NetworkAsmodee.GetVerifyError();
								string text2 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(verifyError);
								this.m_ResultVerifyAsmodee_HaveBoth.SetText(text2);
							}
							this.Enable_HaveBoth_VerifyAsmodeeAccount(true);
						}
						else if (verifyAsmodeeResult == 3)
						{
							if (this.m_ResultVerifyAsmodee_HaveBoth != null)
							{
								string verifyError2 = this.m_NetworkAsmodee.GetVerifyError();
								string text3 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(verifyError2);
								this.m_ResultVerifyAsmodee_HaveBoth.SetText(text3);
							}
							this.Enable_HaveBoth_VerifyAsmodeeAccount(true);
						}
						else if (verifyAsmodeeResult == 1)
						{
							if (this.m_ResultVerifyAsmodee_HaveBoth != null)
							{
								string input_text2 = "${LINK_ACCOUNT_VERIFIED_ASMODEE}";
								string text4 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text2);
								this.m_ResultVerifyAsmodee_HaveBoth.SetText(text4);
							}
							this.Enable_HaveBoth_VerifyPlaydekAccount(true);
						}
					}
				}
			}
		}
		if (this.m_bWaitVerifyPlaydek_HaveBoth)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitVerifyPlaydek_HaveBoth = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultVerifyPlaydek_HaveBoth != null)
					{
						string input_text3 = "${LINK_FAIL_CONNECT_PLAYDEK}";
						string text5 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text3);
						this.m_ResultVerifyPlaydek_HaveBoth.SetText(text5);
					}
					this.Enable_HaveBoth_VerifyPlaydekAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int verifyPlaydekResult = this.m_NetworkAsmodee.GetVerifyPlaydekResult();
					if (verifyPlaydekResult != -1)
					{
						this.m_bWaitVerifyPlaydek_HaveBoth = false;
						this.m_ConnectWaitTime = 0f;
						if (verifyPlaydekResult == 0)
						{
							if (this.m_ResultVerifyPlaydek_HaveBoth != null)
							{
								string verifyError3 = this.m_NetworkAsmodee.GetVerifyError();
								string text6 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(verifyError3);
								this.m_ResultVerifyPlaydek_HaveBoth.SetText(text6);
							}
							this.Enable_HaveBoth_VerifyPlaydekAccount(true);
						}
						else if (verifyPlaydekResult == 3)
						{
							if (this.m_ResultVerifyPlaydek_HaveBoth != null)
							{
								string verifyError4 = this.m_NetworkAsmodee.GetVerifyError();
								string text7 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(verifyError4);
								this.m_ResultVerifyPlaydek_HaveBoth.SetText(text7);
							}
							this.Enable_HaveBoth_VerifyPlaydekAccount(true);
						}
						else if (verifyPlaydekResult == 1)
						{
							if (this.m_ResultVerifyPlaydek_HaveBoth != null)
							{
								string input_text4 = "${LINK_ACCOUNT_VERIFIED_PLAYDEK}";
								string text8 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text4);
								this.m_ResultVerifyPlaydek_HaveBoth.SetText(text8);
							}
							if (this.m_ResultLinkAccount_HaveBoth != null)
							{
								string input_text5 = "${LINK_ACCOUNTS_READY}";
								string text9 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text5);
								this.m_ResultLinkAccount_HaveBoth.SetText(text9);
							}
							this.Enable_HaveBoth_SubmitLinkAccount(true);
						}
					}
				}
			}
		}
		if (this.m_bWaitLinkAccount_HaveBoth)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitLinkAccount_HaveBoth = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultLinkAccount_HaveBoth != null)
					{
						string input_text6 = "${LINK_FAIL_CONNECT_PLAYDEK}";
						string text10 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text6);
						this.m_ResultLinkAccount_HaveBoth.SetText(text10);
					}
					this.Enable_HaveBoth_SubmitLinkAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int linkAccountResult = this.m_NetworkAsmodee.GetLinkAccountResult();
					if (linkAccountResult != -1)
					{
						this.m_bWaitLinkAccount_HaveBoth = false;
						this.m_ConnectWaitTime = 0f;
						if (linkAccountResult == 0)
						{
							if (this.m_ResultLinkAccount_HaveBoth != null)
							{
								string input_text7 = "${LINK_ACCOUNT_FAILED}";
								string text11 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text7);
								this.m_ResultLinkAccount_HaveBoth.SetText(text11);
							}
							this.Enable_HaveBoth_SubmitLinkAccount(true);
						}
						else if (linkAccountResult == 1)
						{
							if (this.m_ResultLinkAccount_HaveBoth != null)
							{
								string input_text8 = "${LINK_ACCOUNT_SUCCESS}";
								string text12 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text8);
								this.m_ResultLinkAccount_HaveBoth.SetText(text12);
							}
							if (this.m_usernamePrompt != null && this.m_VerifyAsmodeeLoginName_HaveBoth != null)
							{
								this.m_usernamePrompt.text = this.m_VerifyAsmodeeLoginName_HaveBoth.text;
							}
							if (this.m_passwordPrompt != null && this.m_VerifyAsmodeePassword_HaveBoth != null)
							{
								this.m_passwordPrompt.text = this.m_VerifyAsmodeePassword_HaveBoth.text;
							}
							this.Enable_HaveBoth_Completed(true);
							PlayerPrefs.SetInt("AsmodeeAccountLink", 1);
						}
					}
				}
			}
		}
		if (this.m_bWaitVerifyAsmodee_AsmodeeOnly)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitVerifyAsmodee_AsmodeeOnly = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultVerifyAsmodee_AsmodeeOnly != null)
					{
						string input_text9 = "${LINK_FAIL_CONNECT_ASMODEE}";
						string text13 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text9);
						this.m_ResultVerifyAsmodee_AsmodeeOnly.SetText(text13);
					}
					this.Enable_AsmodeeOnly_VerifyAsmodeeAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int verifyAsmodeeResult2 = this.m_NetworkAsmodee.GetVerifyAsmodeeResult();
					if (verifyAsmodeeResult2 != -1)
					{
						this.m_bWaitVerifyAsmodee_AsmodeeOnly = false;
						this.m_ConnectWaitTime = 0f;
						if (verifyAsmodeeResult2 == 0)
						{
							if (this.m_ResultVerifyAsmodee_AsmodeeOnly != null)
							{
								string verifyError5 = this.m_NetworkAsmodee.GetVerifyError();
								string text14 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(verifyError5);
								this.m_ResultVerifyAsmodee_AsmodeeOnly.SetText(text14);
							}
							this.Enable_AsmodeeOnly_VerifyAsmodeeAccount(true);
						}
						else if (verifyAsmodeeResult2 == 3)
						{
							if (this.m_ResultVerifyAsmodee_AsmodeeOnly != null)
							{
								string verifyError6 = this.m_NetworkAsmodee.GetVerifyError();
								string text15 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(verifyError6);
								this.m_ResultVerifyAsmodee_AsmodeeOnly.SetText(text15);
							}
							this.Enable_AsmodeeOnly_VerifyAsmodeeAccount(true);
						}
						else if (verifyAsmodeeResult2 == 1)
						{
							if (this.m_ResultVerifyAsmodee_AsmodeeOnly != null)
							{
								string input_text10 = "${LINK_ACCOUNT_VERIFIED_ASMODEE}";
								string text16 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text10);
								this.m_ResultVerifyAsmodee_AsmodeeOnly.SetText(text16);
							}
							if (this.m_ResultLinkAccount_AsmodeeOnly != null)
							{
								string input_text11 = "${LINK_ACCOUNTS_READY}";
								string text17 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text11);
								this.m_ResultLinkAccount_AsmodeeOnly.SetText(text17);
							}
							this.Enable_AsmodeeOnly_LinkPlaydekAccount(true);
						}
					}
				}
			}
		}
		if (this.m_bWaitLinkAccount_AsmodeeOnly)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitLinkAccount_AsmodeeOnly = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultLinkAccount_AsmodeeOnly != null)
					{
						string input_text12 = "${LINK_FAIL_CONNECT_PLAYEK}";
						string text18 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text12);
						this.m_ResultLinkAccount_AsmodeeOnly.SetText(text18);
					}
					this.Enable_AsmodeeOnly_LinkPlaydekAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int linkAccountResult2 = this.m_NetworkAsmodee.GetLinkAccountResult();
					if (linkAccountResult2 != -1)
					{
						this.m_bWaitLinkAccount_AsmodeeOnly = false;
						this.m_ConnectWaitTime = 0f;
						if (linkAccountResult2 == 0)
						{
							if (this.m_ResultLinkAccount_AsmodeeOnly != null)
							{
								string input_text13 = "${LINK_ACCOUNT_FAILED}";
								string text19 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text13);
								this.m_ResultLinkAccount_AsmodeeOnly.SetText(text19);
							}
							this.Enable_AsmodeeOnly_LinkPlaydekAccount(true);
						}
						else if (linkAccountResult2 == 1)
						{
							if (this.m_ResultLinkAccount_AsmodeeOnly != null)
							{
								string input_text14 = "${LINK_ACCOUNT_SUCCESS}";
								string text20 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text14);
								this.m_ResultLinkAccount_AsmodeeOnly.SetText(text20);
							}
							if (this.m_usernamePrompt != null && this.m_VerifyAsmodeeLoginName_AsmodeeOnly != null)
							{
								this.m_usernamePrompt.text = this.m_VerifyAsmodeeLoginName_AsmodeeOnly.text;
							}
							if (this.m_passwordPrompt != null && this.m_VerifyAsmodeePassword_AsmodeeOnly != null)
							{
								this.m_passwordPrompt.text = this.m_VerifyAsmodeePassword_AsmodeeOnly.text;
							}
							this.Enable_AsmodeeOnly_Completed(true);
							PlayerPrefs.SetInt("AsmodeeAccountLink", 1);
						}
					}
				}
			}
		}
		if (this.m_bWaitVerifyPlaydek_PlaydekOnly)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitVerifyPlaydek_PlaydekOnly = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultVerifyPlaydek_PlaydekOnly != null)
					{
						string input_text15 = "${LINK_FAIL_CONNECT_PLAYDEK}";
						string text21 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text15);
						this.m_ResultVerifyPlaydek_PlaydekOnly.SetText(text21);
					}
					this.Enable_PlaydekOnly_VerifyPlaydekAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int verifyPlaydekResult2 = this.m_NetworkAsmodee.GetVerifyPlaydekResult();
					if (verifyPlaydekResult2 != -1)
					{
						this.m_bWaitVerifyPlaydek_PlaydekOnly = false;
						this.m_ConnectWaitTime = 0f;
						if (verifyPlaydekResult2 == 0)
						{
							if (this.m_ResultVerifyPlaydek_PlaydekOnly != null)
							{
								string verifyError7 = this.m_NetworkAsmodee.GetVerifyError();
								string text22 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(verifyError7);
								this.m_ResultVerifyPlaydek_PlaydekOnly.SetText(text22);
							}
							this.Enable_PlaydekOnly_VerifyPlaydekAccount(true);
						}
						else if (verifyPlaydekResult2 == 3)
						{
							if (this.m_ResultVerifyPlaydek_PlaydekOnly != null)
							{
								string verifyError8 = this.m_NetworkAsmodee.GetVerifyError();
								string text23 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(verifyError8);
								this.m_ResultVerifyPlaydek_PlaydekOnly.SetText(text23);
							}
							this.Enable_PlaydekOnly_VerifyPlaydekAccount(true);
						}
						else if (verifyPlaydekResult2 == 1)
						{
							if (this.m_ResultVerifyPlaydek_PlaydekOnly != null)
							{
								string input_text16 = "${LINK_ACCOUNT_VERIFED_PLAYDEK}";
								string text24 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text16);
								this.m_ResultVerifyPlaydek_PlaydekOnly.SetText(text24);
							}
							if (this.m_VerifyPlaydekEmail_PlaydekOnly != null && this.m_CreateAsmodeeEmail_PlaydekOnly != null)
							{
								this.m_CreateAsmodeeEmail_PlaydekOnly.text = this.m_VerifyPlaydekEmail_PlaydekOnly.text;
							}
							if (this.m_VerifyPlaydekPassword_PlaydekOnly != null)
							{
								if (this.m_CreateAsmodeePassword1_PlaydekOnly != null)
								{
									this.m_CreateAsmodeePassword1_PlaydekOnly.text = this.m_VerifyPlaydekPassword_PlaydekOnly.text;
								}
								if (this.m_CreateAsmodeePassword2_PlaydekOnly != null)
								{
									this.m_CreateAsmodeePassword2_PlaydekOnly.text = this.m_VerifyPlaydekPassword_PlaydekOnly.text;
								}
							}
							if (this.m_CreateAsmodeeLoginName_PlaydekOnly != null)
							{
								GCHandle gchandle = GCHandle.Alloc(new byte[64], GCHandleType.Pinned);
								IntPtr intPtr = gchandle.AddrOfPinnedObject();
								AgricolaLib.NetworkGetVerifyAccountName(intPtr, 64);
								VerifyAccountData verifyAccountData = (VerifyAccountData)Marshal.PtrToStructure(intPtr, typeof(VerifyAccountData));
								this.m_CreateAsmodeeLoginName_PlaydekOnly.text = verifyAccountData.username;
								gchandle.Free();
							}
							this.Enable_PlaydekOnly_CreateAsmodeeAccount(true);
						}
					}
				}
			}
		}
		if (this.m_bWaitCreateAsmodee_PlaydekOnly)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitCreateAsmodee_PlaydekOnly = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultCreateAsmodee_PlaydekOnly != null)
					{
						string input_text17 = "${LINK_FAIL_CONNECT_ASMODEE}";
						string text25 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text17);
						this.m_ResultCreateAsmodee_PlaydekOnly.SetText(text25);
					}
					this.Enable_PlaydekOnly_CreateAsmodeeAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int createAsmodeeResult = this.m_NetworkAsmodee.GetCreateAsmodeeResult();
					if (createAsmodeeResult != -1)
					{
						this.m_bWaitCreateAsmodee_PlaydekOnly = false;
						this.m_ConnectWaitTime = 0f;
						if (createAsmodeeResult == 0)
						{
							if (this.m_ResultCreateAsmodee_PlaydekOnly != null)
							{
								string createError = this.m_NetworkAsmodee.GetCreateError();
								string text26 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(createError);
								this.m_ResultCreateAsmodee_PlaydekOnly.SetText(text26);
							}
							this.Enable_PlaydekOnly_CreateAsmodeeAccount(true);
						}
						else if (createAsmodeeResult == 1)
						{
							if (this.m_ResultCreateAsmodee_PlaydekOnly != null)
							{
								string input_text18 = "${LINK_ACCOUNT_CREATED_ASMODEE}";
								string text27 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text18);
								this.m_ResultCreateAsmodee_PlaydekOnly.SetText(text27);
							}
							string value = string.Empty;
							string value2 = string.Empty;
							if (this.m_VerifyPlaydekEmail_PlaydekOnly != null)
							{
								value = this.m_VerifyPlaydekEmail_PlaydekOnly.text;
							}
							if (string.IsNullOrEmpty(value))
							{
								return;
							}
							if (this.m_VerifyPlaydekPassword_PlaydekOnly != null)
							{
								value2 = this.m_VerifyPlaydekPassword_PlaydekOnly.text;
							}
							if (string.IsNullOrEmpty(value2))
							{
								return;
							}
							if (this.m_ResultLinkAccount_PlaydekOnly != null)
							{
								string input_text19 = "${LINK_ACCOUNTS_READY}";
								string text28 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text19);
								this.m_ResultLinkAccount_PlaydekOnly.SetText(text28);
							}
							this.Enable_PlaydekOnly_LinkAccount(true);
						}
					}
				}
			}
		}
		if (this.m_bWaitLinkAccount_PlaydekOnly)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitLinkAccount_PlaydekOnly = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultLinkAccount_PlaydekOnly != null)
					{
						string input_text20 = "${LINK_FAIL_CONNECT_PLAYDEK}";
						string text29 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text20);
						this.m_ResultLinkAccount_PlaydekOnly.SetText(text29);
					}
					this.Enable_PlaydekOnly_LinkAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int linkAccountResult3 = this.m_NetworkAsmodee.GetLinkAccountResult();
					if (linkAccountResult3 != -1)
					{
						this.m_bWaitLinkAccount_PlaydekOnly = false;
						this.m_ConnectWaitTime = 0f;
						if (linkAccountResult3 == 0)
						{
							if (this.m_ResultLinkAccount_PlaydekOnly != null)
							{
								string input_text21 = "${LINK_ACCOUNT_FAILED}";
								string text30 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text21);
								this.m_ResultLinkAccount_PlaydekOnly.SetText(text30);
							}
							this.Enable_PlaydekOnly_LinkAccount(true);
						}
						else if (linkAccountResult3 == 1)
						{
							if (this.m_ResultLinkAccount_PlaydekOnly != null)
							{
								string input_text22 = "${LINK_ACCOUNT_SUCCESS}";
								string text31 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text22);
								this.m_ResultLinkAccount_PlaydekOnly.SetText(text31);
							}
							if (this.m_usernamePrompt != null && this.m_CreateAsmodeeLoginName_PlaydekOnly != null)
							{
								this.m_usernamePrompt.text = this.m_CreateAsmodeeLoginName_PlaydekOnly.text;
							}
							if (this.m_passwordPrompt != null && this.m_CreateAsmodeePassword1_PlaydekOnly != null)
							{
								this.m_passwordPrompt.text = this.m_CreateAsmodeePassword1_PlaydekOnly.text;
							}
							this.Enable_PlaydekOnly_Completed(true);
							PlayerPrefs.SetInt("AsmodeeAccountLink", 1);
						}
					}
				}
			}
		}
		if (this.m_bWaitCreateAsmodee_HaveNone)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitCreateAsmodee_HaveNone = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultCreateAsmodee_HaveNone != null)
					{
						string input_text23 = "${LINK_FAIL_CONNECT_ASMODEE}";
						string text32 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text23);
						this.m_ResultCreateAsmodee_HaveNone.SetText(text32);
					}
					this.Enable_HaveNone_CreateAsmodeeAccount(true);
				}
				else if (this.m_NetworkAsmodee != null)
				{
					int createAsmodeeResult2 = this.m_NetworkAsmodee.GetCreateAsmodeeResult();
					if (createAsmodeeResult2 != -1)
					{
						this.m_bWaitCreateAsmodee_HaveNone = false;
						this.m_ConnectWaitTime = 0f;
						if (createAsmodeeResult2 == 0)
						{
							if (this.m_ResultCreateAsmodee_HaveNone != null)
							{
								string createError2 = this.m_NetworkAsmodee.GetCreateError();
								string text33 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(createError2);
								this.m_ResultCreateAsmodee_HaveNone.SetText(text33);
							}
							this.Enable_HaveNone_CreateAsmodeeAccount(true);
						}
						else if (createAsmodeeResult2 == 1)
						{
							if (this.m_ResultCreateAsmodee_HaveNone != null)
							{
								string input_text24 = "${LINK_ACCOUNT_CREATED_ASMODEE}";
								string text34 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text24);
								this.m_ResultCreateAsmodee_HaveNone.SetText(text34);
							}
							if (this.m_ResultLinkAccount_HaveNone != null)
							{
								string input_text25 = "${LINK_ACCOUNTS_READY}";
								string text35 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text25);
								this.m_ResultLinkAccount_HaveNone.SetText(text35);
							}
							this.Enable_HaveNone_LinkAccounts(true);
						}
					}
				}
			}
		}
		if (this.m_bWaitLinkAccount_HaveNone)
		{
			this.m_ConnectWaitTime += Time.deltaTime;
			if (this.m_ConnectWaitTime >= 2f)
			{
				if (this.m_ConnectWaitTime > 15f)
				{
					this.m_bWaitLinkAccount_HaveNone = false;
					this.m_ConnectWaitTime = 0f;
					if (this.m_ResultLinkAccount_HaveNone != null)
					{
						string input_text26 = "${LINK_FAIL_CONNECT_PLAYDEK}";
						string text36 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text26);
						this.m_ResultLinkAccount_HaveNone.SetText(text36);
					}
					this.Enable_HaveNone_LinkAccounts(true);
					return;
				}
				if (this.m_NetworkAsmodee != null)
				{
					int linkAccountResult4 = this.m_NetworkAsmodee.GetLinkAccountResult();
					if (linkAccountResult4 != -1)
					{
						this.m_bWaitLinkAccount_HaveNone = false;
						this.m_ConnectWaitTime = 0f;
						if (linkAccountResult4 == 0)
						{
							if (this.m_ResultLinkAccount_HaveNone != null)
							{
								string input_text27 = "${LINK_ACCOUNT_FAILED}";
								string text37 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text27);
								this.m_ResultLinkAccount_HaveNone.SetText(text37);
							}
							this.Enable_HaveNone_LinkAccounts(true);
							return;
						}
						if (linkAccountResult4 == 1)
						{
							if (this.m_ResultLinkAccount_HaveNone != null)
							{
								string input_text28 = "${LINK_ACCOUNT_SUCCESS}";
								string text38 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text28);
								this.m_ResultLinkAccount_HaveNone.SetText(text38);
							}
							if (this.m_usernamePrompt != null && this.m_CreateAsmodeeLoginName_HaveNone != null)
							{
								this.m_usernamePrompt.text = this.m_CreateAsmodeeLoginName_HaveNone.text;
							}
							if (this.m_passwordPrompt != null && this.m_CreateAsmodeePassword1_HaveNone != null)
							{
								this.m_passwordPrompt.text = this.m_CreateAsmodeePassword1_HaveNone.text;
							}
							this.Enable_HaveNone_Completed(true);
							PlayerPrefs.SetInt("AsmodeeAccountLink", 1);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x000472EC File Offset: 0x000454EC
	public void OnEnterMenu()
	{
		if (this.m_bHandlePopup)
		{
			this.m_bHandlePopup = false;
			return;
		}
		this.RetreiveSettings();
		if (this.m_rememberPasswordCheckBox.isOn && Network.m_bPausedDuringNetworkSession)
		{
			this.Submit();
		}
		Network.m_bPausedDuringNetworkSession = false;
		if (PlayerPrefs.GetInt("AsmodeeAccountLink", 0) != 1 && this.m_PopupAccountLink != null)
		{
			this.m_PopupAccountLink.SetActive(true);
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00047357 File Offset: 0x00045557
	public void OnExitMenu(bool bUnderPopup)
	{
		if (bUnderPopup)
		{
			return;
		}
		this.StoreSettings();
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00047363 File Offset: 0x00045563
	public void OnClickLoginButton()
	{
		this.Submit();
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0004736B File Offset: 0x0004556B
	public void OnClickForgotPasswordButton()
	{
		Application.OpenURL("https://www.daysofwonder.com/en/reset/");
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00047378 File Offset: 0x00045578
	private void RetreiveSettings()
	{
		bool flag = PlayerPrefs.GetInt("Login_RememberPassword", 0) != 0;
		this.m_rememberPasswordCheckBox.isOn = flag;
		this.m_usernamePrompt.text = PlayerPrefs.GetString("Login_LastUsername", "");
		this.m_passwordPrompt.text = (flag ? PlayerPrefs.GetString("Login_LastPassword", "") : "");
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x000473E0 File Offset: 0x000455E0
	private void StoreSettings()
	{
		PlayerPrefs.SetString("Login_LastUsername", this.m_usernamePrompt.text);
		PlayerPrefs.SetString("Login_LastPassword", this.m_rememberPasswordCheckBox.isOn ? this.m_passwordPrompt.text : "");
		PlayerPrefs.SetInt("Login_RememberPassword", this.m_rememberPasswordCheckBox.isOn ? 1 : 0);
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00047446 File Offset: 0x00045646
	private void Submit()
	{
		if (this.ValidatedAccountInput())
		{
			Network.SetLogin(this.m_usernamePrompt.text, this.m_passwordPrompt.text);
			ScreenManager.instance.PushScene("Connecting");
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0004747C File Offset: 0x0004567C
	private bool ValidatedAccountInput()
	{
		if (string.IsNullOrEmpty(this.m_usernamePrompt.text))
		{
			GameObject scene = ScreenManager.instance.GetScene("ConfirmPopup");
			if (scene != null)
			{
				UI_ConfirmPopup component = scene.GetComponent<UI_ConfirmPopup>();
				if (component)
				{
					this.m_bHandlePopup = true;
					component.Setup(null, "Key_EmailRequried", UI_ConfirmPopup.ButtonFormat.OneButton);
					ScreenManager.instance.PushScene("ConfirmPopup");
				}
			}
			return false;
		}
		if (string.IsNullOrEmpty(this.m_passwordPrompt.text))
		{
			GameObject scene2 = ScreenManager.instance.GetScene("ConfirmPopup");
			if (scene2 != null)
			{
				UI_ConfirmPopup component2 = scene2.GetComponent<UI_ConfirmPopup>();
				if (component2)
				{
					this.m_bHandlePopup = true;
					component2.Setup(null, "Key_PasswordRequired", UI_ConfirmPopup.ButtonFormat.OneButton);
					ScreenManager.instance.PushScene("ConfirmPopup");
				}
			}
			return false;
		}
		return true;
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00047548 File Offset: 0x00045748
	private void Enable_HaveBoth_VerifyAsmodeeAccount(bool bEnable)
	{
		if (this.m_VerifyAsmodeeLoginName_HaveBoth != null)
		{
			this.m_VerifyAsmodeeLoginName_HaveBoth.interactable = bEnable;
		}
		if (this.m_VerifyAsmodeePassword_HaveBoth != null)
		{
			this.m_VerifyAsmodeePassword_HaveBoth.interactable = bEnable;
		}
		if (this.m_SubmitVerifyAsmodee_HaveBoth != null)
		{
			this.m_SubmitVerifyAsmodee_HaveBoth.interactable = bEnable;
		}
		if (this.m_DisableVerifyAsmodee_HaveBoth != null)
		{
			this.m_DisableVerifyAsmodee_HaveBoth.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x000475C0 File Offset: 0x000457C0
	private void Enable_HaveBoth_VerifyPlaydekAccount(bool bEnable)
	{
		if (this.m_VerifyPlaydekEmail_HaveBoth != null)
		{
			this.m_VerifyPlaydekEmail_HaveBoth.interactable = bEnable;
		}
		if (this.m_VerifyPlaydekPassword_HaveBoth != null)
		{
			this.m_VerifyPlaydekPassword_HaveBoth.interactable = bEnable;
		}
		if (this.m_SubmitVerifyPlaydek_HaveBoth != null)
		{
			this.m_SubmitVerifyPlaydek_HaveBoth.interactable = bEnable;
		}
		if (this.m_DisableVerifyPlaydek_HaveBoth != null)
		{
			this.m_DisableVerifyPlaydek_HaveBoth.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00047638 File Offset: 0x00045838
	private void Enable_HaveBoth_SubmitLinkAccount(bool bEnable)
	{
		if (this.m_SubmitLinkAccount_HaveBoth != null)
		{
			this.m_SubmitLinkAccount_HaveBoth.interactable = bEnable;
		}
		if (this.m_DisableLinkAccount_HaveBoth != null)
		{
			this.m_DisableLinkAccount_HaveBoth.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00047671 File Offset: 0x00045871
	private void Enable_HaveBoth_Completed(bool bEnable)
	{
		if (this.m_ButtonDone_HaveBoth != null)
		{
			this.m_ButtonDone_HaveBoth.SetActive(bEnable);
		}
		if (this.m_ButtonCancel_HaveBoth != null)
		{
			this.m_ButtonCancel_HaveBoth.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x000476AC File Offset: 0x000458AC
	public void Reset_HaveBoth()
	{
		this.m_ConnectWaitTime = 0f;
		this.m_bWaitVerifyAsmodee_HaveBoth = false;
		this.m_bWaitVerifyPlaydek_HaveBoth = false;
		this.m_bWaitLinkAccount_HaveBoth = false;
		if (this.m_VerifyAsmodeeLoginName_HaveBoth != null)
		{
			this.m_VerifyAsmodeeLoginName_HaveBoth.text = string.Empty;
		}
		if (this.m_VerifyAsmodeePassword_HaveBoth != null)
		{
			this.m_VerifyAsmodeePassword_HaveBoth.text = string.Empty;
		}
		if (this.m_ResultVerifyAsmodee_HaveBoth != null)
		{
			this.m_ResultVerifyAsmodee_HaveBoth.SetText(string.Empty);
		}
		if (this.m_VerifyPlaydekEmail_HaveBoth != null)
		{
			this.m_VerifyPlaydekEmail_HaveBoth.text = string.Empty;
		}
		if (this.m_VerifyPlaydekPassword_HaveBoth != null)
		{
			this.m_VerifyPlaydekPassword_HaveBoth.text = string.Empty;
		}
		if (this.m_ResultVerifyPlaydek_HaveBoth != null)
		{
			this.m_ResultVerifyPlaydek_HaveBoth.SetText(string.Empty);
		}
		if (this.m_ResultLinkAccount_HaveBoth != null)
		{
			string input_text = "${LINK_MUST_VERIFY_BOTH}";
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultLinkAccount_HaveBoth.SetText(text);
		}
		this.Enable_HaveBoth_VerifyAsmodeeAccount(true);
		this.Enable_HaveBoth_VerifyPlaydekAccount(false);
		this.Enable_HaveBoth_SubmitLinkAccount(false);
		this.Enable_HaveBoth_Completed(false);
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x000477D8 File Offset: 0x000459D8
	public void OnButtonPressed_HaveBoth_VerifyAsmodeeAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.m_VerifyAsmodeeLoginName_HaveBoth != null)
		{
			text = this.m_VerifyAsmodeeLoginName_HaveBoth.text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.m_VerifyAsmodeePassword_HaveBoth != null)
		{
			text2 = this.m_VerifyAsmodeePassword_HaveBoth.text;
		}
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		this.m_bWaitVerifyAsmodee_HaveBoth = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_HaveBoth_VerifyAsmodeeAccount(false);
		if (this.m_ResultVerifyAsmodee_HaveBoth != null)
		{
			string input_text = "${LINK_CONNECTING_ASMODEE}";
			string text3 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultVerifyAsmodee_HaveBoth.SetText(text3);
		}
		this.m_NetworkAsmodee.RequestVerifyAsmodeeAccount(text, text2);
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x00047898 File Offset: 0x00045A98
	public void OnButtonPressed_HaveBoth_VerifyPlaydekAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.m_VerifyPlaydekEmail_HaveBoth != null)
		{
			text = this.m_VerifyPlaydekEmail_HaveBoth.text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.m_VerifyPlaydekPassword_HaveBoth != null)
		{
			text2 = this.m_VerifyPlaydekPassword_HaveBoth.text;
		}
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		this.m_bWaitVerifyPlaydek_HaveBoth = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_HaveBoth_VerifyPlaydekAccount(false);
		if (this.m_ResultVerifyPlaydek_HaveBoth != null)
		{
			string input_text = "${LINK_CONNECTING_PLAYDEK}";
			string text3 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultVerifyPlaydek_HaveBoth.SetText(text3);
		}
		this.m_NetworkAsmodee.RequestVerifyPlaydekAcount(text, text2);
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00047958 File Offset: 0x00045B58
	public void OnButtonPressed_HaveBoth_SubmitLinkAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.m_VerifyPlaydekEmail_HaveBoth != null)
		{
			text = this.m_VerifyPlaydekEmail_HaveBoth.text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.m_VerifyPlaydekPassword_HaveBoth != null)
		{
			text2 = this.m_VerifyPlaydekPassword_HaveBoth.text;
		}
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		this.m_bWaitLinkAccount_HaveBoth = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_HaveBoth_SubmitLinkAccount(false);
		if (this.m_ResultLinkAccount_HaveBoth != null)
		{
			string input_text = "${LINK_CONNECTING_PLAYDEK}";
			string text3 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultLinkAccount_HaveBoth.SetText(text3);
		}
		this.m_NetworkAsmodee.RequestLinkAccount(text, text2);
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00047A18 File Offset: 0x00045C18
	private void Enable_AsmodeeOnly_VerifyAsmodeeAccount(bool bEnable)
	{
		if (this.m_VerifyAsmodeeLoginName_AsmodeeOnly != null)
		{
			this.m_VerifyAsmodeeLoginName_AsmodeeOnly.interactable = bEnable;
		}
		if (this.m_VerifyAsmodeePassword_AsmodeeOnly != null)
		{
			this.m_VerifyAsmodeePassword_AsmodeeOnly.interactable = bEnable;
		}
		if (this.m_SubmitVerifyAsmodee_AsmodeeOnly != null)
		{
			this.m_SubmitVerifyAsmodee_AsmodeeOnly.interactable = bEnable;
		}
		if (this.m_DisableVerifyAsmodee_AsmodeeOnly != null)
		{
			this.m_DisableVerifyAsmodee_AsmodeeOnly.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00047A90 File Offset: 0x00045C90
	private void Enable_AsmodeeOnly_LinkPlaydekAccount(bool bEnable)
	{
		if (this.m_SubmitLinkPlaydek_AsmodeeOnly != null)
		{
			this.m_SubmitLinkPlaydek_AsmodeeOnly.interactable = bEnable;
		}
		if (this.m_DisableLinkPlaydek_AsmodeeOnly != null)
		{
			this.m_DisableLinkPlaydek_AsmodeeOnly.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x00047AC9 File Offset: 0x00045CC9
	private void Enable_AsmodeeOnly_Completed(bool bEnable)
	{
		if (this.m_ButtonDone_AsmodeeOnly != null)
		{
			this.m_ButtonDone_AsmodeeOnly.SetActive(bEnable);
		}
		if (this.m_ButtonCancel_AsmodeeOnly != null)
		{
			this.m_ButtonCancel_AsmodeeOnly.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x00047B04 File Offset: 0x00045D04
	public void Reset_AsmodeeOnly()
	{
		this.m_ConnectWaitTime = 0f;
		this.m_bWaitVerifyAsmodee_AsmodeeOnly = false;
		this.m_bWaitLinkAccount_AsmodeeOnly = false;
		if (this.m_VerifyAsmodeeLoginName_AsmodeeOnly != null)
		{
			this.m_VerifyAsmodeeLoginName_AsmodeeOnly.text = string.Empty;
		}
		if (this.m_VerifyAsmodeePassword_AsmodeeOnly != null)
		{
			this.m_VerifyAsmodeePassword_AsmodeeOnly.text = string.Empty;
		}
		if (this.m_ResultVerifyAsmodee_AsmodeeOnly != null)
		{
			this.m_ResultVerifyAsmodee_AsmodeeOnly.SetText(string.Empty);
		}
		if (this.m_ResultLinkAccount_AsmodeeOnly != null)
		{
			string input_text = "${LINK_MUST_VERIFY}";
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultLinkAccount_AsmodeeOnly.SetText(text);
		}
		this.Enable_AsmodeeOnly_VerifyAsmodeeAccount(true);
		this.Enable_AsmodeeOnly_LinkPlaydekAccount(false);
		this.Enable_AsmodeeOnly_Completed(false);
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x00047BC8 File Offset: 0x00045DC8
	public void OnButtonPressed_AsmodeeOnly_VerifyAsmodeeAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.m_VerifyAsmodeeLoginName_AsmodeeOnly != null)
		{
			text = this.m_VerifyAsmodeeLoginName_AsmodeeOnly.text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.m_VerifyAsmodeePassword_AsmodeeOnly != null)
		{
			text2 = this.m_VerifyAsmodeePassword_AsmodeeOnly.text;
		}
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		this.m_bWaitVerifyAsmodee_AsmodeeOnly = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_AsmodeeOnly_VerifyAsmodeeAccount(false);
		if (this.m_ResultVerifyAsmodee_AsmodeeOnly != null)
		{
			string input_text = "${LINK_CONNECTING_ASMODEE}";
			string text3 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultVerifyAsmodee_AsmodeeOnly.SetText(text3);
		}
		this.m_NetworkAsmodee.RequestVerifyAsmodeeAccount(text, text2);
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x00047C88 File Offset: 0x00045E88
	public void OnButtonPressed_AsmodeeOnly_LinkPlaydekAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string value = string.Empty;
		string value2 = string.Empty;
		if (this.m_VerifyAsmodeeLoginName_AsmodeeOnly != null)
		{
			value = this.m_VerifyAsmodeeLoginName_AsmodeeOnly.text;
		}
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		if (this.m_VerifyAsmodeePassword_AsmodeeOnly != null)
		{
			value2 = this.m_VerifyAsmodeePassword_AsmodeeOnly.text;
		}
		if (string.IsNullOrEmpty(value2))
		{
			return;
		}
		this.m_bWaitLinkAccount_AsmodeeOnly = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_AsmodeeOnly_LinkPlaydekAccount(false);
		if (this.m_ResultLinkAccount_AsmodeeOnly != null)
		{
			string input_text = "${LINK_CONNECTING_PLAYDEK}";
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultLinkAccount_AsmodeeOnly.SetText(text);
		}
		this.m_NetworkAsmodee.RequestLinkAccountPlaceholder();
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x00047D48 File Offset: 0x00045F48
	private void Enable_PlaydekOnly_VerifyPlaydekAccount(bool bEnable)
	{
		if (this.m_VerifyPlaydekEmail_PlaydekOnly != null)
		{
			this.m_VerifyPlaydekEmail_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_VerifyPlaydekPassword_PlaydekOnly != null)
		{
			this.m_VerifyPlaydekPassword_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_SubmitVerifyPlaydek_PlaydekOnly != null)
		{
			this.m_SubmitVerifyPlaydek_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_DisableVerifyPlaydek_PlaydekOnly != null)
		{
			this.m_DisableVerifyPlaydek_PlaydekOnly.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x00047DC0 File Offset: 0x00045FC0
	private void Enable_PlaydekOnly_CreateAsmodeeAccount(bool bEnable)
	{
		if (this.m_CreateAsmodeeEmail_PlaydekOnly != null)
		{
			this.m_CreateAsmodeeEmail_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_CreateAsmodeeLoginName_PlaydekOnly != null)
		{
			this.m_CreateAsmodeeLoginName_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_CreateAsmodeePassword1_PlaydekOnly != null)
		{
			this.m_CreateAsmodeePassword1_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_CreateAsmodeePassword2_PlaydekOnly != null)
		{
			this.m_CreateAsmodeePassword2_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_SubmitCreateAsmodee_PlaydekOnly != null)
		{
			this.m_SubmitCreateAsmodee_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_DisableCreateAsmodee_PlaydekOnly != null)
		{
			this.m_DisableCreateAsmodee_PlaydekOnly.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x00047E6C File Offset: 0x0004606C
	private void Enable_PlaydekOnly_LinkAccount(bool bEnable)
	{
		if (this.m_SubmitLinkAccount_PlaydekOnly != null)
		{
			this.m_SubmitLinkAccount_PlaydekOnly.interactable = bEnable;
		}
		if (this.m_DisableLinkAccount_PlaydekOnly != null)
		{
			this.m_DisableLinkAccount_PlaydekOnly.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x00047EA5 File Offset: 0x000460A5
	private void Enable_PlaydekOnly_Completed(bool bEnable)
	{
		if (this.m_ButtonDone_PlaydekOnly != null)
		{
			this.m_ButtonDone_PlaydekOnly.SetActive(bEnable);
		}
		if (this.m_ButtonCancel_PlaydekOnly != null)
		{
			this.m_ButtonCancel_PlaydekOnly.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x00047EE0 File Offset: 0x000460E0
	public void Reset_PlaydekOnly()
	{
		this.m_ConnectWaitTime = 0f;
		this.m_bWaitVerifyPlaydek_PlaydekOnly = false;
		this.m_bWaitCreateAsmodee_PlaydekOnly = false;
		this.m_bWaitLinkAccount_PlaydekOnly = false;
		if (this.m_VerifyPlaydekEmail_PlaydekOnly != null)
		{
			this.m_VerifyPlaydekEmail_PlaydekOnly.text = string.Empty;
		}
		if (this.m_VerifyPlaydekPassword_PlaydekOnly != null)
		{
			this.m_VerifyPlaydekPassword_PlaydekOnly.text = string.Empty;
		}
		if (this.m_ResultVerifyPlaydek_PlaydekOnly != null)
		{
			this.m_ResultVerifyPlaydek_PlaydekOnly.SetText(string.Empty);
		}
		if (this.m_CreateAsmodeeEmail_PlaydekOnly != null)
		{
			this.m_CreateAsmodeeEmail_PlaydekOnly.text = string.Empty;
		}
		if (this.m_CreateAsmodeeLoginName_PlaydekOnly != null)
		{
			this.m_CreateAsmodeeLoginName_PlaydekOnly.text = string.Empty;
		}
		if (this.m_CreateAsmodeePassword1_PlaydekOnly != null)
		{
			this.m_CreateAsmodeePassword1_PlaydekOnly.text = string.Empty;
		}
		if (this.m_CreateAsmodeePassword2_PlaydekOnly != null)
		{
			this.m_CreateAsmodeePassword2_PlaydekOnly.text = string.Empty;
		}
		if (this.m_ResultCreateAsmodee_PlaydekOnly != null)
		{
			this.m_ResultCreateAsmodee_PlaydekOnly.SetText(string.Empty);
		}
		if (this.m_ResultLinkAccount_PlaydekOnly != null)
		{
			string input_text = "${LINK_MUST_CREATE}";
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultLinkAccount_PlaydekOnly.SetText(text);
		}
		this.Enable_PlaydekOnly_VerifyPlaydekAccount(true);
		this.Enable_PlaydekOnly_CreateAsmodeeAccount(false);
		this.Enable_PlaydekOnly_LinkAccount(false);
		this.Enable_PlaydekOnly_Completed(false);
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x00048048 File Offset: 0x00046248
	public void OnButtonPressed_PlaydekOnly_VerifyPlaydekAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.m_VerifyPlaydekEmail_PlaydekOnly != null)
		{
			text = this.m_VerifyPlaydekEmail_PlaydekOnly.text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.m_VerifyPlaydekPassword_PlaydekOnly != null)
		{
			text2 = this.m_VerifyPlaydekPassword_PlaydekOnly.text;
		}
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		this.m_bWaitVerifyPlaydek_PlaydekOnly = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_PlaydekOnly_VerifyPlaydekAccount(false);
		if (this.m_ResultVerifyPlaydek_PlaydekOnly != null)
		{
			string input_text = "${LINK_CONNECTING_PLAYDEK}";
			string text3 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultVerifyPlaydek_PlaydekOnly.SetText(text3);
		}
		this.m_NetworkAsmodee.RequestVerifyPlaydekAcount(text, text2);
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x00048108 File Offset: 0x00046308
	public void OnButtonPressed_PlaydekOnly_CreateAsmodeeAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		if (this.m_CreateAsmodeeEmail_PlaydekOnly != null)
		{
			text = this.m_CreateAsmodeeEmail_PlaydekOnly.text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.m_CreateAsmodeeLoginName_PlaydekOnly != null)
		{
			text2 = this.m_CreateAsmodeeLoginName_PlaydekOnly.text;
		}
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		if (this.m_CreateAsmodeePassword1_PlaydekOnly != null)
		{
			text3 = this.m_CreateAsmodeePassword1_PlaydekOnly.text;
		}
		if (string.IsNullOrEmpty(text3))
		{
			return;
		}
		if (this.m_CreateAsmodeePassword2_PlaydekOnly != null)
		{
			string text4 = this.m_CreateAsmodeePassword2_PlaydekOnly.text;
			if (string.IsNullOrEmpty(text4))
			{
				return;
			}
			if (text3 != text4)
			{
				if (this.m_ResultCreateAsmodee_PlaydekOnly != null)
				{
					string input_text = "${LINK_PASSWORDS_DO_NOT_MATCH}";
					string text5 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
					this.m_ResultCreateAsmodee_PlaydekOnly.SetText(text5);
				}
				return;
			}
		}
		this.m_bWaitCreateAsmodee_PlaydekOnly = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_PlaydekOnly_CreateAsmodeeAccount(false);
		if (this.m_ResultCreateAsmodee_PlaydekOnly != null)
		{
			string input_text2 = "${LINK_CONNECTING_ASMODEE}";
			string text6 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text2);
			this.m_ResultCreateAsmodee_PlaydekOnly.SetText(text6);
		}
		this.m_NetworkAsmodee.RequestCreateAsmodeeAccount(text, text3, text2);
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00048254 File Offset: 0x00046454
	public void OnButtonPressed_PlaydekOnly_LinkAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.m_VerifyPlaydekEmail_PlaydekOnly != null)
		{
			text = this.m_VerifyPlaydekEmail_PlaydekOnly.text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.m_VerifyPlaydekPassword_PlaydekOnly != null)
		{
			text2 = this.m_VerifyPlaydekPassword_PlaydekOnly.text;
		}
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		this.m_bWaitLinkAccount_PlaydekOnly = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_PlaydekOnly_LinkAccount(false);
		if (this.m_ResultLinkAccount_PlaydekOnly != null)
		{
			string input_text = "${LINK_CONNECTING_PLAYDEK}";
			string text3 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultLinkAccount_PlaydekOnly.SetText(text3);
		}
		this.m_NetworkAsmodee.RequestLinkAccount(text, text2);
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00048314 File Offset: 0x00046514
	private void Enable_HaveNone_CreateAsmodeeAccount(bool bEnable)
	{
		if (this.m_CreateAsmodeeEmail_HaveNone != null)
		{
			this.m_CreateAsmodeeEmail_HaveNone.interactable = bEnable;
		}
		if (this.m_CreateAsmodeeLoginName_HaveNone != null)
		{
			this.m_CreateAsmodeeLoginName_HaveNone.interactable = bEnable;
		}
		if (this.m_CreateAsmodeePassword1_HaveNone != null)
		{
			this.m_CreateAsmodeePassword1_HaveNone.interactable = bEnable;
		}
		if (this.m_CreateAsmodeePassword2_HaveNone != null)
		{
			this.m_CreateAsmodeePassword2_HaveNone.interactable = bEnable;
		}
		if (this.m_SubmitCreateAsmodee_HaveNone != null)
		{
			this.m_SubmitCreateAsmodee_HaveNone.interactable = bEnable;
		}
		if (this.m_DisableCreateAsmodee_HaveNone != null)
		{
			this.m_DisableCreateAsmodee_HaveNone.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x000483C0 File Offset: 0x000465C0
	private void Enable_HaveNone_LinkAccounts(bool bEnable)
	{
		if (this.m_SubmitLinkPlaydek_HaveNone != null)
		{
			this.m_SubmitLinkPlaydek_HaveNone.interactable = bEnable;
		}
		if (this.m_DisableLinkAccount_HaveNone != null)
		{
			this.m_DisableLinkAccount_HaveNone.SetActive(!bEnable);
		}
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x000483F9 File Offset: 0x000465F9
	private void Enable_HaveNone_Completed(bool bEnable)
	{
		if (this.m_ButtonDone_HaveNone != null)
		{
			this.m_ButtonDone_HaveNone.SetActive(bEnable);
		}
		if (this.m_ButtonCancel_HaveNone != null)
		{
			this.m_ButtonCancel_HaveNone.SetActive(!bEnable);
		}
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x00048434 File Offset: 0x00046634
	public void Reset_HaveNone()
	{
		this.m_ConnectWaitTime = 0f;
		this.m_bWaitCreateAsmodee_HaveNone = false;
		this.m_bWaitLinkAccount_HaveNone = false;
		if (this.m_CreateAsmodeeEmail_HaveNone != null)
		{
			this.m_CreateAsmodeeEmail_HaveNone.text = string.Empty;
		}
		if (this.m_CreateAsmodeeLoginName_HaveNone != null)
		{
			this.m_CreateAsmodeeLoginName_HaveNone.text = string.Empty;
		}
		if (this.m_CreateAsmodeePassword1_HaveNone != null)
		{
			this.m_CreateAsmodeePassword1_HaveNone.text = string.Empty;
		}
		if (this.m_CreateAsmodeePassword2_HaveNone != null)
		{
			this.m_CreateAsmodeePassword2_HaveNone.text = string.Empty;
		}
		if (this.m_ResultCreateAsmodee_HaveNone != null)
		{
			this.m_ResultCreateAsmodee_HaveNone.SetText(string.Empty);
		}
		if (this.m_ResultLinkAccount_HaveNone != null)
		{
			string input_text = "${LINK_MUST_CREATE}";
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultLinkAccount_HaveNone.SetText(text);
		}
		this.Enable_HaveNone_CreateAsmodeeAccount(true);
		this.Enable_HaveNone_LinkAccounts(false);
		this.Enable_HaveNone_Completed(false);
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x00048534 File Offset: 0x00046734
	public void OnButtonPressed_HaveNone_CreateAsmodeeAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		if (this.m_CreateAsmodeeEmail_HaveNone != null)
		{
			text = this.m_CreateAsmodeeEmail_HaveNone.text;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.m_CreateAsmodeeLoginName_HaveNone != null)
		{
			text2 = this.m_CreateAsmodeeLoginName_HaveNone.text;
		}
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		if (this.m_CreateAsmodeePassword1_HaveNone != null)
		{
			text3 = this.m_CreateAsmodeePassword1_HaveNone.text;
		}
		if (string.IsNullOrEmpty(text3))
		{
			return;
		}
		if (this.m_CreateAsmodeePassword2_HaveNone != null)
		{
			string text4 = this.m_CreateAsmodeePassword2_HaveNone.text;
			if (string.IsNullOrEmpty(text4))
			{
				return;
			}
			if (text3 != text4)
			{
				if (this.m_ResultCreateAsmodee_HaveNone != null)
				{
					string input_text = "${LINK_PASSWORDS_DO_NOT_MATCH}";
					string text5 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
					this.m_ResultCreateAsmodee_HaveNone.SetText(text5);
				}
				return;
			}
		}
		this.m_bWaitCreateAsmodee_HaveNone = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_HaveNone_CreateAsmodeeAccount(false);
		if (this.m_ResultCreateAsmodee_HaveNone != null)
		{
			string input_text2 = "${LINK_CONNECTING_ASMODEE}";
			string text6 = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text2);
			this.m_ResultCreateAsmodee_HaveNone.SetText(text6);
		}
		this.m_NetworkAsmodee.RequestCreateAsmodeeAccount(text, text3, text2);
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00048680 File Offset: 0x00046880
	public void OnButtonPressed_HaveNone_LinkAccount()
	{
		if (this.m_NetworkAsmodee == null)
		{
			return;
		}
		string value = string.Empty;
		string value2 = string.Empty;
		string value3 = string.Empty;
		if (this.m_CreateAsmodeeEmail_HaveNone != null)
		{
			value = this.m_CreateAsmodeeEmail_HaveNone.text;
		}
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		if (this.m_CreateAsmodeeLoginName_HaveNone != null)
		{
			value2 = this.m_CreateAsmodeeLoginName_HaveNone.text;
		}
		if (string.IsNullOrEmpty(value2))
		{
			return;
		}
		if (this.m_CreateAsmodeePassword1_HaveNone != null)
		{
			value3 = this.m_CreateAsmodeePassword1_HaveNone.text;
		}
		if (string.IsNullOrEmpty(value3))
		{
			return;
		}
		this.m_bWaitLinkAccount_HaveNone = true;
		this.m_ConnectWaitTime = 0f;
		this.Enable_HaveNone_LinkAccounts(false);
		if (this.m_ResultLinkAccount_HaveNone != null)
		{
			string input_text = "${LINK_CONNECTING_PLAYDEK}";
			string text = LocalizationService.Instance.ConvertLocalizationKeysWithStringBuilder(input_text);
			this.m_ResultLinkAccount_HaveNone.SetText(text);
		}
		this.m_NetworkAsmodee.RequestLinkAccountPlaceholder();
	}

	// Token: 0x04000B33 RID: 2867
	public TMP_InputField m_emailPrompt;

	// Token: 0x04000B34 RID: 2868
	public TMP_InputField m_usernamePrompt;

	// Token: 0x04000B35 RID: 2869
	public TMP_InputField m_passwordPrompt;

	// Token: 0x04000B36 RID: 2870
	public Toggle m_rememberPasswordCheckBox;

	// Token: 0x04000B37 RID: 2871
	[SerializeField]
	private GameObject m_PopupAccountLink;

	// Token: 0x04000B38 RID: 2872
	[Header("POPUP - HAVE BOTH")]
	[SerializeField]
	private TMP_InputField m_VerifyAsmodeeLoginName_HaveBoth;

	// Token: 0x04000B39 RID: 2873
	[SerializeField]
	private TMP_InputField m_VerifyAsmodeePassword_HaveBoth;

	// Token: 0x04000B3A RID: 2874
	[SerializeField]
	private Button m_SubmitVerifyAsmodee_HaveBoth;

	// Token: 0x04000B3B RID: 2875
	[SerializeField]
	private GameObject m_DisableVerifyAsmodee_HaveBoth;

	// Token: 0x04000B3C RID: 2876
	[SerializeField]
	private TextMeshProUGUI m_ResultVerifyAsmodee_HaveBoth;

	// Token: 0x04000B3D RID: 2877
	[SerializeField]
	private TMP_InputField m_VerifyPlaydekEmail_HaveBoth;

	// Token: 0x04000B3E RID: 2878
	[SerializeField]
	private TMP_InputField m_VerifyPlaydekPassword_HaveBoth;

	// Token: 0x04000B3F RID: 2879
	[SerializeField]
	private Button m_SubmitVerifyPlaydek_HaveBoth;

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private GameObject m_DisableVerifyPlaydek_HaveBoth;

	// Token: 0x04000B41 RID: 2881
	[SerializeField]
	private TextMeshProUGUI m_ResultVerifyPlaydek_HaveBoth;

	// Token: 0x04000B42 RID: 2882
	[SerializeField]
	private Button m_SubmitLinkAccount_HaveBoth;

	// Token: 0x04000B43 RID: 2883
	[SerializeField]
	private GameObject m_DisableLinkAccount_HaveBoth;

	// Token: 0x04000B44 RID: 2884
	[SerializeField]
	private TextMeshProUGUI m_ResultLinkAccount_HaveBoth;

	// Token: 0x04000B45 RID: 2885
	[SerializeField]
	private GameObject m_ButtonDone_HaveBoth;

	// Token: 0x04000B46 RID: 2886
	[SerializeField]
	private GameObject m_ButtonCancel_HaveBoth;

	// Token: 0x04000B47 RID: 2887
	private bool m_bWaitVerifyAsmodee_HaveBoth;

	// Token: 0x04000B48 RID: 2888
	private bool m_bWaitVerifyPlaydek_HaveBoth;

	// Token: 0x04000B49 RID: 2889
	private bool m_bWaitLinkAccount_HaveBoth;

	// Token: 0x04000B4A RID: 2890
	[Header("POPUP - ASMODEE ONLY")]
	[SerializeField]
	private TMP_InputField m_VerifyAsmodeeLoginName_AsmodeeOnly;

	// Token: 0x04000B4B RID: 2891
	[SerializeField]
	private TMP_InputField m_VerifyAsmodeePassword_AsmodeeOnly;

	// Token: 0x04000B4C RID: 2892
	[SerializeField]
	private Button m_SubmitVerifyAsmodee_AsmodeeOnly;

	// Token: 0x04000B4D RID: 2893
	[SerializeField]
	private GameObject m_DisableVerifyAsmodee_AsmodeeOnly;

	// Token: 0x04000B4E RID: 2894
	[SerializeField]
	private TextMeshProUGUI m_ResultVerifyAsmodee_AsmodeeOnly;

	// Token: 0x04000B4F RID: 2895
	[SerializeField]
	private Button m_SubmitLinkPlaydek_AsmodeeOnly;

	// Token: 0x04000B50 RID: 2896
	[SerializeField]
	private GameObject m_DisableLinkPlaydek_AsmodeeOnly;

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	private TextMeshProUGUI m_ResultLinkAccount_AsmodeeOnly;

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	private GameObject m_ButtonDone_AsmodeeOnly;

	// Token: 0x04000B53 RID: 2899
	[SerializeField]
	private GameObject m_ButtonCancel_AsmodeeOnly;

	// Token: 0x04000B54 RID: 2900
	private bool m_bWaitVerifyAsmodee_AsmodeeOnly;

	// Token: 0x04000B55 RID: 2901
	private bool m_bWaitLinkAccount_AsmodeeOnly;

	// Token: 0x04000B56 RID: 2902
	[Header("POPUP - PLAYDEK ONLY")]
	[SerializeField]
	private TMP_InputField m_VerifyPlaydekEmail_PlaydekOnly;

	// Token: 0x04000B57 RID: 2903
	[SerializeField]
	private TMP_InputField m_VerifyPlaydekPassword_PlaydekOnly;

	// Token: 0x04000B58 RID: 2904
	[SerializeField]
	private Button m_SubmitVerifyPlaydek_PlaydekOnly;

	// Token: 0x04000B59 RID: 2905
	[SerializeField]
	private GameObject m_DisableVerifyPlaydek_PlaydekOnly;

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	private TextMeshProUGUI m_ResultVerifyPlaydek_PlaydekOnly;

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private TMP_InputField m_CreateAsmodeeEmail_PlaydekOnly;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private TMP_InputField m_CreateAsmodeeLoginName_PlaydekOnly;

	// Token: 0x04000B5D RID: 2909
	[SerializeField]
	private TMP_InputField m_CreateAsmodeePassword1_PlaydekOnly;

	// Token: 0x04000B5E RID: 2910
	[SerializeField]
	private TMP_InputField m_CreateAsmodeePassword2_PlaydekOnly;

	// Token: 0x04000B5F RID: 2911
	[SerializeField]
	private Button m_SubmitCreateAsmodee_PlaydekOnly;

	// Token: 0x04000B60 RID: 2912
	[SerializeField]
	private GameObject m_DisableCreateAsmodee_PlaydekOnly;

	// Token: 0x04000B61 RID: 2913
	[SerializeField]
	private TextMeshProUGUI m_ResultCreateAsmodee_PlaydekOnly;

	// Token: 0x04000B62 RID: 2914
	[SerializeField]
	private Button m_SubmitLinkAccount_PlaydekOnly;

	// Token: 0x04000B63 RID: 2915
	[SerializeField]
	private GameObject m_DisableLinkAccount_PlaydekOnly;

	// Token: 0x04000B64 RID: 2916
	[SerializeField]
	private TextMeshProUGUI m_ResultLinkAccount_PlaydekOnly;

	// Token: 0x04000B65 RID: 2917
	[SerializeField]
	private GameObject m_ButtonDone_PlaydekOnly;

	// Token: 0x04000B66 RID: 2918
	[SerializeField]
	private GameObject m_ButtonCancel_PlaydekOnly;

	// Token: 0x04000B67 RID: 2919
	private bool m_bWaitVerifyPlaydek_PlaydekOnly;

	// Token: 0x04000B68 RID: 2920
	private bool m_bWaitCreateAsmodee_PlaydekOnly;

	// Token: 0x04000B69 RID: 2921
	private bool m_bWaitLinkAccount_PlaydekOnly;

	// Token: 0x04000B6A RID: 2922
	[Header("POPUP - HAVE NONE")]
	[SerializeField]
	private TMP_InputField m_CreateAsmodeeEmail_HaveNone;

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	private TMP_InputField m_CreateAsmodeeLoginName_HaveNone;

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	private TMP_InputField m_CreateAsmodeePassword1_HaveNone;

	// Token: 0x04000B6D RID: 2925
	[SerializeField]
	private TMP_InputField m_CreateAsmodeePassword2_HaveNone;

	// Token: 0x04000B6E RID: 2926
	[SerializeField]
	private Button m_SubmitCreateAsmodee_HaveNone;

	// Token: 0x04000B6F RID: 2927
	[SerializeField]
	private GameObject m_DisableCreateAsmodee_HaveNone;

	// Token: 0x04000B70 RID: 2928
	[SerializeField]
	private TextMeshProUGUI m_ResultCreateAsmodee_HaveNone;

	// Token: 0x04000B71 RID: 2929
	[SerializeField]
	private Button m_SubmitLinkPlaydek_HaveNone;

	// Token: 0x04000B72 RID: 2930
	[SerializeField]
	private GameObject m_DisableLinkAccount_HaveNone;

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private TextMeshProUGUI m_ResultLinkAccount_HaveNone;

	// Token: 0x04000B74 RID: 2932
	[SerializeField]
	private GameObject m_ButtonDone_HaveNone;

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	private GameObject m_ButtonCancel_HaveNone;

	// Token: 0x04000B76 RID: 2934
	private bool m_bWaitCreateAsmodee_HaveNone;

	// Token: 0x04000B77 RID: 2935
	private bool m_bWaitLinkAccount_HaveNone;

	// Token: 0x04000B78 RID: 2936
	private float m_ConnectWaitTime;

	// Token: 0x04000B79 RID: 2937
	private const float k_MinConnectWaitTime = 2f;

	// Token: 0x04000B7A RID: 2938
	private const float k_MaxConnectWaitTime = 15f;

	// Token: 0x04000B7B RID: 2939
	private NetworkAsmodee m_NetworkAsmodee;

	// Token: 0x04000B7C RID: 2940
	private EventSystem m_system;

	// Token: 0x04000B7D RID: 2941
	private bool m_bHandlePopup;
}

using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIRegister : MonoBehaviour {


	public InputField username;
	public InputField password;
	public InputField passwordConfirm;

	public GameObject LoginPanel;
	public GameObject RegisterPanel;


	void Start () {

		UserService.Instance.OnRegister = OnRegister;
		UserService.Instance.OnPanelChange = OnPanelChange;
	}
	

	void Update () {
		
	}


	void OnRegister(Result result, string msg)
    {
		MessageBox.Show(msg);
    }

	public void OnClickRegister()
	{
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);

		if (string.IsNullOrEmpty(this.username.text))
        {
			MessageBox.Show("请输入账号");
			return;
        }
		if (string.IsNullOrEmpty(this.password.text))
		{
			MessageBox.Show("请输入密码");
			return;
		}
		if (string.IsNullOrEmpty(this.passwordConfirm.text))
		{
			MessageBox.Show("请输入确认密码");
			return;
		}
		if (this.password.text != this.passwordConfirm.text)
        {
			MessageBox.Show("两次输入的密码不一致");
			return;
        }
		UserService.Instance.SendRegister(this.username.text, this.password.text);
		
	}

	public void OnPanelChange(Result result)
    {
		if (result == Result.Success)
        {
			LoginPanel.SetActive(true);
			RegisterPanel.SetActive(false);
        }
    }

	public void OnClickCancel()
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		LoginPanel.SetActive(true);
		RegisterPanel.SetActive(false);
	}
}

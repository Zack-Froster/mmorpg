using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour {

	public InputField username;
	public InputField password;
	public Button buttonLogin;

	public GameObject registerPanel;
	public GameObject loginPanel;


	void Start () 
	{
		buttonLogin.onClick.AddListener(OnClickLogin);

		UserService.Instance.OnLogin = OnLogin;
	}
	

	void Update () 
	{
	}

	void OnLogin(Result result, string msg)
    {
		if(result == Result.Success)
        {
			//MessageBox.Show(msg);
			SceneManager.Instance.LoadScene("CharSelect");
			SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
		}
        else
        {
			buttonLogin.onClick.AddListener(OnClickLogin);
			MessageBox.Show(msg, "错误", MessageBoxType.Error);
        }
    }

	public void OnClickLogin()
	{
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		if (string.IsNullOrEmpty(this.username.text))
        {
			MessageBox.Show("请输入账号");
			return;
        }
		if(string.IsNullOrEmpty(this.password.text))
        {
			MessageBox.Show("请输入密码");
			return;
        }
		buttonLogin.onClick.RemoveAllListeners();
		UserService.Instance.SendLogin(username.text, password.text);
	}

	public void OnClickOpenRegister()
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		registerPanel.SetActive(true);
		loginPanel.SetActive(false);
	}
}

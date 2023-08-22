using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

	public Text avatarName;
	public Text avatarLevel;

	public UITeam TeamWindow;

	public GameObject chatPanel;
	public GameObject chatSwitch;


	// Use this for initialization
	protected override void OnStart()
	{
		this.UpdateAvatar();
	}

	void UpdateAvatar()
	{
		this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
		this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
	}


	// Update is called once per frame
	void Update() {

	}

	public void OnClickBag()
	{
		UIManager.Instance.Show<UIBag>();
	}

	public void OnClickCharEquip()
	{
		UIManager.Instance.Show<UICharEquip>();
	}

	public void OnClickQuest()
	{
		UIManager.Instance.Show<UIQuestSystem>();
	}

	public void OnClickFriends()
	{
		UIManager.Instance.Show<UIFriends>();
	}

	public void OnClickGuild()
	{
		GuildManager.Instance.ShowGuild();
	}

	public void OnClickRide()
	{
		UIManager.Instance.Show<UIRide>();
	}

	public void OnClickSetting()
	{
		UIManager.Instance.Show<UISetting>();
	}

	public void OnClickSkill()
	{

	}

	public void ShowTeamUI(bool show)
	{
		TeamWindow.ShowTeam(show);
	}

	void FixedUpdate()
	{
		if (chatPanel.activeInHierarchy == true && Input.GetKeyDown(KeyCode.Escape))
		{
			chatPanel.SetActive(false);
			chatSwitch.SetActive(true);
		}
	}

	public void OnClickOpenChat()
	{
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		this.OpenChat();
	}

	public void OpenChat()
    {
		chatPanel.SetActive(true);
		chatSwitch.SetActive(false);
	}
}

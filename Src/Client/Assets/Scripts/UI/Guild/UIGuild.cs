using Managers;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuild : UIWindow {

	public GameObject itemPrefab;
	public ListView listMain;
	public Transform itemRoot;
	public UIGuildInfo uiInfo;
	public UIGuildMemberItem selectedMemberItem;

	public GameObject panelAdmin;
	public GameObject panelLeader;

	// Use this for initialization
	void Start () {
		GuildService.Instance.OnGuildUpdate = UpdateUI;
		this.listMain.onItemSelected += this.OnGuildMemberSelected;
		this.UpdateUI();
	}
	
	void OnDestroy()
    {
		GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }

	// Update is called once per frame
	void UpdateUI () {
		this.uiInfo.Info = GuildManager.Instance.guildInfo;

		ClearList();
		InitItems();

		this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
		this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
	}

	public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
		this.selectedMemberItem = item as UIGuildMemberItem;
    }


	/// <summary>
	/// 初始化所有公会成员列表
	/// </summary>
	void InitItems()
    {
		foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
			GameObject go = Instantiate(itemPrefab, this.listMain.transform);
			UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
			ui.SetGuildMemberInfo(item);
			this.listMain.AddItem(ui);
        }
    }

	void ClearList()
    {
		this.listMain.RemoveAll();
    }

	public void OnClickAppliesList()
    {
		UIManager.Instance.Show<UIGuildApplyList>();
    }
	public void OnClickLeave()
    {
		MessageBox.Show("扩展作业");
    }
	public void OnClickChat()
    {
		if (selectedMemberItem == null)
		{
			MessageBox.Show("请选择要私聊的成员");
			return;
		}

		if (selectedMemberItem.Info.characterId == Models.User.Instance.CurrentCharacter.Id)
        {
			MessageBox.Show("不可以和自己聊天哦~");
			return;
		}

		UIMain.Instance.OpenChat();
		ChatManager.Instance.StartPrivateChat(selectedMemberItem.Info.characterId, selectedMemberItem.Info.Info.Name);
		this.Close(WindowResult.None);
	}
	public void OnClickKickout()
    {
		if (selectedMemberItem == null)
        {
			MessageBox.Show("请选择要剔除的成员");
			return;
        }
		MessageBox.Show(string.Format("要踢【{0}】出公会吗？", this.selectedMemberItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
		{
			GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedMemberItem.Info.Info.Id);
		};
    }
	public void OnClickPromote()
    {
		if (selectedMemberItem == null)
        {
			MessageBox.Show("请选择晋升的成员");
			return;
        }
		if (selectedMemberItem.Info.Title != GuildTitle.None)
        {
			MessageBox.Show("对方已经身份尊贵");
        }
		MessageBox.Show(string.Format("要晋升【{0}】为公会副会长吗？", this.selectedMemberItem.Info.Info.Name), "晋升", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
		{
			GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedMemberItem.Info.Info.Id);
		};
    }
	public void OnClickDepose()
    {
		if (selectedMemberItem == null)
        {
			MessageBox.Show("请选择要罢免的成员");
			return;
        }
		if (selectedMemberItem.Info.Title == GuildTitle.None)
        {
			MessageBox.Show("对方貌似无职可免");
			return;
        }
		if (selectedMemberItem.Info.Title == GuildTitle.President)
        {
			MessageBox.Show("会长不是你能动的");
			return;
        }

		MessageBox.Show(string.Format("要罢免【{0}】的公会职务吗？", this.selectedMemberItem.Info.Info.Name), "罢免职务", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
		{
			GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedMemberItem.Info.Info.Id);
		};
    }

	public void OnClickTransfer()
    {
		if (selectedMemberItem == null)
        {
			MessageBox.Show("请选择要把会长转让给的成员");
			return;
        }
		MessageBox.Show(string.Format("要把会长转给【{0}】吗？", this.selectedMemberItem.Info.Info.Name), "转移会长", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
		{
			GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedMemberItem.Info.Info.Id);
		};
    }

	public void OnClickSetNotice()
    {
		MessageBox.Show("待开发");
    }
}

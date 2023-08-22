using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSystem : UIWindow {

	public Text title;

	public GameObject questPrefab;

	public TabView Tabs;
	public ListView listMain;
	public ListView listBranch;
	public ListView listComplete;
	public ListView listFinish;

	public GameObject mainAndBranchListPanel;
	public GameObject finishListPanel;

	public GameObject buttonAbandon;
	public GameObject questInfoPanel;


	public UIQuestInfo questInfo;


	public ShowQuestStatus showQuestList;
	public enum ShowQuestStatus
    {
		Acceptable,
		InProgress,
		Finish,
    }
	void Start()
    {
		this.listMain.onItemSelected += this.OnQuestSelected;
		this.listBranch.onItemSelected += this.OnQuestSelected;
		this.listComplete.onItemSelected += this.OnQuestSelected;
		this.listFinish.onItemSelected += this.OnQuestSelected;
		this.Tabs.OnTabSelect += OnSelectTab;
		RefreshUI();
		//QuestManager.Instance.OnQuestChanged += RefreshUI;
    }

	void OnSelectTab(int idx)
    {
		showQuestList = (ShowQuestStatus)idx;
		questInfoPanel.SetActive(false);
		RefreshUI();
    }
	private void OnDestroy()
    {
		//QuestManager.Instance.OnQuestChanged -= RefreshUI;
    }

	void RefreshUI()
    {
		ClearAllQuestList();
		InitPanel();
		InitAllQuestItems();
    }

	///<summary>
	///初始化所有任务列表
	/// </summary>
	/// <returns></returns>
	void InitAllQuestItems()
    {
		foreach (var kv in QuestManager.Instance.allQuests)
        {
			if (showQuestList == ShowQuestStatus.Acceptable)
			{
				if (kv.Value.Info == null)
                {
					QuestItemInstantiate(kv.Value, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
                }
			}
            else if(showQuestList == ShowQuestStatus.InProgress)
            {
				if (kv.Value.Info != null && kv.Value.Info.Status == SkillBridge.Message.QuestStatus.InProgress)
                {
					QuestItemInstantiate(kv.Value, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
				}
			}
            else
            {
				if (kv.Value.Info != null)
				{
					if (kv.Value.Info.Status == SkillBridge.Message.QuestStatus.Completed)
					{
						QuestItemInstantiate(kv.Value, this.listComplete.transform);
					}
					else
					{
						QuestItemInstantiate(kv.Value, this.listFinish.transform);
					}
				}
            }
        }
    }

	void InitPanel()
    {
		if (showQuestList == ShowQuestStatus.Finish)
		{
			if (!finishListPanel.activeInHierarchy)
			{
				mainAndBranchListPanel.SetActive(false);
				finishListPanel.SetActive(true);

			}
		}
		else
		{
			if (!mainAndBranchListPanel.activeInHierarchy)
			{
				mainAndBranchListPanel.SetActive(true);
				finishListPanel.SetActive(false);
			}
		}
	}

	void QuestItemInstantiate(Quest quest, Transform transform)
    {
		GameObject go = Instantiate(questPrefab, transform);
		UIQuestItem ui = go.GetComponent<UIQuestItem>();
		ui.SetQuestInfo(quest);
		if (showQuestList == ShowQuestStatus.Acceptable || showQuestList == ShowQuestStatus.InProgress)
		{

			if (quest.Define.Type == QuestType.Main)
			{
				this.listMain.AddItem(ui);
			}
			else
			{
				this.listBranch.AddItem(ui);
			}
        }
        else
        {
			if (quest.Info.Status == SkillBridge.Message.QuestStatus.Completed)
            {
				this.listComplete.AddItem(ui);
            }
            else
            {
				this.listFinish.AddItem(ui);
            }
		}
	}

	void ClearAllQuestList()
    {
		this.listMain.RemoveAll();
		this.listBranch.RemoveAll();
		this.listComplete.RemoveAll();
		this.listFinish.RemoveAll();
    }

	public void OnQuestSelected(ListView.ListViewItem item)
    {
		UIQuestItem questItem = item as UIQuestItem;
		if (showQuestList == ShowQuestStatus.Finish)
        {
			if (questItem.quest.Info.Status == SkillBridge.Message.QuestStatus.Completed)
            {
				listFinish.ClearAllSelected();
            }
			else if(questItem.quest.Info.Status == SkillBridge.Message.QuestStatus.Finished)
			{
				listComplete.ClearAllSelected();
            }
        }
        else
        {

			if (questItem.quest.Define.Type == QuestType.Main)
			{
				listBranch.ClearAllSelected();
			}
			else if (questItem.quest.Define.Type == QuestType.Branch)
			{
				listMain.ClearAllSelected();
			}
		}

		if (showQuestList == ShowQuestStatus.InProgress && !buttonAbandon.activeInHierarchy)
		{
			buttonAbandon.SetActive(true);
        }
        else
        {
			buttonAbandon.SetActive(false);
        }

		this.questInfo.SetQuestInfo(questItem.quest);
    }
}

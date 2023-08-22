using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour {

	public Text title;

	public Text[] targets;

	public Text description;

	public Text overview;

	public Transform rewardItems;
	public GameObject itemPrefab;

	public Text rewardMoney;
	public Text rewardExp;

	public GameObject questInfoPanel;

	public Button abandonButton;

	public Button navButton;
	private int npc = 0;

	// Use this for initialization
	void Start () {
		
	}

	public void SetQuestInfo(Quest quest)
	{
		if(questInfoPanel != null)
        {
			this.questInfoPanel.SetActive(true);
		}

		this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
		if (this.overview != null) this.overview.text = quest.Define.Overview;

		if (this.description != null)
		{
			if (quest.Info == null)
			{
				this.description.text = quest.Define.Dialog;
			}
			else
			{
				if (quest.Info.Status == QuestStatus.Completed)
				{
					this.description.text = quest.Define.DialogFinish;
				}
			}
		}

		this.rewardMoney.text = quest.Define.RewardGold.ToString();
		this.rewardExp.text = quest.Define.RewardExp.ToString();

		//奖励道具实体化
		int i = 0;
		while(i < rewardItems.childCount)
        {
			Destroy(rewardItems.GetChild(i++).gameObject);
        }

		GameObject go1 = (GameObject)GameObject.Instantiate(itemPrefab, rewardItems);
		var rewardItem1 = go1.GetComponent<UIIconItem>();
		if (quest.Define.RewardItem1 > 0)
		{
			var def1 = DataManager.Instance.Items[quest.Define.RewardItem1];
			rewardItem1.SetMainIcon(def1.Icon, quest.Define.RewardItem1Count.ToString());
		}

		GameObject go2 = (GameObject)GameObject.Instantiate(itemPrefab, rewardItems);
		var rewardItem2 = go2.GetComponent<UIIconItem>();
		if (quest.Define.RewardItem2 > 0)
        {
			var def2 = DataManager.Instance.Items[quest.Define.RewardItem2];
			rewardItem2.SetMainIcon(def2.Icon, quest.Define.RewardItem2Count.ToString());
		}

		GameObject go3 = (GameObject)GameObject.Instantiate(itemPrefab, rewardItems);
		var rewardItem3 = go3.GetComponent<UIIconItem>();
		if (quest.Define.RewardItem3 > 0)
        {
			var def3 = DataManager.Instance.Items[quest.Define.RewardItem3];
			rewardItem3.SetMainIcon(def3.Icon, quest.Define.RewardItem3Count.ToString());
		}



		if (this.abandonButton != null)
		{
			this.abandonButton.gameObject.SetActive(quest.Info != null && (quest.Info.Status == QuestStatus.Completed || quest.Info.Status == QuestStatus.InProgress));
		}

		if (this.navButton != null)
        {
			if (quest.Info == null)
			{
				this.npc = quest.Define.AcceptNPC;
				this.navButton.GetComponentInChildren<Text>().text = "前往领取";
			}
			else if (quest.Info.Status == QuestStatus.Completed)
			{
				this.npc = quest.Define.SubmitNPC;
				this.navButton.GetComponentInChildren<Text>().text = "前往提交";
			}
			this.navButton.gameObject.SetActive(this.npc > 0);
		}



		foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
			fitter.SetLayoutVertical();
        }
    }

	public void OnClickAbandon()
    {

    }

	public void OnClickNav()
    {
		Vector3 pos = NpcManager.Instance.GetNpcController(this.npc).gameObject.transform.position;
		User.Instance.CurrentPlayerInputController.StartNav(pos);
		User.Instance.CurrentPlayerInputController.NpcInteractive = NpcManager.Instance.GetNpcController(this.npc).Interactive;
		UIManager.Instance.Close<UIQuestSystem>();
    }
}

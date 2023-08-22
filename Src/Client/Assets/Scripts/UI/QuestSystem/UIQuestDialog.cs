using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestDialog : UIWindow {

	public UIQuestInfo questInfo;

	public Quest quest;

	public GameObject openButtons;
	public GameObject submitButtons;

	void Start()
    {

    }

	public void SetQuest(Quest quest)
    {
		this.quest = quest;
		this.UpdateQuest();

        //设置按钮显示
		if (this.quest.Info == null)
        {
			openButtons.SetActive(true);
			submitButtons.SetActive(false);
        }
        else
        {
			if (this.quest.Info.Status == QuestStatus.Completed)
            {
				openButtons.SetActive(false);
				submitButtons.SetActive(true);
            }
            else
            {
                openButtons.SetActive(false);
                submitButtons.SetActive(false);
            }
        }
    }

    void UpdateQuest()
    {
        if (this.quest != null)
        {
            if (this.questInfo != null)
            {
                this.questInfo.SetQuestInfo(quest);
            }
        }
    }
}

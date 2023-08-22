﻿using Entities;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager> {

	public GameObject nameBarPrefeb;
	public GameObject npcStatusPrefeb;

	private Dictionary<Transform, GameObject> elementNames = new Dictionary<Transform, GameObject>();
	private Dictionary<Transform, GameObject> elementStatus = new Dictionary<Transform, GameObject>();

    protected override void OnStart()
    {
		nameBarPrefeb.SetActive(false);
    }

	void Upate()
    {

    }

    public void AddCharacterNameBar(Transform owner, Character character)
    {
		GameObject goNameBar = Instantiate(nameBarPrefeb, this.transform);
		goNameBar.name = "NameBar" + character.entityId;
		goNameBar.GetComponent<UIWorldElement>().owner = owner;
		goNameBar.GetComponent<UINameBar>().character = character;
		goNameBar.SetActive(true);
		this.elementNames[owner] = goNameBar;
    }

	public void RemoveCharacterNameBar(Transform owner)
    {
        if (this.elementNames.ContainsKey(owner))
        {
			Destroy(this.elementNames[owner]);
			this.elementNames.Remove(owner);
        }

    }

	public void AddNpcQuestStatus(Transform owner, NpcQuestStatus status)
    {
		if (this.elementStatus.ContainsKey(owner))
        {
            elementStatus[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        else
        {
            GameObject go = Instantiate(npcStatusPrefeb, this.transform);
            go.name = "NpcQuestStatus" + owner.name;
            go.GetComponent<UIWorldElement>().owner = owner;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            go.SetActive(true);
            this.elementStatus[owner] = go;
        }
    }

    public void RemoveNpcQuestStatus(Transform owner)
    {
        if (this.elementStatus.ContainsKey(owner))
        {
            Destroy(this.elementStatus[owner]);
            this.elementStatus.Remove(owner);
        }
    }
}

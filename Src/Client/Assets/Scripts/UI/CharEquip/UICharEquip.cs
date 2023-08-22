using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindow {
	public Text title;
	public Text money;

	public GameObject itemPrefab;
	public GameObject itemEquipedPrefab;

	public Transform itemListRoot;

	public List<Transform> slots;

	public GameObject characterPrefab;
	public Transform root;


	// Use this for initialization
	void Start () {
		RefreshUI();
		EquipManager.Instance.OnEquipChanged += RefreshUI;
		CharacterModelCreate();
	}

	private void OnDestroy()
    {
		EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

	void RefreshUI()
    {
		ClearAllEquipList();
		InitAllEquipItems();
		ClearEquipedList();
		InitEquipedItems();
		this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }
	/// <summary>
	/// 初始化所有装备列表
	/// </summary>
	/// <returns></returns>
	void InitAllEquipItems()
    {
		foreach (var kv in ItemManager.Instance.Items)
        {
			if (kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class)
            {
				//已经装备就不显示了
				if (EquipManager.Instance.Contains(kv.Key))
                {
					continue;
                }
				GameObject go = Instantiate(itemPrefab, itemListRoot);
				UIEquipItem ui = go.GetComponent<UIEquipItem>();
				ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }
    }

	void ClearAllEquipList()
    {
		foreach (var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
			Destroy(item.gameObject);
        }
    }

	void ClearEquipedList()
    {
		foreach (var item in slots)
        {
			if (item.childCount > 0)
            {
				Destroy(item.GetChild(0).gameObject);
            }
        }
    }

	/// <summary>
	/// 初始化已经装备的列表
	/// </summary>
	/// <returns></returns>
	void InitEquipedItems()
    {
		for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
			var item = EquipManager.Instance.EquipsOnBody[i];
			if (item != null)
            {
				GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
				UIEquipItem ui = go.GetComponent<UIEquipItem>();
				ui.SetEquipItem(i, item, this, true);
            }
        }
    }
	public void DoEquip(Item item)
    {
		EquipManager.Instance.EquipItem(item);
    }
	public void UnEquip(Item item)
    {
		EquipManager.Instance.UnEquipItem(item);
    }

	public void CharacterModelCreate()
    {
		UnityEngine.Object prefeb = Resources.Load("SelectCharacter/" + User.Instance.CurrentCharacter.Class);
		GameObject go = (GameObject)GameObject.Instantiate(prefeb, root);
		go.SetActive(true);
    }

	private UIEquipItem selectItem;
	public void SelectEquipItem(UIEquipItem item)
    {
		if(this.selectItem != null)
        {
			this.selectItem.Selected = false;
        }
		this.selectItem = item;
    }
}

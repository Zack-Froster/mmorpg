using Common.Data;
using Managers;
using Models;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow {

	public Text title;
	public Text money;

	public GameObject shopItem;
	ShopDefine shop;
	public Transform[] itemRoot;

	// Use this for initialization
	void Start () {
		StartCoroutine(InitItems());
		//StatusService.Instance.RegisterStatusNotify(StatusType.Money, OnSetMoney);
	}

	IEnumerator InitItems()
    {
		int count = 0;
		int page = 0;
		foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
			if(kv.Value.Status > 0)
            {
				GameObject go = Instantiate(shopItem, itemRoot[page]);
				UIShopItem ui = go.GetComponent<UIShopItem>();
				ui.SetShopItem(kv.Key, kv.Value, this);
				count++;
				if (count >= 10)
                {
					count = 0;
					page++;
					itemRoot[page].gameObject.SetActive(true);
                }
            }
        }
		
		yield return null;
    }
	
	public void SetShop(ShopDefine shop)
    {
		this.shop = shop;
		this.title.text = shop.Name;
		this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

	private UIShopItem selectedItem;
	public void SelectShopItem(UIShopItem item)
    {
		if(selectedItem != null)
        {
			selectedItem.Selected = false;
        }
		selectedItem = item;
    }

	public void OnClickBuy()
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_Shop_Purchase);
		if (this.selectedItem == null)
        {
			MessageBox.Show("请选择要购买的道具", "购买提示");
			return;
        }
		if(this.selectedItem.Item.Type == ItemType.Equip || this.selectedItem.Item.Type == ItemType.Ride)
        {
			if (ItemManager.Instance.Items.ContainsKey(this.selectedItem.Item.ID))
			{
				MessageBox.Show("此道具只可购买一次哦~");
				return;
			}
		}
		if(!ShopManager.Instance.BuyItem(this.shop.ID, this.selectedItem.ShopItemID))
        {
			return;
        }
    }

/*	bool OnSetMoney(NStatus status)
    {
		if (status.Type == StatusType.Money)
		{
			this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
			return true;
		}
		return false;
    }*/
	// Update is called once per frame
	void Update () {

	}
	void FixedUpdate()
    {
		if (this.money.text != User.Instance.CurrentCharacter.Gold.ToString())
		{
			this.money.text = User.Instance.CurrentCharacter.Gold.ToString();

		}
	}
}

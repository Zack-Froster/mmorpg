using Managers;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow {

	public Text descript;
	public GameObject itemPrefab;
	public ListView listMain;
	public Text textButtonRide;
	private UIRideItem selectedItem;

	// Use this for initialization
	void Start () {
		this.listMain.onItemSelected += this.OnItemSelected;
		RefreshUI();
	}

	public void OnDestroy()
    {

    }

	public void OnItemSelected(ListView.ListViewItem item)
    {
		this.selectedItem = item as UIRideItem;
		this.descript.text = this.selectedItem.item.Define.Description;
		if (selectedItem.item.Id == Models.User.Instance.CurrentRide)
		{
			textButtonRide.text = "取消坐骑";
			textButtonRide.color = new Color(0.501f, 1, 0, 1);
        }
        else
        {
			textButtonRide.text = "召唤坐骑";
			textButtonRide.color = new Color(1, 0.843f, 0, 1);
		}
	}

	void RefreshUI()
    {
		ClearItems();
		InitItems();
    }


	void InitItems()
    {
		foreach (var kv in ItemManager.Instance.Items)
        {
			if (kv.Value.Define.Type == ItemType.Ride && 
				(kv.Value.Define.LimitClass == CharacterClass.None || kv.Value.Define.LimitClass == Models.User.Instance.CurrentCharacter.Class)) 
			{
				GameObject go = Instantiate(itemPrefab, this.listMain.transform);
				UIRideItem ui = go.GetComponent<UIRideItem>();
				ui.SetEquipItem(kv.Value, this, false);
				if (Models.User.Instance.CurrentRide == ui.item.Id)
                {
					ui.Selected = true;
					this.listMain.SelectedItem = ui;
					this.selectedItem = ui;
                }
				this.listMain.AddItem(ui);
            }
        }
    }

	void ClearItems()
    {
		this.listMain.RemoveAll();
    }

	public void OnRideClick()
    {
		if (this.selectedItem == null)
        {
			MessageBox.Show("请选择要召唤的坐骑", "提示");
			return;
        }
		Models.User.Instance.Ride(this.selectedItem.item.Id);

		textButtonRide.text = "取消坐骑";
		textButtonRide.color = new Color(0.501f, 1, 0, 1);
	}
	
}

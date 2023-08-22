using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRideItem : ListView.ListViewItem {

	public Image icon;
	public Text title;
	public Text limitClass;
	public Text categry;
	public Text level;

	public Image background;
	public Sprite normalBg;
	public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
		this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

	public Item item;

	
	public void SetEquipItem(Item item, UIRide owner, bool equiped)
    {
		this.item = item;
		if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
		if (this.title != null) this.title.text = this.item.Define.Name;
		if (this.limitClass != null) this.limitClass.text = this.item.Define.LimitClass == SkillBridge.Message.CharacterClass.None ? "无限职" : this.item.Define.LimitClass.ToString();
		if (this.categry != null) this.categry.text = this.item.Define.Category.ToString();
		if (this.level != null) this.level.text = "Lv." + item.Define.Level.ToString();
    }
}

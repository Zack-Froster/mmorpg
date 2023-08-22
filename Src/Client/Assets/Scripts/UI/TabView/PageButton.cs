using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageButton : MonoBehaviour {

	public PageTabView pageTabView;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPageAdd()
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		if (this.pageTabView.index >= 0 && this.pageTabView.index < this.pageTabView.tabPages.Length - 1)
		{
			this.pageTabView.SelectTab(this.pageTabView.index + 1);
		}
		return;
	}

	public void OnPageReduce()
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		if (this.pageTabView.index > 0 && this.pageTabView.index < this.pageTabView.tabPages.Length)
		{
			this.pageTabView.SelectTab(this.pageTabView.index - 1);
		}
		return;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTabView : MonoBehaviour {

	public PageButton[] tabButtons;
	public GameObject[] tabPages;
	public Text pageText;

	public int index = -1;

	// Use this for initialization
	IEnumerator Start () {
		for(int i = 0; i < tabButtons.Length; i++)
        {
			tabButtons[i].pageTabView = this;
        }
		yield return new WaitForEndOfFrame();
		SelectTab(0);
	}

	public void SelectTab(int index)
    {
		if(this.index != index)
        {
			for(int i = 0; i < tabPages.Length; i++)
            {
				tabPages[i].SetActive(i == index);
            }
			this.index = index;
        }
		this.pageText.text = this.index + 1 + "/" + tabPages.Length;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

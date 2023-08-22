using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {

	public GameObject []charaters;

	private int currentCharacter = 0;

	public int CurrentCharacter 
	{ 
		get {
			return currentCharacter;
		} 
		set
		{
			this.currentCharacter = value;
			this.UpdateCharacter();
		}
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateCharacter()
    {
		for (int i = 0; i < 3; i++)
        {
			charaters[i].SetActive(i == this.currentCharacter);

		}
    }
}

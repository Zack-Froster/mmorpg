using Managers;
using Models;
using Services;
using SkillBridge.Message;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelect : MonoBehaviour {


	public GameObject panelCreate;
	public GameObject panelSelect;

	public GameObject btnCreateCancel;

	public InputField charName;
	CharacterClass charClass;
	public Transform uiCharList;
	public GameObject uiCharInfo;

	public List<GameObject> uiChars = new List<GameObject>();

	public Image[] titles;
	public Text descs;

	public Text[] names;

	private int selectCharacterIdx = -1;

	public UICharacterView characterView;

	public Button btnPlay;

	void Start () 
	{
		btnPlay.onClick.AddListener(OnClickPlay);
		InitCharacterSelect(true);
		UserService.Instance.OnCharacterCreate = OnCharacterCreate;
	}
	
	public void InitCharacterSelect(bool init)
    {
		panelCreate.SetActive(false);
		panelSelect.SetActive(true);

		if(init)
        {
			foreach (var old in uiChars)
            {
				Destroy(old);
            }
			uiChars.Clear();

			Debug.LogFormat("Count:{0}", User.Instance.Info.Player.Characters);

			for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {
				GameObject go = Instantiate(uiCharInfo, this.uiCharList);
				UICharInfo charInfo = go.GetComponent<UICharInfo>();
				charInfo.info = User.Instance.Info.Player.Characters[i];

				Button button = go.GetComponent<Button>();
				int idx = i;
				button.onClick.AddListener(() =>
				{
					SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
					OnSelectCharacter(idx);
				});
				uiChars.Add(go);
				go.SetActive(true);
            }
        }
    }

	public void OnClickCharacterCreate()
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		panelCreate.SetActive(true);
		panelSelect.SetActive(false);
    }

	public void OnClickCreate()
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		if (string.IsNullOrEmpty(this.charName.text))
        {
			MessageBox.Show("请输入角色名称");
			return;
        }
		UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass);
    }

	public void OnSelectClass(int charClass)
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);

		this.charClass = (CharacterClass)charClass;
		characterView.CurrentCharacter = charClass - 1;

		for (int i = 0; i < 3; i++)
		{
			titles[i].gameObject.SetActive(i == charClass - 1);
			names[i].text = DataManager.Instance.Characters[i + 1].Name;
		}
		descs.text = DataManager.Instance.Characters[charClass].Description;
	}

	void OnCharacterCreate(Result result, string message)
    {
		if(result == Result.Success)
        {
			InitCharacterSelect(true);
        }
		else
        {
			MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
    }
	public void OnSelectCharacter(int idx)
    {
		this.selectCharacterIdx = idx;
		var cha = User.Instance.Info.Player.Characters[idx];
		Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
		characterView.CurrentCharacter = (int)cha.Class - 1;

		for(int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
			this.uiChars[i].GetComponent<UICharInfo>().Selected = idx == i;
        }
    }

	public void OnClickPlay()
	{
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		if (selectCharacterIdx >= 0)
		{
			btnPlay.onClick.RemoveAllListeners();
			UserService.Instance.SendGameEnter(selectCharacterIdx);
		}
	}

	public void OnClickBack()
    {
		SoundManager.Instance.PlaySound(SoundDefine.SFX_Back);
		panelCreate.SetActive(false);
		panelSelect.SetActive(true);
	}

}

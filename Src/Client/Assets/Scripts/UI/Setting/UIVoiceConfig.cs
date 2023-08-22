using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVoiceConfig : MonoBehaviour {

	public Image musicOff;
	public Image soundOff;

	public Toggle toggleMusic;
	public Toggle toggleSound;
	public Slider sliderMusic;
	public Slider sliderSound;

	public bool musicOriginIsOn;
	public bool soundOriginIsOn;
	public int musicOriginValue;
	public int soundOriginValue;

	public GameObject setting;

	public bool isChanged = false;


	// Use this for initialization

	public void Start()
    {
		this.toggleMusic.isOn = Config.MusicOn;
		this.toggleSound.isOn = Config.SoundOn;
		this.sliderMusic.value = Config.MusicVolume;
		this.sliderSound.value = Config.SoundVolume;
    }

	public void OnEnable()
    {
		this.musicOriginIsOn = Config.MusicOn;
		this.soundOriginIsOn = Config.SoundOn;
		this.musicOriginValue = Config.MusicVolume;
		this.soundOriginValue = Config.SoundVolume;
	}


	public void MusicToggle(bool on)
    {
		musicOff.enabled = !on;
		Config.MusicOn = on;
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		if(Config.MusicOn != this.musicOriginIsOn)
        {
			isChanged = true;
        }
        else
        {
			isChanged = false;
		}
	}

	public void SoundToggle(bool on)
    {
		soundOff.enabled = !on;
		Config.SoundOn = on;
		SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
		if (Config.SoundOn != this.soundOriginIsOn)
		{
			isChanged = true;
		}
		else
		{
			isChanged = false;
		}
	}
	
	public void MusicVolume(float vol)
    {
		Config.MusicVolume = (int)vol;
		PlaySound();
		if (Config.MusicVolume != this.musicOriginValue)
		{
			isChanged = true;
		}
		else
		{
			isChanged = false;
		}
	}

	public void SoundVolume(float vol)
    {
		Config.SoundVolume = (int)vol;
		PlaySound();
		if (Config.SoundVolume != this.soundOriginValue)
		{
			isChanged = true;
		}
		else
		{
			isChanged = false;
		}
	}

	float lastPlay = 0;
	private void PlaySound()
    {
		if (Time.realtimeSinceStartup - lastPlay > 0.1)
        {
			lastPlay = Time.realtimeSinceStartup;
			SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }

    public void OnDisable()
    {
        if (isChanged && setting.activeInHierarchy)
        {
            var confirm = MessageBox.Show("是否保存更改的设置？", "游戏声音", MessageBoxType.Confirm, "保存", "取消");
            confirm.OnYes = () =>
            {
                PlayerPrefs.Save();
                isChanged = false;
            };
            confirm.OnNo = () =>
            {
                toggleMusic.isOn = musicOriginIsOn;
                toggleSound.isOn = soundOriginIsOn;
                sliderMusic.value = musicOriginValue;
                sliderSound.value = soundOriginValue;

                Config.MusicOn = musicOriginIsOn;
                Config.SoundOn = soundOriginIsOn;
                Config.MusicVolume = musicOriginValue;
                Config.SoundVolume = soundOriginValue;
                PlayerPrefs.Save();
                isChanged = false;
			};
        }
    }

}

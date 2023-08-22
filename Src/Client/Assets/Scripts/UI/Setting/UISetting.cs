using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindow {

    public UIGameConfig gameConfig;
    public UIVoiceConfig voiceConfig;


	public void OnEnable()
    {
		this.OnClose += OnVoiceConfigChanged;
    }


    public void OnVoiceConfigChanged(UIWindow sender, UIWindow.WindowResult result)
    {
        UISetting setting = (UISetting)sender;

        if (setting.voiceConfig.isChanged)
        {
            var confirm = MessageBox.Show("是否保存更改的设置？", "设置", MessageBoxType.Confirm, "保存", "取消");
            confirm.OnYes = () =>
            {
                PlayerPrefs.Save();
                int test = Config.MusicVolume;
                setting.voiceConfig.isChanged = false;
            };
            confirm.OnNo = () =>
            {
                setting.voiceConfig.toggleMusic.isOn = setting.voiceConfig.musicOriginIsOn;
                setting.voiceConfig.toggleSound.isOn = setting.voiceConfig.soundOriginIsOn;
                setting.voiceConfig.sliderMusic.value = setting.voiceConfig.musicOriginValue;
                setting.voiceConfig.sliderSound.value = setting.voiceConfig.soundOriginValue;

                Config.MusicOn = setting.voiceConfig.musicOriginIsOn;
                Config.SoundOn = setting.voiceConfig.soundOriginIsOn;
                Config.MusicVolume = setting.voiceConfig.musicOriginValue;
                Config.SoundVolume = setting.voiceConfig.soundOriginValue;
                PlayerPrefs.Save();
                int test = Config.MusicVolume;
                setting.voiceConfig.isChanged = false;
            };
        }


    }

}

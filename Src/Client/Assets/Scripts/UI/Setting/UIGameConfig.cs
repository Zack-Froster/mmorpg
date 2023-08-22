using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameConfig : MonoBehaviour {

    public void ExitToCharSelect()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        SceneManager.Instance.LoadScene("CharSelect");
        SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        Services.UserService.Instance.SendGameLeave();
    }

    public void ExitToGameLogin()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        SceneManager.Instance.LoadScene("Loading");
        SoundManager.Instance.PlayMusic(SoundDefine.Music_Login);
        Services.UserService.Instance.SendGameLeave();
    }

    public void ExitGame()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        Services.UserService.Instance.SendGameLeave(true);
    }
}

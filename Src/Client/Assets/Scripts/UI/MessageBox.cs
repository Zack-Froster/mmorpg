using UnityEngine;

class MessageBox
{
    static Object cacheObject = null;

    public static UIMessageBox Show(string message, string title="", MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {

        switch (type)
        {
            case MessageBoxType.Confirm:
                SoundManager.Instance.PlaySound(SoundDefine.SFX_Message_Info);
                break;
            case MessageBoxType.Information:
                SoundManager.Instance.PlaySound(SoundDefine.SFX_Message_Info);
                break;
            case MessageBoxType.Error:
                SoundManager.Instance.PlaySound(SoundDefine.SFX_Message_Error);
                break;
        }

        if(cacheObject==null)
        {
            cacheObject = Resloader.Load<Object>("UI/UIMessageBox");
        }

        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        UIMessageBox msgbox = go.GetComponent<UIMessageBox>();
        msgbox.Init(title, message, type, btnOK, btnCancel);
        return msgbox;
    }
}

public enum MessageBoxType
{
    /// <summary>
    /// Information Dialog with OK button
    /// </summary>
    Information = 1,

    /// <summary>
    /// Confirm Dialog whit OK and Cancel buttons
    /// </summary>
    Confirm = 2,

    /// <summary>
    /// Error Dialog with OK buttons
    /// </summary>
    Error = 3
}
using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopCharMenu : UIWindow, IDeselectHandler
{
    public int targetId;
    public string targetName;

    public void OnDeselect(BaseEventData eventData)
    {
        var ed = eventData as PointerEventData;
        if (ed.hovered.Contains(this.gameObject))
        {
            return;
        }
        this.Close(WindowResult.None);
    }

    public void OnEnable()
    {
        this.GetComponent<Selectable>().Select();
        this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0);
    }

    public void OnChat()
    {
        ChatManager.Instance.StartPrivateChat(targetId, targetName);
        this.Close(WindowResult.None);
    }

    public void OnAddFriend()
    {

        if (targetId == Models.User.Instance.CurrentCharacter.Id)
        {
            MessageBox.Show("不可以添加自己为好友哦~");
        }

        MessageBox.Show(string.Format("确定要添加【{0}】为好友么", targetName), "添加好友", MessageBoxType.Confirm, "添加", "取消").OnYes = () =>
        {
            FriendService.Instance.SendFriendAddRequest(targetId, targetName);
        };
        this.Close(WindowResult.None);
    }

    public void OnInviteTeam()
    {
        if (targetId == Models.User.Instance.CurrentCharacter.Id)
        {
            MessageBox.Show("不可以邀请自己进入队伍哦~");
        }

        MessageBox.Show(string.Format("确定要邀请【{0}】进入队伍么", targetName), "邀请入队", MessageBoxType.Confirm, "邀请", "取消").OnYes = () =>
        {
            TeamService.Instance.SendTeamInviteRequest(targetId, targetName);
        };

        this.Close(WindowResult.None);
    }


}

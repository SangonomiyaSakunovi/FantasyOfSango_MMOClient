using SangoCommon.Classs;
using SangoCommon.Constants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The Main Game Window.

public class MainGameWindow : BaseWindow
{
    public GameObject miniMapBaseLocation;
    public GameObject miniMapLocals;

    private float miniMapScaling = 10;
    private Vector3 HomePos = new Vector3(5.568432f, 0, -21.45944f);
    private Vector3 HillPos = new Vector3(88.04138f, 0, 678.8067f);
    private Vector3 miniHomePos = new Vector3(-8, 198, 0);
    private Vector3 miniHillPos = new Vector3(-115.7629f, -693.5677f, 0);
    private float xChange = 265;
    private float yChange = 930;

    public TMP_Text mainHpText;
    public TMP_Text mainLevelText;
    public Image mainHpFG;
    public Image mainElementBurstFG;

    public TMP_Text guidMissionText;

    public Button avaterInfoButton;
    public Button viewChatButton;
    public Button shopButton;

    public TMP_Text newUnreadChatNotice;

    protected override void InitWindow()
    {
        base.InitWindow();
        miniMapScaling = Vector3.Distance(HomePos, HillPos) / Vector3.Distance(miniHomePos, miniHillPos);
        SetCurrentMission("Main_Pre_01");
    }

    private void Update()
    {
        if (Input.GetButtonDown("OpenDialogWindow"))
        {
            OnGetMissionButtonClick();
        }
        if (Input.GetButtonDown("OpenAvaterInfoWindow"))
        {
            OnAvaterInfoButtonClick();
        }
    }

    public void SetMiniMapTransPosition(Transform playerTrans)
    {
        float moveX = (playerTrans.position.x - HomePos.x) / miniMapScaling;
        float moveY = (playerTrans.position.z - HomePos.z) / miniMapScaling;
        miniMapBaseLocation.transform.position = new Vector3(miniHomePos.x - moveX + xChange, miniHomePos.y - moveY + yChange, 0);
        Vector3 rotations = playerTrans.rotation.eulerAngles;
        miniMapLocals.transform.rotation = Quaternion.Euler(0, 0, -rotations.y);
    }


    public void RefreshMissionUI()
    {

    }

    public void RefreshMainAvaterUI(AvaterAttributeInfo info)
    {
        SetText(mainHpText, info.HP + " / " + info.HPFull, TextColorCode.WhiteColor);
        mainHpFG.fillAmount = (float)info.HP / info.HPFull;
        mainElementBurstFG.fillAmount = (float)info.MP / info.MPFull;
    }

    public void RefreshBackAvaterUI()
    {

    }

    public void SetGuidMissionText()
    {
        string text = MissionUpdateSystem.Instance.GetGuidMissionText();
        SetText(guidMissionText, text, TextColorCode.WhiteColor);
    }

    public void OnAutoFindPathButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        MissionUpdateSystem.Instance.AutoFindMissionPath();
    }

    public void OnAvaterInfoButtonClick()
    {
        AvaterInfoSystem.Instance.OpenAvaterInfoWindow();
    }

    public void OnShopButtonClick()
    {
        ShopInfoSystem.Instance.OpenShopWindow();
    }

    public void OnViewChatButtonClick()
    {
        if (GameManager.Instance.GameMode == GameModeCode.GamePlayMode)
        {
            ChatSystem.Instance.OpenChatWindow();
        }
        else
        {
            ChatSystem.Instance.CloseChatWindow();
        }        
    }

    public void OnGetMissionButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        if (MissionUpdateSystem.Instance.CurrentMissionConfig != null)
        {
            MissionUpdateSystem.Instance.RunMission();
        }
        else
        {
            SangoRoot.AddMessage("更多任务，珊瑚忆梦制作组正在开发中，下次再来探索吧~", TextColorCode.CyanColor);
        }
    }

    public void SetCurrentMission(string id)
    {
        MissionUpdateSystem.Instance.SetCurrentMission(id);
    }

    public void SetNewUnreadChatNotice(bool hasUnread = true)
    {
        if (hasUnread)
        {
            SetText(newUnreadChatNotice, "您有新的未读消息", TextColorCode.WhiteColor);
        }
        else
        {
            SetText(newUnreadChatNotice, "");
        }
    }
}

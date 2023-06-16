using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi

public class ChatSystem : BaseSystem
{
    public static ChatSystem Instance = null;

    public ChatWindow chatWindow;
    public MainGameWindow mainGameWindow;

    private ChatRequest chatRequest;

    private Queue<OnlineAccountChatMessage> allAccountChatMessageQueue = new Queue<OnlineAccountChatMessage>();
    private int maxHistoryUnreadChatNumber = 30;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        chatRequest = GetComponent<ChatRequest>();
    }

    public void OpenChatWindow()
    {
        GameManager.Instance.SetGameMode(GameModeCode.ChatMode);
        GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Show);
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        mainGameWindow.SetNewUnreadChatNotice(false);
        chatWindow.SetWindowState(true);
        foreach(var item in allAccountChatMessageQueue)
        {
            chatWindow.SetOnlineAccountChatBubbleMessage(item);
        }
    }

    public void CloseChatWindow()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        chatWindow.SetWindowState(false);
        GameManager.Instance.SetGameMode(GameModeCode.GamePlayMode);
        GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Hide);
    }

    public void SendChatMessage(string message)
    {
        if (message == "")
        {
            SangoRoot.AddMessage("������Ϣ����Ϊ��", TextColorCode.OrangeColor);
        }
        else if (message.Length > 30)
        {
            SangoRoot.AddMessage("������Ϣ���ܳ���30����Ŷ", TextColorCode.OrangeColor);
        }
        else
        {
            chatRequest.SetOnlineAccountChatMessage(message);
            chatRequest.DefaultRequest();
        }
    }

    public void OnSendChatMessageResponse(ReturnCode returnCode)
    {
        if (returnCode != ReturnCode.Success)
        {
            SangoRoot.AddMessage("�������������˴�����Ϣ����ʧ��", TextColorCode.RedColor);
        }
    }

    public void SetOnlineAccountChatMessage(OnlineAccountChatMessage onlineAccountChatMessage)
    {
        if (chatWindow.gameObject.activeSelf == true)
        {
            if (onlineAccountChatMessage.Account == OnlineAccountCache.Instance.LocalAccount)
            {
                chatWindow.SetLocalAccountChatBubbleMessage(onlineAccountChatMessage);
            }
            else
            {
                chatWindow.SetOnlineAccountChatBubbleMessage(onlineAccountChatMessage);
            }
        }
        else
        {
            mainGameWindow.SetNewUnreadChatNotice(true);                        
        }
        allAccountChatMessageQueue.Enqueue(onlineAccountChatMessage);
        if (allAccountChatMessageQueue.Count > maxHistoryUnreadChatNumber)
        {
            allAccountChatMessageQueue.Dequeue();
        }
    }
}

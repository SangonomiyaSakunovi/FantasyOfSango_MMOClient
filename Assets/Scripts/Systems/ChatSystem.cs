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
            SangoRoot.AddMessage("输入信息不能为空", TextColorCode.OrangeColor);
        }
        else if (message.Length > 30)
        {
            SangoRoot.AddMessage("输入信息不能超过30个字哦", TextColorCode.OrangeColor);
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
            SangoRoot.AddMessage("服务器君出现了错误，信息发送失败", TextColorCode.RedColor);
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

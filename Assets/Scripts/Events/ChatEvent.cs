using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class ChatEvent : BaseEvent
{
    public override void InitEvent()
    {
        NetOpCode = OperationCode.Chat;
        base.InitEvent();       
    }

    public override void OnEvent(SangoNetMessage sangoNetMessage)
    {
        string onlineAccountChatMessageJson = sangoNetMessage.MessageBody.MessageString;
        if (onlineAccountChatMessageJson != null && ChatSystem.Instance != null)
        {
            OnlineAccountChatMessage onlineAccountChatMessage = DeJsonString<OnlineAccountChatMessage>(onlineAccountChatMessageJson);
            ChatSystem.Instance.SetOnlineAccountChatMessage(onlineAccountChatMessage);
        }
    }
}

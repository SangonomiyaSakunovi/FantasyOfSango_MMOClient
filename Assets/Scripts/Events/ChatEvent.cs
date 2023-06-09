using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;

//Developer : SangonomiyaSakunovi
//Discription: 

public class ChatEvent : BaseEvent
{
    public override void InitEvent()
    {
        base.InitEvent();
        EvCode = EventCode.Chat;
    }

    public override void OnEvent(EventData eventData)
    {
        string onlineAccountChatMessageJson = DictTools.GetStringValue(eventData.Parameters, (byte)ParameterCode.OnlineAccountChatMessage);
        if (onlineAccountChatMessageJson != null && ChatSystem.Instance != null)
        {
            OnlineAccountChatMessage onlineAccountChatMessage = DeJsonString<OnlineAccountChatMessage>(onlineAccountChatMessageJson);
            ChatSystem.Instance.SetOnlineAccountChatMessage(onlineAccountChatMessage);
        }
    }
}

using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi

public class ChatRequest : BaseRequest
{
    private OnlineAccountChatMessage onlineAccountChatMessage;

    public override void InitRequset()
    {        
        NetOpCode = OperationCode.Chat;
        base.InitRequset();
    }

    public void SetOnlineAccountChatMessage(string message)
    {
        onlineAccountChatMessage = new OnlineAccountChatMessage
        {
            Account = OnlineAccountCache.Instance.LocalAccount,
            Message = message
        };
    }

    public override void DefaultRequest()
    {
        GameManager.Instance.SetGameMode(GameModeCode.WaitingServerResponseMode);
        string onlineAccountChatMessageJson = SetJsonString(onlineAccountChatMessage);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, onlineAccountChatMessageJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        ReturnCode returnCode = sangoNetMessage.MessageBody.ReturnCode;
        ChatSystem.Instance.OnSendChatMessageResponse(returnCode);
        GameManager.Instance.SetGameMode(GameModeCode.ChatMode);
    }
}

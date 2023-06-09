using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: 

public class ChatRequest : BaseRequest
{
    private OnlineAccountChatMessage onlineAccountChatMessage;

    public override void InitRequset()
    {
        base.InitRequset();
        OpCode = OperationCode.Chat;
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
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        string onlineAccountChatMessageJson = SetJsonString(onlineAccountChatMessage);
        dict.Add((byte)ParameterCode.OnlineAccountChatMessage, onlineAccountChatMessageJson);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        ChatSystem.Instance.OnSendChatMessageResponse(returnCode);
        GameManager.Instance.SetGameMode(GameModeCode.ChatMode);
    }
}

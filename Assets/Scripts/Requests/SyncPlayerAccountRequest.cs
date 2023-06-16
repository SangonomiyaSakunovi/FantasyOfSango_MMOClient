using SangoMMONetProtocol;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi

public class SyncPlayerAccountRequest : BaseRequest
{
    public override void InitRequset()
    {        
        NetOpCode = OperationCode.SyncPlayerAccount;
        base.InitRequset();
    }

    public override void DefaultRequest()
    {
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, "");
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        string tempOnlineAccountJson = sangoNetMessage.MessageBody.MessageString;
        List<string> onlineAccountList = DeJsonString<List<string>>(tempOnlineAccountJson);
        foreach (string account in onlineAccountList)
        {
            IslandOnlineAccountSystem.Instance.InstantiatePlayerCube(account);
        }
    }
}

using ExitGames.Client.Photon;
using SangoCommon.Enums;
using SangoCommon.Tools;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: The Sync Account Request.

public class SyncPlayerAccountRequest : BaseRequest
{
    public override void InitRequset()
    {
        base.InitRequset();
        OpCode = OperationCode.SyncPlayerAccount;
    }

    public override void DefaultRequest()
    {
        NetService.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        string tempOnlineAccountJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.OnlineAccountList);
        List<string> onlineAccountList = DeJsonString<List<string>>(tempOnlineAccountJson);
        foreach (string account in onlineAccountList)
        {
            IslandOnlineAccountSystem.Instance.InstantiatePlayerCube(account);
        }
    }
}

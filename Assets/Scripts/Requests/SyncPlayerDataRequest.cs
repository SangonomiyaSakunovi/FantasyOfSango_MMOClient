using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;

//Developer : SangonomiyaSakunovi
//Discription: The Sync Data Request.

public class SyncPlayerDataRequest : BaseRequest
{
    public string Account { get; private set; }
    public AvaterInfo AvaterInfo { get; private set; }
    public bool IsGetResponse { get; private set; }

    public override void InitRequset()
    {
        base.InitRequset();
        IsGetResponse = false;
    }

    public override void DefaultRequest()
    {
        NetService.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        string avaterInfoJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.AvaterInfo);
        AvaterInfo = DeJsonString<AvaterInfo>(avaterInfoJson);
        IsGetResponse = true;
    }

    public void SetAccoount(string account)
    {
        Account = account;
    }

    public void SetIsGetResponse(bool isGetResponse = false)
    {
        IsGetResponse = isGetResponse;
    }

    public void SetPlayerCache(AvaterInfo avaterInfo)
    {
        AvaterInfo = avaterInfo;
    }
}

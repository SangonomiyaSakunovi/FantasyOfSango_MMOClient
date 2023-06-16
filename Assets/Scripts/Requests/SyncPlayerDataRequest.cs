using SangoMMOCommons.Classs;
using SangoMMONetProtocol;
using System;

//Developer : SangonomiyaSakunovi

public class SyncPlayerDataRequest : BaseRequest
{
    private bool isGetResponse;
    private Action loadingPlayerDataCallBack = null;

    private void Update()
    {
        if (loadingPlayerDataCallBack != null)
        {
            loadingPlayerDataCallBack();
        }
    }

    public override void InitRequset()
    {       
        NetOpCode = OperationCode.SyncPlayerData;
        base.InitRequset();
        isGetResponse = false;
    }

    public override void DefaultRequest()
    {
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, "");
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        string playerDataJson = sangoNetMessage.MessageBody.MessageString;
        PlayerData playerData = DeJsonString<PlayerData>(playerDataJson);
        AvaterInfo AvaterInfo = playerData.AvaterInfo;
        MissionInfo MissionInfo = playerData.MissionInfo;
        ItemInfo itemInfo = playerData.ItemInfo;
        OnlineAccountCache.Instance.SetAvaterInfo(AvaterInfo);
        OnlineAccountCache.Instance.SetMissionInfo(MissionInfo);
        OnlineAccountCache.Instance.SetItemInfo(itemInfo);
        isGetResponse = true;
    }

    public void AsyncLoadPlayerData(Action loadedActionCallBack)
    {
        loadingPlayerDataCallBack = () =>
        {
            if (isGetResponse)
            {
                if (loadedActionCallBack != null)
                {
                    loadedActionCallBack();
                }
                loadingPlayerDataCallBack = null;
                isGetResponse = false;
            }
        };
    }
}

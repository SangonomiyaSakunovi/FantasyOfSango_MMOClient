using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;
using System;

//Developer : SangonomiyaSakunovi
//Discription: The Sync Data Request.

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
        base.InitRequset();
        OpCode = OperationCode.SyncPlayerData;
        isGetResponse = false;
    }

    public override void DefaultRequest()
    {
        NetService.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        string avaterInfoJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.AvaterInfo);
        string missionInfoJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.MissionInfo);
        string itemInfoJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.ItemInfo);
        AvaterInfo AvaterInfo = DeJsonString<AvaterInfo>(avaterInfoJson);
        MissionInfo MissionInfo = DeJsonString<MissionInfo>(missionInfoJson);
        ItemInfo itemInfo = DeJsonString<ItemInfo>(itemInfoJson);
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

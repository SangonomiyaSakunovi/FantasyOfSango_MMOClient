using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class MissionUpdateRequest : BaseRequest
{
    private MissionUpdateReq missionUpdateReq;

    public override void InitRequset()
    {       
        NetOpCode = OperationCode.MissionUpdate;
        base.InitRequset();
    }

    public override void DefaultRequest()
    {
        string missionUpdateRequestJson = SetJsonString(missionUpdateReq);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, missionUpdateRequestJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        string missionUpdateResponseJson = sangoNetMessage.MessageBody.MessageString;
        if (missionUpdateResponseJson != null)
        {
            MissionUpdateRsp missionUpdateRsp = DeJsonString<MissionUpdateRsp>(missionUpdateResponseJson);
            if (missionUpdateRsp.IsCompleteSuccess)
            {
                switch (missionUpdateRsp.MissionUpdateTypeCode)
                {
                    case MissionUpdateTypeCode.Complete:
                        MissionUpdateSystem.Instance.OnMissionCompleteResponse(missionUpdateRsp);
                        break;
                }
            }
            else
            {
                SangoRoot.AddMessage("任务完成失败，请重试", TextColorCode.RedColor);
            }
        }
    }

    public void SetMissionUpdateReq(MissionUpdateReq updateReq)
    {
        missionUpdateReq = updateReq;
    }
}

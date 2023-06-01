using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: 

public class MissionUpdateRequest : BaseRequest
{
    private MissionUpdateReq missionUpdateReq;
    
    public override void InitRequset()
    {
        base.InitRequset();
    }

    public override void DefaultRequest()
    {       
        string missionUpdateRequestJson = SetJsonString(missionUpdateReq);
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        dict.Add((byte)ParameterCode.MissionUpdateReq, missionUpdateRequestJson);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        string missionUpdateResponseJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.MissionUpdateRsp);
        if (missionUpdateResponseJson != null)
        {
            MissionUpdateRsp missionUpdateRsp = DeJsonString<MissionUpdateRsp>(missionUpdateResponseJson);
            if (missionUpdateRsp.IsCompleteSuccess)
            {
                switch (missionUpdateRsp.missionUpdateTypeCode)
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

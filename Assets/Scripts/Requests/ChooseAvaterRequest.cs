using ExitGames.Client.Photon;
using SangoCommon.Enums;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: The Choose Avater Request.

public class ChooseAvaterRequest : BaseRequest
{
    public AvaterCode Avater { get; private set; }

    public override void InitRequset()
    {
        base.InitRequset();
        OpCode = OperationCode.ChooseAvater;
    }
    public override void DefaultRequest()
    {
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        dict.Add((byte)ParameterCode.ChooseAvater, Avater);
        dict.Add((byte)ParameterCode.Account, OnlineAccountCache.Instance.LocalAccount);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }

    public void SetAvater(AvaterCode avater)
    {
        Avater = avater;
    }
}

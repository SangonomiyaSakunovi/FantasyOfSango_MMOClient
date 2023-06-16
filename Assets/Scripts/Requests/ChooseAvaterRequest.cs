using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class ChooseAvaterRequest : BaseRequest
{
    public AvaterCode Avater { get; private set; }

    public override void InitRequset()
    {       
        NetOpCode = OperationCode.ChooseAvater;
        base.InitRequset();
    }
    public override void DefaultRequest()
    {
        ChooseAvaterCode chooseAvaterCode = new()
        {
            Account = OnlineAccountCache.Instance.LocalAccount,
            AvaterCode = Avater
        };
        string chooseAvaterCodeJson = SetJsonString(chooseAvaterCode);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, chooseAvaterCodeJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {

    }

    public void SetAvater(AvaterCode avater)
    {
        Avater = avater;
    }
}

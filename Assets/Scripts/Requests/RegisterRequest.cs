using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class RegisterRequest : BaseRequest
{
    private string account;
    private string password;
    private string nickname;

    public override void InitRequset()
    {       
        NetOpCode = OperationCode.Register;
        base.InitRequset();
    }

    public override void DefaultRequest()
    {
        RegisterReq registerReq = new()
        {
            Account = account,
            Password = password,
            Nickname = nickname
        };
        string registerReqJson = SetJsonString(registerReq);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, registerReqJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        ReturnCode returnCode = sangoNetMessage.MessageBody.ReturnCode;
        RegisterSystem.Instance.OnRegisterResponse(returnCode);
    }

    public void SetAccount(string acct, string pass, string nick)
    {
        account = acct;
        password = pass;
        nickname = nick;
    }
}

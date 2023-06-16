using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class LoginRequest : BaseRequest
{
    private string account;
    private string password;

    public override void InitRequset()
    {       
        NetOpCode = OperationCode.Login;
        base.InitRequset();
    }

    public override void DefaultRequest()
    {
        LoginReq loginReq = new()
        {
            Account = account,
            Password = password
        };
        string loginReqJson = SetJsonString(loginReq);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, loginReqJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        ReturnCode returnCode = sangoNetMessage.MessageBody.ReturnCode;
        LoginSystem.Instance.OnLoginResponse(returnCode);
    }
    public void SetAccount(string acc, string pass)
    {
        account = acc;
        password = pass;
    }
}

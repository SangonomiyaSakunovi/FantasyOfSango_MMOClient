using ExitGames.Client.Photon;
using SangoCommon.Enums;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: The Login Request.

public class LoginRequest : BaseRequest
{
    private string account;
    private string password;

    public override void InitRequset()
    {
        base.InitRequset();
        OpCode = OperationCode.Login;
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        dict.Add((byte)ParameterCode.Account, account);
        dict.Add((byte)ParameterCode.Password, password);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        LoginSystem.Instance.OnLoginResponse(returnCode);
    }
    public void SetAccount(string acc, string pass)
    {
        account = acc;
        password = pass;
    }
}

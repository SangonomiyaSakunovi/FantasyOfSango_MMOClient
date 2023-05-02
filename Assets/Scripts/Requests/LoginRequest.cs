using ExitGames.Client.Photon;
using SangoCommon.Enums;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: The Login Request.

public class LoginRequest : BaseRequest
{
    public string Account { get; private set; }
    public string Password { get; private set; }

    public override void InitRequset()
    {
        base.InitRequset();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        dict.Add((byte)ParameterCode.Account, Account);
        dict.Add((byte)ParameterCode.Password, Password);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        LoginSystem.Instance.OnLoginResponse(returnCode);
    }
    public void SetAccount(string account, string password)
    {
        Account = account;
        Password = password;
    }
}

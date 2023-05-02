using SangoCommon.Enums;

//Developer : SangonomiyaSakunovi
//Discription: The Regist System.

public class RegisterSystem : BaseSystem
{
    public static RegisterSystem Instance = null;

    public RegisterWindow registerWindow;

    private RegisterRequest registerRequest;

    public string Account { get; private set; }
    public string Password { get; private set; }
    public string Nickname { get; private set; }

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        registerRequest = GetComponent<RegisterRequest>();
    }

    public void EnterRegister()
    {
        registerWindow.SetWindowState();
    }

    public void SendRegisterRequest()
    {
        registerRequest.SetAccount(Account, Password, Nickname);
        registerRequest.DefaultRequest();
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            SangoRoot.AddMessage("注册成功，感谢您参加本次测试");
        }
        else
        {
            SangoRoot.AddMessage("注册失败，该用户名已存在，请更换用户名");
        }
    }

    public void SetAccount(string account, string password, string nickname)
    {
        Account = account;
        Password = password;
        Nickname = nickname;
    }
}

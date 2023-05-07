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
            SangoRoot.AddMessage("ע��ɹ�����л���μӱ��β���");
        }
        else
        {
            SangoRoot.AddMessage("ע��ʧ�ܣ����û����Ѵ��ڣ�������û���");
        }
    }

    public void SetAccount(string account, string password, string nickname)
    {
        Account = account;
        Password = password;
        Nickname = nickname;
    }
}

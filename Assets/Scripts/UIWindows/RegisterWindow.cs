using TMPro;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi

public class RegisterWindow : BaseWindow
{
    public TMP_InputField accountInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nicknameInput;
    public Button registerButton;
    public Button returnButton;
    public RegisterWindow registerWindow;
    public LoginWindow loginWindow;

    private RegisterSystem registerSystem = null;

    protected override void InitWindow()
    {
        registerSystem = RegisterSystem.Instance;
        base.InitWindow();
        accountInput.text = "";
        passwordInput.text = "";
        nicknameInput.text = "";
    }

    public void OnRegisterButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        string account = accountInput.text;
        string password = passwordInput.text;
        string nickname = nicknameInput.text;
        if (account != "" && password != "" && nickname != "")
        {
            //Send Request
            registerSystem.SetAccount(account, password, nickname);
            registerSystem.SendRegisterRequest();
        }
        else
        {
            SangoRoot.AddMessage("亲，信息要输入完整哦", TextColorCode.GreenColor);
        }
    }

    public void OnReturnButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        loginWindow.SetWindowState();
        registerWindow.SetWindowState(false);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi

public class LoginWindow : BaseWindow
{
    public TMP_InputField accountInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public Button autoLoginButton;
    public Button autoLoginButtonPress;
    public Button rememberAccountButton;
    public Button rememberAccountButtonPress;
    public LoginWindow loginWindow;
    public RegisterWindow registerWindow;

    private LoginSystem loginSystem = null;

    protected override void InitWindow()
    {
        loginSystem = LoginSystem.Instance;
        base.InitWindow();
    }

    public void OnLoginButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        string account = accountInput.text;
        string password = passwordInput.text;
        if (account != "" && password != "")
        {
            if (rememberAccountButtonPress.gameObject.activeSelf)
            {
                PlayerPrefs.SetString("Account", account);
            }
            if (autoLoginButtonPress.gameObject.activeSelf)
            {
                PlayerPrefs.SetString("Account", account);
                PlayerPrefs.SetString("Password", password);
            }
            //Send Request
            loginSystem.SetAccount(account, password);
            loginSystem.SendLoginRequest();
        }
        else
        {
            SangoRoot.AddMessage("’À∫≈ªÚ√‹¬ÎŒ™ø’", TextColorCode.RedColor);
        }
    }

    public void OnRegisterButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        registerWindow.SetWindowState();
        loginWindow.SetWindowState(false);
    }

    public void OnAutoLoginButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        SetActive(autoLoginButtonPress);
        SetActive(autoLoginButton, false);
    }

    public void OnRememberAccountButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        SetActive(rememberAccountButtonPress);
        SetActive(rememberAccountButton, false);
    }
    public void OnAutoLoginButtonPressClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        SetActive(autoLoginButton);
        SetActive(autoLoginButtonPress, false);
        PlayerPrefs.DeleteKey("Account");
        PlayerPrefs.DeleteKey("Password");
    }

    public void OnRememberAccountButtonPressClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        SetActive(rememberAccountButton);
        SetActive(rememberAccountButtonPress, false);
        PlayerPrefs.DeleteKey("Account");
    }
}

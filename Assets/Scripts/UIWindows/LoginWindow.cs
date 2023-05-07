using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The Login Window.

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
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
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
            SangoRoot.AddMessage("’À∫≈ªÚ√‹¬ÎŒ™ø’");
        }
    }

    public void OnRegisterButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        registerWindow.SetWindowState();
        loginWindow.SetWindowState(false);
    }

    public void OnAutoLoginButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        SetActive(autoLoginButtonPress);
        SetActive(autoLoginButton, false);
    }

    public void OnRememberAccountButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        SetActive(rememberAccountButtonPress);
        SetActive(rememberAccountButton, false);
    }
    public void OnAutoLoginButtonPressClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        SetActive(autoLoginButton);
        SetActive(autoLoginButtonPress, false);
        PlayerPrefs.DeleteKey("Account");
        PlayerPrefs.DeleteKey("Password");
    }

    public void OnRememberAccountButtonPressClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        SetActive(rememberAccountButton);
        SetActive(rememberAccountButtonPress, false);
        PlayerPrefs.DeleteKey("Account");
    }
}

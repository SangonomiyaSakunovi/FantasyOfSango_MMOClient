using SangoMMONetProtocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi

public class LoginSystem : BaseSystem
{
    public static LoginSystem Instance = null;

    public LoginWindow loginWindow;
    private LoginRequest loginRequest;

    public TMP_InputField accountInput;
    public TMP_InputField passwordInput;
    public Button autoLoginButton;
    public Button autoLoginButtonPress;
    public Button rememberAccountButton;
    public Button rememberAccountButtonPress;

    public string Account { get; private set; }
    public string Password { get; private set; }

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        loginRequest = GetComponent<LoginRequest>();
        GetUserInfo();
    }

    public void EnterLogin()
    {
        //LoadScene Async
        resourceService.AsyncLoadScene(SceneConstant.LoginScene, () =>
        {
            loginWindow.SetWindowState();
            if (Account != null)
            {
                if (Password != null)
                {
                    SendLoginRequest();
                    SetActive(autoLoginButtonPress);
                    SetActive(autoLoginButton, false);
                    accountInput.text = Account;
                    passwordInput.text = Password;
                }
                else
                {
                    SetActive(rememberAccountButtonPress);
                    SetActive(rememberAccountButton, false);
                    accountInput.text = Account;
                }
            }
        });
    }

    public void SendLoginRequest()
    {
        loginRequest.SetAccount(Account, Password);
        loginRequest.DefaultRequest();
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            SangoRoot.AddMessage("登录成功，请进行后续测试", TextColorCode.OrangeColor);
            OnlineAccountCache.Instance.SetLocalAccount(Account);
            //Load the MainGame
            CacheSystem.Instance.syncPlayerDataRequest.DefaultRequest();
            CacheSystem.Instance.syncPlayerDataRequest.AsyncLoadPlayerData(() =>
            {
                MainGameSystem.Instance.EnterIslandScene();
                loginWindow.SetWindowState(false);
            });
        }
        else if (returnCode == ReturnCode.AccountOnline)
        {
            SangoRoot.AddMessage("登录失败，该玩家已在线", TextColorCode.OrangeColor);
        }
        else
        {
            SangoRoot.AddMessage("登录失败，请检查用户名或密码是否匹配", TextColorCode.OrangeColor);
        }
    }

    private void GetUserInfo()
    {
        if (PlayerPrefs.HasKey("Account"))
        {
            Account = PlayerPrefs.GetString("Account");
        }
        if (PlayerPrefs.HasKey("Password"))
        {
            Password = PlayerPrefs.GetString("Password");
        }
    }

    public void SetAccount(string account, string password)
    {
        Account = account;
        Password = password;
    }

    private void SetActive(Button button, bool isActive = true)
    {
        button.transform.gameObject.SetActive(isActive);
    }
}

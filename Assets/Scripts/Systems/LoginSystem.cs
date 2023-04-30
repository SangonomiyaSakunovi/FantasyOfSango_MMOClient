using SangoCommon.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription:

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
            if (this.Account != null)
            {
                if (this.Password != null)
                {
                    SendLoginRequest();
                    Debug.Log(this.Account);
                    Debug.Log(this.Password);
                    SetActive(autoLoginButtonPress);
                    SetActive(autoLoginButton, false);
                    accountInput.text = this.Account;
                    passwordInput.text = this.Password;
                }
                else
                {
                    SetActive(rememberAccountButtonPress);
                    SetActive(rememberAccountButton, false);
                    accountInput.text = this.Account;
                }
            }
        });
        audioService.PlayBGAudio(AudioConstant.LoginAudioBG, true);
    }

    public void SendLoginRequest()
    {
        loginRequest.SetAccount(this.Account, this.Password);
        loginRequest.DefaultRequest();
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            SangoRoot.AddMessage("登录成功，请进行后续测试");
            SetOtherAccount();
            //Load the MainGame
            netService.AsyncLoadPlayerData(() =>
            {
                OnlineAccountCache.Instance.SetPlayerCache(CacheSystem.Instance.syncPlayerDataRequest.AvaterInfo);
                MainGameSystem.Instance.EnterMainGame();
                loginWindow.SetWindowState(false);
            });
        }
        else if (returnCode == ReturnCode.AccountOnline)
        {
            SangoRoot.AddMessage("登录失败，该玩家已在线");
        }
        else
        {
            SangoRoot.AddMessage("登录失败，请检查用户名或密码是否匹配");
        }
    }

    private void GetUserInfo()
    {
        if (PlayerPrefs.HasKey("Account"))
        {
            this.Account = PlayerPrefs.GetString("Account");
        }
        if (PlayerPrefs.HasKey("Password"))
        {
            this.Password = PlayerPrefs.GetString("Password");
        }
    }

    private void SetOtherAccount()
    {
        netService.SetAccount(this.Account);
        CacheSystem.Instance.syncPlayerDataRequest.SetAccoount(this.Account);
        CacheSystem.Instance.syncPlayerTransformRequest.SetAccount(this.Account);
        CacheSystem.Instance.syncPlayerAccountRequest.SetAccount(this.Account);
        CacheSystem.Instance.attackCommandRequest.SetAccount(this.Account);
        CacheSystem.Instance.attackDamageRequest.SetAccount(this.Account);
        CacheSystem.Instance.chooseAvaterRequest.SetAccount(this.Account);
    }

    public void SetAccount(string account, string password)
    {
        Account = account;
        Password = password;
    }
}

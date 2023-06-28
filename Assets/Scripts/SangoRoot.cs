using UnityEngine;

//Developer : SangonomiyaSakunovi

public class SangoRoot : MonoBehaviour
{
    public static SangoRoot Instance = null;

    public DynamicWindow dynamicWindow;

    public ClientConfig clientConfig;

    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        CleanUIWindow();
        InitRoot();
        StartGame();
    }

    private void CleanUIWindow()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        dynamicWindow.SetWindowState();
    }

    private void InitRoot()
    {
        InitConfig();
        InitService();
        InitManager();
        InitSystem();
        InitCache();        
    }

    private void StartGame()
    {
        HotFixService.Instance.PrepareHotFix();
        //HotFixSystem.Instance.PrepareHotFix();
        //LoginSystem.Instance.EnterLogin();
    }

    public void InitConfig()
    {
        clientConfig.InitConfig();
    }

    private void InitService()
    {
        ProxyService proxyService = GetComponent<ProxyService>();
        proxyService.InitService();        
        ResourceService resourceService = GetComponent<ResourceService>();
        resourceService.InitService();
        AudioService audioService = GetComponent<AudioService>();
        audioService.InitService();
        NetService netService = GetComponent<NetService>();
        netService.InitService();
        HotFixService hotFixService = GetComponent<HotFixService>();
        hotFixService.InitService();
    }

    private void InitManager()
    {
        GameManager gameManager = GetComponent<GameManager>();
        gameManager.InitManager();
    }

    private void InitCache()
    {
        OnlineAccountCache onlineAccountCache = GetComponent<OnlineAccountCache>();
        onlineAccountCache.InitCache();
    }

    private void InitSystem()
    {
        CacheSystem cacheSystem = GetComponent<CacheSystem>();
        cacheSystem.InitSystem();
        LoadingSystem loadingSystem = GetComponent<LoadingSystem>();
        loadingSystem.InitSystem();
        LoginSystem loginSystem = GetComponent<LoginSystem>();
        loginSystem.InitSystem();
        RegisterSystem registerSystem = GetComponent<RegisterSystem>();
        registerSystem.InitSystem();
        MainGameSystem mainGameSystem = GetComponent<MainGameSystem>();
        mainGameSystem.InitSystem();
        MissionUpdateSystem missionUpdateSystem = GetComponent<MissionUpdateSystem>();
        missionUpdateSystem.InitSystem();
        AvaterInfoSystem avaterInfoSystem = GetComponent<AvaterInfoSystem>();
        avaterInfoSystem.InitSystem();
        WeaponsEnhanceSystem weaponsEnhanceSystem = GetComponent<WeaponsEnhanceSystem>();
        weaponsEnhanceSystem.InitSystem();
        ChatSystem chatSystem = GetComponent<ChatSystem>();
        chatSystem.InitSystem();
        ShopInfoSystem shopInfoSystem = GetComponent<ShopInfoSystem>();
        shopInfoSystem.InitSystem();
        HotFixSystem hotFixSystem = GetComponent<HotFixSystem>();
        hotFixSystem.InitSystem();
    }

    public static void AddMessage(string message, TextColorCode textColor)
    {
        Instance.dynamicWindow.AddMessage(message, textColor);
    }

    private void OnApplicationQuit()
    {
        NetService.Instance.CloseClientInstance();
    }
}

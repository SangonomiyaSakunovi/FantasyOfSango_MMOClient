using UnityEngine;

//Developer : SangonomiyaSakunovi

public class SangoRoot : MonoBehaviour
{
    public static SangoRoot Instance = null;

    public LoadingWindow loadingWindow;
    public DynamicWindow dynamicWindow;

    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        CleanUIWindow();
        InitRoot();
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
        InitService();
        InitManager();
        InitSystem();
        InitCache();
        LoginSystem.Instance.EnterLogin();
    }

    private void InitService()
    {
        NetService netService = GetComponent<NetService>();
        netService.InitService();
        ResourceService resourceService = GetComponent<ResourceService>();
        resourceService.InitService();
        AudioService audioService = GetComponent<AudioService>();
        audioService.InitService();
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

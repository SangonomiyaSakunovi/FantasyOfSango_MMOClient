using SangoCommon.Enums;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: GameRoot, use monoInstance as Tool for other cs to call

public class SangoRoot : MonoBehaviour
{
    public static SangoRoot Instance = null;

    public LoadingWindow loadingWindow;
    public DynamicWindow dynamicWindow;

    private Dictionary<OperationCode, BaseRequest> RequestDict = new Dictionary<OperationCode, BaseRequest>();

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
        InitRequest();
        InitEvent();
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
        audioService.InitServive();
    }

    private void InitManager()
    {
        GameManager gameManager = GetComponent<GameManager>();
        gameManager.InitManager();
    }

    private void InitRequest()
    {
        LoginRequest loginRequest = GetComponent<LoginRequest>();
        loginRequest.InitRequset();
        RegisterRequest registerRequest = GetComponent<RegisterRequest>();
        registerRequest.InitRequset();
        SyncPlayerDataRequest syncPlayerDataRequest = GetComponent<SyncPlayerDataRequest>();
        syncPlayerDataRequest.InitRequset();
        SyncPlayerTransformRequest syncPlayerTransformRequest = GetComponent<SyncPlayerTransformRequest>();
        syncPlayerTransformRequest.InitRequset();
        SyncPlayerAccountRequest syncPlayerAccountRequest = GetComponent<SyncPlayerAccountRequest>();
        syncPlayerAccountRequest.InitRequset();
        AttackCommandRequest attackCommandRequest = GetComponent<AttackCommandRequest>();
        attackCommandRequest.InitRequset();
        AttackDamageRequest attackDamageRequest = GetComponent<AttackDamageRequest>();
        attackDamageRequest.InitRequset();
        ChooseAvaterRequest chooseAvaterRequest = GetComponent<ChooseAvaterRequest>();
        chooseAvaterRequest.InitRequset();
    }

    private void InitEvent()
    {
        NewAccountJoinEvent newAccountJoinEvent = GetComponent<NewAccountJoinEvent>();
        newAccountJoinEvent.InitEvent();
        SyncPlayerTransformEvent syncPlayerTransformEvent = GetComponent<SyncPlayerTransformEvent>();
        syncPlayerTransformEvent.InitEvent();
        AttackCommandEvent attackCommandEvent = GetComponent<AttackCommandEvent>();
        attackCommandEvent.InitEvent();
        AttackResultEvent attackResultEvent = GetComponent<AttackResultEvent>();
        attackResultEvent.InitEvent();
        ChooseAvaterEvent chooseAvaterEvent = GetComponent<ChooseAvaterEvent>();
        chooseAvaterEvent.InitEvent();
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
        MissionSystem missionSystem = GetComponent<MissionSystem>();
        missionSystem.InitSystem();
        AvaterInfoSystem avaterInfoSystem = GetComponent<AvaterInfoSystem>();
        avaterInfoSystem.InitSystem();
    }

    public static void AddMessage(string message)
    {
        Instance.dynamicWindow.AddMessage(message);
    }

    private void OnApplicationQuit()
    {
        NetService.Instance.OnDestory();
    }
}

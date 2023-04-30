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
        //Init the ServiceModule
        NetService netService = GetComponent<NetService>();
        netService.InitService();
        ResourceService resourceService = GetComponent<ResourceService>();
        resourceService.InitService();
        AudioService audioService = GetComponent<AudioService>();
        audioService.InitServive();

        //Init the Manager
        GameManager gameManager = GetComponent<GameManager>();
        gameManager.InitManager();

        //Init the Request
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


        //Init the Event
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

        //Init the System
        CacheSystem cacheSystem = GetComponent<CacheSystem>();
        cacheSystem.InitSystem();
        LoginSystem loginSystem = GetComponent<LoginSystem>();
        loginSystem.InitSystem();
        RegisterSystem registerSystem = GetComponent<RegisterSystem>();
        registerSystem.InitSystem();
        MainGameSystem mainGameSystem = GetComponent<MainGameSystem>();
        mainGameSystem.InitSystem();

        //Init the Cache
        OnlineAccountCache onlineAccountCache = GetComponent<OnlineAccountCache>();
        onlineAccountCache.InitCache();

        //Enter LoginWindow
        loginSystem.EnterLogin();
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

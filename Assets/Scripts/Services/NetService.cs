using SangoIOCPNet;
using SangoMMOCommons.Tools;
using SangoMMONetProtocol;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi

public class NetService : BaseService
{
    public static NetService Instance;

    public IOCPPeer<ClientPeer> ClientInstance;
    private int localIPPort = 52022;

    private Dictionary<OperationCode, BaseRequest> RequestDict = new Dictionary<OperationCode, BaseRequest>();
    private Dictionary<OperationCode, BaseEvent> EventDict = new Dictionary<OperationCode, BaseEvent>();

    public override void InitService()
    {
        string ipAddress = SetIPAddress(ConfigureModeCode.Offline);
        Instance = this;
        InitIOCPLog();
        InitClientInstance(ipAddress, localIPPort);
        InitRequests();
        InitEvents();
    }

    private void InitClientInstance(string ipAddress, int localIPPort)
    {
        ClientInstance = new IOCPPeer<ClientPeer>();
        ClientInstance.InitClient(ipAddress, localIPPort);
    }

    private void InitIOCPLog()
    {
        IOCPLog.LogInfoCallBack = Debug.Log;
        IOCPLog.LogWarningCallBack = Debug.LogWarning;
        IOCPLog.LogErrorCallBack = Debug.LogError;
        IOCPLog.LogDoneCallBack = Debug.Log;
        IOCPLog.LogProcessingCallBack = Debug.Log;
    }

    public void CloseClientInstance()
    {
        ClientInstance.CloseClient();
    }

    private void InitRequests()
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
        ItemEnhanceRequest itemEnhanceRequest = GetComponent<ItemEnhanceRequest>();
        itemEnhanceRequest.InitRequset();
        MissionUpdateRequest missionUpdateRequest = GetComponent<MissionUpdateRequest>();
        missionUpdateRequest.InitRequset();
        ChatRequest chatRequest = GetComponent<ChatRequest>();
        chatRequest.InitRequset();
        ShopInfoRequest shopInfoRequest = GetComponent<ShopInfoRequest>();
        shopInfoRequest.InitRequset();
    }

    private void InitEvents()
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
        ChatEvent chatEvent = GetComponent<ChatEvent>();
        chatEvent.InitEvent();
    }

    private string SetIPAddress(ConfigureModeCode mode)
    {
        string ipAddress = null;
        if (mode == ConfigureModeCode.Online)
        {
            ipAddress = "124.220.20.98";
        }
        else if (mode == ConfigureModeCode.Offline)
        {
            ipAddress = "127.0.0.1";
        }
        return ipAddress;
    }

    public void OnOperationResponseDistribute(SangoNetMessage sangoNetMessage)
    {
        BaseRequest baseRequest = DictTool.GetDictValue<OperationCode, BaseRequest>(sangoNetMessage.MessageHead.OperationCode, RequestDict);
        if (baseRequest != null)
        {
            baseRequest.OnOperationResponse(sangoNetMessage);
        }
        else
        {
            Debug.Log("There is no Request in RequestDict, the OperationCode is: [ " + sangoNetMessage.MessageHead.OperationCode + " ]");
        }
    }

    public void OnEventDataDistribute(SangoNetMessage sangoNetMessage)
    {
        BaseEvent baseEvent = DictTool.GetDictValue<OperationCode, BaseEvent>(sangoNetMessage.MessageHead.OperationCode, EventDict);
        if (baseEvent != null)
        {
            baseEvent.OnEvent(sangoNetMessage);
        }
        else
        {
            Debug.Log("There is no Event in EventDict, the EventCode is: [ " + sangoNetMessage.MessageHead.OperationCode + " ]");
        }
    }

    public void AddRequest(BaseRequest req)
    {
        RequestDict.Add(req.NetOpCode, req);
    }

    public void RemoveRequest(BaseRequest req)
    {
        RequestDict.Remove(req.NetOpCode);
    }

    public void AddEvent(BaseEvent evt)
    {
        EventDict.Add(evt.NetOpCode, evt);
    }

    public void RemoveEvent(BaseEvent evt)
    {
        EventDict.Remove(evt.NetOpCode);
    }

    private enum ConfigureModeCode
    {
        Offline,
        Online
    }
}

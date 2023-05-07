using ExitGames.Client.Photon;
using SangoCommon.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The NetService.

public class NetService : MonoBehaviour, IPhotonPeerListener
{
    public static NetService Instance;
    private static PhotonPeer peer;
    private Dictionary<OperationCode, BaseRequest> RequestDict = new Dictionary<OperationCode, BaseRequest>();
    private Dictionary<EventCode, BaseEvent> EventDict = new Dictionary<EventCode, BaseEvent>();

    public string Account { get; private set; }

    public static PhotonPeer Peer
    {
        get
        {
            return peer;
        }
    }

    public void InitService()
    {
        string ipAddress = SetIPAddress(ConfigureModeCode.Online);
        Instance = this;
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect(ipAddress, "FSOGameServer");
    }

    private string SetIPAddress(ConfigureModeCode mode)
    {
        string ipAddress = null;
        if (mode == ConfigureModeCode.Online)
        {
            ipAddress = "124.220.20.98:5055";
        }
        else if (mode == ConfigureModeCode.Offline)
        {
            ipAddress = "127.0.0.1:5055";
        }
        return ipAddress;
    }

    private void Update()
    {
        if (peer != null)
        {
            peer.Service();
        }

        if (loadingPlayerDataCallBack != null)
        {
            loadingPlayerDataCallBack();
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(statusCode);
    }

    public void OnDestory()
    {
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();
        }
    }

    public void OnEvent(EventData eventData)
    {
        //Client Listen the Event from Server
        EventCode eventCode = (EventCode)eventData.Code;
        BaseEvent _event = null;
        bool isGetEvent = EventDict.TryGetValue(eventCode, out _event);
        if (isGetEvent)
        {
            _event.OnEvent(eventData);
        }
        else
        {
            Debug.Log("There is no Event in EventDict, the EventCode is: [ " + eventCode + " ]");
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        //Client SendRequest and Listen the Response from Server
        OperationCode operationCode = (OperationCode)operationResponse.OperationCode;
        BaseRequest request = null;
        bool isGetRequest = RequestDict.TryGetValue(operationCode, out request);
        if (isGetRequest)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.Log("There is no Request in RequestDict, the OperationCode is: [ " + operationCode + " ]");
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        //Seldom to use
        throw new System.NotImplementedException();
    }

    public void AddRequest(BaseRequest _request)
    {
        //Use OperationCode as Key to add Requst
        RequestDict.Add(_request.OpCode, _request);
    }

    public void RemoveRequest(BaseRequest _request)
    {
        //Use  OperationCode as Key to remove, no need to specify the Request
        RequestDict.Remove(_request.OpCode);
    }

    public void AddEvent(BaseEvent _event)
    {
        //Use OperationCode as Key to add Event
        EventDict.Add(_event.EvCode, _event);
    }

    public void RemoveEvent(BaseEvent _event)
    {
        //Use  OperationCode as Key to remove, no need to specify the Event
        EventDict.Remove(_event.EvCode);
    }

    private Action loadingPlayerDataCallBack = null;

    public void AsyncLoadPlayerData(Action loadedActionCallBack)
    {
        loadingPlayerDataCallBack = () =>
        {
            CacheSystem.Instance.syncPlayerDataRequest.DefaultRequest();
            if (CacheSystem.Instance.syncPlayerDataRequest.IsGetResponse)
            {
                if (loadedActionCallBack != null)
                {
                    loadedActionCallBack();
                }
                loadingPlayerDataCallBack = null;
                CacheSystem.Instance.syncPlayerDataRequest.SetIsGetResponse();
            }
        };
    }

    public void SetAccount(string account)
    {
        Account = account;
    }

    private enum ConfigureModeCode
    {
        Offline,
        Online
    }
}

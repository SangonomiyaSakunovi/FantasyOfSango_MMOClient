using SangoCommon.ServerCode;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription:

public abstract class BaseEvent : MonoBehaviour
{
    protected NetService netService = null;
    protected ResourceService resourceService = null;
    protected AudioService audioService = null;

    public EventCode EvCode;
    public abstract void OnEvent(EventData eventData);

    public virtual void InitEvent()
    {
        netService = NetService.Instance;
        netService.AddEvent(this);
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
    }

    public void OnDestroy()
    {
        NetService.Instance.RemoveEvent(this);
    }

    protected virtual string SetJsonString(object ob)
    {
        string jsonString = JsonSerializer.Serialize(ob);
        return jsonString;
    }

    protected T_obj DeJsonString<T_obj>(string str)
    {
        T_obj obj = JsonSerializer.Deserialize<T_obj>(str);
        return obj;
    }
}

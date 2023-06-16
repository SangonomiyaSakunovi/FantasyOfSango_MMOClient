using SangoMMONetProtocol;
using System.Text.Json;
using UnityEngine;

//Developer : SangonomiyaSakunovi

public abstract class BaseRequest : MonoBehaviour
{
    protected NetService netService = null;
    protected ResourceService resourceService = null;
    protected AudioService audioService = null;

    [HideInInspector]
    public OperationCode NetOpCode;
    public abstract void DefaultRequest();
    public abstract void OnOperationResponse(SangoNetMessage sangoNetMessage);

    public virtual void InitRequset()
    {
        netService = NetService.Instance;        
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
        netService.AddRequest(this);
    }

    public void OnDestroy()
    {
        NetService.Instance.RemoveRequest(this);
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

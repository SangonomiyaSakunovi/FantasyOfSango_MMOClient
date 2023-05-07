using ExitGames.Client.Photon;
using SangoCommon.Enums;
using System.Text.Json;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: Base request, need define json method.

public abstract class BaseRequest : MonoBehaviour
{
    protected NetService netService = null;
    protected ResourceService resourceService = null;
    protected AudioService audioService = null;

    public OperationCode OpCode;
    public abstract void DefaultRequest();
    public abstract void OnOperationResponse(OperationResponse operationResponse);

    public virtual void InitRequset()
    {
        netService = NetService.Instance;
        netService.AddRequest(this);
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
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

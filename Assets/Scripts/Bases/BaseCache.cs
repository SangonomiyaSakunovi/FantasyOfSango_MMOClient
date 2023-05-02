using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The base Cache, use for define base method.

public class BaseCache : MonoBehaviour
{
    protected NetService netService;
    protected ResourceService resourceService;
    protected AudioService audioService;

    public virtual void InitCache()
    {
        netService = NetService.Instance;
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
    }
}

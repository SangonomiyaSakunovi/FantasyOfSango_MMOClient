using UnityEngine;

//Developer : SangonomiyaSakunovi

public class BaseSystem : MonoBehaviour
{
    protected NetService netService;
    protected ResourceService resourceService;
    protected AudioService audioService;

    public virtual void InitSystem()
    {
        netService = NetService.Instance;
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
    }
}

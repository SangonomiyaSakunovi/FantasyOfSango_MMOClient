using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The baseManager, base method should define here.

public class BaseManager : MonoBehaviour
{
    protected NetService netService;
    protected ResourceService resourceService;
    protected AudioService audioService;

    public virtual void InitManager()
    {
        netService = NetService.Instance;
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
    }
}

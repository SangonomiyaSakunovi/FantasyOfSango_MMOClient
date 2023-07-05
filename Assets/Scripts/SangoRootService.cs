using UnityEngine;

//Developer: SangonomiyaSakunovi

public class SangoRootService : MonoBehaviour
{
    private GameObject sangoRootObject;
    private SangoRoot sangoRoot;

    private void Start()
    {
        sangoRootObject = GameObject.FindGameObjectWithTag("SangoGameRoot");
        sangoRoot = sangoRootObject.GetComponent<SangoRoot>();
        sangoRoot.enabled = true;
    }
}

using UnityEngine;
using SangoCommon.Constants;

//Developer : SangonomiyaSakunovi
//Discription: The Flare hehaviour.

public class WandFlareController : MonoBehaviour
{
    private float speed = WeaponConstant.WandFlareSpeed;
    private float smoothLerpTime = TimeConstant.SmoothLerpTime;
    private Rigidbody rb;

    private Vector3 targetPosition;

    private bool isReady = false;
    private bool isCure = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (isReady)
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        else if (isCure)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 2 * smoothLerpTime * Time.deltaTime);
        }
    }

    public void SetIsReady(bool bo = false)
    {
        isReady = bo;
        isCure = bo;
    }

    public void SetIsCure(Vector3 tragetpos)
    {
        targetPosition = tragetpos;
        isCure = true;
    }
}

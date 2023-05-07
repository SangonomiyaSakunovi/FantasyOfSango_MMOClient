using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Camera.

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public Transform player = null;
    private Vector3 offsetPosition;
    private bool mouse1Down;

    public float distance = 12;
    public float scrollSpeed = 10;
    public float RotateSpeed = 2;

    public static CameraController Instance = null;

    private void Start()
    {
        Instance = this;
        LockCursor();
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = offsetPosition + player.position;
            SetRotate();
            SetScroll();
            SetShowCursor();
        }
        else
        {
            return;
        }
    }

    public void InitCamera()
    {
        transform.LookAt(player.position);
        offsetPosition = transform.position - (player.position - new Vector3(0, 2, 0));
    }

    private void SetShowCursor()
    {
        if (Input.GetKey("z"))
        {
            LockCursor(false);
        }
        else
        {
            LockCursor();
        }
    }

    private void SetScroll()
    {
        distance = offsetPosition.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, 2, 20);
        offsetPosition = offsetPosition.normalized * distance;
    }

    private void SetRotate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mouse1Down = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            mouse1Down = false;
        }
        if (mouse1Down)
        {
            transform.RotateAround(player.position, player.up, RotateSpeed * Input.GetAxis("Mouse X"));

            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;
            transform.RotateAround(player.position, transform.right, -RotateSpeed * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;

            if (x < 0 || x > 80)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }
        }
        offsetPosition = transform.position - player.position;
    }
    public void LockCursor(bool bo = true)
    {
        if (bo)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}

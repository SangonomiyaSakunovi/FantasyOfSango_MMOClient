using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Camera.

public class CameraController : MonoBehaviour
{
    private Transform playerTrans;
    private Vector3 offsetPosition;
    private bool mouse1Down;

    public float distance = 12;
    public float scrollSpeed = 10;
    public float RotateSpeed = 2;

    public static CameraController Instance = null;

    private void Start()
    {
        Instance = this;
        GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Hide);
    }

    private void Update()
    {
        if (playerTrans != null && GameManager.Instance.GameMode == GameModeCode.GamePlayMode)
        {
            transform.position = offsetPosition + playerTrans.position;
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
        transform.LookAt(playerTrans.position);
        offsetPosition = transform.position - (playerTrans.position - new Vector3(0, 2, 0));
    }

    private void SetShowCursor()
    {
        if (GameManager.Instance.GameMode == GameModeCode.GamePlayMode)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Show);
            }
            else
            {
                GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Hide);
            }
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
            transform.RotateAround(playerTrans.position, playerTrans.up, RotateSpeed * Input.GetAxis("Mouse X"));

            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;
            transform.RotateAround(playerTrans.position, transform.right, -RotateSpeed * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;

            if (x < 0 || x > 80)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }
        }
        offsetPosition = transform.position - playerTrans.position;
    }

    public void SetPlayerTrans(Transform transform)
    {
        playerTrans = transform;
    }
}

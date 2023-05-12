using SangoCommon.Constants;
using SangoCommon.Enums;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: Player cube.

public class MovePlayerCubeController : MonoBehaviour
{
    public string OnlineAccount { get; private set; }
    public GameObject AvaterObject { get; private set; }
    public AvaterCode OnlinePlayerAvater { get; private set; }
    public bool isLocalPlayer = true;

    private float smoothLerpTime = TimeConstant.SmoothLerpTime;

    //The Attribute of Avater
    private float moveOffsetLimit = 0.2f;
    private float angleLimit = 30f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void FixedUpdate()
    {
        AsyncMove();
        AsyncTowards();
        if (isLocalPlayer)
        {
            ChangeAvater();
        }
    }

    private void ChangeAvater()
    {
        if (Input.GetKeyUp("1"))
        {
            AvaterObject.SetActive(false);
            SetAvaterObject(AvaterCode.SangonomiyaKokomi);
            AvaterObject.SetActive(true);
            CacheSystem.Instance.chooseAvaterRequest.SetAvater(AvaterCode.SangonomiyaKokomi);
            CacheSystem.Instance.chooseAvaterRequest.DefaultRequest();
        }
        if (Input.GetKeyUp("2"))
        {
            AvaterObject.SetActive(false);
            SetAvaterObject(AvaterCode.Yoimiya);
            AvaterObject.SetActive(true);
            CacheSystem.Instance.chooseAvaterRequest.SetAvater(AvaterCode.Yoimiya);
            CacheSystem.Instance.chooseAvaterRequest.DefaultRequest();
        }
        if (Input.GetKeyUp("3"))
        {
            AvaterObject.SetActive(false);
            SetAvaterObject(AvaterCode.Ayaka);
            AvaterObject.SetActive(true);
            CacheSystem.Instance.chooseAvaterRequest.SetAvater(AvaterCode.Ayaka);
            CacheSystem.Instance.chooseAvaterRequest.DefaultRequest();
        }
    }

    private void AsyncTowards()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothLerpTime * Time.deltaTime);
    }

    private void AsyncMove()
    {
        if (Vector3.Distance(transform.position, targetPosition) > moveOffsetLimit)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothLerpTime * Time.deltaTime);
            AvaterObject.GetComponent<MovePlayerAniController>().AsyncMoveAni(true);
            if (isLocalPlayer)
            {
                MainGameSystem.Instance.SetMiniMapTransPosition(transform);
            }           
        }
        else
        {
            AvaterObject.GetComponent<MovePlayerAniController>().AsyncMoveAni(false);
        }
    }

    public void SetOnlineAccount(string onlineAccount)
    {
        isLocalPlayer = false;
        OnlineAccount = onlineAccount;
    }

    public void SetTransform(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
    }

    public void SetAvaterObject(AvaterCode avaterCode)
    {
        switch (avaterCode)
        {
            case AvaterCode.SangonomiyaKokomi:
                {
                    AvaterObject = transform.Find(AvaterConstant.SangonomiyaKokomiName).gameObject;
                    break;
                }
            case AvaterCode.Yoimiya:
                {
                    AvaterObject = transform.Find(AvaterConstant.YoimiyaName).gameObject;
                    break;
                }
            case AvaterCode.Ayaka:
                {
                    AvaterObject = transform.Find(AvaterConstant.AyakaName).gameObject;
                    break;
                }
        }
        if (isLocalPlayer)
        {
            OnlineAccountCache.Instance.SetLocalAvater(avaterCode);
        }
        else
        {
            OnlinePlayerAvater = avaterCode;
        }
        InitAvaterNowPosition();
    }

    private void InitAvaterNowPosition()
    {
        AvaterObject.transform.localPosition = new Vector3(0, 0, 0);
        AvaterObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}

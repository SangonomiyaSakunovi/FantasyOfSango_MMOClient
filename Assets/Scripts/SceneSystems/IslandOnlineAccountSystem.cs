using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The online account System.

public class IslandOnlineAccountSystem : MonoBehaviour
{
    public static IslandOnlineAccountSystem Instance = null;

    private Dictionary<string, TransformOnline> onlineAccountAvaterTransformDict = null;
    private Dictionary<string, GameObject> onlineAccountPlayerCubeDict = null;

    private void Start()
    {
        Instance = this;
        onlineAccountAvaterTransformDict = new Dictionary<string, TransformOnline>();
        onlineAccountPlayerCubeDict = new Dictionary<string, GameObject>();
        CacheSystem.Instance.syncPlayerAccountRequest.DefaultRequest();
    }

    public GameObject GetOnlineCurrentGameobject(string account)
    {
        GameObject gameObject = DictTools.GetDictValue<string, GameObject>(onlineAccountPlayerCubeDict, account);
        return gameObject;
    }

    public AvaterCode GetOnlineCurrentAvater(string account)
    {
        AvaterCode avaterCurrent = DictTools.GetDictValue<string, GameObject>(onlineAccountPlayerCubeDict, account).GetComponent<MovePlayerCubeController>().AvaterName;
        return avaterCurrent;
    }

    public void InstantiatePlayerCube(string onlineAccount)
    {
        if (!onlineAccountPlayerCubeDict.ContainsKey(onlineAccount))
        {
            GameObject playerCube = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.PlayerCube));
            GameObject tempKokomi = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.SangonomiyaKokomiPath));
            GameObject tempYoimiya = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.YoimiyaPath));
            GameObject tempAyaka = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.AyakaPath));
            tempKokomi.SetActive(true);
            SetChildAvater(tempKokomi, playerCube);
            SetChildAvater(tempYoimiya, playerCube);
            SetChildAvater(tempAyaka, playerCube);
            playerCube.GetComponent<MovePlayerCubeController>().SetOnlineAccount(onlineAccount);
            playerCube.GetComponent<MovePlayerCubeController>().SetAvaterNow(AvaterCode.SangonomiyaKokomi);
            tempKokomi.GetComponent<AttackControllerSangonomiyaKokomi>().SetOnlineAccount();
            tempYoimiya.GetComponent<AttackControllerYoimiya>().SetOnlineAccount();
            tempAyaka.GetComponent<AttackControllerAyaka>().SetOnlineAccount();
            if (onlineAccountAvaterTransformDict.ContainsKey(onlineAccount))
            {
                TransformOnline transformCache = onlineAccountAvaterTransformDict[onlineAccount];
                Vector3 targetPosition = new Vector3(transformCache.Vector3Position.X, transformCache.Vector3Position.Y, transformCache.Vector3Position.Z);
                Quaternion targetRotation = new Quaternion(transformCache.QuaternionRotation.X, transformCache.QuaternionRotation.Y, transformCache.QuaternionRotation.Z, transformCache.QuaternionRotation.W);
                playerCube.GetComponent<MovePlayerCubeController>().SetTransform(targetPosition, targetRotation);
            }
            onlineAccountPlayerCubeDict.Add(onlineAccount, playerCube);
        }
    }

    private void SetChildAvater(GameObject childObject, GameObject parentObject)
    {
        childObject.transform.parent = parentObject.transform;
        childObject.transform.localPosition = new Vector3(0, 0, 0);
        childObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetOnlineAvaterTransform(List<TransformOnline> playerTransList)
    {
        for (int i = 0; i < playerTransList.Count; i++)
        {
            if (playerTransList[i].Account != "")
            {
                if (onlineAccountAvaterTransformDict.ContainsKey(playerTransList[i].Account))
                {
                    onlineAccountAvaterTransformDict[playerTransList[i].Account] = playerTransList[i];
                }
                else
                {
                    onlineAccountAvaterTransformDict.Add(playerTransList[i].Account, playerTransList[i]);
                }
                GameObject gameObject = DictTools.GetDictValue<string, GameObject>(onlineAccountPlayerCubeDict, playerTransList[i].Account);
                if (gameObject != null && !gameObject.GetComponent<MovePlayerCubeController>().isLocalPlayer)
                {
                    Vector3 targetPosition = new Vector3(playerTransList[i].Vector3Position.X, playerTransList[i].Vector3Position.Y, playerTransList[i].Vector3Position.Z);
                    Quaternion targetRotation = new Quaternion(playerTransList[i].QuaternionRotation.X, playerTransList[i].QuaternionRotation.Y, playerTransList[i].QuaternionRotation.Z, playerTransList[i].QuaternionRotation.W);
                    gameObject.GetComponent<MovePlayerCubeController>().SetTransform(targetPosition, targetRotation);
                    Debug.Log("其他客户端位置为:" + targetPosition);
                }
            }
        }
    }

    public void SetOnlineAvaterAttack(AttackCommand attackCommand)
    {
        if (attackCommand != null)
        {
            GameObject gameObject = DictTools.GetDictValue<string, GameObject>(onlineAccountPlayerCubeDict, attackCommand.Account);
            if (gameObject != null)
            {
                AvaterCode avaterCurrent = gameObject.GetComponent<MovePlayerCubeController>().AvaterName;
                SkillCode skillCode = attackCommand.SkillCode;
                Vector3 attackPosition = new Vector3(attackCommand.Vector3Position.X, attackCommand.Vector3Position.Y, attackCommand.Vector3Position.Z);
                Quaternion attackRotation = new Quaternion(attackCommand.QuaternionRotation.X, attackCommand.QuaternionRotation.Y, attackCommand.QuaternionRotation.Z, attackCommand.QuaternionRotation.W);
                if (avaterCurrent == AvaterCode.SangonomiyaKokomi)
                {
                    gameObject.GetComponent<MovePlayerCubeController>().AvaterNow.GetComponent<AttackControllerSangonomiyaKokomi>().SetAttackCommand(skillCode, attackPosition, attackRotation);
                }
                else if (avaterCurrent == AvaterCode.Yoimiya)
                {
                    gameObject.GetComponent<MovePlayerCubeController>().AvaterNow.GetComponent<AttackControllerYoimiya>().SetAttackCommand(skillCode, attackPosition, attackRotation);
                }
            }
        }
    }

    public void SetOnlineAvaterAttackResult(AttackResult attackResult)
    {
        if (attackResult != null)
        {
            if (attackResult.DamageNumber > 0)
            {
                GameObject gameObject = DictTools.GetDictValue<string, GameObject>(onlineAccountPlayerCubeDict, attackResult.DamagerAccount);
                if (gameObject != null)
                {
                    AvaterCode avaterCurrent = gameObject.GetComponent<MovePlayerCubeController>().AvaterName;
                    if (avaterCurrent == AvaterCode.SangonomiyaKokomi)
                    {
                        gameObject.GetComponent<MovePlayerCubeController>().AvaterNow.GetComponent<AttackControllerSangonomiyaKokomi>().SetDamaged(attackResult);
                    }
                    else if (avaterCurrent == AvaterCode.Yoimiya)
                    {
                        gameObject.GetComponent<MovePlayerCubeController>().AvaterNow.GetComponent<AttackControllerYoimiya>().SetDamaged(attackResult);
                    }
                }
            }
            else    //in this kind, the online avater has been cured by other client!!! Attention!!!
            {

            }
        }
    }

    public void SetChoosedAvater(string account, AvaterCode avater)
    {
        GameObject gameObject = DictTools.GetDictValue<string, GameObject>(onlineAccountPlayerCubeDict, account);
        gameObject.GetComponent<MovePlayerCubeController>().AvaterNow.SetActive(false);
        gameObject.GetComponent<MovePlayerCubeController>().SetAvaterNow(avater);
        gameObject.GetComponent<MovePlayerCubeController>().AvaterNow.SetActive(true);
    }
}

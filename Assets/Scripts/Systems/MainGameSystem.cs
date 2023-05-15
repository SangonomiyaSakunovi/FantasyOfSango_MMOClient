using Assets.Scripts.Common.Constant;
using SangoCommon.Classs;
using SangoCommon.Enums;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The MainGame Sytem.

public class MainGameSystem : BaseSystem
{
    public static MainGameSystem Instance = null;
    public MainGameWindow mainGameWindow;

    [HideInInspector]
    public GameObject playerCapsule = null;
    [HideInInspector]
    public GameObject playerCube = null;

    private Transform[] npcTransformArray;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
    }

    public void EnterIslandScene()
    {
        resourceService.AsyncLoadScene(SceneConstant.MainGameScene, () =>
        {
            //Load Avater
            InitiateLocalAvater();
            playerCube.GetComponent<MovePlayerCubeController>().SetAvaterObject(AvaterCode.SangonomiyaKokomi);
            playerCube.GetComponent<MovePlayerCubeController>().AvaterObject.SetActive(true);
            OnlineAccountCache.Instance.SetLocalAvater(AvaterCode.SangonomiyaKokomi);
            CameraController.Instance.player = playerCube.transform;
            CameraController.Instance.InitCamera();
            //Load UI
            mainGameWindow.SetWindowState();
            mainGameWindow.RefreshMainAvaterUI(OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0]);
            mainGameWindow.RefreshBackAvaterUI();
            mainGameWindow.RefreshMissionUI();
            //LoadMusic
            audioService.LoadAudio(AudioConstant.NormalFightBG);
            //PlayMusic
            audioService.PlayBGAudio(AudioConstant.MainGameBG, true);
            //MiniMap
            SetMiniMapTransPosition(playerCube.transform);
            //LoadPos
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainGameMap mainGameMap = map.GetComponent<MainGameMap>();
            npcTransformArray = mainGameMap.NpcTransformArray;
            //LoadEnemy
            InitiateEnemy();
        });
    }

    private void InitiateLocalAvater()
    {
        playerCapsule = (GameObject)Instantiate(Resources.Load(AvaterConstant.PlayerCapsule));
        playerCube = (GameObject)Instantiate(Resources.Load(AvaterConstant.PlayerCube));
        GameObject tempKokomi = (GameObject)Instantiate(Resources.Load(AvaterConstant.SangonomiyaKokomiPath));
        GameObject tempYoimiya = (GameObject)Instantiate(Resources.Load(AvaterConstant.YoimiyaPath));
        GameObject tempAyaka = (GameObject)Instantiate(Resources.Load(AvaterConstant.AyakaPath));
        SetChildAvater(tempKokomi, playerCube);
        SetChildAvater(tempYoimiya, playerCube);
        SetChildAvater(tempAyaka, playerCube);
        playerCube.transform.position = playerCapsule.transform.position;
        playerCube.transform.rotation = playerCapsule.transform.rotation;
    }

    private void SetChildAvater(GameObject childObject, GameObject parentObject)
    {
        childObject.transform.parent = parentObject.transform;
        childObject.transform.localPosition = new Vector3(0, 0, 0);
        childObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void InitiateEnemy()
    {
        Instantiate(Resources.Load(EnemyConstant.HilichulPath));
    }


    public void SetLocalAvaterAttackResult(AttackResult attackResult)
    {
        if (attackResult.DamageNumber > 0)
        {
            if (OnlineAccountCache.Instance.LocalAvater == AvaterCode.SangonomiyaKokomi)
            {
                playerCube.transform.Find(AvaterConstant.SangonomiyaKokomiName).GetComponent<AttackControllerSangonomiyaKokomi>().SetDamaged(attackResult);
            }
            else if (OnlineAccountCache.Instance.LocalAvater == AvaterCode.Yoimiya)
            {
                playerCube.transform.Find(AvaterConstant.YoimiyaName).GetComponent<AttackControllerYoimiya>().SetDamaged(attackResult);
            }
            SangoRoot.AddMessage("你被玩家" + attackResult.AttackerAccount + "攻击了，受到伤害-" + attackResult.DamageNumber + "HP");
            AvaterAttributeInfo tempAttribute = attackResult.DamagerAvaterInfo.AttributeInfoList[0];
            mainGameWindow.RefreshMainAvaterUI(tempAttribute);
        }
        else    //in this kind, the avater has been cured
        {
            GameObject healerGameobject = IslandOnlineAccountSystem.Instance.GetOnlineCurrentGameobject(attackResult.AttackerAccount);
            AvaterCode healerAvater = IslandOnlineAccountSystem.Instance.GetOnlineCurrentAvater(attackResult.AttackerAccount);
            if (healerAvater == AvaterCode.SangonomiyaKokomi)
            {
                healerGameobject.GetComponent<AttackControllerSangonomiyaKokomi>().SetCureResult(playerCube.transform.position);
            }
            SangoRoot.AddMessage("你被玩家" + attackResult.AttackerAccount + "治疗了，治疗量为" + -attackResult.DamageNumber + "HP");
            AvaterAttributeInfo tempAttribute = attackResult.DamagerAvaterInfo.AttributeInfoList[0];
            mainGameWindow.RefreshMainAvaterUI(tempAttribute);
        }
    }

    public void SetMiniMapTransPosition(Transform playerTrans)
    {
        mainGameWindow.SetMiniMapTransPosition(playerTrans);
    }

    public void OpenMissionWindow()
    {

    }
}

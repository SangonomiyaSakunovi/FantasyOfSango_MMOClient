using Assets.Scripts.Common.Constant;
using SangoCommon.Classs;
using SangoCommon.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The MainGame Sytem.

public class MainGameSystem : BaseSystem
{
    public static MainGameSystem Instance = null;

    public MainGameWindow mainGameWindow;

    public GameObject miniMapBaseLocation;
    public GameObject miniMapLocals;
    private float miniMapScaling = 10;
    private Vector3 HomePos = new Vector3(5.568432f, 0, -21.45944f);
    private Vector3 HillPos = new Vector3(88.04138f, 0, 678.8067f);

    private Vector3 miniHomePos = new Vector3(-8, 198, 0);
    private Vector3 miniHillPos = new Vector3(-115.7629f, -693.5677f, 0);

    private float xChange = 265;
    private float yChange = 930;

    [HideInInspector]
    public GameObject playerCapsule = null;
    [HideInInspector]
    public GameObject playerCube = null;

    public AvaterCode LocalAvaterCurrent { get; private set; }

    public TMP_Text hpText;
    public TMP_Text levelText;
    public Image hpFG;
    public Image elementBurstFG;

    private AvaterInfo avaterInfo = null;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        miniMapScaling = Vector3.Distance(HomePos, HillPos) / Vector3.Distance(miniHomePos, miniHillPos);
    }

    public void EnterMainGame()
    {
        resourceService.AsyncLoadScene(SceneConstant.MainGameScene, () =>
        {
            //Load Avater
            InitiateLocalAvater();
            playerCube.GetComponent<MovePlayerCubeController>().SetAvaterNow(AvaterCode.SangonomiyaKokomi);
            playerCube.GetComponent<MovePlayerCubeController>().AvaterNow.SetActive(true);
            LocalAvaterCurrent = AvaterCode.SangonomiyaKokomi;
            CameraController.Instance.player = playerCube.transform;
            CameraController.Instance.InitCamera();
            //Load UI
            mainGameWindow.SetWindowState();
            avaterInfo = OnlineAccountCache.Instance.AvaterInfo;
            RefreshMainGameUI(avaterInfo.AttributeInfoList[0].HP, avaterInfo.AttributeInfoList[0].HPFull,
            avaterInfo.AttributeInfoList[0].MP, avaterInfo.AttributeInfoList[0].MPFull);
            //LoadMusic
            audioService.LoadAudio(AudioConstant.NormalFightBG);
            //PlayMusic
            audioService.PlayBGAudio(AudioConstant.MainGameBG, true);
            //MiniMap
            SetMiniMapTransPosition(playerCube.transform);
            //LoadEnemy
            InitiateEnemy();
        });
    }

    public void RefreshMainGameUI(int hp, int hpFull, int mp, int mpFull)
    {
        SetText(hpText, hp + " / " + hpFull);
        hpFG.fillAmount = (float)hp / hpFull;
        elementBurstFG.fillAmount = (float)mp / hpFull;
    }

    private void InitiateLocalAvater()
    {
        playerCapsule = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.PlayerCapsule));
        playerCube = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.PlayerCube));
        GameObject tempKokomi = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.SangonomiyaKokomiPath));
        GameObject tempYoimiya = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.YoimiyaPath));
        GameObject tempAyaka = (GameObject)GameObject.Instantiate(Resources.Load(AvaterConstant.AyakaPath));
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
        GameObject.Instantiate(Resources.Load(EnemyConstant.HilichulPath));
    }

    public void RefreshAvaterUI()
    {

    }

    public void SetLocalAvaterAttackResult(AttackResult attackResult)
    {
        if (attackResult.DamageNumber > 0)
        {
            if (LocalAvaterCurrent == AvaterCode.SangonomiyaKokomi)
            {
                playerCube.transform.Find(AvaterConstant.SangonomiyaKokomiName).GetComponent<AttackControllerSangonomiyaKokomi>().SetDamaged(attackResult);
            }
            else if (LocalAvaterCurrent == AvaterCode.Yoimiya)
            {
                playerCube.transform.Find(AvaterConstant.YoimiyaName).GetComponent<AttackControllerYoimiya>().SetDamaged(attackResult);
            }
            SangoRoot.AddMessage("你被玩家" + attackResult.AttackerAccount + "攻击了，受到伤害-" + attackResult.DamageNumber + "HP");
            AvaterAttributeInfo tempAttribute = attackResult.DamagerAvaterInfo.AttributeInfoList[0];
            Instance.RefreshMainGameUI(tempAttribute.HP, tempAttribute.HPFull, tempAttribute.MP, tempAttribute.MPFull);
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
            Instance.RefreshMainGameUI(tempAttribute.HP, tempAttribute.HPFull, tempAttribute.MP, tempAttribute.MPFull);
        }
    }

    public void SetMiniMapTransPosition(Transform playerTrans)
    {
        float moveX = (playerTrans.position.x - HomePos.x) / miniMapScaling;
        float moveY = (playerTrans.position.z - HomePos.z) / miniMapScaling;
        miniMapBaseLocation.transform.position = new Vector3(miniHomePos.x - moveX + xChange, miniHomePos.y - moveY + yChange, 0);
        Vector3 rotations = playerTrans.rotation.eulerAngles;
        miniMapLocals.transform.rotation = Quaternion.Euler(0, 0, -rotations.y);
    }
}

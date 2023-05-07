using Assets.Scripts.Common.Constant;
using SangoCommon.Classs;
using SangoCommon.Enums;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: Just test the hilichurl behaviour.

public class TestHilichurl : MonoBehaviour
{
    public Transform EnemyHpUIRoot;

    private FSMSystem fSMSystem;
    private Vector3 path1 = new Vector3(-41.90256f, 6.667f, 123.9594f);
    private Vector3 path2 = new Vector3(-71.56f, 12.49f, 150.52f);
    private Vector3 path3 = new Vector3(-72.574f, 10.009f, 133.421f);

    private bool IsAttacking;
    private int hpFull;
    private int hp;

    void Awake()
    {
        InitFSM();
        hpFull = 100;
        hp = 100;
        IsAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        fSMSystem.UpdateFSM(this.gameObject);
        if (IsAttacking)
        {
            SangoRoot.Instance.dynamicWindow.UpdateEnemyHpUI(gameObject.name, hp, EnemyHpUIRoot.position);
        }
    }

    void InitFSM()
    {
        List<Vector3> pathList = new List<Vector3> { path1, path2, path3 };
        fSMSystem = new FSMSystem();
        FSMState patrolState = new PatrolState(fSMSystem, pathList, EnemyConstant.HilichurlPatrolSpeed, 10);
        FSMState chaseState = new ChaseState(fSMSystem, EnemyConstant.HilichurlChaseSpeed, 20, 3);
        FSMState hilichurlAttackState = new HilichurlAttackState(fSMSystem, 3);
        fSMSystem.AddFSMState(patrolState);
        fSMSystem.AddFSMState(chaseState);
        fSMSystem.AddFSMState(hilichurlAttackState);
        patrolState.AddFSMTransition(FSMTransitionCode.NoticePlayer, FSMStateCode.Chase);
        chaseState.AddFSMTransition(FSMTransitionCode.LostPlayer, FSMStateCode.Patrol);
        chaseState.AddFSMTransition(FSMTransitionCode.ApproachPlayer, FSMStateCode.HilichurlAttack);
        hilichurlAttackState.AddFSMTransition(FSMTransitionCode.PlayerRunAway, FSMStateCode.Chase);
    }

    public void PlayEnemyHpUIItem()
    {
        SangoRoot.Instance.dynamicWindow.AddEnemyHpUI(gameObject.name, 100, EnemyHpUIRoot.position);
        IsAttacking = true;
    }

    public void SetDamaged(AvaterCode avater, SkillCode skill, Vector3 attakerPos)
    {
        AudioService.Instance.PlayBGAudio(AudioConstant.NormalFightBG, true);
        if (!IsAttacking)
        {
            PlayEnemyHpUIItem();
        }
        //Just for test!!!
        if (skill == SkillCode.ElementAttack)
        {
            ElementApplication elementApplicationCache = null;
            if (avater == AvaterCode.SangonomiyaKokomi)   //KokomiAttack with Hydro
            {
                elementApplicationCache = new ElementApplication(ElementTypeCode.Hydro, 2);
            }
            else if (avater == AvaterCode.Yoimiya)   //YoimiyaAttack with Pyro
            {
                elementApplicationCache = new ElementApplication(ElementTypeCode.Pyro, 2);
            }
            else if (avater == AvaterCode.Ayaka)
            {
                elementApplicationCache = new ElementApplication(ElementTypeCode.Cryo, 2);
            }
            gameObject.GetComponent<ElementSystem>().ElementReaction(FightTypeCode.PVE, skill, elementApplicationCache, attakerPos, gameObject.name);
        }
    }
}

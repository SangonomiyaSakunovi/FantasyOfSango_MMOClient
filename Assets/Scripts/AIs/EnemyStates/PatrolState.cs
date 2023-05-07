using Assets.Scripts.Common.Constant;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The PatrolState, the enemy should in this state if no triger or in idle.

public class PatrolState : FSMState
{
    private List<Vector3> pathList = new List<Vector3>();
    private float patrolSpeed;
    private float patrolToChaseDis;
    private int index = 0;
    private Transform playerTrans;
    public PatrolState(FSMSystem fSMSystem, List<Vector3> path, float speed, float patrolToChaseDistance) : base(fSMSystem)
    {
        stateCode = FSMStateCode.Patrol;
        pathList = path;
        patrolSpeed = speed;
        playerTrans = MainGameSystem.Instance.playerCube.transform;
        patrolToChaseDis = patrolToChaseDistance;
    }
    public override void Act(GameObject npc)
    {
        npc.transform.LookAt(pathList[index]);
        npc.transform.Translate(Vector3.forward * Time.deltaTime * patrolSpeed);
        if (Vector3.Distance(npc.transform.position, pathList[index]) < EnemyConstant.EnemyToTargetPointFSMDistance)
        {
            index++;
            index %= pathList.Count;
        }
    }

    public override void Reason(GameObject npc)
    {
        if (Vector3.Distance(playerTrans.position, npc.transform.position) < patrolToChaseDis)
        {
            fSMSystem.OnFSMTransition(FSMTransitionCode.NoticePlayer);
        }
    }    
}

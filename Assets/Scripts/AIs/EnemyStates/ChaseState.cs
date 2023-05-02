using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The ChaseState, you can define what to do here.

public class ChaseState : FSMState
{
    private Transform playerTrans;
    private float chaseSpeed;
    private float chaseToPatrolDis;
    private float chaseToAttackDis;
    public ChaseState(FSMSystem fSMSystem, float speed, float chaseToPatrolDistance, float chaseToAttackDistance) : base(fSMSystem)
    {
        stateCode = FSMStateCode.Chase;
        chaseSpeed = speed;
        chaseToPatrolDis = chaseToPatrolDistance;
        chaseToAttackDis = chaseToAttackDistance;
        playerTrans = MainGameSystem.Instance.playerCube.transform;
    }
    public override void Act(GameObject npc)
    {
        npc.transform.LookAt(playerTrans.position);
        npc.transform.Translate(Vector3.forward * Time.deltaTime * chaseSpeed);
    }

    public override void Reason(GameObject npc)
    {
        if (Vector3.Distance(playerTrans.position, npc.transform.position) > chaseToPatrolDis)
        {
            fSMSystem.OnFSMTransition(FSMTransitionCode.LostPlayer);
        }
        else if (Vector3.Distance(playerTrans.position, npc.transform.position) < chaseToAttackDis)
        {
            fSMSystem.OnFSMTransition(FSMTransitionCode.ApproachPlayer);
        }
    }
}

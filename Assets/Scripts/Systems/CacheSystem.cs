using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Cache System.

public class CacheSystem : BaseSystem
{
    public static CacheSystem Instance = null;

    [HideInInspector]
    public SyncPlayerDataRequest syncPlayerDataRequest;
    [HideInInspector]
    public SyncPlayerTransformRequest syncPlayerTransformRequest;
    [HideInInspector]
    public SyncPlayerAccountRequest syncPlayerAccountRequest;
    [HideInInspector]
    public AttackCommandRequest attackCommandRequest;
    [HideInInspector]
    public AttackDamageRequest attackDamageRequest;
    [HideInInspector]
    public ChooseAvaterRequest chooseAvaterRequest;

    [HideInInspector]
    public NewAccountJoinEvent newAccountJoinEvent;
    [HideInInspector]
    public SyncPlayerTransformEvent syncPlayerTransformEvent;
    [HideInInspector]
    public AttackCommandEvent attackCommandEvent;
    [HideInInspector]
    public AttackResultEvent attackResultEvent;
    [HideInInspector]
    public ChooseAvaterEvent chooseAvaterEvent;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        syncPlayerDataRequest = GetComponent<SyncPlayerDataRequest>();
        syncPlayerTransformRequest = GetComponent<SyncPlayerTransformRequest>();
        syncPlayerAccountRequest = GetComponent<SyncPlayerAccountRequest>();
        attackCommandRequest = GetComponent<AttackCommandRequest>();
        attackDamageRequest = GetComponent<AttackDamageRequest>();
        chooseAvaterRequest = GetComponent<ChooseAvaterRequest>();

        newAccountJoinEvent = GetComponent<NewAccountJoinEvent>();
        syncPlayerTransformEvent = GetComponent<SyncPlayerTransformEvent>();
        attackCommandEvent = GetComponent<AttackCommandEvent>();
        attackResultEvent = GetComponent<AttackResultEvent>();
        chooseAvaterEvent = GetComponent<ChooseAvaterEvent>();
    }
}

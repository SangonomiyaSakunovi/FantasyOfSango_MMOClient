using SangoCommon.Classs;
using SangoCommon.Enums;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Yoimiya Attack.

public class AttackControllerYoimiya : MonoBehaviour
{
    private Transform playerLeftHandBone;
    private Animator animator;
    private Transform wandPoint;

    public bool isLocalPlayer = true;

    private bool isAttacking = false;
    private List<Flare> flareList = new List<Flare>();

    private Vector3 attackPosition;
    private Quaternion attackRotation;

    void Start()
    {
        playerLeftHandBone = transform.Find(AvaterConstant.YoimiyaLeftBone);
        Transform weaponTrans = Instantiate(Resources.Load<Transform>(WeaponConstant.Wand01Path));
        weaponTrans.parent = playerLeftHandBone;
        weaponTrans.localPosition = new Vector3(0f, 0.02f, 0.0f);
        weaponTrans.localRotation = Quaternion.Euler(0f, 0.0f, 0f);
        weaponTrans.localScale = new Vector3(0.5f, 0.5f, 1f);
        animator = GetComponent<Animator>();
        wandPoint = playerLeftHandBone.Find(WeaponConstant.Wand01Point);
        GameObjectPools<Flare>.MaxCount = 20;
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            SetAttack();
        }
        CleanObjectPool();
    }

    private void SetAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Ground") && !isAttacking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isAttacking = true;
                CacheSystem.Instance.attackCommandRequest.SetAttackCommand(SkillCode.Attack, transform.position, transform.rotation);
                CacheSystem.Instance.attackCommandRequest.DefaultRequest();
                animator.SetBool("AttackBool", true);
                Invoke("SetShoot", 0.8f);
                AttackExam(SkillCode.Attack);
                isAttacking = false;
            }
            //if (Input.GetButtonDown("E"))
            //{
            //    isAttacking = true;

            //}
        }
    }

    private void AsyncAttack()
    {
        animator.SetBool("AttackBool", true);
        Invoke("SetShoot", 0.8f);
    }

    private void CleanObjectPool()
    {
        for (int i = flareList.Count - 1; i >= 0; i--)
        {
            if (Time.time - flareList[i].endTime > 0)
            {
                flareList[i].flare.SetActive(false);
                flareList[i].flare.GetComponent<WandFlareController>().SetIsReady();
                GameObjectPools<Flare>.RecyclePool(flareList[i]);
                flareList.RemoveAt(i);
            }
        }
    }

    private void SetShoot()
    {
        Flare flare = GameObjectPools<Flare>.GetObject();
        flare.flare.transform.position = wandPoint.position;
        flare.flare.transform.forward = transform.forward;
        flare.flare.GetComponent<WandFlareController>().SetIsReady(true);
        flare.flare.SetActive(true);
        animator.SetBool("AttackBool", false);
        flare.endTime = Time.time + WeaponConstant.WandFlareEndTime;
        flareList.Add(flare);
    }

    private class Flare
    {
        public GameObject flare = (GameObject)Instantiate(Resources.Load(WeaponConstant.Flare01Path));
        public float endTime;
    }

    public void SetOnlineAccount()
    {
        isLocalPlayer = false;
    }

    public void SetAttackCommand(SkillCode skillCode, Vector3 position, Quaternion rotation)
    {
        attackPosition = position;
        attackRotation = rotation;
        if (skillCode == SkillCode.Attack)
        {
            AsyncAttack();
        }
    }

    private void AttackExam(SkillCode skillCode)
    {
        Collider[] colliders = new Collider[10];
        Physics.OverlapSphereNonAlloc(wandPoint.position, 3, colliders);
        foreach (var item in colliders)
        {
            if (item != null)
            {
                if (item.CompareTag("Player") && !item.gameObject.GetComponent<MovePlayerAniController>().isLocalPlayer)
                {
                    Debug.Log(item.gameObject.GetComponent<MovePlayerAniController>().OnlineAccount);
                    //CacheSystem.Instance.attackDamageRequest.SetAttackDamage(item.gameObject.GetComponent<MoveController>().OnlineAccount,
                    //skillCode,wandPoint.position,item.gameObject.transform.position,DateTime.Now.ToUniversalTime());
                    //CacheSystem.Instance.attackDamageRequest.DefaultRequest();
                }
                if (item.CompareTag("Enemy"))
                {
                    TestElementAttack(item);
                }
            }
        }
    }

    public void SetDamaged(AttackResult attackResult)
    {
        animator.SetTrigger("DamagedTriger");
        AudioService.Instance.PlayUIAudio(AudioConstant.DamagedAudio);
        animator.SetTrigger("IdleTriger");
    }

    //Just for Test ElementSystem!!!
    private void TestElementAttack(Collider collider)
    {
        collider.gameObject.GetComponent<TestHilichurl>().SetDamaged(AvaterCode.Yoimiya, SkillCode.ElementAttack, transform.position);
    }
}

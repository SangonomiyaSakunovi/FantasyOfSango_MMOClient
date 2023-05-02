using SangoCommon.Classs;
using SangoCommon.Enums;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Ayaka Attack.

public class AttackControllerAyaka : MonoBehaviour
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
        playerLeftHandBone = transform.Find(AvaterConstant.AyakaLeftBone);
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
                AttackExam(SkillCode.Attack);
                animator.SetBool("AttackBool", true);
                Invoke("SetSangonomiyaKokomiA", 0.2f);
            }
            if (Input.GetKeyDown("e"))
            {
                //The ElementAttack
                isAttacking = true;
                AttackExam(SkillCode.ElementAttack);

            }
            if (Input.GetKeyDown("q"))
            {
                //The ElementBurst
                //The element can to all avater
                isAttacking = true;
                CacheSystem.Instance.attackCommandRequest.SetAttackCommand(SkillCode.ElementBurst, transform.position, transform.rotation);
                CacheSystem.Instance.attackCommandRequest.DefaultRequest();
                AttackExam(SkillCode.ElementBurst);
                animator.SetBool("ElementBurstBool", true);
                Invoke("SetSangonomiyaKokomiQ", 0.2f);
            }
        }
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

    private void SetSangonomiyaKokomiA()
    {
        animator.SetBool("AttackBool", false);
        isAttacking = false;
    }

    private void SetSangonomiyaKokomiE()
    {
        animator.SetBool("ElementAttackBool", false);
        isAttacking = false;
    }

    private void SetSangonomiyaKokomiQ()
    {
        animator.SetBool("ElementBurstBool", false);
        isAttacking = false;
    }

    private class Flare
    {
        public GameObject flare = (GameObject)Instantiate(Resources.Load(WeaponConstant.Flare01Path));
        public float endTime;
        public void Clean()
        {
            flare = null;
        }
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
            animator.SetBool("AttackBool", true);
            Invoke("SetSangonomiyaKokomiA", 0.2f);
        }
        else if (skillCode == SkillCode.ElementAttack)
        {
            animator.SetBool("ElementAttackBool", true);
            Invoke("SetSangonomiyaKokomiE", 0.2f);
        }
        else if (skillCode == SkillCode.ElementBurst)
        {
            animator.SetBool("ElementBurstBool", true);
            Invoke("SetSangonomiyaKokomiQ", 0.2f);
        }
    }

    private void AttackExam(SkillCode skillCode)
    {
        if (skillCode == SkillCode.Attack)
        {
            Collider[] colliders = new Collider[10];
            Physics.OverlapSphereNonAlloc(wandPoint.position, 3, colliders);
            foreach (var item in colliders)
            {
                if (item != null)
                {
                    if (item.CompareTag("Player") && !item.gameObject.GetComponent<MovePlayerAniController>().isLocalPlayer)
                    {
                        //CacheSystem.Instance.attackDamageRequest.SetAttackDamage(item.gameObject.GetComponent<MoveController>().OnlineAccount,
                        //skillCode, wandPoint.position, item.gameObject.transform.position, DateTime.Now.ToUniversalTime());
                        //CacheSystem.Instance.attackDamageRequest.DefaultRequest();
                    }
                    if (item.CompareTag("Enemy"))
                    {
                        TestElementAttack(item);
                    }
                }
            }
        }
        else if (skillCode == SkillCode.ElementAttack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("Player") && !hit.collider.GetComponent<MovePlayerAniController>().isLocalPlayer)
                {
                    animator.SetBool("ElementAttackBool", true);
                    //CacheSystem.Instance.attackDamageRequest.SetAttackDamage(hit.collider.gameObject.GetComponent<MoveController>().OnlineAccount,
                    //skillCode, wandPoint.position, hit.collider.gameObject.transform.position, DateTime.Now.ToUniversalTime());
                    //CacheSystem.Instance.attackDamageRequest.DefaultRequest();
                    SetCureFlare(hit.collider.gameObject.transform.position);
                }
            }
            Invoke("SetSangonomiyaKokomiE", 0.2f);
        }
        else if (skillCode == SkillCode.ElementBurst)
        {

        }
    }

    private void SetCureFlare(Vector3 hitPos)
    {
        Flare flare = GameObjectPools<Flare>.GetObject();
        flare.flare.transform.position = wandPoint.position;
        flare.flare.transform.forward = hitPos;
        flare.flare.GetComponent<WandFlareController>().SetIsCure(hitPos);
        flare.endTime = Time.time + WeaponConstant.WandFlareEndTime;
        flareList.Add(flare);
        flare.flare.SetActive(true);
        Invoke("SetSangonomiyaKokomiE", 0.2f);
    }

    public void SetDamaged(AttackResult attackResult)
    {
        animator.SetTrigger("DamagedTriger");
        AudioService.Instance.PlayUIAudio(AudioConstant.DamagedAudio);
        animator.SetTrigger("IdleTriger");
    }

    public void SetCureResult(Vector3 hitPos)
    {
        animator.SetBool("ElementAttackBool", true);
        SetCureFlare(hitPos);
    }

    //Just for Test ElementSystem!!!
    private void TestElementAttack(Collider collider)
    {
        collider.gameObject.GetComponent<TestHilichurl>().SetDamaged(AvaterCode.Ayaka, SkillCode.ElementAttack, transform.position);
    }
}

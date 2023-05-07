using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: Player Animation.

public class MovePlayerAniController : MonoBehaviour
{
    private Animator animator;

    private float BlendChangeSpeed = 5.0f;
    private float targetBlend;
    private float currentBlend;

    public string OnlineAccount { get; private set; }
    public bool isLocalPlayer = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (currentBlend != targetBlend)
        {
            UpdateMixBlend();
        }
    }

    private void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < BlendChangeSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= BlendChangeSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += BlendChangeSpeed * Time.deltaTime;
        }
        animator.SetFloat("MoveBlend", currentBlend);
    }

    public void SetOnlineAccount(string onlineAccount)
    {
        isLocalPlayer = false;
        OnlineAccount = onlineAccount;
    }

    public void AsyncMoveAni(bool isMove)
    {
        if (isMove)
        {
            SetBlend(1f);
        }
        else
        {
            SetBlend(0f);
        }
    }
}

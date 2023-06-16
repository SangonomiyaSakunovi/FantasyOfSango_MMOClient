using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class AttackResultEvent : BaseEvent
{
    public override void InitEvent()
    {
        NetOpCode = OperationCode.AttackDamage;
        base.InitEvent();       
    }
    public override void OnEvent(SangoNetMessage sangoNetMessage)
    {
        string attackResultJson = sangoNetMessage.MessageBody.MessageString;
        {
            if (attackResultJson != null && IslandOnlineAccountSystem.Instance != null)
            {
                AttackResult attackResult = DeJsonString<AttackResult>(attackResultJson);
                if (attackResult.DamagerAccount == OnlineAccountCache.Instance.LocalAccount)
                {
                    MainGameSystem.Instance.SetLocalAvaterAttackResult(attackResult);
                }
                else
                {
                    IslandOnlineAccountSystem.Instance.SetOnlineAvaterAttackResult(attackResult);
                }
            }
        }
    }
}

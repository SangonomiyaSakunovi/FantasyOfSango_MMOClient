using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi
//Discription: Attack Commond Event.

public class AttackCommandEvent : BaseEvent
{
    public override void InitEvent()
    {
        NetOpCode = OperationCode.AttackCommand;
        base.InitEvent();       
    }
    public override void OnEvent(SangoNetMessage sangoNetMessage)
    {
        string attackCommandJson = sangoNetMessage.MessageBody.MessageString;
        if (attackCommandJson != null && IslandOnlineAccountSystem.Instance != null)
        {
            AttackCommand attackCommand = DeJsonString<AttackCommand>(attackCommandJson);
            IslandOnlineAccountSystem.Instance.SetOnlineAvaterAttack(attackCommand);
        }
    }
}

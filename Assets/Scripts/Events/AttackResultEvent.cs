using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;

//Developer : SangonomiyaSakunovi
//Discription: Attack Result Event.

public class AttackResultEvent : BaseEvent
{
    public override void InitEvent()
    {
        base.InitEvent();
    }
    public override void OnEvent(EventData eventData)
    {
        string attackResultJson = DictTools.GetStringValue(eventData.Parameters, (byte)ParameterCode.AttackResult);
        {
            if (attackResultJson != null && IslandOnlineAccountSystem.Instance != null)
            {
                AttackResult attackResult = DeJsonString<AttackResult>(attackResultJson);
                if (attackResult.DamagerAccount == NetService.Instance.Account)
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

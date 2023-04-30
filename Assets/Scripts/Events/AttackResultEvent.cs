using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;

//Developer : SangonomiyaSakunovi
//Discription:

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
                AttackResult attackResultCache = DeJsonString<AttackResult>(attackResultJson);
                if (attackResultCache.DamagerAccount == NetService.Instance.Account)
                {
                    MainGameSystem.Instance.SetLocalAvaterAttackResult(attackResultCache);
                }
                else
                {
                    IslandOnlineAccountSystem.Instance.SetOnlineAvaterAttackResult(attackResultCache);
                }
            }
        }
    }
}

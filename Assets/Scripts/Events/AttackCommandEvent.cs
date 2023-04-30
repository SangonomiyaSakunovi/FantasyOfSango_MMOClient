using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;

//Developer : SangonomiyaSakunovi
//Discription:

public class AttackCommandEvent : BaseEvent
{
    public override void InitEvent()
    {
        base.InitEvent();
    }
    public override void OnEvent(EventData eventData)
    {
        string attackCommandJson = DictTools.GetStringValue(eventData.Parameters, (byte)ParameterCode.AttackCommand);
        if (attackCommandJson != null && IslandOnlineAccountSystem.Instance != null)
        {
            AttackCommand attackCommandCache = DeJsonString<AttackCommand>(attackCommandJson);
            IslandOnlineAccountSystem.Instance.SetOnlineAvaterAttack(attackCommandCache);
        }
    }
}

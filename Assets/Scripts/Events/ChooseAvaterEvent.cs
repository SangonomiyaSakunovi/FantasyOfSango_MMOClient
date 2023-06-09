using ExitGames.Client.Photon;
using SangoCommon.Enums;
using SangoCommon.Tools;

//Developer : SangonomiyaSakunovi
//Discription: ChooseAvater Event.

public class ChooseAvaterEvent : BaseEvent
{
    public override void InitEvent()
    {
        base.InitEvent();
        EvCode = EventCode.ChooseAvater;
    }
    public override void OnEvent(EventData eventData)
    {
        AvaterCode avater = (AvaterCode)DictTools.GetDictValue<byte, object>(eventData.Parameters, (byte)ParameterCode.ChooseAvater);
        string account = DictTools.GetStringValue(eventData.Parameters, (byte)ParameterCode.Account);
        if (account != OnlineAccountCache.Instance.LocalAccount)
        {
            IslandOnlineAccountSystem.Instance.SetChoosedAvater(account, avater);
        }        
    }
}

using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class ChooseAvaterEvent : BaseEvent
{
    public override void InitEvent()
    {
        NetOpCode = OperationCode.ChooseAvater;
        base.InitEvent();        
    }
    public override void OnEvent(SangoNetMessage sangoNetMessage)
    {
        string chooseAvaterCodeJson = sangoNetMessage.MessageBody.MessageString;
        ChooseAvaterCode chooseAvaterCode = DeJsonString<ChooseAvaterCode>(chooseAvaterCodeJson);
        AvaterCode avater = chooseAvaterCode.AvaterCode;
        string account = chooseAvaterCode.Account;
        if (account != OnlineAccountCache.Instance.LocalAccount)
        {
            IslandOnlineAccountSystem.Instance.SetChoosedAvater(account, avater);
        }
    }
}

using SangoMMOCommons.Classs;
using SangoMMONetProtocol;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi

public class NewAccountJoinEvent : BaseEvent
{
    public string Account { get; private set; }

    Stack<string> newAccountJoinEventStack = new Stack<string>();

    public override void InitEvent()
    {        
        NetOpCode = OperationCode.SyncPlayerAccount;
        base.InitEvent();
        newAccountJoinEventStack.Push("-1");
    }
    public override void OnEvent(SangoNetMessage sangoNetMessage)
    {
        string newAccountOnlineJson = sangoNetMessage.MessageBody.MessageString;
        NewAccountOnline newAccountOnline = DeJsonString<NewAccountOnline>(newAccountOnlineJson);
        string tempAccount = newAccountOnline.Account;
        if (tempAccount != null)
        {
            if (newAccountJoinEventStack.Peek() != tempAccount)
            {
                newAccountJoinEventStack.Push(tempAccount);
                IslandOnlineAccountSystem.Instance.InstantiatePlayerCube(tempAccount);
            }
        }
    }
}

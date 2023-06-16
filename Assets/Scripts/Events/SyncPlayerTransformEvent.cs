using SangoMMOCommons.Classs;
using SangoMMONetProtocol;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: Sync Player Transform Event.

public class SyncPlayerTransformEvent : BaseEvent
{
    public override void InitEvent()
    {        
        NetOpCode = OperationCode.SyncPlayerTransform;
        base.InitEvent();
    }
    public override void OnEvent(SangoNetMessage sangoNetMessage)
    {
        string playerTransformJson = sangoNetMessage.MessageBody.MessageString;
        if (playerTransformJson != null && IslandOnlineAccountSystem.Instance != null)
        {
            List<TransformOnline> playerTransformList = DeJsonString<List<TransformOnline>>(playerTransformJson);
            IslandOnlineAccountSystem.Instance.SetOnlineAvaterTransform(playerTransformList);
        }
    }
}

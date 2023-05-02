using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: Sync Player Transform Event.

public class SyncPlayerTransformEvent : BaseEvent
{
    public override void InitEvent()
    {
        base.InitEvent();
    }
    public override void OnEvent(EventData eventData)
    {
        string playerTransformJson = DictTools.GetStringValue(eventData.Parameters, (byte)ParameterCode.PlayerTransformList);
        if (playerTransformJson != null && IslandOnlineAccountSystem.Instance != null)
        {
            List<TransformOnline> playerTransformList = DeJsonString<List<TransformOnline>>(playerTransformJson);
            IslandOnlineAccountSystem.Instance.SetOnlineAvaterTransform(playerTransformList);
        }
    }
}

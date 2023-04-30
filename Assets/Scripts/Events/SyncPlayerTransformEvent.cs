using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription:

public class SyncPlayerTransformEvent : BaseEvent
{
    public override void InitEvent()
    {
        base.InitEvent();
    }
    public override void OnEvent(EventData eventData)
    {
        string playerTransformCacheJson = DictTools.GetStringValue(eventData.Parameters, (byte)ParameterCode.PlayerTransformCacheList);
        if (playerTransformCacheJson != null && IslandOnlineAccountSystem.Instance != null)
        {
            List<TransformOnline> playerTransformCacheList = DeJsonString<List<TransformOnline>>(playerTransformCacheJson);
            IslandOnlineAccountSystem.Instance.SetOnlineAvaterTransform(playerTransformCacheList);
        }
    }
}

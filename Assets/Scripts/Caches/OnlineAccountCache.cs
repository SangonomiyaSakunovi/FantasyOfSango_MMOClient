using SangoCommon.Classs;
using SangoCommon.Enums;

//Developer : SangonomiyaSakunovi
//Discription:

public class OnlineAccountCache : BaseCache
{
    public static OnlineAccountCache Instance = null;

    public AvaterCode LocalAvater { get; private set; }
    public AvaterInfo AvaterInfo { get; private set; }
    public MissionInfo MissionInfo { get; private set; }

    public override void InitCache()
    {
        base.InitCache();
        Instance = this;
    }

    public void SetAvaterInfo(AvaterInfo avaterInfo)
    {
        AvaterInfo = avaterInfo;
    }

    public void SetMissionInfo(MissionInfo missionInfo)
    {
        MissionInfo = missionInfo;
    }

    public void SetLocalAvater(AvaterCode avaterCode)
    {
        LocalAvater = avaterCode;
    }
}

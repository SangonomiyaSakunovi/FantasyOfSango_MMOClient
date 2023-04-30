using SangoCommon.Classs;

//Developer : SangonomiyaSakunovi
//Discription:

public class OnlineAccountCache : BaseCache
{
    public static OnlineAccountCache Instance = null;

    public AvaterInfo AvaterInfo { get; private set; }

    public override void InitCache()
    {
        base.InitCache();
        Instance = this;
    }

    public void SetPlayerCache(AvaterInfo playerCache)
    {
        AvaterInfo = playerCache;
    }

}

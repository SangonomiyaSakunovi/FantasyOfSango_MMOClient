using UnityEngine;
using YooAsset;

//Developer: SangonomiyaSakunovi

public class HotFixConfig : MonoBehaviour
{
    private EPlayMode ePlayMode = EPlayMode.EditorSimulateMode;
    private CDNServerModeCode cndServerMode = CDNServerModeCode.Local;
    private SangoApplicationCode sangoApplication = SangoApplicationCode.FOS_MMO;

    #region CDNServerConfig
    public string GetCNDServerAddress()
    {
        string cndAddress = "";
        switch (cndServerMode)
        {
            case CDNServerModeCode.Local:
                switch (sangoApplication)
                {
                    case SangoApplicationCode.FOS_MMO:
                        cndAddress = "http://127.0.0.1/CNDServer_MMO";
                        break;
                    case SangoApplicationCode.FOS_AR:
                        cndAddress = "http://127.0.0.1/CDNServer_AR";
                        break;
                }
                break;
            case CDNServerModeCode.Remote:
                //TODO
                break;
        }
        return cndAddress;
    }

    public EPlayMode GetEPlayMode()
    {
        return ePlayMode;
    }

    public enum CDNServerModeCode
    {
        Local,
        Remote
    }

    public enum SangoApplicationCode
    {
        FOS_MMO,
        FOS_AR
    }
    #endregion
}

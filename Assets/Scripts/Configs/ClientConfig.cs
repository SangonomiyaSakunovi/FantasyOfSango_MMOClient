using UnityEngine;
using YooAsset;

//Developer: SangonomiyaSakunovi

public class ClientConfig : MonoBehaviour
{
    //Do not call these directly!!!
    public SangoServerModeCode sangoServerMode = SangoServerModeCode.Offline;
    public SangoApplicationCode sangoApplication = SangoApplicationCode.FOS_MMO;
    public EPlayMode ePlayMode = EPlayMode.EditorSimulateMode;
    public CDNServerModeCode cndServerMode = CDNServerModeCode.Local;

    public static ClientConfig Instance;

    public void InitConfig()
    {
        Instance = this;
    }

    #region ClientIPConfig
    public string GetIPAddress()
    {
        string ipAddress = "";
        switch (sangoServerMode)
        {
            case SangoServerModeCode.Offline:
                ipAddress = "127.0.0.1";
                break;
            case SangoServerModeCode.Online:
                ipAddress = "124.220.20.98";
                break;
        }
        return ipAddress;
    }

    public int GetPort()
    {
        int port = 0;
        switch (sangoApplication)
        {
            case SangoApplicationCode.FOS_MMO:
                port = 52022;
                break;
            case SangoApplicationCode.FOS_AR:
                port = 52037;
                break;
        }
        return port;
    }   

    public enum SangoServerModeCode
    {
        Offline,
        Online
    }

    public enum SangoApplicationCode
    {
        FOS_MMO,
        FOS_AR
    }
    #endregion

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
    #endregion
}

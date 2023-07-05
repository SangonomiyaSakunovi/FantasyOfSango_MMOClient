using UnityEngine;

//Developer: SangonomiyaSakunovi

public class ClientConfig : MonoBehaviour
{
    private SangoServerModeCode sangoServerMode = SangoServerModeCode.Offline;
    private SangoApplicationCode sangoApplication = SangoApplicationCode.FOS_MMO;

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
}

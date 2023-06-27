using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Developer: SangonomiyaSakunovi

public class HotFixSystem : BaseSystem
{
    public static HotFixSystem Instance;

    public HotFixWindow hotFixWindow;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
    }

    public void EnterHotFix()
    {

    }

    public void RunHotFix()
    {
        HotFixService.Instance.DownloadSangoAssets();
        HotFixService.Instance.LoadDll();
    }

    public void OnHotFix()
    {
        //TODO
        //服务器主动发来的事件，要求进行热更新，该处先不做
    }
}

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
        //�����������������¼���Ҫ������ȸ��£��ô��Ȳ���
    }
}

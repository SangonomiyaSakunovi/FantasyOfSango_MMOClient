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

    public void OpenHotFixWindow(long totalDownloadBytes)
    {
        hotFixWindow.SetHotFixInfoText(totalDownloadBytes);
        hotFixWindow.SetWindowState();
    }

    public void CloseHotFixWindow()
    {
        hotFixWindow.SetWindowState(false);
    }

    public void RunHotFix()
    {
        HotFixService.Instance.RunHotFix();
        hotFixWindow.SetWindowState(false);
    }

    public void OnHotFix()
    {
        //TODO
        //�����������������¼���Ҫ������ȸ��£��ô��Ȳ���
    }
}

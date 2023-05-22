using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The WeaponsEnhanceSystem.

public class WeaponsEnhanceSystem : BaseSystem
{
    public static WeaponsEnhanceSystem Instance = null;

    public AvaterInfoWindow avaterInfoWindow;
    public WeaponsEnhanceWindow weaponsEnhanceWindow;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
    }

    public void OpenWeaponsEnhanceWindow()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        avaterInfoWindow.SetWindowState(false);
        weaponsEnhanceWindow.SetWindowState();
    }

    public void CloseWeaponsEnhanceWindow()
    {
        audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
        weaponsEnhanceWindow.SetWindowState(false);
        avaterInfoWindow.SetWindowState();        
    }
}

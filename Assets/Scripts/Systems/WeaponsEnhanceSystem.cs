using SangoCommon.Classs;
using SangoCommon.Enums;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The WeaponsEnhanceSystem.

public class WeaponsEnhanceSystem : BaseSystem
{
    public static WeaponsEnhanceSystem Instance = null;

    public AvaterInfoWindow avaterInfoWindow;
    public WeaponsEnhanceWindow weaponsEnhanceWindow;

    private ItemEnhanceRequest itemEnhanceRequest;

    private WeaponEnhanceResultPreCal weaponEnhanceResultPreCal;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        itemEnhanceRequest = GetComponent<ItemEnhanceRequest>();
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

    public bool IsCanEnhanceWeapon(int enhanceCoinRequire)
    {
        bool isCanEnhanceWeapon = false;
        if (enhanceCoinRequire <= OnlineAccountCache.Instance.ItemInfo.Coin)
        {
            isCanEnhanceWeapon = true;
        }
        return isCanEnhanceWeapon;
    }

    public WeaponEnhanceResultPreCal GetWeaponEnhanceResultPreCal(ItemEnhanceReq itemEnhanceReq)
    {        
        weaponEnhanceResultPreCal.WeaponEnhanceCoinConsume = 100 * itemEnhanceReq.ItemModelMaterialList.Count;
        
        WeaponValueConfig avaterWeaponValueConfig = resourceService.GetWeaponValueConfig(itemEnhanceReq.ItemId);
        int tempWeaponExp = OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0].WeaponExp;
        for (int i = 0; i < itemEnhanceReq.ItemModelMaterialList.Count; i++)
        {
            WeaponValueConfig materialWeaponValueConfig = resourceService.GetWeaponValueConfig(itemEnhanceReq.ItemModelMaterialList[i]);
            tempWeaponExp +=  materialWeaponValueConfig.weaponAccumulateExp / 2;           
        }
        for (int i = 0; i < itemEnhanceReq.ItemRawMaterialList.Count; i++)
        {

        }      
        if (tempWeaponExp < avaterWeaponValueConfig.weaponEnhanceLevelExp)
        {
            weaponEnhanceResultPreCal.WeaponEnhanceExpResult = tempWeaponExp;
            weaponEnhanceResultPreCal.WeaponEnhanceIdResult = itemEnhanceReq.ItemId;
        }
        else
        {
            string[] weaponIdSplit = itemEnhanceReq.ItemId.Split('_');
            RecursionCallUpdateOnlineAccountWeaponInfo(weaponIdSplit, tempWeaponExp, avaterWeaponValueConfig.weaponEnhanceLevelExp);
        }
        return weaponEnhanceResultPreCal;
    }

    private void RecursionCallUpdateOnlineAccountWeaponInfo(string[] tempWeaponIdSplit, int oldTempExp, int oldRequireExp)
    {
        if (int.Parse(tempWeaponIdSplit[2]) % 5 == 0)
        {
            //TODO How to deal this leftExp?
            int leftExp = oldTempExp - oldRequireExp;
            weaponEnhanceResultPreCal.WeaponEnhanceExpResult = 0;
            weaponEnhanceResultPreCal.WeaponEnhanceIdResult = tempWeaponIdSplit[0] + "_" + tempWeaponIdSplit[1] + "_" + tempWeaponIdSplit[2] + "_S";
        }
        else
        {
            tempWeaponIdSplit[2] = (int.Parse(tempWeaponIdSplit[2]) + 1).ToString();
            string newWeaponId = tempWeaponIdSplit[0] + "_" + tempWeaponIdSplit[1] + "_" + tempWeaponIdSplit[2];
            int newWeaponExp = oldTempExp - oldRequireExp;
            WeaponValueConfig newAvaterWeaponValueConfig = resourceService.GetWeaponValueConfig(newWeaponId);
            if (newWeaponExp > newAvaterWeaponValueConfig.weaponEnhanceLevelExp)
            {
                RecursionCallUpdateOnlineAccountWeaponInfo(tempWeaponIdSplit, newWeaponExp, newAvaterWeaponValueConfig.weaponEnhanceLevelExp);
            }
            else
            {
                weaponEnhanceResultPreCal.WeaponEnhanceIdResult = newWeaponId;
                weaponEnhanceResultPreCal.WeaponEnhanceExpResult = newWeaponExp;
            }
        }
    }

    public void SendWeaponEnhanceRequest(ItemEnhanceReq itemEnhanceReq)
    {
        bool isCanEnhanceOrBreakWeapon = false;
        if (itemEnhanceReq.EnhanceTypeCode == EnhanceTypeCode.Enhance)
        {
            isCanEnhanceOrBreakWeapon = IsCanEnhanceWeapon(weaponEnhanceResultPreCal.WeaponEnhanceCoinConsume);
        }
        else
        {
            isCanEnhanceOrBreakWeapon = IsCanBreakWeapon(itemEnhanceReq);
        }
        if (isCanEnhanceOrBreakWeapon)
        {
            itemEnhanceRequest.SetWeaponEnhanceReq(itemEnhanceReq);
            itemEnhanceRequest.DefaultRequest();
        }
        else
        {
            SangoRoot.AddMessage("摩拉不够了，需要继续任务寻找更多的摩拉哦", TextColorCode.PurpleColor);
        }
    }

    private bool IsCanBreakWeapon(ItemEnhanceReq itemEnhanceReq)
    {
        bool isCanBreakWeapon = false;
        //TODO
        return isCanBreakWeapon;
    }

    public void OnWeaponEnhanceResponse(ItemEnhanceRsp itemEnhanceRsp)
    {
        if (itemEnhanceRsp.EnhanceTypeCode == EnhanceTypeCode.Enhance)
        {
            UpdateWeaponEnhanceResult(itemEnhanceRsp);
        }
        else
        {
            UpdateWeaponBreakResult(itemEnhanceRsp);
        }
    }

    private void UpdateWeaponEnhanceResult(ItemEnhanceRsp itemEnhanceRsp)
    {
        //TODO
        SangoRoot.AddMessage("强化成功", TextColorCode.GreenColor);
        OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0].WeaponId = weaponEnhanceResultPreCal.WeaponEnhanceIdResult;
        OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0].WeaponExp = weaponEnhanceResultPreCal.WeaponEnhanceExpResult;
        OnlineAccountCache.Instance.ItemInfo.Coin -= weaponEnhanceResultPreCal.WeaponEnhanceCoinConsume;
        weaponsEnhanceWindow.RefreshUI();
    }

    private void UpdateWeaponBreakResult(ItemEnhanceRsp itemEnhanceRsp)
    {

    }
}

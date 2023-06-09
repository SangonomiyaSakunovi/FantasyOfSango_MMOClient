using SangoCommon.Classs;
using SangoCommon.Enums;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: The WeaponsEnhanceWindow.

public class WeaponsEnhanceWindow : BaseWindow
{
    public Transform functionButtonsBaseTransform;
    public Transform enhanceMaterialShowBaseTransform;
    public Transform enhanceMaterialAddButtonsBaseTransform;
    public Button closeButton;

    private Image[] functionButtonImageArray = new Image[3];
    private Image[] enhanceMaterialShowImageArray = new Image[6];
    private Image[] enhanceMaterialAddButtonImageArray = new Image[6];

    private int functionButtonCurrentIndex;
    private int functionWindowCurrentIndex;

    public TMP_Text weaponType;
    public TMP_Text weaponName;
    public TMP_Text weaponLevel;
    public TMP_Text weaponExp;
    public TMP_Text weaponBaseATK;
    public TMP_Text weaponAbility1Name;
    public TMP_Text weaponAbility1Value;
    public TMP_Text weaponAbility2Name;
    public TMP_Text weaponAbility2Value;

    public TMP_Text coinLeftValue;

    public TMP_Text weaponEnhanceRequireCoin;
    public Button enhanceWeaponButton;

    public GameObject EnhanceWnd;

    private ItemEnhanceReq itemEnhanceReq;
    private int[] enhanceMaterialAddResultArray = new int[6];

    protected override void InitWindow()
    {
        base.InitWindow();
        RegistFunctionButtonClickEvents();       
        OnFunctionButtonClick(1);
        RefreshUI();                
    }

    protected override void ClearWindow()
    {
        base.ClearWindow();
        EndFunctionButtonClickEvents();
    }

    public void RefreshUI()
    {
        InitRequest();
        InitUI();
        InitEnhanceMaterialButtonClick();
    }

    private void InitRequest()
    {
        functionButtonCurrentIndex = -1;
        //TODO Twe should record the item chooosed
        if (itemEnhanceReq == null)
        {
            itemEnhanceReq = new ItemEnhanceReq
            {
                ItemTypeCode = ItemTypeCode.Weapon,
                EnhanceTypeCode = EnhanceTypeCode.Enhance,
                ItemId = OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0].WeaponId,
                ItemModelMaterialList = new List<string>(),
                ItemRawMaterialList = new List<string>()
            };
        }
        else
        {
            itemEnhanceReq.ItemTypeCode = ItemTypeCode.Weapon;
            itemEnhanceReq.EnhanceTypeCode = EnhanceTypeCode.Enhance;
            itemEnhanceReq.ItemId = OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0].WeaponId;
            itemEnhanceReq.ItemModelMaterialList.Clear();
            itemEnhanceReq.ItemRawMaterialList.Clear();
        }

    }

    private void InitUI()
    {
        SetText(coinLeftValue, OnlineAccountCache.Instance.ItemInfo.Coin);
        string[] weaponIdSplitArray = OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0].WeaponId.Split('_');
        SetText(weaponType, weaponIdSplitArray[0]);
        WeaponDetailsConfig weaponDetailsConfig = resourceService.GetWeaponDetailsConfig(weaponIdSplitArray[0] + "_" + weaponIdSplitArray[1]);
        SetText(weaponName, weaponDetailsConfig.weaponName);
        SetText(weaponLevel, "Lv. " + weaponIdSplitArray[2]);
        SetText(weaponEnhanceRequireCoin, 0);
        WeaponValueConfig weaponValueConfig = resourceService.GetWeaponValueConfig(OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0].WeaponId);
        SetText(weaponBaseATK, weaponValueConfig.weaponBaseATK);
        SetText(weaponExp, OnlineAccountCache.Instance.AvaterInfo.AttributeInfoList[0].WeaponExp + "/" + weaponValueConfig.weaponEnhanceLevelExp);
        string[] weaponAbility1 = GetWeaponAbilitySplitArrary(weaponValueConfig.weaponAbility1);
        if (weaponAbility1 != null)
        {
            SetText(weaponAbility1Name, weaponAbility1[0]);
            SetText(weaponAbility1Value, weaponAbility1[1]);
        }
        else
        {
            SetText(weaponAbility1Name, "???");
            SetText(weaponAbility1Value, "?");
        }
        string[] weaponAbility2 = GetWeaponAbilitySplitArrary(weaponValueConfig.weaponAbility2);
        if (weaponAbility2 != null)
        {
            SetText(weaponAbility2Name, weaponAbility2[0]);
            SetText(weaponAbility2Value, weaponAbility2[1]);
        }
        else
        {
            SetText(weaponAbility2Name, "???");
            SetText(weaponAbility2Value, "?");
        }
    }

    private string[] GetWeaponAbilitySplitArrary(string weaponAbility)
    {
        if (weaponAbility == "-1")
        {
            return null;
        }
        else
        {
            return weaponAbility.Split('#');
        }
    }


    private void RegistFunctionButtonClickEvents()
    {
        for (int i = 0; i < functionButtonsBaseTransform.childCount; i++)
        {
            Image image = functionButtonsBaseTransform.GetChild(i).GetComponent<Image>();
            OnClick(image.gameObject, (object args) =>
            {
                OnFunctionButtonClick((int)args);
                audioService.PlayUIAudio(AudioConstant.ClickUIButton);
            }, i);
            functionButtonImageArray[i] = image;
        }
        for (int j = 0; j < enhanceMaterialAddButtonsBaseTransform.childCount; j++)
        {
            Image buttonImage = enhanceMaterialAddButtonsBaseTransform.GetChild(j).GetComponent<Image>();
            Image showImage = enhanceMaterialShowBaseTransform.GetChild(j).GetComponent<Image>();
            OnClick(buttonImage.gameObject, (object args) =>
            {
                OnEnhanceMaterialButtonClick((int)args);
                audioService.PlayUIAudio(AudioConstant.ClickUIButton);
            }, j);
            enhanceMaterialAddButtonImageArray[j] = buttonImage;
            enhanceMaterialAddResultArray[j] = 0;
            enhanceMaterialShowImageArray[j] = showImage;
        }
    }

    private void EndFunctionButtonClickEvents()
    {
        for (int i = 0; i < functionButtonsBaseTransform.childCount; i++)
        {
            Image image = functionButtonsBaseTransform.GetChild(i).GetComponent<Image>();
            OnEndClickEvents(image.gameObject);
            functionButtonImageArray[i] = null;
        }
        for (int j = 0; j < enhanceMaterialAddButtonsBaseTransform.childCount; j++)
        {
            Image image = enhanceMaterialAddButtonsBaseTransform.GetChild(j).GetComponent<Image>();
            OnEndClickEvents(image.gameObject);
            enhanceMaterialAddButtonImageArray[j] = null;
            enhanceMaterialAddResultArray[j] = 0;
        }
    }

    private void OnFunctionButtonClick(int index)
    {
        functionButtonCurrentIndex = index;
        for (int i = 0; i < functionButtonImageArray.Length; i++)
        {
            if (functionButtonImageArray[i] != null)
            {
                if (i == functionButtonCurrentIndex)
                {
                    SetSprite(functionButtonImageArray[i], ButtonUIConstant.ItemFunctionButtonSelectedImagePath);
                    switch (functionButtonCurrentIndex)
                    {
                        case 1:
                            if (functionWindowCurrentIndex != functionButtonCurrentIndex)
                            {
                                SetActive(EnhanceWnd);
                                functionWindowCurrentIndex = functionButtonCurrentIndex;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    SetSprite(functionButtonImageArray[i], ButtonUIConstant.ItemFunctionButtonUnSelectedImagePath);
                }
            }
        }
    }

    private void InitEnhanceMaterialButtonClick()
    {
        for (int i = 0; i < enhanceMaterialShowImageArray.Length; i++)
        {
            enhanceMaterialAddResultArray[i] = 0;
            SetSprite(enhanceMaterialShowImageArray[i], ButtonUIConstant.ItemEnhanceMaterialAddButtonDefaultImagePath);
        }
    }

    private void OnEnhanceMaterialButtonClick(int index)
    {
        if (enhanceMaterialAddButtonImageArray[index] != null)
        {
            if (enhanceMaterialAddResultArray[index] == 0)
            {
                SetSprite(enhanceMaterialShowImageArray[index], WeaponModelUIConstant.SwordBluntImagePath);
                enhanceMaterialAddResultArray[index] = 1;
                //TODO OpenItemChooseWindow, and in this version, you only can add this material
                itemEnhanceReq.ItemModelMaterialList.Add("Sword_Blunt_1");
            }
            else
            {
                SetSprite(enhanceMaterialShowImageArray[index], ButtonUIConstant.ItemEnhanceMaterialAddButtonDefaultImagePath);
                enhanceMaterialAddResultArray[index] = 0;
                //TODO OpenItemChooseWindow, and in this version, you only can add this material
                itemEnhanceReq.ItemModelMaterialList.Remove("Sword_Blunt_1");
            }
            WeaponEnhanceResultPreCal weaponEnhanceResultPreCal = WeaponsEnhanceSystem.Instance.GetWeaponEnhanceResultPreCal(itemEnhanceReq);
            string[] weaponIdSplitArray = weaponEnhanceResultPreCal.WeaponEnhanceIdResult.Split('_');
            SetText(weaponLevel, "Lv. " + weaponIdSplitArray[2], TextColorCode.GreenColor);
            WeaponValueConfig weaponValueConfig = resourceService.GetWeaponValueConfig(weaponEnhanceResultPreCal.WeaponEnhanceIdResult);
            SetText(weaponBaseATK, weaponValueConfig.weaponBaseATK, TextColorCode.GreenColor);
            SetText(weaponExp, weaponEnhanceResultPreCal.WeaponEnhanceExpResult + "/" + weaponValueConfig.weaponEnhanceLevelExp, TextColorCode.GreenColor);
            bool isCanEnhanceWeapon = WeaponsEnhanceSystem.Instance.IsCanEnhanceWeapon(weaponEnhanceResultPreCal.WeaponEnhanceCoinConsume);
            if (isCanEnhanceWeapon)
            {
                SetText(weaponEnhanceRequireCoin, weaponEnhanceResultPreCal.WeaponEnhanceCoinConsume, TextColorCode.GreenColor);
            }
            else
            {
                SetText(weaponEnhanceRequireCoin, weaponEnhanceResultPreCal.WeaponEnhanceCoinConsume, TextColorCode.RedColor);
            }
        }
    }

    public void OnCloseButtonClick()
    {
        WeaponsEnhanceSystem.Instance.CloseWeaponsEnhanceWindow();
    }

    public void OnEnhanceWeaponButtonClick()
    {
        WeaponsEnhanceSystem.Instance.SendWeaponEnhanceRequest(itemEnhanceReq);
    }
}

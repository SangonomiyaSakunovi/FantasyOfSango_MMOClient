//Developer : SangonomiyaSakunovi
//Discription: The AvaterInfoWindow.

using SangoCommon.Classs;
using SangoCommon.Enums;
using TMPro;
using UnityEngine.UI;

public class AvaterInfoWindow : BaseWindow
{
    public TMP_Text avaterName;
    public TMP_Text avaterDescription;
    public TMP_Text avaterElementType;
    public TMP_Text avaterLevel;
    public TMP_Text avaterMaxHP;
    public TMP_Text avaterAttack;
    public TMP_Text avaterDefence;
    public TMP_Text avaterElementalMastery;
    public TMP_Text maxStamina;
    public TMP_Text avaterFriendShip;

    public Button closeButton;

    protected override void InitWindow()
    {
        base.InitWindow();
        RefreshUI(AvaterCode.SangonomiyaKokomi);
    }

    private void RefreshUI(AvaterCode avater)
    {
        AvaterInfo avaterInfo = OnlineAccountCache.Instance.AvaterInfo;
        AvaterAttributeInfo avaterAttribute = null;
        for (int i = 0; i< avaterInfo.AttributeInfoList.Count; i++)
        {
            if (avaterInfo.AttributeInfoList[i].Avater == avater)
            {
                avaterAttribute = avaterInfo.AttributeInfoList[i];
                break;
            }
        }
        SetText(avaterName, avaterAttribute.Avater.ToString());
        SetText(avaterElementType, avaterAttribute.ElementType.ToString());
        SetText(avaterMaxHP, avaterAttribute.HPFull);
        SetText(avaterAttack, avaterAttribute.Attack);
        SetText(avaterDefence, avaterAttribute.Defence);
    }

    public void OnCloseButtonClick()
    {
        AvaterInfoSystem.Instance.CloseAvaterInfoWindow();
    }
}

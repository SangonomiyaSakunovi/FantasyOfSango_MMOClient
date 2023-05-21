//Developer : SangonomiyaSakunovi
//Discription: The AvaterInfoWindow.

using SangoCommon.Classs;
using SangoCommon.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public RawImage avaterShowRawImage;
    private Vector2 clickPos;

    public Button closeButton;
    public Button weaponsButton;

    protected override void InitWindow()
    {
        base.InitWindow();
        RegistTouchEvent();
        RefreshUI(AvaterCode.SangonomiyaKokomi);
    }

    private void RefreshUI(AvaterCode avater)
    {
        AvaterInfo avaterInfo = OnlineAccountCache.Instance.AvaterInfo;
        AvaterAttributeInfo avaterAttribute = null;
        for (int i = 0; i < avaterInfo.AttributeInfoList.Count; i++)
        {
            if (avaterInfo.AttributeInfoList[i].Avater == avater)
            {
                avaterAttribute = avaterInfo.AttributeInfoList[i];
                break;
            }
        }
        string avaterNameTemp = "";
        switch (avater)
        {
            case AvaterCode.SangonomiyaKokomi:
                avaterNameTemp = "珊瑚宫心海";
                break;
            case AvaterCode.Yoimiya:
                avaterNameTemp = "宵宫";
                break;
            case AvaterCode.Ayaka:
                avaterNameTemp = "神里绫华";
                break;
            case AvaterCode.Aether:
                avaterNameTemp = "旅行者：空";
                break;
        }
        SetText(avaterName, avaterNameTemp);
        SetText(avaterElementType, avaterAttribute.ElementType.ToString());
        SetText(avaterMaxHP, avaterAttribute.HPFull);
        SetText(avaterAttack, avaterAttribute.Attack);
        SetText(avaterDefence, avaterAttribute.Defence);
    }

    public void OnCloseButtonClick()
    {
        AvaterInfoSystem.Instance.CloseAvaterInfoWindow();
    }

    public void OnWeaponsButtonClick()
    {

    }

    private void RegistTouchEvent()
    {
        OnClickDown(avaterShowRawImage.gameObject, (PointerEventData pointerEvent) =>
        {
            clickPos = pointerEvent.position;
            AvaterInfoSystem.Instance.SetAvaterShowCubeCurrentRotation();
        });
        OnDrag(avaterShowRawImage.gameObject, (PointerEventData pointerEvent) =>
        {
            float rotation = -(pointerEvent.position.x - clickPos.x) * 0.5f;
            AvaterInfoSystem.Instance.SetAvaterShowRotation(rotation);
        });
    }
}

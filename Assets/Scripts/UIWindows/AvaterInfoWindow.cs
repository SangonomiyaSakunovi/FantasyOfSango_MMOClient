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

    public Transform buttonsTransform;
    private Image[] buttonBGImageArray = new Image[5];
    private int currentButtonIndex;

    protected override void InitWindow()
    {
        base.InitWindow();
        RegistTouchEvents();
        RefreshUI(AvaterCode.SangonomiyaKokomi);
        RegistClickEvents();
        OnItemClick(0);
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
        SetText(avaterName, avaterNameTemp, TextColorCode.WhiteColor);
        SetText(avaterElementType, avaterAttribute.ElementType.ToString(), TextColorCode.WhiteColor);
        SetText(avaterMaxHP, avaterAttribute.HPFull, TextColorCode.WhiteColor);
        SetText(avaterAttack, avaterAttribute.Attack, TextColorCode.WhiteColor);
        SetText(avaterDefence, avaterAttribute.Defence, TextColorCode.WhiteColor);
    }

    private void RegistClickEvents()
    {
        for (int i = 0; i < buttonsTransform.childCount; i++)
        {
            Image image = buttonsTransform.GetChild(i).GetComponent<Image>();
            OnClick(image.gameObject, (object args) =>
            {
                OnItemClick((int)args);
                audioService.PlayUIAudio(AudioConstant.ClickButtonUI);
            }, i);
            buttonBGImageArray[i] = image;
        }
    }

    private void OnItemClick(int index)
    {
        currentButtonIndex = index;
        for (int i = 0; i < buttonBGImageArray.Length; i++)
        {
            Transform trans = buttonBGImageArray[i].transform;
            if (i == currentButtonIndex)
            {
                SetSprite(buttonBGImageArray[i], ButtonConstant.EnhanceSelectedImagePath);
            }
            else
            {
                SetSprite(buttonBGImageArray[i], ButtonConstant.EnhanceUnSelectedImagePath);
            }
        }
    }

    public void OnCloseButtonClick()
    {
        AvaterInfoSystem.Instance.CloseAvaterInfoWindow();
    }

    private void RegistTouchEvents()
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

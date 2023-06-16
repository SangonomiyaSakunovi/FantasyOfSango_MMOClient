using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi

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

    public Transform functionButtonsBaseTransform;
    private Image[] functionButtonImageArray = new Image[5];
    private int functionButtonCurrentIndex;
    private int functionWindowCurrentIndex;

    protected override void InitWindow()
    {
        base.InitWindow();
        RegistAvaterTouchEvents();
        RegistFunctionButtonClickEvents();
        InitUI();
        RefreshUI(AvaterCode.SangonomiyaKokomi);
        OnFunctionButtonClick(0);
    }

    protected override void ClearWindow()
    {
        base.ClearWindow();
        EndAvaterTouchEvents();
        EndFunctionButtonClickEvents();
    }

    public void InitUI()
    {
        functionButtonCurrentIndex = -1;
        functionWindowCurrentIndex = -1;
    }

    public void RefreshUI(AvaterCode avater)
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

    private void RegistFunctionButtonClickEvents()
    {
        for (int i = 0; i < functionButtonsBaseTransform.childCount; i++)
        {
            Image image = functionButtonsBaseTransform.GetChild(i).GetComponent<Image>();
            OnClick(image.gameObject, (object args) =>
            {
                audioService.PlayUIAudio(AudioConstant.ClickUIButton);
                OnFunctionButtonClick((int)args);             
            }, i);
            functionButtonImageArray[i] = image;
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
    }

    private void OnFunctionButtonClick(int index)
    {
        functionButtonCurrentIndex = index;
        for (int i = 0; i < functionButtonImageArray.Length; i++)
        {
            if (functionButtonImageArray[i] != null)
            {
                Transform trans = functionButtonImageArray[i].transform;
                if (i == functionButtonCurrentIndex)
                {
                    SetSprite(functionButtonImageArray[i], ButtonUIConstant.ItemFunctionButtonSelectedImagePath);
                    switch (functionButtonCurrentIndex)
                    {
                        case 1:
                            if (functionWindowCurrentIndex != functionButtonCurrentIndex)
                            {
                                WeaponsEnhanceSystem.Instance.OpenWeaponsEnhanceWindow();
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

    public void OnCloseButtonClick()
    {
        AvaterInfoSystem.Instance.CloseAvaterInfoWindow();
    }

    private void RegistAvaterTouchEvents()
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

    private void EndAvaterTouchEvents()
    {
        OnEndClickEvents(avaterShowRawImage.gameObject);
    }
}

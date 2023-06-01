using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: Base window, need define UI method.

public class BaseWindow : MonoBehaviour
{
    protected NetService netService = null;
    protected ResourceService resourceService = null;
    protected AudioService audioService = null;
    public void SetWindowState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWindow();
        }
        else
        {
            ClearWindow();
        }
    }

    protected virtual void InitWindow()
    {
        netService = NetService.Instance;
        resourceService = ResourceService.Instance;
        audioService = AudioService.Instance;
    }

    protected virtual void ClearWindow()
    {
        netService = null;
        resourceService = null;
        audioService = null;
    }

    protected T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t == null)
        {
            t = gameObject.AddComponent<T>();
        }
        return t;
    }

    protected void RemoveComponent<T>(GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t != null)
        {
            Destroy(t);
        }
    }

    protected string GetTextWithHexColor(string text, TextColorCode textColor)
    {
        string result = "";
        string colorStart = "<color=";
        string colorEnd = ">";
        string textEnd = "</color>";
        switch (textColor)
        {
            case TextColorCode.WhiteColor:
                result = colorStart + ColorConstant.WhiteColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.RedColor:
                result = colorStart + ColorConstant.RedColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.GreenColor:
                result = colorStart + ColorConstant.GreenColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.BlueColor:
                result = colorStart + ColorConstant.BlueColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.CyanColor:
                result = colorStart + ColorConstant.CyanColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.PurpleColor:
                result = colorStart + ColorConstant.PurpleColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.GoldColor:
                result = colorStart + ColorConstant.GoldColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.OrangeColor:
                result = colorStart + ColorConstant.OrangeColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.VaporizeColor:
                result = colorStart + ColorConstant.VaporizeColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.MeltColor:
                result = colorStart + ColorConstant.MeltColorHex + colorEnd + text + textEnd;
                break;
            case TextColorCode.FrozenColor:
                result = colorStart + ColorConstant.FrozenColorHex + colorEnd + text + textEnd;
                break;
        }
        return result;
    }

    #region SetText
    protected void SetText(TMP_Text tMP_Text, string text)
    {
        tMP_Text.text = text;
    }
    protected void SetText(TMP_Text tMP_Text, int number)
    {
        tMP_Text.text = number.ToString();
    }
    protected void SetText(TMP_Text tMP_Text, string text, TextColorCode textColor)
    {
        string result = GetTextWithHexColor(text, textColor);
        tMP_Text.text = result;
    }
    protected void SetText(TMP_Text tMP_Text, int number, TextColorCode textColor)
    {
        string result = GetTextWithHexColor(number.ToString(), textColor);
        tMP_Text.text = result;
    }
    #endregion   

    #region SetActive
    protected void SetActive(GameObject gameObject, bool isActive = true)
    {
        gameObject.SetActive(isActive);
    }
    protected void SetActive(Image image, bool isActive = true)
    {
        image.transform.gameObject.SetActive(isActive);
    }
    protected void SetActive(TMP_Text tMP_Text, bool isActive = true)
    {
        tMP_Text.transform.gameObject.SetActive(isActive);
    }
    protected void SetActive(Button button, bool isActive = true)
    {
        button.transform.gameObject.SetActive(isActive);
    }
    #endregion

    #region SetSprite
    protected void SetSprite(Image image, string path, bool isCache = false)
    {
        Sprite sprite = resourceService.LoadSprite(path, isCache);
        Image imageComponent = image.GetComponent<Image>();
        imageComponent.sprite = sprite;
    }
    #endregion

    #region ClickEvents
    protected void OnClick(GameObject gameObject, Action<object> pointerObject, object clickArgs)
    {
        ClickListener clickListener = GetOrAddComponent<ClickListener>(gameObject);
        clickListener.onClick = pointerObject;
        clickListener.clickArguments = clickArgs;
    }

    protected void OnClickDown(GameObject gameObject, Action<PointerEventData> pointerEvent)
    {
        ClickListener clickListener = GetOrAddComponent<ClickListener>(gameObject);
        clickListener.onClickDown = pointerEvent;
    }

    protected void OnClickUp(GameObject gameObject, Action<PointerEventData> pointerEvent)
    {
        ClickListener clickListener = GetOrAddComponent<ClickListener>(gameObject);
        clickListener.onClickUp = pointerEvent;
    }

    protected void OnDrag(GameObject gameObject, Action<PointerEventData> pointerEvent)
    {
        ClickListener clickListener = GetOrAddComponent<ClickListener>(gameObject);
        clickListener.onDrag = pointerEvent;
    }
    
    protected void OnEndClickEvents(GameObject gameObject)
    {
        RemoveComponent<ClickListener>(gameObject);
    }
    #endregion
}

using SangoCommon.Constants;
using TMPro;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription: 

public class ShopInfoConfirmWindow : BaseWindow
{
    public TMP_Text shopInfoText;
    public Button shopConfirmButton;
    public Button shopCancelButton;
    public TMP_InputField shopNumberInputField;

    private string shopItemId;

    protected override void InitWindow()
    {
        base.InitWindow();
        RefreshUI();
    }

    public void SetShopItemId(string shopItem)
    {
        shopItemId = shopItem;
    }

    private void RefreshUI()
    {
        string tempShowText = "";
        switch (shopItemId)
        {
            case ItemConstant.Resin:
                tempShowText = "花费" + GetTextWithHexColor("1金币", TextColorCode.RedColor) + "可购买" + GetTextWithHexColor("1点", TextColorCode.RedColor) + "体力";               
                break;
            case ItemConstant.Coin:
                tempShowText = "花费" + GetTextWithHexColor("100希夏普币", TextColorCode.RedColor) + "可购买" + GetTextWithHexColor("1000枚", TextColorCode.RedColor) + "金币";
                break;
        }
        SetText(shopInfoText, tempShowText);
        shopNumberInputField.text = "";
    }

    public void OnShopConfirmButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        string shopNumberInput = shopNumberInputField.text;
        if (shopNumberInput != "")
        {
            bool isNumeral = ExamIfInputIsNumeral(shopNumberInput);
            if (isNumeral)
            {
                int shopNumber = int.Parse(shopNumberInput);
                ShopInfoSystem.Instance.SendShopInfoRequest(shopNumber);
            }
            else
            {
                SangoRoot.AddMessage("亲，只能输入数字哦", TextColorCode.RedColor);
            }
        }
        SangoRoot.AddMessage("亲，请输入购买数量哦", TextColorCode.RedColor);
    }

    public void OnShopCancelButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        ShopInfoSystem.Instance.CloseShopInfoConfirmWindow();
    }
}

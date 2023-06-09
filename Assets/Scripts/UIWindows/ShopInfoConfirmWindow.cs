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
                tempShowText = "����" + GetTextWithHexColor("1���", TextColorCode.RedColor) + "�ɹ���" + GetTextWithHexColor("1��", TextColorCode.RedColor) + "����";               
                break;
            case ItemConstant.Coin:
                tempShowText = "����" + GetTextWithHexColor("100ϣ���ձ�", TextColorCode.RedColor) + "�ɹ���" + GetTextWithHexColor("1000ö", TextColorCode.RedColor) + "���";
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
                SangoRoot.AddMessage("�ף�ֻ����������Ŷ", TextColorCode.RedColor);
            }
        }
        SangoRoot.AddMessage("�ף������빺������Ŷ", TextColorCode.RedColor);
    }

    public void OnShopCancelButtonClick()
    {
        audioService.PlayUIAudio(AudioConstant.ClickUIButton);
        ShopInfoSystem.Instance.CloseShopInfoConfirmWindow();
    }
}

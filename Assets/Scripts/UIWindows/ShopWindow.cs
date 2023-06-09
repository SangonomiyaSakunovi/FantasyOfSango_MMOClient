using SangoCommon.Constants;
using UnityEngine.UI;

//Developer : SangonomiyaSakunovi
//Discription:

public class ShopWindow : BaseWindow
{
    public Button coinButton;
    public Button resinButton;

    public Button closeButton;

    protected override void InitWindow()
    {
        base.InitWindow();
    }

    public void OnCoinButtonClick()
    {
        ShopInfoSystem.Instance.OpenShopInfoConfirmWindow(ItemConstant.Coin);
    }
    public void OnResinButtonClick()
    {
        ShopInfoSystem.Instance.OpenShopInfoConfirmWindow(ItemConstant.Resin);
    }

    public void OnCloseButtonClick()
    {
        ShopInfoSystem.Instance.CloseShopWindow();
    }
}

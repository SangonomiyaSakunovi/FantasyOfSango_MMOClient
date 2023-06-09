using SangoCommon.Classs;

//Developer : SangonomiyaSakunovi
//Discription: 

public class ShopInfoSystem : BaseSystem
{
    public static ShopInfoSystem Instance = null;

    public ShopWindow shopWindow;
    public ShopInfoConfirmWindow shopInfoConfirmWindow;
    public MainGameWindow mainGameWindow;

    private ShopInfoRequest shopInfoRequest;

    private string currentShopItemId;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        shopInfoRequest = GetComponent<ShopInfoRequest>();
    }

    public void SendShopInfoRequest(int shopNum)
    {
        ShopInfoReq tradeInfoReq = new ShopInfoReq()
        {
            ShopItemId = currentShopItemId,
            ShopNumber = shopNum
        };
        shopInfoRequest.SetShopInfoRequest(tradeInfoReq);
        shopInfoRequest.DefaultRequest();
    }

    public void OpenShopWindow()
    {
        GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Show);
        GameManager.Instance.SetGameMode(GameModeCode.ShopMode);
        shopWindow.SetWindowState(true);
        mainGameWindow.SetWindowState(false);
    }

    public void CloseShopWindow()
    {
        mainGameWindow.SetWindowState(true);
        shopWindow.SetWindowState(false);
        GameManager.Instance.SetGameMode(GameModeCode.GamePlayMode);
        GameManager.Instance.SetCursorShowType(CursorShowTypeCode.Hide);
    }

    public void OpenShopInfoConfirmWindow(string tradeItemId)
    {       
        currentShopItemId = tradeItemId;
        shopInfoConfirmWindow.SetShopItemId(tradeItemId);
        shopInfoConfirmWindow.SetWindowState(true);
    }

    public void CloseShopInfoConfirmWindow()
    {
        shopInfoConfirmWindow.SetWindowState(false);        
    }
}

using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class ShopInfoRequest : BaseRequest
{
    private ShopInfoReq shopInfoReq;

    public override void InitRequset()
    {        
        NetOpCode = OperationCode.Shop;
        base.InitRequset();
    }

    public override void DefaultRequest()
    {
        string shopInfoRequestJson = SetJsonString(shopInfoReq);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, shopInfoRequestJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {

    }

    public void SetShopInfoRequest(ShopInfoReq shopInfo)
    {
        shopInfoReq = shopInfo;
    }
}

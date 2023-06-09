using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: 

public class ShopInfoRequest : BaseRequest
{
    private ShopInfoReq shopInfoReq;

    public override void InitRequset()
    {
        base.InitRequset();
        OpCode = OperationCode.Shop;
    }

    public override void DefaultRequest()
    {
        string shopInfoRequestJson = SetJsonString(shopInfoReq);
        Dictionary<byte,object> dict = new Dictionary<byte,object>();
        dict.Add((byte)ParameterCode.ShopInfoReq, shopInfoRequestJson);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        
    }

    public void SetShopInfoRequest(ShopInfoReq shopInfo)
    {
        shopInfoReq = shopInfo;
    }
}

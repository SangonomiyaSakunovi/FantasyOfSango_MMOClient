using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

public class ItemEnhanceRequest : BaseRequest
{
    private ItemEnhanceReq itemEnhanceReq;

    public override void InitRequset()
    {      
        NetOpCode = OperationCode.ItemEnhance;
        base.InitRequset();
    }

    public void SetWeaponEnhanceReq(ItemEnhanceReq enhanceReq)
    {
        itemEnhanceReq = enhanceReq;
    }

    public override void DefaultRequest()
    {
        GameManager.Instance.SetGameMode(GameModeCode.WaitingServerResponseMode);
        string itemEnhanceRequstJson = SetJsonString(itemEnhanceReq);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, itemEnhanceRequstJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        string itemEnhanceResponseJson = sangoNetMessage.MessageBody.MessageString;
        if (itemEnhanceResponseJson != null)
        {
            ItemEnhanceRsp itemEnhanceRsp = DeJsonString<ItemEnhanceRsp>(itemEnhanceResponseJson);
            if (itemEnhanceRsp.IsEnhanceSuccess)
            {
                switch (itemEnhanceRsp.ItemTypeCode)
                {
                    case ItemTypeCode.Weapon:
                        WeaponsEnhanceSystem.Instance.OnWeaponEnhanceResponse(itemEnhanceRsp);
                        break;
                }
            }
            else
            {
                SangoRoot.AddMessage("ǿ��ʧ�ܣ�������", TextColorCode.RedColor);
            }
        }
        else
        {
            SangoRoot.AddMessage("����δ֪����������", TextColorCode.RedColor);
        }
        GameManager.Instance.SetGameMode(GameModeCode.ConfigureItemMode);
    }
}

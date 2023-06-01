using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Tools;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi
//Discription: The ItemEnhance Request.

public class ItemEnhanceRequest : BaseRequest
{
    private ItemEnhanceReq itemEnhanceReq;

    public override void InitRequset()
    {
        base.InitRequset();
    }

    public void SetWeaponEnhanceReq(ItemEnhanceReq enhanceReq)
    {
        itemEnhanceReq = enhanceReq;
    }

    public override void DefaultRequest()
    {
        GameManager.Instance.SetGameMode(GameModeCode.WaitingServerResponseMode);
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        string itemEnhanceRequstJson = SetJsonString(itemEnhanceReq);
        dict.Add((byte)ParameterCode.ItemEnhanceReq, itemEnhanceRequstJson);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        string itemEnhanceResponseJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.ItemEnhanceRsp);
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
                SangoRoot.AddMessage("强化失败，请重试", TextColorCode.RedColor);
            }
        }
        else
        {
            SangoRoot.AddMessage("出现未知错误，请重试", TextColorCode.RedColor);
        }
        GameManager.Instance.SetGameMode(GameModeCode.ConfigureItemMode);
    }
}

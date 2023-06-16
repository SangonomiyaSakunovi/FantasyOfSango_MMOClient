using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using SangoMMONetProtocol;
using System;
using UnityEngine;

//Developer : SangonomiyaSakunovi

public class AttackCommandRequest : BaseRequest
{
    private AttackCommand attackCommand;
    public override void InitRequset()
    {        
        NetOpCode = OperationCode.AttackCommand;
        base.InitRequset();
    }

    public void SetAttackCommand(SkillCode skillCode, Vector3 position, Quaternion rotation)
    {
        attackCommand = new AttackCommand
        {
            Account = OnlineAccountCache.Instance.LocalAccount,
            SkillCode = skillCode,
            Vector3Position = new Vector3Position
            {
                X = (float)Math.Round(position.x, 2),
                Y = (float)Math.Round(position.y, 2),
                Z = (float)Math.Round(position.z, 2)
            },
            QuaternionRotation = new QuaternionRotation
            {
                X = (float)Math.Round(rotation.x, 2),
                Y = (float)Math.Round(rotation.y, 2),
                Z = (float)Math.Round(rotation.z, 2),
                W = (float)Math.Round(rotation.w, 2)
            }
        };
    }

    public override void DefaultRequest()
    {
        string attackCommandJson = SetJsonString(attackCommand);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, attackCommandJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {

    }
}

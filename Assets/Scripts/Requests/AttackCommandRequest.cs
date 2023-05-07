using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using SangoCommon.Structs;

//Developer : SangonomiyaSakunovi
//Discription: The Attack Command Request.

public class AttackCommandRequest : BaseRequest
{
    public string Account { get; private set; }
    private AttackCommand attackCommand;
    public override void InitRequset()
    {
        base.InitRequset();
    }

    public void SetAttackCommand(SkillCode skillCode, Vector3 position, Quaternion rotation)
    {
        attackCommand = new AttackCommand
        {
            Account = this.Account,
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
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        dict.Add((byte)ParameterCode.AttackCommand, attackCommandJson);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }

    public void SetAccount(string account)
    {
        Account = account;
    }
}

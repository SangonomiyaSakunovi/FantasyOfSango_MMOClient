using ExitGames.Client.Photon;
using SangoCommon.Classs;
using SangoCommon.Enums;
using SangoCommon.Structs;
using SangoCommon.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription:

public class AttackDamageRequest : BaseRequest
{
    public string Account { get; private set; }
    public string DamagerAccount { get; private set; }
    private AttackDamage attackDamageCache;
    public override void InitRequset()
    {
        base.InitRequset();
    }

    public void SetAttackDamage(FightTypeCode fightType, string damager, SkillCode skillCode, ElementReactionCode elementReaction, Vector3 attakerPos, Vector3 damagerPos)
    {
        attackDamageCache = new AttackDamage
        {
            FightTypeCode = fightType,
            AttackerAccount = Account,
            DamagerAccount = damager,
            SkillCode = skillCode,
            ElementReactionCode = elementReaction,
            AttackerVector3Position = new Vector3Position
            {
                X = attakerPos.x,
                Y = attakerPos.y,
                Z = attakerPos.z
            },
            DamagerVector3Position = new Vector3Position
            {
                X = damagerPos.x,
                Y = damagerPos.y,
                Z = damagerPos.z
            },
            DateTime = DateTime.Now.ToUniversalTime()
        };
        DefaultRequest();
    }

    public override void DefaultRequest()
    {
        string attackDamageJson = SetJsonString(attackDamageCache);
        Dictionary<byte, object> dict = new Dictionary<byte, object>();
        dict.Add((byte)ParameterCode.AttackDamage, attackDamageJson);
        NetService.Peer.OpCustom((byte)OpCode, dict, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        string attackResultJson = DictTools.GetStringValue(operationResponse.Parameters, (byte)ParameterCode.AttackResult);
        if (attackResultJson != null)
        {
            AttackResult attackResultCache = DeJsonString<AttackResult>(attackResultJson);
            if (attackResultCache.DamageNumber > 0)
            {
                IslandOnlineAccountSystem.Instance.SetOnlineAvaterAttackResult(attackResultCache);
                SangoRoot.AddMessage("你攻击了玩家" + attackResultCache.DamagerAccount + "本次伤害为" + attackResultCache.DamageNumber + "HP");
            }
            else    //in this kind, the avater has been cured
            {
                IslandOnlineAccountSystem.Instance.SetOnlineAvaterAttackResult(attackResultCache);
                SangoRoot.AddMessage("你治疗了玩家" + attackResultCache.DamagerAccount + "本次治疗量为" + -attackResultCache.DamageNumber + "HP");
            }


        }
    }

    public void SetAccount(string account)
    {
        Account = account;
    }
}

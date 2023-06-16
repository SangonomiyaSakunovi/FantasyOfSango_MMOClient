using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using SangoMMONetProtocol;
using System;
using UnityEngine;

//Developer : SangonomiyaSakunovi

public class AttackDamageRequest : BaseRequest
{
    private AttackDamage attackDamage;

    public override void InitRequset()
    {       
        NetOpCode = OperationCode.AttackDamage;
        base.InitRequset();
    }

    public void SetAttackDamage(FightTypeCode fightType, string damager, SkillCode skillCode, ElementReactionCode elementReaction, Vector3 attakerPos, Vector3 damagerPos)
    {
        attackDamage = new AttackDamage
        {
            FightTypeCode = fightType,
            AttackerAccount = OnlineAccountCache.Instance.LocalAccount,
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
        string attackDamageJson = SetJsonString(attackDamage);
        netService.ClientInstance.ClientPeer.SendOperationRequest(NetOpCode, attackDamageJson);
    }

    public override void OnOperationResponse(SangoNetMessage sangoNetMessage)
    {
        string attackResultJson = sangoNetMessage.MessageBody.MessageString;
        if (attackResultJson != null)
        {
            AttackResult attackResult = DeJsonString<AttackResult>(attackResultJson);
            if (attackResult.DamageNumber > 0)
            {
                IslandOnlineAccountSystem.Instance.SetOnlineAvaterAttackResult(attackResult);
                SangoRoot.AddMessage("你攻击了玩家" + attackResult.DamagerAccount + "本次伤害为" + attackResult.DamageNumber + "HP", TextColorCode.OrangeColor);
            }
            else    //in this kind, the avater has been cured
            {
                IslandOnlineAccountSystem.Instance.SetOnlineAvaterAttackResult(attackResult);
                SangoRoot.AddMessage("你治疗了玩家" + attackResult.DamagerAccount + "本次治疗量为" + -attackResult.DamageNumber + "HP", TextColorCode.OrangeColor);
            }
        }
    }
}

using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using System;

//Developer: SangonomiyaSakunovi

[Serializable]
public class AttackCommand
{
    public string Account { get; set; }
    public SkillCode SkillCode { get; set; }
    public Vector3Position Vector3Position { get; set; }
    public QuaternionRotation QuaternionRotation { get; set; }
}

[Serializable]
public class AttackDamage
{
    public FightTypeCode FightTypeCode { get; set; }
    public string AttackerAccount { get; set; }
    public string DamagerAccount { get; set; }
    public SkillCode SkillCode { get; set; }
    public ElementReactionCode ElementReactionCode { get; set; }
    public Vector3Position AttackerVector3Position { get; set; }
    public Vector3Position DamagerVector3Position { get; set; }
    public DateTime DateTime { get; set; }
}

[Serializable]
public class AttackResult
{
    public string AttackerAccount { get; set; }
    public string DamagerAccount { get; set; }
    public int DamageNumber { get; set; }
    public AvaterInfo AttackerAvaterInfo { get; set; }
    public AvaterInfo DamagerAvaterInfo { get; set; }
}

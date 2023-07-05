using SangoMMOCommons.Enums;
using System;

//Developer: SangonomiyaSakunovi

[Serializable]
public class ChooseAvaterCode
{
    public string Account { get; set; }
    public AvaterCode AvaterCode { get; set; }
}

[Serializable]
public class AvaterAttributeInfo
{
    public AvaterCode Avater { get; set; }
    public int HP { get; set; }
    public int HPFull { get; set; }
    public int MP { get; set; }
    public int MPFull { get; set; }
    public int Attack { get; set; }
    public int Defence { get; set; }
    public ElementTypeCode ElementType { get; set; }
    public int ElementGauge { get; set; }
    public string WeaponId { get; set; }
    public int WeaponExp { get; set; }
}

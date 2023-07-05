using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using System;

//Developer: SangonomiyaSakunovi

[Serializable]
public class NPCGameObject
{
    public string _id { get; set; }
    public NPCCode NPCCode { get; set; }
    public Vector3Position Vector3Position { get; set; }
    public QuaternionRotation QuaternionRotation { get; set; }
    public NPCAttributeInfo NPCAttributeInfo { get; set; }
    public AOISceneGrid AOISceneGrid { get; set; }
}

[Serializable]
public class NPCAttributeInfo
{
    public int HP { get; set; }
    public int HPFull { get; set; }
    public int Attack { get; set; }
    public int Defence { get; set; }
    public ElementTypeCode ElementType { get; set; }
    public int ElementGauge { get; set; }
}

using System;
using System.Collections.Generic;

//Developer: SangonomiyaSakunovi

[Serializable]
public class PlayerData
{
    public AvaterInfo AvaterInfo { get; set; }
    public MissionInfo MissionInfo { get; set; }
    public ItemInfo ItemInfo { get; set; }
}

[Serializable]
public class UserInfo
{
    public string _id { get; set; }
    public string Account { get; set; }
    public string Password { get; set; }
    public string Nickname { get; set; }
}

[Serializable]
public class AvaterInfo
{
    public string _id { get; set; }
    public string Account { get; set; }
    public string Nickname { get; set; }
    public List<AvaterAttributeInfo> AttributeInfoList { get; set; }
}

[Serializable]
public class MissionInfo
{
    public string _id { get; set; }
    public string Account { get; set; }
    public List<string> MainMissionIdList { get; set; }
    public List<string> DailyMissionIdList { get; set; }
    public List<string> OptionalMissionIdList { get; set; }
}

[Serializable]
public class ItemInfo
{
    public string _id { get; set; }
    public string Account { get; set; }
    public int Coin { get; set; }
    public List<string> WeaponEnhanceMaterialList { get; set; }
}

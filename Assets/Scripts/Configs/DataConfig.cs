//Developer : SangonomiyaSakunovi
//Discription: The Configs.

using SangoCommon.Enums;
using UnityEngine;

public class DataConfig<T>
{
    public string _id;
}

public class MissionConfig : DataConfig<MissionConfig>
{
    public string missionName;
    public string npcID;
    public string dialogAvaterImageArray;
    public string dialogTextArray;
    public string dialogAudioArray;
    public string actionID;
    public string guidText;
}

public class IslandSceneConfig : DataConfig<IslandSceneConfig>
{
    public SceneCode sceneCode;
    public Vector3 mainCameraPosition;
    public Quaternion mainCameraRotation;
    public Vector3 localPlayerPosition;
    public Quaternion localPlayerRotation;
}

public class WeaponBreakConfig : DataConfig<WeaponBreakConfig>
{
    public int weaponBreakCoin;
    public string weaponBreakMaterial1;
    public string weaponBreakMaterial2;
}

public class WeaponInfoConfig : DataConfig<WeaponInfoConfig>
{
    public string weaponName;
    public string weaponDescribe;
}

public class WeaponValueConfig : DataConfig<WeaponValueConfig>
{
    public int weaponLevel;
    public int weaponBaseATK;
    public string weaponAbility1;
    public string weaponAbility2;
    public int weaponEnhanceLevelExp;
}

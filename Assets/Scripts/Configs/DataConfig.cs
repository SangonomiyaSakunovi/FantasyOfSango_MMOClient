using SangoMMOCommons.Enums;
using UnityEngine;

//Developer : SangonomiyaSakunovi

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
    public int coinRewards;
    public int worldExpRewards;
    public string material1Rewards;
    public string material2Rewards;
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

public class WeaponDetailsConfig : DataConfig<WeaponDetailsConfig>
{
    public string weaponName;
    public int weaponQuanlity;
    public string weaponDescribe;
}

public class WeaponValueConfig : DataConfig<WeaponValueConfig>
{
    public int weaponBaseATK;
    public string weaponAbility1;
    public string weaponAbility2;
    public int weaponEnhanceLevelExp;
    public int weaponAccumulateExp;
}

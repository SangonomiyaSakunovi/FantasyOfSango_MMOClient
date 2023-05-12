//Developer : SangonomiyaSakunovi
//Discription: The Configs.

using SangoCommon.Enums;
using UnityEngine;

public class DataConfig<T>
{
    public string _id;
}

public class IslandMissionConfig : DataConfig<IslandMissionConfig>
{
    public string npcID;
    public string dialogArray;
    public string dialogAudio;
    public string actionID;
}

public class IslandSceneConfig : DataConfig<IslandSceneConfig>
{
    public SceneCode sceneCode;
    public Vector3 mainCameraPosition;
    public Quaternion mainCameraRotation;
    public Vector3 localPlayerPosition;
    public Quaternion localPlayerRotation;
}

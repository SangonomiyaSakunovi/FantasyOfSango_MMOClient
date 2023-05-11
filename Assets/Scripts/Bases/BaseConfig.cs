using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The Configs.

public class BaseConfig<T>
{
    public string _id;
}

public class IslandMissionConfig : BaseConfig<IslandMissionConfig>
{
    public string npcID;
    public string dialogArray;
    public string dialogAudio;
    public string actionID;   
}

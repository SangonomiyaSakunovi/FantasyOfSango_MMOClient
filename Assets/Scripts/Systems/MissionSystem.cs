using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The MissionSystem, all the mission behaviour should define here.

public class MissionSystem : BaseSystem
{
    public static MissionSystem Instance = null;

    private MissionConfig missionConfig = null;

    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
    }

    public void RunMission(MissionConfig config)
    {
        if (config != null)
        {
            missionConfig = config;
        }
        if (missionConfig.npcID != "-1")
        {

        }
        else
        {
            MainGameSystem.Instance.OpenMissionWindow();
        }
    }
}

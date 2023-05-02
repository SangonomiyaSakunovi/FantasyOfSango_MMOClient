using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The TransitionSystem, all transition can define here.

public class FSMSystem
{
    private Dictionary<FSMStateCode, FSMState> fSMStateDict = new Dictionary<FSMStateCode, FSMState>();
    private FSMStateCode currentfSMStateCode;
    private FSMState currentfSMState;

    public void UpdateFSM(GameObject npc)
    {
        currentfSMState.Reason(npc);
        currentfSMState.Act(npc);        
    }

    public void AddFSMState(FSMState fSMState)
    {        
        if (currentfSMState == null)
        {
            currentfSMState = fSMState;
            currentfSMStateCode = fSMState.StateCode;
        }
        if (fSMStateDict.ContainsKey(fSMState.StateCode))
        {
            Debug.Log(fSMState.StateCode + " is already in Dict"); return;
        }
        fSMStateDict.Add(fSMState.StateCode, fSMState);
    }

    public void DeletFSMState(FSMStateCode fSMStateCode)
    {        
        if (!fSMStateDict.ContainsKey(fSMStateCode))
        {
            Debug.Log("There is no " + fSMStateCode + " in Dict"); return;
        }
        fSMStateDict.Remove(fSMStateCode);
    }

    public void OnFSMTransition(FSMTransitionCode fSMTransitionCode)
    {        
        FSMStateCode newFSMStateCode = currentfSMState.GetFSMStateCode(fSMTransitionCode);
        if (newFSMStateCode == FSMStateCode.Null)
        {
            Debug.Log("CurrentState is null"); return;
        }
        if (!fSMStateDict.ContainsKey(newFSMStateCode))
        {
            Debug.Log("There is no " + newFSMStateCode + " in Dict"); return;
        }
        currentfSMState.DoBeforeEntering();
        Debug.Log("判定规则是"+fSMTransitionCode);
        Debug.Log("新的状态是" + newFSMStateCode);
        currentfSMState = fSMStateDict[newFSMStateCode];
        currentfSMStateCode = newFSMStateCode; 
    }
}

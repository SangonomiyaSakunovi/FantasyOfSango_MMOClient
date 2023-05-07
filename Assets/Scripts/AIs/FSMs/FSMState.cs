using System.Collections.Generic;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: The BaseState, use to define what to do.

public abstract class FSMState
{
    protected FSMStateCode stateCode;
    public FSMStateCode StateCode { get { return stateCode; } }
    protected Dictionary<FSMTransitionCode, FSMStateCode> fSMMappingDict = new Dictionary<FSMTransitionCode, FSMStateCode>();

    protected FSMSystem fSMSystem;

    public FSMState(FSMSystem system)
    {
        this.fSMSystem = system;
    }

    public void AddFSMTransition(FSMTransitionCode fSMTransitionCode, FSMStateCode fSMStateCode)
    {        
        if (fSMMappingDict.ContainsKey(fSMTransitionCode))
        {
            Debug.Log(fSMTransitionCode + " is already in Dict"); return;
        }
        fSMMappingDict.Add(fSMTransitionCode, fSMStateCode);
    }

    public void DeletFSMTransition(FSMTransitionCode fSMTransitionCode)
    {        
        if (!fSMMappingDict.ContainsKey(fSMTransitionCode))
        {
            Debug.Log("There is no "+ fSMTransitionCode + " in Dict"); return;
        }
        fSMMappingDict.Remove(fSMTransitionCode);
    }

    public FSMStateCode GetFSMStateCode(FSMTransitionCode fSMTransitionCode)
    {
        if (fSMMappingDict.ContainsKey(fSMTransitionCode))
        {
            return fSMMappingDict[fSMTransitionCode];
        }
        else
        {
            return FSMStateCode.Null;
        }
    }

    public virtual void DoBeforeEntering() { }
    public virtual void DoAfterExiting() { }
    public abstract void Act(GameObject npc);
    public abstract void Reason(GameObject npc);
}

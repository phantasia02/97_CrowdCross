using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CMoveStateBase : CMovableStatePototype
{

    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eMove; }

    public CMoveStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
       
    }

    protected override void InState()
    {
  
    }

    protected override void updataState()
    {

    }

    protected override void OutState()
    {

    }

    public override void UpdateOriginalAnimation()
    {
      
    }
}

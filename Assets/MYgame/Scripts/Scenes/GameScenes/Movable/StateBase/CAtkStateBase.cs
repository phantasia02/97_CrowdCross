using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAtkStateBase : CMovableStatePototype
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eAtk; }

    public CAtkStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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
}

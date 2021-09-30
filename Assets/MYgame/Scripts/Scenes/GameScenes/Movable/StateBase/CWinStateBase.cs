using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWinStateBase : CMovableStatePototype
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eWin; }

    public CWinStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
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

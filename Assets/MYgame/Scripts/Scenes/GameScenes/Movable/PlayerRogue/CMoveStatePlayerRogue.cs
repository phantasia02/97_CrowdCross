using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStatePlayerRogue : CMoveStateBase
{
    CPlayerRogueMemoryShare m_MyPlayerRogueMemoryShare = null;

    public CMoveStatePlayerRogue(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueMemoryShare = (CPlayerRogueMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {

        base.InState();
        SetAnimationState(CAnimatorStateCtl.EState.eRun, Random.Range(0.8f, 1.2f));
    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {
        base.OutState();
    }
}

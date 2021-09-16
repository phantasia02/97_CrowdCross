using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStatePlayerRogueGroup : CWaitStateBase
{
    CPlayerRogueGroupMemoryShare m_MyPlayerRogueGroupMemoryShare = null;

    public CWaitStatePlayerRogueGroup(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueGroupMemoryShare = (CPlayerRogueGroupMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();

       // m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.SetMoveBuff(CMovableBase.ESpeedBuff.eDrag, 0.0f);
        m_MyPlayerRogueGroupMemoryShare.m_MySplineFollower.follow = false;
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.SetAllPlayerRogueState(StaticGlobalDel.EMovableState.eWait);
    }

    protected override void updataState()
    {
        base.updataState();
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void MouseDown()
    {
        m_MyPlayerRogueGroupMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eDrag;
    }
}

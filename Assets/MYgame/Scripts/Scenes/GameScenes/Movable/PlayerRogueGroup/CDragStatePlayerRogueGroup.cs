using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDragStatePlayerRogueGroup : CDragStateBase
{
    CPlayerRogueGroupMemoryShare m_MyPlayerRogueGroupMemoryShare = null;

    public CDragStatePlayerRogueGroup(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueGroupMemoryShare = (CPlayerRogueGroupMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.SetAllPlayerRogueState(StaticGlobalDel.EMovableState.eMove);
        m_MyPlayerRogueGroupMemoryShare.m_MySplineFollower.follow = true;
        m_MyPlayerRogueGroupMemoryShare.m_MySplineFollower.enabled = true;

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.ResetMoveBuff();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSpeed();
    }

    protected override void updataState()
    {
        base.updataState();

        //m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSplineFollowerOffset();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSpeed();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.updateFollwer();
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void MouseDrag()
    {
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.MouseDrag();
    }

    public override void MouseUp()
    {
        m_MyPlayerRogueGroupMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eWait;
    }
}

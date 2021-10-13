using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStatePlayerRogueGroup : CMoveStateBase
{
    CPlayerRogueGroupMemoryShare m_MyPlayerRogueGroupMemoryShare = null;

    public CMoveStatePlayerRogueGroup(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueGroupMemoryShare = (CPlayerRogueGroupMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {

        base.InState();


    }

    protected override void updataState()
    {
        base.updataState();

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSpeed();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateRearrangementTime();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSplineFollowerOffset();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.updateFollwer();
    }

    protected override void OutState()
    {
        base.OutState();
    }


}

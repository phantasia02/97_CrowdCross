using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathStatePlayerRogueGroup : CDeathStateBase
{
    CPlayerRogueGroupMemoryShare m_MyPlayerRogueGroupMemoryShare = null;

    public CDeathStatePlayerRogueGroup(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueGroupMemoryShare = (CPlayerRogueGroupMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();

        m_MyPlayerRogueGroupMemoryShare.m_MySplineFollower.follow = false;
        m_MyGameManager.SetState(CGameManager.EState.eGameOver);
        if (m_MyPlayerRogueGroupMemoryShare.m_MyHandTransform != null)
            m_MyPlayerRogueGroupMemoryShare.m_MyHandTransform.gameObject.SetActive(false);
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

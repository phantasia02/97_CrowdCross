using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathStatePlayerRogue : CDeathStateBase
{
    CPlayerRogueMemoryShare m_MyPlayerRogueMemoryShare = null;

    public CDeathStatePlayerRogue(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueMemoryShare = (CPlayerRogueMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        m_MyPlayerRogueMemoryShare.m_MyGroup.RemoveAllPlayerRogue(m_MyPlayerRogueMemoryShare.m_MyPlayerRogue);
        m_MyPlayerRogueMemoryShare.m_MyPlayerRogue.ShowMyCollision(false);
        SetAnimationState(CAnimatorStateCtl.EState.eDeath);
    }

    protected override void updataState()
    {
        base.updataState();

        if (MomentinTime(0.1f))
        {
            m_MyPlayerRogueMemoryShare.m_MyRigidbody.Sleep();
            m_MyPlayerRogueMemoryShare.m_MyGroup.AllPlayerRoguePool.RemoveObj(m_MyPlayerRogueMemoryShare.m_MyPlayerRogue);
        }
    }

    protected override void OutState()
    {
        base.OutState();
    }
}

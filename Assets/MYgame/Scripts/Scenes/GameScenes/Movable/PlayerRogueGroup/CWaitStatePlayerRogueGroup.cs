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

        //m_MyPlayerRogueGroupMemoryShare.m_AllPlayerRoguePool.AllCurObj[]

        base.InState();
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

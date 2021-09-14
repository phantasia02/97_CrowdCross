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

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.SetAllPlayerRogueState(StaticGlobalDel.EMovableState.eMove);
    }

    protected override void updataState()
    {
        base.updataState();

        m_MyPlayerRogueGroupMemoryShare.m_MyMovable.transform.Translate(0.0f, 0.0f, m_MyMemoryShare.m_TotleSpeed * Time.deltaTime);
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void MouseDown()
    {
    }
}

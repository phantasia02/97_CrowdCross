using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStateEnemy : CWaitStateBase
{
    CEnemyMemoryShare m_MyEnemyMemoryShare = null;

    public CWaitStateEnemy(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyEnemyMemoryShare = (CEnemyMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        SetAnimationState(CAnimatorStateCtl.EState.eIdle, Random.Range(0.8f, 1.2f));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaitStatePlayerRogue : CWaitStateBase
{
    CPlayerRogueMemoryShare m_MyPlayerRogueMemoryShare = null;

    public CWaitStatePlayerRogue(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueMemoryShare = (CPlayerRogueMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        SetAnimationState(CAnimatorStateCtl.EState.eIdle, Random.Range(0.8f, 1.2f));
    }

    protected override void updataState()
    {
        base.updataState();

        m_MyPlayerRogueMemoryShare.m_MyPlayerRogue.UpdataDir();
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagDoorGroup)
        {
            m_MyPlayerRogueMemoryShare.m_MyGroup.OnTriggerEnter(other);
        }
    }

    public override void OnCollisionEnter(Collision other)
    {
        m_MyPlayerRogueMemoryShare.m_MyCarCollisionPlayerRogue.CollisionEnter(other);

        base.OnCollisionEnter(other);
    }
}

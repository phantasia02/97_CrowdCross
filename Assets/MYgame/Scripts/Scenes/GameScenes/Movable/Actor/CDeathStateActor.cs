using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDeathStateActor : CMoveStateBase
{
    CActorMemoryShare m_MyActorMemoryShare = null;
    CActor m_TargetActorBuff = null;
    bool m_End = false;

    public CDeathStateActor(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyActorMemoryShare = (CActorMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        
        SetAnimationState(CAnimatorStateCtl.EState.eDeath);
        m_MyMemoryShare.m_MyMovable.AnimatorStateCtl.LockState = CAnimatorStateCtl.EState.eDeath;
        //m_TargetActorBuff = m_MyActorMemoryShare.m_Target;

        //for (int i = 0; i < m_MyActorMemoryShare.m_AllChildCollider.Length; i++)
        //{
        //    m_MyActorMemoryShare.m_AllChildCollider[i].gameObject.SetActive(false);
        //}

        m_MyGameManager.RemoveMemberGroup(m_MyActorMemoryShare.m_MyMovable.transform);

        m_MyActorMemoryShare.m_MyMovable.transform.forward = -m_MyActorMemoryShare.m_DeathImpactDir;
        m_MyActorMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        m_MyActorMemoryShare.m_MyRigidbody.drag = 1.1f;
        m_MyActorMemoryShare.m_MyRigidbody.mass = 1.0f;
        m_MyActorMemoryShare.m_MyRigidbody.useGravity = true;
        Vector3 lTempForce = m_MyActorMemoryShare.m_DeathImpactDir + Vector3.up * 2.0f;
        m_MyActorMemoryShare.m_MyRigidbody.AddForce(lTempForce * Random.Range(100.0f, 200.0f));
        //m_End = false;
        // Time.timeScale = 0.1f;
    }

    protected override void updataState()
    {
        base.updataState();

        if (MomentinTime(3.0f))
        {
            for (int i = 0; i < m_MyActorMemoryShare.m_AllChildCollider.Length; i++)
                m_MyActorMemoryShare.m_AllChildCollider[i].gameObject.SetActive(false);

            m_MyActorMemoryShare.m_MyRigidbody.useGravity = false;
            m_MyActorMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    protected override void OutState()
    {
        base.OutState();
    }
}

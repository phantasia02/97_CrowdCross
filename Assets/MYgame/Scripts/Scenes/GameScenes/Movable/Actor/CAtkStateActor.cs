using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAtkStateActor : CMoveStateBase
{
    CActorMemoryShare m_MyActorMemoryShare = null;
    CActor m_TargetActorBuff = null;
    bool m_End = false;

    public CAtkStateActor(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyActorMemoryShare = (CActorMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        SetAnimationState(CAnimatorStateCtl.EState.eAtk);
        m_TargetActorBuff = m_MyActorMemoryShare.m_Target;

        m_MyActorMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        m_MyActorMemoryShare.m_MyRigidbody.drag = 100.0f;
        m_End = false;
       // Time.timeScale = 0.1f;

        CAnimatorStateCtl lTempAnimatorStateCtl = m_MyActorMemoryShare.m_MyMovable.AnimatorStateCtl;
        lTempAnimatorStateCtl.m_KeyFramMessageCallBack = (CAnimatorStateCtl.cAnimationCallBackPar lTemp) => 
        {
            if (lTemp.iIndex == 0)
            {
                m_TargetActorBuff.ActorTypeDataHp = m_TargetActorBuff.ActorTypeDataHp - 1;
            }
            else if (lTemp.iIndex == 1)
            {
                SetAnimationState(CAnimatorStateCtl.EState.eIdle);
                m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eAtk;
                m_MyActorMemoryShare.m_MyMovable.SameStatusUpdate = true;
            }

        };
    }

    protected override void updataState()
    {
        base.updataState();



        Vector3 lTemp2DDis = m_TargetActorBuff.transform.position - m_MyActorMemoryShare.m_MyMovable.transform.position;
        lTemp2DDis.y = 0.0f;
        float lTempsqrDis = lTemp2DDis.sqrMagnitude;
        float lTempAtkDis = m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis * m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis;

        if (lTempsqrDis >= lTempAtkDis)
            m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eMove;
        else
        {
            //CAnimatorStateCtl lTempAnimatorStateCtl = m_MyActorMemoryShare.m_MyMovable.AnimatorStateCtl;
            //if (lTempAnimatorStateCtl.PlayingEnd && !m_End)
            //{
            //    //Debug.Log($" m_TargetActorBuff.ActorTypeDataHp = { m_TargetActorBuff.ActorTypeDataHp}");
            //    //m_TargetActorBuff.ActorTypeDataHp = m_TargetActorBuff.ActorTypeDataHp - 1;
            //    // SetAnimationState(CAnimatorStateCtl.EState.eAtk);
            //    // lTempAnimatorStateCtl.m_ThisAnimator.ResetTrigger();

            //    SetAnimationState(CAnimatorStateCtl.EState.eIdle);
            //    m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eAtk;
            //    m_MyActorMemoryShare.m_MyMovable.SameStatusUpdate = true;
            //    m_End = true;
            //}
        }

    }

    protected override void OutState()
    {
        base.OutState();
    }
}

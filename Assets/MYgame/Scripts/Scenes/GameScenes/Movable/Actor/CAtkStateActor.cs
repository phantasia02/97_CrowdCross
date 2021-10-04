using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAtkStateActor : CMoveStateBase
{
    CActorMemoryShare m_MyActorMemoryShare = null;

    public CAtkStateActor(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyActorMemoryShare = (CActorMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        SetAnimationState(CAnimatorStateCtl.EState.eAtk);
        m_MyActorMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        m_MyActorMemoryShare.m_MyRigidbody.drag = 100.0f;
       // Time.timeScale = 0.1f;

        CAnimatorStateCtl lTempAnimatorStateCtl = m_MyActorMemoryShare.m_MyMovable.AnimatorStateCtl;
        lTempAnimatorStateCtl.m_KeyFramMessageCallBack = (CAnimatorStateCtl.cAnimationCallBackPar lTemp) => 
        {
            if (lTemp.iIndex == 0 && m_MyActorMemoryShare.m_Target != null)
            {
                Vector3 lTempV3 = m_MyActorMemoryShare.m_Target.transform.position - m_MyActorMemoryShare.m_MyMovable.transform.position;
                lTempV3.y = 0.0f;
               // Vector3 lTempDis = lTempV3;
                float lTempAtkDis = m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis * m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis;
                if (lTempV3.sqrMagnitude <= lTempAtkDis)
                {
                    m_MyActorMemoryShare.m_Target.ActorTypeDataHp = m_MyActorMemoryShare.m_Target.ActorTypeDataHp - m_MyActorMemoryShare.m_ActorTypeData.m_AtkPower;

                    if (m_MyActorMemoryShare.m_Target.ActorTypeDataHp <= 0)
                    {
                        m_MyActorMemoryShare.m_Target.SetReadyDeath(lTempV3.normalized);
                        m_MyActorMemoryShare.m_Target = m_MyActorMemoryShare.m_Target = null;
                        //m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eWait;
                    }
                }
            }
            else if (lTemp.iIndex == 1)
            {
                if (m_MyActorMemoryShare.m_Target == null)
                {
                    CActor lTempActor = m_MyActorMemoryShare.m_MyActor.GetNextTarget();
                    if (lTempActor == null)
                    {
                        m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eWait;
                        return;
                    }
                    else
                        m_MyActorMemoryShare.m_Target = lTempActor;
                }

                AnimationEndToState();
            }

        };
    }

    protected override void updataState()
    {
        base.updataState();

        //Vector3 lTemp2DDis = m_TargetActorBuff.transform.position - m_MyActorMemoryShare.m_MyMovable.transform.position;
        //lTemp2DDis.y = 0.0f;
        //float lTempsqrDis = lTemp2DDis.sqrMagnitude;
        //float lTempAtkDis = m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis * m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis;

        //if (lTempsqrDis >= lTempAtkDis)
        //    m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eMove;
        //else
        //{
        //    //CAnimatorStateCtl lTempAnimatorStateCtl = m_MyActorMemoryShare.m_MyMovable.AnimatorStateCtl;
        //    //if (lTempAnimatorStateCtl.PlayingEnd && !m_End)
        //    //{
        //    //    //Debug.Log($" m_TargetActorBuff.ActorTypeDataHp = { m_TargetActorBuff.ActorTypeDataHp}");
        //    //    //m_TargetActorBuff.ActorTypeDataHp = m_TargetActorBuff.ActorTypeDataHp - 1;
        //    //    // SetAnimationState(CAnimatorStateCtl.EState.eAtk);
        //    //    // lTempAnimatorStateCtl.m_ThisAnimator.ResetTrigger();

        //    //    SetAnimationState(CAnimatorStateCtl.EState.eIdle);
        //    //    m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eAtk;
        //    //    m_MyActorMemoryShare.m_MyMovable.SameStatusUpdate = true;
        //    //    m_End = true;
        //    //}
        //}

    }

    protected override void OutState()
    {
        base.OutState();
    }

    public void AnimationEndToState()
    {
        Vector3 lTempV3 = m_MyActorMemoryShare.m_Target.transform.position - m_MyActorMemoryShare.m_MyMovable.transform.position;
        lTempV3.y = 0.0f;
        float lTempAtkDis = m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis * m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis;
        if (lTempV3.sqrMagnitude <= lTempAtkDis)
        {
            SetAnimationState(CAnimatorStateCtl.EState.eIdle);
            m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eAtk;
            m_MyActorMemoryShare.m_MyMovable.SameStatusUpdate = true;
        }
        else
            m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eMove;
    }
}

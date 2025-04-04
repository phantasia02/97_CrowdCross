using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTargetMoveStateActor : CMoveStateBase
{
    CActorMemoryShare m_MyActorMemoryShare = null;

    public CTargetMoveStateActor(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyActorMemoryShare = (CActorMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();

        m_MyActorMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        m_MyActorMemoryShare.m_MyRigidbody.drag = 10.0f;
        m_MyActorMemoryShare.m_MyRigidbody.mass = 1.0f;
        //m_MyActorMemoryShare.m_MyMovable.gameObject.layer = (int)StaticGlobalDel.ELayerIndex.eEndPlayerActor;
        SetAnimationState(CAnimatorStateCtl.EState.eRun, Random.Range(0.8f, 1.2f));
    }

    protected override void updataState()
    {
        base.updataState();

        if (m_MyActorMemoryShare.m_Target == null)
            return;
        else if (m_MyActorMemoryShare.m_Target.CurState == StaticGlobalDel.EMovableState.eDeath)
        {
            m_MyActorMemoryShare.m_Target = m_MyActorMemoryShare.m_MyActor.GetNextTarget();

            if (m_MyActorMemoryShare.m_Target == null)
            {
                m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eWin;
                return;
            }
        }


        Vector3 lTempMyforward = m_MyActorMemoryShare.m_MyMovable.transform.forward;
        lTempMyforward.y = 0.0f;
        Vector3 lTemp2DDis = m_MyActorMemoryShare.m_Target.transform.position - m_MyActorMemoryShare.m_MyMovable.transform.position;
        lTemp2DDis.y = 0.0f;
        Vector3 lTemp2DDir = lTemp2DDis;
        lTemp2DDir.Normalize();
        float lTempsqrDis = lTemp2DDis.sqrMagnitude;
        float lTempAtkDis = m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis * m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis;
        //if (lTempsqrDis >= lTempAtkDis || (Vector3.Dot(lTemp2DDir, lTempMyforward) < 0.95f))
        //{
            if (Vector3.Dot(lTemp2DDir, lTempMyforward) < 0.95f)
                m_MyActorMemoryShare.m_MyMovable.transform.forward = Vector3.Lerp(lTempMyforward, lTemp2DDir, Time.deltaTime * 10.0f);
            else
                m_MyActorMemoryShare.m_MyMovable.transform.forward = lTemp2DDir;

           // if (lTempsqrDis >= lTempAtkDis)
                m_MyActorMemoryShare.m_MyMovable.transform.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * m_MyActorMemoryShare.m_ActorTypeData.m_Speed));
        //}
        //else
        //{
        //    m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eAtk;
        //}
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == m_MyActorMemoryShare.m_MyActor.TargetIndex())
        {
            CActor m_lBuffTarget = other.gameObject.GetComponentInParent<CActor>();

            if (!(m_lBuffTarget.CurState == StaticGlobalDel.EMovableState.eDeath || m_lBuffTarget.ChangState == StaticGlobalDel.EMovableState.eDeath))
            {
                Vector3 lTempV3 = m_lBuffTarget.transform.position - m_MyActorMemoryShare.m_MyMovable.transform.position;
                lTempV3.y = 0.0f;
                lTempV3.Normalize();


                m_lBuffTarget.SetReadyDeath(lTempV3);
                m_lBuffTarget.LockChangState = StaticGlobalDel.EMovableState.eDeath;
                m_lBuffTarget.ChangState = StaticGlobalDel.EMovableState.eDeath;

                m_MyActorMemoryShare.m_MyActor.SetReadyDeath(-lTempV3);
                m_MyActorMemoryShare.m_MyActor.LockChangState = StaticGlobalDel.EMovableState.eDeath;
                m_MyActorMemoryShare.m_MyActor.ChangState = StaticGlobalDel.EMovableState.eDeath;
            }
            //if (m_lBuffTarget != m_MyActorMemoryShare.m_Target)
            //{
            //    if (m_MyActorMemoryShare.m_Target == null)
            //        m_MyActorMemoryShare.m_Target = m_lBuffTarget;
            //    else
            //    {
            //        Vector3 lTemp2DDis = m_MyActorMemoryShare.m_Target.transform.position - m_MyActorMemoryShare.m_MyMovable.transform.position;
            //        lTemp2DDis.y = 0.0f;
            //        float lTempAtkDis = m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis * m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis;

            //        if (lTemp2DDis.sqrMagnitude >= lTempAtkDis)
            //            m_MyActorMemoryShare.m_Target = m_lBuffTarget;
            //    }
            //}
        }

        base.OnCollisionEnter(other);
    }
}

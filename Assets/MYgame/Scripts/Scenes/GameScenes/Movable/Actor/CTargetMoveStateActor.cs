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

        m_MyActorMemoryShare.m_MyMovable.gameObject.layer = (int)StaticGlobalDel.ELayerIndex.eEndActor;
        SetAnimationState(CAnimatorStateCtl.EState.eRun, Random.Range(0.8f, 1.2f));
    }

    protected override void updataState()
    {
        base.updataState();

        if (m_MyActorMemoryShare.m_Target == null)
            return;

        Vector3 lTemp2DDis = m_MyActorMemoryShare.m_Target.transform.position - m_MyActorMemoryShare.m_MyMovable.transform.position;
        lTemp2DDis.y = 0.0f;
        Vector3 lTemp2DDir = lTemp2DDis;
        float lTempsqrDis = lTemp2DDis.sqrMagnitude;
        float lTempAtkDis = m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis * m_MyActorMemoryShare.m_ActorTypeData.m_AtkDis;
        if (lTempsqrDis >= lTempAtkDis)
        {
            lTemp2DDir.Normalize();

            Vector3 lTempMyforward = m_MyActorMemoryShare.m_MyMovable.transform.forward;
            lTempMyforward.y = 0.0f;
            m_MyActorMemoryShare.m_MyMovable.transform.forward = Vector3.Lerp(lTempMyforward, lTemp2DDir, Time.deltaTime * 10.0f);
            m_MyActorMemoryShare.m_MyMovable.transform.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * m_MyActorMemoryShare.m_ActorTypeData.m_Speed));
        }
        else
        {
            m_MyActorMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eAtk;
        }
    }

    protected override void OutState()
    {
        base.OutState();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CMoveStatePlayerRogue : CMoveStateBase
{
    CPlayerRogueMemoryShare m_MyPlayerRogueMemoryShare = null;

    public CMoveStatePlayerRogue(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueMemoryShare = (CPlayerRogueMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        SetAnimationState(CAnimatorStateCtl.EState.eRun, Random.Range(0.8f, 1.2f));

        if ((int)m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK >= (int)StaticGlobalDel.EBoolState.eFlase)
        {
            Sequence lTempSequence = DOTween.Sequence();
            lTempSequence.Append(m_MyPlayerRogueMemoryShare.m_MyMovable.transform.DOLocalMove
                (m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition, 0.2f).SetEase(Ease.Linear));

            lTempSequence.AppendCallback(() => 
            {
                m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eTrue;
                m_MyPlayerRogueMemoryShare.m_MyMovable.ChangState = m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState;
                m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState = StaticGlobalDel.EMovableState.eMax;
            });
        }
    }

    protected override void updataState()
    {
        base.updataState();
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

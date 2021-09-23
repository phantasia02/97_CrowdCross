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


        //if ((int)m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK >= (int)StaticGlobalDel.EBoolState.eFlase)
        //{
        //    Sequence lTempSequence = DOTween.Sequence();
        //    lTempSequence.Append(m_MyPlayerRogueMemoryShare.m_MyMovable.transform.DOLocalMove
        //        (m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition, 0.4f).SetEase(Ease.Linear));
        //    lTempSequence.AppendCallback(() => 
        //    {
        //       // Debug.Log($"m_MyPlayerRogueMemoryShare.m_MyMovable.ChangState = {m_MyPlayerRogueMemoryShare.m_MyMovable.ChangState}");
        //        m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eTrue;
        //        m_MyPlayerRogueMemoryShare.m_MyMovable.ChangState = m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState;
        //        m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState = StaticGlobalDel.EMovableState.eMax;
        //    });
        //}
    }

    protected override void updataState()
    {
        base.updataState();

        if ((int)m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK < (int)StaticGlobalDel.EBoolState.eFlase)
            return;

        float lTempMoveDis = Time.deltaTime * 10.0f;
        Vector3 lTempV3 = Vector3.MoveTowards(m_MyPlayerRogueMemoryShare.m_MyMovable.transform.localPosition, m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition, lTempMoveDis);
        Vector3 lTempV3Sqr = m_MyPlayerRogueMemoryShare.m_MyMovable.transform.localPosition - m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition;
        lTempV3Sqr.y = 0.0f;
       // Debug.Log($"lTempV3Sqr.sqrMagnitude = {lTempV3Sqr.sqrMagnitude}  --- lTempMoveDis * lTempMoveDis = {lTempMoveDis * lTempMoveDis}");
        if (lTempV3Sqr.sqrMagnitude > (lTempMoveDis * lTempMoveDis))
            m_MyPlayerRogueMemoryShare.m_MyMovable.transform.localPosition = lTempV3;
        else
        {
            m_MyPlayerRogueMemoryShare.m_MyMovable.transform.localPosition = m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition;
            m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eTrue;

            //if (m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState != StaticGlobalDel.EMovableState.eMax && m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState != StaticGlobalDel.EMovableState.eNull)
        
            //StaticGlobalDel.EMovableState lTempstate = StaticGlobalDel.EMovableState.eWait;
            //if (m_MyPlayerRogueMemoryShare.m_MyGroup.CurState == StaticGlobalDel.EMovableState.eMove)
            //    lTempstate = StaticGlobalDel.EMovableState.eMove;
            //else if (m_MyPlayerRogueMemoryShare.m_MyGroup.CurState == StaticGlobalDel.EMovableState.eWait)
            //    lTempstate = StaticGlobalDel.EMovableState.eWait;

            m_MyPlayerRogueMemoryShare.m_MyMovable.ChangState = m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState;

            m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState = StaticGlobalDel.EMovableState.eMax;
        }
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

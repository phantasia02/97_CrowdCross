using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CWinStateActor : CMoveStateBase
{
    CActorMemoryShare m_MyActorMemoryShare = null;

    public CWinStateActor(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyActorMemoryShare = (CActorMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();
        SetAnimationState(CAnimatorStateCtl.EState.eWin, Random.Range(0.8f, 1.2f));

        Tween lTempTween = m_MyActorMemoryShare.m_MyMovable.transform.DOLocalRotate(new Vector3(0.0f, 360.0f, 0.0f), Random.Range(5.0f, 6.2f), RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
        lTempTween.SetLoops(-1, LoopType.Incremental);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CResultStatePlayerRogueGroup : CWinStateBase
{
    CPlayerRogueGroupMemoryShare m_MyPlayerRogueGroupMemoryShare = null;

    public CResultStatePlayerRogueGroup(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueGroupMemoryShare = (CPlayerRogueGroupMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();

        m_MyPlayerRogueGroupMemoryShare.m_MySplineFollower.enabled = false;
        m_MyGameManager.PlayerResultCamera.SetActive(true);
        //m_MyPlayerRogueGroupMemoryShare.m_PlayerRoguePoolParent.gameObject.SetActive(false);
        CEnemyGroup lTempEnemyGroup = m_MyGameManager.EnemyGroup;

        lTempEnemyGroup.gameObject.SetActive(true);
        Transform lTempResulPos = m_MyGameManager.ResulPos;

        Sequence lTempSequence = DOTween.Sequence();
        lTempSequence.Append(m_MyPlayerRogueGroupMemoryShare.m_MyMovable.transform.DOMove(lTempResulPos.position, 5.0f).SetEase(Ease.Linear));
        lTempSequence.AppendCallback(() =>
        {


            m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateTarget();
        });
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

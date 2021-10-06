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
        m_MyPlayerRogueGroupMemoryShare.m_MyCanvas.gameObject.SetActive(false);
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.SetPlayerTargetDir(m_MyPlayerRogueGroupMemoryShare.m_MyMovable.transform.forward);

        CEnemyGroup lTempEnemyGroup = m_MyGameManager.EnemyGroup;
        lTempEnemyGroup.gameObject.SetActive(true);
        Transform lTempResulPos = m_MyGameManager.ResulPos;

        for (int i = 0; i < m_MyPlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count; i++)
            m_MyPlayerRogueGroupMemoryShare.m_AllPlayerRogueObj[i].ChangState = StaticGlobalDel.EMovableState.eMove;


        Sequence lTempSequence = DOTween.Sequence();
        lTempSequence.AppendInterval(1.5f);
        lTempSequence.Join(m_MyPlayerRogueGroupMemoryShare.m_MyMovable.transform.DOMove(lTempResulPos.position, 1.5f).SetEase(Ease.Linear));
        lTempSequence.AppendCallback(() =>
        {
            m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.AddTargetGroup();
        });
        lTempSequence.AppendInterval(0.5f);
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

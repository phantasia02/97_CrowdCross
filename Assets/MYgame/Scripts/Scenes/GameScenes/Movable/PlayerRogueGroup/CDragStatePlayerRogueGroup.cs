using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDragStatePlayerRogueGroup : CDragStateBase
{
    CPlayerRogueGroupMemoryShare m_MyPlayerRogueGroupMemoryShare = null;

    public CDragStatePlayerRogueGroup(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerRogueGroupMemoryShare = (CPlayerRogueGroupMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
        base.InState();

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.SetAllPlayerRogueState(StaticGlobalDel.EMovableState.eMove);
        m_MyPlayerRogueGroupMemoryShare.m_MySplineFollower.follow = true;
        m_MyPlayerRogueGroupMemoryShare.m_MySplineFollower.enabled = true;

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.ResetMoveBuff();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSpeed();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.updateFollwer();
    }

    protected override void updataState()
    {
        base.updataState();

        //m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSplineFollowerOffset();
    
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSpeed();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateRearrangementTime();
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.UpdateSplineFollowerOffset();

        Vector2 lTempDirV2 = m_MyPlayerRogueGroupMemoryShare.m_TargetOffset - m_MyPlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset;
        Vector3 lTempDirV3 = Vector3.zero;
        lTempDirV3.x = lTempDirV2.x;
        lTempDirV3.z = lTempDirV2.y;
        lTempDirV3 = lTempDirV3.normalized + (m_MyPlayerRogueGroupMemoryShare.m_MyMovable.transform.forward * 0.5f);

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.SetPlayerTargetDir(lTempDirV3.normalized);

        
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.updateFollwer();
    }

    protected override void OutState()
    {
        base.OutState();
    }

    public override void MouseDrag()
    {
        m_MyPlayerRogueGroupMemoryShare.m_PlayerRogueGroup.MouseDrag();
    }

    public override void MouseUp()
    {
        m_MyPlayerRogueGroupMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eWait;
    }
}

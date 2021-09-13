using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerRogueGroupMemoryShare : CMemoryShareBase
{
    public CPlayerRogueGroup    m_PlayerRogueGroup = null;

};

public class CPlayerRogueGroup : CMovableBase
{
    protected CPlayerRogueGroupMemoryShare m_PlayerRogueGroupMemoryShare = null;

    public override EMovableType MyMovableType() { return EMovableType.ePlayerRogueGroup; }



    //protected override void AddInitState()
    //{
    //}


    protected override void CreateMemoryShare()
    {
        m_PlayerRogueGroupMemoryShare = new CPlayerRogueGroupMemoryShare();
        m_MyMemoryShare = m_PlayerRogueGroupMemoryShare;
        m_PlayerRogueGroupMemoryShare.m_PlayerRogueGroup = this;


        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetCurState(StaticGlobalDel.EMovableState.eWait);
    }
}

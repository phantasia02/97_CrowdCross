using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CPlayerRogueMemoryShare : CMemoryShareBase
{
    public CPlayerRogue         m_MyPlayerRogue = null;
    public CPlayerRogueGroup    m_MyGroup       = null;
    public Transform            m_TargetDummy   = null;
    public int                  m_GroupIndex    = -1;
};

public class CPlayerRogue : CActor
{
    public class CSetParentData
    {
        public CPlayerRogueGroup Group = null;
    }

    public override EMovableType MyMovableType() { return EMovableType.ePlayerRogue; }

    public int CurGroupIndex { set { m_MyPlayerRogueMemoryShare.m_GroupIndex = value; } }
    protected CPlayerRogueMemoryShare m_MyPlayerRogueMemoryShare = null;

    public void SetParentData(ref CSetParentData data)
    {
        m_MyPlayerRogueMemoryShare.m_MyGroup = data.Group;

        Transform lTempAllTargetDummyTransform = m_MyPlayerRogueMemoryShare.m_MyGroup.AllTargetDummyTransform;
        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;
        GameObject lTempObj = Instantiate(lTempCGGameSceneData.m_AllOtherObj[(int)CGGameSceneData.EOtherObj.eDummyObj], lTempAllTargetDummyTransform.position, lTempAllTargetDummyTransform.rotation, lTempAllTargetDummyTransform);
        m_MyPlayerRogueMemoryShare.m_TargetDummy = lTempObj.transform;
    }

    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStatePlayerRogue(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayerRogue(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyPlayerRogueMemoryShare = new CPlayerRogueMemoryShare();
        m_MyMemoryShare = m_MyPlayerRogueMemoryShare;
        m_MyPlayerRogueMemoryShare.m_MyPlayerRogue = this;


       // m_MyPlayerRogueMemoryShare.m_TargetDummy = this;

        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
       // SetCurState(StaticGlobalDel.EMovableState.eWait);
    }

    public void SetTargetPos(Vector3 pos, bool updatapos = false)
    {
        m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.position = pos;

        if (updatapos)
            this.transform.position = m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.position;
    }

    public void MyAddList(int listindex)
    {
        CurGroupIndex = listindex;
        this.gameObject.SetActive(true);
    }

    public void MyRemove()
    {
        CurGroupIndex = -1;
        this.gameObject.SetActive(false);
    }


}

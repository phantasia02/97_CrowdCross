using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CPlayerRogueMemoryShare : CMemoryShareBase
{
    public CPlayerRogue                     m_MyPlayerRogue = null;
    public CPlayerRogueGroup                m_MyGroup       = null;
    public Transform                        m_TargetDummy   = null;
    public int                              m_GroupIndex    = -1;
    public StaticGlobalDel.EBoolState       m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eFlase;
    public StaticGlobalDel.EMovableState    m_MoveTargetBuffCurState =  StaticGlobalDel.EMovableState.eMax;
    public CCarCollisionPlayerRogue         m_MyCarCollisionPlayerRogue =  null;
};

public class CPlayerRogue : CActor
{
    public class CSetParentData
    {
        public CPlayerRogueGroup Group = null;
    }

    public override EMovableType MyMovableType() { return EMovableType.ePlayerRogue; }

    public override StaticGlobalDel.EMovableState ChangState
    {
        set
        {
            if ((int)m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK >= (int)StaticGlobalDel.EBoolState.eFlase)
            {
                if (m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK == StaticGlobalDel.EBoolState.eFlase)
                {
                    m_ChangState = StaticGlobalDel.EMovableState.eMove;
                    SameStatusUpdate = true;
                    m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eFlasePlaying;
                }

                m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState = value;
                return;
            }

            m_ChangState = value;
        }
        get { return m_ChangState; }
    }

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
        m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayerRogue(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyPlayerRogueMemoryShare = new CPlayerRogueMemoryShare();
        m_MyMemoryShare = m_MyPlayerRogueMemoryShare;
        m_MyPlayerRogueMemoryShare.m_MyPlayerRogue = this;

        m_MyPlayerRogueMemoryShare.m_MyCarCollisionPlayerRogue = new CCarCollisionPlayerRogue(m_MyPlayerRogueMemoryShare);
       // m_MyPlayerRogueMemoryShare.m_TargetDummy = this;

        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
       // SetCurState(StaticGlobalDel.EMovableState.eWait);
    }


    public void SetTargetPos(Vector3 Localpos, bool updatapos = false)
    {
        m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition = Localpos;

        if (updatapos)
        {
            this.transform.localPosition = m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition;
            m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eTrue;
        }
        else
        {
            this.transform.localPosition = Vector3.zero;
            m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eFlase;
            ChangState = StaticGlobalDel.EMovableState.eMove;
        }
    }

    public void MyAddList(int listindex)
    {
        CurGroupIndex = listindex;
        this.gameObject.SetActive(true);
    }

    public void MyRemove()
    {
        CurGroupIndex = -1;
        SetCurState(StaticGlobalDel.EMovableState.eDeath);
        this.gameObject.SetActive(false);
    }

    //public override void OnTriggerEnter(Collider other)
    //{


    //    base.OnTriggerEnter(other);
    //}

    public override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == (int)StaticGlobalDel.ELayerIndex.eCarCollider)
        {
            m_MyPlayerRogueMemoryShare.m_MyRigidbody.AddForceAtPosition(other.contacts[0].normal * Random.Range(20.0f, 30.0f), other.contacts[0].point, ForceMode.VelocityChange);
            //other.contacts[0].normal
            // other.contacts[0].point
            // m_MyPlayerRogueMemoryShare.m_MyGroup.OnTriggerEnter(other);
        }

        base.OnCollisionEnter(other);
    }

}

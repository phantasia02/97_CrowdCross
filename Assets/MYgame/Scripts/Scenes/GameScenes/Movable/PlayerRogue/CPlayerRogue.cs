using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CPlayerRogueMemoryShare : CActorMemoryShare
{
    public CPlayerRogue                     m_MyPlayerRogue             = null;
    public CPlayerRogueGroup                m_MyGroup                   = null;
    public Transform                        m_TargetDummy               = null;
    public float                            m_CurRingDis                = 0.0f;
    public int                              m_GroupIndex                = -1;
    public StaticGlobalDel.EBoolState       m_MoveTargetDummyOK         = StaticGlobalDel.EBoolState.eFlase;
    public StaticGlobalDel.EMovableState    m_MoveTargetBuffCurState    = StaticGlobalDel.EMovableState.eMax;
    public CCarCollisionPlayerRogue         m_MyCarCollisionPlayerRogue = null;
    public Transform                        m_HandTransform             = null;
    public GameObject                       m_MyArms                    = null;
    public CGGameSceneData.EArmsType        m_MyArmsType                = CGGameSceneData.EArmsType.eMax;
    public Vector3                          m_TargetDir                 = Vector3.forward;
   // public CEnemy                           m_TargetEnemy               = null;
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
            if (m_MyPlayerRogueMemoryShare.m_MyMovable.LockChangState == value)
            {
                base.ChangState = value;
                return;
            }

            if (LockChangState != StaticGlobalDel.EMovableState.eMax)
                return;


            if (LockChangState == StaticGlobalDel.EMovableState.eMax && (int)m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK >= (int)StaticGlobalDel.EBoolState.eFlase)
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

            base.ChangState = value;
        }
        get { return m_ChangState; }
    }

    // ==================== SerializeField ===========================================


    [SerializeField] protected Transform m_HandTransform = null;
    // ==================== SerializeField ===========================================
    public Vector3 TargetDir
    {
        set {m_MyPlayerRogueMemoryShare.m_TargetDir = value; }
        get { return m_MyPlayerRogueMemoryShare.m_TargetDir; }
    }
    

    public int CurGroupIndex { set { m_MyPlayerRogueMemoryShare.m_GroupIndex = value; } }
    public float CurRingDis { get { return m_MyPlayerRogueMemoryShare.m_CurRingDis; } }
    public Transform CurTargetDummy { get { return m_MyPlayerRogueMemoryShare.m_TargetDummy; } }
    public StaticGlobalDel.EBoolState MoveTargetDummyOK { set { m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK =  StaticGlobalDel.EBoolState.eTrue; } }

    protected CPlayerRogueMemoryShare m_MyPlayerRogueMemoryShare = null;
    public override int TargetMask() { return (int)StaticGlobalDel.g_EndEnemyActorMask; }
    public override int TargetIndex() { return (int)StaticGlobalDel.ELayerIndex.eEndEnemyActor; }

    float m_updateDirval = 1.0f;

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
        m_AllState[(int)StaticGlobalDel.EMovableState.eAtk].AllThisState.Add(new CAtkStateActor(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinStateActor(this));

        m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayerRogue(this));       // eDeath index 0
        m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStateActor(this));             // eDeath index 1

        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayerRogue(this));         // eMove index 0
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CTargetMoveStateActor(this));         // eMove index 1
    }

    protected override void CreateMemoryShare()
    {
        m_MyPlayerRogueMemoryShare = new CPlayerRogueMemoryShare();
        m_MyMemoryShare = m_MyActorMemoryShare = m_MyPlayerRogueMemoryShare;
        m_MyActorMemoryShare.m_MyActor = m_MyPlayerRogueMemoryShare.m_MyPlayerRogue = this;

        m_MyPlayerRogueMemoryShare.m_MyCarCollisionPlayerRogue = new CCarCollisionPlayerRogue(m_MyPlayerRogueMemoryShare);
        m_MyPlayerRogueMemoryShare.m_HandTransform = m_HandTransform;
        // m_MyPlayerRogueMemoryShare.m_TargetDummy = this;
        m_updateDirval = Random.Range(10.0f, 15.0f);
        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
       // SetCurState(StaticGlobalDel.EMovableState.eWait);
    }


    public void SetTargetPos(CTargetPositionData LocalposData, bool updatapos = false)
    {
        if (m_MyPlayerRogueMemoryShare.m_GroupIndex == -1)
            return;

        m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition = LocalposData.m_TargetPosition;
        m_MyPlayerRogueMemoryShare.m_CurRingDis = LocalposData.m_RingDis;

        if (updatapos)
        {
            this.transform.localPosition = m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition;
            m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eTrue;
            ChangState = StaticGlobalDel.EMovableState.eWait;
        }
        else
        {
            this.transform.localPosition = Vector3.zero;
            m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eFlase;
            SameStatusUpdate = true;
            ChangState = StaticGlobalDel.EMovableState.eMove;
        }
    }
   
    public void SetTargetupdatePos(CTargetPositionData LocalposData)
    {
        if (m_MyPlayerRogueMemoryShare.m_GroupIndex == -1)
            return;

        Vector3 lTempV3 = Vector3.zero;

        lTempV3 = CurTargetDummy.localPosition - LocalposData.m_TargetPosition;
        lTempV3.y = 0.0f;

        if (lTempV3.sqrMagnitude < 0.1f)
            return;

        m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition = LocalposData.m_TargetPosition;
        m_MyPlayerRogueMemoryShare.m_CurRingDis = LocalposData.m_RingDis;
        lTempV3 = this.transform.localPosition - m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition;
        float SqrDis = lTempV3.sqrMagnitude;

        if (lTempV3.sqrMagnitude > 0.1f)
        {
            StaticGlobalDel.EBoolState lOldeBoolState = m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK;
            m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eFlase;
            SameStatusUpdate = true;
           // SetCurState(StaticGlobalDel.EMovableState.eMove);
            ChangState = lOldeBoolState == StaticGlobalDel.EBoolState.eFlasePlaying ? StaticGlobalDel.EMovableState.eMove : CurState;
            //m_MyPlayerRogueMemoryShare.m_MoveTargetBuffCurState = StaticGlobalDel.EMovableState.eWait;
        }
        else
        {
            this.transform.localPosition = m_MyPlayerRogueMemoryShare.m_TargetDummy.transform.localPosition;
            m_MyPlayerRogueMemoryShare.m_MoveTargetDummyOK = StaticGlobalDel.EBoolState.eTrue;
        }
    }

    public void ShowAdd(bool Show)
    {
        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;
        if (Show)
        {
            m_MyPlayerRogueMemoryShare.m_MyPlayerRogue.ShowMyCollision(true);
            m_MyPlayerRogueMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            m_MyPlayerRogueMemoryShare.m_MyMovable.LockChangState = StaticGlobalDel.EMovableState.eMax;

            //m_MyPlayerRogueMemoryShare.m_MyArmsType = (CGGameSceneData.EArmsType)Random.Range(0, (int)CGGameSceneData.EArmsType.eMax);
            //m_MyPlayerRogueMemoryShare.m_MyArms = lTempCGGameSceneData.GetArms(m_MyPlayerRogueMemoryShare.m_MyArmsType) ;
            //m_MyPlayerRogueMemoryShare.m_MyArms.transform.parent = m_MyPlayerRogueMemoryShare.m_HandTransform;
            //m_MyPlayerRogueMemoryShare.m_MyArms.transform.localPosition = Vector3.zero;
            //m_MyPlayerRogueMemoryShare.m_MyArms.transform.rotation = m_MyPlayerRogueMemoryShare.m_HandTransform.rotation;
            //Rigidbody lTempRigidbody = m_MyPlayerRogueMemoryShare.m_MyArms.GetComponent<Rigidbody>();
            //lTempRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            //lTempRigidbody.useGravity = false;
        }
        else
        {
            SetCurState(StaticGlobalDel.EMovableState.eNull);
        }

        AnimatorStateCtl.m_ThisAnimator.enabled = Show;
    }

    public void MyAddList(int listindex)
    {
        CurGroupIndex = listindex;
        this.gameObject.SetActive(true);
    }

    public void MyRemove()
    {
        if (m_MyPlayerRogueMemoryShare.m_MyArms != null)
        {
            CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;
            lTempCGGameSceneData.RemoveArmsType(m_MyPlayerRogueMemoryShare.m_MyArmsType, m_MyPlayerRogueMemoryShare.m_MyArms);
            m_MyPlayerRogueMemoryShare.m_MyArmsType = CGGameSceneData.EArmsType.eMax;
            m_MyPlayerRogueMemoryShare.m_MyArms = null;
            m_MyPlayerRogueMemoryShare.m_MyRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        CurGroupIndex = -1;
        ShowAdd(false);
        // ShowMyCollision(false);
        //SetCurState(StaticGlobalDel.EMovableState.eNull);
        this.gameObject.SetActive(false);
    }

    public void SetReadyResult()
    {
        MoveTargetDummyOK = StaticGlobalDel.EBoolState.eTrue;
        SetStateIndex(StaticGlobalDel.EMovableState.eMove, 1);
        SetStateIndex(StaticGlobalDel.EMovableState.eDeath, 1);
        AnimatorStateCtl.SetStateIndividualIndex(CAnimatorStateCtl.EState.eDeath, 1);
        m_MyActorMemoryShare.m_MyMovable.gameObject.layer = (int)StaticGlobalDel.ELayerIndex.eEndPlayerActor;

        for (int i = 0; i < m_MyActorMemoryShare.m_AllChildCollider.Length; i++)
            m_MyActorMemoryShare.m_AllChildCollider[i].gameObject.layer = (int)StaticGlobalDel.ELayerIndex.eEndPlayerActor;
    }

    public override void RemoveGroup()
    {
        m_MyPlayerRogueMemoryShare.m_MyGroup.RemoveAllPlayerRogue(this);

        if (m_MyPlayerRogueMemoryShare.m_MyArms != null)
        {
            m_MyPlayerRogueMemoryShare.m_MyArms.transform.parent = null;
            Rigidbody lTempRigidbody = m_MyPlayerRogueMemoryShare.m_MyArms.GetComponent<Rigidbody>();
            lTempRigidbody.useGravity = true;
            lTempRigidbody.constraints = RigidbodyConstraints.None;

            Vector3 lTempV3 = m_MyPlayerRogueMemoryShare.m_DeathImpactDir + Vector3.up;
            lTempRigidbody.AddForceAtPosition(lTempV3 * Random.Range(150.0f, 300.0f), Random.insideUnitSphere);

            Collider lTempCollider = lTempRigidbody.GetComponentInChildren<Collider>(true);
            lTempCollider.enabled = true;
        }

        base.RemoveGroup();
    }

    public void UpdataDir()
    {
        Vector3 lTempV3 = this.transform.forward;
        lTempV3.y = 0.0f;
        if (Vector3.Dot(TargetDir, lTempV3) < 0.99f)
            this.transform.forward = Vector3.Lerp(this.transform.forward, TargetDir, m_updateDirval * Time.deltaTime);
    }

    //public override void RemoveMyObj()
    //{

    //}
}

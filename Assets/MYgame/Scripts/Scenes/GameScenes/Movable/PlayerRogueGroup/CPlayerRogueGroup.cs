using System.Collections;
using Dreamteck.Splines;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;
using DG.Tweening;
using UniRx;
using System;

public class CTargetPositionData
{
    public Vector3 m_TargetPosition;
    public float m_RingDis = 1.0f;
}

public class CPlayerRogueGroupMemoryShare : CMemoryShareBase
{
    public bool                         m_bDown                     = false;
    public Vector3                      m_OldMouseDownPos           = Vector3.zero;
    public CPlayerRogueGroup            m_PlayerRogueGroup          = null;
    public CObjPool<CPlayerRogue>       m_AllPlayerRoguePool        = new CObjPool<CPlayerRogue>();
    public List<CPlayerRogue>           m_AllPlayerRogueObj         = new List<CPlayerRogue>();
    public Transform                    m_AllPlayerRogueTransform   = null;
    public Transform                    m_AllTargetDummyTransform   = null;
    public SplineFollower               m_DummyCameraFollwer        = null;
    public SplineFollower               m_MySplineFollower          = null;
    public Vector2                      m_TargetOffset              = Vector3.zero;
    public StaticGlobalDel.EBoolState   m_CarCollision              = StaticGlobalDel.EBoolState.eFlase;
    public List<CTargetPositionData>    m_TargetPositionList        = null;
    public Text                         m_ShowCountText             = null;
    public Canvas                       m_MyCanvas                  = null;
    public Transform                    m_PlayerRoguePoolParent     = null;
    public Transform                    m_PlayerRogueGroupTrigger   = null;
    public Transform                    m_CountCanvasTarget         = null;
    public int                          m_CarInCount                = 0;
    public bool                         m_bRearrangement            = false;
    public float                        m_HalfRingDis               = 0.0f;
};

public class CPlayerRogueGroup : CMovableBase
{
    public override EObjType ObjType() { return EObjType.ePlayerRogueGroup; }
    const float CfHalfWidth = 4.0f;
    const float CfTotleWidth = CfHalfWidth * 2.0f;
    const int CstInitQueueCount = 300;

    protected CPlayerRogueGroupMemoryShare m_PlayerRogueGroupMemoryShare = null;

    public override EMovableType MyMovableType() { return EMovableType.ePlayerRogueGroup; }

    // ==================== SerializeField ===========================================



    [SerializeField] Transform m_AllPlayerRogueTransform = null;
    public Transform AllPlayerRogueTransform { get { return m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueTransform; } }
    [SerializeField] Transform m_AllTargetDummyTransform = null;
    public Transform AllTargetDummyTransform { get { return m_PlayerRogueGroupMemoryShare.m_AllTargetDummyTransform; } }

    [SerializeField] Transform m_PlayerRoguePoolParent      = null;
    [SerializeField] Transform m_PlayerRogueGroupTrigger    = null;
    [SerializeField] Transform m_CountTargetDummy           = null;
    [SerializeField] SplineFollower m_DummyCameraFollwer    = null;
    [SerializeField] [Range(1, 300)] protected int m_InitCurListCount = 1;
    // ==================== SerializeField ===========================================

    public SplineFollower MySplineFollower { get { return m_PlayerRogueGroupMemoryShare.m_MySplineFollower; } }
    public int CurPlayerRogueCount { get { return m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count; } }
    public List<CPlayerRogue> AllPlayerRogueList { get { return m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj; } }
    public CObjPool<CPlayerRogue> AllPlayerRoguePool { get { return m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool; } }

    protected CPlayerRogue.CSetParentData m_BuffSetParentData = new CPlayerRogue.CSetParentData();
    protected bool m_PlayerRogueUpdatapos = true;
    protected float m_CurRearrangementPassTime = 0.0f;
    protected float m_MaxRearrangementTime = 0.5f;


    public StaticGlobalDel.EBoolState CarCollision
    {
        set { m_PlayerRogueGroupMemoryShare.m_CarCollision = value; }
        get { return m_PlayerRogueGroupMemoryShare.m_CarCollision; }
    }


    public void SetBoolRearrangement(bool BoolRearrangement, float MaXTime = 0.1f)
    {
        if (m_PlayerRogueGroupMemoryShare.m_bRearrangement == BoolRearrangement && MaXTime >= m_MaxRearrangementTime)
            return;

        m_PlayerRogueGroupMemoryShare.m_bRearrangement = BoolRearrangement;
        if (m_PlayerRogueGroupMemoryShare.m_bRearrangement)
        {
            m_CurRearrangementPassTime = 0.0f;
            m_MaxRearrangementTime = MaXTime;
        }
    }

    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStatePlayerRogueGroup(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayerRogueGroup(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eDrag].AllThisState.Add(new CDragStatePlayerRogueGroup(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CResultStatePlayerRogueGroup(this));
    }

    protected override void CreateMemoryShare()
    {
        m_BuffSetParentData.Group = this;

        m_PlayerRogueGroupMemoryShare = new CPlayerRogueGroupMemoryShare();
        m_MyMemoryShare = m_PlayerRogueGroupMemoryShare;

        m_PlayerRogueGroupMemoryShare.m_PlayerRogueGroup = this;
        m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueTransform     = m_AllPlayerRogueTransform;
        m_PlayerRogueGroupMemoryShare.m_AllTargetDummyTransform     = m_AllTargetDummyTransform;
        m_PlayerRogueGroupMemoryShare.m_MySplineFollower            = this.gameObject.GetComponent<SplineFollower>();
        m_PlayerRogueGroupMemoryShare.m_DummyCameraFollwer          = m_DummyCameraFollwer;
        m_PlayerRogueGroupMemoryShare.m_ShowCountText               = this.gameObject.GetComponentInChildren<Text>();
        m_PlayerRogueGroupMemoryShare.m_MyCanvas                    = this.gameObject.GetComponentInChildren<Canvas>();
        m_PlayerRogueGroupMemoryShare.m_PlayerRoguePoolParent       = m_PlayerRoguePoolParent;
        m_PlayerRogueGroupMemoryShare.m_PlayerRogueGroupTrigger     = m_PlayerRogueGroupTrigger;
        m_PlayerRogueGroupMemoryShare.m_CountCanvasTarget           = m_CountTargetDummy;

        SetBaseMemoryShare();

        CObjPool<CPlayerRogue> lTempAllPlayerRoguePool = m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool;
        m_PlayerRogueGroupMemoryShare.m_TargetPositionList = StaticGlobalDel.GetPositionListAround(m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueTransform.localPosition,
            new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f }, new int[] { 8, 20, 30, 50, 70, 100, 130, 160, 200});

        lTempAllPlayerRoguePool.NewObjFunc = NewPlayerRogue;
        lTempAllPlayerRoguePool.RemoveObjFunc = RemovePlayerRogue;

        lTempAllPlayerRoguePool.InitDefPool(CstInitQueueCount);

        AddCurPlayerRogueCount(m_InitCurListCount);
        ResetMoveBuff(true);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetCurState(StaticGlobalDel.EMovableState.eWait);
        m_PlayerRogueUpdatapos = false;


        UpdatePlayerRogueCountObservable().Subscribe(value => {
            if (value == 0)
            {
                this.ChangState = StaticGlobalDel.EMovableState.eDeath;
                m_MyGameManager.SetState( CGameManager.EState.eGameOver);
            }
        }).AddTo(this.gameObject);
    }

    protected override void Update()
    {
        base.Update();

        // if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady)
        InputUpdata();
    }

    public void UpdateRearrangementTime()
    {
        if (m_PlayerRogueGroupMemoryShare.m_bRearrangement)
        {
            m_CurRearrangementPassTime += Time.deltaTime;
            if (m_CurRearrangementPassTime >= m_MaxRearrangementTime)
            {
                Rearrangement();
                ResetTriggerSize();
            }
        }
    }

    public void UpdateSpeed()
    {
        if (m_MyMemoryShare.m_TotleSpeed != m_MyMemoryShare.m_TargetTotleSpeed)
        {
            m_MyMemoryShare.m_TotleSpeed = Mathf.Lerp(m_MyMemoryShare.m_TotleSpeed, m_MyMemoryShare.m_TargetTotleSpeed, 5.0f * Time.deltaTime);

            if (Mathf.Abs(m_MyMemoryShare.m_TotleSpeed - m_MyMemoryShare.m_TargetTotleSpeed) < 0.01f)
                m_MyMemoryShare.m_TotleSpeed = m_MyMemoryShare.m_TargetTotleSpeed;


            m_PlayerRogueGroupMemoryShare.m_MySplineFollower.followSpeed = m_MyMemoryShare.m_TotleSpeed;
        }
    }

    public void UpdateSplineFollowerOffset()
    {
        if (Mathf.Abs(m_PlayerRogueGroupMemoryShare.m_TargetOffset.x - m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset.x) > 0.0001f)
        {
            Vector2 lTempV2 = Vector2.MoveTowards(m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset, m_PlayerRogueGroupMemoryShare.m_TargetOffset, CfTotleWidth * 1.0f * Time.deltaTime);

            if (Mathf.Abs(m_PlayerRogueGroupMemoryShare.m_TargetOffset.x - m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset.x) < 0.01f)
                lTempV2 = m_PlayerRogueGroupMemoryShare.m_TargetOffset;

            m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset = lTempV2;
        }
    }

    public override void InputUpdata()
    {
        if (Input.GetMouseButtonDown(0))
            PlayerMouseDown();
        else if (Input.GetMouseButton(0))
            PlayerMouseDrag();
        else if (Input.GetMouseButtonUp(0))
            PlayerMouseUp();
    }

    public void PlayerMouseDown()
    {
        m_PlayerRogueGroupMemoryShare.m_bDown = true;
        m_PlayerRogueGroupMemoryShare.m_OldMouseDownPos = Input.mousePosition;
        OnMouseDown();
    }

    public void PlayerMouseDrag()
    {
        if (!m_PlayerRogueGroupMemoryShare.m_bDown)
            return;

        OnMouseDrag();
        m_PlayerRogueGroupMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void PlayerMouseUp()
    {
        if (m_PlayerRogueGroupMemoryShare.m_bDown)
        {
            OnMouseUp();
            m_PlayerRogueGroupMemoryShare.m_bDown = false;
            m_PlayerRogueGroupMemoryShare.m_OldMouseDownPos = Vector3.zero;
        }
    }

    public void MouseDrag()
    {

        float lTempMoveX = Input.mousePosition.x - m_PlayerRogueGroupMemoryShare.m_OldMouseDownPos.x;
        //float lTempMoveRatio = TotleSpeedRatio;

        lTempMoveX = (lTempMoveX / Screen.width) * CfTotleWidth;
        //Vector2 lTempOffset = m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset;
        Vector2 lTempOffset = m_PlayerRogueGroupMemoryShare.m_TargetOffset;
       // lTempOffset.x += lTempMoveX * lTempMoveRatio;
        lTempOffset.x += lTempMoveX;

        float lTemp = Math.Max(CfHalfWidth - m_PlayerRogueGroupMemoryShare.m_HalfRingDis, 0.0f);
        m_PlayerRogueGroupMemoryShare.m_TargetOffset = Vector2.ClampMagnitude(lTempOffset, lTemp);
        // m_PlayerRogueGroupMemoryShare.m_TargetOffset = lTempOffset;
        //Debug.Log($"m_PlayerRogueGroupMemoryShare.m_TargetOffset = {m_PlayerRogueGroupMemoryShare.m_TargetOffset}");
        // m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset = lTempOffset;
    }

    public CPlayerRogue NewPlayerRogue()
    {
        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;

        GameObject lTempObj = Instantiate(lTempCGGameSceneData.m_AllOtherObj[(int)CGGameSceneData.EOtherObj.ePlayerRogue], AllPlayerRogueTransform.position, AllPlayerRogueTransform.rotation, m_PlayerRogueGroupMemoryShare.m_PlayerRoguePoolParent);
        CPlayerRogue lTempPlayerRogue = lTempObj.GetComponent<CPlayerRogue>();
        lTempPlayerRogue.SetParentData(ref m_BuffSetParentData);
        //lTempPlayerRogue.SetHandArms(CGGameSceneData.EArmsType.eMace);
        return lTempPlayerRogue;
    }

    public void CountText(){m_PlayerRogueGroupMemoryShare.m_ShowCountText.text = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count.ToString();}

    public bool RemoveAllPlayerRogue(CPlayerRogue RemovePlayerRogue)
    {
        List<CPlayerRogue> lTempAllPlayerRogue = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj;
        bool lTempbool = lTempAllPlayerRogue.Remove(RemovePlayerRogue);
        RemovePlayerRogue.transform.parent = m_PlayerRogueGroupMemoryShare.m_PlayerRoguePoolParent;

        OnUpdatePlayerRogueCount(lTempAllPlayerRogue.Count);

        CountText();
        return lTempbool;
    }

    public void ResetTriggerSize()
    {
        if (m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count == 0)
        {
            m_PlayerRogueGroupMemoryShare.m_PlayerRogueGroupTrigger.localScale = new Vector3(0.0f, 2.0f, 0.0f);
            return;
        }

        Vector3 lTempV3 = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj[m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count - 1].CurTargetDummy.localPosition;
        lTempV3.y = 0.0f;
        float lTempMagnitude = lTempV3.magnitude;
        float lTempRingDis = lTempMagnitude  + 2.0f;
        m_PlayerRogueGroupMemoryShare.m_PlayerRogueGroupTrigger.localScale = new Vector3(lTempRingDis, 2.0f, lTempRingDis);

      //  m_PlayerRogueGroupMemoryShare.m_CountCanvasTarget.localPosition = new Vector3(0.0f, 2.0f, lTempRingDis);

        if (m_PlayerRogueUpdatapos)
            m_PlayerRogueGroupMemoryShare.m_MyCanvas.transform.localPosition = new Vector3(0.0f, 3.0f, lTempMagnitude);
        else
            m_PlayerRogueGroupMemoryShare.m_MyCanvas.transform.DOLocalMoveZ(lTempMagnitude, 0.3f).SetEase(Ease.Linear);


        m_PlayerRogueGroupMemoryShare.m_HalfRingDis = lTempMagnitude * 0.5f;
    }

    public void AddCurPlayerRogueCount(int Count)
    {
        if (Count < 0)
            return;

        CPlayerRogue lTempPlayerRogue = null;
        int lTempAllPlayerRogueIndex = 0;
        CObjPool<CPlayerRogue> lTempAllPlayerRoguePool = m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool;
        for (int i = 0; i < Count; i++)
        {
            lTempPlayerRogue = lTempAllPlayerRoguePool.AddObj();

            lTempPlayerRogue.transform.rotation = this.transform.rotation;
            lTempPlayerRogue.transform.localPosition = Vector3.zero;
            
            lTempPlayerRogue.ShowAdd(true);
            lTempPlayerRogue.transform.parent = AllPlayerRogueTransform;
            lTempAllPlayerRogueIndex = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count;
            m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Add(lTempPlayerRogue);
            lTempPlayerRogue.MyAddList(lTempAllPlayerRogueIndex);
            lTempPlayerRogue.SetTargetPos(m_PlayerRogueGroupMemoryShare.m_TargetPositionList[lTempAllPlayerRogueIndex], m_PlayerRogueUpdatapos);
        }

        Vector3 lTempv3 = Vector3.zero;
        //List<CPlayerRogue> lTempAllPlayerRogue = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj;
        for (int i = 0; i < m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count; i++)
        {
            lTempv3 = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj[i].transform.position - m_PlayerRogueGroupMemoryShare.m_TargetPositionList[i].m_TargetPosition;
            lTempv3.y = 0.0f;
            if (lTempv3.sqrMagnitude >= 0.1f)
                m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj[i].SetTargetupdatePos(m_PlayerRogueGroupMemoryShare.m_TargetPositionList[i]);
        }

        ResetTriggerSize();
        CountText();
    }

    public void Rearrangement()
    {
        List<CPlayerRogue> lTempAllPlayerRogue = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj;
        //var rnd = new System.Random();
        //lTempAllPlayerRogue.OrderBy(item => rnd.Next());

        Vector3 lTempV3 = Vector3.zero;
        for (int i = 0; i < lTempAllPlayerRogue.Count; i++)
            lTempAllPlayerRogue[i].SetTargetupdatePos(m_PlayerRogueGroupMemoryShare.m_TargetPositionList[i]);

        m_PlayerRogueGroupMemoryShare.m_bRearrangement = false;
        CountText();
    }

    public void RemovePlayerRogue(CPlayerRogue RemoveRogue)
    {
        RemoveRogue.transform.localPosition = Vector3.zero;
        RemoveRogue.MyRemove();
    }

    public void SetAllPlayerRogueState(StaticGlobalDel.EMovableState setState)
    {
        List<CPlayerRogue> lTempAllPlayerRogue = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj;
        for (int i = 0; i < lTempAllPlayerRogue.Count; i++)
            lTempAllPlayerRogue[i].ChangState = setState;
    }

    public void updateFollwer() { m_PlayerRogueGroupMemoryShare.m_DummyCameraFollwer.SetPercent(m_PlayerRogueGroupMemoryShare.m_MySplineFollower.modifiedResult.percent); }

    public override void UpdateCurSpeed()
    {
        base.UpdateCurSpeed();
        m_PlayerRogueGroupMemoryShare.m_MySplineFollower.followSpeed = m_MyMemoryShare.m_TargetTotleSpeed;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagDoorGroup)
        {
            CDoorGroup lTempDoorGroup = other.GetComponentInParent<CDoorGroup>();
            lTempDoorGroup.ShowCollider(false);

            CDoorGroup.EAllPosDoorType lTempDoorDis;
            if (MySplineFollower.motion.offset.x < 0.0f)
                lTempDoorDis = CDoorGroup.EAllPosDoorType.eLPosDoor;
            else
                lTempDoorDis = CDoorGroup.EAllPosDoorType.eRPosDoor;

            CDoor m_TempDoorObj = lTempDoorGroup.GetDoor(lTempDoorDis);
            m_TempDoorObj.gameObject.SetActive(false);

            if (m_TempDoorObj.MyMathematicsSymbol == CDoor.EMathematicsSymbol.eAdd)
            {
                AddCurPlayerRogueCount(m_TempDoorObj.Number);
            }
            else if (m_TempDoorObj.MyMathematicsSymbol == CDoor.EMathematicsSymbol.eMultiply)
            {
                AddCurPlayerRogueCount((m_TempDoorObj.Number - 1) * CurPlayerRogueCount);
            }
            else if (m_TempDoorObj.MyMathematicsSymbol == CDoor.EMathematicsSymbol.eDivide)
            {

            }
            else if (m_TempDoorObj.MyMathematicsSymbol == CDoor.EMathematicsSymbol.eSubtract)
            {

            }
        }
        else if (other.tag == StaticGlobalDel.TagCarCollider)
            m_PlayerRogueGroupMemoryShare.m_CarInCount++;
        else if (other.tag == StaticGlobalDel.TagEndResult)
        {
            other.gameObject.SetActive(false);

            SetStateIndex(StaticGlobalDel.EMovableState.eWin, 0);
            LockChangState = StaticGlobalDel.EMovableState.eWin;
            ChangState = StaticGlobalDel.EMovableState.eWin;
        }
        else if (other.tag == StaticGlobalDel.TagShowEnemy)
        {
            m_MyGameManager.EnemyGroup.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
        }

        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagCarCollider)
        {
            int lTempCarInCount = m_PlayerRogueGroupMemoryShare.m_CarInCount;
            m_PlayerRogueGroupMemoryShare.m_CarInCount--;
            //Debug.Log($"lTempCarInCount = {lTempCarInCount}  ----- m_PlayerRogueGroupMemoryShare.m_CarInCount = {m_PlayerRogueGroupMemoryShare.m_CarInCount}");
            if (lTempCarInCount == 1 && m_PlayerRogueGroupMemoryShare.m_CarInCount == 0)
            {
                SetBoolRearrangement(true);
            }
        }

        base.OnTriggerExit(other);
    }

    public void SetPlayerTargetDir(Vector3 Dir)
    {
        for (int i = 0; i < m_MyGameManager.MyPlayerRogueGroup.AllPlayerRogueList.Count; i++)
            m_MyGameManager.MyPlayerRogueGroup.AllPlayerRogueList[i].TargetDir = Dir;

    }

    public void AddTargetGroup()
    {
        List<CActor> AllPlayerRogue = m_MyGameManager.MyPlayerRogueGroup.AllPlayerRogueList.ToList<CActor>();
        List<CActor> AllEnemy = m_MyGameManager.EnemyGroup.AllEnemy.ToList<CActor>();

        for (int i = 0; i < AllPlayerRogue.Count; i++)
            m_MyGameManager.AddGroup(AllPlayerRogue[i].DummyRef);

        for (int i = 0; i < AllEnemy.Count; i++)
        {
            m_MyGameManager.AddGroup(AllEnemy[i].DummyRef);
            AllEnemy[i].AnimatorStateCtl.m_ThisAnimator.enabled = true;
            AllEnemy[i].OpenCollider(true);
        }
    }

    public void UpdateTarget()
    {
        m_MyGameManager.ResultEndCamera.gameObject.SetActive(true);

        int i = 0;
        for (i = 0; i < m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count; i++)
            m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj[i].SetReadyResult();

        List<CActor> AllPlayerRogue = m_MyGameManager.MyPlayerRogueGroup.AllPlayerRogueList.ToList<CActor>();
        List<CActor> AllEnemy         = m_MyGameManager.EnemyGroup.AllEnemy.ToList<CActor>();
        var rnd = new System.Random();
        AllPlayerRogue.OrderBy(item => rnd.Next());
        AllEnemy.OrderBy(item => rnd.Next());

        

        int lTempMaxCount = Mathf.Min(AllPlayerRogue.Count, AllEnemy.Count);

        
        for (i = 0; i < lTempMaxCount; i++)
        {
            AllPlayerRogue[i].SetTarget(AllEnemy[i]);
            AllEnemy[i].SetTarget(AllPlayerRogue[i]);

            AllPlayerRogue[i].ChangState = StaticGlobalDel.EMovableState.eMove;
            AllEnemy[i].ChangState = StaticGlobalDel.EMovableState.eMove;

            //m_MyGameManager.AddGroup(AllPlayerRogue[i].transform);
            //m_MyGameManager.AddGroup(AllEnemy[i].transform);
        }

        //return;

        if (AllPlayerRogue.Count == AllEnemy.Count)
            return;

        List<CActor> lTempMuchActor = AllPlayerRogue.Count > AllEnemy.Count ? AllPlayerRogue : AllEnemy;
        List<CActor> lTempMinActor = AllPlayerRogue.Count < AllEnemy.Count ? AllPlayerRogue : AllEnemy;

        for (; i < lTempMuchActor.Count; i++)
        {
            lTempMuchActor[i].SetTarget(lTempMinActor[i% lTempMinActor.Count]);
            lTempMuchActor[i].ChangState = StaticGlobalDel.EMovableState.eMove;
        //    m_MyGameManager.AddGroup(lTempMuchActor[i].transform);
        }


    }

    // ===================== UniRx ======================
    Subject<int> m_PlayerRogueCountEvent;

    public void OnUpdatePlayerRogueCount(int value)
    {
        if (m_PlayerRogueCountEvent != null)
            m_PlayerRogueCountEvent.OnNext(value);
    }

    public UniRx.Subject<int> UpdatePlayerRogueCountObservable()
    {
        return m_PlayerRogueCountEvent ?? (m_PlayerRogueCountEvent = new Subject<int>());
    }

    // ===================== UniRx ======================
}

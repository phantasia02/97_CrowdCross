using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CPlayerMemoryShare : CMemoryShareBase
{
    public bool                     m_bDown                 = false;
    public Vector3                  m_OldMouseDownPos       = Vector3.zero;
    public CinemachineVirtualCamera m_PlayerNormalCamera    = null;
    public CinemachineVirtualCamera m_PlayerWinLoseCamera   = null;
    public Vector3                  m_OldMouseDragDirNormal = Vector3.zero;
    public CPlayer                  m_MyPlayer              = null;
    public Vector3[]                m_AllPathPoint          = new Vector3[8];
    public int                      m_CurStandPointindex    = 0;
    public Vector3                  m_TargetStandPoint      = Vector3.zero;
    public Collider                 m_SwordeCollider        = null;
    public TrailRenderer            m_SwordeTrailRenderer   = null;
};

public class CPlayer : CActor
{
    protected float m_MaxMoveDirSize = 0;

    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    [SerializeField] CinemachineVirtualCamera m_PlayerNormalCamera = null;
    public CinemachineVirtualCamera PlayerNormalFollowObj { get { return m_PlayerNormalCamera; } }

    [SerializeField] CinemachineVirtualCamera m_PlayerWinLoseCamera = null;
    public CinemachineVirtualCamera PlayerWinLoseCamera { get { return m_PlayerWinLoseCamera; } }

    [SerializeField] Collider       m_SwordeCollider    = null;
    [SerializeField] TrailRenderer  m_SwordeTrailRenderer = null;
    [SerializeField] CStandPoint    m_StartStandPoint   = null;
    public CStandPoint StartStandPoint { get { return m_StartStandPoint; } }

    [SerializeField] CStandPoint    m_EndStandPoint     = null;


    protected Vector3 m_OldMouseDragDir = Vector3.zero;


 

    protected override void AddInitState()
    {
        //m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStatePlayer(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayer(this));
        ////m_AllState[(int)StaticGlobalDel.EMovableState.eJump].AllThisState.Add(= new CJumpStatePlayer(this);
        ////m_AllState[(int)StaticGlobalDel.EMovableState.eHit] .AllThisState.Add(= new CHitStatePlayer(this);
        //m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinStatePlayer(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayer(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyPlayerMemoryShare = new CPlayerMemoryShare();
        m_MyMemoryShare = m_MyPlayerMemoryShare;

        m_MyPlayerMemoryShare.m_PlayerNormalCamera  = m_PlayerNormalCamera;
        m_MyPlayerMemoryShare.m_PlayerWinLoseCamera = m_PlayerWinLoseCamera;
        m_MyPlayerMemoryShare.m_MyPlayer            = this;
        m_MyPlayerMemoryShare.m_SwordeCollider      = m_SwordeCollider;
        m_MyPlayerMemoryShare.m_SwordeTrailRenderer = m_SwordeTrailRenderer;
        

        SetBaseMemoryShare();

        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 10.0f;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetCurState(StaticGlobalDel.EMovableState.eWait);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady)
            InputUpdata();


        //transform.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * 3.0f));
       // m_MyPlayerMemoryShare.m_DamiCameraFollwer.
        //CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
        //if (lTempGameSceneWindow && lTempGameSceneWindow.GetShow())
        //    lTempGameSceneWindow.SetBouncingBedCount(m_MyGameManager.GetFloorBouncingBedBoxCount(m_MyMemoryShare.m_FloorNumber));
    }


    public override void InputUpdata()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            PlayerMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            PlayerMouseUp();
        }
    }

    public void PlayerMouseDown()
    {
        //if (!PlayerCtrl())
        //{
        //    if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        //    {
        //        m_AllState[(int)m_CurState].MouseDown();
        //    }
        //}

        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDown();

        m_MyPlayerMemoryShare.m_bDown = true;
        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void PlayerMouseDrag()
    {
        //if (!PlayerCtrl())
        //    return;
        if (!m_MyPlayerMemoryShare.m_bDown)
            return;

        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDrag();

        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void PlayerMouseUp()
    {
        if (m_MyPlayerMemoryShare.m_bDown)
        {
            DataState lTempDataState = m_AllState[(int)CurState];
            if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
                lTempDataState.AllThisState[lTempDataState.index].MouseUp();

            m_MyPlayerMemoryShare.m_bDown = false;
            m_MyPlayerMemoryShare.m_OldMouseDownPos = Vector3.zero;
        }
    }

    public void PlayStart()
    {
        ChangState = StaticGlobalDel.EMovableState.eMove;

        m_MyPlayerMemoryShare.m_AllPathPoint[0] = m_StartStandPoint.transform.position;

        CDragHeroFloor lTempDragHeroFloor = m_MyGameManager.MyDragHeroFloor;

        bool grounded = true;
        RaycastHit lTempRaycastHit;
        Vector3 lTempV3 = Vector3.zero;
        for (int i = 0; i < lTempDragHeroFloor.AllDragHeroDragHeroGroup.Count; i++)
        {
            lTempV3 = lTempDragHeroFloor.AllDragHeroDragHeroGroup[i].transform.position;
            lTempV3.y = transform.position.y;
            lTempV3.y += 1.0f;
            grounded = Physics.Raycast(lTempV3, Vector3.down, out lTempRaycastHit, 10.0f, StaticGlobalDel.g_BridgeMask);
            if (grounded)
            {
                lTempV3 = lTempRaycastHit.point;
                lTempV3.y += -0.1f;
            }
            else
                lTempV3 = Vector3.zero;

            m_MyPlayerMemoryShare.m_AllPathPoint[1 + i] = lTempV3;
        }
        

        m_MyPlayerMemoryShare.m_AllPathPoint[m_MyPlayerMemoryShare.m_AllPathPoint.Length - 1] = m_EndStandPoint.transform.position;

        NextStandPoint();
    }

    public void NextStandPoint()
    {
        int Curindex = m_MyPlayerMemoryShare.m_CurStandPointindex;
        int Nextindex = Curindex + 1;
        Vector3[] lTempStandPoint = m_MyPlayerMemoryShare.m_AllPathPoint;
        m_MyPlayerMemoryShare.m_CurStandPointindex = Nextindex;

        if (Nextindex + 2 == lTempStandPoint.Length && Mathf.Abs(lTempStandPoint[lTempStandPoint.Length - 2].y - m_StartStandPoint.transform.position.y) < 0.5f)
        {
            ChangState = StaticGlobalDel.EMovableState.eWin;
            return;
        }


        if (Mathf.Abs( lTempStandPoint[Nextindex].y - m_StartStandPoint.transform.position.y) < 0.5f)
        {
            m_MyPlayerMemoryShare.m_TargetStandPoint = lTempStandPoint[Nextindex];
            ChangState = StaticGlobalDel.EMovableState.eMove;
        }
        else
        {
            m_MyGameManager.Enemy.ChangState = StaticGlobalDel.EMovableState.eAtk;
            ChangState = StaticGlobalDel.EMovableState.eDeath;
        }

        SameStatusUpdate = true;
    }


    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Arrow")
        {
            CArrow lTempArrow = other.GetComponentInParent<CArrow>();

            DataState lTempDataState = m_AllState[(int)CurState];
           // ((CDeathStatePlayer)lTempDataState.AllThisState[lTempDataState.index]).DieRotate(lTempArrow);
        }
        else if (other.tag == "BridgeFloor")
        {
            CObjDestruction lTempObjDestruction = other.GetComponentInParent<CObjDestruction>();
            lTempObjDestruction.DestroyMesh();

            m_MyGameManager.AllDragHeroDie();
        }

        base.OnTriggerEnter(other);
    }
}

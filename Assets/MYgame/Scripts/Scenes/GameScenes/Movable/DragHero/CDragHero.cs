using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBridgeMoveData
{
    public Transform    m_BridgeTransform   = null;
    public Transform    m_BridgeEndTransform = null;
}

public class CDragHeroShare : CMemoryShareBase
{
    public CDragHero                m_MyDragHero            = null;
    public int                      m_BridgeAddX            = 0;
    public Transform                m_JumpTargetTransform   = null;
    public int                      m_GroupIndex            = 0;
    public CDragHeroGroup           m_MyDragHeroGroup       = null;
    public List<CBridgeMoveData>    m_AllBridgeMoveData     = new List<CBridgeMoveData>();
    public AfterImageEffects        m_AfterImageEffects     = null;
    public Transform                m_AllTransformObj       = null;
};

public class CDragHero : CActor
{
    protected CDragHeroShare m_MyDragHeroShare = null;

    [SerializeField] protected Transform m_TopFloor   = null;
    public Transform TopFloor { get { return m_TopFloor; } }

    [SerializeField] protected Transform m_DragHeroStandPoint = null;
    public Transform DragHeroStandPoint { get { return m_DragHeroStandPoint; } }

    [SerializeField] Transform m_AllBridgeParent = null;
    public Transform AllBridgeParent { get { return m_AllBridgeParent; } }

    public Vector3 TopSpreads { get { return m_DragHeroStandPoint.position - this.transform.position; } }

    [SerializeField] Transform m_AllObj = null;
    
    public CDragHeroGroup MyDragHeroGroup
    {
        get { return m_MyDragHeroShare.m_MyDragHeroGroup; }
        set
        {
            m_MyDragHeroShare.m_MyDragHeroGroup = value;
            this.transform.parent = m_MyDragHeroShare.m_MyDragHeroGroup.transform;
        }
    }

    public int BridgeAddX
    {
        set { m_MyDragHeroShare.m_BridgeAddX = Mathf.Clamp(value, -1, 1); }
        get { return m_MyDragHeroShare.m_BridgeAddX; }
    }

    public Transform JumpTargetTransform
    {
        set { m_MyDragHeroShare.m_JumpTargetTransform = value; }
        get { return m_MyDragHeroShare.m_JumpTargetTransform; }
    }

    public int GroupIndex
    {
        set { m_MyDragHeroShare.m_GroupIndex = value; }
        get { return m_MyDragHeroShare.m_GroupIndex; }
    }

   // protected List<Transform> m_AllBridge   = new List<Transform>();
    protected override void AddInitState()
    {
        //m_AllState[(int)StaticGlobalDel.EMovableState.eJump].AllThisState.Add(new CJumpStateDragHero(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStateDragHero(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStateDragHero(this));
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < m_MyDragHeroShare.m_AllBridgeMoveData.Count; i++)
            m_MyDragHeroShare.m_AllBridgeMoveData[i].m_BridgeEndTransform.parent = AllBridgeParent;

        SetCurState(StaticGlobalDel.EMovableState.eWait);
    }

    protected override void CreateMemoryShare()
    {
        m_MyDragHeroShare = new CDragHeroShare();
        m_MyMemoryShare = m_MyDragHeroShare;

        m_MyDragHeroShare.m_MyDragHero = this;
        m_MyDragHeroShare.m_AfterImageEffects = this.GetComponentInChildren<AfterImageEffects>(true);
        m_MyDragHeroShare.m_AllTransformObj = m_AllObj;

        GameObject lTempObj = null;
        CGGameSceneData lTempGGameSceneData = CGGameSceneData.SharedInstance;
        CBridgeMoveData lTempBridgeMoveData = null;
        Transform lTempTransform = null;
        for (int i = 0; i < AllBridgeParent.childCount; i++)
        {
            lTempTransform = AllBridgeParent.GetChild(i);
            if (lTempTransform != null)
            {
                lTempBridgeMoveData = new CBridgeMoveData();
                lTempBridgeMoveData.m_BridgeTransform = lTempTransform;
                lTempObj = GameObject.Instantiate(lTempGGameSceneData.m_AllOtherObj[(int)CGGameSceneData.EOtherObj.eDummyTransform], this.transform);
                lTempBridgeMoveData.m_BridgeEndTransform = lTempObj.transform;
                lTempBridgeMoveData.m_BridgeEndTransform.position = TopFloor.position;
                m_MyDragHeroShare.m_AllBridgeMoveData.Add(lTempBridgeMoveData);
            }
        }

        SetBaseMemoryShare();

        MyDragHeroGroup = this.GetComponentInParent<CDragHeroGroup>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady)
            InputUpdata();
    }

    public void OnEnable()
    {
        m_MyGameManager.AddAllDragHero(this);
    }


    public void AddBridge(CBridgeMoveData AddBridge)
    {
        AddBridge.m_BridgeTransform.parent      = AllBridgeParent;
        AddBridge.m_BridgeEndTransform.parent   = AllBridgeParent;
        m_MyDragHeroShare.m_AllBridgeMoveData.Add(AddBridge);
    }

    public void RemoveAllBridge(CDragHero AddBridgeDragHero)
    {
        if (AddBridgeDragHero == null)
            return;

        CBridgeMoveData lTempBridgeMoveData = null;
        for (int i = m_MyDragHeroShare.m_AllBridgeMoveData.Count - 1; i >= 0; i--)
        {
            lTempBridgeMoveData = m_MyDragHeroShare.m_AllBridgeMoveData[0];
            m_MyDragHeroShare.m_AllBridgeMoveData.Remove(lTempBridgeMoveData);
            AddBridgeDragHero.AddBridge(lTempBridgeMoveData);
        }
    }

    public void UpdateBridgePos()
    {
        Vector3 lTempV3 = AllBridgeParent.transform.position;
        for (int i = 0; i < m_MyDragHeroShare.m_AllBridgeMoveData.Count; i++)
        {
            lTempV3 = AllBridgeParent.transform.position;
            lTempV3.x += (i * BridgeAddX);
            //m_MyDragHeroShare.m_AllBridgeMoveData[i].m_BridgeTransform.position = lTempV3;
            m_MyDragHeroShare.m_AllBridgeMoveData[i].m_BridgeEndTransform.position = lTempV3;
        }
    }
}

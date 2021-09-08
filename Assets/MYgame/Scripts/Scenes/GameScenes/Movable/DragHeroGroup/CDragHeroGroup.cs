using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CDragHeroGroupShare : CMemoryShareBase
{
    public CDragHeroGroup   m_MyDragHeroGroup           = null;
    public List<CDragHero>  m_MyAllDragHero             = new List<CDragHero>();
    public Transform[]      m_MyAllHeroStandTransform   = new Transform[8];
    public Vector3          m_v3OriginalpPos            = Vector3.zero;
    public Collider         m_Trigger                   = null;
    public int              m_OldAddX                   = 0;
};

public class CDragHeroGroup : CMovableBase
{
    protected CDragHeroGroupShare m_MyDragHeroGroupShare = null;

    // ==================== SerializeField ===========================================

    [Range(0, 10)]
    [SerializeField] protected int m_NewDragHeroCount = 0;
    [SerializeField] protected GameObject m_AllColliderTrigger = null;
    [SerializeField] protected Collider m_Trigger = null;
    // ==================== SerializeField ===========================================

    public int DragHeroCount { get { return m_MyDragHeroGroupShare.m_MyAllDragHero.Count; } }

    protected int m_DragHeroFloorIndex = -1;
    public int DragHeroFloorIndex
    {
        get { return m_DragHeroFloorIndex; }
        set { m_DragHeroFloorIndex = value; }
    }

    protected CDragHeroFloor m_MyDragHeroFloor = null;

    //CStandPoint m_MyStandPoint = null;
    //public CStandPoint MyStandPoint { get { return m_MyStandPoint; } }

    protected override void AddInitState()
    {

        //m_AllState[(int)StaticGlobalDel.EMovableState.eDrag].AllThisState.Add(new CDragStateDragHeroGroup(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStateDragHeroGroup(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStateDragHeroGroup(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eJump].AllThisState.Add(new CJumpStateDragHeroGroup(this));
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetCurState(StaticGlobalDel.EMovableState.eWait);
    }

    protected override void CreateMemoryShare()
    {
        m_MyDragHeroFloor = this.gameObject.GetComponentInParent<CDragHeroFloor>();
        m_MyDragHeroGroupShare = new CDragHeroGroupShare();
        m_MyMemoryShare = m_MyDragHeroGroupShare;

        m_MyDragHeroGroupShare.m_MyDragHeroGroup    = this;
        m_MyDragHeroGroupShare.m_v3OriginalpPos     = this.transform.position;
        m_MyDragHeroGroupShare.m_Trigger            = m_Trigger;


        CGGameSceneData lTempGGameSceneData = CGGameSceneData.SharedInstance;
        int lTempindex = 0;
        GameObject lTempDragHeroObj = null;

       // lTempindex = (int)CGGameSceneData.EOtherObj.eStandPoint;
        //lTempDragHeroObj = GameObject.Instantiate(lTempGGameSceneData.m_AllOtherObj[lTempindex], this.transform);
        //m_MyStandPoint = lTempDragHeroObj.GetComponent<CStandPoint>();

        lTempindex = (int)CGGameSceneData.EOtherObj.eDragHero;
        lTempDragHeroObj = null;
        CDragHero lTempCDragHero = null;

        for (int i = 0; i < m_NewDragHeroCount; i++)
        {
            lTempCDragHero = null;
            lTempDragHeroObj = GameObject.Instantiate(lTempGGameSceneData.m_AllOtherObj[lTempindex], this.transform);
            lTempCDragHero = lTempDragHeroObj.GetComponent<CDragHero>();
            m_MyDragHeroGroupShare.m_MyAllDragHero.Add(lTempCDragHero);
        }

        for (int i = 0; i < m_MyDragHeroGroupShare.m_MyAllDragHero.Count; i++)
        {
            m_MyDragHeroGroupShare.m_MyAllDragHero[i].BridgeAddX = 0;
            m_MyDragHeroGroupShare.m_MyAllDragHero[i].GroupIndex = i;
        }

        lTempCDragHero = lTempGGameSceneData.m_AllOtherObj[lTempindex].GetComponent<CDragHero>();
        lTempindex = (int)CGGameSceneData.EOtherObj.eJumpTransform;
        Vector3 lTempSpread = lTempCDragHero.TopSpreads;
        Vector3 lTempV3 = this.transform.position;
        GameObject lTempObj = null;
        for (int i = 0; i < m_MyDragHeroGroupShare.m_MyAllHeroStandTransform.Length; i++)
        {
            lTempObj = GameObject.Instantiate(lTempGGameSceneData.m_AllOtherObj[lTempindex], this.transform);
            lTempObj.name = lTempObj.name + i.ToString("00");
            lTempObj.transform.position = lTempV3;
            m_MyDragHeroGroupShare.m_MyAllHeroStandTransform[i] = lTempObj.transform;
            lTempV3 = lTempObj.transform.position + lTempSpread;
        }

        UpdateDragHeroPos();

        SetBaseMemoryShare();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void AddCombine(CDragHeroGroup AddDragHeroGroup)
    {
        int lTempAddX = AddDragHeroGroup.transform.position.x - this.transform.position.x < 0.0 ? -1 : -1;
        CDragHero lTempDragHero = null;

        for (int i = AddDragHeroGroup.DragHeroCount - 1; i >= 0; i--)
            AddDragHeroData(AddDragHeroGroup.RemoveDragHero(0));

        for (int i = 0; i < m_MyDragHeroGroupShare.m_MyAllDragHero.Count; i++)
        {
            m_MyDragHeroGroupShare.m_MyAllDragHero[i].BridgeAddX = lTempAddX;
            m_MyDragHeroGroupShare.m_MyAllDragHero[i].GroupIndex = i;
        }

        lTempDragHero = m_MyDragHeroGroupShare.m_MyAllDragHero[m_MyDragHeroGroupShare.m_MyAllDragHero.Count - 1];
        for (int i = 0; i < m_MyDragHeroGroupShare.m_MyAllDragHero.Count - 1; i++)
            m_MyDragHeroGroupShare.m_MyAllDragHero[i].RemoveAllBridge(lTempDragHero);

        m_MyDragHeroGroupShare.m_OldAddX = lTempAddX;
        UpdateDragHeroPos(true);
        m_MyDragHeroGroupShare.m_MyAllDragHero[m_MyDragHeroGroupShare.m_MyAllDragHero.Count - 1].UpdateBridgePos();

        ChangState = StaticGlobalDel.EMovableState.eJump;
        m_MyGameManager.SetState(CGameManager.EState.ePlayHold);
    }

    public void AddDragHeroData(CDragHero addDragHero)
    {
        addDragHero.MyDragHeroGroup = this;
        //addDragHero.gameObject.transform.parent = this.transform;
        m_MyDragHeroGroupShare.m_MyAllDragHero.Add(addDragHero);
    }

    public CDragHero RemoveDragHero(int index)
    {
        if (index < 0 || m_MyDragHeroGroupShare.m_MyAllDragHero.Count <= index)
            return null;

        CDragHero lTempDragHero = m_MyDragHeroGroupShare.m_MyAllDragHero[index];
        m_MyDragHeroGroupShare.m_MyAllDragHero.Remove(lTempDragHero);

        return lTempDragHero;
    }

    public void ClearGroupList()
    {
        m_MyDragHeroGroupShare.m_MyAllDragHero.Clear();
    }

    public void UpdateDragHeroPos(bool Animation = false)
    {
        m_AllColliderTrigger.SetActive(m_MyDragHeroGroupShare.m_MyAllDragHero.Count != 0);

        if (m_MyDragHeroGroupShare.m_MyAllDragHero.Count == 0)
            return;
        
        Vector3 lTempV3 = this.transform.position;
        Transform lTempTransform = null;
        Transform DragHeroStandPointTransform = this.transform;
        CDragHero lTempDragHero = null;

        Sequence lTempSequence = null;

        if (Animation)
            lTempSequence = DOTween.Sequence();

        for (int i = m_MyDragHeroGroupShare.m_MyAllDragHero.Count - 1; i >= 0; i--)
        {
            lTempDragHero = m_MyDragHeroGroupShare.m_MyAllDragHero[i];
            lTempTransform = m_MyDragHeroGroupShare.m_MyAllDragHero[i].transform;
            DragHeroStandPointTransform = m_MyDragHeroGroupShare.m_MyAllHeroStandTransform[i];

            if (Animation)
            {
                if (Vector3.SqrMagnitude(DragHeroStandPointTransform.position - lTempTransform.position) > 0.5f)
                {
                    lTempDragHero.JumpTargetTransform = DragHeroStandPointTransform;
                    lTempDragHero.ChangState = StaticGlobalDel.EMovableState.eJump;
                   // DragHeroJump(lTempDragHero);
                   // lTempSequence.AppendInterval(0.1f);
                }
            }
            else
                m_MyDragHeroGroupShare.m_MyAllDragHero[i].transform.position = DragHeroStandPointTransform.position;
        }


        m_AllColliderTrigger.transform.localScale = new Vector3(1.0f, (float)m_MyDragHeroGroupShare.m_MyAllDragHero.Count, 1.0f);
    }

    public void CheckJumpOK()
    {
      //  ((CJumpStateDragHeroGroup)m_AllState[(int)StaticGlobalDel.EMovableState.eJump].AllThisState[0]).SwitchWait();
    }

}

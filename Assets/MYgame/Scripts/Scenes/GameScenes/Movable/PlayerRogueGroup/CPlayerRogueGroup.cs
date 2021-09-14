using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//public class CPlayerRogueAndTarget
//{
//    public CPlayerRogue m_PlayerRogue = null;
//    public Transform    m_TargetDummy = null;
//}

public class CPlayerRogueGroupMemoryShare : CMemoryShareBase
{
    public CPlayerRogueGroup                    m_PlayerRogueGroup          = null;
    public CObjPool<CPlayerRogue>               m_AllPlayerRoguePool        = new CObjPool<CPlayerRogue>();
    public Transform                            m_AllPlayerRogueTransform   = null;
    public Transform                            m_AllTargetDummyTransform   = null;
};

public class CPlayerRogueGroup : CMovableBase
{
    const int CstInitQueueCount = 200;

    protected CPlayerRogueGroupMemoryShare m_PlayerRogueGroupMemoryShare = null;

    public override EMovableType MyMovableType() { return EMovableType.ePlayerRogueGroup; }

    // ==================== SerializeField ===========================================

    [SerializeField] CinemachineVirtualCamera m_PlayerNormalCamera = null;
    public CinemachineVirtualCamera PlayerNormalFollowObj { get { return m_PlayerNormalCamera; } }

    [SerializeField] CinemachineVirtualCamera m_PlayerWinLoseCamera = null;
    public CinemachineVirtualCamera PlayerWinLoseCamera { get { return m_PlayerWinLoseCamera; } }

    [SerializeField] Transform m_AllPlayerRogueTransform = null;
    public Transform AllPlayerRogueTransform { get { return m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueTransform; } }
    [SerializeField] Transform m_AllTargetDummyTransform = null;
    public Transform AllTargetDummyTransform { get { return m_PlayerRogueGroupMemoryShare.m_AllTargetDummyTransform; } }


    [SerializeField] [Range(1, 300)] protected int m_InitCurListCount  = 1;
    // ==================== SerializeField ===========================================

    protected CPlayerRogue.CSetParentData m_BuffSetParentData = new CPlayerRogue.CSetParentData();
    protected bool m_PlayerRogueUpdatapos = true;

    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStatePlayerRogueGroup(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayerRogueGroup(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eDrag].AllThisState.Add(new CDragStatePlayerRogueGroup(this));
    }

    protected override void CreateMemoryShare()
    {
        m_BuffSetParentData.Group = this;

        m_PlayerRogueGroupMemoryShare = new CPlayerRogueGroupMemoryShare();
        m_MyMemoryShare = m_PlayerRogueGroupMemoryShare;

        m_PlayerRogueGroupMemoryShare.m_PlayerRogueGroup = this;
        m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueTransform = m_AllPlayerRogueTransform;
        m_PlayerRogueGroupMemoryShare.m_AllTargetDummyTransform = m_AllTargetDummyTransform;

        SetBaseMemoryShare();

        CObjPool<CPlayerRogue> lTempAllPlayerRoguePool = m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool;
        List<Vector3> targetPositionList = GetPositionListAround(this.transform.position, new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f}, new int[] { 8, 20, 30, 50, 70, 100 });

        lTempAllPlayerRoguePool.NewObjFunc       = NewPlayerRogue;
        lTempAllPlayerRoguePool.RemoveObjFunc    = (CPlayerRogue RemoveRogue) => { RemoveRogue.MyRemove(); };
        lTempAllPlayerRoguePool.AddListObjFunc   = (CPlayerRogue AddRogue, int index) => 
        {
            AddRogue.MyAddList(index);
            AddRogue.SetTargetPos(targetPositionList[index], m_PlayerRogueUpdatapos);
        };

        lTempAllPlayerRoguePool.InitDefPool(CstInitQueueCount);

        for (int i = 0; i < m_InitCurListCount; i++)
            lTempAllPlayerRoguePool.AddObj();
    }

    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetCurState(StaticGlobalDel.EMovableState.eMove);
        m_PlayerRogueUpdatapos = false;
    }

    protected override void Update()
    {
        base.Update();

       // if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady)
            InputUpdata();
    }


    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));

        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        Vector3 ApplyRotationToVector(Vector3 vec, float angle) { return Quaternion.Euler(0, angle, 0) * vec; }

        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0, 0), angle);
            Vector3 position = startPosition + dir * (distance + Random.Range(-0.5f, 0.5f));
            positionList.Add(position);
        }
        return positionList;
    }
    
    public CPlayerRogue NewPlayerRogue()
    {
        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;

        GameObject lTempObj = Instantiate(lTempCGGameSceneData.m_AllOtherObj[(int)CGGameSceneData.EOtherObj.ePlayerRogue], AllPlayerRogueTransform.position, AllPlayerRogueTransform.rotation, AllPlayerRogueTransform.transform);
        CPlayerRogue lTempPlayerRogue = lTempObj.GetComponent<CPlayerRogue>();
        lTempPlayerRogue.SetParentData(ref m_BuffSetParentData);

        return lTempPlayerRogue;
    }

    public void SetAllPlayerRogueState(StaticGlobalDel.EMovableState setState)
    {
        CObjPool<CPlayerRogue> lTempAllPlayerRoguePool = m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool;

        for (int i = 0; i < lTempAllPlayerRoguePool.CurAllObjCount; i++)
            lTempAllPlayerRoguePool.AllCurObj[i].ChangState = setState;
    }
}

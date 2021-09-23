using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Dreamteck.Splines;

//public class CPlayerRogueAndTarget
//{
//    public CPlayerRogue m_PlayerRogue = null;
//    public Transform    m_TargetDummy = null;
//}

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
    public List<Vector3>                m_TargetPositionList        = null;
    public Text                         m_ShowCountText             = null;
    public Canvas                       m_MyCanvas                  = null;

};

public class CPlayerRogueGroup : CMovableBase
{
    const float CfHalfWidth = 3.0f;
    const float CfTotleWidth = CfHalfWidth * 2.0f;
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

    [SerializeField] SplineFollower m_DummyCameraFollwer = null;
    [SerializeField] [Range(1, 300)] protected int m_InitCurListCount = 1;
    // ==================== SerializeField ===========================================

    public SplineFollower MySplineFollower { get { return m_PlayerRogueGroupMemoryShare.m_MySplineFollower; } }
    public int CurPlayerRogueCount { get { return m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count; } }
    public CObjPool<CPlayerRogue> AllPlayerRoguePool { get { return m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool; } }

    protected CPlayerRogue.CSetParentData m_BuffSetParentData = new CPlayerRogue.CSetParentData();
    protected bool m_PlayerRogueUpdatapos = true;

    public StaticGlobalDel.EBoolState CarCollision
    {
        set { m_PlayerRogueGroupMemoryShare.m_CarCollision = value; }
        get { return m_PlayerRogueGroupMemoryShare.m_CarCollision; }
    }

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
        m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueTransform     = m_AllPlayerRogueTransform;
        m_PlayerRogueGroupMemoryShare.m_AllTargetDummyTransform     = m_AllTargetDummyTransform;
        m_PlayerRogueGroupMemoryShare.m_MySplineFollower            = this.gameObject.GetComponent<SplineFollower>();
        m_PlayerRogueGroupMemoryShare.m_DummyCameraFollwer          = m_DummyCameraFollwer;
        m_PlayerRogueGroupMemoryShare.m_ShowCountText               = this.gameObject.GetComponentInChildren<Text>();
        m_PlayerRogueGroupMemoryShare.m_MyCanvas                    = this.gameObject.GetComponentInChildren<Canvas>();

        SetBaseMemoryShare();

        CObjPool<CPlayerRogue> lTempAllPlayerRoguePool = m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool;
        m_PlayerRogueGroupMemoryShare.m_TargetPositionList = GetPositionListAround(m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueTransform.localPosition, new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f }, new int[] { 8, 20, 30, 50, 70, 100 });

        lTempAllPlayerRoguePool.NewObjFunc = NewPlayerRogue;
        lTempAllPlayerRoguePool.RemoveObjFunc = RemovePlayerRogue;

        //lTempAllPlayerRoguePool.AddListObjFunc = (CPlayerRogue AddRogue, int index) =>
        //{
        //    //AddRogue.MyAddList(index);
        //    //AddRogue.SetTargetPos(m_PlayerRogueGroupMemoryShare.m_TargetPositionList[index], m_PlayerRogueUpdatapos);
        //};

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
    }

    protected override void Update()
    {
        base.Update();

        // if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady)
        InputUpdata();
    }

    public void UpdateSpeed()
    {
        if (m_MyMemoryShare.m_TotleSpeed != m_MyMemoryShare.m_TargetTotleSpeed)
        {
            m_MyMemoryShare.m_TotleSpeed = Mathf.Lerp(m_MyMemoryShare.m_TotleSpeed, m_MyMemoryShare.m_TargetTotleSpeed, 10.0f * Time.deltaTime);

            if (Mathf.Abs(m_MyMemoryShare.m_TotleSpeed - m_MyMemoryShare.m_TargetTotleSpeed) < 0.001f)
                m_MyMemoryShare.m_TotleSpeed = m_MyMemoryShare.m_TargetTotleSpeed;

            m_PlayerRogueGroupMemoryShare.m_MySplineFollower.followSpeed = m_MyMemoryShare.m_TotleSpeed;
        }
    }

    public void UpdateSplineFollowerOffset()
    {
        //if (Mathf.Abs(m_PlayerRogueGroupMemoryShare.m_TargetOffset.x - m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset.x) < 0.0001f)
        //{
        //    Vector2 lTempV2 = Vector2.MoveTowards(m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset, m_PlayerRogueGroupMemoryShare.m_TargetOffset, CfTotleWidth * Time.deltaTime);
        //    Debug.Log($" 3333333333333333 lTempV2 = {lTempV2}");
        //    if (Mathf.Abs(m_PlayerRogueGroupMemoryShare.m_TargetOffset.x - m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset.x) < 0.001f)
        //        lTempV2 = m_PlayerRogueGroupMemoryShare.m_TargetOffset;


        //    Debug.Log($" lTempV2 = {lTempV2}");
        //    m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset = lTempV2;
        //}
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
        Vector2 lTempOffset = m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset;
       // lTempOffset.x += lTempMoveX * lTempMoveRatio;
        lTempOffset.x += lTempMoveX;
        lTempOffset = Vector2.ClampMagnitude(lTempOffset, CfHalfWidth);

       // m_PlayerRogueGroupMemoryShare.m_TargetOffset = lTempOffset;
       // Debug.Log($"m_PlayerRogueGroupMemoryShare.m_TargetOffset = {m_PlayerRogueGroupMemoryShare.m_TargetOffset}");
        m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset = lTempOffset;
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

    public void CountText(){m_PlayerRogueGroupMemoryShare.m_ShowCountText.text = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count.ToString();}

    public bool RemoveAllPlayerRogue(CPlayerRogue RemovePlayerRogue)
    {
        List<CPlayerRogue> lTempAllPlayerRogue = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj;
        bool lTempbool = lTempAllPlayerRogue.Remove(RemovePlayerRogue);


        CountText();
        return lTempbool;
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

            lTempAllPlayerRogueIndex = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Count;
            m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj.Add(lTempPlayerRogue);
            lTempPlayerRogue.MyAddList(lTempAllPlayerRogueIndex);
            lTempPlayerRogue.SetTargetPos(m_PlayerRogueGroupMemoryShare.m_TargetPositionList[lTempAllPlayerRogueIndex], m_PlayerRogueUpdatapos);
        }

        CountText();
    }

    public void RemovePlayerRogue(CPlayerRogue RemoveRogue)
    {
        RemoveRogue.MyRemove();

        for (int i = 0; i < AllPlayerRoguePool.CurAllObjCount; i++)
        {
            if (AllPlayerRoguePool.AllCurObj[i].CurState == StaticGlobalDel.EMovableState.eDeath)
                return;
        }

        List<CPlayerRogue> lTempAllPlayerRogue = m_PlayerRogueGroupMemoryShare.m_AllPlayerRogueObj;

        for (int i = 0; i < lTempAllPlayerRogue.Count; i++)
            lTempAllPlayerRogue[i].SetTargetupdatePos(m_PlayerRogueGroupMemoryShare.m_TargetPositionList[i]);

        CountText();
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

        base.OnTriggerEnter(other);
    }
}

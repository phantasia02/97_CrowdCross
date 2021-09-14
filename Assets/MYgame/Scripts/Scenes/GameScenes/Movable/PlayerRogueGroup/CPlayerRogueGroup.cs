using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Dreamteck.Splines;

//public class CPlayerRogueAndTarget
//{
//    public CPlayerRogue m_PlayerRogue = null;
//    public Transform    m_TargetDummy = null;
//}

public class CPlayerRogueGroupMemoryShare : CMemoryShareBase
{
    public bool                         m_bDown                      = false;
    public Vector3                      m_OldMouseDownPos            = Vector3.zero;
    public CPlayerRogueGroup            m_PlayerRogueGroup           = null;
    public CObjPool<CPlayerRogue>       m_AllPlayerRoguePool         = new CObjPool<CPlayerRogue>();
    public Transform                    m_AllPlayerRogueTransform    = null;
    public Transform                    m_AllTargetDummyTransform    = null;
    public SplineFollower               m_DummyCameraFollwer         = null;
    public SplineFollower               m_MySplineFollower           = null;
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

    [SerializeField] SplineFollower m_DummyCameraFollwer = null;
    [SerializeField] [Range(1, 300)] protected int m_InitCurListCount = 1;
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
        m_PlayerRogueGroupMemoryShare.m_MySplineFollower = this.gameObject.GetComponent<SplineFollower>();
        m_PlayerRogueGroupMemoryShare.m_DummyCameraFollwer = m_DummyCameraFollwer;

        SetBaseMemoryShare();

        CObjPool<CPlayerRogue> lTempAllPlayerRoguePool = m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool;
        List<Vector3> targetPositionList = GetPositionListAround(this.transform.position, new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f }, new int[] { 8, 20, 30, 50, 70, 100 });

        lTempAllPlayerRoguePool.NewObjFunc = NewPlayerRogue;
        lTempAllPlayerRoguePool.RemoveObjFunc = (CPlayerRogue RemoveRogue) => { RemoveRogue.MyRemove(); };
        lTempAllPlayerRoguePool.AddListObjFunc = (CPlayerRogue AddRogue, int index) =>
      {
          AddRogue.MyAddList(index);
          AddRogue.SetTargetPos(targetPositionList[index], m_PlayerRogueUpdatapos);
      };

        lTempAllPlayerRoguePool.InitDefPool(CstInitQueueCount);

        for (int i = 0; i < m_InitCurListCount; i++)
            lTempAllPlayerRoguePool.AddObj();

        ResetMoveBuff(true);
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

    public void UpdateSpeed()
    {
        if (m_MyMemoryShare.m_TotleSpeed != m_MyMemoryShare.m_TargetTotleSpeed)
        {
            m_MyMemoryShare.m_TotleSpeed = Mathf.Lerp(m_MyMemoryShare.m_TotleSpeed, m_MyMemoryShare.m_TargetTotleSpeed, 3.0f * Time.deltaTime);

            if (Mathf.Abs(m_MyMemoryShare.m_TotleSpeed - m_MyMemoryShare.m_TargetTotleSpeed) < 0.001f)
                m_MyMemoryShare.m_TotleSpeed = m_MyMemoryShare.m_TargetTotleSpeed;

            m_PlayerRogueGroupMemoryShare.m_MySplineFollower.followSpeed = m_MyMemoryShare.m_TotleSpeed;
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
        const float CfHalfWidth = 3.0f;
        const float CfTotleWidth = CfHalfWidth * 2.0f;
        float lTempMoveX = Input.mousePosition.x - m_PlayerRogueGroupMemoryShare.m_OldMouseDownPos.x;
        //float lTempMoveRatio = TotleSpeedRatio;

        //lTempMoveX = (lTempMoveX / Screen.width) * CfTotleWidth;

        //Vector3 lTempOffset = this.transform.position;
        //lTempOffset.x += lTempMoveX * lTempMoveRatio;

        //lTempOffset.x = Mathf.Clamp(lTempOffset.x, -CfHalfWidth, CfHalfWidth);
        
        //this.transform.position = lTempOffset;


        lTempMoveX = (lTempMoveX / Screen.width) * CfTotleWidth;
        Vector2 lTempOffset = m_PlayerRogueGroupMemoryShare.m_MySplineFollower.motion.offset;
       // lTempOffset.x += lTempMoveX * lTempMoveRatio;
        lTempOffset.x += lTempMoveX;
        lTempOffset = Vector2.ClampMagnitude(lTempOffset, CfHalfWidth);

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

    public void SetAllPlayerRogueState(StaticGlobalDel.EMovableState setState)
    {
        CObjPool<CPlayerRogue> lTempAllPlayerRoguePool = m_PlayerRogueGroupMemoryShare.m_AllPlayerRoguePool;

        for (int i = 0; i < lTempAllPlayerRoguePool.CurAllObjCount; i++)
            lTempAllPlayerRoguePool.AllCurObj[i].ChangState = setState;
    }

    public void updateFollwer() { m_PlayerRogueGroupMemoryShare.m_DummyCameraFollwer.SetPercent(m_PlayerRogueGroupMemoryShare.m_MySplineFollower.modifiedResult.percent); }

    private void UpdateCurSpeed()
    {
        m_MyMemoryShare.m_TotleSpeed = m_MyMemoryShare.m_TargetTotleSpeed;
        m_PlayerRogueGroupMemoryShare.m_MySplineFollower.followSpeed = m_MyMemoryShare.m_TargetTotleSpeed;
    }

    public void SetMoveBuff(ESpeedBuff type, float ratio, bool updateCurSpeed = false)
    {
        m_MyMemoryShare.m_Buff[(int)type] = ratio;
        float lTempMoveRatio = 1.0f;

        for (int i = 0; i < m_MyMemoryShare.m_Buff.Length; i++)
            lTempMoveRatio *= m_MyMemoryShare.m_Buff[i];

        m_MyMemoryShare.m_TargetTotleSpeed = StaticGlobalDel.g_DefMovableTotleSpeed * lTempMoveRatio;
        //m_MyMemoryShare.m_TotleSpeed 
        if (updateCurSpeed)
            UpdateCurSpeed();
    }

    public void ResetMoveBuff(bool updateCurSpeed = false)
    {
        for (int i = 0; i < m_MyMemoryShare.m_Buff.Length; i++)
            m_MyMemoryShare.m_Buff[i] = 1.0f;

        m_MyMemoryShare.m_TargetTotleSpeed = StaticGlobalDel.g_DefMovableTotleSpeed;
        if (updateCurSpeed)
            UpdateCurSpeed();
    }
}

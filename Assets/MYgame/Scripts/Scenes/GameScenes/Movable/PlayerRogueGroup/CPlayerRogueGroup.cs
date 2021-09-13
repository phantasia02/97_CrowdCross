using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CPlayerRogueGroupMemoryShare : CMemoryShareBase
{
    public CPlayerRogueGroup    m_PlayerRogueGroup      = null;
    public List<GameObject>     m_AllPlayerRogue        = new List<GameObject>();
    public Queue                m_AllPlayerRoguePool    = new Queue();
};

public class CPlayerRogueGroup : CMovableBase
{
    protected CPlayerRogueGroupMemoryShare m_PlayerRogueGroupMemoryShare = null;

    public override EMovableType MyMovableType() { return EMovableType.ePlayerRogueGroup; }


    [SerializeField] CinemachineVirtualCamera m_PlayerNormalCamera = null;
    public CinemachineVirtualCamera PlayerNormalFollowObj { get { return m_PlayerNormalCamera; } }

    [SerializeField] CinemachineVirtualCamera m_PlayerWinLoseCamera = null;
    public CinemachineVirtualCamera PlayerWinLoseCamera { get { return m_PlayerWinLoseCamera; } }
    //protected override void AddInitState()
    //{
    //}


    protected override void CreateMemoryShare()
    {
        m_PlayerRogueGroupMemoryShare = new CPlayerRogueGroupMemoryShare();
        m_MyMemoryShare = m_PlayerRogueGroupMemoryShare;
        m_PlayerRogueGroupMemoryShare.m_PlayerRogueGroup = this;


        SetBaseMemoryShare();

        List<Vector3> targetPositionList = GetPositionListAround(this.transform.position, new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f }, new int[] { 8, 20, 30, 50, 70, 100 });


        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;

        for (var i = 0; i < 100; i++)
        {
            GameObject step = Instantiate(lTempCGGameSceneData.m_AllOtherObj[(int)CGGameSceneData.EOtherObj.ePlayerRogue], targetPositionList[i], this.transform.rotation, this.transform);
            m_PlayerRogueGroupMemoryShare.m_AllPlayerRogue.Add(step);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
       // SetCurState(StaticGlobalDel.EMovableState.eWait);
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
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
}

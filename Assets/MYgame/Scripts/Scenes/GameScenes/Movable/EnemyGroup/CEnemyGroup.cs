using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyGroup : CGameObjBas
{
    public override EObjType ObjType() { return EObjType.eEnemyGroup; }

    [SerializeField] protected int m_EnemyCount = 1;
    protected float m_RingDis = 30.0f;
    public List<CEnemy> m_AllEnemy = new List<CEnemy>();
    public List<CEnemy> AllEnemy { get { return m_AllEnemy; } }
    
    protected override void Awake()
    {
        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;
        GameObject lpototypeObj = null;

        float m_DoubleRingDis = m_RingDis * 2.0f;
        float lTempRadiusX_Radius = m_DoubleRingDis / (float)(m_EnemyCount + 1);
        float lTempXRandom_Pos = lTempRadiusX_Radius / 4.0f;
        Vector3 lTempV3 = this.transform.position;
        Vector3 EnemyPos = Vector3.zero;
        CEnemy lTempEnemy = null;

        //for (int i = 0; i < m_EnemyCount; i++)
        //{
        //    EnemyPos = this.transform.position
        //            + this.transform.right * ((lTempRadiusX_Radius * (i + 1)) - m_RingDis + Random.Range(-lTempXRandom_Pos, lTempXRandom_Pos))
        //            + this.transform.forward * Random.Range(-1.0f, 1.0f);

        //    EnemyPos.y = this.transform.position.y;

        //    lpototypeObj = Instantiate(lTempCGGameSceneData.m_AllOtherObj[(int)CGGameSceneData.EOtherObj.eEnemy]);
        //    lpototypeObj.transform.parent   = this.transform;
        //    lpototypeObj.transform.rotation = this.transform.rotation;
        //    lpototypeObj.transform.position = EnemyPos;

        //    lTempEnemy = lpototypeObj.GetComponent<CEnemy>();
        //    m_AllEnemy.Add(lTempEnemy);
        //}

        List<CTargetPositionData> lTempTargetPositionData = StaticGlobalDel.GetPositionListAround(this.transform.position,
            new float[] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f }, new int[] { 8, 20, 30, 50, 70, 100, 130, 160, 200 });

        for (int i = 0; i < m_EnemyCount; i++)
        {
            lpototypeObj = Instantiate(lTempCGGameSceneData.m_AllOtherObj[(int)CGGameSceneData.EOtherObj.eEnemy], this.transform);
            lpototypeObj.transform.rotation = this.transform.rotation;
            lpototypeObj.transform.position = lTempTargetPositionData[i].m_TargetPosition;
            lTempEnemy = lpototypeObj.GetComponent<CEnemy>();
            m_AllEnemy.Add(lTempEnemy);
        }
    }


    public CEnemy GetIndexToEnemy(int index)
    {
        if (index < 0)
            return null;

        index = index % m_AllEnemy.Count;
        return m_AllEnemy[index];
    }

    public bool RemoveEnemy(CEnemy remove)
    {
        return m_AllEnemy.Remove(remove);
    }


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}

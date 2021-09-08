using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyMemoryShare : CMemoryShareBase
{
    public CEnemy       m_MyEnemy       = null;
    public GameObject   m_ArrowObj      = null;
    public Transform    m_ArrowRefPoint = null;
};

public class CEnemy : CActor
{

    protected CEnemyMemoryShare m_MyEnemyMemoryShare = null;

    [SerializeField] GameObject m_ArrowObj = null;
    [SerializeField] Transform  m_ArrowRefPoint = null;


    protected override void AddInitState()
    {

        //m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStateEnemy(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStateEnemy(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eAtk].AllThisState.Add(new CATKStateEnemy(this));
    }


    protected override void CreateMemoryShare()
    {
        m_MyEnemyMemoryShare = new CEnemyMemoryShare();
        m_MyMemoryShare = m_MyEnemyMemoryShare;
        m_MyEnemyMemoryShare.m_MyEnemy = this;
        m_MyEnemyMemoryShare.m_ArrowObj = m_ArrowObj;
        m_MyEnemyMemoryShare.m_ArrowRefPoint = m_ArrowRefPoint;

        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SetCurState(StaticGlobalDel.EMovableState.eWait);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyMemoryShare : CActorMemoryShare
{
    public CEnemy m_MyEnemy = null;
    
};

public class CEnemy : CActor
{
    public override EMovableType MyMovableType() { return EMovableType.eEnemy; }

    protected CEnemyMemoryShare m_MyEnemyMemoryShare = null;

    

    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStateEnemy(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayerRogue(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayerRogue(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CTargetMoveStateActor(this));         // eMove index 0
    }

    protected override void CreateMemoryShare()
    {
        m_MyEnemyMemoryShare = new CEnemyMemoryShare();
        m_MyMemoryShare = m_MyActorMemoryShare = m_MyEnemyMemoryShare;
        m_MyEnemyMemoryShare.m_MyEnemy = this;

        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // SetCurState(StaticGlobalDel.EMovableState.eWait);
    }

    public bool SetTarget(CPlayerRogue target)
    {
        m_MyEnemyMemoryShare.m_Target = target;
        return true;
    }
}

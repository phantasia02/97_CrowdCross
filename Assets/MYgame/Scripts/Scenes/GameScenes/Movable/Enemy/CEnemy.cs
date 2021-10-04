using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyMemoryShare : CActorMemoryShare
{
    public CEnemy m_MyEnemy = null;
    public CEnemyGroup m_MyEnemyGroup = null;
    
};

public class CEnemy : CActor
{
    public override EMovableType MyMovableType() { return EMovableType.eEnemy; }

    protected CEnemyMemoryShare m_MyEnemyMemoryShare = null;
    public override int TargetMask() { return (int)StaticGlobalDel.g_EndPlayerActorMask; }


    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStateEnemy(this));
        //m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayerRogue(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStateActor(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CTargetMoveStateActor(this));         // eMove index 0

        m_AllState[(int)StaticGlobalDel.EMovableState.eAtk].AllThisState.Add(new CAtkStateActor(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyEnemyMemoryShare = new CEnemyMemoryShare();
        m_MyMemoryShare = m_MyActorMemoryShare = m_MyEnemyMemoryShare;
        m_MyActorMemoryShare.m_MyActor = m_MyEnemyMemoryShare.m_MyEnemy = this;
       

        SetBaseMemoryShare();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_MyEnemyMemoryShare.m_MyEnemyGroup = this.GetComponentInParent<CEnemyGroup>();

        // SetCurState(StaticGlobalDel.EMovableState.eWait);
    }

    public bool SetTarget(CPlayerRogue target)
    {
        m_MyEnemyMemoryShare.m_Target = target;
        return true;
    }

    public override void RemoveGroup()
    {
        m_MyEnemyMemoryShare.m_MyEnemyGroup.RemoveEnemy(this);
        base.RemoveGroup();
    }

}

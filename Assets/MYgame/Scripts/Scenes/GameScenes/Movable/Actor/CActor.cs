using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CActorMemoryShare : CMemoryShareBase
{
    public ActorType m_ActorTypeData    = null;
    public CActor m_Target              = null;
    public CActor m_MyActor             = null;
    public int m_Hp                     = 3;
};

public abstract class CActor : CMovableBase
{
    //[SerializeField] protected Transform m_MyFloorStartPoint = null;
    //public Transform MyFloorStartPoint { get { return m_MyFloorStartPoint; } }
    //  abstract public EMovableType MyMovableType();
    protected CActorMemoryShare m_MyActorMemoryShare = null;
    [SerializeField] ActorType m_ActorData = null;

    public int ActorTypeDataHp
    {
        set { m_MyActorMemoryShare.m_Hp = value; }
        get { return m_MyActorMemoryShare.m_Hp; }
    }

    public CActor m_wachTarget = null;

    protected override void Start()
    {
        base.Start();
        m_MyActorMemoryShare.m_ActorTypeData = m_ActorData;
    }

    public bool SetTarget(CActor target)
    {
        m_MyActorMemoryShare.m_Target = target;
        m_wachTarget = m_MyActorMemoryShare.m_Target;

        return true;
    }
}

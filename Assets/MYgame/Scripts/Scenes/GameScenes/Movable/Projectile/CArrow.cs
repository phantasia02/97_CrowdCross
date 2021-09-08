using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CArrowShare : CMemoryShareBase
{
   
    public CArrow       m_MyArrow           = null;
    public Vector3      m_Targetpos         = Vector3.forward;
    public Transform    m_ParentTransform   = null;
 
};

public class CArrow : CMovableBase
{
    protected CArrowShare m_MyArrowShare = null;

    protected Transform m_ParentTransform = null;
    public Transform ParentTransform
    {
        set { m_ParentTransform = value; }
        get { return m_ParentTransform; }
    }

    protected override bool AutoAwake() { return false; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void AddInitState()
    {
      //  m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStateArrow(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyArrowShare = new CArrowShare();
        m_MyMemoryShare = m_MyArrowShare;
        m_MyArrowShare.m_MyArrow = this;
        m_MyArrowShare.m_ParentTransform = m_ParentTransform;
        m_MyArrowShare.m_Targetpos = m_MyArrowShare.m_ParentTransform.position;
        m_MyArrowShare.m_Targetpos.y += 1.0f;
        m_MyArrowShare.m_Targetpos.x -= 20.0f;

        SetBaseMemoryShare();
    }
}

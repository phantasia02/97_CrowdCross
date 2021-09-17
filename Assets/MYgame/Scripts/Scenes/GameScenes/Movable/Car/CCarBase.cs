using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CCarBaseMemoryShare : CMemoryShareBase
{
    public CCarBase m_CarBase = null;
    public CCarCreatePos m_MyCarCreatePos = null;
    public CAllCarCreatePos m_MyAllCarCreatePos = null;
    public int m_CarCreatePosindex = -1;
};

public abstract class CCarBase : CMovableBase
{
    [SerializeField] protected float m_DefSpeed = StaticGlobalDel.g_DefMovableTotleSpeed;
    public override float DefSpeed { get { return m_DefSpeed; } }

    [SerializeField] protected float m_CarLong = 1.0f;
    public float CarLong { get { return m_CarLong; } }

    protected CCarBaseMemoryShare m_MyCarBaseMemoryShare = null;

    public CCarCreatePos MyCarCreatePos { get { return m_MyCarBaseMemoryShare.m_MyCarCreatePos; } }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 a = transform.position;
        Vector3 b = transform.position + (-transform.forward * m_CarLong);

        Handles.DrawAAPolyLine(a, b);

        // 畫固定大小的球  不會因為 攝影機遠近 影響的球
        void DrawSphere(Vector3 p)
        {
            Gizmos.DrawSphere(p, HandleUtility.GetHandleSize(p));
        }

        DrawSphere(a);
        DrawSphere(b);
    }
#endif

    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStateNormalCar(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyCarBaseMemoryShare = new CCarBaseMemoryShare();
        m_MyMemoryShare = m_MyCarBaseMemoryShare;
        m_MyCarBaseMemoryShare.m_CarBase = this;

        SetBaseMemoryShare();
    }

    public void AddToList(int index)
    {
        this.gameObject.SetActive(true);
        m_MyCarBaseMemoryShare.m_MyCarCreatePos = this.GetComponentInParent<CCarCreatePos>();
        m_MyCarBaseMemoryShare.m_CarCreatePosindex = index;
        ChangState = StaticGlobalDel.EMovableState.eMove;
    }

    public void MyRemove()
    {
 
        SetCurState(StaticGlobalDel.EMovableState.eDeath);
        m_MyCarBaseMemoryShare.m_MyCarCreatePos = null;
        m_MyCarBaseMemoryShare.m_CarCreatePosindex = -1;
        this.gameObject.SetActive(false);

    }

    public void RemovCarPool()
    {
        MyCarCreatePos.RemoveCar(this);
        m_MyCarBaseMemoryShare.m_MyAllCarCreatePos.AllCarBasePool.RemoveObj(this);
    }

    public override void OnTriggerEnter(Collider other)
    {
        //if (other.tag == )

        base.OnTriggerEnter(other);
    }
}

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

    public enum ECarModIndex
    {
        eNormalCar  = 0,
        eSpeedCar   = 1,
        eMax
    }


    protected float m_DefSpeed = StaticGlobalDel.g_DefMovableTotleSpeed;
    public override float DefSpeed { get { return m_DefSpeed; } }

    [SerializeField] protected CCarMod[] m_AllCarMod = null;

    protected float m_CarLong = 1.0f;
    public float CarLong { get { return m_CarLong; } }

    protected CCarBaseMemoryShare m_MyCarBaseMemoryShare = null;

    public CCarCreatePos MyCarCreatePos { get { return m_MyCarBaseMemoryShare.m_MyCarCreatePos; } }

    

    protected override void AddInitState()
    {
        
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStateNormalCar(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyCarBaseMemoryShare = new CCarBaseMemoryShare();
        m_MyMemoryShare = m_MyCarBaseMemoryShare;
        m_MyCarBaseMemoryShare.m_CarBase = this;

        m_MyCarBaseMemoryShare.m_MyAllCarCreatePos = this.GetComponentInParent<CAllCarCreatePos>();

        SetBaseMemoryShare();
    }

    public void AddToList(int index)
    {
        this.gameObject.SetActive(true);
        m_MyCarBaseMemoryShare.m_MyCarCreatePos = this.GetComponentInParent<CCarCreatePos>();
        m_MyCarBaseMemoryShare.m_CarCreatePosindex = index;
        ChangState = StaticGlobalDel.EMovableState.eMove;
    }

    public void SetCarMod(ECarModIndex Eindex)
    {
        int index = (int)Eindex;
        for (int i = 0; i < m_AllCarMod.Length; i++)
            m_AllCarMod[i].gameObject.SetActive(false);

        m_AllCarMod[index].gameObject.SetActive(true);
        m_DefSpeed = m_AllCarMod[index].DefSpeed;
        ResetMoveBuff(true);
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
        if (other.tag == StaticGlobalDel.TagCarEnd)
            RemovCarPool();


       // base.OnTriggerEnter(other);
    }
}

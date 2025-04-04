using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CCarBaseMemoryShare : CMemoryShareBase
{
    public CCarBase         m_CarBase           = null;
    public CCarBase         m_ForwardCarBase    = null;
    public CCarBase         m_NextCarBase       = null;
    public CCarCreatePos    m_MyCarCreatePos    = null;
    public CAllCarCreatePos m_MyAllCarCreatePos = null;
    public int              m_CarCreatePosindex = -1;
    public bool             m_InTrafficLightBox = false;
};

public abstract class CCarBase : CMovableBase
{

#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    Handles.color = Color.red;
    //    Vector3 pos = transform.position;
    //    Handles.DrawAAPolyLine(6, pos, pos + transform.forward * 3.0f);
    //    Handles.color = Color.white;
    //}
#endif

    public enum ECarModIndex
    {
        eNormalCar  = 0,
        eSpeedCar   = 1,
        eMax
    }


    protected float m_DefSpeed = StaticGlobalDel.g_DefMovableTotleSpeed;
    public override float DefSpeed { get { return m_DefSpeed; } }

    [SerializeField] protected CCarMod[] m_AllCarMod = null;
    [SerializeField] protected Transform m_RaycastTransform = null;

    protected float m_CarLong = 1.0f;
    public float CarLong { get { return m_CarLong; } }

    protected CCarBaseMemoryShare m_MyCarBaseMemoryShare = null;

    public CCarCreatePos MyCarCreatePos { get { return m_MyCarBaseMemoryShare.m_MyCarCreatePos; } }

    public CCarBase ForwardCarBase
    {
        set { m_MyCarBaseMemoryShare.m_ForwardCarBase = value; }
        get { return m_MyCarBaseMemoryShare.m_ForwardCarBase; }
    }

    public CCarBase NextCarBase
    {
        set { m_MyCarBaseMemoryShare.m_NextCarBase = value; }
        get { return m_MyCarBaseMemoryShare.m_NextCarBase; }
    }

    public bool InTrafficLightBox
    {
        set { m_MyCarBaseMemoryShare.m_InTrafficLightBox = value; }
        get { return m_MyCarBaseMemoryShare.m_InTrafficLightBox; }
    }

    protected CTrafficLightGroup.ELightIndex m_CurTrafficLight = CTrafficLightGroup.ELightIndex.eMax;
    public CTrafficLightGroup.ELightIndex CurTrafficLight{set{m_CurTrafficLight = value;}}

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
        if (m_MyCarBaseMemoryShare.m_NextCarBase != null)
        {
            m_MyCarBaseMemoryShare.m_NextCarBase.ForwardCarBase = null;
            m_MyCarBaseMemoryShare.m_NextCarBase = null;
        }
 
        SetCurState(StaticGlobalDel.EMovableState.eDeath);
        m_MyCarBaseMemoryShare.m_MyCarCreatePos = null;
        m_MyCarBaseMemoryShare.m_ForwardCarBase = null;
        m_MyCarBaseMemoryShare.m_InTrafficLightBox = false;
        m_CurTrafficLight = CTrafficLightGroup.ELightIndex.eMax;
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

    public void UpdateSpeed()
    {
        if (m_MyMemoryShare.m_TotleSpeed != m_MyMemoryShare.m_TargetTotleSpeed)
        {
            m_MyMemoryShare.m_TotleSpeed = Mathf.Lerp(m_MyMemoryShare.m_TotleSpeed, m_MyMemoryShare.m_TargetTotleSpeed, 10.0f * Time.deltaTime);
            if (Mathf.Abs(m_MyMemoryShare.m_TotleSpeed - m_MyMemoryShare.m_TargetTotleSpeed) < 0.01f)
                m_MyMemoryShare.m_TotleSpeed = m_MyMemoryShare.m_TargetTotleSpeed;
        }
    }

    public void ForwardObjCheck()
    {
        bool lTempStopRaycast = false;
        Ray Tempray = new Ray(m_RaycastTransform.position, this.transform.forward);
        bool lTempRaycast = Physics.Raycast(Tempray, out RaycastHit hit, m_CarLong + (m_DefSpeed * 0.1f));
        if (lTempRaycast)
        {
            if ((m_CurTrafficLight == CTrafficLightGroup.ELightIndex.eRed || m_CurTrafficLight == CTrafficLightGroup.ELightIndex.eYellow) && 
                !InTrafficLightBox && 
                hit.collider.gameObject.tag == StaticGlobalDel.TagTrafficLightGroup)
            {
                    lTempStopRaycast = true;
            }

            if (hit.collider.gameObject.tag == StaticGlobalDel.TagCarCollider)
                lTempStopRaycast = true;

        }

        if (lTempStopRaycast)
            SetMoveBuff(ESpeedBuff.eCar, 0.0f);
        else
        {
            if (m_MyMemoryShare.m_TargetTotleSpeed <= 0.1f)
                ResetMoveBuff();
        }
    }
}

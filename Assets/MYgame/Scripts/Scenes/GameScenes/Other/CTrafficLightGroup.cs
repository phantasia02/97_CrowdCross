using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class CTrafficLightGroup : MonoBehaviour
{
    public enum ELightIndex
    {
        eRed        = 0,
        eYellow     = 1,
        eGreen      = 2,
        eMax
    }

    protected ELightIndex m_CurLightIndex = ELightIndex.eGreen;
    protected CTrafficLightObj[] m_AllTrafficLightObj = null;
    protected int m_CarCount = 0;
    [SerializeField] protected CCarCreatePos[] m_AllCarCreatePos = null;
    //protected List<CCarBase> m_TriggerInCar = new List<CCarBase>();
    protected UniRx.ReactiveCollection<CCarBase> m_ReactiveTriggerInCar = new UniRx.ReactiveCollection<CCarBase>();
   // protected UniRx.ReactiveProperty<int> m_ReactiveTriggerInCarCount = new UniRx.ReactiveProperty<int>();

    private void Awake()
    {
        m_AllTrafficLightObj = this.GetComponentsInChildren<CTrafficLightObj>();

        SetState(ELightIndex.eGreen);

        m_ReactiveTriggerInCar.ObserveRemove().Subscribe(_ =>
        {
            if (m_CurLightIndex == ELightIndex.eYellow)
            {
                if (m_ReactiveTriggerInCar.Count == 0)
                    SetState(ELightIndex.eRed);
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetState(ELightIndex lsetLightIndex)
    {

        ELightIndex lOldLightIndex = m_CurLightIndex;
        //m_StateTime = 0.0f;
        //m_StateCount = 0;
        //m_StateUnscaledTime = 0.0f;
        m_CurLightIndex = lsetLightIndex;

        CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;

        switch (m_CurLightIndex)
        {
            case ELightIndex.eRed:
                {
                    UniRx.Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_=> { SetState(ELightIndex.eGreen); });
                }
                break;
            case ELightIndex.eYellow:
                {

                }
                break;
            case ELightIndex.eGreen:
                {
                    UniRx.Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => 
                    {
                        if (m_ReactiveTriggerInCar.Count ==  0)
                            SetState(ELightIndex.eRed);
                        else
                            SetState(ELightIndex.eYellow);
                    });
                }
                break;
        }

        for (int i = 0; i < m_AllTrafficLightObj.Length; i++)
            m_AllTrafficLightObj[i].ShowLightIndex(m_CurLightIndex);


        for (int i = 0; i < m_AllCarCreatePos.Length; i++)
            m_AllCarCreatePos[i].CurTrafficLight = m_CurLightIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == StaticGlobalDel.TagCarCollider)
        {
            CCarBase lTempCarBase = other.gameObject.GetComponentInParent<CCarBase>();
            m_ReactiveTriggerInCar.Add(lTempCarBase);
        }

      //  m_TriggerInCar.Add();
        m_CarCount++;
        //Debug.Log("OnTriggerEnter 1111111111111");
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagCarCollider)
        {
            CCarBase lTempCarBase = other.gameObject.GetComponentInParent<CCarBase>();
            m_ReactiveTriggerInCar.Remove(lTempCarBase);
        }

        m_CarCount--;
      //  Debug.Log("OnTriggerExit 222222222222222222");
    }

    // ===================== UniRx ======================
   
    //public void OnUpdatePlayerRogueCount(int value)
    //{
    //    //if (m_ReactiveTriggerInCar != null)
    //    //    m_PlayerRogueCountEvent.OnNext(value);
    //}

    //public UniRx.ReactiveProperty<List<CCarBase>> ReactiveTriggerInCar()
    //{
    //    return m_ReactiveTriggerInCar ?? (m_ReactiveTriggerInCar = new UniRx.ReactiveProperty<List<CCarBase>>());
    //}

    // ===================== UniRx ======================
}

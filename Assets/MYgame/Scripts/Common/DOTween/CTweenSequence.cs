using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class CTweenSequence : MonoBehaviour
{
    [SerializeField] protected GameObject m_MyGameObject = null;
    public List<DOTweenModTest> m_ListDOTween;
    Sequence m_MySequence;
    CanvasGroup m_MyCanvasGroup = null;
    Renderer m_MyRenderer;

    private void Awake()
    {

    }

    public void BuildSequence()
    {
        if (m_MyGameObject == null)
            m_MyGameObject = this.gameObject;

        m_MyCanvasGroup     = m_MyGameObject.GetComponent<CanvasGroup>();
        m_MyRenderer        = m_MyGameObject.GetComponent<Renderer>();
        m_MySequence        = DOTween.Sequence();
        m_MySequence.Pause();
        m_MySequence.SetAutoKill(false);

        for (int i = 0; i < m_ListDOTween.Count; i++)
        {
            if (m_ListDOTween[i] != null)
            {
                m_ListDOTween[i].MyTransform = m_MyGameObject.transform;
                //if (m_ListDOTween[i].m_ELoopTweenType != DOTweenModTest.ELoopType.eNull)
                //{
                if (m_ListDOTween[i].m_MySequenceType == DOTweenModTest.ESequenceType.eAppend)
                {
                    if (m_ListDOTween[i].m_TweenType == DOTweenModTest.ETweenType.eCanvasGroupAlpha || m_ListDOTween[i].m_TweenType == DOTweenModTest.ETweenType.eMaterialAlpha)
                        AddAppendCallbackAlphaZero(m_ListDOTween[i]);

                    m_MySequence.Append(m_ListDOTween[i].GetTween());
                }
                else if (m_ListDOTween[i].m_MySequenceType == DOTweenModTest.ESequenceType.eJoin)
                {
                    m_MySequence.Join(m_ListDOTween[i].GetTween());
                }
                else if (m_ListDOTween[i].m_MySequenceType == DOTweenModTest.ESequenceType.eInterval)
                    m_MySequence.AppendInterval(m_ListDOTween[i].m_StartTime);
                else if (m_ListDOTween[i].m_MySequenceType == DOTweenModTest.ESequenceType.eInsert)
                    m_MySequence.Insert(m_ListDOTween[i].m_StartTime, m_ListDOTween[i].GetTween());
                else if (m_ListDOTween[i].m_MySequenceType == DOTweenModTest.ESequenceType.eLoop)
                    m_ListDOTween[i].GetTween();
                else if (m_ListDOTween[i].m_MySequenceType == DOTweenModTest.ESequenceType.eCallback)
                {
                    DOTweenModTest lTempDOTweenModTest = m_ListDOTween[i];
                    m_MySequence.AppendCallback(() => { lTempDOTweenModTest.FuncCallBack(); });
                }

                //else if (m_ListDOTween[i].m_MySequenceType == DOTweenModTest.ESequenceType.eCallback && m_ListDOTween[i].m_UnityEvent != null)
                //    m_MySequence.AppendCallback(m_ListDOTween[i].FuncTest);

                // }
                //else
                //    m_ListDOTween[i].GetTween();
            }
        }
    }

    void AddAppendCallbackAlphaZero(DOTweenModTest lTempDOTweenModTest)
    {
        m_MySequence.AppendCallback(() =>
        {
            if (lTempDOTweenModTest.m_TweenType == DOTweenModTest.ETweenType.eCanvasGroupAlpha && m_MyCanvasGroup != null)
                m_MyCanvasGroup.alpha = 0.0f;
            else if (lTempDOTweenModTest.m_TweenType == DOTweenModTest.ETweenType.eMaterialColorAlpha && m_MyRenderer != null)
            {
                Material lTempMaterial = m_MyRenderer.material;
                Color lTempColor = lTempMaterial.color;
                lTempColor.a = 0.0f;
                lTempMaterial.color = lTempColor;
                
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show()
    {

    }

    public void PlayForward(){ m_MySequence.PlayForward(); }
    public void ResetSequence(){m_MySequence.Restart();}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AAAA()
    {
        Debug.Log("Debug AAAAAAAAAAAAAAAAAAAAAAAAAAA");
    }
}

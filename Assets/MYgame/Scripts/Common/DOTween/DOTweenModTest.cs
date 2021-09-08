using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public delegate void TweenSequenceCallBack();


[System.Serializable]
public class DOTweenModTest
{
    

    public enum ESequenceType
    {
        eAppend         = 0,
        eJoin           = 1,
        eInsert         = 2,
        eInterval       = 3,
        eLoop           = 4,
        eCallback       = 5,
    }


    public enum ELoopType
    {
        eRestart        = 0,
        eYoyo           = 1,
        eIncremental    = 2,
    }

    public enum ETweenType
    {
        eMoveWordpos        = 0,
        eRotate             = 1,
        eScale              = 2,
        eLocalMovePos       = 3,
        eLocalRotate        = 4,
        eCanvasGroupAlpha   = 5,
        eMaterialColorAlpha = 6,
        eMaterialAlpha      = 7,
    }

    Renderer    m_MyRenderer;
    Transform   m_MyTransform;
    CanvasGroup m_MyCanvasGroup;
    public Transform MyTransform 
    {
        get { return m_MyTransform; }
        set
        {
            m_MyTransform   = value;
            m_MyRenderer    = m_MyTransform.gameObject.GetComponent<Renderer>();
            m_MyCanvasGroup = m_MyTransform.gameObject.GetComponent<CanvasGroup>();
        }
    }
    
    public ESequenceType                m_MySequenceType        = ESequenceType.eAppend;
    public ETweenType                   m_TweenType;
    public ELoopType                    m_ELoopTweenType        = ELoopType.eRestart;
    public AnimationCurve               m_MyAnimationCurve      = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    public Transform                    m_TargetTransform;
    public Vector3                      m_TargetV3              = Vector3.zero;
    public Color                        m_TargetColor           = Color.white;
    public float                        m_TargetAlpha           = 0.0f;
    public float                        m_Duration              = 1.0f;
    public float                        m_StartTime             = 0.0f;
    public int                          m_LoopCount             = -1;
    public UnityEvent                   m_UnityEvent            = null;
    public bool                         m_startZero             = false;
    public string                       m_MaterialEnableKeyword = "";
    public string                       m_MaterialKeywordToID   = "";
    public int                          m_shPropColorID           = 0;

    public DOTweenModTest()
    {
        
    }



    public Tween GetTween()
    {
        Tween lTempTween = null;

        if (!m_MyTransform)
        {
            Debug.LogError($"DOTweenModTest {m_MyTransform.gameObject.name} m_MyTransform = null");
            return lTempTween;
        }



        switch (m_TweenType)
        {
            case ETweenType.eMoveWordpos:
                { lTempTween = m_MyTransform.DOMove(m_TargetTransform.position, m_Duration).SetEase(m_MyAnimationCurve); }
                break;
            case ETweenType.eLocalMovePos:
                { lTempTween = m_MyTransform.DOLocalMove(m_MyTransform.localPosition + m_TargetV3, m_Duration).SetEase(m_MyAnimationCurve); }
                break;
            case ETweenType.eRotate:
                { lTempTween = m_MyTransform.DORotate(m_TargetV3, m_Duration).SetEase(m_MyAnimationCurve); }
                break;
            case ETweenType.eLocalRotate:
                { lTempTween = m_MyTransform.DOLocalRotate(m_TargetV3, m_Duration, RotateMode.FastBeyond360).SetEase(m_MyAnimationCurve); }
                break;
            case ETweenType.eScale:
                { lTempTween = m_MyTransform.DOScale(m_TargetV3, m_Duration).SetEase(m_MyAnimationCurve); }
                break;
            case ETweenType.eCanvasGroupAlpha:
                { lTempTween = m_MyCanvasGroup.DOFade(1.0f, m_Duration).SetEase(m_MyAnimationCurve); }
                break;
            case ETweenType.eMaterialColorAlpha:
                {
                    m_shPropColorID = Shader.PropertyToID(m_MaterialKeywordToID);
                    Material lTempMaterial = m_MyRenderer.materials[0];
                    lTempTween = lTempMaterial.DOColor(m_TargetColor, m_shPropColorID, m_Duration).SetEase(m_MyAnimationCurve);
    
                }
                break;
            case ETweenType.eMaterialAlpha:
                {
                    Material lTempMaterial = m_MyRenderer.material;
                    Color lTempColor = lTempMaterial.color;
                    lTempColor.a = 1.0f;
                    
                    lTempTween = lTempMaterial.DOColor(lTempColor, m_Duration);
                }
                break;
        }

        if (m_MySequenceType == ESequenceType.eLoop)
        {
            DG.Tweening.LoopType lTempLoopType = (LoopType)m_ELoopTweenType;
            lTempTween = lTempTween.SetLoops(m_LoopCount, lTempLoopType);
        }

        return lTempTween;
    }


    public void FuncCallBack ()
    {
        if (m_UnityEvent != null)
            m_UnityEvent.Invoke();
    }
}

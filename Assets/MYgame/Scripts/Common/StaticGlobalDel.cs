using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticGlobalDel 
{
    public enum EMovableState
    {
        eNull           = 0,
        eWait           = 1,
        eDrag           = 2,
        eMove           = 3,
        eAtk            = 4,
        eJump           = 5,
        eHit            = 6,
        eWin            = 7,
        eDeath          = 8,
        eMax
    }

    public enum ELayerIndex
    {
        eDragRange      = 8,
        eDragHeroGroup  = 9,
        eBridge         = 10,
        eDragCanNotPass = 11,
        eMax
    }

    // tag DragHeroGroup
    // tag CanNotPass
    // tag DragHeroBridge
    // tag PlayerArms
    // tag Arrow
    // tag BridgeFloor
    // tag EventSystem

    public const int    g_DragRangeMask         = 1 << (int)ELayerIndex.eDragRange;
    public const int    g_DragHeroGroupMask     = 1 << (int)ELayerIndex.eDragHeroGroup;
    public const int    g_BridgeMask            = 1 << (int)ELayerIndex.eBridge;
    public const int    g_DragCanNotPassMask    = 1 << (int)ELayerIndex.eDragCanNotPass;
    
    public const float  g_fcbaseWidth                   = 1242.0f;
    public const float  g_fcbaseHeight                  = 2688.0f;
    public const float  g_fcbaseOrthographicSize        = 18.75f;
    public const float  g_fcbaseResolutionWHRatio       = g_fcbaseWidth / g_fcbaseHeight;
    public const float  g_fcbaseResolutionHWRatio       = g_fcbaseHeight / g_fcbaseWidth;
    public const float  g_TUA                           = Mathf.PI * 2.0f;
    // ============= Speed ====================
    public const float g_DefMovableTotleSpeed = 20.0f;
    // ============= Hp ====================
    public const int g_DefHp = 10;
    public const int g_MaxHp = 20;
    public const int g_RefFXGoodHp = 10;
    public const int g_RefFXBadHp  = 10;
    public const float g_DefHpRatio = 0.5f;

    public static GameObject NewFxAddParentShow(this Transform ParentTransform, CGGameSceneData.EAllFXType Fxtype)
    {
        CGGameSceneData lTempGGameSceneData = CGGameSceneData.SharedInstance;
        GameObject lTempFx = GameObject.Instantiate(lTempGGameSceneData.m_AllFX[(int)Fxtype], ParentTransform);
        lTempFx.transform.position = ParentTransform.position;

        return lTempFx;
    }
}


public class CLerpTargetPos
{
    float m_TargetRange = 0.1f;
    public float TargetRange
    {
        get { return m_TargetRange; }
        set { m_TargetRange = value; }
    }

    bool m_bTargetOK = false;
    public bool TargetOK
    {
        get { return m_bTargetOK; }
        set { m_bTargetOK = value; }
    }

    Vector3 m_Targetv3 = Vector3.zero;
    public Vector3 Targetv3
    {
        get { return m_Targetv3; }
        set { m_Targetv3 = value; }
    }

    Transform m_CurTransform = null;
    public Transform CurTransform
    {
        get { return m_CurTransform; }
        set { m_CurTransform = value; }
    }

    public void UpdateTargetPos()
    {
        if (!TargetOK)
        {
            if (Vector3.SqrMagnitude(CurTransform.position - Targetv3) > TargetRange)
                CurTransform.position = Vector3.Lerp(CurTransform.position, Targetv3, 0.5f);
            else
            {
                CurTransform.position = Targetv3;
                TargetOK = true;
            }
        }
    }
}

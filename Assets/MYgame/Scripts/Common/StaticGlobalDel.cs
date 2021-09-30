using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticGlobalDel 
{
    public enum EBoolState
    {
        eTrue           = 0,
        eTruePlaying    = 1,
        eFlase          = 2,
        eFlasePlaying   = 3,
        eMax
    }


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
        ePlayerRogue    = 6,
        eCarCollider    = 7,
        eEndActor       = 8,
        eMax
    }

    public const string TagDoorGroup        = "DoorGroup";
    public const string TagCarEnd           = "CarEnd";
    public const string TagCarCollider      = "TagCarCollider";
    public const string TagPlayerRogueGroup = "PlayerRogueGroupTag";
    public const string TagEndResult        = "EndResult";
    public const string TagPlayerRogue      = "TagPlayerRogue";
    public const string TagEnemy            = "TagEnemy";

    public const int g_PlayerRogueMask  = 1 << (int)ELayerIndex.ePlayerRogue;
    public const int g_CarColliderMask  = 1 << (int)ELayerIndex.eCarCollider;
    public const int g_EndActorMask     = 1 << (int)ELayerIndex.eEndActor;
    
    public const float  g_fcbaseWidth                   = 1080.0f;
    public const float  g_fcbaseHeight                  = 2340.0f;
    public const float  g_fcbaseOrthographicSize        = 18.75f;
    public const float  g_fcbaseResolutionWHRatio       = g_fcbaseWidth / g_fcbaseHeight;
    public const float  g_fcbaseResolutionHWRatio       = g_fcbaseHeight / g_fcbaseWidth;
    public const float  g_TUA                           = Mathf.PI * 2.0f;
    // ============= Speed ====================
    public const float g_DefMovableTotleSpeed = 20.0f;

    public static GameObject NewFxAddParentShow(this Transform ParentTransform, CGGameSceneData.EAllFXType Fxtype)
    {
        CGGameSceneData lTempGGameSceneData = CGGameSceneData.SharedInstance;
        GameObject lTempFx = GameObject.Instantiate(lTempGGameSceneData.m_AllFX[(int)Fxtype], ParentTransform);
        lTempFx.transform.position = ParentTransform.position;

        return lTempFx;
    }

    public static void SetMaterialRenderingMode(this Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                //  material.("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
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

public class CObjPool<T>
{

    protected List<T>  m_AllCurObj     = new List<T>();
    public List<T>  AllCurObj { get { return m_AllCurObj; } }
    public int CurAllObjCount { get { return m_AllCurObj.Count; } }

    protected Queue<T> m_AllObjPool    = new Queue<T>();

    public delegate T NewObjDelegate();
    protected NewObjDelegate m_NewObjFunc = null;
    public NewObjDelegate NewObjFunc{set { m_NewObjFunc = value; }}

    public delegate void RemoveObjDelegate(T Obj);
    protected RemoveObjDelegate m_RemoveObjFunc = null;
    public RemoveObjDelegate RemoveObjFunc { set { m_RemoveObjFunc = value; } }

    public delegate void AddListObjDelegate(T Obj, int index);
    protected AddListObjDelegate m_AddListObjFunc = null;
    public AddListObjDelegate AddListObjFunc { set { m_AddListObjFunc = value; } }

    public void InitDefPool(int InitCount)
    {
        for (int i = 0; i < InitCount; i++)
        {
            T lTempTObj = m_NewObjFunc();

            if (m_RemoveObjFunc != null)
                m_RemoveObjFunc(lTempTObj);

            m_AllObjPool.Enqueue(lTempTObj);
        }
    }

    public T AddObj()
    {
        T lTempTObj;
        if (m_AllObjPool.Count == 0)
            lTempTObj = m_NewObjFunc();
        else
            lTempTObj = m_AllObjPool.Dequeue();

        if (m_AddListObjFunc != null)
            m_AddListObjFunc(lTempTObj, CurAllObjCount);

        m_AllCurObj.Add(lTempTObj);

        return lTempTObj;
    }

    public bool RemoveObj(T removeData)
    {
        bool lbTemp = m_AllCurObj.Remove(removeData);
        if (!lbTemp)
            return lbTemp;

        if (m_RemoveObjFunc != null)
            m_RemoveObjFunc(removeData);

        m_AllObjPool.Enqueue(removeData);

        return lbTemp;
    }

    
}

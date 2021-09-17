using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CGGameSceneData : CSingletonMonoBehaviour<CGGameSceneData>
{

    public enum EAllFXType
    {
        eAddUp  = 0,
        eMax,
    };

    public enum EOtherObj
    {
        ePlayerRogue        = 0,
        eDummyObj           = 1,
        eMax,
    };

    public enum ECarType
    {
        eNormalCar = 0,
        eMax,
    };

    [SerializeField] public GameObject[]    m_AllFX                 = null;
    [SerializeField] public GameObject[]    m_AllOtherObj           = null;
    [SerializeField] public GameObject[]    m_AllCar                = null;
}

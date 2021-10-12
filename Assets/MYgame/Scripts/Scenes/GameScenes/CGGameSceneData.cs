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
        eEnemy              = 2,
        eDidFloor           = 3,
        eMax,
    };

    public enum ECarType
    {
        eNormalCar = 0,
        eMax,
    };

    public enum EArmsType
    {
        eMace           = 0,
        eArmature       = 1,
        eAxe            = 2,
        eHam            = 3,
        ePlunger        = 4,
        eLollipop_02    = 5,
        eLeg            = 6,
        eRolling_Pin    = 7,
        eBaby_Hammer    = 8,
        eMax,
    };

    [SerializeField] public GameObject[]    m_AllFX                 = null;
    [SerializeField] public GameObject[]    m_AllOtherObj           = null;
    [SerializeField] public GameObject[]    m_AllCar                = null;
    [SerializeField] public GameObject[]    m_AllArms               = null;

    public CObjPool<GameObject>[] m_AllArmsPool = new CObjPool<GameObject>[(int)EArmsType.eMax];
    int m_CurNewArmsCount = 0;

    private void Awake()
    {
        //for (int i = 0; i < (int)EArmsType.eMax; i++)
        //{
        //    m_CurNewArmsCount = i;
        //    m_AllArmsPool[i] = new CObjPool<GameObject>();
        //    m_AllArmsPool[i].NewObjFunc = NewArms;
        //    m_AllArmsPool[i].RemoveObjFunc = RemoveArms;
        //    m_AllArmsPool[i].InitDefPool(10);
        //}
    }

    public GameObject GetArms(EArmsType lType)
    {
        int lTempindex = (int)lType;
        m_CurNewArmsCount = lTempindex;
        GameObject lTempObj = m_AllArmsPool[lTempindex].AddObj();
        lTempObj.SetActive(true);

        return lTempObj;
    }

    public void RemoveArmsType(EArmsType lTempType, GameObject lTempObj)
    {
        if (lTempType == EArmsType.eMax)
            return;

        int lTempindex = (int)lTempType;

        m_AllArmsPool[lTempindex].RemoveObj(lTempObj);
    }

    public GameObject NewArms()
    {
        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;
        GameObject lTempObj = Instantiate(m_AllArms[m_CurNewArmsCount], this.transform);
        lTempObj.transform.localPosition = Vector3.zero;
        //Rigidbody lTempRigidbody = m_MyPlayerRogueMemoryShare.m_MyArms.AddComponent<Rigidbody>();
        //lTempRigidbody.AddForceAtPosition(lTempV3 * Random.Range(80.0f, 130.0f), lTemppointV3);
        return lTempObj;
    }

    public void RemoveArms(GameObject Arms)
    {
        Arms.transform.parent = this.transform;
        Arms.transform.localPosition = Vector3.zero;
        Arms.SetActive(false);
    }
}

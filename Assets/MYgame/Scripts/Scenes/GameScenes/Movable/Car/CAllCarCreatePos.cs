using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAllCarCreatePos : CGameObjBas
{
    const int CstInitQueueCount = 30;

    [SerializeField] protected Transform m_CarPoodTransform = null;


    public override EObjType ObjType() { return EObjType.eAllCarCreatePos; }

    protected List<CCarCreatePos> m_AllCarCreatePos = new List<CCarCreatePos>();

    protected CObjPool<CCarBase> m_AllCarBasePool = new CObjPool<CCarBase>();
    public CObjPool<CCarBase> AllCarBasePool { get { return m_AllCarBasePool; } }

    public void AddAllCarCreatePos(CCarCreatePos addobj) { m_AllCarCreatePos.Add(addobj); }
    public void RemoveAllCarCreatePos(CCarCreatePos removeobj) { m_AllCarCreatePos.Remove(removeobj); }

    protected override void Awake()
    {
        base.Awake();

        AllCarBasePool.NewObjFunc = NewCarBase;
        AllCarBasePool.RemoveObjFunc = (CCarBase RemoveRogue) => { RemoveRogue.MyRemove(); };
        AllCarBasePool.InitDefPool(CstInitQueueCount);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public CCarBase NewCarBase()
    {
        CGGameSceneData lTempCGGameSceneData = CGGameSceneData.SharedInstance;

        GameObject lTempObj = Instantiate(lTempCGGameSceneData.m_AllCar[(int)CGGameSceneData.ECarType.eNormalCar], m_CarPoodTransform.position, m_CarPoodTransform.rotation, m_CarPoodTransform.transform);
        CCarBase lTempCarBase = lTempObj.GetComponent<CCarBase>();
        return lTempCarBase;
    }

    public CCarBase GetCarBase()
    {
        return AllCarBasePool.AddObj();
    }
}

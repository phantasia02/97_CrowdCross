using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCarCreatePos : CGameObjBas
{
    protected CAllCarCreatePos m_MyAllCarCreatePos = null;

    protected List<CCarBase> m_MyAllCar = new List<CCarBase>();

    [SerializeField] protected Collider m_EndCollider = null;

    public override EObjType ObjType() { return EObjType.eCarCreatePos; }

    [SerializeField] protected float m_MaxCreateCarDis = 50.0f;
    [SerializeField] protected float m_MinCreateCarDis = 5.0f;
    protected float m_NextAddCarDisSqr = float.MaxValue;

    protected override void Awake()
    {
        base.Awake();
        
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

        if (m_MyAllCar.Count > 0)
        {
            CCarBase lTempCarBase = m_MyAllCar[m_MyAllCar.Count - 1];

            Vector3 lTempV3 = lTempCarBase.transform.position - this.transform.position;
            lTempV3.y = 0.0f;

            if (Vector3.SqrMagnitude(lTempV3) >= m_NextAddCarDisSqr)
            {
                AddCarChild(this.transform.position);
                SetNextAddCarDisSqr();
            }
        }
    }

    public void SetNextAddCarDisSqr()
    {
        if (m_MyAllCar.Count > 0)
        {
            CCarBase lTempCarBase = m_MyAllCar[m_MyAllCar.Count - 1];
            float lTempDisSqr = lTempCarBase.CarLong + Random.Range(m_MinCreateCarDis, m_MaxCreateCarDis);
            m_NextAddCarDisSqr = lTempDisSqr * lTempDisSqr;
        }
        else
            m_NextAddCarDisSqr = float.MaxValue;
    }

    public void AddCar(CCarBase car)
    {
        int Addindex = m_MyAllCar.Count;
        m_MyAllCar.Add(car);
        car.transform.parent = this.transform;
        car.transform.localPosition = Vector3.zero;
        car.transform.rotation = this.transform.rotation;
        car.AddToList(Addindex);
    }

    public void RemoveCar(CCarBase car)
    {
        m_MyAllCarCreatePos.AllCarBasePool.RemoveObj(car);
    }

    public CCarBase AddCarChild(Vector3 lpos)
    {
        CCarBase lTempCarBase = m_MyAllCarCreatePos.AllCarBasePool.AddObj();
        AddCar(lTempCarBase);
        lTempCarBase.transform.position = lpos;
        return lTempCarBase;
    }

    private void OnEnable()
    {
        m_MyAllCarCreatePos = this.gameObject.GetComponentInParent<CAllCarCreatePos>();
        m_MyAllCarCreatePos.AddAllCarCreatePos(this);

      //  Vector3 lTempv3pos2 = Vector3.zero;
        Vector3 lTempv3pos = Vector3.Lerp(transform.position, m_EndCollider.transform.position, 0.6f);
        lTempv3pos.y = 0.0f;

        Vector3 lTempv3Dir = transform.position - m_EndCollider.transform.position;
        lTempv3Dir.y = 0.0f;
        lTempv3Dir.Normalize();

        CCarBase lTempCarBase = AddCarChild(lTempv3pos);

        for (int i = 0; i < 10; i++)
        {
            lTempv3pos = Vector3.MoveTowards(lTempCarBase.transform.position, this.transform.position, lTempCarBase.CarLong + Random.Range(m_MinCreateCarDis, m_MaxCreateCarDis));
            lTempv3pos.y = 0.0f;

            if (Vector3.SqrMagnitude(lTempv3pos - this.transform.position) < 1.0f)
                break;

            lTempCarBase = AddCarChild(lTempv3pos);
        }

        SetNextAddCarDisSqr();
    }

    private void OnDisable()
    {
        if (isApplicationQuitting)
            return;

        m_MyAllCarCreatePos.RemoveAllCarCreatePos(this);

        if (m_MyAllCarCreatePos.enabled)
        {
            for (int i = m_MyAllCar.Count - 1; i >= 0; i--)
                m_MyAllCarCreatePos.AllCarBasePool.RemoveObj(m_MyAllCar[i]);
        }

        m_MyAllCar.Clear();
    }
}
